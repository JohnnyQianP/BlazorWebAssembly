using Blazor.WebAssembly.Shared.AuthModel;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using BootstrapBlazor.Components;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Blazor.WebAssembly.Client.Component;
//using Microsoft.Extensions.Logging;


namespace Blazor.WebAssembly.Client.Pages.Auth
{
    public partial class Login
    {
        private string message { get; set; }

        private TokenRequest _tokenModel { get; set; } = new TokenRequest();
        /// <summary>
        /// 
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            //var state = await _stateProvider.GetAuthenticationStateAsync();
            //if (state != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
            //{
            //    Navigation.NavigateTo("/blazorwebassembly");
            //}
            
        }

        private async Task SubmitAsync()//EditContext context
        {
            message = "";
            //await Task.Yield();
            var re = await _authenticationManager.Login(_tokenModel);

            if (!re.Succeeded)
            {
                message = re.Messages;
            }
        }
    }
}
