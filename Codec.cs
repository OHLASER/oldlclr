using System;
using System.Runtime.InteropServices;


namespace oldlclr
{
    public class Codec : IDisposable, ICloneable
    {


        /// <summary>
        /// Intanciate reciever in process heap
        /// </summary>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_codec_create")]
        private static extern IntPtr CreateI();


        /// <summary>
        /// Increment reference count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_codec_retain")]
        private static extern uint Retain(IntPtr objPtr);

        /// <summary>
        /// Decrement reference count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_codec_release")]
        private static extern uint Release(IntPtr objPtr);


        /// <summary>
        /// get processing data
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_codec_get_processing_data")]
        private static extern IntPtr GetProcessingData(IntPtr objPtr);

        /// <summary>
        /// set processing data
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_codec_set_processing_data")]
        private static extern int SetProcessingData(IntPtr objPtr, IntPtr strPtr);



        /// <summary>
        /// get processing data type
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_codec_get_data_type")]
        private static extern IntPtr GetDataType(IntPtr objPtr);

        /// <summary>
        /// set processing data type
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_codec_set_data_type")]
        private static extern int SetDataType(IntPtr objPtr, IntPtr strPtr);


        /// <summary>
        /// decode form encoded string
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_codec_decode")]
        private static extern int Decode(IntPtr objPtr, IntPtr strPtr);



        /// <summary>
        /// encode to string
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_codec_encode")]
        private static extern IntPtr Encode(IntPtr objPtr);

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

                    SetDataType(ObjectPtr, strObj.ObjectPtr);
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
            AttachRef(CreateI());
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
            throw new NotImplementedException();
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
            IntPtr strPtr = GetProcessingData(ObjectPtr);
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
                SetProcessingData(ObjectPtr, strData.ObjectPtr);
            }
            else
            {
                SetProcessingData(ObjectPtr, IntPtr.Zero);
            }
        }

        /// <summary>
        /// get data type as Str
        /// </summary>
        /// <returns></returns>
        private Str GetDataTypeAsStr()
        {
            IntPtr typePtr = GetDataType(ObjectPtr);
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

                Decode(ObjectPtr, encodedStr.ObjectPtr);
            }
        }

        internal Str EncodeI()
        {
            IntPtr encodedPtr = Encode(ObjectPtr);

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
