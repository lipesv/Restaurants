using Microsoft.AspNetCore.Mvc;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecastService)
    {
        _logger = logger;
        _weatherForecastService = weatherForecastService;
    }

    [HttpPost("generate")]
    public IActionResult Generate([FromQuery] int count, [FromBody] WeatherForecastRequest request)
    {
        if (count < 0)
            return BadRequest("Number of results to be returned must be a postive value.");

        if (request.Max < request.Min)
            return BadRequest("Maximum temperature must be greater or equal Minimum temperature.");

        var forecasts = _weatherForecastService.GetForecasts(count, request.Min, request.Max);

        return Ok(forecasts);
    }
}
