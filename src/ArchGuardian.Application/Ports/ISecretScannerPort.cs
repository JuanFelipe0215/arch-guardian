using ArchGuardian.Application.DTOs;

namespace ArchGuardian.Application.Ports;

public interface ISecretScannerPort
{
    // permite buscar secrets harcodeados
    ScanResultDto Scan(string content);
}