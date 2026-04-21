using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AntlrPredictionMode.Tests;

[TestClass]
public class PredictionModeTests
{
    private readonly ParseModeRunner _runner = new ParseModeRunner();

    [DataTestMethod]
    [DataRow("a;")]
    [DataRow("a=b;")]
    [DataRow("f(x);")]
    [DataRow("f(g(x));")]
    public void ValidInputs_ParseInTwoStageMode(string input)
    {
        var result = _runner.ParseWithSllThenLlFallback(input);

        Assert.IsTrue(result.Success, $"Expected successful parse. Input={input}, Error={result.Error}");
    }

    [DataTestMethod]
    [DataRow("f(x)=y;")]
    [DataRow("f(g(x))=y;")]
    public void AssignmentAfterCallExpression_UsuallyNeedsExtraPredictionWork(string input)
    {
        var sllOnly = _runner.ParseSllOnly(input);
        var twoStage = _runner.ParseWithSllThenLlFallback(input);

        Assert.IsTrue(twoStage.Success, $"Two-stage parse should succeed. Input={input}, Error={twoStage.Error}");
        Assert.IsTrue(!sllOnly.Success || twoStage.UsedFallback,
            $"Expected either SLL-only failure or LL fallback evidence. Input={input}");
    }

    [TestMethod]
    public void InvalidInput_FailsInAllModes()
    {
        const string input = "f(=x);";

        var sllOnly = _runner.ParseSllOnly(input);
        var twoStage = _runner.ParseWithSllThenLlFallback(input);

        Assert.IsFalse(sllOnly.Success, "SLL-only parse should fail for invalid syntax.");
        Assert.IsFalse(twoStage.Success, "Two-stage parse should still fail for invalid syntax.");
    }
}
