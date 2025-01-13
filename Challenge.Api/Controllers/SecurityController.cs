using Challenge.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Api.Controllers;

[ApiController]
[Route("api/security")]
public class SecurityController : ControllerBase
{
    private readonly ILogger<SecurityController> _logger;
    private readonly ISecurityService _securityService;

    public SecurityController(ILogger<SecurityController> logger,
                                ISecurityService securityService)
    {
        _logger = logger;
        _securityService = securityService;
    }

    [HttpPost("get-isin-ids")]
    public IActionResult GetIsinSecurityIdsAsync([FromBody]List<string>isinIds)
    {
        if (isinIds == null || !isinIds.Any())
            return BadRequest("List of ids is empty.");

        return Ok(_securityService.GetIsinPricesAsync(isinIds));
    }

}
