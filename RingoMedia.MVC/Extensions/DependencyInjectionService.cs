using RingoMedia.MVC.Implementaion;
using RingoMedia.MVC.Interface;

namespace RingoMedia.MVC.Extensions;

public static class DependencyInjectionService
{
    public static IHostApplicationBuilder AddDependencyInjection(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ILogs, Logs>();
        builder.Services.AddSingleton<IFileUploadService, FileUploadService>();
        builder.Services.AddSingleton<IEmailSender, EmailSender>();


        return builder;
    }
}