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
  [Route("posts")]
  public class PostsController : ControllerBase
  {
    private DatabaseContextNew _database;
    private PostRepository _repository;
    private readonly ILogger<PostsController> _logger;
    private readonly IJWTService _authentication;

    public PostsController(ILogger<PostsController> logger, DatabaseContextNew database, IJWTService jwt)
    {
      _logger = logger;
      _database = database;

      _repository = new PostRepository(_database);
      _authentication = jwt;
    }

    [HttpGet]
    public async Task<IEnumerable<PostWithUserDTO>> GetAllPostsAsync()
    {
      return await _repository.GetAllPostsAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostWithUserDTO>> GetPostByIDAsync(int id)
    {
      var post = await _repository.GetPostByIDAsync(id);

      if (post is null) return NotFound();

      return post;
    }

    [HttpPost, Authorize]
    public async Task<ActionResult<PostWithUserDTO>> CreatePostAsync([FromBody] CreatePostDTO postDTO)
    {
      var userId = _authentication.ValidateJwtToken(Request.ExtractToken());

      if (userId == null) return Unauthorized();

      var createdPost = await _repository.CreatePostAsync(postDTO, (int)userId);

      return createdPost;
    }

    [HttpPut("{id}"), Authorize]
    public async Task<ActionResult> UpdatePostAsync([FromBody] UpdatePostDTO postDTO, int id)
    {
      var userId = _authentication.ValidateJwtToken(Request.ExtractToken());

      if (userId == null) return Unauthorized();

      var updatedComment = await _repository.UpdatePostAsync(id, postDTO, (int)userId);

      if (updatedComment) return NoContent();
      else return Unauthorized();
    }

    [HttpDelete("{id}"), Authorize]
    public async Task<ActionResult> DeletePostAsync(int id)
    {
      var userId = _authentication.ValidateJwtToken(Request.ExtractToken());

      if (userId == null) return Unauthorized();

      var result = await _repository.DeletePostAsync(id, (int)userId);

      if (result) return NoContent();
      else return NotFound();
    }
  }
}