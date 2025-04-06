# CSScratch

**CSScratch** is a transpiler that converts C# code into [GoboScript](https://github.com/aspizu/goboscript) - a text-based language for programming [Scratch 3.0](https://scratch.mit.edu) projects. This tool allows developers to write logic-heavy Scratch projects using C# syntax and then compile.

> ✨ Write Scratch programs like a pro - with C#!

---

## 🚀 Features
- 🔄 Transpiles valid C# into equivalent GoboScript
- ⚙️ Supports basic types, control flow, loops, and functions
- 🧪 Designed to simplify complex logic-heavy Scratch projects
- 💡 Great for teaching C# fundamentals through Scratch

---

## 📦 Requirements

Before using CSScratch, make sure you have the following installed:

- [.NET 9.0 SDK or later](https://dotnet.microsoft.com/download)
- [`goboscript`](https://github.com/aspizu/goboscript) CLI in your system `PATH`

You can verify that `goboscript` is accessible by running:

```bash
goboscript --version
```

---

## 📥 Installation
Clone the repository:

```bash
git clone https://github.com/Fynn93/CSScratch.git
cd CSScratch
```

Build the project:

```bash
dotnet build
```

Or use the precompiled binary from the [Releases](https://github.com/yourusername/csscratch/releases) page.

---

## 🧑‍💻 Usage
Basic command-line usage:

```bash
CSScratch path/to/source_directory path/to/output_directory
```

### Example
Input C# (`MyProgram.cs`):

```csharp
[ScratchEvent(ScratchEventType.Flag)]
private static void SayHello()
{
    say("Hello, World!");
}
```

Output GoboScript (`MyProgram.gs`):

```gobo
proc Main__Initialize  
{
    say "Hello World";
}

onflag
{
    Main__Initialize;
}
```

---

## 🧩 Features
- Variables
- `if`, `else`, `while`
- Functions with parameters
- Events
- Basic math and string operations
- Calling methods from other sprites (`public static` )

> 📌 Not all C# features are supported. CSScratch is focused on the intersection of C# and what GoboScript/Scratch can express.

---

## 📚 Documentation
More examples and a full language reference are available on the [Wiki](https://github.com/Fynn93/CSScratch/wiki).

---

## 🛠️ Contributing
Want to help expand the transpiler?

- Fork the repo
- Create a new branch
- Submit a pull request

Please check out the [Contributing Guide](CONTRIBUTING.md) before you start!

---

## 🧪 Tests
Run the test suite using:

```bash
dotnet test
```

---

## 📄 License
MIT License. See [LICENSE](LICENSE) for details.

---

## ❤️ Acknowledgments
- [Aspizu](https://github.com/aspizu) for the amazing GoboScript compiler
- The Scratch Team for their open platform
- Everyone who contributes ideas and code!

---

Happy Scratching with C#! 🧃
