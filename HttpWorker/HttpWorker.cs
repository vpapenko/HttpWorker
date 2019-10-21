using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;

namespace HttpWorker
{
    internal class HttpWorker : INotifyPropertyChanged
    {
        ///Dictionary for unprocessed HttpCall
        readonly Dictionary<IHttpCall, bool> httpCallDictionary = new Dictionary<IHttpCall, bool>();

        ///Timer to switch LongOperation property.
        readonly System.Timers.Timer longOperationTimer;

        bool networkNotAvailable;
        bool longOperationInProcess;
        bool working;


        public HttpWorker()
        {
            longOperationTimer = new System.Timers.Timer(2000);
            longOperationTimer.Elapsed += LongOperationTimer_Elapsed;
            longOperationTimer.AutoReset = false;
            longOperationTimer.Enabled = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        ///HTTP client. Can be addition setup in API implementation
        public HttpClient Client { get; private set; } = new HttpClient();

        /// <summary>
        /// Indicate that long time operation is in process
        /// </summary>
        public double LongOperationStartTime
        {
            get
            {
                return longOperationTimer.Interval;
            }
            set
            {
                longOperationTimer.Interval = value;
            }
        }

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
        /// Indicate that network is not available
        /// </summary>
        public bool NetworkNotAvailable
        {
            get
            {
                return networkNotAvailable;
            }
            private set
            {
                if (value != networkNotAvailable)
                {
                    networkNotAvailable = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Indicate that long operation is in process
        /// </summary>
        public bool LongOperationInProcess
        {
            get
            {
                return longOperationInProcess;
            }
            private set
            {
                if (value != longOperationInProcess)
                {
                    longOperationInProcess = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Indicate that worker is working.
        /// </summary>
        public bool Working
        {
            get
            {
                return working;
            }
            private set
            {
                if (value != working)
                {
                    working = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Add new request.
        /// Worker will start automatically
        /// </summary>
        /// <param name="call"></param>
        public void Add(IHttpCall call)
        {
            lock (httpCallDictionary)
            {
                httpCallDictionary.Add(call, true);
                longOperationTimer.Start();
                Working = true;
                ThreadPool.QueueUserWorkItem(WorkUntilSuccess, call);
            }
        }


        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LongOperationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            LongOperationInProcess = true;
        }

        /// <summary>
        /// Try to process HttpCall until success or manual stop.
        /// </summary>
        /// <param name="call">HttpCall to process</param>
        private void WorkUntilSuccess(object callObject)
        {
            IHttpCall call = (IHttpCall)callObject;
            long count = 0;
            int sleep = 0;
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
                bool success = ProcessCall(call);
                if (success)
                {
                    NetworkNotAvailable = false;
                    Remove(call);
                    return;
                }

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
                HttpResponseMessage response = null;
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
                        throw new NotSupportedException(string.Format("Not supported HttpCallTypeEnum: {0}"
                            , HttpCallTypeEnum.Put.ToString()));
                }
                string responseString = "";

                if (response?.IsSuccessStatusCode == true)
                {
                    responseString = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }

                call.SetResult(response.StatusCode, responseString);
                return true;
            }
            catch(HttpRequestException)
            {
                return false;
            }
        }

        /// <summary>
        /// Remove processed call. If all calls are processed then update statuses.
        /// </summary>
        /// <param name="call">Processed call</param>
        private void Remove(IHttpCall call)
        {
            lock (httpCallDictionary)
            {
                httpCallDictionary.Remove(call);
                if (httpCallDictionary.Count == 0)
                {
                    longOperationTimer.Stop();
                    LongOperationInProcess = false;
                    Working = false;
                }
            }
        }
    }
}
