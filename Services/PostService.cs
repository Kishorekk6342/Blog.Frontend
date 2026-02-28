using Blog.Frontend.Models;
using System.Net.Http.Json;
using System.Net.Http.Headers;


namespace Blog.Frontend.Services
{
    public class PostService
    {
        private readonly HttpClient _http;
        public event Action? OnPostChanged; // 🔥 ADD THIS

        public event Action? OnUserProfileChanged;

        public void NotifyUserProfileChanged()
        {
            OnUserProfileChanged?.Invoke();
        }
        public PostService(HttpClient http)
        {
            _http = http;
        }

        // ============================
        // 🔧 HELPER (SAFE AUTH HANDLING)
        // ============================
        private HttpRequestMessage CreateRequest(
            HttpMethod method,
            string url,
            string? token = null)
        {
            var request = new HttpRequestMessage(method, url);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return request;
        }

        // ============================
        // 🌍 PUBLIC HOME FEED
        // ============================
        // ============================
        // 🌍 HOME FEED (Auth-aware)
        // ============================
        public async Task<List<PostDto>> GetHomeFeed(string? token = null)
        {
            var request = CreateRequest(
                HttpMethod.Get,
                "api/Post/feed?page=1&pageSize=20",
                token  // ✅ Pass token so backend knows who you are
            );

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return new();

            return await response.Content
                .ReadFromJsonAsync<List<PostDto>>() ?? new();
        }



        // ============================
        // ✍️
        // 
        // ============================
        public async Task<bool> CreatePost(CreatePostDto dto, string token)
        {
            var request = CreateRequest(HttpMethod.Post, "api/Post", token);
            request.Content = JsonContent.Create(dto);

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
                NotifyPostChanged(); // ✅ IMPORTANT

            return response.IsSuccessStatusCode;
        }

        // ============================
        // 👤 GET MY POSTS
        // ============================
        public async Task<List<PostDto>> GetMyPosts(string token)
        {
            var request = CreateRequest(
                HttpMethod.Get,
                "api/Post/my-posts",
                token
            );

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return new();

            return await response.Content
                .ReadFromJsonAsync<List<PostDto>>() ?? new();
        }

        // ============================
        // 👥 GET USER POSTS
        // ============================
        public async Task<List<PostDto>> GetUserPosts(
            Guid userId,
            string token)
        {
            var request = CreateRequest(
                HttpMethod.Get,
                $"api/Post/user/{userId}",
                token
            );

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return new();

            return await response.Content
                .ReadFromJsonAsync<List<PostDto>>() ?? new();
        }

        // ============================
        // ✏️ UPDATE POST
        // ============================
        public async Task<bool> UpdatePost(Guid postId, UpdatePostDto dto, string token)
        {
            var request = CreateRequest(HttpMethod.Put, $"api/Post/{postId}", token);
            request.Content = JsonContent.Create(dto);

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
                NotifyPostChanged(); // ✅ IMPORTANT

            return response.IsSuccessStatusCode;
        }
        public void NotifyPostChanged()
        {
            OnPostChanged?.Invoke();
        }

        // ============================
        // 🗑️ DELETE POST
        // ============================
        public async Task<bool> DeletePost(
            Guid postId,
            string token)
        {
            var request = CreateRequest(
                HttpMethod.Delete,
                $"api/Post/{postId}",
                token
            );

            var response = await _http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}