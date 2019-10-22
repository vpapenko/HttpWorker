using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpWorker
{
    internal interface IHttpCall<TResult>
        : IHttpCall
    {
        Task<TResult> Task { get; }
    }

    internal interface IHttpCall
    {
        void SetResult(HttpStatusCode statusCode, string responseString);
        HttpCallTypeEnum HttpType { get; set; }
        Uri Uri { get; set; }
        HttpContent Content { get; set; }
    }
}
