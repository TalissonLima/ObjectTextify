namespace ObjectTextify;

public class TextFormatterOptions
{
    public int DefaultIndent { get; set; } = 0;
    public bool AppendFinalNewLine { get; set; } = false;
    public bool IgnoreNullValues { get; set; } = false;

    public bool StrictMode { get; set; } = true;
    public string InvalidFallback { get; set; } = "[INVALID]";

    public string DefaultDateFormat { get; set; } = "dd/MM/yyyy";
    public string DefaultTimeFormat { get; set; } = "HH:mm";
    public string DefaultDateTimeFormat { get; set; } = "dd/MM/yyyy HH:mm";
    public string DefaultBoolTrueText { get; set; } = "Sim";
    public string DefaultBoolFalseText { get; set; } = "Não";
    public string DefaultEnumFormat { get; set; } = "G";

    public Dictionary<Type, Func<object, string>> TypeFormatters { get; } = [];
}
