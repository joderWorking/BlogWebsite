using CodePulse.API.Data.Infrastrucutre.Interfaces;
using CodePulse.API.Data.Repositories;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO.BlogPost;
using CodePulse.API.Services.Constracts;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Services;

public class BlogPostService : IBlogPostService
{
    private readonly IBlogPostRepo _blogPostRepo;

    private readonly ICategoryService _categoryService;
    private readonly IUnitOfWork _unitOfWork;

    public BlogPostService(IBlogPostRepo blogPostRepo, IUnitOfWork unitOfWork, ICategoryService categoryService)
    {
        _blogPostRepo = blogPostRepo;
        _unitOfWork = unitOfWork;
        _categoryService = categoryService;
    }

    public async Task<BlogPost?> AddAsync(CreateBlogPostRequestDto request)
    {
        var blogPost = new BlogPost
        {
            Title = request.Title,
            ShortDescription = request.ShortDescription,
            Content = request.Content,
            FeaturedImageUrl = request.FeaturedImageUrl,
            UrlHandle = request.UrlHandle,
            PublishedDate = request.PublishedDate,
            Author = request.Author,
            IsVisible = request.IsVisible,
            Categories = new List<Category>()
        };
        if (request.Categories != null)
            foreach (var categoryId in request.Categories)
            {
                var categoryExist = await _categoryService.GetCategoryById(categoryId);
                if (categoryExist is not null) blogPost.Categories.Add(categoryExist);
            }

        await _blogPostRepo.AddAsync(blogPost);

        var isSucess = await _unitOfWork.SaveChangesAsync();
        if (isSucess) return blogPost;
        return null;
    }

    public async Task<BlogPost?> DeleteBlogPostAsync(Guid Id)
    {
        var blogPost = await _blogPostRepo.DeleteAsync(Id);
        if (blogPost is null) return null;
        await _unitOfWork.SaveChangesAsync();
        return blogPost;
    }

    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
        return await _blogPostRepo.GetAllAsync();
    }

    public async Task<IEnumerable<BlogPost>> GetAllUpdatedAsync()
    {
        return await _blogPostRepo.GetAllUpdatedAsync(x => x.Include(x => x.Categories), null);
    }

    public async Task<BlogPost?> GetByIdAsyc(Guid Id)
    {
        return await _blogPostRepo.GetByIdUpdatedAsync(x => x.Include(x => x.Categories), x => x.Id == Id);
    }

    public async Task<BlogPost?> GetByUrl(string url)
    {
        return await _blogPostRepo.GetByIdUpdatedAsync(x => x.Include(x => x.Categories), x => x.UrlHandle == url);
    }

    public async Task<bool> UpdateById(BlogPost blogPost)
    {
        _blogPostRepo.Update(blogPost);
        return await _unitOfWork.SaveChangesAsync();
    }
}