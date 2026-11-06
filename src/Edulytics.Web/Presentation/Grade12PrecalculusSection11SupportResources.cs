namespace Edulytics.Web.Presentation;

/// <summary>
/// Independent Edulytics text alternatives for the six original Section 1.1
/// optional web resources. No third-party video, images or link destinations
/// are embedded or asserted to remain available.
/// </summary>
public static class Grade12PrecalculusSection11SupportResources
{
    public sealed record Resource(int OriginalMediaOrdinal,
        string Title, string FirstPartyExplanation, string RelatedPracticeNumbers);

    public static readonly IReadOnlyList<Resource> All =
    [
        new(1, "Functions from relations", "A relation assigns ordered-pair outputs. A function permits at most one output per input; compare (2,4) and (2,5), which cannot both appear in a function.", "1,6"),
        new(2, "Vertical-line reasoning", "A vertical line x=1 crosses the relation x=y² at both (1,1) and (1,−1). This is a precise counterexample to a single-valued function.", "3,10,41"),
        new(3, "Meaning of function notation", "For f(x)=x², the input x=−2 produces output 4. Each x has one output even though two different inputs may share the output 4.", "2,9,27"),
        new(4, "Graphical test of functionhood", "A circle x²+y²=4 fails the vertical-line test at x=0 because y=±2. The nonvertical line y=2x+1 passes.", "40,44,47,50"),
        new(5, "One-to-one depends on domain", "f(x)=x² is not one-to-one on all real inputs because f(−2)=f(2)=4. Restricted to nonnegative x, it becomes one-to-one.", "4,5,9,92"),
        new(6, "Horizontal-line evidence on graphs", "The horizontal level y=4 cuts y=x² at two distinct x-values, −2 and 2. Therefore this function is not one-to-one on its full real domain.", "55,56,57,58,59"),
    ];
}
