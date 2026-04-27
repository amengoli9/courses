// KISS — VIOLAZIONE: 9 parametri, stringhe magiche, 5+ responsabilità mescolate,
// side effect nascosto (Console.WriteLine), complessità ciclomatica ~18.

public string ProcessUserInput(string input, string mode, bool validate, bool log,
                               int? maxLength, string? prefix, string? suffix,
                               Dictionary<string, string>? replacements,
                               Func<string, bool>? customValidator)
{
    if (input == null)
    {
        if (mode == "strict")
            throw new ArgumentNullException(nameof(input));
        else
            return "";
    }

    var result = input;

    if (replacements != null && replacements.Any())
    {
        foreach (var kvp in replacements)
        {
            if (result.Contains(kvp.Key))
            {
                if (mode == "case-insensitive")
                {
                    var index = result.IndexOf(kvp.Key, StringComparison.OrdinalIgnoreCase);
                    while (index >= 0)
                    {
                        result = result.Remove(index, kvp.Key.Length).Insert(index, kvp.Value);
                        index = result.IndexOf(kvp.Key, index + kvp.Value.Length,
                                               StringComparison.OrdinalIgnoreCase);
                    }
                }
                else
                {
                    result = result.Replace(kvp.Key, kvp.Value);
                }
            }
        }
    }

    if (prefix != null) result = prefix + result;
    if (suffix != null) result = result + suffix;

    if (maxLength.HasValue && result.Length > maxLength.Value)
    {
        if (mode == "truncate")
            result = result.Substring(0, maxLength.Value);
        else if (mode == "strict")
            throw new ArgumentException("Input troppo lungo");
    }

    if (validate)
    {
        if (string.IsNullOrWhiteSpace(result))
        {
            if (mode == "strict") throw new ArgumentException("Input vuoto");
            return "";
        }

        if (customValidator != null && !customValidator(result))
        {
            if (mode == "strict") throw new ArgumentException("Validazione fallita");
            return "";
        }
    }

    if (log)
        Console.WriteLine($"[{DateTime.Now}] Processed: {input} -> {result}");

    return result;
}
