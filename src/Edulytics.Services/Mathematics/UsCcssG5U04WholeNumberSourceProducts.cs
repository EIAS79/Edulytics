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
        new("usccss.g5.u04.l06.original_carried_multiplication", "Original source G5 U04:L06 composing algorithm 241×23", "241 × 23", 5543, "whole-number product"),
        new("usccss.g5.u04.l06.original_carried_multiplication", "Original source G5 U04:L06 composing algorithm 241×3", "241 × 3", 723, "whole-number product"),
        new("usccss.g5.u04.l06.original_carried_multiplication", "Original source G5 U04:L06 composing algorithm 241×20", "241 × 20", 4820, "whole-number product"),
        new("usccss.g5.u04.l06.original_carried_multiplication", "Original source G5 U04:L06 composing algorithm 182×41", "182 × 41", 7462, "whole-number product"),
        new("usccss.g5.u04.l06.original_carried_multiplication", "Original source G5 U04:L06 composing algorithm 304×23", "304 × 23", 6992, "whole-number product"),
        new("usccss.g5.u04.l06.original_carried_multiplication", "Original source G5 U04:L06 composing algorithm 647×9", "647 × 9", 5823, "whole-number product"),
        new("usccss.g5.u04.l06.original_carried_multiplication", "Original source G5 U04:L06 composing algorithm 647×50", "647 × 50", 32350, "whole-number product"),
        new("usccss.g5.u04.l06.original_carried_multiplication", "Original source G5 U04:L06 composing algorithm 647×59", "647 × 59", 38173, "whole-number product"),
        new("usccss.g5.u04.l06.original_carried_multiplication", "Original source G5 U04:L06 composing algorithm 264×38", "264 × 38", 10032, "whole-number product"),
        new("usccss.g5.u04.l06.original_three_factor_number_talk", "Original source L06 Number Talk three factors, intermediate product 6 multiplied by ten", "6 × 10", 60, "whole-number product"),
        new("usccss.g5.u04.l06.original_three_factor_number_talk", "Original source L06 Number Talk three factors, intermediate product 80 multiplied by ten", "80 × 10", 800, "whole-number product"),
        new("usccss.g5.u04.l06.original_three_factor_number_talk", "Original source L06 Number Talk three factors, intermediate product 400 multiplied by ten", "400 × 10", 4000, "whole-number product"),
        new("usccss.g5.u04.l06.original_three_factor_number_talk", "Original source L06 Number Talk three factors, intermediate product 486 multiplied by ten", "486 × 10", 4860, "whole-number product"),
        new("usccss.g5.u04.l07.source_composing_products_and_commutativity", "Original U04:L07 source diagram or composing exercise 417 by 28", "417 × 28", 11676, "whole-number product"),
        new("usccss.g5.u04.l07.source_composing_products_and_commutativity", "Original U04:L07 source diagram or composing exercise 28 by 417", "28 × 417", 11676, "whole-number product"),
        new("usccss.g5.u04.l07.source_composing_products_and_commutativity", "Original U04:L07 source diagram or composing exercise 256 by 5", "256 × 5", 1280, "whole-number product"),
        new("usccss.g5.u04.l07.source_composing_products_and_commutativity", "Original U04:L07 source diagram or composing exercise 587 by 8", "587 × 8", 4696, "whole-number product"),
        new("usccss.g5.u04.l07.source_composing_products_and_commutativity", "Original U04:L07 source diagram or composing exercise 809 by 9", "809 × 9", 7281, "whole-number product"),
        new("usccss.g5.u04.l08.original_distributive_number_talk", "Original G5 U04:L08 Number Talk using distributive property", "6 × (15)", 90, "exact product"),
        new("usccss.g5.u04.l08.original_distributive_number_talk", "Original G5 U04:L08 Number Talk using distributive property", "6 × (17)", 102, "exact product"),
        new("usccss.g5.u04.l08.original_distributive_number_talk", "Original G5 U04:L08 Number Talk using distributive property", "6 × (2 1/3)", 14, "exact product"),
        new("usccss.g5.u04.l08.original_distributive_number_talk", "Original G5 U04:L08 Number Talk using distributive property", "6 × (2 2/3)", 16, "exact product"),
        new("usccss.g5.u04.l08.original_digits_target_products", "Edulytiks-computed trial from original source digits 3,5,6,8,9: 893×56. Teacher checks source digit uniqueness and global closest-product strategy", "893 × 56", 50008, "whole-number product"),
        new("usccss.g5.u04.l08.original_digits_target_products", "Edulytiks-computed trial from original source digits 3,5,6,8,9: 589×36. Teacher checks source digit uniqueness and global closest-product strategy", "589 × 36", 21204, "whole-number product"),
        new("usccss.g5.u04.l08.original_digits_target_products", "Edulytiks-computed trial from original source digits 3,5,6,8,9: 856×39. Teacher checks source digit uniqueness and global closest-product strategy", "856 × 39", 33384, "whole-number product"),
        new("usccss.g5.u04.l08.original_digits_target_products", "Edulytiks-computed trial from original source digits 3,5,6,8,9: 693×58. Teacher checks source digit uniqueness and global closest-product strategy", "693 × 58", 40194, "whole-number product"),
        new("usccss.g5.u04.l09.source_birdhouse_min_volumes", "IM 2021 source birdhouse chickadee, rectangular interior floor 4 inches by 4 inches, MINIMUM height 6 inches", "4 × 4 × 6", 96, "cubic inches (minimum volume)"),
        new("usccss.g5.u04.l09.source_birdhouse_max_volumes", "IM 2021 source birdhouse chickadee, rectangular interior floor 4 inches by 4 inches, MAXIMUM height 10 inches", "4 × 4 × 10", 160, "cubic inches (maximum volume)"),
        new("usccss.g5.u04.l09.source_birdhouse_min_volumes", "IM 2021 source birdhouse wood duck, rectangular interior floor 10 inches by 18 inches, MINIMUM height 10 inches", "10 × 18 × 10", 1800, "cubic inches (minimum volume)"),
        new("usccss.g5.u04.l09.source_birdhouse_max_volumes", "IM 2021 source birdhouse wood duck, rectangular interior floor 10 inches by 18 inches, MAXIMUM height 24 inches", "10 × 18 × 24", 4320, "cubic inches (maximum volume)"),
        new("usccss.g5.u04.l09.source_birdhouse_min_volumes", "IM 2021 source birdhouse barn owl, rectangular interior floor 10 inches by 18 inches, MINIMUM height 15 inches", "10 × 18 × 15", 2700, "cubic inches (minimum volume)"),
        new("usccss.g5.u04.l09.source_birdhouse_max_volumes", "IM 2021 source birdhouse barn owl, rectangular interior floor 10 inches by 18 inches, MAXIMUM height 18 inches", "10 × 18 × 18", 3240, "cubic inches (maximum volume)"),
        new("usccss.g5.u04.l09.source_birdhouse_min_volumes", "IM 2021 source birdhouse red-headed woodpecker, rectangular interior floor 6 inches by 6 inches, MINIMUM height 12 inches", "6 × 6 × 12", 432, "cubic inches (minimum volume)"),
        new("usccss.g5.u04.l09.source_birdhouse_max_volumes", "IM 2021 source birdhouse red-headed woodpecker, rectangular interior floor 6 inches by 6 inches, MAXIMUM height 15 inches", "6 × 6 × 15", 540, "cubic inches (maximum volume)"),
        new("usccss.g5.u04.l09.source_birdhouse_min_volumes", "IM 2021 source birdhouse bluebird, rectangular interior floor 5 inches by 5 inches, MINIMUM height 6 inches", "5 × 5 × 6", 150, "cubic inches (minimum volume)"),
        new("usccss.g5.u04.l09.source_birdhouse_max_volumes", "IM 2021 source birdhouse bluebird, rectangular interior floor 5 inches by 5 inches, MAXIMUM height 12 inches", "5 × 5 × 12", 300, "cubic inches (maximum volume)"),
        new("usccss.g5.u04.l09.source_birdhouse_min_volumes", "IM 2021 source birdhouse swallow, rectangular interior floor 6 inches by 6 inches, MINIMUM height 6 inches", "6 × 6 × 6", 216, "cubic inches (minimum volume)"),
        new("usccss.g5.u04.l09.source_birdhouse_max_volumes", "IM 2021 source birdhouse swallow, rectangular interior floor 6 inches by 6 inches, MAXIMUM height 8 inches", "6 × 6 × 8", 288, "cubic inches (maximum volume)"),
        new("usccss.g5.u04.l19.source_state_rectangular_area_products", "Original IM K5 2021 G5 U04:L19 source Rhode Island IDEALIZED rectangle approximate size 77km by 60km, compare to original garbage-patch rough area of 1,000,000 square kilometers (not actual surveyed state area)", "77 × 60", 4620, "square kilometers of source approximate rectangular model"),
        new("usccss.g5.u04.l19.source_state_rectangular_area_products", "Original IM K5 2021 G5 U04:L19 source Delaware IDEALIZED rectangle approximate size 154km by 48km, compare to original garbage-patch rough area of 1,000,000 square kilometers (not actual surveyed state area)", "154 × 48", 7392, "square kilometers of source approximate rectangular model"),
        new("usccss.g5.u04.l19.source_state_rectangular_area_products", "Original IM K5 2021 G5 U04:L19 source New Mexico IDEALIZED rectangle approximate size 596km by 552km, compare to original garbage-patch rough area of 1,000,000 square kilometers (not actual surveyed state area)", "596 × 552", 328992, "square kilometers of source approximate rectangular model"),
        new("usccss.g5.u04.l21.source_food_waste_annual_ounces", "Original 2021 Grade5 U04:L21 printed national average 219 POUNDS food waste per person per YEAR. Convert original source using16 ounces per pound", "219 × 16", 3504, "ounces of food waste per person per modeled year"),
        new("usccss.g5.u04.l21.independent_group_annual_estimates", "INDEPENDENT EDULYTIKS ILLUSTRATIVE POPULATION (NOT PRINTED SOURCE SIZE): hypothetical four-person family, if EACH person generates the original printed 219 pounds/year", "219 × 4", 876, "pounds of estimated annual food waste for the hypothetical population"),
        new("usccss.g5.u04.l21.independent_group_annual_estimates", "INDEPENDENT EDULYTIKS ILLUSTRATIVE POPULATION (NOT PRINTED SOURCE SIZE): hypothetical twenty-five-person class, if EACH person generates the original printed 219 pounds/year", "219 × 25", 5475, "pounds of estimated annual food waste for the hypothetical population"),
        new("usccss.g5.u04.l21.independent_group_annual_estimates", "INDEPENDENT EDULYTIKS ILLUSTRATIVE POPULATION (NOT PRINTED SOURCE SIZE): hypothetical 500-person school, if EACH person generates the original printed 219 pounds/year", "219 × 500", 109500, "pounds of estimated annual food waste for the hypothetical population"),
        new(L01Estimate,"Edulytiks-original supplemental nearest-thousand product estimate", "17 × 149", 3000, "", true),
        new(L01Estimate,"Edulytiks-original supplemental nearest-thousand product estimate", "19 × 149", 3000, "", true),
        new(L01Estimate,"Edulytiks-original supplemental nearest-thousand product estimate", "20 × 149", 3000, "", true),
        new(L01Estimate,"Edulytiks-original supplemental nearest-thousand product estimate", "21 × 149", 3000, "", true),
        new(L01Estimate,"Edulytiks-original supplemental nearest-thousand product estimate", "22 × 149", 3000, "", true),
        new(L01Estimate,"Edulytiks-original supplemental nearest-thousand product estimate", "15 × 149", 2000, "", true),
        new(L01Estimate,"Edulytiks-original supplemental nearest-thousand product estimate", "14 × 149", 2000, "", true),
        new(L01Estimate,"Edulytiks-original supplemental nearest-thousand product estimate", "13 × 149", 2000, "", true),
        new(L01Estimate,"Edulytiks-original supplemental nearest-thousand product estimate", "24 × 149", 4000, "", true),
        new(L01Estimate,"Edulytiks-original supplemental nearest-thousand product estimate", "26 × 149", 4000, "", true),
        new(L01Estimate,"Edulytiks-original supplemental nearest-thousand product estimate", "27 × 149", 4000, "", true),
        new(L01Estimate,"Edulytiks-original supplemental nearest-thousand product estimate", "31 × 149", 5000, "", true),
        new("usccss.g5.u04.l02.original_partial_products_totals","Edulytiks-original supplementary area model with 215 by 34","215 × 34",7310,"square units of an area model"),
        new("usccss.g5.u04.l19.source_state_rectangular_area_products","Edulytiks-original supplementary rectangular-area product with sides 13 and 17","13 × 17",221,"square units"),
        new("usccss.g5.u04.l21.source_food_waste_annual_ounces","Edulytiks-original supplementary yearly food-weight model: 100 ounces daily for 365 days","100 × 365",36500,"ounces per year"),
        new("usccss.g5.u04.l21.source_food_waste_annual_ounces","Edulytiks-original supplementary yearly food-weight model: 150 ounces daily for 365 days","150 × 365",54750,"ounces per year"),
        new("usccss.g5.u04.l21.source_food_waste_annual_ounces","Edulytiks-original supplementary yearly food-weight model: 200 ounces daily for 365 days","200 × 365",73000,"ounces per year"),
        new("usccss.g5.u04.l21.independent_group_annual_estimates","Edulytiks-original supplementary group annual estimate: 25 ounces daily over 365 days","25 × 365",9125,"ounces per year"),
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
            if(e.Context.StartsWith("Edulytiks-original",StringComparison.Ordinal))
            {
                prompt=$"{e.Context}: estimate {e.Expression} to the nearest thousand. "+
                    $"Select 1: {e.Result-1000}; 2: {e.Result+1000}; "+
                    $"3: {e.Result}; 4: {e.Result+2000}. "+
                    "Justify rounding and estimate with place value: TEACHER_REVIEW_REQUIRED.";
                solution=$"The closest thousand for {e.Expression} is {e.Result}; "+
                    "the selected estimate is a numerical subskill, not teacher-scored reasoning.";
            }
            else
            {
                prompt="IM2021 original Activity1: 18×149. Which estimate is most reasonable? "+
                    "Select 1: 2000; 2: 4000; 3: 3000; 4: 1500. "+
                    "Explain why your estimate is plausible and whether the exact product "+
                    "is more or less: TEACHER_REVIEW_REQUIRED.";
                solution="18×149=2682, closest to 3000 of the four original options; "+
                    "explain that 18×150≈2700 and the product is less than the selected 3000.";
            }
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
            prompt=$"{e.Context}. Calculate the {(e.Context.StartsWith("Edulytiks-original",StringComparison.Ordinal)?"supplementary":"source")} exact value of {e.Expression}. "+
                $"Select 1: {opts[0]}; 2: {opts[1]}; 3: {opts[2]}. "+
                "Multi-digit place-value reasoning and original estimate discussion are TEACHER_REVIEW_REQUIRED.";
            solution=$"The exact source-aligned product {e.Expression} is {correct} {e.Units}. "+
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
