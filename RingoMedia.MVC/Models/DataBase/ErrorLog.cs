using System.ComponentModel.DataAnnotations;

namespace RingoMedia.MVC.Models.DataBase;

public class ErrorLog
{
    [Key]
    public int Id { get; set; }
    public required string Message { get; set; }
    public required string Function { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}
