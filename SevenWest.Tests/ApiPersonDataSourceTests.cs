using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using SevenWest.Core.Entities;
using SevenWest.Core.Services;
using SevenWest.Tests.Helpers;

namespace SevenWest.Tests
{
    [TestClass]
    public class ApiPersonDataSourceTests
    {
        [TestMethod]
        public async Task ApiPersonDataSourceTests_Success()
        {
            // Arrange
            var logger = new Moq.Mock<ILogger<ApiPersonDataSource>>();
            var configuration = new Moq.Mock<IConfiguration>();
            configuration.SetupGet(c => c["PersonApi"]).Returns(@"https://google.com/").Verifiable();
            var memoryCache = new FakeMemoryCache();

            var httpClientFactory = ArrangeHttpClient(
                new HttpClientResponse()
                {
                    UrlSuffix = "https://google.com/",
                    ResponseFunc = (r) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(FileHelper.GetAsString(@".\TestData\example_data.json"))
                        }
                    )
                }
            );

            var sut = new ApiPersonDataSource(logger.Object, configuration.Object, memoryCache, httpClientFactory);

            // Act
            var data = await sut.Get();

            // Assert
            data.Count.Should().Be(6);
        }

        [TestMethod]
        public async Task ApiPersonDataSourceTests_HttpError()
        {
            var logger = new Moq.Mock<ILogger<ApiPersonDataSource>>();
            var configuration = new Moq.Mock<IConfiguration>();
            configuration.SetupGet(c => c["PersonApi"]).Returns(@"https://google.com/").Verifiable();
            var memoryCache = new FakeMemoryCache();

            var httpClientFactory = ArrangeHttpClient(
                new HttpClientResponse()
                {
                    UrlSuffix = "https://google.com/",
                    ResponseFunc = (r) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound))
                }
            );

            var sut = new ApiPersonDataSource(logger.Object, configuration.Object, memoryCache, httpClientFactory);

            // Act
            Func<Task> act = async () => await sut.Get();

            // Assert
            act.Should().Throw<HttpRequestException>();
        }

        [TestMethod]
        public async Task ApiPersonDataSourceTests_BadData()
        {
            // Arrange
            var logger = new Moq.Mock<ILogger<ApiPersonDataSource>>();
            var configuration = new Moq.Mock<IConfiguration>();
            configuration.SetupGet(c => c["PersonApi"]).Returns(@"https://google.com/").Verifiable();
            var memoryCache = new FakeMemoryCache();

            var httpClientFactory = ArrangeHttpClient(
                new HttpClientResponse()
                {
                    UrlSuffix = "https://google.com/",
                    ResponseFunc = (r) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(FileHelper.GetAsString(@".\TestData\example_data_bad.json"))
                    })
                }
            );

            var sut = new ApiPersonDataSource(logger.Object, configuration.Object, memoryCache, httpClientFactory);

            // Act
            Func<Task> act = async () => await sut.Get();

            // Assert
            act.Should().Throw<JsonReaderException>();
        }

        private IHttpClientFactory ArrangeHttpClient(params HttpClientResponse[] responses)
        {
            var responseList = responses.ToList();
            var httpClient = new HttpClient(new FakeHttpMessageHandler((r) =>
            {
                var currentResponse = responseList.FirstOrDefault();
                if (currentResponse == null)
                {
                    throw new Exception("No response defined");
                }

                if (r.RequestUri.AbsoluteUri.EndsWith(currentResponse.UrlSuffix))
                {
                    var response = currentResponse.ResponseFunc(r).Result;
                    response.RequestMessage = r;
                    return response;
                }

                throw new Exception("Mismatched url response defined");
            }), false);

            var httpClientFactory = new Moq.Mock<IHttpClientFactory>();
            httpClientFactory.Setup(x =>x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            return httpClientFactory.Object;
        }

        public class HttpClientResponse
        {
            public string UrlSuffix { get; set; }
            public Func<HttpRequestMessage, Task<HttpResponseMessage>> ResponseFunc { get; set; }
        }
    }
}
