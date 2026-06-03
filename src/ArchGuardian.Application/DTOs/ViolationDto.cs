using ArchGuardian.Domain.ValueObjects;

namespace ArchGuardian.Application.DTOs;
// violación detectada por la IA pues este es el objeto que viaja hasta los comentarios de github 

public record ViolationDto(
    string FilePath,
    int LineNumber,
    Severity Severity,
    string ViolationType,
    string Explanation,
    string SuggestedFix
);