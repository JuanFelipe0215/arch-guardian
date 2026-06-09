using System.Text.Json;

namespace ArchGuardian.API.Middleware;

public sealed class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantResolutionMiddleware> _logger;

    public TenantResolutionMiddleware(
        RequestDelegate next,
        ILogger<TenantResolutionMiddleware> logger
    )
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/webhook"))
        {
            await _next(context);
            return;
        }

        context.Request.EnableBuffering();
        context.Request.Body.Position = 0;

        using var doc = await JsonDocument.ParseAsync(
            context.Request.Body,
            cancellationToken: context.RequestAborted);

        context.Request.Body.Position = 0;

        if (doc.RootElement.TryGetProperty("installation", out var installation) &&
            installation.TryGetProperty("id", out var installationId))
        {
            var id = installationId.GetInt64();
            context.Items["InstallationId"] = id;
            _logger.LogDebug("InstallationId resuelto: {InstallationId}", id);
        }
        else
        {
            _logger.LogWarning("Payload sin installation.id");
        }

        await _next(context);
    }
}
