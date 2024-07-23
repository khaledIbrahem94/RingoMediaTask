using RingoMedia.MVC.Data;
using RingoMedia.MVC.Interface;
using RingoMedia.MVC.Models.DataBase;

namespace RingoMedia.MVC.Implementaion;

public class Logs(IServiceProvider _serviceProvider) : ILogs
{
    public async Task<EmailLog> AddEmailLog(EmailLog email)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var _dbcontext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (email.Id == 0)
            {
                await _dbcontext.EmailsLog.AddAsync(email);
            }
            else
            {
                _dbcontext.EmailsLog.Update(email);
            }

            await _dbcontext.SaveChangesAsync();
            return email;
        }
    }

    public async Task AddError(ErrorLog error)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            error.Date = DateTime.UtcNow;
            var _dbcontext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await _dbcontext.ErrorsLog.AddAsync(error);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
