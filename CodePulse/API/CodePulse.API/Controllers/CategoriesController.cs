using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Models.DTO.Category;
using CodePulse.API.Services.Constracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto req)
    {
        var result = await _categoryService.AddAsync(req);

        if (result)
            return Ok(new { message = "Category created successfully" });
        return BadRequest(new
            { message = "Failed to create the category. Please check the provided data and try again." });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories(
        [FromQuery] string? query,
        [FromQuery] string? sortBy,
        [FromQuery] string? sortDirection,
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        var categoryList = await _categoryService.GetAllCategoriesAsync(query,sortBy,sortDirection,page,pageSize);
        var response = categoryList.Select(x => new CategoryDto
        {
            Id = x.Id,
            Name = x.Name,
            UrlHandle = x.UrlHandle
        }).ToList();

        return new JsonResult(response);
    }

    [HttpGet]
    [Route("{Id}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] string Id)
    {
        try
        {
            var categoryId = Guid.Parse(Id);
            var category = await _categoryService.GetCategoryById(categoryId);
            if (category != null)
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Item found successfully.",
                    Data = category
                });
            return NotFound(new
            {
                StatusCode = 404,
                Message = "Item not found.",
                Error = "No item with the given ID was found."
            });
        }
        catch (FormatException ex)
        {
            return BadRequest(new
            {
                StatusCode = 400,
                Message = "Invalid ID format.",
                Error = ex.Message
            });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> UpdateCategoryById([FromBody] CategoryDto entity)
    {
        var category = new Category
        {
            Id = entity.Id,
            Name = entity.Name,
            UrlHandle = entity.UrlHandle
        };
        var result = await _categoryService.UpdateCategory(category);
        if (result != null)
            return Ok(result);
        return NotFound("Category not found");
    }

    [HttpDelete]
    [Route("{Id}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> DeleteCategoryById([FromRoute] string Id)
    {
        try
        {
            var guiID = Guid.Parse(Id);
            var item = await _categoryService.DeleteCategoryById(guiID);

            if (item is null) return NotFound("Category not found");
            return Ok(item);
        }
        catch (FormatException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}