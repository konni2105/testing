using Poc.Application.Features.Simulations.DTOs;

namespace Poc.Application.Features.Simulations.Interfaces;

public interface ISimulationService
{
    Task<SimulationResponseDto> CreateSimulationAsync(
     CreateSimulationDto request,
     int userId,
     CancellationToken cancellationToken = default);
}