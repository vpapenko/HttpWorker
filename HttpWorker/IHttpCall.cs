using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace HttpWorker
{
    public interface IHttpCall
    {
        HttpCallTypeEnum HttpType { get; set; }
        Uri Uri { get; set; }
        HttpContent Content { get; set; }
        void SetResult(HttpStatusCode statusCode, string responseString);
    }
}
