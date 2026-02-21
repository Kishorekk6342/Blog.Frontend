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

        public async Task<Guid> GetUserId()
        {
            var token = await GetToken();

            if (string.IsNullOrWhiteSpace(token))
                return Guid.Empty;

            try
            {
                var payload = token.Split('.')[1];

                var jsonBytes = Convert.FromBase64String(PadBase64(payload));
                var json = System.Text.Encoding.UTF8.GetString(jsonBytes);

                var doc = System.Text.Json.JsonDocument.Parse(json);

                // Most JWTs use "sub" as user id
                if (doc.RootElement.TryGetProperty("sub", out var sub))
                {
                    return Guid.Parse(sub.GetString()!);
                }
            }
            catch
            {
                // ignore errors
            }

            return Guid.Empty;
        }

        private string PadBase64(string base64)
        {
            int remainder = base64.Length % 4;
            if (remainder == 2)
                return base64 + "==";
            if (remainder == 3)
                return base64 + "=";
            return base64;
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
    