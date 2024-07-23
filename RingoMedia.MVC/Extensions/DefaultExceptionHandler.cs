using Microsoft.AspNetCore.Diagnostics;

namespace RingoMedia.MVC.Extensions;

public class DefaultExceptionHandler(ILogger<DefaultExceptionHandler> _logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception.ToString());
        await httpContext.Response.WriteAsJsonAsync(exception.Message, cancellationToken);

        return true;
    }
}
