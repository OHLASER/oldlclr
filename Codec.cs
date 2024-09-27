using System;
using System.Runtime.InteropServices;

namespace oldlclr
{
    public partial class Codec : IDisposable
    {
        private static partial class NativeMethods
        {
            /// <summary>
            /// Intanciate reciever in process heap
            /// </summary>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_codec_create")]
            public static partial IntPtr CreateI();

            /// <summary>
            /// Increment reference count
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_codec_retain")]
            public static partial uint Retain(IntPtr objPtr);

            /// <summary>
            /// Decrement reference count
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_codec_release")]
            public static partial uint Release(IntPtr objPtr);

            /// <summary>
            /// get processing data
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_codec_get_processing_data")]
            public static partial IntPtr GetProcessingData(IntPtr objPtr);

            /// <summary>
            /// set processing data
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_codec_set_processing_data")]
            public static partial int SetProcessingData(IntPtr objPtr, IntPtr strPtr);

            /// <summary>
            /// get processing data type
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_codec_get_data_type")]
            public static partial IntPtr GetDataType(IntPtr objPtr);

            /// <summary>
            /// set processing data type
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_codec_set_data_type")]
            public static partial int SetDataType(IntPtr objPtr, IntPtr strPtr);

            /// <summary>
            /// decode form encoded string
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_codec_decode")]
            public static partial int Decode(IntPtr objPtr, IntPtr strPtr);

            /// <summary>
            /// encode to string
            /// </summary>
            /// <param name="objPtr"></param>
            /// <returns></returns>
            [LibraryImport("oldl", EntryPoint = "oldl_codec_encode")]
            public static partial IntPtr Encode(IntPtr objPtr);
        }

        /// <summary>
        /// Native Object pointer
        /// </summary>
        public IntPtr ObjectPtr { get; private set; }

        /// <summary>
        /// processing data
        /// </summary>
        public string DataType
        {
            get
            {
                Str strObj = GetDataTypeAsStr();
                return strObj?.GetContentsAsString();
            }

            set
            {
                if (value != null)
                {
                    using Str strObj = new(value);

                    _ = NativeMethods.SetDataType(ObjectPtr, strObj.ObjectPtr);
                }
            }
        }

        /// <summary>
        /// DataContents
        /// </summary>
        public byte[] Data
        {
            get
            {
                using Str strObj = GetProcessingDataAsStr();
                return strObj?.Contents;
            }

            set => SetProcessingData(value);
        }

        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// constructor
        /// </summary>
        public Codec()
        {
            AttachRef(NativeMethods.CreateI());
        }

        ~Codec()
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
        /// Get processing data as str
        /// </summary>
        /// <returns></returns>
        private Str GetProcessingDataAsStr()
        {
            IntPtr strPtr = NativeMethods.GetProcessingData(ObjectPtr);
            Str result = null;
            if (IntPtr.Zero != strPtr)
            {
                result = new Str(strPtr);
            }

            return result;
        }

        /// <summary>
        /// set processing data
        /// </summary>
        /// <param name="data"></param>
        private void SetProcessingData(byte[] data)
        {
            if (data != null)
            {
                using Str strData = new(data);

                SetProcessingDataAsStr(strData);
            }
            else
            {
                SetProcessingDataAsStr(null);
            }
        }

        /// <summary>
        /// set processing data as Str
        /// </summary>
        /// <param name="strData"></param>
        private void SetProcessingDataAsStr(Str strData)
        {
            if (strData != null)
            {
                _ = NativeMethods.SetProcessingData(ObjectPtr, strData.ObjectPtr);
            }
            else
            {
                _ = NativeMethods.SetProcessingData(ObjectPtr, IntPtr.Zero);
            }
        }

        /// <summary>
        /// get data type as Str
        /// </summary>
        /// <returns></returns>
        private Str GetDataTypeAsStr()
        {
            IntPtr typePtr = NativeMethods.GetDataType(ObjectPtr);
            Str result = null;
            if (IntPtr.Zero != typePtr)
            {
                result = new Str(typePtr);
            }

            return result;
        }

        internal void Decode(Str encodedStr)
        {
            if (encodedStr != null)
            {
                _ = NativeMethods.Decode(ObjectPtr, encodedStr.ObjectPtr);
            }
        }

        internal Str EncodeI()
        {
            IntPtr encodedPtr = NativeMethods.Encode(ObjectPtr);

            Str result = null;
            if (IntPtr.Zero != encodedPtr)
            {
                result = new Str(encodedPtr);
            }

            return result;
        }

        /// <summary>
        /// encode codec contents
        /// </summary>
        /// <returns></returns>
        public byte[] Encode()
        {
            using Str strData = EncodeI();
            return strData?.Contents;
        }
    }
}
