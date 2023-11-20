using System;
using System.Runtime.InteropServices;
using System.Text;

namespace oldlclr
{
    public class ReceiverHandler
    {
        /// <summary>
        /// Unmanaged object for ReceiverHandler interface
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct UnmanagedObjectLayout
        {
            /// <summary>
            /// virtual function pointer
            /// </summary>
            internal IntPtr Vtbl;
            /// <summary>
            /// object pointer
            /// </summary>
            internal IntPtr ObjectPtr;
            /// <summary>
            /// magic number
            /// </summary>
            internal int Magic;
        }

        /// <summary>
        /// Unmanaged object for ReceiverHandler interface
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct UnmanagedNoMagicObjectLayout
        {
            /// <summary>
            /// virtual function pointer
            /// </summary>
            internal IntPtr Vtbl;
        }

        /// <summary>
        /// magic number for unmanaged object
        /// </summary>
        private static int MagicCode
        {
            get
            {
                string magicWord = "oh-laser";
                byte[] byteArray = Encoding.UTF8.GetBytes(magicWord);

                int result;
                result = (byteArray[0] << (8 * 3))
                    | (byteArray[1] << (8 * 2))
                    | (byteArray[2] << (8 * 1))
                    | byteArray[3];

                return result;
            }
        }


        /// <summary>
        /// reciever handler
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        internal static ReceiverHandler DecodeRecieverHandler(IntPtr objPtr)
        {
            var objLayout = Marshal.PtrToStructure<UnmanagedObjectLayout>(objPtr);
            return (objLayout.Magic == MagicCode) switch
            {
                true => GCHandle.FromIntPtr(objLayout.ObjectPtr).Target as ReceiverHandler,
                _ => null
            };
        }


        /// <summary>
        /// release unmanaged object
        /// </summary>
        /// <param name="objPtr"></param>
        internal static void ReleaseRecieverHandler(IntPtr objPtr)
        {
            var objLayout = Marshal.PtrToStructure<UnmanagedNoMagicObjectLayout>(objPtr);

            var vtbl = Marshal.PtrToStructure<Receiver.HandlerIVtbl>(objLayout.Vtbl);

            vtbl.Release(objPtr);


        }


        /// <summary>
        /// Virtual function table
        /// </summary>
        private Receiver.HandlerIVtbl Vtbl;
        /// <summary>
        /// アンマネージドオブジェクトに渡す構造体
        /// </summary>
        internal IntPtr UnmanagedPtr;

        /// <summary>
        /// data link service
        /// </summary>
        internal IService DataLinkService;

        /// <summary>
        /// Constructor
        /// </summary>
        internal ReceiverHandler()
        {
            Vtbl = new()
            {
                Retain = Retain,
                Release = Release,
                LoadData = LoadData,
                GetStatus = GetStatus
            };

            IntPtr unmanagedPtr;
            unmanagedPtr = Marshal.AllocHGlobal(Marshal.SizeOf<UnmanagedObjectLayout>());

            if (unmanagedPtr != IntPtr.Zero)
            {
                IntPtr vtblPtr;
                vtblPtr = Marshal.AllocHGlobal(Marshal.SizeOf<Receiver.HandlerIVtbl>());
                if (vtblPtr != IntPtr.Zero)
                {
                    Marshal.StructureToPtr(Vtbl, vtblPtr, false);
                    UnmanagedObjectLayout UnmanagedObject = new()
                    {
                        Vtbl = vtblPtr,
                        ObjectPtr = GCHandle.ToIntPtr(GCHandle.Alloc(this)),
                        Magic = MagicCode
                    };
                    Marshal.StructureToPtr(UnmanagedObject, unmanagedPtr, false);
                }
                else
                {
                    Marshal.FreeHGlobal(unmanagedPtr);
                    unmanagedPtr = IntPtr.Zero;
                }
            }

            this.UnmanagedPtr = unmanagedPtr;
            Retain(this.UnmanagedPtr);
        }


        /// <summary>
        /// increment reference
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public uint Retain(IntPtr obj)
        {
            uint result;
            lock (this)
            {
                result = ++RefCount;
            }

            return result;
        }

        /// <summary>
        /// decrement reference count
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public uint Release(IntPtr obj)
        {
            uint result;
            lock (this)
            {
                result = --RefCount;
            }
            if (result == 0)
            {

                UnmanagedObjectLayout rawObject = Marshal.PtrToStructure<UnmanagedObjectLayout>(obj);

                if (UnmanagedPtr != IntPtr.Zero)
                {
                    UnmanagedObjectLayout UnmanagedObject;

                    UnmanagedObject = (UnmanagedObjectLayout)Marshal.PtrToStructure(UnmanagedPtr, typeof(UnmanagedObjectLayout));


                    Marshal.DestroyStructure(UnmanagedObject.Vtbl, typeof(Receiver.HandlerIVtbl));
                    Marshal.FreeHGlobal(UnmanagedObject.Vtbl);
                    GCHandle thisHandle;
                    thisHandle = GCHandle.FromIntPtr(UnmanagedObject.ObjectPtr);
                    thisHandle.Free();
                    Marshal.DestroyStructure(UnmanagedPtr, typeof(UnmanagedObjectLayout));
                    UnmanagedPtr = IntPtr.Zero;

                }

            }

            return result;
        }
        /// <summary>
        /// decrement reference count
        /// </summary>
        /// <returns></returns>
        internal uint Release()
        {
            return Release(UnmanagedPtr);
        }

        public int LoadData(IntPtr objPtr, IntPtr data, uint length, IntPtr dataNamePtr)
        {
            IService dataLinkService = DataLinkService;

            return  dataLinkService switch
            {
                not null => (int)LoadData(data, length, dataNamePtr, dataLinkService),
                _ => (int)ErrorCode.RECEIVER_IS_NOT_ATTACHED
            };
        }

        private static ErrorCode LoadData(IntPtr data, uint length, IntPtr dataNamePtr, IService dataLinkService)
        {
            using Codec codec = new();
            using Str strObj = new(data, length);
            codec.Decode(strObj);

            byte[] processingData = codec.Data;

            return processingData switch
            {
                not null and { Length: > 0 } => dataLinkService.LoadProcessingData(codec.DataType, processingData, dataNamePtr),
                _ => ErrorCode.INVALID_DATA_FORMAT
            };
        }

        public IntPtr GetStatus(IntPtr obj)
        {
            IntPtr result = IntPtr.Zero;
            IService dataLinkService = DataLinkService;

            if (dataLinkService != null)
            {
                DateTime? finishedLoading = dataLinkService.TimeOfFinishedLoading;
                DateTime? startedLoading = dataLinkService.TimeOfStartedLoading;

                DateTime? finishedProcessing = dataLinkService.TimeOfFinishedProcessing;
                DateTime? startedProcessing = dataLinkService.TimeOfStartedProcessing;
                string dataName = dataLinkService.DataName;


                Status status = new();
                status.SetStartedTimeOfLoading(startedLoading);
                status.SetFinishedTimeOfLoading(finishedLoading);
                status.SetStartedTimeOfProcessing(startedProcessing);
                status.SetFinishedTimeOfProcessing(finishedProcessing);
                status.SetDataName(dataName);
                status.ProcessedCount = dataLinkService.ProcessingCount;
                status.Code = dataLinkService.StatusCode;
                result = status.ObjectPtr;
            }

            return result;
        }

        private uint RefCount;

    }
}
