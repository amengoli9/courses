// KISS — CORRETTO: pipeline esplicita, enum al posto di stringhe magiche,
// ogni step è un metodo isolato e testabile, logging delegato a ILogger.

public enum ProcessingMode
{
    Permissive,       // null, troppo lungo, validazione fallita → ""
    Strict,           // tutto → eccezione
    CaseInsensitive   // sostituzioni case-insensitive
}

public sealed record ProcessingOptions
{
    public ProcessingMode Mode { get; init; } = ProcessingMode.Permissive;
    public int? MaxLength { get; init; }
    public string? Prefix { get; init; }
    public string? Suffix { get; init; }
    public IReadOnlyDictionary<string, string>? Replacements { get; init; }
    public Func<string, bool>? CustomValidator { get; init; }
}

public class StringProcessor
{
    private readonly ProcessingOptions _options;
    private readonly ILogger _logger;

    public StringProcessor(ProcessingOptions options, ILogger logger)
    {
        _options = options;
        _logger = logger;
    }

    public string Process(string? input)
    {
        if (input is null) return HandleNull();

        var result = input;
        result = ApplyReplacements(result);
        result = ApplyAffixes(result);
        result = ApplyMaxLength(result);
        ValidateOrThrow(result);

        _logger.LogInformation("Processed: {Input} -> {Result}", input, result);
        return result;
    }

    private string HandleNull() => _options.Mode == ProcessingMode.Strict
        ? throw new ArgumentNullException("input")
        : "";

    private string ApplyReplacements(string s)
    {
        if (_options.Replacements is null) return s;

        var comparison = _options.Mode == ProcessingMode.CaseInsensitive
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        foreach (var (key, value) in _options.Replacements)
            s = ReplaceAll(s, key, value, comparison);

        return s;
    }

    private static string ReplaceAll(string source, string oldValue, string newValue,
                                     StringComparison comparison)
    {
        var result = new StringBuilder();
        var index = 0;
        while (index < source.Length)
        {
            var match = source.IndexOf(oldValue, index, comparison);
            if (match < 0)
            {
                result.Append(source, index, source.Length - index);
                break;
            }
            result.Append(source, index, match - index);
            result.Append(newValue);
            index = match + oldValue.Length;
        }
        return result.ToString();
    }

    private string ApplyAffixes(string s) =>
        $"{_options.Prefix}{s}{_options.Suffix}";

    private string ApplyMaxLength(string s)
    {
        if (_options.MaxLength is null || s.Length <= _options.MaxLength) return s;

        return _options.Mode == ProcessingMode.Strict
            ? throw new ArgumentException("Input troppo lungo")
            : s[.._options.MaxLength.Value];
    }

    private void ValidateOrThrow(string s)
    {
        if (string.IsNullOrWhiteSpace(s) && _options.Mode == ProcessingMode.Strict)
            throw new ArgumentException("Input vuoto");

        if (_options.CustomValidator?.Invoke(s) == false &&
            _options.Mode == ProcessingMode.Strict)
            throw new ArgumentException("Validazione fallita");
    }
}
