using CodePulse.API.Data.EFs;
using CodePulse.API.Data.Infrastructure.Interfaces;
using CodePulse.API.Data.Infrastrucutre;
using CodePulse.API.Models.Domain;

namespace CodePulse.API.Data.Repositories;

public interface ICategoryRepo : IGenericReposiory<Category>
{
}

public class CategoryRepo : GenericRepository<Category>, ICategoryRepo
{
    public CategoryRepo(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}