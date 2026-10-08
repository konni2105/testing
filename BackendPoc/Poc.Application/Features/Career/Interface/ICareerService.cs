using Poc.Application.Features.Career.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poc.Application.Features.Career.Interface
{
    public interface ICareerService
    {
        Task<CareerResponseDto> CreateAsync(CreateCareerDto dto);

        Task<CareerResponseDto?> GetByIdAsync(int id);

        Task<List<CareerResponseDto>> GetAllAsync();

        Task<CareerResponseDto?> UpdateAsync(int id, UpdateCareerDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
