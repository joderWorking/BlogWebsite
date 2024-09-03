namespace CodePulse.API.Data.Infrastrucutre.Interfaces;

public interface IUnitOfWork
{
    Task<bool> SaveChangesAsync();
}