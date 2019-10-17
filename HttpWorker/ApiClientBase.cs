using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
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
        readonly HttpWorker httpWorker = new HttpWorker();

        public ApiClientBase()
        {
            httpWorker.PropertyChanged += HttpWorker_PropertyChanged;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Allow to setup HTTP header of requests.
        /// </summary>
        public HttpRequestHeaders DefaultRequestHeaders
        {
            get
            {
                return httpWorker.Client.DefaultRequestHeaders;
            }
        }

        public bool NetworkNotAvailable
        {
            get
            {
                return httpWorker.NetworkNotAvailable;
            }
        }

        public bool Working
        {
            get
            {
                return httpWorker.Working;
            }
        }

        public bool LongOperationInProcess
        {
            get
            {
                return httpWorker.LongOperationInProcess;
            }
        }

        /// <summary>
        /// Add HTTP get to queue
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="responseConverter">Func to convert HTTP response to <T> type</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        public async Task<T> AddGetCall<T>(Uri uri, Func<HttpStatusCode, string, T> responseConverter)
        {
            HttpCall<T> call = new HttpCall<T>(responseConverter)
            {
                HttpType = HttpCallTypeEnum.Get,
                Uri = uri
            };
            httpWorker.Add(call);
            return await call.Task;
        }

        /// <summary>
        /// Add HTTP post to queue
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server</param>
        /// <param name="responseConverter">Func to convert HTTP response to <T> type</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        public async Task<T> AddPostCall<T>(Uri uri, HttpContent content, Func<HttpStatusCode, string, T> responseConverter)
        {
            HttpCall<T> call = new HttpCall<T>(responseConverter)
            {
                HttpType = HttpCallTypeEnum.Post,
                Uri = uri,
                Content = content
            };
            httpWorker.Add(call);
            return await call.Task;
        }

        /// <summary>
        /// Add HTTP delete to queue
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="responseConverter">Func to convert HTTP response to <T> type</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        public async Task<T> AddDeleteCall<T>(Uri uri, Func<HttpStatusCode, string, T> responseConverter)
        {
            HttpCall<T> call = new HttpCall<T>(responseConverter)
            {
                HttpType = HttpCallTypeEnum.Delete,
                Uri = uri
            };
            httpWorker.Add(call);
            return await call.Task;
        }

        /// <summary>
        /// Add HTTP put to queue
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server</param>
        /// <param name="responseConverter">Func to convert HTTP response to <T> type</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        public async Task<T> AddPutCall<T>(Uri uri, HttpContent content, Func<HttpStatusCode, string, T> responseConverter)
        {
            HttpCall<T> call = new HttpCall<T>(responseConverter)
            {
                HttpType = HttpCallTypeEnum.Put,
                Uri = uri,
                Content = content
            };
            httpWorker.Add(call);
            return await call.Task;
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        /// <summary>
        /// Add request to queue.
        /// </summary>
        /// <param name="call"></param>
        private void AddCall(IHttpCall call)
        {
            httpWorker.Add(call);
        }

        private void HttpWorker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HttpWorker.NetworkNotAvailable))
            {
                OnPropertyChanged(nameof(NetworkNotAvailable));
            }
            else if (e.PropertyName == nameof(HttpWorker.LongOperationInProcess))
            {
                OnPropertyChanged(nameof(LongOperationInProcess));
            }
            else if (e.PropertyName == nameof(HttpWorker.Working))
            {
                OnPropertyChanged(nameof(Working));
            }
        }
    }
}
