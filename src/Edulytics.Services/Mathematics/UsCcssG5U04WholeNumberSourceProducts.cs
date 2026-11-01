using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Reuses ExactSkillContractQuestionEngine for 2021 G5 Unit 04 source-aligned
/// whole-number multiplication. This bounded adapter stores exact independent
/// source exercise cases and selected digital choices, NOT a new engine.
/// Source estimates, open strategy and diagram proof need teacher review.
/// Written tests AUTHORED_NOT_RUN.
/// </summary>
public static class UsCcssG5U04WholeNumberSourceProducts
{
    public const string L01Product="usccss.g5.u04.l01.source_multi_digit_product_values";
    public const string L01Estimate="usccss.g5.u04.l01.source_reasonable_product_estimate";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private sealed record SourceCase(string Family,string Context,string Expression,
        int Result,string Units,bool SourceEstimate=false);
    private static readonly SourceCase[] Cases =
    [
        new(L01Product,"Original IM Grade 5 U04 L01 number talk","50 × 6",300,""),
        new(L01Product,"Original IM Grade 5 U04 L01 number talk","50 × 60",3000,""),
        new(L01Product,"Original IM Grade 5 U04 L01 number talk","50 × 600",30000,""),
        new(L01Product,"Original IM Grade 5 U04 L01 number talk","600 × 500",300000,""),
        new(L01Product,"Original IM Grade 5 U04 L01 Activity 2","18 × 9",162,""),
        new(L01Product,"Original IM Grade 5 U04 L01 Activity 2","18 × 49",882,""),
        new(L01Product,"Original IM Grade 5 U04 L01 Activity 2","18 × 149",2682,""),
        new(L01Estimate,"Which of the FOUR original source benchmarks is the MOST REASONABLE estimate of 18×149?",
            "18 × 149, estimate",3000,"",true),
        new("usccss.g5.u04.l02.original_partial_products_totals", "Original textbook U04:L02 area model of 42 by 33; teacher reviews diagram organization", "42 × 33", 1386, "square units of area model"),
        new("usccss.g5.u04.l02.original_partial_products_totals", "Original textbook U04:L02 area model of 142 by 33; teacher reviews diagram organization", "142 × 33", 4686, "square units of area model"),
        new("usccss.g5.u04.l02.original_partial_products_totals", "Original textbook U04:L02 area model of 315 by 24; teacher reviews diagram organization", "315 × 24", 7560, "square units of area model"),
        new("usccss.g5.u04.l02.original_area_diagram_cells", "Original source 42×33 or 315×24 rectangular partial-product cell using factors 30 and 40", "30 × 40", 1200, "partial-product units"),
        new("usccss.g5.u04.l02.original_area_diagram_cells", "Original source 42×33 or 315×24 rectangular partial-product cell using factors 30 and 2", "30 × 2", 60, "partial-product units"),
        new("usccss.g5.u04.l02.original_area_diagram_cells", "Original source 42×33 or 315×24 rectangular partial-product cell using factors 3 and 40", "3 × 40", 120, "partial-product units"),
        new("usccss.g5.u04.l02.original_area_diagram_cells", "Original source 42×33 or 315×24 rectangular partial-product cell using factors 3 and 2", "3 × 2", 6, "partial-product units"),
        new("usccss.g5.u04.l02.original_area_diagram_cells", "Original source 42×33 or 315×24 rectangular partial-product cell using factors 300 and 20", "300 × 20", 6000, "partial-product units"),
        new("usccss.g5.u04.l02.original_area_diagram_cells", "Original source 42×33 or 315×24 rectangular partial-product cell using factors 10 and 20", "10 × 20", 200, "partial-product units"),
        new("usccss.g5.u04.l02.original_area_diagram_cells", "Original source 42×33 or 315×24 rectangular partial-product cell using factors 5 and 20", "5 × 20", 100, "partial-product units"),
        new("usccss.g5.u04.l02.original_area_diagram_cells", "Original source 42×33 or 315×24 rectangular partial-product cell using factors 300 and 4", "300 × 4", 1200, "partial-product units"),
        new("usccss.g5.u04.l02.original_area_diagram_cells", "Original source 42×33 or 315×24 rectangular partial-product cell using factors 10 and 4", "10 × 4", 40, "partial-product units"),
        new("usccss.g5.u04.l02.original_area_diagram_cells", "Original source 42×33 or 315×24 rectangular partial-product cell using factors 5 and 4", "5 × 4", 20, "partial-product units"),
        new("usccss.g5.u04.l03.original_partial_product_algorithm", "Original source L03 multiply 245 by 35, and use place-value partial products", "245 × 35", 8575, "whole-number product"),
        new("usccss.g5.u04.l03.original_partial_product_algorithm", "Original source L03 multiply 245 by 30, and use place-value partial products", "245 × 30", 7350, "whole-number product"),
        new("usccss.g5.u04.l03.original_partial_product_algorithm", "Original source L03 multiply 245 by 5, and use place-value partial products", "245 × 5", 1225, "whole-number product"),
        new("usccss.g5.u04.l03.original_partial_product_algorithm", "Original source L03 multiply 4 by 5342, and use place-value partial products", "4 × 5342", 21368, "whole-number product"),
        new("usccss.g5.u04.l03.original_partial_product_algorithm", "Original source L03 multiply 5 by 5342, and use place-value partial products", "5 × 5342", 26710, "whole-number product"),
        new("usccss.g5.u04.l03.original_245_by_35_partial_cells", "Source 245×35 six-cell partial product for 200 and 30", "200 × 30", 6000, "partial-product contribution"),
        new("usccss.g5.u04.l03.original_245_by_35_partial_cells", "Source 245×35 six-cell partial product for 40 and 30", "40 × 30", 1200, "partial-product contribution"),
        new("usccss.g5.u04.l03.original_245_by_35_partial_cells", "Source 245×35 six-cell partial product for 5 and 30", "5 × 30", 150, "partial-product contribution"),
        new("usccss.g5.u04.l03.original_245_by_35_partial_cells", "Source 245×35 six-cell partial product for 200 and 5", "200 × 5", 1000, "partial-product contribution"),
        new("usccss.g5.u04.l03.original_245_by_35_partial_cells", "Source 245×35 six-cell partial product for 40 and 5", "40 × 5", 200, "partial-product contribution"),
        new("usccss.g5.u04.l03.original_245_by_35_partial_cells", "Source 245×35 six-cell partial product for 5 and 5", "5 × 5", 25, "partial-product contribution"),
        new("usccss.g5.u04.l04.original_carried_multi_digit_products", "Original source U04:L04 standard algorithm multiplication 318 by 3; composing place-value units must be explained", "318 × 3", 954, "whole-number product"),
        new("usccss.g5.u04.l04.original_carried_multi_digit_products", "Original source U04:L04 standard algorithm multiplication 3615 by 4; composing place-value units must be explained", "3615 × 4", 14460, "whole-number product"),
        new("usccss.g5.u04.l04.original_carried_multi_digit_products", "Original source U04:L04 standard algorithm multiplication 16023 by 3; composing place-value units must be explained", "16023 × 3", 48069, "whole-number product"),
        new("usccss.g5.u04.l04.original_carried_multi_digit_products", "Original source U04:L04 standard algorithm multiplication 27326 by 3; composing place-value units must be explained", "27326 × 3", 81978, "whole-number product"),
        new("usccss.g5.u04.l04.original_carried_multi_digit_products", "Original source U04:L04 standard algorithm multiplication 10215 by 6; composing place-value units must be explained", "10215 × 6", 61290, "whole-number product"),
        new("usccss.g5.u04.l04.original_warmup_partial_products", "Original source U04:L04 Number Talk partial product 3 by 3", "3 × 3", 9, "exact result"),
        new("usccss.g5.u04.l04.original_warmup_partial_products", "Original source U04:L04 Number Talk partial product 3 by 20", "3 × 20", 60, "exact result"),
        new("usccss.g5.u04.l04.original_warmup_partial_products", "Original source U04:L04 Number Talk partial product 3 by 600", "3 × 600", 1800, "exact result"),
        new("usccss.g5.u04.l04.original_warmup_partial_products", "Original source U04:L04 Number Talk partial product 3 by 623", "3 × 623", 1869, "exact result"),
        new("usccss.g5.u04.l05.original_no_composing_products", "Original source U04:L05 no-composing standard algorithm 413 by 21", "413 × 21", 8673, "whole-number product"),
        new("usccss.g5.u04.l05.original_no_composing_products", "Original source U04:L05 no-composing standard algorithm 202 by 12", "202 × 12", 2424, "whole-number product"),
        new("usccss.g5.u04.l05.original_no_composing_products", "Original source U04:L05 no-composing standard algorithm 122 by 33", "122 × 33", 4026, "whole-number product"),
        new("usccss.g5.u04.l05.original_no_composing_products", "Original source U04:L05 no-composing standard algorithm 321 by 24", "321 × 24", 7704, "whole-number product"),
        new("usccss.g5.u04.l05.original_no_composing_products", "Original source U04:L05 no-composing standard algorithm 301 by 24", "301 × 24", 7224, "whole-number product"),
        new("usccss.g5.u04.l05.original_number_talk_and_diego_correction", "Original source U04:L05 Number Talk or Diego partial row 20 by 3", "20 × 3", 60, "partial-product value"),
        new("usccss.g5.u04.l05.original_number_talk_and_diego_correction", "Original source U04:L05 Number Talk or Diego partial row 24 by 3", "24 × 3", 72, "partial-product value"),
        new("usccss.g5.u04.l05.original_number_talk_and_diego_correction", "Original source U04:L05 Number Talk or Diego partial row 120 by 3", "120 × 3", 360, "partial-product value"),
        new("usccss.g5.u04.l05.original_number_talk_and_diego_correction", "Original source U04:L05 Number Talk or Diego partial row 140 by 3", "140 × 3", 420, "partial-product value"),
        new("usccss.g5.u04.l05.original_number_talk_and_diego_correction", "Original source U04:L05 Number Talk or Diego partial row 301 by 4", "301 × 4", 1204, "partial-product value"),
        new("usccss.g5.u04.l05.original_number_talk_and_diego_correction", "Original source U04:L05 Number Talk or Diego partial row 301 by 20", "301 × 20", 6020, "partial-product value"),
        // END_G5_U04_SOURCE_CASES
    ];
    public static bool Supports(string? family)=>
        !string.IsNullOrWhiteSpace(family) &&
        Cases.Any(x=>string.Equals(x.Family,family,StringComparison.Ordinal));
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p,
        out SourceCase? current)
    {
        current=null;
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("case",out int index)||index<0||
           !p.TryGetValue("slot",out int slot)||slot is <0 or >2)return false;
        var entries=Cases.Where(x=>x.Family==family).ToArray();
        if(index>=entries.Length)return false;
        current=entries[index];
        return !current.SourceEstimate || slot==2;
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported Grade5 Unit4 source family.");
        ArgumentNullException.ThrowIfNull(random);
        var cases=Cases.Where(x=>x.Family==family).ToArray();
        int n=random.Next(cases.Length);
        var e=cases[n];
        int slot=e.SourceEstimate?2:random.Next(3);
        IReadOnlyDictionary<string,int> p=new Dictionary<string,int>(StringComparer.Ordinal)
            {["case"]=n,["slot"]=slot};
        if(!Valid(family,p,out _))throw new InvalidOperationException("Invalid bounded source item.");
        string prompt,solution;
        if(e.SourceEstimate)
        {
            prompt="IM2021 original Activity1: 18×149. Which estimate is most reasonable? "+
                "Select 1: 2000; 2: 4000; 3: 3000; 4: 1500. "+
                "Explain why your estimate is plausible and whether the exact product "+
                "is more or less: TEACHER_REVIEW_REQUIRED.";
            solution="18×149=2682, closest to 3000 of the four original options; "+
                "explain that 18×150≈2700 and the product is less than the selected 3000.";
        }
        else
        {
            int correct=e.Result;
            int wrongA=correct+Math.Max(10,correct/10);
            int wrongB=Math.Max(1,correct-Math.Max(10,correct/10));
            int[] opts=slot switch
            {
                0=>[correct,wrongA,wrongB],
                1=>[wrongA,correct,wrongB],
                _=>[wrongA,wrongB,correct]
            };
            prompt=$"{e.Context}. Calculate the exact value of {e.Expression}. "+
                $"Select 1: {opts[0]}; 2: {opts[1]}; 3: {opts[2]}. "+
                "Multi-digit place-value reasoning and original estimate discussion are TEACHER_REVIEW_REQUIRED.";
            solution=$"The exact original-source product {e.Expression} is {correct} {e.Units}. "+
                "The student strategy, place-value decomposition and estimation justification need teacher scoring.";
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out _))throw new InvalidOperationException("Invalid source parameters.");
        return (p["slot"]+1).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? answer)=>
        Valid(family,p,out _) &&
        string.Equals(answer,(p!["slot"]+1).ToString(CultureInfo.InvariantCulture),
            StringComparison.Ordinal);
}
