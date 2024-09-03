using CodePulse.API.Data.EFs;
using CodePulse.API.Data.Infrastructure.Interfaces;
using CodePulse.API.Data.Infrastrucutre;
using CodePulse.API.Models.Domain;

namespace CodePulse.API.Data.Repositories;

public interface IBlogPostRepo : IGenericReposiory<BlogPost>
{
}

public class BlogPostRepo : GenericRepository<BlogPost>, IBlogPostRepo
{
    public BlogPostRepo(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}