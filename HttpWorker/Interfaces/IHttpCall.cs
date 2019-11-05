using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpWorker
{
    public interface IHttpCall<TResult>
        : IHttpCall
    {
        new Task<TResult> Task { get; }
    }

    public interface IHttpCall
    {
        Task Task { get; }
        void SetResult(HttpStatusCode statusCode, string responseString);
        HttpCallTypeEnum HttpType { get; set; }
        Uri Uri { get; set; }
        HttpContent Content { get; set; }
    }
}
