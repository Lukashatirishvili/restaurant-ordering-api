using Microsoft.AspNetCore.Mvc;
using RestaurantOrdering.Api.DTOs;
using RestaurantOrdering.Api.Services;
using RestaurantOrdering.Api.Services.Categories;

namespace RestaurantOrdering.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : Controller
{
    private readonly ICategorySerivce _categorySerivce;

    public CategoriesController(ICategorySerivce categorySerivce)
    {
        _categorySerivce = categorySerivce;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponseDto>>> GetAll()
    {
        var result = await _categorySerivce.GetAllAsync();
        
        return Ok(result.Data);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryResponseDto>> GetById(int id)
    {
        var result = await _categorySerivce.GetByIdAsync(id);

        if (!result.Success)
        {
            return HandleServiceError(result);
        }
        
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponseDto>> Create(CreateCategoryDto request)
    {
        var result = await _categorySerivce.CreateAsync(request);

        if (!result.Success)
        {
            return HandleServiceError(result);
        }
        
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryResponseDto>> Update(int id, UpdateCategoryDto request)
    {
        var result = await _categorySerivce.UpdateAsync(id, request);

        if (!result.Success)
        {
            return HandleServiceError(result);
        }
        
        return Ok(result.Data);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _categorySerivce.DeleteAsync(id);

        if (!result.Success)
        {
            return HandleServiceError(result);
        }
        
        return NoContent();
    }

    private ActionResult HandleServiceError<T>(ServiceResult<T> result)
    {
        return result.ErrorType switch
        {
            ServiceErrorType.BadRequest => BadRequest(result.ErrorMessage),
            ServiceErrorType.NotFound => NotFound(result.ErrorMessage),
            _ => BadRequest(result.ErrorMessage)
        };
    }
}