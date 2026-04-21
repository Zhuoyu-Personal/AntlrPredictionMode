# AntlrPredictionMode (.NET Framework 4.8)

This repo is a **hands-on ANTLR4 demo** for understanding how parsing behaves in:

- **SLL only** mode (fast, can fail on ambiguous contexts)
- **SLL -> LL fallback** mode (practical production strategy)

---

## 1) Prerequisites

You need a machine with:

- .NET SDK (8+ is fine) or Visual Studio 2022
- ability to restore NuGet packages

> This project targets **`net48`** and uses `Microsoft.NETFramework.ReferenceAssemblies`, so it can be built on non-Windows hosts too.

---

## 2) How to run the program

From repository root:

```bash
dotnet restore
dotnet run --project src/AntlrPredictionMode/AntlrPredictionMode.csproj
```

The app prints each sample input and shows:

- SLL-only result
- SLL->LL fallback result
- parse/diagnostic errors (if any)

---

## 3) How to run the tests

```bash
dotnet test
```

Test cases are in:

- `tests/AntlrPredictionMode.Tests/PredictionModeTests.cs`

---

## 4) Generated parser code (checked in)

The grammar is:

- `src/AntlrPredictionMode/Grammar/PredictionDemo.g4`

Generated C# parser/lexer files are checked in under:

- `src/AntlrPredictionMode/Generated/Grammar/`

This means you can inspect parser code directly in the repo without building first.

### Regenerate parser files

```bash
./scripts_generate_parser.sh
```

---

## 5) Grammar intent

The grammar intentionally mixes:

- expression statements
- assignment statements
- call expressions with nesting

Examples like `f(x)=y;` and `f(g(x))=y;` are useful when exploring where SLL may need extra context and when LL fallback helps.
