namespace DataGenerator.Models;

public class GeneratorConfig
{
    public const int MinAgeYears = 18;
    public const int MaxAgeYears = 90;
    public int PatientCount { get; set; } = 100;
    public int DelayBetweenRequestsMs { get; set; } = 100;
}