using System;
using System.IO;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SevenWest.Core.Entities;
using SevenWest.Core.Services;

namespace SevenWest.Tests
{
    [TestClass]
    public class JsonDataSourceTests
    {
        [TestMethod]
        public void JsonDataSourceTests_ExampleFileProcessed()
        {
            // Arrange
            var logger = new Moq.Mock<ILogger<JsonDataSource<Person>>>();
            var sut = new JsonDataSource<Person>(logger.Object);
            
            // Act
            sut.Initialise(@".\TestData\example_data.json");

            // Assert
            sut.Data.Count.Should().Be(6);
        }

        [TestMethod]
        public void JsonDataSourceTests_MissingFile()
        {
            // Arrange
            var logger = new Moq.Mock<ILogger<JsonDataSource<Person>>>();
            var sut = new JsonDataSource<Person>(logger.Object);

            // Act
            Action act = () => sut.Initialise(@".\TestData\example_data_missing.json");

            // Assert
            act.Should().Throw<FileNotFoundException>();
        }

        [TestMethod]
        public void JsonDataSourceTests_BadFile()
        {
            // Arrange
            var logger = new Moq.Mock<ILogger<JsonDataSource<Person>>>();
            var sut = new JsonDataSource<Person>(logger.Object);

            // Act
            Action act = () => sut.Initialise(@".\TestData\example_data_bad.json");

            // Assert
            act.Should().Throw<JsonReaderException>();
        }
    }
}
