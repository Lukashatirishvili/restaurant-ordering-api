using Microsoft.EntityFrameworkCore;
using RestaurantOrdering.Api.Data;
using RestaurantOrdering.Api.DTOs;
using RestaurantOrdering.Api.Models;

namespace RestaurantOrdering.Api.Services.Categories;

public class CategoryService : ICategorySerivce
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<ServiceResult<List<CategoryResponseDto>>> GetAllAsync()
    {
        var categories = await _context.Categories
            .OrderBy(c => c.Name)
            .Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
            })
            .ToListAsync();

        return ServiceResult<List<CategoryResponseDto>>.Ok(categories);
    }

    public async Task<ServiceResult<CategoryResponseDto>> GetByIdAsync(int id)
    {
        var category = await _context.Categories
            .Where(c => c.Id == id)
            .Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
            })
            .FirstOrDefaultAsync();

        if (category == null)
        {
            return ServiceResult<CategoryResponseDto>.NotFound("Category not found");
        }

        return ServiceResult<CategoryResponseDto>.Ok(category);
    }

    public async Task<ServiceResult<CategoryResponseDto>> CreateAsync(CreateCategoryDto request)
    {
        var name = request.Name.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            return ServiceResult<CategoryResponseDto>.BadRequest("Category name is required");
        }
        
        var categoryExists = await _context.Categories.AnyAsync(c => c.Name == name);

        if (categoryExists)
        {
            return ServiceResult<CategoryResponseDto>.BadRequest("Category already exists");
        }

        var category = new Category
        {
            Name = name,
            Description = request.Description?.Trim()
        };
        
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var response = new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
        
        return ServiceResult<CategoryResponseDto>.Ok(response);
    }

    public async Task<ServiceResult<CategoryResponseDto>> UpdateAsync(int id, UpdateCategoryDto request)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return ServiceResult<CategoryResponseDto>.NotFound("Category not found");
        }
        
        var name = request.Name.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            return ServiceResult<CategoryResponseDto>.BadRequest("Category name is required");
        }
        
        var categoryExists = await _context.Categories.AnyAsync(c => c.Name == name && c.Id != id);

        if (categoryExists)
        {
            return ServiceResult<CategoryResponseDto>.BadRequest("Category already exists");
        }
        
        category.Name = name;
        category.Description = request.Description?.Trim();
        
        await _context.SaveChangesAsync();

        var response = new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
        
        return ServiceResult<CategoryResponseDto>.Ok(response);
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        var category = await _context.Categories
            .Include(c => c.MenuItems)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            return ServiceResult<bool>.NotFound("Category not found");
        }

        if (category.MenuItems.Any())
        {
            return ServiceResult<bool>.BadRequest("Cannot Delete Category because it has menu items");
        }
        
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        
        return ServiceResult<bool>.Ok(true);
    }
}