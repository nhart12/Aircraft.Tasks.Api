using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aircraft.Tasks.Internal.Private.Services;
using System;
using Aircraft.Tasks.Core.Services;
using Aircraft.Tasks.Internal.Private.Repositories;
using Aircraft.Tasks.Internal.Private.Repositories.DbModels;
using System.Collections.Generic;
using Aircraft.Tasks.Core.Contracts;
using Aircraft.Tasks.Core.Contracts.Requests;
using FluentAssertions;
using System.Linq;

namespace Aircraft.Tasks.Internal.Tests
{
    [TestClass]
    public class AircraftTaskServiceTests
    {
        private Mock<IDateTimeProvider> mockDateTimeProvider;
        private Mock<IAirCraftUtilizationRepository> mockAircraftUtilizationRepository;
        private IAirCraftTaskService airCraftTaskService;

        private List<TaskDto> sampleTasks;
        [TestInitialize]
        public void Setup()
        {
            mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(new DateTime(2018, 6, 19));
            mockDateTimeProvider.Setup(x => x.Now()).Returns(new DateTime(2018, 6, 19));

            mockAircraftUtilizationRepository = new Mock<IAirCraftUtilizationRepository>();
            mockAircraftUtilizationRepository.Setup(x => x.GetAirCraftUtilization(1)).Returns(new AirCraftUtilization
            {
                CurrentHours = 550,
                DailyHours = 0.7
            });
            mockAircraftUtilizationRepository.Setup(x => x.GetAirCraftUtilization(2)).Returns(new AirCraftUtilization
            {
                CurrentHours = 200,
                DailyHours = 1.1
            });

            airCraftTaskService = new AirCraftTaskService(mockDateTimeProvider.Object, mockAircraftUtilizationRepository.Object);

            sampleTasks = new List<TaskDto>()
            {
                new TaskDto()
                {
                    ItemNumber = 1,
                    Description = "Item 1",
                    LogDate = new DateTime(2018,4,7),
                    LogHours = null,
                    IntervalMonths = null,
                    IntervalHours = null
                },
                new TaskDto()
                {
                    ItemNumber = 2,
                    Description = "Item 2",
                    LogDate = new DateTime(2018,4,7),
                    LogHours = 100,
                    IntervalMonths = 12,
                    IntervalHours = 500
                },
                new TaskDto()
                {
                    ItemNumber = 3,
                    Description = "Item 3",
                    LogDate = new DateTime(2018,6,1),
                    LogHours = 150,
                    IntervalMonths = null,
                    IntervalHours = 400
                },
                new TaskDto()
                {
                    ItemNumber = 4,
                    Description = "Item 4",
                    LogDate = new DateTime(2018,6,1),
                    LogHours = 150,
                    IntervalMonths = 6,
                    IntervalHours = null
                },
            };
        }

        [TestMethod]
        public void Verify_AircraftNotExists_ReturnsNull()
        {
            // Arrange
            var request = new TaskDueListRequestDto()
            {
                AirCraftId = 3,
                Tasks = sampleTasks
            };

            // Act
            var result = airCraftTaskService.CalculateDueDates(request);

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void VerfifySampleTasksAircraft1CalculationsCorrect()
        {
            // Arrange
            var request = new TaskDueListRequestDto()
            {
                AirCraftId = 1,
                Tasks = sampleTasks
            };

            // Act
            var result = airCraftTaskService.CalculateDueDates(request);

            //Assert
            result.AirCraftId.Should().Be(1);
            var firstResult = result.Tasks.ElementAt(0);
            var secondResult = result.Tasks.ElementAt(1);
            var thirdResult = result.Tasks.ElementAt(2);
            var fourthResult = result.Tasks.ElementAt(3);

            firstResult.ItemNumber.Should().Be(3);
            firstResult.NextDue.Should().Be(new DateTime(2018, 6, 19));

            secondResult.ItemNumber.Should().Be(2);
            secondResult.NextDue.Should().Be(new DateTime(2018, 8, 29));

            thirdResult.ItemNumber.Should().Be(4);
            thirdResult.NextDue.Should().Be(new DateTime(2018, 12, 1));

            fourthResult.ItemNumber.Should().Be(1);
            fourthResult.NextDue.Should().BeNull();
        }

        [TestMethod]
        public void VerfifySampleTasksAircraft2CalculationsCorrect()
        {
            // Arrange
            var request = new TaskDueListRequestDto()
            {
                AirCraftId = 2,
                Tasks = sampleTasks
            };

            // Act
            var result = airCraftTaskService.CalculateDueDates(request);

            //Assert
            result.AirCraftId.Should().Be(2);
            var firstResult = result.Tasks.ElementAt(0);
            var secondResult = result.Tasks.ElementAt(1);
            var thirdResult = result.Tasks.ElementAt(2);
            var fourthResult = result.Tasks.ElementAt(3);

            firstResult.ItemNumber.Should().Be(4);
            firstResult.NextDue.Should().Be(new DateTime(2018, 12, 1));

            secondResult.ItemNumber.Should().Be(2);
            secondResult.NextDue.Should().Be(new DateTime(2019, 4, 7));

            thirdResult.ItemNumber.Should().Be(3);
            thirdResult.NextDue.Should().Be(new DateTime(2019, 5, 3));

            fourthResult.ItemNumber.Should().Be(1);
            fourthResult.NextDue.Should().BeNull();
        }
    }
}
