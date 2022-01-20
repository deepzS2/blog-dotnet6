namespace Blog.DTOs
{
  // ==================== Authentication DTOs ====================
  public record SignInUserDTO(string Email, string Password);
  public record SignUpUserDTO(string Username, string Email, string Password);


  // ==================== User DTOs ====================
  public record UpdateUserDTO(string Username, string Email, string Password);
  public record UserDTO(int Id, string Username, string Email, DateTime CreatedAt, DateTime UpdatedAt);
  public record UserWithPostDTO(int Id, string Username, string Email, DateTime CreatedAt, DateTime UpdatedAt, List<PostDTO> posts) : UserDTO(Id, Username, Email, CreatedAt, UpdatedAt);


  // ==================== Post DTOs ====================
  public record PostDTO(int Id, string Content, string Title, DateTime CreatedAt, DateTime UpdatedAt);
  public record PostWithUserDTO(int Id, string Content, string Title, DateTime CreatedAt, DateTime UpdatedAt, UserDTO? user) : PostDTO(Id, Content, Title, CreatedAt, UpdatedAt);
  public record CreatePostDTO(string Content, string Title);
  public record UpdatePostDTO(string Content, string Title);

  // ==================== Comment DTOs ====================
  public record CommentDTO(int Id, string Content, DateTime CreatedAt, DateTime UpdatedAt, UserDTO Author);
  public record CommentWithPostDTO(int Id, string Content, DateTime CreatedAt, DateTime UpdatedAt, UserDTO Author, PostDTO Post) : CommentDTO(Id, Content, CreatedAt, UpdatedAt, Author);
  public record CreateCommentDTO(string Content);
  public record UpdateCommentDTO(string Content);
}