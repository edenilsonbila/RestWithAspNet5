using CalculadoraRest.Data.VO;
using CalculadoraRest.Model;
using CalculadoraRest.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraRest.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MySqlContext _context;

        public UserRepository(MySqlContext context)
        {
            _context = context;
        }
        public User ValidadeCredentials(UserVO user)
        {
            var pass = ComputeHash(user.Password, new SHA256CryptoServiceProvider());

            return _context.Users.FirstOrDefault(u => (u.UserName == user.UserName) && (u.Password == pass));
        }

        public User ValidadeCredentials(string username)
        {
            return _context.Users.SingleOrDefault(u => (u.UserName == username));
        }

        public bool RevokeToken(string username)
        {
            var user = _context.Users.SingleOrDefault(u => (u.UserName == username));

            if (user == null) return false;

            user.RefreshToken = null;
            _context.SaveChanges();

            return true;
        }


        public User RefreshUserInfo(User user)
        {
            if (!_context.Users.Any(e => e.Id.Equals(user.Id))) return null;

            var result = _context.Users.SingleOrDefault(_ => _.Id.Equals(user.Id));

            if (result != null)
            {
                try
                {
                    _context.Entry(user).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }

        private string ComputeHash(string input, SHA256CryptoServiceProvider algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes);
        }


    }
}
