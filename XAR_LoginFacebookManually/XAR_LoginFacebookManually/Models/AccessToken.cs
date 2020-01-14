using System;
namespace XAR_LoginFacebookManually.Models
{
    public class AccessToken
    {
        public string Token { get; set; }
        public int DataAccessExpiration { get; set; }
        public int ExpiresIn { get; set; }
    }
}
