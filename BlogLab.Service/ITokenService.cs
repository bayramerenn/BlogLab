using BlogLab.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogLab.Service
{
    public interface ITokenService
    {
        public string CreateToken(ApplicationUserIdentity user);
    }
}