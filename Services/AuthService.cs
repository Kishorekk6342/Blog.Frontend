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

        public async Task<string?> Login(LoginDto model)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/auth/login", model);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    return result?.Token;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<string?> Signup(SignupDto model)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/auth/signup", model);

                if (response.IsSuccessStatusCode)
                {
                    return null; // success
                }

                // Read backend error message
                var error = await response.Content.ReadAsStringAsync();
                return string.IsNullOrWhiteSpace(error) ? "Signup failed" : error;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}