using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HttpWorker.Interfaces;

namespace HttpWorker
{
    /// <summary>
    /// Class that represent one request.
    /// It store all data to run request and contains Task to return result.
    /// </summary>
    public class HttpCall
        : IHttpCall
    {
        private readonly TaskCompletionSource<bool> _taskCompletionSource = new TaskCompletionSource<bool>();

        public HttpCallTypeEnum HttpType { get; set; }
        public Uri Uri { get; set; }
        public HttpContent Content { get; set; }
        
        public Task Task => _taskCompletionSource.Task;

        /// <summary>
        /// Function to set result of this call.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="response"></param>
        public void SetResult(HttpStatusCode statusCode, string response)
        {
            try
            {
                _taskCompletionSource.SetResult(true);
            }
            catch (Exception ex)
            {
                _taskCompletionSource.SetException(ex);
            }
        }
    }
}
