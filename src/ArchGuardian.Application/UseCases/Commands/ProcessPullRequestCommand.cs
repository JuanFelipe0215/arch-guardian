using MediatR;

namespace ArchGuardian.Application.UseCases.Commands;

public sealed record ProcessPullRequestCommand(
    long InstallationId,
    string Owner,
    string Repo,
    int PullRequestNumber,
    string CommitId,
    string Action
) : IRequest<Unit>;