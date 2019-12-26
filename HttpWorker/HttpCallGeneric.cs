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
    /// <typeparam name="TResult">return type</typeparam>
    public class HttpCall<TResult>
        : IHttpCall<TResult>
    {
        private readonly Func<HttpResponseMessage, string, TResult> _responseConverter;
        private readonly TaskCompletionSource<TResult> _taskCompletionSource = new TaskCompletionSource<TResult>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="responseConverter">Function to convert HTTP response to type T</param>
        public HttpCall(Func<HttpResponseMessage, string, TResult> responseConverter)
        {
            _responseConverter = responseConverter ?? throw new ArgumentNullException(nameof(responseConverter));
        }

        public HttpCallTypeEnum HttpType { get; set; }
        public Uri Uri { get; set; }
        public HttpContent Content { get; set; }
        
        public Task<TResult> Task => _taskCompletionSource.Task;
        Task IHttpCall.Task => null;

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
                var result = _responseConverter(response, content);
                _taskCompletionSource.SetResult(result);
            }
            catch (Exception ex)
            {
                _taskCompletionSource.SetException(ex);
            }
        }

        /// <summary>
        /// Function to set exception.
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns></returns>
        public void SetException(Exception exception)
        {
            _taskCompletionSource.SetException(exception);
        }
    }
}
