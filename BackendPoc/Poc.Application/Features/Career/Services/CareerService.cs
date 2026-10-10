using Microsoft.EntityFrameworkCore;
using Poc.Application.Common.Interfaces;
using Poc.Application.Features.Career.DTO;
using Poc.Application.Features.Career.Interface;
using CareerEntity = Poc.Domain.Entities.Career;
using Poc.Application.Common.Pagination;


namespace Poc.Application.Features.Career.Services
{
    public class CareerService : ICareerService
    {
        private readonly IAppDbContext _context;

        public CareerService(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<CareerResponseDto> CreateAsync(
            CreateCareerDto dto)
        {
            var career = new CareerEntity
            {
                Title = dto.Title,
                Description = dto.Description,
                MinimumSalary = dto.MinimumSalary,
                MaximumSalary = dto.MaximumSalary,
                RequiredExperience = dto.RequiredExperience,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            _context.Careers.Add(career);

            await _context.SaveChangesAsync();

            return MapToResponseDto(career);
        }

        public async Task<CareerResponseDto?> GetByIdAsync(int id)
        {
            var career = await _context.Careers
                .FirstOrDefaultAsync(c => c.Id == id);

            if (career == null)
                return null;

            return MapToResponseDto(career);
        }

        public async Task<List<CareerResponseDto>> GetAllAsync()
        {
            var careers = await _context.Careers
                .OrderBy(c => c.Title)
                .ToListAsync();

            return careers
                .Select(MapToResponseDto)
                .ToList();
        }

        public async Task<CareerResponseDto?> UpdateAsync(
            int id,
            UpdateCareerDto dto)
        {
            var career = await _context.Careers
                .FirstOrDefaultAsync(c => c.Id == id);

            if (career == null)
                return null;

            career.Title = dto.Title;
            career.Description = dto.Description;
            career.MinimumSalary = dto.MinimumSalary;
            career.MaximumSalary = dto.MaximumSalary;
            career.RequiredExperience = dto.RequiredExperience;
            career.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return MapToResponseDto(career);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var career = await _context.Careers
                .FirstOrDefaultAsync(c => c.Id == id);

            if (career == null)
                return false;

            career.IsActive = false;

            await _context.SaveChangesAsync();

            return true;
        }

        private static CareerResponseDto MapToResponseDto(
            CareerEntity career)
        {
            return new CareerResponseDto
            {
                Id = career.Id,
                Title = career.Title,
                Description = career.Description,
                MinimumSalary = career.MinimumSalary,
                MaximumSalary = career.MaximumSalary,
                RequiredExperience = career.RequiredExperience,
                IsActive = career.IsActive,
                CreatedDate = career.CreatedDate
            };
        }



        public async Task<PagedResponse<CareerResponseDto>> GetAllAsync(
            int pageNumber,
            int pageSize)
        {
            var query = _context.Careers
                .AsNoTracking()
                .OrderBy(c => c.Id);

            var totalCount = await query.CountAsync();

            var careers = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CareerResponseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    MinimumSalary = c.MinimumSalary,
                    MaximumSalary = c.MaximumSalary,
                    RequiredExperience = c.RequiredExperience,
                    IsActive = c.IsActive,
                    CreatedDate = c.CreatedDate
                })
                .ToListAsync();

            return new PagedResponse<CareerResponseDto>
            {
                Items = careers,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(
                    totalCount / (double)pageSize)
            };
        }


    }
}