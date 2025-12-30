using System.Net.Http.Json;
using Blog.Frontend.Models;

namespace Blog.Frontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> Register(RegisterDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/Auth/register", dto);
            return res.IsSuccessStatusCode;
        }

        public async Task<string?> Login(LoginDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/Auth/login", dto);

            if (!res.IsSuccessStatusCode)
                return null;

            var data = await res.Content.ReadFromJsonAsync<LoginResponse>();
            return data?.Token;
        }

        private class LoginResponse
        {
            public string Token { get; set; } = "";
        }
    }
}
