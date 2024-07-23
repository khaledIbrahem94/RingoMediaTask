using Hangfire.Server;
using RingoMedia.MVC.Models.DataBase;

namespace RingoMedia.MVC.Interface;

public interface IEmailSender
{
    Task<bool> SendEmailAsync(Reminder reminder, PerformContext context);
}
