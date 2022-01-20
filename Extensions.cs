using System.Data;
using Blog.DTOs;
using Blog.Models;

namespace Blog
{
  public static class Extensions
  {
    /// <summary>
    /// Return user as DTO
    /// </summary>
    public static UserDTO AsDTO(this User user)
    {
      return new UserDTO(user.Id, user.Username, user.Email, user.CreatedAt, user.UpdatedAt);
    }

    /// <summary>
    /// Return user with posts as DTO (don't forget to include posts on query!)
    /// </summary>
    public static UserWithPostDTO AsDTO(this User user, IEnumerable<Post> postsList)
    {
      var posts = postsList.Select(p => p.AsDTO());

      return new UserWithPostDTO(user.Id, user.Username, user.Email, user.CreatedAt, user.UpdatedAt, posts.ToList());
    }

    /// <summary>
    /// Return post as DTO
    /// </summary>
    public static PostDTO AsDTO(this Post post)
    {
      return new PostDTO(post.Id, post.Content, post.Title, post.CreatedAt, post.UpdatedAt);
    }

    /// <summary>
    /// Return post with user as DTO (don't forget to include user on query!)
    /// </summary>
    public static PostWithUserDTO AsDTO(this Post post, User user)
    {
      return new PostWithUserDTO(post.Id, post.Content, post.Title, post.CreatedAt, post.UpdatedAt, user.AsDTO());
    }

    /// <summary>
    /// Return comment with author as DTO
    /// </summary>
    public static CommentDTO AsDTO(this Comment comment, User user)
    {
      var author = user.AsDTO();

      return new CommentDTO(comment.Id, comment.Content, comment.CreatedAt, comment.UpdatedAt, author);
    }

    /// <summary>
    /// Return comment with author and post as DTO (don't forget to include both on query!)
    /// </summary>
    public static CommentWithPostDTO AsDTO(this Comment comment, User user, Post post)
    {
      var author = user.AsDTO();
      var postDto = post.AsDTO();

      return new CommentWithPostDTO(comment.Id, comment.Content, comment.CreatedAt, comment.UpdatedAt, author, postDto);
    }

    /// <summary>
    /// Extract Bearer Token from Authorization header 
    /// </summary>
    public static string ExtractToken(this HttpRequest request)
    {
      var header = request.Headers.Authorization.ToString();

      return header.Split(" ")[1];
    }
  }
}