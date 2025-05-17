using Microsoft.AspNetCore.Mvc;

namespace RecomendationsService.Controllers;

[ApiController]
[Route("[controller]")]
public class PingController : ControllerBase
{
    private readonly ILogger<PingController> _logger;

    public PingController(ILogger<PingController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "PingA")]
    public string Get()
    {
        return "Pong";
    }
}