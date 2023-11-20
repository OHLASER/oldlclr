using System;
using System.Runtime.InteropServices;

namespace oldlclr
{
    /// <summary>
    /// External api server module
    /// </summary>
    public class Receiver
        : ICloneable, IDisposable
    {
        /// <summary>
        /// delegate to take IntPtr as parameter and return uint
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public delegate uint UintFuncIntPtr(IntPtr arg);

        /// <summary>
        /// delegate to take IntPtr, IntPtr, uint and return int
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <returns></returns>
        public delegate int IntFuncIntPtrIntPtrUintIntPtr(IntPtr arg1, IntPtr arg2, uint arg3, IntPtr arg4);

        /// <summary>
        /// delegate to take an IntPtr and return int
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public delegate IntPtr IntPtrFuncIntPtr(IntPtr arg);

        /// <summary>
        /// Virtual function table
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct HandlerIVtbl
        {
            public UintFuncIntPtr Retain;
            public UintFuncIntPtr Release;
            public IntFuncIntPtrIntPtrUintIntPtr LoadData;
            public IntPtrFuncIntPtr GetStatus;
        }





        /// <summary>
        /// Intanciate receiver in process heap
        /// </summary>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_create")]
        private static extern IntPtr CreateI();

        /// <summary>
        /// Increment reference count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_retain")]
        private static extern uint Retain(IntPtr objPtr);

        /// <summary>
        /// Decrement reference count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_release")]
        private static extern uint Release(IntPtr objPtr);


        /// <summary>
        /// set receiver handler
        /// </summary>
        /// <param name="objPtr"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_set_handler")]
        private static extern int SetHandler(IntPtr objPtr, IntPtr handler);

        /// <summary>
        /// get receiver handler
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_get_handler")]
        private static extern IntPtr GetHandler(IntPtr objPtr);


        /// <summary>
        /// start to listening message from client
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_start")]
        private static extern int Start(IntPtr objPtr);


        /// <summary>
        /// stop to listening message from client
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_stop_communication")]
        private static extern int Stop(IntPtr objPtr);

        /// <summary>
        /// Native Object pointer
        /// </summary>
        public IntPtr ObjectPtr { get; private set; }

        /// <summary>
        /// data link service
        /// </summary>
        public IService Service
        {
            get => GetService();
            set => SetService(value);
        }



        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// constructor 
        /// </summary>
        public Receiver()
        {
            AttachRef(CreateI());
        }

        ~Receiver()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }
        public void Attach(IntPtr objPtr)
        {
            if (IntPtr.Zero != objPtr)
            {
                Retain(objPtr);
            }
            AttachRef(objPtr);
        }

        /// <summary>
        /// bind this and objPtr(Native object pointer)
        /// </summary>
        /// <param name="objPtr"></param>
        public void AttachRef(IntPtr objPtr)
        {
            if (IntPtr.Zero != ObjectPtr)
            {
                Release(ObjectPtr);
            }
            ObjectPtr = objPtr;

        }

        public object Clone()
        {
            Receiver result = (Receiver)base.MemberwiseClone();
            Retain(result.ObjectPtr);

            return result;

        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                Attach(IntPtr.Zero);

                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// start listening to message from client
        /// </summary>
        public void Start()
        {
            Start(ObjectPtr);
        }

        /// <summary>
        /// stop listening to message from client
        /// </summary>
        public void Stop()
        {
            Stop(ObjectPtr);
        }


        /// <summary>
        /// get service
        /// </summary>
        /// <returns></returns>
        public IService GetService()
        {
            IntPtr objPtr = GetHandler(ObjectPtr);

            IService result = null;
            if (objPtr != IntPtr.Zero)
            {
                ReceiverHandler recieverHdlr = ReceiverHandler.DecodeRecieverHandler(objPtr);

                result = recieverHdlr?.DataLinkService;
            }
            return result;
        }


        /// <summary>
        /// set service
        /// </summary>
        /// <param name="dataLinkService"></param>
        public void SetService(IService dataLinkService)
        {
            IntPtr objPtr = GetHandler(ObjectPtr);

            if (objPtr != IntPtr.Zero)
            {
                ReceiverHandler recieverHdlr = ReceiverHandler.DecodeRecieverHandler(objPtr);

                if (recieverHdlr != null)
                {
                    recieverHdlr.DataLinkService = dataLinkService;
                }
                ReceiverHandler.ReleaseRecieverHandler(objPtr);
            }
            else
            {
                ReceiverHandler recieverHdlr = new()
                {
                    DataLinkService = dataLinkService
                };

                SetHandler(ObjectPtr, recieverHdlr.UnmanagedPtr);

                recieverHdlr.Release();

            }
        }



    }
}
