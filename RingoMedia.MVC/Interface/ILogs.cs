using RingoMedia.MVC.Models.DataBase;

namespace RingoMedia.MVC.Interface;

public interface ILogs
{
    Task AddError(ErrorLog error);
    Task<EmailLog> AddEmailLog(EmailLog email);
}
