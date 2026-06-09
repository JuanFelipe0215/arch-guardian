using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArchGuardian.API.Filters;

public sealed class GitHubSignatureFilter : IAsyncActionFilter
{
    private const string SignatureHeader = "X-Hub-Signature-256";
    private readonly ILogger<GitHubSignatureFilter> _logger;
    private readonly string _secret;

    public GitHubSignatureFilter(
        IConfiguration configuration,
        ILogger<GitHubSignatureFilter> logger)
    {
        _logger = logger;
        _secret = configuration["GITHUB_WEBHOOK_SECRET"]
            ?? throw new InvalidOperationException(
                "GITHUB_WEBHOOK_SECRET no está configurado.");
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;

        if (!request.Headers.TryGetValue(SignatureHeader, out var signatureHeader))
        {
            _logger.LogWarning("Webhook sin cabecera {Header}", SignatureHeader);
            context.Result = new UnauthorizedResult();
            return;
        }

        request.Body.Position = 0;
        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        if (!IsValidSignature(body, signatureHeader!))
        {
            _logger.LogWarning("Firma HMAC-SHA256 inválida.");
            context.Result = new UnauthorizedObjectResult(new { error = "Invalid signature." });
            return;
        }

        await next();
    }

    private bool IsValidSignature(string body, string signatureHeader)
    {
        if (!signatureHeader.StartsWith("sha256=", StringComparison.OrdinalIgnoreCase))
            return false;

        var receivedHash = signatureHeader["sha256=".Length..];
        var secretBytes = Encoding.UTF8.GetBytes(_secret);
        var bodyBytes = Encoding.UTF8.GetBytes(body);

        using var hmac = new HMACSHA256(secretBytes);
        var computedHash = hmac.ComputeHash(bodyBytes);
        var computedHashHex = Convert.ToHexString(computedHash).ToLowerInvariant();

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(computedHashHex),
            Encoding.UTF8.GetBytes(receivedHash.ToLowerInvariant()));
    }
}