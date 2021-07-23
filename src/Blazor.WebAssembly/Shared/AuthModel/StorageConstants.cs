using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.WebAssembly.Shared.AuthModel
{
    public static class StorageConstants
    {
        public static class Local
        {
            public static string Preference = "clientPreference";

            public static string AuthToken = "authToken";
            //public static string RefreshToken = "refreshToken";
            //public static string UserImageURL = "userImageURL";
        }

        public static class Server
        {
            public static string Preference = "serverPreference";
        }
    }
}
