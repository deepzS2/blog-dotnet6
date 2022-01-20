using Blog.Contexts;
using Blog.DTOs;
using Blog.Models;
using Blog.Repositories;
using Blog.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
  [ApiController]
  [Route("users")]
  public class UsersController : ControllerBase
  {
    private DatabaseContextNew _database;
    private UserRepository _repository;
    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger, DatabaseContextNew database)
    {
      _logger = logger;
      _database = database;

      _repository = new UserRepository(_database);
    }

    [HttpGet]
    public async Task<IEnumerable<UserWithPostDTO>> GetAllUsersAsync()
    {
      return await _repository.GetAllUsersAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserWithPostDTO>> GetUserByIdAsync(int id)
    {
      var blogUser = await _repository.FindOneAsync(id);

      if (blogUser is null) return NotFound();

      return blogUser;
    }


  }
}