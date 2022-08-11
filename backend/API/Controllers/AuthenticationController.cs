using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeather")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost(Name = "UserLogin")]
        public IActionResult UserLogin(string User, string pass)
        {
            return Ok();
        }
    }
}