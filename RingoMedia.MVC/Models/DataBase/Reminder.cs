using System.ComponentModel.DataAnnotations;

namespace RingoMedia.MVC.Models.DataBase;

public class Reminder
{
    [Key]
    public int Id { get; set; }
    public required string Title { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    public string? HangfireJobId { get; set; }
    public DateTime SendingDateTime { get; set; } = DateTime.Now;
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
}
