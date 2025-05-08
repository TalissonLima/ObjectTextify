# 📝 ObjectTextify

[![NuGet](https://img.shields.io/nuget/v/ObjectTextify)](https://www.nuget.org/packages/ObjectTextify)
[![Downloads](https://img.shields.io/nuget/dt/ObjectTextify)](https://www.nuget.org/packages/ObjectTextify)
![Build](https://img.shields.io/github/actions/workflow/status/TalissonLima/ObjectTextify/build.yml)
![License](https://img.shields.io/github/license/TalissonLima/ObjectTextify)
![Stars](https://img.shields.io/github/stars/TalissonLima/ObjectTextify?style=social)

**ObjectTextify** is a C# library designed for serializing objects into plain text with custom formatting options. It provides various attributes to format properties in objects, handle complex structures like tables and nested objects, and support for advanced features such as templates and conditional formatting.

This library is ideal for generating reports, logs, or any text-based output that requires flexibility in formatting object data.

## 📚 Table of Contents
- [Installation](#-installation)
- [Features](#-features)
- [Examples](#-examples)
- [Contributing](#-contributing)

## 📦 Installation

You can install **ObjectTextify** via NuGet:

```bash
dotnet add package ObjectTextify
```

## ✨ Features

| Attribute                | Description                                                                                            |
|--------------------------|--------------------------------------------------------------------------------------------------------|
| **AffixAttribute**       | Adds custom prefix and/or suffix to a property value.                                                  |
| **CaptionAttribute**     | Adds a label and optional formatting to a property, such as length and separator.                      |
| **ColumnAttribute**      | Specifies the header text and alignment for table columns.                                             |
| **FixedLengthAttribute** | Ensures that a property is formatted to a fixed length, with optional alignment and padding character. |
| **FormatAttribute**      | Specifies a custom format string for property values.                                                  |
| **IgnoreAttribute**      | Prevents a property from being serialized.                                                             |
| **IndentAttribute**      | Adds indentation to a class or property, useful for nested objects.                                    |
| **LineBreakAttribute**   | Adds line breaks before a property or class when serializing.                                          |
| **SpaceAttribute**       | Adds space before or after a property value (left, right, or both).                                    |
| **TableAttribute**       | Defines how to display an object as a table with optional column dividers and alignment.               |
| **TemplateAttribute**    | Uses a template string to format the output, with placeholders for properties.                         |
| **TitleAttribute**       | Adds a title to a class or property, with optional length and alignment.                               |

## 💡 Examples

```C#
var person = new Person
{
    Name = "John Doe",
    Age = 30,
    BirthDate = DateTime.Now
};

var formattedText = TextFormatter.Format(person);
Console.WriteLine(formattedText);
```

## 🤝 Contributing

If you'd like to contribute to the ObjectTextify project, feel free to open an issue or submit a pull request.