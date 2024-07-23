using Hangfire;

namespace RingoMedia.MVC.Extensions;

public static class HangFireService
{
    public static IHostApplicationBuilder AddHandFire(this IHostApplicationBuilder builder, string? connectionString)
    {
        builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
        builder.Services.AddHangfireServer();
        builder.Services.AddSingleton<HangfireExceptionFilter>();
        return builder;
    }

    public static void UseHangFire(IServiceProvider serviceProvider)
    {
        GlobalJobFilters.Filters.Add(serviceProvider.GetRequiredService<HangfireExceptionFilter>());
    }
}
