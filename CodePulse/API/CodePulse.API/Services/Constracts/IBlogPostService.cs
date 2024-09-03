using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO.BlogPost;

namespace CodePulse.API.Services.Constracts;

public interface IBlogPostService
{
    Task<BlogPost?> AddAsync(CreateBlogPostRequestDto blogPostDto);
    Task<IEnumerable<BlogPost>> GetAllAsync();
    Task<IEnumerable<BlogPost>> GetAllUpdatedAsync();
    Task<BlogPost?> GetByIdAsyc(Guid Id);
    Task<bool> UpdateById(BlogPost blogPost);
    Task<BlogPost?> DeleteBlogPostAsync(Guid Id);
    Task<BlogPost?> GetByUrl(string url);
}