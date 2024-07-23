using Microsoft.EntityFrameworkCore;
using RingoMedia.MVC.Data;
using RingoMedia.MVC.Extensions;
using RingoMedia.MVC.Models.Models;

namespace RingoMedia.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddExceptionHandler<DefaultExceptionHandler>();

            builder.Services.Configure<Urls>(builder.Configuration.GetSection("Urls"));

            // AddDbConnectionString
            string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            builder.AddHandFire(connectionString);
            builder.AddEmailService();
            builder.AddDependencyInjection();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
            }
            HangFireService.UseHangFire(app.Services);

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
