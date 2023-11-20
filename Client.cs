using System;
using System.Runtime.InteropServices;

namespace oldlclr
{
    /// <summary>
    /// HARUKA data link client 
    /// </summary>
    public class Client
        : ICloneable, IDisposable
    {

        /// <summary>
        /// Intanciate client in process heap
        /// </summary>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_client_create")]
        private static extern IntPtr CreateI();

        /// <summary>
        /// Increment reference count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_client_retain")]
        private static extern uint Retain(IntPtr objPtr);

        /// <summary>
        /// Decrement reference count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_client_release")]
        private static extern uint Release(IntPtr objPtr);

        /// <summary>
        /// get data link reciever status
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_client_get_receiver_status")]
        private static extern IntPtr GetStatus(IntPtr objPtr);


        /// <summary>
        /// get data link reciever status
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_client_load_data")]
        private static extern int LoadData(IntPtr objPtr, byte[] data, uint dataLength, byte[] cStrName);


        /// <summary>
        /// connect data link reciever
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_client_connect")]
        private static extern int Connect(IntPtr objPtr);

        /// <summary>
        /// disconnect data link reciever
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_client_disconnect")]
        private static extern int Disconnect(IntPtr objPtr);


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
        public Client() => AttachRef(CreateI());
        ~Client()
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
            Client result;
            result = (Client)base.MemberwiseClone();
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
        /// get data link server status
        /// </summary>
        /// <returns></returns>
        public Status GetStatus()
        {
            Status result = null;
            IntPtr statusPtr = GetStatus(ObjectPtr);
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

            int state = LoadData(ObjectPtr, data, (uint)data.Length, dataNameByteArray1);

            return state == 0;
        }


        /// <summary>
        /// connect to data link reciever
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            int state = Connect(ObjectPtr);
            return state == 0;
        }

        /// <summary>
        /// disconnect from data link reciever.
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            int state = Disconnect(ObjectPtr);
            return state == 0;
        }

    }
}
