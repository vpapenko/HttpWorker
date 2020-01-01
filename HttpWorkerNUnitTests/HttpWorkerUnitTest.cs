using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HttpWorker;
using HttpWorkerNUnitTests.Common;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace HttpWorkerNUnitTests
{
    public class HttpWorkerUnitTest
    {
        [Test]
        public async Task HttpWorkerTestSimpleRun()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://test").Respond("application/string", "10");
            var client = mockHttp.ToHttpClient();
            var worker = new Worker(client);

            var call = new HttpCall<int>(Converter.IntConverterStatic)
            {
                HttpType = HttpCallTypeEnum.Get,
                Uri = new Uri("http://test")
            };

            var result = await worker.AddCall(call);
            Assert.AreEqual(10, result);
        }

        [Test]
        public void HttpWorkerTestMultipleRun()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://test/10").Respond("application/string", "10");
            mockHttp.When("http://test/20").Respond("application/string", "20");
            var client = mockHttp.ToHttpClient();
            var worker = new Worker(client);

            Assert.AreEqual(false, worker.Working);
            Run(10, worker).RunAndForget();
            Run(20, worker).RunAndForget();
            Assert.AreEqual(true, worker.Working);
            Thread.Sleep(1000);
            Assert.AreEqual(false, worker.Working);
        }

        [Test]
        public void HttpWorkerTestCountOfUnprocessedHttpCalls()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://test/10").Respond("application/string", "10");
            mockHttp.When("http://test/20").Respond("application/string", "20");
            var client = mockHttp.ToHttpClient();
            var worker = new Worker(client);

            Assert.AreEqual(0, worker.CountOfUnprocessedHttpCalls);
            Run(10, worker).RunAndForget();
            Run(20, worker).RunAndForget();
            Assert.AreEqual(2, worker.CountOfUnprocessedHttpCalls);
            Thread.Sleep(1000);
            Assert.AreEqual(0, worker.CountOfUnprocessedHttpCalls);
        }

        [Test]
        public void HttpWorkerTestLongOperationInProcess()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://test/10").Respond("application/string", "10");
            mockHttp.When("http://test/20").Respond("application/string", "20");
            var client = mockHttp.ToHttpClient();
            var worker = new Worker(client)
            {
                LongOperationStartTime = 50
            };

            Assert.AreEqual(false, worker.Working);
            Assert.AreEqual(false, worker.LongOperationInProcess);
            Run(10, worker, 1000).RunAndForget();
            Run(20, worker, 1000).RunAndForget();
            Assert.AreEqual(true, worker.Working);
            Assert.AreEqual(false, worker.LongOperationInProcess);
            Thread.Sleep(900);
            Assert.AreEqual(true, worker.LongOperationInProcess);
            Assert.AreEqual(true, worker.Working);
            Thread.Sleep(2000);
            Assert.AreEqual(false, worker.LongOperationInProcess);
            Assert.AreEqual(false, worker.Working);
        }

        [Test]
        public void HttpWorkerTestNetworkNotAvailable()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://test").Respond(GetHttpResponseMessage<HttpRequestException>);
            mockHttp.When("http://test").Respond(GetHttpResponseMessage<HttpRequestException>);
            var client = mockHttp.ToHttpClient();
            var worker = new Worker(client)
            {
                RetrySleepTime1 = 50,
                RetrySleepTime2 = 50
            };

            _networkNotAvailable = true;

            Assert.AreEqual(false, worker.Working);
            Assert.AreEqual(false, worker.NetworkNotAvailable);
            Run(worker).RunAndForget();
            Run(worker).RunAndForget();
            Thread.Sleep(100);
            Assert.AreEqual(true, worker.Working);
            Thread.Sleep(1000);
            Assert.AreEqual(true, worker.NetworkNotAvailable);
            Assert.AreEqual(true, worker.Working);
            _networkNotAvailable = false;
            Thread.Sleep(1000);
            Assert.AreEqual(false, worker.NetworkNotAvailable);
            Assert.AreEqual(false, worker.Working);
        }

        [Test]
        public void HttpWorkerTestNetworkNotAvailableCustomException1()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://test").Respond(GetHttpResponseMessage<CustomException>);
            var client = mockHttp.ToHttpClient();
            var worker = new Worker(client)
            {
                RetrySleepTime1 = 50,
                RetrySleepTime2 = 50
            };
            Exception exception = null;
            _networkNotAvailable = true;

            Assert.AreEqual(false, worker.Working);
            Assert.AreEqual(false, worker.NetworkNotAvailable);
            try
            {
                Run(worker).Wait();
            }
            catch(Exception ex)
            {
                exception = ex;
            }

            Assert.AreEqual(false, worker.Working);
            Assert.AreEqual(false, worker.NetworkNotAvailable);
            Assert.IsNotNull(exception);
            Assert.AreEqual(typeof(AggregateException), exception.GetType());
            Assert.AreEqual(1, ((AggregateException)exception).InnerExceptions.Count);
            Assert.AreEqual(typeof(CustomException), ((AggregateException)exception).InnerExceptions[0].GetType());

            exception = null;
            worker.RetryOnException.Add(typeof(Exception));
            Task.Run(() => { 
                Thread.Sleep(1000); 
                _networkNotAvailable = false; });
            Task.Run(() => {
                Thread.Sleep(500);
                Assert.AreEqual(true, worker.Working);
                Assert.AreEqual(true, worker.NetworkNotAvailable);
            });

            try
            {
                Run(worker).Wait();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.AreEqual(false, worker.Working);
            Assert.AreEqual(false, worker.NetworkNotAvailable);
            Assert.IsNull(exception);


            mockHttp.When("http://test1").Respond(GetHttpResponseMessageAggregate<Exception>);
            _networkNotAvailable = true;
            exception = null;
            worker.RetryOnException.Add(typeof(Exception));
            Task.Run(() => {
                Thread.Sleep(1000);
                _networkNotAvailable = false;
            });
            Task.Run(() => {
                Thread.Sleep(500);
                Assert.AreEqual(true, worker.Working);
                Assert.AreEqual(true, worker.NetworkNotAvailable);
            });

            try
            {
                Run(worker, "http://test1").Wait();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.AreEqual(false, worker.Working);
            Assert.AreEqual(false, worker.NetworkNotAvailable);
            Assert.IsNull(exception);
        }

        private bool _networkNotAvailable;
        public Task<HttpResponseMessage> GetHttpResponseMessage<T>() where T : Exception, new()
        {
            if (_networkNotAvailable)
            {
                throw new T();
            }
            else
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK);
                return Task.FromResult(result);
            }
        }

        public Task<HttpResponseMessage> GetHttpResponseMessageAggregate<T>() where T : Exception, new()
        {
            if (_networkNotAvailable)
            {
                throw new AggregateException(new[] { new T() });
            }
            else
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK);
                return Task.FromResult(result);
            }
        }

        public async Task Run(int id, Worker worker, int sleepTime = 100)
        {
            var converter = new Converter() { SleepTime = sleepTime };
            var call = new HttpCall<int>(converter.IntConverter)
            {
                HttpType = HttpCallTypeEnum.Get,
                Uri = new Uri($"http://test/{id}")
            };

            var result = await worker.AddCall(call);
            Assert.AreEqual(id, result);
        }

        public async Task Run(Worker worker, string uri = "http://test")
        {
            var call = new HttpCall()
            {
                HttpType = HttpCallTypeEnum.Get,
                Uri = new Uri(uri)
            };

            await worker.AddCall(call);
        }

        class CustomException : Exception
        {
        }
    }
}