using System;
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
        readonly Worker httpWorker = new Worker();

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

        /// <summary>
        /// Count of unprocessed HTTP calls.
        /// </summary>
        public int CountOfUnprocessedHttpCalls
        {
            get
            {
                return httpWorker.CountOfUnprocessedHttpCalls;
            }
        }

        /// <summary>
        /// Indicate that network is not available
        /// </summary>
        public bool NetworkNotAvailable
        {
            get
            {
                return httpWorker.NetworkNotAvailable;
            }
        }

        /// <summary>
        /// Indicate that long operation is in process
        /// </summary>
        public bool LongOperationInProcess
        {
            get
            {
                return httpWorker.LongOperationInProcess;
            }
        }

        /// <summary>
        /// Indicate that worker is working.
        /// </summary>
        public bool Working
        {
            get
            {
                return httpWorker.Working;
            }
        }

        /// <summary>
        /// Indicate that long time operation is in process
        /// </summary>
        public double LongOperationStartTime
        {
            get
            {
                return httpWorker.LongOperationStartTime;
            }
            set
            {
                httpWorker.LongOperationStartTime = value;
            }
        }

        /// <summary>
        /// In case of unsuccessful requests, after this count of attempts we slow down and make sleep before next attempt.
        /// Also after this count of attempt we set NetworkNotAvailable statuses.
        /// </summary>
        public int RetrySleepTimer1 
        {
            get => httpWorker.RetrySleepTimer1;
            set => httpWorker.RetrySleepTimer1 = value;
        }
        public int RetrySleepTime1 
        {
            get => httpWorker.RetrySleepTime1;
            set => httpWorker.RetrySleepTime1 = value;
        }

        /// <summary>
        /// If count of unsuccessful attempts are higher we slow down more.
        /// </summary>
        public int RetrySleepTimer2 
        {
            get => httpWorker.RetrySleepTimer2;
            set => httpWorker.RetrySleepTimer2 = value;
        }
        public int RetrySleepTime2
        {
            get => httpWorker.RetrySleepTime2;
            set => httpWorker.RetrySleepTime2 = value;
        }


        /// <summary>
        /// Add HTTP get to queue
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="responseConverter">Func to convert HTTP response to <TResult> type</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        public async Task<TResult> AddGetCall<TResult>(Uri uri, Func<HttpStatusCode, string, TResult> responseConverter)
        {
            HttpCall<TResult> call = new HttpCall<TResult>(responseConverter)
            {
                HttpType = HttpCallTypeEnum.Get,
                Uri = uri
            };
            return await httpWorker.AddCall<TResult>(call);
        }

        /// <summary>
        /// Add HTTP post to queue
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server</param>
        /// <param name="responseConverter">Func to convert HTTP response to <TResult> type</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        public async Task<TResult> AddPostCall<TResult>(Uri uri, HttpContent content, Func<HttpStatusCode, string, TResult> responseConverter)
        {
            HttpCall<TResult> call = new HttpCall<TResult>(responseConverter)
            {
                HttpType = HttpCallTypeEnum.Post,
                Uri = uri,
                Content = content
            };
            return await httpWorker.AddCall<TResult>(call);
        }

        /// <summary>
        /// Add HTTP delete to queue
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="responseConverter">Func to convert HTTP response to <TResult> type</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        public async Task<TResult> AddDeleteCall<TResult>(Uri uri, Func<HttpStatusCode, string, TResult> responseConverter)
        {
            HttpCall<TResult> call = new HttpCall<TResult>(responseConverter)
            {
                HttpType = HttpCallTypeEnum.Delete,
                Uri = uri
            };
            return await httpWorker.AddCall<TResult>(call);
        }

        /// <summary>
        /// Add HTTP put to queue
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server</param>
        /// <param name="responseConverter">Func to convert HTTP response to <TResult> type</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        public async Task<TResult> AddPutCall<TResult>(Uri uri, HttpContent content, Func<HttpStatusCode, string, TResult> responseConverter)
        {
            HttpCall<TResult> call = new HttpCall<TResult>(responseConverter)
            {
                HttpType = HttpCallTypeEnum.Put,
                Uri = uri,
                Content = content
            };
            return await httpWorker.AddCall<TResult>(call);
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private void HttpWorker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Worker.NetworkNotAvailable))
            {
                OnPropertyChanged(nameof(NetworkNotAvailable));
            }
            else if (e.PropertyName == nameof(Worker.LongOperationInProcess))
            {
                OnPropertyChanged(nameof(LongOperationInProcess));
            }
            else if (e.PropertyName == nameof(Worker.Working))
            {
                OnPropertyChanged(nameof(Working));
            }
            else if (e.PropertyName == nameof(Worker.CountOfUnprocessedHttpCalls))
            {
                OnPropertyChanged(nameof(CountOfUnprocessedHttpCalls));
            }
        }
    }
}
