
using ArchGuardian.Domain.ValueObjests;

namespace ArchGuardian.Domain.Entities;

public class PullRequestAnalysis
{
    private readonly List<Violation> _violations = new();
    
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string RepositoryFullName { get; private set; } = string.Empty;
    public int PullRequestNumber { get; private set; }
    public DateTime AnalyzedAt { get; private set; }
    public AnalysisStatus Status { get; private set; }
    public SupportedLanguage Language { get; private set; }
    
    public IReadOnlyList<Violation> Violations => _violations.AsReadOnly();
    
    private PullRequestAnalysis() {}

    public static PullRequestAnalysis Create(
        Guid tenantId,
        string repositoryFullName,
        int pullRequestNumber,
        SupportedLanguage language
    )
    {
        return new PullRequestAnalysis
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            RepositoryFullName = repositoryFullName,
            PullRequestNumber = pullRequestNumber,
            Language = language,
            AnalyzedAt = DateTime.UtcNow,
            Status = AnalysisStatus.Pending
        };
    }

    public void AddViolation(Violation violation)
    {
        _violations.Add(violation);
    }

    public void Complete()
    {
        Status = AnalysisStatus.Completed;
    }

    public void fail()
    {
        Status = AnalysisStatus.Failed;
    }

    public void Skip()
    {
        Status = AnalysisStatus.Skipped;
    }
}