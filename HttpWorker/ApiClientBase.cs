using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace HttpWorker
{
    /// <summary>
    /// API base class.
    /// Contains all API states and main methods.
    /// </summary>
    public abstract class ApiClientBase : INotifyPropertyChanged
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
        /// Add request to queue.
        /// </summary>
        /// <param name="call"></param>
        protected void AddCall(IHttpCall call)
        {
            httpWorker.Add(call);
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
