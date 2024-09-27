using System;

namespace oldlclr
{
    /// <summary>
    /// server
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// time of started loading
        /// </summary>
        DateTime? TimeOfStartedLoading
        {
            get;
        }

        /// <summary>
        /// time of finished loading
        /// </summary>
        DateTime? TimeOfFinishedLoading
        {
            get;
        }

        /// <summary>
        /// time of started processing
        /// </summary>
        DateTime? TimeOfStartedProcessing
        {
            get;
        }

        /// <summary>
        /// time of finished processing
        /// </summary>
        DateTime? TimeOfFinishedProcessing
        {
            get;
        }

        /// <summary>
        /// laser processing data name
        /// </summary>
        string DataName
        {
            get;
        }

        /// <summary>
        /// Processing count
        /// </summary>
        uint ProcessingCount
        {
            get;
        }

        /// <summary>
        /// service status
        /// </summary>
        StatusCode StatusCode
        {
            get;
        }

        /// <summary>
        /// load data processing data
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="data"></param>
        ErrorCode LoadProcessingData(string dataType, byte[] data, IntPtr dataName);
    }
}
