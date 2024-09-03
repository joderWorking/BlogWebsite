using CodePulse.API.Data.EFs;
using CodePulse.API.Data.Infrastructure.Interfaces;
using CodePulse.API.Data.Infrastrucutre;
using CodePulse.API.Models.Domain;

namespace CodePulse.API.Data.Repositories;

public interface IBlogImageRepo : IGenericReposiory<BlogImage>
{
    public Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
}

public class BlogImageRepo : GenericRepository<BlogImage>, IBlogImageRepo
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public BlogImageRepo(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment,
        IHttpContextAccessor httpContextAccessor) : base(dbContext)
    {
        _webHostEnvironment = webHostEnvironment;
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
    {
        // 1-Upload the Image to API/Images folder
        var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images",
            $"{blogImage.FileName}{blogImage.FileExtension}");
        using (var fileStream = new FileStream(localPath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        // 2-Update to the database
        if (_httpContextAccessor.HttpContext is not null && _httpContextAccessor.HttpContext.Request is not null)
        {
            var httpRequest = _httpContextAccessor.HttpContext.Request;
            var urlPath =
                $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";
            blogImage.Url = urlPath;
        }

        await _dbContext.BlogImages.AddAsync(blogImage);
        await _dbContext.SaveChangesAsync();
        return blogImage;
    }
}