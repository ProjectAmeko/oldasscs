namespace Ameko.AssCS;

/// <summary>
/// Optional extra data in the subtitles.
/// </summary>
/// <author>Zahuczky</author>
public class AssExtradata
{
    public static int NextId { get; set; } = 0;

    public int Id { get; set; }
    public string? Key { get; set; }
    public char ValueType { get; set; }
    public string? Value { get; set; }

    public static AssExtradata Make(string data)
    {
        if (data.StartsWith("Data:"))
        {
            data = data["Data:".Length..];
        }
        else throw new AssException("Invalid extradata type.");

        var tokens = data.TrimStart().Split(',');
        var id = int.Parse(tokens[0]);
        var key = tokens[1];
        var valueType = tokens[2][0];
        var val = tokens[2][1..];

        // See if the next available ID needs to be upped
        NextId = (id != NextId) ? (Math.Max(id, NextId) == NextId ? NextId : id + 1) : id + 1;

        return new AssExtradata
        {
            Id = id,
            Key = key,
            ValueType = valueType,
            Value = val
        };
    }
    
    public override string ToString() =>
        $"Data: {Id},{Key},{ValueType}{Value}";

}