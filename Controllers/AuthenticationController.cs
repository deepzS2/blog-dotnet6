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
  [Route("/")]
  public class AuthenticationController : ControllerBase
  {
    private DatabaseContextNew _database;
    private AuthenticationRepository _repository;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(ILogger<AuthenticationController> logger, DatabaseContextNew database, IJWTService jwt)
    {
      _logger = logger;
      _database = database;

      // var secret = configuration["Jwt:Key"];
      // _repository = new AuthenticationRepository(_database, secret != null ? secret : "default");
      _repository = new AuthenticationRepository(database, jwt);
    }

    [HttpPost("/signup")]
    public async Task<ActionResult<User?>> SignUpAsync([FromBody] SignUpUserDTO user)
    {
      var createdUser = await _repository.SignUpAsync(user);

      return createdUser;
    }

    [HttpPost("/signin")]
    public async Task<ActionResult<AuthenticatedUser>> SignInAsync([FromBody] SignInUserDTO user)
    {
      var authenticatedUser = await _repository.SignInAsync(user);

      if (authenticatedUser is null) return Unauthorized();

      return authenticatedUser;
    }

    [HttpGet("/@me"), Authorize]
    public async Task<ActionResult<User>> GetAuthenticatedUserAsync()
    {
      var headerAuth = Request.Headers.Authorization.ToString();

      // Split header (Bearer <TOKEN HERE>) and get last element
      var user = await _repository.GetAuthenticatedUserAsync(headerAuth.Split(" ")[1]);

      if (user == null) return Unauthorized();
      else return user;
    }
  }
}