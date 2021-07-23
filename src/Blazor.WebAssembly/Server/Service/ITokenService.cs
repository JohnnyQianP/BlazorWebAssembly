using Blazor.WebAssembly.Shared;
using Blazor.WebAssembly.Shared.AuthModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.WebAssembly.Server.Service
{
    public interface ITokenService
    {
        Task<Result<UserToken>> LoginAsync(TokenRequest model);
    }

    public class TokenService : ITokenService
    {
        private Dictionary<string, string> _localUsers;
        private Dictionary<string, List<string>> _localUserRoles;

        public TokenService() 
        {
            _localUsers = new Dictionary<string, string> { { "admin", "admin" }, { "user1", "user1" } };
            _localUserRoles = new Dictionary<string, List<string>> { { "admin", new List<string> { "Permissions.FetchData.Search", "Permissions.Chat.All", "Permissions.Counter.All", "Permissions.Index.All" } },
                { "user1", new List<string> {"Permissions.Counter.All", "Permissions.Index.All" } } };
        }
        public async Task<Result<UserToken>> LoginAsync(TokenRequest model)
        {
            return await MockValTokenResponse(model);
        }

        private async Task<Result<UserToken>> MockValTokenResponse(TokenRequest model)
        {
            if (!_localUsers.ContainsKey(model.UserName))
            {
                return await Task.FromResult(new Result<UserToken> { Succeeded = false, Messages = "用户不存在" });
            }
            if (_localUsers[model.UserName] != model.Password)
            {
                return await Task.FromResult(new Result<UserToken> { Succeeded = false, Messages = "密码不正确" });
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.UserName)
            };
            if (_localUserRoles.ContainsKey(model.UserName))
            {
                foreach (var item in _localUserRoles[model.UserName])
                {
                    claims.Add(new Claim(ClaimTypes.Role, item));
                }
            }
            var token = GenerateEncryptedToken(GetSigningCredentials(), claims);
            if (string.IsNullOrEmpty(token))
            {
                return await Task.FromResult(new Result<UserToken> { Succeeded = false, Messages = "token未取到" });
            }
            return new Result<UserToken> { Data = new UserToken { AccessToken = token, UserName = model.UserName } };
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes("qazwsxedcDH823J$5DSGS!@$g");
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            try
            {
                var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.UtcNow.AddDays(2),
               signingCredentials: signingCredentials);
                var tokenHandler = new JwtSecurityTokenHandler();
                var encryptedToken = tokenHandler.WriteToken(token);
                return encryptedToken;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
    }
}
