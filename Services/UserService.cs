using System.Net.Http.Json;

namespace Blog.Frontend.Services;

public class UserService
{
    private readonly HttpClient _http;

    public UserService(HttpClient http)
    {
        _http = http;
    }

    public async Task<UserProfileDto?> GetMyProfile()
    {
        try
        {
            return await _http.GetFromJsonAsync<UserProfileDto>("api/User/profile");
        }
        catch
        {
            return null;
        }
    }

    public async Task<UserProfileDto?> GetUserProfile(Guid userId)
    {
        try
        {
            return await _http.GetFromJsonAsync<UserProfileDto>($"api/User/profile/{userId}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdateProfile(UpdateProfileDto dto)
    {
        try
        {
            var response = await _http.PutAsJsonAsync("api/User/profile", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<UserSettingsDto?> GetSettings()
    {
        try
        {
            return await _http.GetFromJsonAsync<UserSettingsDto>("api/User/settings");
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<UserSearchResultDto>> SearchUsers(string query)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<UserSearchResultDto>();

            return await _http.GetFromJsonAsync<List<UserSearchResultDto>>($"api/User/search?query={query}")
                   ?? new List<UserSearchResultDto>();
        }
        catch
        {
            return new List<UserSearchResultDto>();
        }
    }
}

// DTOs
public class UserProfileDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Bio { get; set; }
    public string? Location { get; set; }
    public string? Website { get; set; }
    public int PostCount { get; set; }
    public int FollowerCount { get; set; }
    public int FollowingCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UpdateProfileDto
{
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Bio { get; set; }
    public string? Location { get; set; }
    public string? Website { get; set; }
}

public class UserSettingsDto
{
    public string Email { get; set; } = "";
    public bool EmailNotifications { get; set; }
    public bool PostNotifications { get; set; }
    public bool CommentNotifications { get; set; }
    public bool PrivateProfile { get; set; }
}

public class UserSearchResultDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = "";
    public string? Bio { get; set; }
}