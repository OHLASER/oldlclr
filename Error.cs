using System;
using System.Runtime.InteropServices;

namespace oldlclr
{
    /// <summary>
    /// manage data link error
    /// </summary>
    public class Error
    {
        /// <summary>
        /// get error code on this thread.
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_error_get_error")]
        static extern int GetErrorI();


        /// <summary>
        /// get error code on this thread.
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_error_set_error")]
        static extern void SetErrorI(int error);



        /// <summary>
        /// get error code on this thread.
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        [DllImport("oldl", EntryPoint = "oldl_error_get_error_as_json")]
        static extern IntPtr GetErrorAsJsonI();



        /// <summary>
        /// get error code on this thread.
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        public static int GetError()
        {
            return GetErrorI();
        }

        /// <summary>
        /// get error code on this thread.
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        public static void SetError(ErrorCode code)
        {
            SetErrorI((int)code);
        }

        /// <summary>
        /// get error code on this thread.
        /// </summary>
        /// <param name="objPtr"></param>
        /// <returns></returns>
        public static string GetErrorAsJson()
        {
            IntPtr jsonPtr;
            jsonPtr = GetErrorAsJsonI();
            string result;
            result = null;
            if (IntPtr.Zero != jsonPtr)
            {
                Str strObj;
                strObj = new Str(jsonPtr);
                result = strObj.GetContentsAsString();

                strObj.Dispose();
            }

            return result;
        }

    }
}
