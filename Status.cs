using System;
using System.Runtime.InteropServices;


namespace oldlclr
{
    public class Status : IDisposable, ICloneable
    {
        /// <summary>
        /// Intanciate receiver in process heap
        /// </summary>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_create")]
        private static extern IntPtr CreateI();

        /// <summary>
        /// Increment reference count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_retain")]
        private static extern uint Retain(IntPtr objPtr);

        /// <summary>
        /// Decrement reference count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_release")]
        private static extern uint Release(IntPtr objPtr);


        /// <summary>
        /// Set started time of processing
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_set_started_time_of_processing")]
        private static extern int SetStartedTimeOfProcessing(IntPtr objPtr, IntPtr strPtr);

        /// <summary>
        /// Get started time of processing
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_get_started_time_of_processing")]
        private static extern IntPtr GetStartedTimeOfProcessing(IntPtr objPtr);



        /// <summary>
        /// Set started time of loading
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_set_started_time_of_loading")]
        private static extern int SetStartedTimeOfLoading(IntPtr objPtr, IntPtr strPtr);

        /// <summary>
        /// Get started time of loading
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_get_started_time_of_loading")]
        private static extern IntPtr GetStartedTimeOfLoading(IntPtr objPtr);




        /// <summary>
        /// Set finished time of processing
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_set_finished_time_of_processing")]
        private static extern int SetFinishedTimeOfProcessing(IntPtr objPtr, IntPtr strPtr);

        /// <summary>
        /// Get finished time of processing
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_get_finished_time_of_processing")]
        private static extern IntPtr GetFinishedTimeOfProcessing(IntPtr objPtr);



        /// <summary>
        /// Set finished time of loading
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_set_finished_time_of_loading")]
        private static extern int SetFinishedTimeOfLoading(IntPtr objPtr, IntPtr strPtr);

        /// <summary>
        /// Get data name
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_get_data_name")]
        private static extern IntPtr GetDataName(IntPtr objPtr);



        /// <summary>
        /// Set data name
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_set_data_name")]
        private static extern int SetDataName(IntPtr objPtr, IntPtr strPtr);


        /// <summary>
        /// Get finished time of loading
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_get_finished_time_of_loading")]
        private static extern IntPtr GetFinishedTimeOfLoading(IntPtr objPtr);


        /// <summary>
        /// Set processed count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_set_processed_count")]
        private static extern int SetProcessedCount(IntPtr objPtr, uint processedCount);

        /// <summary>
        /// Get processed count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>          
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_get_processed_count")]
        private static extern int GetProcessedCount(IntPtr objPtr, out uint processedCount);




        /// <summary>
        /// Set processed count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_set_processing_status")]
        private static extern int SetStatusCode(IntPtr objPtr, int statusCode);

        /// <summary>
        /// Get processed count
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_get_processing_status")]
        private static extern int GetStatusCode(IntPtr objPtr, ref int statusCode);


        /// <summary>
        /// To json string
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_receiver_hdlr_status_to_json")]
        private static extern IntPtr ToJsonStr(IntPtr objPtr);




        /// <summary>
        /// date time format
        /// </summary>
        private static string DateTimeFormat => "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Laser processed count
        /// </summary>

        public uint ProcessedCount
        {
            get
            {
                GetProcessedCount(ObjectPtr, out var result);
                return result;
            }
            set => SetProcessedCount(ObjectPtr, value);
        }

        /// <summary>
        /// StatusCode
        /// </summary>
        public StatusCode Code
        {
            get => GetStatusCode();
            set => SetStatusCode(value);
        }

        /// <summary>
        /// Native Object pointer
        /// </summary>

        public IntPtr ObjectPtr { get; private set; }

        private bool disposedValue;

        /// <summary>
        /// constructor
        /// </summary>
        public Status()
        {
            AttachRef(CreateI());
        }



        /// <summary>
        /// destructor
        /// </summary>
        ~Status()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        /// <summary>
        ///  bind this and objPtr
        /// </summary>
        /// <param name="objPtr"></param>
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
            Status result = (Status)this.MemberwiseClone();

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
        /// set started time of processing
        /// </summary>
        /// <param name="timeOfProcessing"></param>
        public void SetStartedTimeOfProcessing(Str timeOfProcessing)
        {
            if (timeOfProcessing != null)
            {
                SetStartedTimeOfProcessing(ObjectPtr, timeOfProcessing.ObjectPtr);
            }
        }

        /// <summary>
        /// set started time of processing
        /// </summary>
        /// <param name="timeOfProcessing"></param>
        public void SetStartedTimeOfProcessing(string timeOfProcessing)
        {
            if (timeOfProcessing != null)
            {
                using Str strObj = new(timeOfProcessing);
                SetStartedTimeOfProcessing(strObj);
            }
        }

        /// <summary>
        /// set started time of processing
        /// </summary>
        /// <param name="timeOfProcessing"></param>
        public void SetStartedTimeOfProcessing(DateTime timeOfProcessing)
        {
            SetStartedTimeOfProcessing(timeOfProcessing.ToString(DateTimeFormat));
        }

        /// <summary>
        /// set started time of processing
        /// </summary>
        /// <param name="timeOfProcessing"></param>
        public void SetStartedTimeOfProcessing(DateTime? timeOfProcessing)
        {
            if (timeOfProcessing != null)
            {
                SetStartedTimeOfProcessing((DateTime)timeOfProcessing);
            }
        }

        /// <summary>
        /// set started time of loading
        /// </summary>
        /// <param name="timeOfLoading"></param>
        public void SetStartedTimeOfLoading(Str timeOfLoading)
        {
            if (timeOfLoading != null)
            {
                SetStartedTimeOfLoading(ObjectPtr, timeOfLoading.ObjectPtr);
            }
        }

        /// <summary>
        /// set started time of loading
        /// </summary>
        /// <param name="timeOfLoading"></param>
        public void SetStartedTimeOfLoading(string timeOfLoading)
        {
            if (timeOfLoading != null)
            {
                using Str strObj = new(timeOfLoading);
                SetStartedTimeOfLoading(strObj);
            }
        }

        /// <summary>
        /// set started time of loading
        /// </summary>
        /// <param name="timeOfLoading"></param>
        public void SetStartedTimeOfLoading(DateTime timeOfLoading)
        {
            SetStartedTimeOfLoading(timeOfLoading.ToString(DateTimeFormat));
        }

        /// <summary>
        /// set started time of loading
        /// </summary>
        /// <param name="timeOfLoading"></param>
        public void SetStartedTimeOfLoading(DateTime? timeOfLoading)
        {
            if (timeOfLoading != null)
            {
                SetStartedTimeOfLoading((DateTime)timeOfLoading);
            }
        }

        /// <summary>
        /// set finished time of loading
        /// </summary>
        /// <param name="timeOfLoading"></param>
        public void SetFinishedTimeOfLoading(Str timeOfLoading)
        {
            if (timeOfLoading != null)
            {
                SetFinishedTimeOfLoading(ObjectPtr, timeOfLoading.ObjectPtr);
            }
        }

        /// <summary>
        /// set finished time of loading
        /// </summary>
        /// <param name="timeOfLoading"></param>
        public void SetFinishedTimeOfLoading(string timeOfLoading)
        {
            if (timeOfLoading != null)
            {
                using Str strObj = new(timeOfLoading);
                SetFinishedTimeOfLoading(strObj);
            }
        }

        /// <summary>
        /// set finished time of loading
        /// </summary>
        /// <param name="timeOfLoading"></param>
        public void SetFinishedTimeOfLoading(DateTime timeOfLoading)
        {
            SetFinishedTimeOfLoading(timeOfLoading.ToString(DateTimeFormat));
        }

        /// <summary>
        /// set finished time of loading
        /// </summary>
        /// <param name="timeOfLoading"></param>
        public void SetFinishedTimeOfLoading(DateTime? timeOfLoading)
        {
            if (timeOfLoading != null)
            {
                SetFinishedTimeOfLoading((DateTime)timeOfLoading);
            }
        }

        /// <summary>
        /// set finished time of processing
        /// </summary>
        /// <param name="timeOfProcessing"></param>
        public void SetFinishedTimeOfProcessing(Str timeOfProcessing)
        {
            if (timeOfProcessing != null)
            {
                SetFinishedTimeOfProcessing(ObjectPtr, timeOfProcessing.ObjectPtr);
            }
        }


        /// <summary>
        /// set data name
        /// </summary>
        /// <param name="dataName"></param>
        public void SetDataName(Str dataName)
        {
            if (dataName != null)
            {
                SetDataName(ObjectPtr, dataName.ObjectPtr);
            }
        }

        /// <summary>
        /// set data name
        /// </summary>
        /// <param name="dataName"></param>
        public void SetDataName(string dataName)
        {
            if (dataName != null)
            {
                using Str strObj = new(dataName);
                SetDataName(strObj);
            }
        }


        /// <summary>
        /// set finished time of processing
        /// </summary>
        /// <param name="timeOfProcessing"></param>
        public void SetFinishedTimeOfProcessing(string timeOfProcessing)
        {
            if (timeOfProcessing != null)
            {
                using Str strObj = new(timeOfProcessing);
                SetFinishedTimeOfProcessing(strObj);
            }
        }

        /// <summary>
        /// set finished time of processing
        /// </summary>
        /// <param name="timeOfProcessing"></param>
        public void SetFinishedTimeOfProcessing(DateTime timeOfProcessing)
        {
            SetFinishedTimeOfProcessing(timeOfProcessing.ToString(DateTimeFormat));
        }

        /// <summary>
        /// set finished time of processing
        /// </summary>
        /// <param name="timeOfProcessing"></param>
        public void SetFinishedTimeOfProcessing(DateTime? timeOfProcessing)
        {
            if (timeOfProcessing != null)
            {
                SetFinishedTimeOfProcessing((DateTime)timeOfProcessing);
            }
        }

        /// <summary>
        /// set status code
        /// </summary>
        /// <param name="code"></param>
        public void SetStatusCode(StatusCode code)
        {
            SetStatusCode(ObjectPtr, (int)code);
        }


        /// <summary>
        /// get status code
        /// </summary>
        /// <returns></returns>
        public StatusCode GetStatusCode()
        {
            int code = 0;
            GetStatusCode(ObjectPtr, ref code);
            return (StatusCode)code;
        }

        /// <summary>
        /// to json string
        /// </summary>
        /// <returns></returns>
        public Str ToJsonStr()
        {
            Str result = null;
            IntPtr jsonPtr = ToJsonStr(ObjectPtr);
            if (IntPtr.Zero != jsonPtr)
            {
                result = new Str(jsonPtr);
            }
            return result;
        }

        /// <summary>
        /// to json
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            using Str jsonStr = ToJsonStr();
            return jsonStr?.GetContentsAsString();
        }
    }
}
