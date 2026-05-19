namespace Clodu.API.Models;

public class ShardLocation
{
    public int Index { get; set; }
    public string Path { get; set; } = string.Empty;
    public bool IsParity { get; set; }
    public long SizeBytes { get; set; }
    public string? NodeId { get; set; }
}