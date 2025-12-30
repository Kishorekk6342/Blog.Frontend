namespace Blog.Frontend.Services
{
    public class PostService
    {
        private readonly HttpClient _http;

        public PostService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> GetPublicPosts()
        {
            return await _http.GetStringAsync("api/Post/public");
        }
    }
}
