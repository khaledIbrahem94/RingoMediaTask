using System.ComponentModel.DataAnnotations;

namespace RingoMedia.MVC.Models.DataBase;

public class EmailLog
{
    [Key]
    public int Id { get; set; }
    public required string Email { get; set; }
    public bool Sent { get; set; }
    public DateTime SendingDate { get; set; }
    public string? HangfireJobId { get; set; }
}
