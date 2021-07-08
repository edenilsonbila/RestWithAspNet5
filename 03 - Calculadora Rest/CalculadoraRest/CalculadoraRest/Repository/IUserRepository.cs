using CalculadoraRest.Data.VO;
using CalculadoraRest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculadoraRest.Repository
{
    public interface IUserRepository
    {
        User ValidadeCredentials(UserVO user);

        User ValidadeCredentials(string username);

        bool RevokeToken(string username);

        User RefreshUserInfo(User user);
    }
}
