using CodePulse.API.Data.EFs;
using CodePulse.API.Data.Infrastrucutre.Interfaces;

namespace CodePulse.API.Data.Infrastrucutre;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> SaveChangesAsync()
    {
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
}