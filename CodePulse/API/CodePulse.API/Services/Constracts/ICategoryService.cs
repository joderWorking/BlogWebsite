using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Models.DTO.Category;

namespace CodePulse.API.Services.Constracts;

public interface ICategoryService
{
    public Task<bool> AddAsync(CreateCategoryRequestDto req);
    public Task<IEnumerable<Category>> GetAllCategoriesAsync(string? query = null,  string? sortBy = null,
        string? sortDirection = null, int? page = 1, int? pageSize = 100 );
    public Task<Category?> GetCategoryById(Guid id);
    public Task<CategoryDto?> UpdateCategory(Category category);
    public Task<Category?> DeleteCategoryById(Guid id);
}