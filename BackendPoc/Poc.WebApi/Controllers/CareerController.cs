
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poc.Application.Common.Pagination;
using Poc.Application.Common.DTOs;
using Poc.Application.Features.Career.DTO;
using Poc.Application.Features.Career.Interface;

namespace Poc.WebApi.Controllers;

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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _careerService.GetByIdAsync(id);

        if (result == null)
            return NotFound();
        return Ok(
            ApiResponseDto<CareerResponseDto>.CreateSuccess(
                "Career retrieved successfully.",
                result));
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
      ApiResponseDto<PagedResponse<CareerResponseDto>>.CreateSuccess(
          "Careers retrieved successfully.",
          result));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(
       typeof(ApiResponseDto<CareerResponseDto>),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCareerDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);

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

        var response = ApiResponseDto<CareerResponseDto>.CreateSuccess(
      "Career created successfully.",
      result);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCareerDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);

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

        return Ok(
    ApiResponseDto<CareerResponseDto>.CreateSuccess(
        "Career updated successfully.",
        result));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _careerService.DeleteAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}
