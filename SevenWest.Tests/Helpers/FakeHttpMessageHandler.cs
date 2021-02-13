using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SevenWest.Tests.Helpers
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private Func<HttpRequestMessage, HttpResponseMessage> _responseFunc;

        public FakeHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responseFunc)
        {
            this._responseFunc = responseFunc;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var responseTask = new TaskCompletionSource<HttpResponseMessage>();
            responseTask.SetResult(_responseFunc(request));

            return responseTask.Task;
        }
    }
}
