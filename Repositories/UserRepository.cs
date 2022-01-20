using Microsoft.EntityFrameworkCore;
using System.Data;

using Blog.Contexts;
using Blog.DTOs;

namespace Blog.Repositories
{
  public class UserRepository
  {
    internal DatabaseContextNew _database { get; set; }

    public UserRepository(DatabaseContextNew database)
    {
      _database = database;
    }

    public async Task<List<UserWithPostDTO>> GetAllUsersAsync()
    {
      var result = from u in _database.Users
                   select u.AsDTO(
                     from p in _database.Posts where p.UserID == u.Id select p
                   );

      return await result.AsNoTracking().ToListAsync();
    }

    public async Task<UserWithPostDTO?> FindOneAsync(int id)
    {
      var result = await _database.Users.Include(u => u.Posts).FirstOrDefaultAsync(user => user.Id == id);

      if (result == null) return null;

      return result.AsDTO(result.Posts.Select(post => post));
    }
  }
}