using NanoFabric.Docimax.Identity.Models;
using System.Linq;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Identity.Repositories
{
    public class LoginUserRepository : RepositoryBase<LoginUser, IdentityDbContext>, ILoginUserRepository
    {
        public LoginUserRepository(IdentityDbContext dbContext) : base(dbContext)
        {
        }

        public LoginUser Authenticate(string _userName, string _userPassword)
        {
            var entity = DbContext.LoginUsers.FirstOrDefault(p => p.UserName == _userName &&
                p.Password == _userPassword);

            return entity;
        }
    }
}
