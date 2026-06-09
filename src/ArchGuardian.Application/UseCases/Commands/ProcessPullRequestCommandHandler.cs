using MediatR;
using Microsoft.Extensions.Logging;

namespace ArchGuardian.Application.UseCases.Commands;

public sealed class ProcessPullRequestCommandHandler : IRequestHandler<ProcessPullRequestCommand, Unit>
{
    private readonly ILogger<ProcessPullRequestCommandHandler> _logger;
    
    public ProcessPullRequestCommandHandler(ILogger<ProcessPullRequestCommandHandler> logger)
    {
        _logger = logger;
    }

    public Task<Unit> Handle(
        ProcessPullRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "PR #{PrNumber} en {Owner}/{Repo} recibido. Action: {Action} | InstallationId: {InstallationId}",
            request.PullRequestNumber,
            request.Owner,
            request.Repo,
            request.Action,
            request.InstallationId
        );
        
        return Task.FromResult(Unit.Value);
    }
}