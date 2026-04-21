using System;
using System.Collections.Generic;

namespace AntlrPredictionMode;

internal static class Program
{
    private static void Main()
    {
        var runner = new ParseModeRunner();

        var samples = new Dictionary<string, string>
        {
            ["Simple expression statement"] = "a;",
            ["Simple assignment"] = "a=b;",
            ["Simple call"] = "f(x);",
            ["Call used as assignment LHS"] = "f(x)=y;",
            ["Nested call"] = "f(g(x));",
            ["Nested call used as assignment LHS"] = "f(g(x))=y;",
            ["Intentionally invalid"] = "f(=x);"
        };

        foreach (var sample in samples)
        {
            Console.WriteLine($"=== {sample.Key} ===");
            Console.WriteLine(sample.Value);

            var sll = runner.ParseSllOnly(sample.Value);
            Print("SLL-only", sll);

            var fallback = runner.ParseWithSllThenLlFallback(sample.Value);
            Print("SLL->LL", fallback);

            Console.WriteLine();
        }
    }

    private static void Print(string label, ParseResult result)
    {
        Console.WriteLine($"  [{label}] Success={result.Success}, Mode={result.Mode}, Fallback={result.UsedFallback}");

        if (!result.Success)
        {
            Console.WriteLine($"    Error: {result.Error}");
        }

        foreach (var message in result.Diagnostics)
        {
            Console.WriteLine($"    {message}");
        }
    }
}
