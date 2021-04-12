using System;
using System.Collections.Generic;
using Braveior.BuddyRewards.API.Controllers;
using Braveior.BuddyRewards.DTO;
using Braveior.BuddyRewards.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Serilog;
using Xunit;

namespace Braveior.BuddyRewards.API.Test
{
    public class BuddyRewardControllerTest
    {

        private readonly Mock<ILogger> _mockLogger;
        private readonly Mock<IDataService> _mockDataService;
        private readonly BuddyRewardsController _apiController;

        public BuddyRewardControllerTest()
        {
            _mockLogger = new Mock<ILogger>();
            _mockDataService = new Mock<IDataService>();
            _apiController = new BuddyRewardsController(_mockDataService.Object, _mockLogger.Object);
        }

        [Fact]
        public void GetAverageRatings_when_returns_positiveresult()
        {
            // Arrange
            List<AverageRatingDTO> averageRatings = new List<AverageRatingDTO>()
              {
                new AverageRatingDTO(){ Month = 1 , Star = 4.1 },
                new AverageRatingDTO(){ Month = 2 , Star = 2 }
              };
                    List<GraphDataDTO> expectedGraphDataResponse = new List<GraphDataDTO>()
              {
                new GraphDataDTO() { XVal = "Jan", YVal = 4.1 },
                new GraphDataDTO() { XVal = "Feb", YVal = 2 }
              };
            _mockDataService.Setup(x => x.GetAverageRatings("603a347c714addbd3322fc79")).Returns(averageRatings);
            // Act
            var actionResult = _apiController.GetAverageRatings("603a347c714addbd3322fc79") as OkObjectResult;
            var graphDataResponse = actionResult.Value as List<GraphDataDTO>;
            // Assert
            _mockLogger.Verify(x => x.Information("Method Start"), Times.Once);
            Assert.Equal(expectedGraphDataResponse[0].XVal, graphDataResponse[0].XVal);
            Assert.Equal(expectedGraphDataResponse[0].YVal, graphDataResponse[0].YVal);
            Assert.Equal(expectedGraphDataResponse[1].XVal, graphDataResponse[1].XVal);
            Assert.Equal(expectedGraphDataResponse[1].YVal, graphDataResponse[1].YVal);
        }

    }
}
