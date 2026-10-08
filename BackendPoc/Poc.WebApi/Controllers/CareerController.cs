using Microsoft.AspNetCore.Mvc;
using Poc.Application.Features.Career.DTO;
using Poc.Application.Features.Career.Interface;

namespace Poc.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CareersController : ControllerBase
    {
        private readonly ICareerService _careerService;

        public CareersController(ICareerService careerService)
        {
            _careerService = careerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCareerDto dto)
        {
            var result = await _careerService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _careerService.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _careerService.GetAllAsync();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateCareerDto dto)
        {
            var result = await _careerService.UpdateAsync(id, dto);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _careerService.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
