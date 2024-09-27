using System;
using System.Runtime.InteropServices;

namespace oldlclr
{
    /// <summary>
    /// HARUKA data link client
    /// </summary>
    public partial class Client
        : ICloneable, IDisposable
    {
        private static partial class NativeMethods
        {
            /// <summary>
            /// Intanciate client in process heap
            /// </summary>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_client_create")]
            public static partial IntPtr CreateI();

            /// <summary>
            /// Increment reference count
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_client_retain")]
            public static partial uint Retain(IntPtr objPtr);

            /// <summary>
            /// Decrement reference count
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_client_release")]
            public static partial uint Release(IntPtr objPtr);

            /// <summary>
            /// get data link reciever status
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_client_get_receiver_status")]
            public static partial IntPtr GetStatus(IntPtr objPtr);

            /// <summary>
            /// get data link reciever status
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_client_load_data")]
            public static partial int LoadData(IntPtr objPtr, [In]byte[] data, uint dataLength, [In]byte[] cStrName);

            /// <summary>
            /// connect data link reciever
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_client_connect")]
            public static partial int Connect(IntPtr objPtr);

            /// <summary>
            /// disconnect data link reciever
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_client_disconnect")]
            public static partial int Disconnect(IntPtr objPtr);
        }

        /// <summary>
        /// generate name
        /// </summary>
        /// <returns></returns>
        private static string GenerateDataName() => $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.pdf";

        /// <summary>
        /// Native Object pointer
        /// </summary>
        public IntPtr ObjectPtr { get; private set; }

        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// constructor
        /// </summary>
        public Client() => AttachRef(NativeMethods.CreateI());
        ~Client()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }
        public void Attach(IntPtr objPtr)
        {
            if (IntPtr.Zero != objPtr)
            {
                _ = NativeMethods.Retain(objPtr);
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
                _ = NativeMethods.Release(ObjectPtr);
            }
            ObjectPtr = objPtr;
        }

        public object Clone()
        {
            Client result = (Client)base.MemberwiseClone();
            _ = NativeMethods.Retain(result.ObjectPtr);

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
        /// get data link server status
        /// </summary>
        /// <returns></returns>
        public Status GetStatus()
        {
            Status result = null;
            IntPtr statusPtr = NativeMethods.GetStatus(ObjectPtr);
            if (statusPtr != IntPtr.Zero)
            {
                result = new Status();
                result.AttachRef(statusPtr);
            }

            return result;
        }

        /// <summary>
        /// send data to data server(Laser processing application)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataName"></param>
        /// <returns></returns>
        public bool LoadData(byte[] data, string dataName)
        {
            dataName ??= GenerateDataName();

            byte[] dataNameByteArray = System.Text.Encoding.UTF8.GetBytes(dataName);

            byte[] dataNameByteArray1 = new byte[dataNameByteArray.Length + 1];
            Array.Copy(dataNameByteArray, dataNameByteArray1, dataNameByteArray.Length);
            dataNameByteArray1[dataNameByteArray.Length] = 0;

            int state = NativeMethods.LoadData(ObjectPtr, data, (uint)data.Length, dataNameByteArray1);

            return state == 0;
        }

        /// <summary>
        /// connect to data link reciever
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            int state = NativeMethods.Connect(ObjectPtr);
            return state == 0;
        }

        /// <summary>
        /// disconnect from data link reciever.
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            int state = NativeMethods.Disconnect(ObjectPtr);
            return state == 0;
        }
    }
}
