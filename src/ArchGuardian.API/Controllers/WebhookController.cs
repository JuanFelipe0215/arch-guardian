using ArchGuardian.API.Filters;
using ArchGuardian.Application.UseCases.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Text.Json;

namespace ArchGuardian.API.Controllers;

[ApiController]
[Route("webhook")]
public sealed class WebhookController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(
        IMediator mediator,
        ILogger<WebhookController> logger
    )
    {
        _mediator = mediator;
        _logger = logger;
    }


    [HttpPost]
    [ServiceFilter<GitHubSignatureFilter>]
    [EnableRateLimiting("webhook")]
    public async Task<IActionResult> ReceiveWebhook(CancellationToken cancellationToken)
    {
        var eventType = Request.Headers["X-GitHub-Event"].ToString();

        if (eventType == "ping")
        {
            _logger.LogInformation("GitHub ping recibido. Webhook configurado correctamente.");
            return Ok(new { message = "pong" });
        }

        if (eventType != "pull_request")
        {
            _logger.LogDebug("Evento ignorado: {EventType}", eventType);
            return Ok(new { message = "event ignored"  });
        }
        
        Request.Body.Position = 0;
        using var doc = await JsonDocument.ParseAsync(Request.Body, cancellationToken: cancellationToken);
        
        var root = doc.RootElement;
        var action = root.GetProperty("action").GetString()!;

        if (action is not ("opened" or "synchronize"))
        {
            _logger.LogDebug("Acción de PR ignorada: {Action}", action);
            return Ok(new { message = "action ignored" });
        }
        
        var installationId = HttpContext.Items["InstallationId"] is long id ? id : 0L;
        var owner = root.GetProperty("repository").GetProperty("owner").GetProperty("login").GetString()!;
        var repo = root.GetProperty("repository").GetProperty("name").GetString()!;
        var prNumber = root.GetProperty("pull_request").GetProperty("number").GetInt32();
        var commitId = root.GetProperty("pull_request").GetProperty("head").GetProperty("sha").GetString()!;

        var command = new ProcessPullRequestCommand(
            InstallationId: installationId,
            Owner: owner,
            Repo: repo,
            PullRequestNumber: prNumber,
            CommitId: commitId,
            Action: action
        );
        
        _ = Task.Run(() => _mediator.Send(command, CancellationToken.None), cancellationToken);
        return Ok(new { message = "processing" });

        
    }
    
}
