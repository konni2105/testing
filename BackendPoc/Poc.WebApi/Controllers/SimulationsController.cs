
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poc.Application.Common.DTOs;
using Poc.Application.Features.Simulations.DTOs;
using Poc.Application.Features.Simulations.Interfaces;

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
        var result = await _simulationService.CreateSimulationAsync(
            request,
            cancellationToken);

        var response = ApiResponseDto<SimulationResponseDto>
        .CreateSuccess(
            "Simulation created successfully.",
            result);

        return Ok(response);
    }
}
