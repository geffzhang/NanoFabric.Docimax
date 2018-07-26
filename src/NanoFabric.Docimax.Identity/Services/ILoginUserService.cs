using NanoFabric.Docimax.Identity.Models;

namespace NanoFabric.Docimax.Identity.Services
{
    public interface ILoginUserService
    {
        bool Authenticate(string _userName, string _userPassword, out LoginUser loginUser);
    }
}
