using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;

namespace HttpWorker
{
    internal class HttpWorker : INotifyPropertyChanged
    {
        // BackgroundWorker for processing of HttpCall queue
        readonly BackgroundWorker backgroundWorker = new BackgroundWorker();

        //Queue for HttpCall
        readonly ConcurrentQueue<IHttpCall> queue = new ConcurrentQueue<IHttpCall>();

        //Timer to switch LongOperation property.
        readonly System.Timers.Timer longOperationTimer;

        bool networkNotAvailable;
        bool longOperationInProcess;
        bool longRun;
        bool working;
        double longOperationStartTime = 2000;


        public HttpWorker()
        {
            longOperationTimer = new System.Timers.Timer(longOperationStartTime);
            longOperationTimer.Elapsed += LongOperationTimer_Elapsed;
            longOperationTimer.AutoReset = false;
            longOperationTimer.Enabled = false;
            SetupWorker();
        }


        public event RunWorkerCompletedEventHandler RunWorkerCompleted;
        public event PropertyChangedEventHandler PropertyChanged;

        //HTTP client. Can be addition setup in API implementation
        public HttpClient Client { get; private set; } = new HttpClient();

        /// <summary>
        /// Indicate that long time operation is in process
        /// </summary>
        public double LongOperationStartTime
        {
            get
            {
                return longOperationStartTime;
            }
            set
            {
                longOperationStartTime = value;
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


        public IHttpCall[] Queue
        {
            get
            {
                return queue.ToArray();
            }
        }

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
            queue.Enqueue(call);
            Start();
        }

        /// <summary>
        /// Start manually
        /// </summary>
        public void Start()
        {
            longOperationTimer.Start();
            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Stop manually
        /// </summary>
        public void Stop()
        {
            longOperationTimer.Stop();
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
        }

        public void Dispose()
        {
            Stop();
        }


        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetupWorker()
        {
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        private void LongOperationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            LongOperationInProcess = true;
        }

        /// <summary>
        /// This handler will be executed after process of all requests.
        /// We need to stop timer and update states.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            longOperationTimer.Stop();
            LongOperationInProcess = false;
            Working = false;
            RunWorkerCompleted?.Invoke(this, e);
        }

        /// <summary>
        /// Main function of BackgroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //While queue is not empty we process request
                while (queue.TryDequeue(out IHttpCall call))
                {
                    Working = true;
                    WorkUntilSuccess((BackgroundWorker)sender, call);
                }
            }
            finally
            {
                e.Cancel = true;
                e.Result = true;
            }
        }

        /// <summary>
        /// Try to process HttpCall until success or manual stop.
        /// </summary>
        /// <param name="bwAsync">BackgroundWorker to check if cancellation requested</param>
        /// <param name="call">HttpCall to process</param>
        private void WorkUntilSuccess(BackgroundWorker bwAsync, IHttpCall call)
        {
            long count = 0;
            int sleep = 0;
            while (!bwAsync.CancellationPending)
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
                    return;
                }
            }
            return;
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
                        response = Client.GetAsync(call.Uri).Result;
                        break;
                    case HttpCallTypeEnum.Post:
                        response = Client.PostAsync(call.Uri, call.Content).Result;
                        break;
                }

                string responseString = "";

                if (response?.IsSuccessStatusCode == true)
                {
                    responseString = response.Content.ReadAsStringAsync().Result;
                }

                call.SetResult(response.StatusCode, responseString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
