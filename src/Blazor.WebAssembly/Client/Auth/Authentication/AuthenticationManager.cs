using Blazor.WebAssembly.Shared;
using Blazor.WebAssembly.Shared.AuthModel;
using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazor.WebAssembly.Shared.Util;

namespace Blazor.WebAssembly.Client.Auth.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly ILocalStorageService _localStorage;
       
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly HttpClient _httpClient;
        public AuthenticationManager(ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider, HttpClient httpClient) 
        {
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
            _httpClient = httpClient;
            
        }

        
        public async Task<Result<UserToken>> Login(TokenRequest model)
        {
            //var result = new Result<UserToken> { Data=new UserToken() { AccessToken= "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidXNlcjEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiUGVybWlzc2lvbnMuQ291bnRlci5BbGwiLCJQZXJtaXNzaW9ucy5JbmRleC5BbGwiXSwiZXhwIjoxNjI2NzEzNDI2fQ.bUkxmNTyFyzEpoGgninydjwcbkJmrfXySSPRgLCR71U", UserName="asda" } };
            var response = await _httpClient.PostAsJsonAsync("/blazorwebassembly/api/identity/token", model);
            var result = await  response.ToResult<UserToken>();
            if (!result.Succeeded)
            {
                return result;
            }
            await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, result.Data.AccessToken);
            await ((DiyBlazorStateProvider)this._authenticationStateProvider).StateChangedAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.AccessToken);
            return result;
        }

        public async Task<string> GetUser() 
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();

            if (string.IsNullOrEmpty(state.User.Identity.Name))
            {
                return "noname";
            }
            return state.User.Identity.Name;
        }

        public async Task<Result> Logout()
        {
            return await Task.FromResult(new Result());
        }

        
    }
}
