using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;

namespace AntlrPredictionMode;

public sealed class ParseModeRunner
{
    public ParseResult ParseSllOnly(string source)
    {
        var parser = BuildParser(source);
        var diagnostics = new PredictionDiagnostics();

        parser.RemoveErrorListeners();
        parser.ErrorHandler = new BailErrorStrategy();
        parser.Interpreter.PredictionMode = PredictionMode.SLL;
        parser.AddErrorListener(diagnostics);
        parser.AddErrorListener(new DiagnosticErrorListener());

        try
        {
            parser.prog();
            return ParseResult.Success("SLL", usedFallback: false, diagnostics.Messages);
        }
        catch (Exception ex)
        {
            return ParseResult.Failure("SLL", usedFallback: false, ex.Message, diagnostics.Messages);
        }
    }

    public ParseResult ParseWithSllThenLlFallback(string source)
    {
        var sllResult = ParseSllOnly(source);
        if (sllResult.Success)
        {
            return sllResult;
        }

        var parser = BuildParser(source);
        var diagnostics = new PredictionDiagnostics();

        parser.RemoveErrorListeners();
        parser.ErrorHandler = new DefaultErrorStrategy();
        parser.Interpreter.PredictionMode = PredictionMode.LL;
        parser.AddErrorListener(diagnostics);
        parser.AddErrorListener(new DiagnosticErrorListener());

        try
        {
            parser.prog();
            return ParseResult.Success("LL", usedFallback: true, diagnostics.Messages);
        }
        catch (Exception ex)
        {
            return ParseResult.Failure("LL", usedFallback: true, ex.Message, diagnostics.Messages);
        }
    }

    private static PredictionDemoParser BuildParser(string source)
    {
        var input = new AntlrInputStream(source);
        var lexer = new PredictionDemoLexer(input);
        var tokens = new CommonTokenStream(lexer);
        return new PredictionDemoParser(tokens);
    }
}

public sealed class PredictionDiagnostics : BaseErrorListener
{
    private readonly List<string> _messages = new List<string>();

    public IReadOnlyList<string> Messages => _messages;

    public override void SyntaxError(
        [NotNull] IRecognizer recognizer,
        IToken offendingSymbol,
        int line,
        int charPositionInLine,
        [NotNull] string msg,
        RecognitionException e)
    {
        _messages.Add($"SyntaxError at {line}:{charPositionInLine} => {msg}");
    }
}

public sealed class ParseResult
{
    private ParseResult(string mode, bool success, bool usedFallback, string? error, IReadOnlyList<string> diagnostics)
    {
        Mode = mode;
        Success = success;
        UsedFallback = usedFallback;
        Error = error;
        Diagnostics = diagnostics;
    }

    public string Mode { get; }

    public bool Success { get; }

    public bool UsedFallback { get; }

    public string? Error { get; }

    public IReadOnlyList<string> Diagnostics { get; }

    public static ParseResult Success(string mode, bool usedFallback, IReadOnlyList<string> diagnostics)
        => new ParseResult(mode, success: true, usedFallback, error: null, diagnostics);

    public static ParseResult Failure(string mode, bool usedFallback, string error, IReadOnlyList<string> diagnostics)
        => new ParseResult(mode, success: false, usedFallback, error, diagnostics);
}
