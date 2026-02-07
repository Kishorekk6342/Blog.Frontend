using Microsoft.JSInterop;

namespace Blog.Frontend.Services
{
    public class AuthStateService
    {
        private const string TokenKey = "authToken";
        private readonly IJSRuntime _js;

        private string? _token;
        private bool _initialized;

        public event Action? OnChange;

        public AuthStateService(IJSRuntime js)
        {
            _js = js;
        }

        // 🔑 Load token from localStorage (on app start)
        public async Task InitializeAsync()
        {
            if (_initialized)
                return;

            _token = await _js.InvokeAsync<string?>(
                "localStorage.getItem",
                TokenKey
            );

            _initialized = true;
            NotifyStateChanged();
        }

        public bool IsLoggedIn()
        {
            return !string.IsNullOrWhiteSpace(_token);
        }

        public async Task<string?> GetToken()
        {
            if (!_initialized)
                await InitializeAsync();

            return _token;
        }

        public async Task SetToken(string token)
        {
            _token = token;

            await _js.InvokeVoidAsync(
                "localStorage.setItem",
                TokenKey,
                token
            );

            NotifyStateChanged();
        }

        public async Task Logout()
        {
            _token = null;

            await _js.InvokeVoidAsync(
                "localStorage.removeItem",
                TokenKey
            );

            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
    