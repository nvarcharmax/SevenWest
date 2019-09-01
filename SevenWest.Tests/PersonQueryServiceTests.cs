using System;
using System.IO;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SevenWest.Core.Entities;
using SevenWest.Core.Services;

namespace SevenWest.Tests
{
    [TestClass]
    public class PersonQueryServiceTests
    {
        [TestMethod]
        public void PersonQueryServiceTests_ExampleFileProcessed()
        {
            // Arrange
            // TODO: Currently reusing the JsonDataSource service - with proper unit tests, we should ideally generate the data here
            var dataSourcelogger = new Moq.Mock<ILogger<JsonDataSource<Person>>>();
            var dataSource = new JsonDataSource<Person>(dataSourcelogger.Object);

            var logger = new Moq.Mock<ILogger<PersonQueryService>>();
            var sut = new PersonQueryService(logger.Object, dataSource);

            dataSource.Initialise(@".\TestData\example_data.json");

            // Act
            var getFullNamesByIdResults = sut.GetFullNamesById(53);
            var getCommaSeparatedFirstNamesByAgeResults = sut.GetCommaSeparatedFirstNamesByAge(23);
            var getGenderDistributionByAgeResults = sut.GetGenderDistributionByAge();

            // Assert
            getFullNamesByIdResults.Should().Contain(x => x == "Bill Bryson");

            getCommaSeparatedFirstNamesByAgeResults.Should().Contain(x => x == "Bill,Frank");

            getGenderDistributionByAgeResults.Should().ContainInOrder(
                "Age: 23 M: 1 T: 1 Y: 0 F: 0",
                "Age: 54 M: 1 T: 0 Y: 0 F: 0",
                "Age: 66 M: 0 T: 0 Y: 2 F: 1"

            );

        }

    }
}
