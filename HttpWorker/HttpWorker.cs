using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using HttpWorker.Interfaces;

namespace HttpWorker
{
    public class Worker : INotifyPropertyChanged
    {
        ///Dictionary for unprocessed HttpCall
        private readonly HashSet<IHttpCall> _httpCallHashSet = new HashSet<IHttpCall>();

        ///Timer to switch LongOperation property.
        private readonly System.Timers.Timer _longOperationTimer;

        private int _countOfUnprocessedHttpCalls;
        private bool _networkNotAvailable;
        private bool _longOperationInProcess;
        private bool _working;

        public Worker()
        {
            _longOperationTimer = new System.Timers.Timer(20);
            _longOperationTimer.Elapsed += LongOperationTimer_Elapsed;
            _longOperationTimer.AutoReset = false;
            _longOperationTimer.Enabled = false;
        }

        public Worker(HttpClient client)
            : this()
        {
            Client = client;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        ///HTTP client. Can be addition setup in API implementation
        public HttpClient Client { get; } = new HttpClient();

        /// <summary>
        /// Count of unprocessed HTTP calls.
        /// </summary>
        public int CountOfUnprocessedHttpCalls
        {
            get => _countOfUnprocessedHttpCalls;
            private set
            {
                if (_countOfUnprocessedHttpCalls == value) return;
                _countOfUnprocessedHttpCalls = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indicate that long time operation is in process
        /// </summary>
        public double LongOperationStartTime
        {
            get
            {
                lock (_httpCallHashSet)
                {
                    return _longOperationTimer.Interval;
                }
            }
            set
            {
                lock (_httpCallHashSet)
                {
                    _longOperationTimer.Interval = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Indicate that network is not available
        /// </summary>
        public bool NetworkNotAvailable
        {
            get => _networkNotAvailable;
            private set
            {
                if (_networkNotAvailable == value) return;
                _networkNotAvailable = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indicate that long operation is in process
        /// </summary>
        public bool LongOperationInProcess
        {
            get => _longOperationInProcess;
            private set
            {
                if (_longOperationInProcess == value) return;
                _longOperationInProcess = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indicate that worker is working.
        /// </summary>
        public bool Working
        {
            get => _working;
            private set
            {
                if (_working == value) return;
                _working = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// List of exception types which expected during http request.
        /// Worker will retry request if exception of this type occurred.
        /// </summary>
        public List<Type> RetryOnException { get; } = new List<Type>() { typeof(HttpRequestException) };

        /// <summary>
        /// In case of unsuccessful requests, after this count of attempts we slow down and make sleep before next attempt.
        /// Also after this count of attempt we set NetworkNotAvailable statuses.
        /// </summary>
        public int RetrySleepTimer1 { get; set; } = 2;
        public int RetrySleepTime1 { get; set; } = 2000;

        /// <summary>
        /// If count of unsuccessful attempts are higher we slow down more.
        /// </summary>
        public int RetrySleepTimer2 { get; set; } = 50;
        public int RetrySleepTime2 { get; set; } = 15000;


        /// <summary>
        /// Add new request and return request result.
        /// </summary>
        /// <param name="call"></param>
        public async Task AddCall(IHttpCall call)
        {
            try
            {
                await AddAndCall(call);
            }
            finally
            {
                Remove(call);
            }
        }

        /// <summary>
        /// Add new request and return request result.
        /// </summary>
        /// <param name="call"></param>
        public async Task<TResult> AddCall<TResult>(IHttpCall<TResult> call)
        {
            try
            {
                var result = await AddAndCall(call);
                return result;
            }
            finally
            {
                Remove(call);
            }
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Try to process HttpCall until success or manual stop.
        /// </summary>
        /// <param name="callObject">HttpCall to process</param>
        private void WorkUntilSuccess(object callObject)
        {
            var call = (IHttpCall)callObject;
            var count = 0;
            var sleep = 0;
            while (true)
            {
                //Counting attempts, updating statuses and try to process request.
                count++;
                if (count > RetrySleepTimer2)
                {
                    sleep = RetrySleepTime2;
                }
                else if (count > RetrySleepTimer1)
                {
                    NetworkNotAvailable = true;
                    sleep = RetrySleepTime1;
                }
                Thread.Sleep(sleep);
                var success = ProcessCall(call);
                if (!success) continue;
                NetworkNotAvailable = false;
                return;

            }
        }

        /// <summary>
        /// Try to make HTTP request
        /// </summary>
        /// <param name="call">HttpCall to precess</param>
        /// <returns>Is request successful</returns>
        private bool ProcessCall(IHttpCall call)
        {
            try
            {
                HttpResponseMessage response;
                switch (call.HttpType)
                {
                    case HttpCallTypeEnum.Get:
                        response = Client.GetAsync(call.Uri).ConfigureAwait(false).GetAwaiter().GetResult();
                        break;
                    case HttpCallTypeEnum.Post:
                        response = Client.PostAsync(call.Uri, call.Content).ConfigureAwait(false).GetAwaiter().GetResult();
                        break;
                    case HttpCallTypeEnum.Delete:
                        response = Client.DeleteAsync(call.Uri).ConfigureAwait(false).GetAwaiter().GetResult();
                        break;
                    case HttpCallTypeEnum.Put:
                        response = Client.PutAsync(call.Uri, call.Content).ConfigureAwait(false).GetAwaiter().GetResult();
                        break;
                    default:
                        throw new NotSupportedException(
                            $"Not supported HttpCallTypeEnum: {HttpCallTypeEnum.Put.ToString()}");
                }
                var content = "";

                if (response?.IsSuccessStatusCode == true)
                {
                    content = response.Content?.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }

                if (response != null) call.SetResult(response, content);
                return true;
            }
            catch (Exception ex)
            {
                var type = ex.GetType();
                if (RetryOnException.Contains(type))
                {
                    return false;
                }
                else if (type == typeof(AggregateException))
                {
                    foreach (var innerException in ((AggregateException)ex).InnerExceptions)
                    {
                        if (RetryOnException.Contains(innerException.GetType()))
                        {
                            return false;
                        }
                    }
                }
                call.SetException(ex);
                return true;
            }
        }

        private async Task<TResult> AddAndCall<TResult>(IHttpCall<TResult> call)
        {
            var task = call.Task;
            Add(call);
            var result = await task;
            return result;
        }

        private async Task AddAndCall(IHttpCall call)
        {
            var task = call.Task;
            Add(call);
            await task;
        }

        private void Add(IHttpCall call)
        {
            lock (_httpCallHashSet)
            {
                _httpCallHashSet.Add(call);
                CountOfUnprocessedHttpCalls = _httpCallHashSet.Count;
                if (!_longOperationTimer.Enabled)
                {
                    _longOperationTimer.Start();
                }
                Working = true;
                ThreadPool.QueueUserWorkItem(WorkUntilSuccess, call);
            }
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        /// <summary>
        /// Remove processed call. If all calls are processed then update statuses.
        /// </summary>
        /// <param name="call">Processed call</param>
        internal void Remove(IHttpCall call)
        {
            lock (_httpCallHashSet)
            {
                _httpCallHashSet.Remove(call);
                if (_httpCallHashSet.Count == 0)
                {
                    _longOperationTimer.Stop();
                    LongOperationInProcess = false;
                    Working = false;
                }
                CountOfUnprocessedHttpCalls = _httpCallHashSet.Count;
            }
        }

        private void LongOperationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            LongOperationInProcess = true;
        }
    }
}
