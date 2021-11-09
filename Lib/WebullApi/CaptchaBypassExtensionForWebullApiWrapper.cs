namespace Lib.WebullApi
{
    public static class CaptchaBypassExtensionForWebullApiWrapper
    {
        private static readonly string _jumpUrlFormatString = "https://invest.webull.com/auth/jump?access_token={0}&redirect_uri=https%3A%2F%2Fwww.webull.com%2Fcenter&userDomain=nauser";
        public static string GetCaptchaBypassUrlViaEmail(this WebullApiWrapper @this, string email, string password) =>
            string.Format(_jumpUrlFormatString, @this.LoginViaEmail(email, password)?.AccessToken);

        public static string GetCaptchaBypassUrlViaPhoneNumber(this WebullApiWrapper @this, string email, string password) =>
            string.Format(_jumpUrlFormatString, @this.LoginViaEmail(email, password)?.AccessToken);
    }


}
