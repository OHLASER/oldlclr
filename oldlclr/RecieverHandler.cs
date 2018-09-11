using System;
using System.Runtime.InteropServices;
using System.Text;

namespace oldlclr
{
    public class RecieverHandler
    {
        /// <summary>
        /// Unmanaged object for RecieverHandler interface
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
        /// Unmanaged object for RecieverHandler interface
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
        static int MagicCode
        {
            get
            {
                string magicWord;
                magicWord = "oh-laser";

                byte[] byteArray;
                byteArray = Encoding.UTF8.GetBytes(magicWord);

                int result;
                result = byteArray[0] << (8 * 3)
                    | byteArray[1] << (8 * 2) 
                    | byteArray[2] << (8 * 1) 
                    | byteArray[3];

                return result;
            }
        }


        /// <summary>
        /// reciever handler
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        internal static RecieverHandler DecodeRecieverHandler(IntPtr objPtr)
        {
            RecieverHandler result;
            result = null;

            UnmanagedObjectLayout objLayout;

            objLayout = Marshal.PtrToStructure<UnmanagedObjectLayout>(objPtr);

            if (objLayout.Magic == MagicCode)
            {
                GCHandle hdl;
                hdl = GCHandle.FromIntPtr(objLayout.ObjectPtr);

                result = (RecieverHandler)hdl.Target;

            }


            return result;

        }


        /// <summary>
        /// release unmanaged object
        /// </summary>
        /// <param name="objPtr"></param>
        internal static void ReleaseRecieverHandler(IntPtr objPtr)
        {

            UnmanagedNoMagicObjectLayout objLayout;
            objLayout = Marshal.PtrToStructure<UnmanagedNoMagicObjectLayout>(objPtr);

            Reciever.HandlerIVtbl vtbl;

            vtbl = Marshal.PtrToStructure<Reciever.HandlerIVtbl>(objLayout.Vtbl);

            vtbl.Release(objPtr);
           

        }


        /// <summary>
        /// Virtual function table
        /// </summary>
        Reciever.HandlerIVtbl Vtbl;
        /// <summary>
        /// アンマネージドオブジェクトに渡す構造体
        /// </summary>
        internal IntPtr UnmanagedPtr;

        /// <summary>
        /// data link service
        /// </summary>
        internal Service DataLinkService;

        /// <summary>
        /// Constructor
        /// </summary>
        internal RecieverHandler()
        {

            Vtbl.Retain = Retain;
            Vtbl.Release = Release;
            Vtbl.LoadData = LoadData;
            Vtbl.GetStatus = GetStatus;


            IntPtr unmanagedPtr;
            unmanagedPtr = Marshal.AllocHGlobal(Marshal.SizeOf<UnmanagedObjectLayout>());

            if (unmanagedPtr != IntPtr.Zero)
            {
                UnmanagedObjectLayout UnmanagedObject;
                IntPtr vtblPtr;
                vtblPtr = Marshal.AllocHGlobal(Marshal.SizeOf<Reciever.HandlerIVtbl>());
                if (vtblPtr != IntPtr.Zero)
                {
                    Marshal.StructureToPtr(Vtbl, vtblPtr, false);
                    UnmanagedObject.Vtbl = vtblPtr;
                    UnmanagedObject.ObjectPtr = GCHandle.ToIntPtr(GCHandle.Alloc(this));
                    UnmanagedObject.Magic = MagicCode;
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
            lock(this)
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
                UnmanagedObjectLayout rawObject;

                rawObject = Marshal.PtrToStructure<UnmanagedObjectLayout>(obj);

                if (UnmanagedPtr != IntPtr.Zero)
                {
                    UnmanagedObjectLayout UnmanagedObject;

                    UnmanagedObject = (UnmanagedObjectLayout)Marshal.PtrToStructure(UnmanagedPtr, typeof(UnmanagedObjectLayout));


                    Marshal.DestroyStructure(UnmanagedObject.Vtbl, typeof(Reciever.HandlerIVtbl));
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
            int result;
            result = 0;


            Service dataLinkService;
            dataLinkService = DataLinkService;
            if (dataLinkService != null)
            {
                Codec codec;
                codec = new Codec();

                Str strObj;
                strObj = new Str(data, length);

                codec.Decode(strObj);

                byte[] processingData;
                processingData = codec.Data;

                string dataName;
                dataName = null;
                if (dataNamePtr != IntPtr.Zero)
                {
                    int dataLength;
                    dataLength = 0;
                    while (true)
                    {
                        byte tempValue;
                        tempValue = Marshal.ReadByte(dataNamePtr, dataLength);
                            
                        
                        if (tempValue == 0)
                        {
                            break;
                        }
                        dataLength++;
                    }
                    byte[] buffer;
                    buffer = new byte[dataLength];
                    Marshal.Copy(dataNamePtr, buffer, 0, dataLength);
                    dataName = System.Text.Encoding.UTF8.GetString(buffer);
                }


                if (processingData != null && processingData.Length > 0)
                {
                    ErrorCode state;
                    state = dataLinkService.LoadProcessingData(codec.DataType, processingData, dataName);
                    result = (int)state;
                }
                else
                {
                    result = (int)ErrorCode.INVALID_DATA_FORMAT;
                }



                strObj.Dispose();

                codec.Dispose();

            }
            else
            {
                result = (int)ErrorCode.RECIEVER_IS_NOT_ATTACHED;
            }



            return result;

        }

        public IntPtr GetStatus(IntPtr obj)
        {
            IntPtr result;

            result = IntPtr.Zero;

            Service dataLinkService;
            dataLinkService = DataLinkService;
            if (dataLinkService != null)
            {
                DateTime? finishedLoading;
                DateTime? startedLoading;
                DateTime? finishedProcessing;
                DateTime? startedProcessing;
                string dataName;

                finishedLoading = dataLinkService.TimeOfFinishedLoading;
                startedLoading = dataLinkService.TimeOfStartedLoading;

                finishedProcessing = dataLinkService.TimeOfFinishedProcessing;
                startedProcessing = dataLinkService.TimeOfStartedProcessing;
                dataName = dataLinkService.DataName;

                
                Status status;
                status = new Status();
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

        uint RefCount; 

    }
}
