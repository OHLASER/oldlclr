using System;
using System.Runtime.InteropServices;

namespace oldlclr
{
    public class Str : ICloneable, IDisposable
    {
        /// <summary>
        /// Intanciate reciever in process heap
        /// </summary>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_str_create_00")]
        private static extern IntPtr CreateI(byte[] byteArray, uint length);

        /// <summary>
        /// Intanciate reciever in process heap
        /// </summary>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_str_create_00")]
        private static extern IntPtr CreateI(IntPtr dataPtr, uint length);


        /// <summary>
        /// Intanciate reciever in process heap
        /// </summary>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_str_create_01")]
        private static extern IntPtr CreateI(byte[] byteArray);

        /// <summary>
        /// Increment reference count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_str_retain")]
        private static extern uint Retain(IntPtr objPtr);

        /// <summary>
        /// Decrement reference count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_str_release")]
        private static extern uint Release(IntPtr objPtr);


        /// <summary>
        /// Get length
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_str_get_length")]
        private static extern uint GetLength(IntPtr objPtr);


        /// <summary>
        /// CopyContents
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_str_copy_contents")]
        private static extern int CopyContents(IntPtr objPtr,
            byte[] buffer, uint size);

        /// <summary>
        /// Native Object pointer
        /// </summary>
        public IntPtr ObjectPtr { get; private set; }

        /// <summary>
        /// Length of data
        /// </summary>
        public int Length
        {
            get
            {
                int result;
                result = (int)GetLength(ObjectPtr);
                return result;
            }
        }

        /// <summary>
        /// DataContents
        /// </summary>
        public byte[] Contents
        {
            get
            {

                byte[] result;
                result = new byte[Length];
                CopyContents(ObjectPtr, result, (uint)result.Length);

                return result;
            }
        }


        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// constructor 
        /// </summary>
        public Str(byte[] byteArray) => AttachRef(CreateI(byteArray, (uint)byteArray.Length));

        /// <summary>
        /// construct zero terminate utf8 string
        /// </summary>
        /// <param name="str"></param>
        public Str(string str)
        {
            byte[] strBytes;
            strBytes = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] strBytesZero;
            strBytesZero = new byte[strBytes.Length + 1];
            Array.Copy(strBytes, strBytesZero, strBytes.Length);
            strBytesZero[^1] = 0;

            AttachRef(CreateI(strBytesZero, (uint)strBytesZero.Length));

        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="objPtr"></param>
        internal Str(IntPtr objPtr, uint length) => AttachRef(CreateI(objPtr, length));

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="objPtr"></param>
        internal Str(IntPtr objPtr) => AttachRef(objPtr);



        ~Str()
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

        object ICloneable.Clone()
        {

            Str result;
            result = (Str)base.MemberwiseClone();

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
        /// get contents as string
        /// </summary>
        /// <returns></returns>
        public string GetContentsAsString()
        {
            byte[] strBytes;

            strBytes = Contents;
            string result;
            result = null;
            if (strBytes.Length > 0)
            {
                int length;
                if (strBytes[^1] == 0)
                {
                    length = strBytes.Length - 1;
                }
                else
                {
                    length = strBytes.Length;
                }
                result = System.Text.Encoding.UTF8.GetString(strBytes, 0, length);
            }

            return result;
        }


    }
}
