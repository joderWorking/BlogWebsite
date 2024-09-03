using CodePulse.API.Data.Infrastrucutre.Interfaces;
using CodePulse.API.Data.Repositories;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Models.DTO.Category;
using CodePulse.API.Services.Constracts;

namespace CodePulse.API.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepo _categoryRepo;
    private readonly IUnitOfWork _unitOfWork;


    public CategoryService(ICategoryRepo categoryRepo, IUnitOfWork unitOfWork)
    {
        _categoryRepo = categoryRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> AddAsync(CreateCategoryRequestDto req)
    {
        var category = new Category
        {
            Name = req.Name,
            UrlHandle = req.UrlHandle
        };
        await _categoryRepo.AddAsync(category);
        return await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Category?> DeleteCategoryById(Guid id)
    {
        var category = await _categoryRepo.DeleteAsync(id);
        if (category != null) await _unitOfWork.SaveChangesAsync();
        return category;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(string? query = null,
        string? sortBy = null,
        string? sortDirection = null,
        int? page = 1, int? pageSize = 100 )
    {
        if(!string.IsNullOrEmpty(query))
           return await _categoryRepo.GetAllAsync(x => x.Name.Contains(query),sortBy, sortDirection);;
        
        return await _categoryRepo.GetAllAsync(null,sortBy,sortDirection, page, pageSize);

    }

    public async Task<Category?> GetCategoryById(Guid id)
    {
        return await _categoryRepo.GetByIdAsync(id);
    }

    public async Task<CategoryDto?> UpdateCategory(Category category)
    {
        _categoryRepo.Update(category);
        var updated = await _unitOfWork.SaveChangesAsync();
        if (updated)
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
        return null;
    }
}