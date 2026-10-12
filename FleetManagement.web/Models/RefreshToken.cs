using System.ComponentModel.DataAnnotations.Schema;

namespace FleetManagement.Web.Models
{
    [Table("refresh_tokens")]
    public class RefreshToken
    {
        [Column("id")]           public long Id { get; set; }
        [Column("user_id")]      public string UserId { get; set; } = string.Empty;
        [Column("token_hash")]   public string TokenHash { get; set; } = string.Empty;
        [Column("jti")]          public string Jti { get; set; } = string.Empty;
        [Column("expires_at")]   public DateTime ExpiresAt { get; set; }
        [Column("revoked_at")]   public DateTime? RevokedAt { get; set; }
        [Column("replaced_by")]  public string? ReplacedBy { get; set; }
        [Column("user_agent")]   public string? UserAgent { get; set; }
        [Column("ip_address")]   public string? IpAddress { get; set; }
        [Column("created_at")]   public DateTime CreatedAt { get; set; }
    }
}