using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Poc.Application.Common.Responses;
using Poc.Application.Features.Career.DTO;
using Poc.Application.Features.Career.Interface;
using Poc.Application.Common.Pagination;
using Microsoft.AspNetCore.Authorization;

namespace Poc.WebApi.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CareersController : ControllerBase
    {
        private readonly ICareerService _careerService;
        private readonly IValidator<CreateCareerDto> _createValidator;
        private readonly IValidator<UpdateCareerDto> _updateValidator;

        public CareersController(
          ICareerService careerService,
          IValidator<CreateCareerDto> createValidator,
          IValidator<UpdateCareerDto> updateValidator)
        {
            _careerService = careerService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCareerDto dto)
        {
            var validationResult =
               await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                });
            }

            var result = await _careerService.CreateAsync(dto);
            var response = new ApiResponse<CareerResponseDto>(
               true,
               "Career created successfully.",
               result);

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

            return Ok(new ApiResponse<CareerResponseDto>(
                true,
                "Career retrieved successfully.",
                result));
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var result = await _careerService.GetAllAsync();

        //    return Ok(new ApiResponse<List<CareerResponseDto>>(
        //        true,
        //        "Careers retrieved successfully.",
        //        result));
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateCareerDto dto)
        {
            var validationResult =
              await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                });
            }

            var result = await _careerService.UpdateAsync(id, dto);

            if (result == null)
                return NotFound();

            return Ok(new ApiResponse<CareerResponseDto>(
        true,
        "Career updated successfully.",
        result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _careerService.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest(new
                {
                    message = "pageNumber must be at least 1, " +
                              "and pageSize must be between 1 and 100."
                });
            }

            var result = await _careerService.GetAllAsync(
                pageNumber, pageSize);


            return Ok(
                new ApiResponse<PagedResponse<CareerResponseDto>>(
                    true,
                    "Careers retrieved successfully.",
                    result
                )
            );
        }



        //[HttpGet("test-exception")]
        //public IActionResult TestException()
        //{
        //    throw new Exception("Test exception for middleware");
        //}

    }
}
