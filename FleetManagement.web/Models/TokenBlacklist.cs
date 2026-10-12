using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetManagement.Web.Models
{
    [Table("token_blacklist")]
    public class TokenBlacklist
    {
        [Key]
        [Column("jti")]         public string Jti { get; set; } = string.Empty;
        [Column("user_id")]     public string? UserId { get; set; }
        [Column("expires_at")]  public DateTime ExpiresAt { get; set; }
        [Column("reason")]      public string? Reason { get; set; }
        [Column("created_at")]  public DateTime CreatedAt { get; set; }
    }
}