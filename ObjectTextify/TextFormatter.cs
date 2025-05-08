using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ObjectTextify;

public static class TextFormatter //teste CI
{
    public static TextFormatterOptions Options { get; } = new();

    private static readonly HashSet<Type> SimpleTypes =
    [
        typeof(string),
        typeof(decimal), typeof(decimal?),
        typeof(DateTime), typeof(DateTime?),
        typeof(Guid), typeof(Guid?),
        typeof(TimeSpan), typeof(TimeSpan?),
        typeof(DateOnly), typeof(DateOnly?),
        typeof(TimeOnly), typeof(TimeOnly?)
    ];

    public static string Format<T>(T obj)
    {
        if (obj == null)
            return "";

        var sb = new StringBuilder();

        if (obj.GetType().GetCustomAttribute<TableAttribute>() is TableAttribute tableAttr)
            AppendTable(obj, sb, tableAttr, Options.DefaultIndent);
        else
            SerializeObject(obj, sb, Options.DefaultIndent);

        if (Options.AppendFinalNewLine)
            _ = sb.AppendLine();

        return sb.ToString();
    }

    public static async Task<string> FormatAsync<T>(T obj) => await Task.Run(() => Format(obj));

    private static void SerializeObject(object? obj, StringBuilder sb, int levelIndent)
    {
        if (obj == null)
            return;

        var type = obj.GetType();

        if (type.IsAbstract || type.IsInterface)
            return;

        var (currentIndent, indent) = ReturnIndent(type.GetCustomAttribute<IndentAttribute>(), levelIndent);
        AppendBreakLine(type.GetCustomAttribute<LineBreakAttribute>(), sb);
        AppendTitle(type.GetCustomAttribute<TitleAttribute>(), sb, indent);

        var properties = GetProperties(type, obj);

        var templateClass = type.GetCustomAttribute<TemplateAttribute>();
        if (templateClass != null)
            ProcessTemplate(properties, type, obj, sb, indent, templateClass);
        else
            AppendProperties(properties, type, obj, sb, currentIndent);
    }

    private static (int, string) ReturnIndent(IndentAttribute? indentAttr, int indent)
    {
        int newIndent = indent;
        if (indentAttr != null)
            newIndent += indentAttr.Indent;

        if (newIndent <= 0)
            newIndent = 0;

        return (newIndent, new(' ', newIndent));
    }

    private static void AppendBreakLine(LineBreakAttribute? lineBreak, StringBuilder sb)
    {
        if (lineBreak == null)
            return;

        for (int i = 0; i < lineBreak.Lines; i++)
            _ = sb.AppendLine();
    }

    private static void AppendTitle(TitleAttribute? title, StringBuilder sb, string indent)
    {
        if (title == null)
            return;

        if (sb.Length > 0)
            _ = sb.AppendLine();

        _ = title.Length > 0
            ? sb.AppendLine(indent + AlignText(title.Text, title.Length, title.Alignment, title.Format))
            : sb.AppendLine(indent + title.Text);
    }

    private static void ProcessTemplate(IEnumerable<PropertyInfo> properties, Type type, object obj, StringBuilder sb, string indent, TemplateAttribute templateClass)
    {
        string output = templateClass.Template;
        var props = properties.Select(p => p.Name).ToHashSet();

        foreach (Match match in Regex.Matches(output, @"\{([a-zA-Z0-9_.]+)\}"))
        {
            string fullTag = match.Groups[0].Value;
            string placeholder = match.Groups[1].Value;

            var (prop, value) = GetPropertyInfoByPath(obj, placeholder);

            string replacement = prop != null && IsPrimitiveLike(prop.PropertyType, value)
                ? FormatPrimitiveProperty(type, prop, value, indent)
                : value?.ToString() ?? "";

            output = output.Replace(fullTag, replacement);
        }

        if (Options.StrictMode && output.Contains('{') && output.Contains('}'))
            throw new InvalidOperationException("Template contains unprocessed placeholders.");

        sb.Append(indent + output);
    }

    private static void AppendProperties(IEnumerable<PropertyInfo> properties, Type type, object obj, StringBuilder sb, int currentIndent)
    {
        var breakAll = type.GetCustomAttribute<LineBreakAllAttribute>() != null;

        foreach (var prop in properties)
        {
            var (totalIndent, indent) = ReturnIndent(prop.GetCustomAttribute<IndentAttribute>(), currentIndent);

            AppendBreakLine(prop.GetCustomAttribute<LineBreakAttribute>(), sb);
            AppendTitle(prop.GetCustomAttribute<TitleAttribute>(), sb, indent);

            var value = prop.GetValue(obj);

            if (IsPrimitiveLike(prop.PropertyType, value))
            {
                _ = sb.Append(FormatPrimitiveProperty(type, prop, value, indent));
            }
            else if (prop.GetCustomAttribute<TableAttribute>() is TableAttribute tableAttr)
            {
                AppendTable(value, sb, tableAttr, totalIndent);
            }
            else if (value is not string && value is IEnumerable enumerable)
            {
                bool isFirst = true;

                foreach (var item in enumerable)
                {
                    if (!isFirst)
                        _ = sb.AppendLine();
                    isFirst = false;

                    SerializeObject(item, sb, totalIndent);
                }

                continue;
            }
            else
            {
                SerializeObject(value, sb, totalIndent);
            }

            if (breakAll)
                _ = sb.AppendLine();
        }
    }

    private static void AppendTable(object? value, StringBuilder sb, TableAttribute tableAttr, int currentIndent)
    {
        if (value == null)
            return;

        IEnumerable<object> items;
        Type itemType;

        if (value is not string && value is IEnumerable enumerable)
        {
            items = [.. enumerable.Cast<object>()];
            itemType = items.FirstOrDefault()?.GetType() ?? typeof(object);
        }
        else
        {
            items = [value];
            itemType = value.GetType();
        }

        var (tableIndent, indent) = ReturnIndent(itemType.GetCustomAttribute<IndentAttribute>(), currentIndent);

        var columns = GetTableColumns(itemType);
        if (columns.Count == 0)
            return;

        var widths = columns.ToDictionary(
            t => t.Path,
            t =>
            {
                int headerLen = t.Header.Length;
                int maxValueLen = items
                    .Select(i => FormatPrimitiveProperty(itemType, t.Prop, GetPropertyInfoByPath(i, t.Path).Instance, ""))
                        .DefaultIfEmpty("")
                        .Max(s => s.Length);
                return Math.Max(headerLen, maxValueLen);
            });

        sb.AppendLine(indent + string.Join(tableAttr.ColumnSeparator, columns.Select(t =>
            ColumnAlignText(t.Prop, tableIndent, t.Header, widths[t.Path], t.Align)
        )));

        if (tableAttr.IncludeDivider)
        {
            sb.AppendLine(indent + string.Join(tableAttr.DividerSpace ? ' ' : tableAttr.Divider, columns.Select(t =>
                new string(tableAttr.Divider, widths[t.Path])
            )));
        }

        foreach (var item in items)
        {
            var row = string.Join(tableAttr.ColumnSeparator, columns.Select(t =>
            {
                var (prop, value) = GetPropertyInfoByPath(item, t.Path);
                string val = FormatPrimitiveProperty(itemType, prop, value, "");
                return ColumnAlignText(prop, tableIndent, val, widths[t.Path], t.Align);
            }));

            sb.AppendLine(indent + row);
        }
    }

    private static List<(string Path, PropertyInfo Prop, string Header, TextAlign Align)> GetTableColumns(Type type, string parentPath = "", string fullPath = "")
    {
        var columns = new List<(string Path, PropertyInfo Prop, string Header, TextAlign Align)>();

        foreach (var prop in GetProperties(type, null))
        {
            string path = string.IsNullOrEmpty(parentPath) ? prop.Name : $"{parentPath}.{prop.Name}";
            string full = string.IsNullOrEmpty(fullPath) ? prop.Name : $"{fullPath}.{prop.Name}";

            if (IsPrimitiveLike(prop.PropertyType, ""))
            {
                var colAttr = prop.GetCustomAttribute<ColumnAttribute>();
                var header = ApplySpacing(type, prop, colAttr?.Text ?? prop.Name);
                columns.Add((path, prop, header, colAttr?.Align ?? TextAlign.Left));
            }
            else
            {
                if (prop.PropertyType != typeof(object) && !columns.Any(c => c.Path == path))
                    columns.AddRange(GetTableColumns(prop.PropertyType, path, full));
            }
        }

        return columns;
    }

    private static (PropertyInfo? Prop, object? Instance) GetPropertyInfoByPath(object obj, string path)
    {
        object? current = obj;
        PropertyInfo? lastProp = null;

        var parts = path.Split('.');

        foreach (var part in parts)
        {
            if (current == null)
                return (null, null);

            var prop = current.GetType().GetProperty(part);

            if (prop == null)
            {
                if (Options.StrictMode)
                    throw new InvalidOperationException($"Property '{part}' not found in {current.GetType().Name}");

                return (null, Options.InvalidFallback);
            }

            current = prop.GetValue(current);
            lastProp = prop;
        }

        if (lastProp == null)
            throw new InvalidOperationException($"Invalid path: {path}");

        return (lastProp, current);
    }

    private static bool IsPrimitiveLike(Type type, object? value)
    {
        return value == null ||
           type.IsPrimitive ||
           type.IsEnum ||
           SimpleTypes.Contains(type);
    }

    private static IEnumerable<PropertyInfo> GetProperties(Type type, object? obj)
    {
        var props = type.GetProperties().Where(w => w.GetCustomAttribute<IgnoreAttribute>() == null);

        if (Options.IgnoreNullValues && obj != null)
            props = props.Where(p => p.GetValue(obj) != null);

        return props;
    }

    private static string FormatPrimitiveProperty(Type type, PropertyInfo? prop, object? value, string indent)
    {
        if (prop == null)
            return "";

        string caption = GetCaption(prop, indent, out string adjustedIndent);
        string formattedValue = FormatValue(prop, value);
        formattedValue = ApplyFixedLength(prop, formattedValue);
        formattedValue = ApplyAfix(prop, formattedValue);
        formattedValue = ApplySpacing(type, prop, formattedValue);

        return caption + adjustedIndent + formattedValue;
    }

    private static string GetCaption(PropertyInfo prop, string indent, out string newIndent)
    {
        var attr = prop.GetCustomAttribute<CaptionAttribute>();
        newIndent = indent;
        if (attr == null)
            return "";

        newIndent = "";
        return indent + (attr.Length > 0
            ? AlignText(attr.Text, attr.Length, TextAlign.Left, attr.Format)
            : attr.Text);
    }

    private static string FormatValue(PropertyInfo prop, object? value)
    {
        if (value == null)
            return "";

        var formatAttr = prop.GetCustomAttribute<FormatAttribute>();
        if (formatAttr != null && value is IFormattable formattable)
            return formattable.ToString(formatAttr.Format, null);

        if (Options.TypeFormatters.TryGetValue(prop.PropertyType, out var formatter))
            return formatter(value);

        return value switch
        {
            DateTime dt => dt.ToString(Options.DefaultDateTimeFormat),
            DateOnly d => d.ToString(Options.DefaultDateFormat),
            TimeOnly t => t.ToString(Options.DefaultTimeFormat),
            Enum e => e.ToString(Options.DefaultEnumFormat),
            bool b => b ? Options.DefaultBoolTrueText : Options.DefaultBoolFalseText,
            _ => value.ToString() ?? string.Empty
        };
    }

    private static string ApplyAfix(PropertyInfo prop, string value)
    {
        var affix = prop.GetCustomAttribute<AffixAttribute>();
        if (affix == null)
            return value;

        return affix.Left + value + affix.Right;
    }

    private static string ApplySpacing(Type type, PropertyInfo prop, string value)
    {
        var space = prop.GetCustomAttribute<SpaceAttribute>() ?? type.GetCustomAttribute<SpaceDefaultAttribute>();

        if (space == null)
            return value;

        if (space.Position is SpacePosition.Left or SpacePosition.Both)
            value = ' ' + value;
        if (space.Position is SpacePosition.Right or SpacePosition.Both)
            value += ' ';

        return value;
    }

    private static string ApplyFixedLength(PropertyInfo prop, string value)
    {
        var fixedAttr = prop.GetCustomAttribute<FixedLengthAttribute>();
        if (fixedAttr == null)
            return value;

        return AlignText(value, fixedAttr.Length, fixedAttr.Alignment, fixedAttr.Format);
    }

    private static string ColumnAlignText(PropertyInfo? prop, int tableIndent, string text, int length, TextAlign align)
    {
        string newLine = "";

        if (prop != null)
        {
            var lineBreak = prop.GetCustomAttribute<LineBreakAttribute>();
            if (lineBreak != null)
            {
                newLine = string.Join("", Enumerable.Repeat(Environment.NewLine, lineBreak.Lines));
                var (_, indent) = ReturnIndent(prop.GetCustomAttribute<IndentAttribute>(), tableIndent);
                newLine += indent;
            }

        }

        return newLine + AlignText(text, length, align, ' ');
    }

    private static string AlignText(string text, int length, TextAlign align, char fill)
    {
        if (string.IsNullOrEmpty(text))
            text = "";

        if (text.Length > length)
            return text[..length];

        int padding = length - text.Length;
        return align switch
        {
            TextAlign.Left => text.PadRight(length, fill),
            TextAlign.Right => text.PadLeft(length, fill),
            TextAlign.Center => new string(fill, padding / 2) + text + new string(fill, (padding + 1) / 2),
            _ => text
        };
    }
}