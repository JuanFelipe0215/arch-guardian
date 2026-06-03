namespace ArchGuardian.Domain.Entities;

public class TenantConfiguration
{
    public Guid TenantId { get; private set; }
    public string GitHubInstallationId { get; private set; } = string.Empty;
    public string LlmProvider { get; private set; } = string.Empty;
    public string? CustomRules { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private TenantConfiguration() { }

    public static TenantConfiguration Create(
        string gitHubInstallationId,
        string llmProvider)
    {
        return new TenantConfiguration
        {
            TenantId = Guid.NewGuid(),
            GitHubInstallationId = gitHubInstallationId,
            LlmProvider = llmProvider,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateCustomRules(string? customRules)
    {
        CustomRules = customRules;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}