
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poc.Application.Common.DTOs;
using Poc.Application.Features.Simulations.DTOs;
using Poc.Application.Features.Simulations.Interfaces;
using System.Security.Claims;
namespace Poc.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SimulationsController : ControllerBase
{
    private readonly ISimulationService _simulationService;

    public SimulationsController(ISimulationService simulationService)
    {
        _simulationService = simulationService;
    }

    [HttpPost]
    public async Task<ActionResult<SimulationResponseDto>> CreateSimulation(
        [FromBody] CreateSimulationDto request,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null ||
            !int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var result = await _simulationService.CreateSimulationAsync(
            request,
            userId,
            cancellationToken);

        var response = ApiResponseDto<SimulationResponseDto>
        .CreateSuccess(
            "Simulation created successfully.",
            result);

        return Ok(response);
    }
}
