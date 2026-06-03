namespace ArchGuardian.Application.DTOs;

// resultado del escaneo de secrets antes de enviar el código a la IA lo hice por temas de seguridad.

public record ScanResultDto(
    bool HasSecrets,
    IReadOnlyList<string> DetectedPatterns
);
