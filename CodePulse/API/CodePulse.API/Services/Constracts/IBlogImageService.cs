using CodePulse.API.Models.Domain;

namespace CodePulse.API.Services.Constracts;

public interface IBlogImageService
{
    Task<BlogImage> UploadImage(IFormFile file, BlogImage blogImage);
    Task<IEnumerable<BlogImage>> GetAllAsync();
}