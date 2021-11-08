using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lib.WebullApi
{
    public partial class WebullApiWrapper
    {
        private readonly HttpClient _httpClient;

        private const string _salt = "wl_app-a&b@!423^";

        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public string DeviceIdFilePath { get; set; } = @"..\..\..\deviceid.txt";

        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>
        {
            { "Accept", "*/*" },
            { "Accept-Encoding", "gzip, deflate"},
            { "platform", "web"},
            { "app", "global"},
            { "ver", "3.36.12"},
            { "User-Agent", "*" },

        };
        private readonly IEndpoints _endpoints;

        private string? _deviceId;

        public string DeviceId => _deviceId ??= GenerateDeviceId();

        private string GenerateDeviceId()
        {

            var deviceIdFileInfo = new FileInfo(Path.
                GetFullPath(Path.
                Combine(Environment.CurrentDirectory, DeviceIdFilePath)));

            if (deviceIdFileInfo.Exists)
            {
                using var reader = deviceIdFileInfo.OpenText();
                return reader.ReadToEnd();
            }

            var deviceId = Guid.NewGuid().ToString("n");

            using var fileStream = deviceIdFileInfo.Create();

            var deviceIdBytes = Encoding.UTF8.GetBytes(deviceId);

            fileStream.Write(deviceIdBytes, 0, deviceIdBytes.Length);

            return deviceId;

        }
        public WebullApiWrapper(IEndpoints endpoints)
        {
            _endpoints = endpoints;
            _httpClient = new ();
            foreach (var (name, value) in _headers.Append(new("did", DeviceId)))
            {
                _httpClient.DefaultRequestHeaders.Add(name, value);
            }
        }

        public WebullApiWrapper() : this(new DefaultWebullEndpoints())
        {
        }

        public LoginResponseData? Login(AccountType accountType, string account, string password)
            => LoginAsync(accountType, account, password).Result;

        public LoginResponseData? LoginViaEmail(string email, string password)
            => LoginViaEmailAsync(email, password).Result;

        public LoginResponseData? LoginViaPhoneNumber(string phoneNumber, string password)
            => LoginViaPhoneNumberAsync(phoneNumber, password).Result;

        async public Task<LoginResponseData?> LoginAsync(AccountType accountType, string account, string password)
        {

            ArgumentNullException.ThrowIfNull(account, nameof(account));

            ArgumentNullException.ThrowIfNull(password, nameof(password));


            var saltedPassword = _salt + password;

            var hashedPassword = Convert.
                ToHexString(MD5.Create().
                ComputeHash(Encoding.UTF8.
                GetBytes(saltedPassword))).
                ToLower();


            var data = new
            {
                account,
                AccountType = ((int)accountType).ToString(),
                DeviceId,
                Grade = 1,
                Pwd = hashedPassword
            };

            var response = await _httpClient.PostAsync(_endpoints.Login, JsonContent.Create(data, data.GetType(), null, _serializerOptions));


            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponseData>(_serializerOptions);
            }

            return null;


        }


        public Task<LoginResponseData?> LoginViaEmailAsync(string email, string password) => LoginAsync(AccountType.Email, email, password);

        public Task<LoginResponseData?> LoginViaPhoneNumberAsync(string phoneNumber, string password) => LoginAsync(AccountType.PhoneNumer, phoneNumber, password);




    }



}
