using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.WebApi.Models;

namespace OpenTelemetry.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApiDbContext _dbContext;

        public ApiController(ILogger<ApiController> logger, IHttpClientFactory clientFactory, ApiDbContext dbContext)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {

            // Generate a random number between 1 and 100
            Random random = new Random();
            int randomNumber = random.Next(1, 101);

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

                _dbContext.ApiResponses.Add(apiResponse);
                await _dbContext.SaveChangesAsync();

                return Ok(apiResponse);
            }

            return BadRequest();
        }
    }
}
