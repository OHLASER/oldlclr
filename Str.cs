using System;
using System.Runtime.InteropServices;

namespace oldlclr
{
    public partial class Str : ICloneable, IDisposable
    {
        private static partial class NativeMethods
        {
            /// <summary>
            /// Intanciate reciever in process heap
            /// </summary>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_str_create_00")]
            public static partial IntPtr CreateI([In]byte[] byteArray, uint length);

            /// <summary>
            /// Intanciate reciever in process heap
            /// </summary>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_str_create_00")]
            public static partial IntPtr CreateI(IntPtr dataPtr, uint length);

            /// <summary>
            /// Intanciate reciever in process heap
            /// </summary>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_str_create_01")]
            public static partial IntPtr CreateI([In]byte[] byteArray);

            /// <summary>
            /// Increment reference count
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_str_retain")]
            public static partial uint Retain(IntPtr objPtr);

            /// <summary>
            /// Decrement reference count
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_str_release")]
            public static partial uint Release(IntPtr objPtr);

            /// <summary>
            /// Get length
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_str_get_length")]
            public static partial uint GetLength(IntPtr objPtr);

            /// <summary>
            /// CopyContents
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_str_copy_contents")]
            public static partial int CopyContents(IntPtr objPtr,
                [In, Out]byte[] buffer, uint size);
        }

        /// <summary>
        /// Native Object pointer
        /// </summary>
        public IntPtr ObjectPtr { get; private set; }

        /// <summary>
        /// Length of data
        /// </summary>
        public int Length => (int)NativeMethods.GetLength(ObjectPtr);

        /// <summary>
        /// DataContents
        /// </summary>
        public byte[] Contents
        {
            get
            {
                byte[] result = new byte[Length];
                _ = NativeMethods.CopyContents(ObjectPtr, result, (uint)result.Length);

                return result;
            }
        }

        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// constructor
        /// </summary>
        public Str(byte[] byteArray) => AttachRef(NativeMethods.CreateI(byteArray, (uint)byteArray.Length));

        /// <summary>
        /// construct zero terminate utf8 string
        /// </summary>
        /// <param name="str"></param>
        public Str(string str)
        {
            byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] strBytesZero = new byte[strBytes.Length + 1];
            Array.Copy(strBytes, strBytesZero, strBytes.Length);
            strBytesZero[^1] = 0;

            AttachRef(NativeMethods.CreateI(strBytesZero, (uint)strBytesZero.Length));
        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="objPtr"></param>
        internal Str(IntPtr objPtr, uint length) => AttachRef(NativeMethods.CreateI(objPtr, length));

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

        object ICloneable.Clone()
        {
            Str result = (Str)base.MemberwiseClone();

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
        /// get contents as string
        /// </summary>
        /// <returns></returns>
        public string GetContentsAsString()
        {
            byte[] strBytes = Contents;
            string result = null;
            if (strBytes.Length > 0)
            {
                int length = strBytes[^1] switch
                {
                    0 => strBytes.Length - 1,
                    _ => strBytes.Length,
                };
                result = System.Text.Encoding.UTF8.GetString(strBytes, 0, length);
            }

            return result;
        }
    }
}
