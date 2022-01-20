using Blog.Contexts;
using Blog.Models;
using Blog.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories;

public class PostRepository
{
  internal DatabaseContextNew _database { get; set; }

  public PostRepository(DatabaseContextNew database)
  {
    _database = database;
  }

  public async Task<List<PostWithUserDTO>> GetAllPostsAsync()
  {
    var posts = from p in _database.Posts
                join u in _database.Users on p.UserID equals u.Id
                select p.AsDTO(u);

    return await posts.AsNoTracking().ToListAsync();
  }

  public async Task<PostWithUserDTO?> GetPostByIDAsync(int postId)
  {
    var post = await _database.Posts.Include(p => p.User).SingleOrDefaultAsync(post => post.Id == postId);

    if (post == null) return null;

    return post.AsDTO(post.User);
  }

  public async Task<PostWithUserDTO> CreatePostAsync(CreatePostDTO postDTO, int userId)
  {
    var post = new Post
    {
      Content = postDTO.Content,
      Title = postDTO.Title,
      UserID = userId
    };

    _database.Posts.Add(post);
    await _database.SaveChangesAsync();

    return await GetPostByIDAsync(post.Id);
  }

  public async Task<bool> UpdatePostAsync(int postId, UpdatePostDTO postDTO, int userId)
  {
    var post = await _database.Posts.SingleOrDefaultAsync(post => post.Id == postId);

    if (post == null || post.UserID != userId) return false;

    if (postDTO.Content != null && postDTO.Content.Trim() != String.Empty) post.Content = postDTO.Content;
    if (postDTO.Title != null && postDTO.Title.Trim() != String.Empty) post.Title = postDTO.Title;

    _database.Posts.Update(post);
    await _database.SaveChangesAsync();

    return true;
  }

  public async Task<bool> DeletePostAsync(int postId, int userId)
  {
    var post = await _database.Posts.SingleOrDefaultAsync(post => post.Id == postId);

    if (post == null || post.UserID != userId) return false;

    _database.Posts.Remove(post);
    await _database.SaveChangesAsync();

    return true;
  }
}