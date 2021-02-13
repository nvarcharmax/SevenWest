using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SevenWest.Core.Entities;
using SevenWest.Core.Services;
using SevenWest.Tests.Helpers;

namespace SevenWest.Tests
{
    [TestClass]
    public class PersonQueryServiceTests
    {
        [TestMethod]
        public async Task PersonQueryServiceTests_ExampleFileProcessed()
        {
            // Arrange
            var data = FileHelper.GetAsJson<List<Person>>(@".\TestData\example_data.json");
            var dataSource = new Moq.Mock<IPersonDataSource>();
            dataSource.Setup(x => x.Get()).Returns(Task.FromResult(data));

            var logger = new Moq.Mock<ILogger<PersonQueryService>>();
            var sut = new PersonQueryService(logger.Object, dataSource.Object);

            // Act
            var getFullNamesByIdResults = await sut.GetFullNamesById(53);
            var getCommaSeparatedFirstNamesByAgeResults = await sut.GetCommaSeparatedFirstNamesByAge(23);
            var getGenderDistributionByAgeResults = await sut.GetGenderDistributionByAge();

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
