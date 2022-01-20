using Blog.Contexts;
using Blog.DTOs;
using Blog.Models;
using Blog.Repositories;
using Blog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
  [ApiController]
  [Route("comments")]
  public class CommentsController : ControllerBase
  {
    private DatabaseContextNew _database;
    private CommentRepository _repository;
    private readonly ILogger<CommentRepository> _logger;
    private readonly IJWTService _authentication;

    public CommentsController(ILogger<CommentRepository> logger, DatabaseContextNew database, IJWTService jwt)
    {
      _logger = logger;
      _database = database;

      _repository = new CommentRepository(_database);
      _authentication = jwt;
    }

    [HttpGet("post/{postId}")]
    public async Task<IEnumerable<CommentDTO>> GetAllCommentsFromPostAsync(int postId)
    {
      return await _repository.GetCommentsByPostIDAsync(postId);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDTO>> GetCommentByIDAsync(int id)
    {
      var comment = await _repository.GetCommentByIDAsync(id);

      if (comment is null) return NotFound();

      return comment;
    }

    [HttpPost("post/{postId}"), Authorize]
    public async Task<ActionResult<CommentWithPostDTO>> CreateCommentAsync([FromBody] CreateCommentDTO commentDTO, int postId)
    {
      var userId = _authentication.ValidateJwtToken(Request.ExtractToken());

      if (userId == null) return Unauthorized();

      var createdPost = await _repository.CreateCommentAsync(commentDTO, (int)userId, postId);

      return createdPost;
    }

    [HttpPut("{id}"), Authorize]
    public async Task<ActionResult> UpdateCommentAsync([FromBody] UpdateCommentDTO commentDTO, int id)
    {
      var userId = _authentication.ValidateJwtToken(Request.ExtractToken());

      if (userId == null) return Unauthorized();

      var updatedComment = await _repository.UpdateCommentAsync(commentDTO, id, (int)userId);

      if (updatedComment) return NoContent();
      else return NotFound();
    }

    [HttpDelete("{id}"), Authorize]
    public async Task<ActionResult> DeleteCommentAsync(int id)
    {
      var userId = _authentication.ValidateJwtToken(Request.ExtractToken());

      if (userId == null) return Unauthorized();

      var result = await _repository.DeleteCommentAsync(id, (int)userId);

      if (result) return NoContent();
      else return NotFound();
    }
  }
}