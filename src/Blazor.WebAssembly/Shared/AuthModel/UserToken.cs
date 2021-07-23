using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.WebAssembly.Shared.AuthModel
{
    public class UserToken
    {
        public string UserName { get; set; }
        public string AccessToken { get; set; }
    }
}
