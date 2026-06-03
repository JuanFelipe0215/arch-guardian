using ArchGuardian.Application.DTOs;

namespace ArchGuardian.Application.Ports;



public interface ILlmAnalyzerPort
{
    // Envio el diff del PR a la IA y recibe las violaciones que detecte
    Task<IReadOnlyList<ViolationDto>> AnalyzeAsync(
        CodeDiffDto diff,
        string? customRules = null, 
        CancellationToken ct = default
    );
}