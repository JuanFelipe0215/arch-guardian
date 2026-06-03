namespace ArchGuardian.Application.Ports;


public interface ICachePort
{
    
    Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
    
    // Guarda un valor en cache con tiempo de expiracion
    Task SetAsync<T>(string key, T value, TimeSpan ttl ,CancellationToken ct = default);
}