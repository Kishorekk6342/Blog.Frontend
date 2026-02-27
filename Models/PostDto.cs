namespace Blog.Frontend.Models
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
        public Guid AuthorId { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsLiked { get; set; }

        public string? ImageUrl { get; set; }
    }

    public class CreatePostDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsPublic { get; set; }   // ❌ NO DEFAULT VALUE

        public string? ImageUrl { get; set; }
    }

    public class UpdatePostDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
        public string? ImageUrl { get; set; }   // ✅ ADD THIS


    }

    public class UserSearchResultDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = "";
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } = "";
    }
}