using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models;

[Table("posts")]
public class Post
{
  [Column("id"), Key]
  public int Id { get; set; }

  [Column("title")]
  public string Title { get; set; } = null!;

  [Column("content")]
  public string Content { get; set; } = null!;

  [Column("created_at"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public DateTime CreatedAt { get; set; }

  [Column("updated_at"), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
  public DateTime UpdatedAt { get; set; }



  // Post belongs to one user 
  [Column("user_id"), ForeignKey("User")]
  public int UserID { get; set; }
  public User User { get; set; } = null!;

  [ForeignKey("PostID")]
  public ICollection<Comment> Comments { get; set; } = null!;
}