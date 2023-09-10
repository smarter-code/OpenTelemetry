using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using OpenTelemetry.WebApi.Models;

namespace OpenTelemetry.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public ApiController(ILogger<ApiController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {

            // Generate a random number between 1 and 100
            Random random = new Random();
            int randomNumber = random.Next(1, 101);
            _logger.LogInformation($"The generated random number is {randomNumber}");

            // Construct the URL dynamically
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://jsonplaceholder.typicode.com/todos/{randomNumber}");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadAsStringAsync();
                var apiResponse = new ApiResponse
                {
                    Payload = payload,
                    Date = DateTime.Now
                };

                // Save to database using Microsoft.Data.SqlClient
                string connectionString = "Data Source=apiresponses.db";
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    using (SqliteCommand command = new SqliteCommand("INSERT INTO ApiResponses (Payload, Date) VALUES (@Payload, @Date)", connection))
                    {
                        command.Parameters.Add("@Payload", SqliteType.Text).Value = payload;
                        command.Parameters.Add("@Date", SqliteType.Text).Value = DateTime.Now;
                        await command.ExecuteNonQueryAsync();
                    }
                }


                return Ok(apiResponse);
            }

            return BadRequest();
        }
    }
}
