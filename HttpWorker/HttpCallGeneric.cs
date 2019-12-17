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
    /// <typeparam name="TResult">return type</typeparam>
    public class HttpCall<TResult>
        : IHttpCall<TResult>
    {
        private readonly Func<HttpStatusCode, string, TResult> _responseConverter;
        private readonly TaskCompletionSource<TResult> _taskCompletionSource = new TaskCompletionSource<TResult>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="responseConverter">Function to convert HTTP response to type T</param>
        public HttpCall(Func<HttpStatusCode, string, TResult> responseConverter)
        {
            this._responseConverter = responseConverter ?? throw new ArgumentNullException(nameof(responseConverter));
        }

        public HttpCallTypeEnum HttpType { get; set; }
        public Uri Uri { get; set; }
        public HttpContent Content { get; set; }
        
        public Task<TResult> Task => _taskCompletionSource.Task;
        Task IHttpCall.Task => null;

        /// <summary>
        /// Function to set result of this call.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="response"></param>
        public void SetResult(HttpStatusCode statusCode, string response)
        {
            try
            {
                var result = _responseConverter(statusCode, response);
                _taskCompletionSource.SetResult(result);
            }
            catch (Exception ex)
            {
                _taskCompletionSource.SetException(ex);
            }
        }
    }
}
