using System;
using XAR_LoginFacebookManually.Models;

namespace XAR_LoginFacebookManually.Services
{
    public static class FacebookServices
    {
        private const string ClientID = "490349874952920";
        private const string Scope = "user_friends";
        private const string Host = "https://graph.facebook.com/facebook";
        public const string Redirect_Url = "https://www.facebook.com/connect/login_success.html";
        public static string Url_Login = $"https://www.facebook.com/v5.0/dialog/oauth?client_id={ClientID}&response_type=token&scope={Scope}&redirect_uri={Redirect_Url}";
        public static AccessToken AccessToken { get; private set; }


        

        private static string ExtraTokenFromUrlRedirect(string url, string start, string end)
        {
            int positionStart = url.LastIndexOf(start, StringComparison.CurrentCulture)+ start.Length;
            int positionEnd = url.LastIndexOf(end, StringComparison.CurrentCulture);
            int length = positionEnd - positionStart;
            var result = url.Substring(positionStart, length);
            return result;
        }

        public static bool UpdateToken(string url)
        {
            if (AccessToken == null)
                AccessToken = new AccessToken();

            string[] keyFind = { "#access_token=", "&data_access_expiration_time=", "&expires_in=" };
            int conditionSuccess = 3;
            foreach(var key in keyFind)
            {
                if (url.Contains(key))
                    conditionSuccess--;
            }

            if (conditionSuccess == 0)
            {
                AccessToken.Token = ExtraTokenFromUrlRedirect(url, keyFind[0], keyFind[1]);
                AccessToken.DataAccessExpiration = int.Parse(ExtraTokenFromUrlRedirect(url, keyFind[1], keyFind[2]));
                AccessToken.ExpiresIn = int.Parse(ExtraTokenFromUrlRedirect(url, keyFind[1], keyFind[2]));
                return true;//get Success;
            }
            return false;
        }
    }
}
