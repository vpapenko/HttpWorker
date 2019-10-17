﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpWorker
{
    /// <summary>
    /// Class that represent one request.
    /// It store all data to run request and contains Task to return result.
    /// </summary>
    /// <typeparam name="T">return type</typeparam>
    public class HttpCall<T>
        : IHttpCall
    {
        readonly Func<HttpStatusCode, string, T> responseConverter;
        TaskCompletionSource<T> TaskCompletionSource = new TaskCompletionSource<T>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="responseConverter">Function to convert HTTP response to type T</param>
        public HttpCall(Func<HttpStatusCode, string, T> responseConverter)
        {
            this.responseConverter = responseConverter ?? throw new ArgumentNullException(nameof(responseConverter));
        }

        public HttpCallTypeEnum HttpType { get; set; }
        public Uri Uri { get; set; }
        public HttpContent Content { get; set; }


        public Task<T> Task { get { return TaskCompletionSource.Task; } }

        /// <summary>
        /// Function to set result of this call.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="response"></param>
        public void SetResult(HttpStatusCode statusCode, string response)
        {
            try
            {
                T result = responseConverter(statusCode, response);
                TaskCompletionSource.SetResult(result);
            }
            catch (Exception ex)
            {
                TaskCompletionSource.SetException(ex);
            }
        }
    }
}