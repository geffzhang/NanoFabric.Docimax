
using NanoFabric.Docimax.Identity.Models;
using NanoFabric.Docimax.Identity.Repositories;
using System;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Identity.Services
{
    public class LoginUserService : ILoginUserService
    {
        private ILoginUserRepository loginUserRepository;

        public LoginUserService(ILoginUserRepository _loginUserRepository)
        {
            this.loginUserRepository = _loginUserRepository;
        }

        public bool Authenticate(string _userName, string _userPassword, out LoginUser loginUser)
        { 
            loginUser = loginUserRepository.Authenticate(_userName, _userPassword);
            return loginUser == null ? false : true;
        }
    }
}
