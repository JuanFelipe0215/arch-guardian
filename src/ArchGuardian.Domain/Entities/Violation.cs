using ArchGuardian.Domain.ValueObjests;

namespace ArchGuardian.Domain.Entities;

public class Violation
{
    public Guid Id { get; private set; }
    public string FilePath { get; private set; } =  string.Empty;
    public int LineNumber { get; private set; }
    public Severity Severity { get; private set; } 
    public string ViolationType { get; private set; } = string.Empty ;
    public string Explanation { get; private set; } = string.Empty ;
    public string SuggestedFix { get; private set; } = string.Empty ;
    
    private Violation() {}

    public static Violation Create(
        string filePath,
        int lineNumber,
        Severity severity,
        string violationType,
        string explanation,
        string suggestedFix
    )
    {
        return new Violation
        {
            Id = Guid.NewGuid(),
            FilePath = filePath,
            LineNumber = lineNumber,
            Severity = severity,
            ViolationType =  violationType,
            Explanation =  explanation,
            SuggestedFix = suggestedFix
        };
    }
}