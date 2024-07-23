using Microsoft.EntityFrameworkCore;
using RingoMedia.MVC.Models.DataBase;

namespace RingoMedia.MVC.Data;

public class AppDbContext : DbContext
{
    public DbSet<Department> Departments { get; set; }
    public DbSet<Reminder> Reminders { get; set; }
    public DbSet<ErrorLog> ErrorsLog { get; set; }
    public DbSet<EmailLog> EmailsLog { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
