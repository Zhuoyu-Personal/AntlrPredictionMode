# Running and debugging quick guide

## Run console demo

```bash
dotnet restore
dotnet run --project src/AntlrPredictionMode/AntlrPredictionMode.csproj
```

## Run tests

```bash
dotnet test
```

## Generated parser/lexer C# code location

The grammar is here:

- `src/AntlrPredictionMode/Grammar/PredictionDemo.g4`

Generated C# files are committed here:

- `src/AntlrPredictionMode/Generated/Grammar/`

## Regenerate generated parser code

```bash
./scripts_generate_parser.sh
```
