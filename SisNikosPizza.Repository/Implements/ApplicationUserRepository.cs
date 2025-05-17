using SisNikosPizza.Domain.Models;
using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Repositories.Interfaces;
using SisNikosPizza.Repository.Implements;

namespace SisNikosPizza.Repositories.Implementations;

public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
{
    private readonly SisNikosPizzaBbContext _db;
    public ApplicationUserRepository(SisNikosPizzaBbContext db) : base(db)
    {
        _db = db;
    }
}
