using ArchGuardian.Application.DTOs;
using ArchGuardian.Domain.ValueObjects;

namespace ArchGuardian.Application.Ports;

public interface ILanguageAnalyzerPort
{
    
    SupportedLanguage Language { get; }

    
    Task<IReadOnlyList<ViolationDto>> AnalyzeAsync(
        string fileContent,
        CancellationToken ct = default
    );
}