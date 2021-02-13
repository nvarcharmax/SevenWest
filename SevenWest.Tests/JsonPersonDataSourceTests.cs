using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using SevenWest.Core.Entities;
using SevenWest.Core.Services;

namespace SevenWest.Tests
{
    [TestClass]
    public class JsonPersonDataSourceTests
    {
        [TestMethod]
        public async Task JsonDataSourceTests_ExampleFileProcessed()
        {
            // Arrange
            var logger = new Moq.Mock<ILogger<JsonPersonDataSource>>();
            var configuration = new Moq.Mock<IConfiguration>();
            configuration.SetupGet(c => c["PersonFileLocation"]).Returns(@".\TestData\example_data.json");
            var sut = new JsonPersonDataSource(logger.Object, configuration.Object);

            // Act
            var data = await sut.Get();

            // Assert
            data.Count.Should().Be(6);
        }

        [TestMethod]
        public async Task JsonDataSourceTests_MissingFile()
        {
            // Arrange
            var logger = new Moq.Mock<ILogger<JsonPersonDataSource>>();
            var configuration = new Moq.Mock<IConfiguration>();
            configuration.SetupGet(c => c["PersonFileLocation"]).Returns(@".\TestData\example_data_missing.json");
            var sut = new JsonPersonDataSource(logger.Object, configuration.Object);

            // Act
            Func<Task> act = async () => await sut.Get();

            // Assert
            act.Should().Throw<FileNotFoundException>();
        }

        [TestMethod]
        public async Task JsonDataSourceTests_BadFile()
        {
            // Arrange
            var logger = new Moq.Mock<ILogger<JsonPersonDataSource>>();
            var configuration = new Moq.Mock<IConfiguration>();
            configuration.SetupGet(c => c["PersonFileLocation"]).Returns(@".\TestData\example_data_bad.json");
            var sut = new JsonPersonDataSource(logger.Object, configuration.Object);

            // Act
            Func<Task> act = async () => await sut.Get();

            // Assert
            act.Should().Throw<JsonReaderException>();
        }
    }
}
