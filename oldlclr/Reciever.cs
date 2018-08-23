using System;
using System.Runtime.InteropServices;

namespace oldlclr
{
    /// <summary>
    /// External api server module
    /// </summary>
    public class Reciever
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
        /// Intanciate reciever in process heap
        /// </summary>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_reciever_create")]
        static extern IntPtr CreateI();

        /// <summary>
        /// Increment reference count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_reciever_retain")]
        static extern uint Retain(IntPtr objPtr);

        /// <summary>
        /// Decrement reference count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_reciever_release")]
        static extern uint Release(IntPtr objPtr);

       
        /// <summary>
        /// set reciever handler
        /// </summary>
        /// <param name="objPtr"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_reciever_set_handler")]
        static extern int SetHandler(IntPtr objPtr, IntPtr handler);

        /// <summary>
        /// get reciever handler
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_reciever_get_handler")]
        static extern IntPtr GetHandler(IntPtr objPtr);


        /// <summary>
        /// start to listening message from client
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_reciever_start")]
        static extern int Start(IntPtr objPtr);


        /// <summary>
        /// stop to listening message from client
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_reciever_stop_communication")]
        static extern int Stop(IntPtr objPtr);



        /// <summary>
        /// Native object pointer
        /// </summary>
        private IntPtr ObjectPtrValue;

        /// <summary>
        /// Native Object pointer
        /// </summary>
        public IntPtr ObjectPtr
        {
            get
            {
                return ObjectPtrValue;
            }
        }

        /// <summary>
        /// data link service
        /// </summary>
        public Service Service
        {
            get
            {
                return GetService();
            }
            set
            {
                SetService(value);
            }
        }



        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// constructor 
        /// </summary>
        public Reciever()
        {
            AttachRef(CreateI());
        }
         ~Reciever()
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
            ObjectPtrValue = objPtr;

        }

        public object Clone()
        {
            Reciever result;
            result = (Reciever)base.MemberwiseClone();
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
        public Service GetService()
        {
            IntPtr objPtr;
            objPtr = GetHandler(ObjectPtr);

            Service result;
            result = null;
            if (objPtr != IntPtr.Zero)
            {
                RecieverHandler recieverHdlr;
                recieverHdlr = RecieverHandler.DecodeRecieverHandler(objPtr);

                if (recieverHdlr != null)
                {
                    result = recieverHdlr.DataLinkService;
                }
            }
            return result;
        }


        /// <summary>
        /// set service
        /// </summary>
        /// <param name="dataLinkService"></param>
        public void SetService(Service dataLinkService)
        {
            IntPtr objPtr;
            objPtr = GetHandler(ObjectPtr);

            if (objPtr != IntPtr.Zero)
            {
                RecieverHandler recieverHdlr;
                recieverHdlr = RecieverHandler.DecodeRecieverHandler(objPtr);

                if (recieverHdlr != null)
                {
                    recieverHdlr.DataLinkService = dataLinkService;
                }
                RecieverHandler.ReleaseRecieverHandler(objPtr);
            }
            else
            {
                RecieverHandler recieverHdlr;
                recieverHdlr = new RecieverHandler();

                recieverHdlr.DataLinkService = dataLinkService;

                SetHandler(ObjectPtr, recieverHdlr.UnmanagedPtr);

                recieverHdlr.Release();

            }
        }



    }
}
