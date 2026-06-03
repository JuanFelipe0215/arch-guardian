namespace ArchGuardian.Application.DTOs;

// diff completo de un Pull Request.
public record CodeDiffDto(
    string Owner,
    string Repo,
    int PrNumber,
    string CommitId,
    IReadOnlyList<FileDiffDto> Files
);

// archivo específico modificado dentro del PR.
public record FileDiffDto(
    string FilePath,
    string Extension,
    IReadOnlyList<LineDiffDto> ChangedLines
);

// Representa un archivo específico modificado dentro del PR.
public record LineDiffDto(
    int LineNumber, 
    string Content
);

