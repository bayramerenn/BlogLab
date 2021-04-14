using BlogLab.Models.Account;

namespace BlogLab.Service
{
    public interface ITokenService
    {
        public string CreateToken(ApplicationUserIdentity user);
    }
}