using CodePulse.API.Data.Repositories;
using CodePulse.API.Models.Domain;
using CodePulse.API.Services.Constracts;

namespace CodePulse.API.Services;

public class BlogImageService : IBlogImageService
{
    private readonly IBlogImageRepo _blogImageRepo;

    public BlogImageService(IBlogImageRepo blogImageRepo)
    {
        _blogImageRepo = blogImageRepo;
    }

    public Task<IEnumerable<BlogImage>> GetAllAsync()
    {
        var result = _blogImageRepo.GetAllAsync();
        return result;
    }

    public async Task<BlogImage> UploadImage(IFormFile file, BlogImage blogImage)
    {
        var response = await _blogImageRepo.Upload(file, blogImage);
        return response;
    }
}