using ArchGuardian.Application.DTOs;

namespace ArchGuardian.Application.Ports;



public interface IGitHubPort
{
    

    Task<CodeDiffDto> GetPullRequestDiffAsync(
        string owner,
        string repo,
        int prNumber,
        CancellationToken ct = default
    );
    
    

    Task PostReviewCommentAsync(
        string owner,
        string repo,
        int prNumber, 
        string commitId,
        string filePath,
        int line,
        string body,
        CancellationToken ct = default
    );
    
    Task<string> GetArchitectureRulesAsync(
        string owner,
        string repo,
        CancellationToken ct = default
    );
}