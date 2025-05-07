# 📝 ObjectTextify

**ObjectTextify** é uma biblioteca C# projetada para serializar objetos em texto simples com opções de formatação personalizadas. Ela oferece diversos atributos para formatar propriedades de objetos, lidar com estruturas complexas como tabelas e objetos aninhados, e suporta recursos avançados como templates e formatação condicional.

Esta biblioteca é ideal para gerar relatórios, logs ou qualquer saída baseada em texto que exija flexibilidade na formatação dos dados dos objetos.

## 📦 Instalação

Você pode instalar **ObjectTextify** via NuGet:

```bash
dotnet add package ObjectTextify
```

## ✨ Features and Attributes

**AffixAttribute** | Adiciona um prefixo e/ou sufixo personalizados ao valor de uma propriedade.

**CaptionAttribute** | Adiciona um rótulo e formatação opcional a uma propriedade, como comprimento e separador.

**ColumnAttribute** | Especifica o texto de cabeçalho e o alinhamento das colunas de uma tabela.

**FixedLengthAttribute** | Garante que uma propriedade seja formatada para um comprimento fixo, com alinhamento e caractere de preenchimento opcionais.

**FormatAttribute** | Especifica uma string de formatação personalizada para os valores das propriedades.

**IgnoreAttribute** | Impede que uma propriedade seja serializada.

**IndentAttribute** | Adiciona indentação a uma classe ou propriedade, útil para objetos aninhados.

**LineBreakAttribute** | Adiciona quebras de linha antes de uma propriedade ou classe durante a serialização.

**SpaceAttribute** | Adiciona espaço antes ou depois do valor de uma propriedade (à esquerda, à direita ou em ambos os lados).

**TableAttribute** | Define como exibir um objeto como uma tabela, com divisores de colunas e alinhamento opcionais.

**TemplateAttribute** | Usa uma string de template para formatar a saída, com placeholders para propriedades.

**TitleAttribute** | Adiciona um título a uma classe ou propriedade, com comprimento e alinhamento opcionais.

## 💡 Exemplos

```C#
var pessoa = new Pessoa
{
    Nome = "João Silva",
    Idade = 30,
    DataNascimento = DateTime.Now
};

var textoFormatado = TextFormatter.Format(pessoa);
Console.WriteLine(textoFormatado);
```

## 🤝 Contribuindo

Se você deseja contribuir para o projeto ObjectTextify, fique à vontade para abrir um issue ou enviar um pull request.