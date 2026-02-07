using Blog.Frontend.Models;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace Blog.Frontend.Services
{
    public class PostService
    {
        private readonly HttpClient _http;

        public PostService(HttpClient http)
        {
            _http = http;
        }

        // 🌍 PUBLIC HOME FEED
        public async Task<List<PostDto>> GetHomeFeed()
        {
            // 🔥 Clear auth for public feed
            _http.DefaultRequestHeaders.Authorization = null;
            var response = await _http.GetAsync("api/Post/feed?page=1&pageSize=20");
            if (!response.IsSuccessStatusCode)
                return new();
            return await response.Content.ReadFromJsonAsync<List<PostDto>>() ?? new();
        }

        // ✅ CREATE POST (THIS WAS MISSING!)
        public async Task<bool> CreatePost(CreatePostDto dto, string token)
        {
            try
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _http.PostAsJsonAsync("api/Post", dto);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // Get user's own posts
        public async Task<List<PostDto>> GetMyPosts(string token)
        {
            try
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _http.GetAsync("api/Post/my-posts");
                if (!response.IsSuccessStatusCode)
                    return new();

                return await response.Content.ReadFromJsonAsync<List<PostDto>>() ?? new();
            }
            catch
            {
                return new();
            }
        }

        // Get specific user's posts
        public async Task<List<PostDto>> GetUserPosts(Guid userId, string token)
        {
            try
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _http.GetAsync($"api/Post/user/{userId}");
                if (!response.IsSuccessStatusCode)
                    return new();

                return await response.Content.ReadFromJsonAsync<List<PostDto>>() ?? new();
            }
            catch
            {
                return new();
            }
        }

        // Update post
        public async Task<bool> UpdatePost(Guid postId, UpdatePostDto dto, string token)
        {
            try
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _http.PutAsJsonAsync($"api/Post/{postId}", dto);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // Delete post
        public async Task<bool> DeletePost(Guid postId, string token)
        {
            try
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _http.DeleteAsync($"api/Post/{postId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}