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
        static extern IntPtr CreateI();


        /// <summary>
        /// Increment reference count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_codec_retain")]
        static extern uint Retain(IntPtr objPtr);

        /// <summary>
        /// Decrement reference count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_codec_release")]
        static extern uint Release(IntPtr objPtr);


        /// <summary>
        /// get processing data
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_codec_get_processing_data")]
        static extern IntPtr GetProcessingData(IntPtr objPtr);

        /// <summary>
        /// set processing data
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_codec_set_processing_data")]
        static extern int SetProcessingData(IntPtr objPtr, IntPtr strPtr);



        /// <summary>
        /// get processing data type
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_codec_get_data_type")]
        static extern IntPtr GetDataType(IntPtr objPtr);

        /// <summary>
        /// set processing data type
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_codec_set_data_type")]
        static extern int SetDataType(IntPtr objPtr, IntPtr strPtr);


        /// <summary>
        /// decode form encoded string
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldle_codec_decode")]
        static extern int Decode(IntPtr objPtr, IntPtr strPtr);



        /// <summary>
        /// encode to string
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldle_codec_encode")]
        static extern IntPtr Encode(IntPtr objPtr);





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
        /// processing data
        /// </summary>
        public string DataType
        {
            get
            {
                Str strObj;
                strObj = GetDataTypeAsStr();
                string result;
                result = null;
                if (strObj != null)
                {
                    result = strObj.GetContentsAsString();
                }
                return result;
            }

            set
            {
                if (value != null)
                {
                    Str strObj;
                    strObj = new Str(value);

                    SetDataType(ObjectPtr, strObj.ObjectPtr);

                    strObj.Dispose();
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

                byte[] result;
                result = null;

                Str strObj;
                strObj = GetProcessingDataAsStr();
                if (strObj != null)
                {
                    result = strObj.Contents;
                    strObj.Dispose();
                }
                return result;
            }

            set
            {
                SetProcessingData(value);
            }
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
            ObjectPtrValue = objPtr;
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
        Str GetProcessingDataAsStr()
        {
            IntPtr strPtr;
            strPtr = GetProcessingData(ObjectPtr);
            Str result;
            result = null;
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
        void SetProcessingData(byte[] data)
        {
            if (data != null)
            {
                Str strData;
                strData = new Str(data);

                SetProcessingDataAsStr(strData);
                strData.Dispose();
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
        void SetProcessingDataAsStr(Str strData)
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
        Str GetDataTypeAsStr()
        {
            IntPtr typePtr;
            typePtr = GetDataType(ObjectPtr);
            Str result;
            result = null;
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

            IntPtr encodedPtr;
            encodedPtr = Encode(ObjectPtr);

            Str result;
            result = null;
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
            byte[] result;
            Str strData;
            result = null;
            strData = EncodeI();

            if (strData != null)
            {
                result = strData.Contents;
            }
            if (strData != null)
            {
                strData.Dispose();
            }

            return result;
        }

    }
}
