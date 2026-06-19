using RestaurantOrdering.Api.DTOs;
using RestaurantOrdering.Api.Models;

namespace RestaurantOrdering.Api.Services.Categories;

public interface ICategorySerivce
{
    Task<ServiceResult<List<CategoryResponseDto>>> GetAllAsync();
    Task<ServiceResult<CategoryResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<CategoryResponseDto>> CreateAsync(CreateCategoryDto request);
    Task<ServiceResult<CategoryResponseDto>> UpdateAsync(int id, UpdateCategoryDto request);
    Task<ServiceResult<bool>> DeleteAsync(int id);
}