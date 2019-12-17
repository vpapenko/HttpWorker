using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpWorker.Interfaces
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
