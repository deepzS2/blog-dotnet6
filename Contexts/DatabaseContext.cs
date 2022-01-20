using System;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Blog.Contexts
{
  public class DatabaseContextNew : DbContext
  {
    public DatabaseContextNew(DbContextOptions<DatabaseContextNew> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>();

      modelBuilder.Entity<Post>()
        .HasOne(p => p.User)
        .WithMany(u => u.Posts)
        .HasForeignKey(p => p.UserID)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<Comment>().HasOne(c => c.Author).WithMany(u => u.Comments).HasForeignKey(c => c.UserID).IsRequired().OnDelete(DeleteBehavior.Cascade);
      modelBuilder.Entity<Comment>().HasOne(c => c.Post).WithMany(p => p.Comments).HasForeignKey(c => c.PostID).IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
  }

  public class DatabaseContext : IDisposable
  {
    public MySqlConnection Connection { get; }

    public DatabaseContext(string connectionString)
    {
      Connection = new MySqlConnection(connectionString);
    }

    public void Dispose() => Connection.Dispose();
  }
}