using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HttpWorker
{
    /// <summary>
    /// API base class.
    /// Contains all API states and main methods.
    /// </summary>
    public class ApiClientBase : INotifyPropertyChanged
    {
        readonly Worker _httpWorker = new Worker();

        public ApiClientBase()
        {
            _httpWorker.PropertyChanged += HttpWorker_PropertyChanged;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Allow to setup HTTP header of requests.
        /// </summary>
        public HttpRequestHeaders DefaultRequestHeaders => _httpWorker.Client.DefaultRequestHeaders;

        public int CountOfUnprocessedHttpCalls => _httpWorker.CountOfUnprocessedHttpCalls;

        public bool NetworkNotAvailable => _httpWorker.NetworkNotAvailable;

        public bool LongOperationInProcess => _httpWorker.LongOperationInProcess;

        public bool Working => _httpWorker.Working;

        /// <summary>
        /// Add HTTP get to queue
        /// </summary>
        /// <typeparam name="TResult">Result of operation</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="responseConverter">Func to convert HTTP response to TResult type</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        public async Task<TResult> AddGetCall<TResult>(Uri uri, Func<HttpResponseMessage, string, TResult> responseConverter)
        {
            var call = new HttpCall<TResult>(responseConverter)
            {
                HttpType = HttpCallTypeEnum.Get,
                Uri = uri
            };
            return await _httpWorker.AddCall(call);
        }

        /// <summary>
        /// Add HTTP post to queue
        /// </summary>
        /// <typeparam name="TResult">Result of operation</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server</param>
        /// <param name="responseConverter">Func to convert HTTP response to TResult type</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        public async Task<TResult> AddPostCall<TResult>(Uri uri, HttpContent content, Func<HttpResponseMessage, string, TResult> responseConverter)
        {
            var call = new HttpCall<TResult>(responseConverter)
            {
                HttpType = HttpCallTypeEnum.Post,
                Uri = uri,
                Content = content
            };
            return await _httpWorker.AddCall(call);
        }

        /// <summary>
        /// Add HTTP delete to queue
        /// </summary>
        /// <typeparam name="TResult">Result of operation</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="responseConverter">Func to convert HTTP response to TResult type</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        public async Task<TResult> AddDeleteCall<TResult>(Uri uri, Func<HttpResponseMessage, string, TResult> responseConverter)
        {
            var call = new HttpCall<TResult>(responseConverter)
            {
                HttpType = HttpCallTypeEnum.Delete,
                Uri = uri
            };
            return await _httpWorker.AddCall(call);
        }

        /// <summary>
        /// Add HTTP put to queue
        /// </summary>
        /// <typeparam name="TResult">Result of operation</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server</param>
        /// <param name="responseConverter">Func to convert HTTP response to TResult type</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        public async Task<TResult> AddPutCall<TResult>(Uri uri, HttpContent content, Func<HttpResponseMessage, string, TResult> responseConverter)
        {
            var call = new HttpCall<TResult>(responseConverter)
            {
                HttpType = HttpCallTypeEnum.Put,
                Uri = uri,
                Content = content
            };
            return await _httpWorker.AddCall(call);
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private void HttpWorker_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Worker.NetworkNotAvailable):
                    OnPropertyChanged(nameof(NetworkNotAvailable));
                    break;
                case nameof(Worker.LongOperationInProcess):
                    OnPropertyChanged(nameof(LongOperationInProcess));
                    break;
                case nameof(Worker.Working):
                    OnPropertyChanged(nameof(Working));
                    break;
                case nameof(Worker.CountOfUnprocessedHttpCalls):
                    OnPropertyChanged(nameof(CountOfUnprocessedHttpCalls));
                    break;
            }
        }
    }
}
