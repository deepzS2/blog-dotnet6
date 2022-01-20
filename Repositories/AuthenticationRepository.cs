using Blog.Contexts;
using Blog.DTOs;
using Blog.Models;
using Blog.Services;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories
{
  public record AuthenticatedUser(User user, string token);

  public class AuthenticationRepository
  {
    private readonly int _hash = 12;

    internal DatabaseContextNew _database { get; set; }
    public readonly IJWTService _jwtService;

    public AuthenticationRepository(DatabaseContextNew database, IJWTService jwt)
    {
      _database = database;
      _jwtService = jwt;
    }

    public async Task<User?> GetCreatedUserData(int id)
    {
      var user = await _database.Users.FirstOrDefaultAsync(user => user.Id == id);

      return user;
    }

    public async Task<User?> SignUpAsync(SignUpUserDTO user)
    {
      var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, _hash);

      var createdUser = new User
      {
        Email = user.Email,
        Password = hashedPassword,
        Username = user.Username
      };

      _database.Users.Add(createdUser);
      await _database.SaveChangesAsync();

      return await GetCreatedUserData(createdUser.Id);
    }

    public async Task<AuthenticatedUser?> SignInAsync(SignInUserDTO userDTO)
    {
      var result = await _database.Users.FirstOrDefaultAsync(user => user.Email == userDTO.Email);

      if (result == null) return null;
      if (!BCrypt.Net.BCrypt.Verify(userDTO.Password, result.Password)) return null;

      var token = _jwtService.GenerateJWTToken(result.Id);

      return new AuthenticatedUser(result, token);
    }

    public async Task<User?> GetAuthenticatedUserAsync(string token)
    {
      var id = _jwtService.ValidateJwtToken(token);

      if (id != null)
      {
        var user = await _database.Users.FirstOrDefaultAsync(user => user.Id == id);

        return user;
      }
      else
      {
        return null;
      }
    }
  }
}