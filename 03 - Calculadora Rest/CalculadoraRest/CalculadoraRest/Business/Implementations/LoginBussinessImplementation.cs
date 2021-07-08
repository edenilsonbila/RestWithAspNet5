using CalculadoraRest.Configurations;
using CalculadoraRest.Data.VO;
using CalculadoraRest.Repository;
using CalculadoraRest.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CalculadoraRest.Business.Implementations
{
    public class LoginBussinessImplementation : ILoginBusiness
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private TokenConfiguration _configuration;

        private IUserRepository _repository;
        private ITokenService _tokenService;

        public LoginBussinessImplementation(TokenConfiguration configuration, IUserRepository repository, ITokenService tokenService)
        {
            _configuration = configuration;
            _repository = repository;
            _tokenService = tokenService;
        }

        public bool RevokeToken(string username)
        {
            return _repository.RevokeToken(username);
        }

        public TokenVO ValidateCredentials(UserVO userCredentials)
        {
            var user = _repository.ValidadeCredentials(userCredentials);
            if (user == null) return null;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireTime = DateTime.Now.AddDays(_configuration.DaysToExpire);

            _repository.RefreshUserInfo(user);

            DateTime createdDate = DateTime.Now;
            DateTime expirationDate = createdDate.AddMinutes(_configuration.Minutes);

            return new TokenVO(true,
                createdDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken);
        }

        public TokenVO ValidateCredentials(TokenVO token)
        {
            var accessToken = token.AccessToken;
            var refreshToken = token.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            var userName = principal.Identity.Name;

            var user = _repository.ValidadeCredentials(userName);

            if (user == null || 
                user.RefreshToken != refreshToken || 
                user.RefreshTokenExpireTime <= DateTime.Now) return null;

            accessToken = _tokenService.GenerateAccessToken(principal.Claims);
            refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            _repository.RefreshUserInfo(user);

            DateTime createdDate = DateTime.Now;
            DateTime expirationDate = createdDate.AddMinutes(_configuration.Minutes);

            return new TokenVO(true,
                createdDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken);
        }
    }
}
