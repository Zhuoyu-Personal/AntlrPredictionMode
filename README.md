# AntlrPredictionMode (.NET Framework 4.8)

This repository demonstrates ANTLR4 parsing behavior differences between **SLL** and **LL** prediction modes using a .NET Framework 4.8 project.

## What is included

- A .NET Framework 4.8 console project
- A non-trivial grammar file: `src/AntlrPredictionMode/Grammar/PredictionDemo.g4`
- ANTLR4 C# runtime via NuGet (`Antlr4.Runtime.Standard`)
- Build-time grammar code generation via `Antlr4BuildTasks`
- A test project with multiple focused cases for SLL vs LL behavior

## Why this grammar?

The grammar intentionally combines:

- expression statements vs assignment statements
- function-call expressions
- nested call syntax

These constructs create prefixes that are easy to parse with SLL in many cases, but may require LL fallback in ambiguous contexts.

## Running locally

1. Install .NET SDK that can build `net48` projects.
2. Restore and build:

```bash
dotnet restore
dotnet build
```

3. Run the demo app:

```bash
dotnet run --project src/AntlrPredictionMode/AntlrPredictionMode.csproj
```

4. Run tests:

```bash
dotnet test
```

## Expected learning outcomes

- See where strict SLL parsing can fail fast.
- See how two-stage parsing (SLL + LL fallback) recovers those cases.
- Inspect parser diagnostics that indicate full-context attempts.
