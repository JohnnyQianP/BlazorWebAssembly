using Blazor.WebAssembly.Shared;
using Blazor.WebAssembly.Shared.AuthModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.WebAssembly.Client.Auth.Authentication
{
    public interface IAuthenticationManager
    {
        Task<Result<UserToken>> Login(TokenRequest model);

        Task<Result> Logout();

        Task<string> GetUser();
    }
}
