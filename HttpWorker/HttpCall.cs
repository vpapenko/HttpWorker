using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpWorker
{
    /// <summary>
    /// Class that represent one request.
    /// It store all data to run request and contains Task to return result.
    /// </summary>
    public class HttpCall
        : IHttpCall
    {
        TaskCompletionSource<bool> TaskCompletionSource = new TaskCompletionSource<bool>();

        public HttpCallTypeEnum HttpType { get; set; }
        public Uri Uri { get; set; }
        public HttpContent Content { get; set; }
        
        public Task Task { get { return TaskCompletionSource.Task; } }

        /// <summary>
        /// Function to set result of this call.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="response"></param>
        public void SetResult(HttpStatusCode statusCode, string response)
        {
            try
            {
                TaskCompletionSource.SetResult(true);
            }
            catch (Exception ex)
            {
                TaskCompletionSource.SetException(ex);
            }
        }
    }
}
