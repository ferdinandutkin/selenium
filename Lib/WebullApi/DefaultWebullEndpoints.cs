namespace Lib.WebullApi
{
    public partial class WebullApiWrapper
    {
        public class DefaultWebullEndpoints : IEndpoints
        {
            private readonly string _userApi = "https://userapi.webull.com/api";
            public string Login => $"{_userApi}/passport/login/v5/account";
        }
    }
   
}
