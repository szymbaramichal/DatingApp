using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.Api.Entities;

[Table("Photos")] //no db set, so we use this attr
public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }

    //Navigation props
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
}