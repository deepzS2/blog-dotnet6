using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models;

[Table("comments")]
public class Comment
{
  [Column("id"), Key]
  public int Id { get; set; }

  [Column("content"), Required]
  public string Content { get; set; } = null!;

  [Column("created_at"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public DateTime CreatedAt { get; set; }

  [Column("updated_at"), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
  public DateTime UpdatedAt { get; set; }



  // Post belongs to one user 
  [Column("user_id"), ForeignKey("Author")]
  public int UserID { get; set; }
  public User Author { get; set; } = null!;

  [Column("post_id"), ForeignKey("Post")]
  public int PostID { get; set; }
  public Post Post { get; set; } = null!;
}