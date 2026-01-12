using Microsoft.JSInterop;

namespace Blog.Frontend.Services
{
    public class AuthStateService
    {
        private const string TokenKey = "authToken";
        private readonly IJSRuntime _js;

        public AuthStateService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SetToken(string token)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        }

        public async Task<string?> GetToken()
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
        }

        public async Task<bool> IsLoggedIn()
        {
            var token = await GetToken();
            return !string.IsNullOrWhiteSpace(token);
        }

        public async Task Logout()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        }
    }
}
