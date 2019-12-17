using System;
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
        /// <param name="response">HTTP response of operation</param>
        /// <param name="content">Response from server</param>
        /// <returns></returns>
        public void SetResult(HttpResponseMessage response, string content)
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
