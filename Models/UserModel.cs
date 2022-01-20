using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models;

[Table("users")]
public class User
{
  [Column("id"), Key]
  public int Id { get; set; }

  [Column("username"), Required]
  public string Username { get; set; } = null!;

  [Column("email"), Required]
  public string Email { get; set; } = null!;

  [Column("password"), Required]
  public string Password { get; set; } = null!;

  [Column("created_at"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public DateTime CreatedAt { get; set; }

  [Column("updated_at"), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
  public DateTime UpdatedAt { get; set; }

  [ForeignKey("UserID")]
  public ICollection<Post> Posts { get; set; } = null!;

  [ForeignKey("UserID")]
  public ICollection<Comment> Comments { get; set; } = null!;
}