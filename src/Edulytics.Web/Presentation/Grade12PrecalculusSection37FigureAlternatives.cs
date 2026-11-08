namespace Edulytics.Web.Presentation;

/// <summary>Independently authored teaching explanations for the 23 mathematical
/// concepts shown by local source Section 3.7 figures. Not image reproductions.
/// Stage D runtime not executed.</summary>
public static class Grade12PrecalculusSection37FigureAlternatives
{
    public sealed record FigureSkill(int OriginalFigureNumber,
        string Topic, string IndependentModel, string StudentExplanation,
        int RelatedIndependentGraphExercise);
    public static readonly IReadOnlyList<FigureSkill> All =
    [
        new(1, "Reciprocal and reciprocal-square families", "Compare y=1/(x−1) and y=1/(x−1)², noting the one-sided signs at x=1.", "Different pole parity gives opposite versus same-sign divergence.", 61),
        new(2, "Arrow notation at a vertical pole and infinity", "For y=1/(x+2), list one-sided limits at −2 and both end limits.", "Translate graph behavior into x→a± and x→±∞ notation.", 62),
        new(3, "Vertical asymptote definition", "For y=2/(x−3), mark x=3 as excluded and describe one-sided unbounded behavior.", "A vertical asymptote is a limiting behavior, not an included graph point.", 57),
        new(4, "End behavior arrow notation", "For y=−3/(x+1), describe outputs for very large positive and negative inputs.", "Distinguish end behavior from local behavior.", 62),
        new(5, "Horizontal asymptote line", "For y=4/(x−2), identify y=0 and draw the level independently.", "A horizontal asymptote is an end-limit, not an uncrossable barrier.", 61),
        new(6, "Read one-sided and end limits from an independently plotted curve", "For y=(x−1)/((x+2)(x−3)), identify both vertical poles and the zero.", "Graph-to-arrow reading requires branch signs and the correct excluded inputs.", 57),
        new(7, "Translated reciprocal graph", "For y=2/(x+3)+1, mark x=−3 and y=1.", "Read shifts from parent y=1/x with nonzero asymptotes.", 62),
        new(8, "Original-domain zero exclusion with a removable hole", "For y=(x−1)(x+2)/((x−1)(x−4)), identify the hole and vertical pole.", "A cancelled factor still forbids its input; x=1 is a hole at y=−1 and x=4 remains a vertical pole.", 61),
        new(9, "Two uncancelled denominator roots", "For y=5/((x−1)(x+4)), identify vertical poles x=1 and x=−4.", "Real uncancelled denominator zeros produce vertical asymptotes.", 57),
        new(10, "Distinguish removable discontinuity and vertical asymptote", "For y=(x+2)(x−3)/((x+2)(x−1)), find hole (−2,5/3) and pole x=1.", "A domain hole and asymptote have different limiting behavior.", 58),
        new(11, "Hole versus remaining denominator factor", "For y=(x−2)(x+5)/((x−2)(x+3)), identify hole (2,7/5) and pole x=−3.", "Factor, cancel, and preserve original exclusions.", 59),
        new(12, "Numerator lower degree and horizontal limit zero", "For y=(x−2)/((x+1)(x−4)), find horizontal y=0.", "A rational curve may cross its horizontal asymptote at a finite input.", 59),
        new(13, "Linear slant end asymptote from polynomial division", "For y=(x²+4x+7)/(x+1), divide to y=x+3+4/(x+1).", "The quotient line y=x+3 controls end behavior.", 61),
        new(14, "Equal-degree leading coefficient horizontal limit", "For y=(5x²+2x+2)/(2x²+3), identify horizontal y=5/2 and the finite crossing x=11/4.", "A horizontal level can be crossed where the rational function equals 5/2.", 62),
        new(15, "Several poles and zero end-limit", "For y=(x+2)/((x−1)(x+3)(x−4)), identify three vertical poles and horizontal y=0.", "Compare three branch intervals and zero degree dominance.", 60),
        new(16, "Intercepts alongside asymptotes", "For y=(x−1)(x+3)/((x−2)(x+4)), derive zeros and y-intercept before sketching.", "Crossings from numerator roots must not coincide with denominator exclusions.", 57),
        new(17, "Odd-multiplicity pole branches", "For y=1/(x−3), verify opposite one-sided infinities.", "A simple denominator zero changes local sign.", 61),
        new(18, "Even-multiplicity pole branches", "For y=1/(x−3)², verify same positive infinity on both sides.", "The square factor does not change sign across the excluded input.", 63),
        new(19, "Repeated root and repeated pole affect touching/crossing", "For y=(x−1)²/((x+2)²(x−3)), compare contact at x=1 with second-order pole x=−2.", "A repeated numerator zero can touch; even denominator multiplicity gives same-side divergence.", 63),
        new(20, "Use signs to fill intervals between vertical asymptotes", "For y=(x−2)²/((x+2)(x−4)), evaluate sample signs in all intervals.", "A sign chart fixes branch placement without tracing a publisher graph.", 63),
        new(21, "Complete missing curve branches using critical inputs", "For y=(x−1)²/((x+3)(x−4)), use x=−3,1,4 and end y=1.", "Infer branch locations from factors, a tangency and end behavior.", 64),
        new(22, "Infer rational factors from visually marked intercepts and poles", "Interpret independently drawn exercise 57 as a factored rational function.", "Source-equivalent skill is algebraic reconstruction using graph features.", 57),
        new(23, "Infer factor multiplicities from one- versus two-sided shape", "Interpret independently drawn exercise 64, noting the double zero at x=1.", "Tangency identifies an even numerator multiplicity; poles identify denominator factors.", 64),
    ];
}
