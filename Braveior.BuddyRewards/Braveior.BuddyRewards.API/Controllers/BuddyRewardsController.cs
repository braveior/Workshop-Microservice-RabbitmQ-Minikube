using Braveior.BuddyRewards.DTO;
using Braveior.BuddyRewards.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Braveior.BuddyRewards.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuddyRewardsController : ControllerBase
    {
        private readonly IDataService _service;
        private readonly ILogger _logger;

        public BuddyRewardsController(IDataService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }
        /// <summary>
        /// Endpoint to get paged data for members 
        /// </summary>
        /// <param name="tableState"></param>
        /// <returns></returns>

        [HttpPost("getmembers")]
        public IActionResult GetMembers(TableStateDTO tableState)
        {
            
            var members = _service.GetMembers(tableState);
            
            return Ok(members);
        }

        /// <summary>
        /// Endpoint to Search member by start string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("searchmember/{key}")]
        public IActionResult SearchMember(string key)
        {

            _logger.Information("Search Member service method call start");
            var members = _service.SearchMember(key);
            _logger.Information("Search Member service method call end");
            return Ok(members);
            
        }

        /// <summary>
        /// Endpoint to add Rating for a user
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        [HttpPost("addrating")]
        public async Task<IActionResult> Addrating(RatingDTO rating)
        {
            await _service.AddRating(rating);
            SendMessage(rating);
            return Ok();
        }

        private void SendMessage(RatingDTO rating)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbit-mq-service-service",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "buddyrewards.rating",
                           durable: false,
                           exclusive: false,
                           autoDelete: false,
                           arguments: null);
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(rating));

                channel.BasicPublish(exchange: "",
                           routingKey: "buddyrewards.rating",
                           basicProperties: null,
                           body: body);
            }
        }

        /// <summary>
                /// Endpoint to get paged data for the member ratings
                /// </summary>
                /// <param name="tableState"></param>
                /// <returns></returns>
        [HttpPost("getratings")]
        public IActionResult GetRatings(TableStateDTO tableState)
        {
            return Ok(_service.GetRatings(tableState));
        }

        /// <summary>
        /// Endpoint to get monthly average ratings for member
        /// </summary>
        /// <param name="ratedfor"></param>
        /// <returns></returns>
        [HttpGet("getaverageRatings/{ratedfor}")]
        public IActionResult GetAverageRatings(string ratedfor)
        {
            _logger.Information("Method Start");
            var averageRatings = _service.GetAverageRatings(ratedfor);
            var graphData = BuildGraphData(averageRatings);
            _logger.Information("Method End");
            return Ok(graphData);
        }

        /// <summary>
        /// Method to build graphdata
        /// </summary>
        /// <param name="ratings"></param>
        /// <returns></returns>
        private List<GraphDataDTO> BuildGraphData(List<AverageRatingDTO> ratings)
        {
            List<GraphDataDTO> graphData = new List<GraphDataDTO>();
            foreach (var rating in ratings)
            {
                graphData.Add(new GraphDataDTO { XVal = this.getMonthName(rating.Month), YVal = rating.Star });
            }
            return graphData;
        }

      

        /// <summary>
        /// Helper method to get Month name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string getMonthName(int id)
        {
            switch (id)
            {
                case 1:
                    return "Jan";

                case 2:
                    return "Feb";

                case 3:
                    return "Mar";

                case 4:
                    return "Apr";

                case 5:
                    return "May";

                case 6:
                    return "Jun";

                case 7:
                    return "Jul";

                case 8:
                    return "Aug";

                case 9:
                    return "Sep";

                case 10:
                    return "Oct";

                case 11:
                    return "Nov";

                case 12:
                    return "Dec";
                default:
                    return "";

            }
        }


    }

}
