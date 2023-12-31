﻿using Microsoft.AspNetCore.Mvc;
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



                return Ok(apiResponse);
            }

            return BadRequest();
        }
    }
}
