using Blog.Contexts;
using Blog.Models;
using Blog.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories;

public class CommentRepository
{
  internal DatabaseContextNew _database { get; set; }

  public CommentRepository(DatabaseContextNew database)
  {
    _database = database;
  }

  public async Task<List<CommentWithPostDTO>> GetAllCommentsFromPostAsync()
  {
    var result = _database.Comments.Include(c => c.Author).Select(comment => comment.AsDTO(comment.Author, comment.Post));

    return await result.AsNoTracking().ToListAsync();
  }

  public async Task<List<CommentDTO>> GetCommentsByPostIDAsync(int postId)
  {
    var comments = _database.Comments.Include(c => c.Author).Where(comment => comment.PostID == postId).Select(comment => comment.AsDTO(comment.Author));

    return await comments.AsNoTracking().ToListAsync();
  }

  public async Task<CommentWithPostDTO?> GetCommentByIDAsync(int commentId)
  {
    var comment = await _database.Comments.Include(c => c.Author).Include(c => c.Post).SingleOrDefaultAsync(comment => comment.Id == commentId);

    if (comment != null) return comment.AsDTO(comment.Author, comment.Post);
    else return null;
  }

  public async Task<CommentWithPostDTO> CreateCommentAsync(CreateCommentDTO commentDTO, int userId, int postId)
  {
    var comment = new Comment
    {
      Content = commentDTO.Content,
      UserID = userId,
      PostID = postId
    };

    _database.Comments.Add(comment);
    await _database.SaveChangesAsync();

    return await GetCommentByIDAsync(comment.Id);
  }

  public async Task<bool> UpdateCommentAsync(UpdateCommentDTO commentDTO, int commentId, int userId)
  {
    var comment = await _database.Comments.SingleOrDefaultAsync(comment => comment.Id == commentId);

    if (comment == null || comment.UserID != userId) return false;

    if (commentDTO.Content != null && commentDTO.Content.Trim() != String.Empty) comment.Content = commentDTO.Content;

    _database.Comments.Update(comment);
    await _database.SaveChangesAsync();

    return true;
  }

  public async Task<bool> DeleteCommentAsync(int commentId, int userId)
  {
    var comment = await _database.Comments.SingleOrDefaultAsync(comment => comment.Id == commentId);

    if (comment == null || comment.UserID != userId) return false;

    _database.Comments.Remove(comment);
    await _database.SaveChangesAsync();

    return true;
  }
}