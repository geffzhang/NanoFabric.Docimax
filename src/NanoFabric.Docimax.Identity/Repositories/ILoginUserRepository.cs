using NanoFabric.Docimax.Identity.Models;

namespace NanoFabric.Docimax.Identity.Repositories
{
    public interface ILoginUserRepository : IRepository<LoginUser, IdentityDbContext>
    {
        LoginUser Authenticate(string _userName, string _userPassword);
    }
}
