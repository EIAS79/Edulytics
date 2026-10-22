using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Original Edulytics exact questions for 21 IM2021 G2 Unit6 lessons; no inaccessible publisher graphics or physical evidence.</summary>
internal static class UsCcssG2Unit6ExactQuestions
{
    private static readonly IReadOnlySet<string> Families=new HashSet<string>(StringComparer.Ordinal){
        "usccss.g2.u06.l01.triangle_sides",
        "usccss.g2.u06.l01.quadrilateral_vertices",
        "usccss.g2.u06.l01.pentagon_sides",
        "usccss.g2.u06.l01.hexagon_vertices",
        "usccss.g2.u06.l02.close_quadrilateral",
        "usccss.g2.u06.l02.close_pentagon",
        "usccss.g2.u06.l02.close_hexagon",
        "usccss.g2.u06.l02.vertices_for_sides",
        "usccss.g2.u06.l03.perimeter_square",
        "usccss.g2.u06.l03.rectangle_perimeter",
        "usccss.g2.u06.l03.triangle_three_sides",
        "usccss.g2.u06.l03.two_same_sides",
        "usccss.g2.u06.l04.cube_faces",
        "usccss.g2.u06.l04.cube_vertices",
        "usccss.g2.u06.l04.cube_edges",
        "usccss.g2.u06.l04.triangular_prism_faces",
        "usccss.g2.u06.l05.sum_tens_and_ones",
        "usccss.g2.u06.l05.tens_exchange",
        "usccss.g2.u06.l05.polygon_sides",
        "usccss.g2.u06.l05.add_place_value",
        "usccss.g2.u06.l06.equal_square_tiles",
        "usccss.g2.u06.l06.two_trapezoids",
        "usccss.g2.u06.l06.four_square_tiles",
        "usccss.g2.u06.l06.composite_rectangle_perimeter",
        "usccss.g2.u06.l07.halves_in_whole",
        "usccss.g2.u06.l07.thirds_in_whole",
        "usccss.g2.u06.l07.fourths_in_whole",
        "usccss.g2.u06.l07.pieces_in_groups",
        "usccss.g2.u06.l08.halve_equal_area",
        "usccss.g2.u06.l08.quarter_equal_area",
        "usccss.g2.u06.l08.three_wholes_in_fourths",
        "usccss.g2.u06.l08.number_talk_difference",
        "usccss.g2.u06.l09.thirds_rebuild_whole",
        "usccss.g2.u06.l09.halves_rebuild_whole",
        "usccss.g2.u06.l09.fourths_rebuild_whole",
        "usccss.g2.u06.l09.thirds_remaining",
        "usccss.g2.u06.l10.quarter_groups",
        "usccss.g2.u06.l10.two_quarter_groups",
        "usccss.g2.u06.l10.four_quarter_groups",
        "usccss.g2.u06.l10.optional_center_tens",
        "usccss.g2.u06.l11.quarter_past_minute",
        "usccss.g2.u06.l11.half_past_minute",
        "usccss.g2.u06.l11.quarter_to_minute",
        "usccss.g2.u06.l11.hour_mark_to_minutes",
        "usccss.g2.u06.l12.clock_five_steps",
        "usccss.g2.u06.l12.half_clock_minutes",
        "usccss.g2.u06.l12.clock_minute_nine",
        "usccss.g2.u06.l12.minute_label_05",
        "usccss.g2.u06.l13.clock_minute_at_three",
        "usccss.g2.u06.l13.clock_minute_at_six",
        "usccss.g2.u06.l13.clock_minute_at_nine",
        "usccss.g2.u06.l13.clock_count_by_five",
        "usccss.g2.u06.l14.center_addition",
        "usccss.g2.u06.l14.center_three_addends",
        "usccss.g2.u06.l14.center_tens",
        "usccss.g2.u06.l14.polygon_side_count",
        "usccss.g2.u06.l15.penny_value",
        "usccss.g2.u06.l15.nickel_value",
        "usccss.g2.u06.l15.dime_value",
        "usccss.g2.u06.l15.three_coins_total",
        "usccss.g2.u06.l16.quarter_value",
        "usccss.g2.u06.l16.mixed_coin_total",
        "usccss.g2.u06.l16.quarters_and_dimes",
        "usccss.g2.u06.l16.make_66_fewest",
        "usccss.g2.u06.l17.four_quarters_one_dollar",
        "usccss.g2.u06.l17.ten_dimes_one_dollar",
        "usccss.g2.u06.l17.nickels_to_dollar",
        "usccss.g2.u06.l17.remaining_to_dollar",
        "usccss.g2.u06.l18.source_cost_pencils",
        "usccss.g2.u06.l18.fictional_money_remaining",
        "usccss.g2.u06.l18.fictional_money_combined",
        "usccss.g2.u06.l18.add_two_shop_items",
        "usccss.g2.u06.l19.source_whole_part_gap",
        "usccss.g2.u06.l19.two_step_add_subtract",
        "usccss.g2.u06.l19.mental_make_ten",
        "usccss.g2.u06.l19.two_step_money_gap",
        "usccss.g2.u06.l20.optional_center_sum",
        "usccss.g2.u06.l20.optional_coin_comparison",
        "usccss.g2.u06.l20.center_make_ten",
        "usccss.g2.u06.l20.money_remaining",
        "usccss.g2.u06.l21.four_pattern_blocks",
        "usccss.g2.u06.l21.at_least_hexagon",
        "usccss.g2.u06.l21.fictional_block_cost",
        "usccss.g2.u06.l21.swap_price_difference"
    };
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is not null&&Families.Contains(f);
    private static int Lesson(string f)=>int.Parse(f.Substring("usccss.g2.u06.l".Length,2),CultureInfo.InvariantCulture);
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown Grade2 U06 first-party family");
        int a=random.Next(1,7),b=random.Next(2,9),c=random.Next(0,10),p=random.Next(0,5),
            n=random.Next(0,4),d=random.Next(0,3),q=random.Next(0,3),
            paid=random.Next(60,101),price=random.Next(10,51);
        var data=new Dictionary<string,int>(StringComparer.Ordinal){
            ["lesson"]=Lesson(family),["a"]=a,["b"]=b,["c"]=c,["p"]=p,
            ["n"]=n,["d"]=d,["q"]=q,["paid"]=paid,["price"]=price
        };
        int value;string prompt;
        switch(family)
        {
            case "usccss.g2.u06.l01.triangle_sides":
                value=3;
                prompt=$"FIRST-PARTY — How many straight sides has a triangle?";
                break;
            case "usccss.g2.u06.l01.quadrilateral_vertices":
                value=4;
                prompt=$"FIRST-PARTY — How many corners does a quadrilateral have?";
                break;
            case "usccss.g2.u06.l01.pentagon_sides":
                value=5;
                prompt=$"FIRST-PARTY — How many straight sides make a pentagon?";
                break;
            case "usccss.g2.u06.l01.hexagon_vertices":
                value=6;
                prompt=$"FIRST-PARTY — How many vertices has a hexagon?";
                break;
            case "usccss.g2.u06.l02.close_quadrilateral":
                value=4;
                prompt=$"FIRST-PARTY — How many straight segments must CLOSE a quadrilateral?";
                break;
            case "usccss.g2.u06.l02.close_pentagon":
                value=5;
                prompt=$"FIRST-PARTY — How many straight segments must CLOSE a pentagon?";
                break;
            case "usccss.g2.u06.l02.close_hexagon":
                value=6;
                prompt=$"FIRST-PARTY — How many straight segments must CLOSE a hexagon?";
                break;
            case "usccss.g2.u06.l02.vertices_for_sides":
                value=a+2;
                prompt=$"FIRST-PARTY — A first-party closed polygon is drawn with {a+2} straight sides. How many corners does it have?";
                break;
            case "usccss.g2.u06.l03.perimeter_square":
                value=4*a;
                prompt=$"FIRST-PARTY — First-party FOUR-side shape has EACH side {a} cm. What is its perimeter in cm?";
                break;
            case "usccss.g2.u06.l03.rectangle_perimeter":
                value=2*(a+b);
                prompt=$"FIRST-PARTY — First-party RECTANGLE sides are {a} cm and {b} cm. What is perimeter (cm)?";
                break;
            case "usccss.g2.u06.l03.triangle_three_sides":
                value=a+b+(a+b-1);
                prompt=$"FIRST-PARTY — A valid constructed triangle has side lengths {a},{b},{a+b-1} cm. How many cm total perimeter?";
                break;
            case "usccss.g2.u06.l03.two_same_sides":
                value=2*a;
                prompt=$"FIRST-PARTY — A made-up drawing has TWO sides of {a} cm each. How many cm for these two sides combined (not the full perimeter)?";
                break;
            case "usccss.g2.u06.l04.cube_faces":
                value=6;
                prompt=$"FIRST-PARTY — How many flat square faces has a cube?";
                break;
            case "usccss.g2.u06.l04.cube_vertices":
                value=8;
                prompt=$"FIRST-PARTY — How many corners (vertices) on a cube?";
                break;
            case "usccss.g2.u06.l04.cube_edges":
                value=12;
                prompt=$"FIRST-PARTY — How many straight edges does a cube have?";
                break;
            case "usccss.g2.u06.l04.triangular_prism_faces":
                value=5;
                prompt=$"FIRST-PARTY — How many flat faces does a triangular prism have?";
                break;
            case "usccss.g2.u06.l05.sum_tens_and_ones":
                value=10*a+b;
                prompt=$"FIRST-PARTY — FICTIONAL mental addition: {10*a}+{b} = ?";
                break;
            case "usccss.g2.u06.l05.tens_exchange":
                value=10*a;
                prompt=$"FIRST-PARTY — FICTIONAL {a} rods of TEN ones each have how many ones?";
                break;
            case "usccss.g2.u06.l05.polygon_sides":
                value=a+2;
                prompt=$"FIRST-PARTY — An original Edulytics card describes a closed polygon with {a+2} sides. How many corners?";
                break;
            case "usccss.g2.u06.l05.add_place_value":
                value=100*a+10*b+c;
                prompt=$"FIRST-PARTY — FICTIONAL number: {a} hundreds, {b} tens and {c} ones. What numeral?";
                break;
            case "usccss.g2.u06.l06.equal_square_tiles":
                value=a*b;
                prompt=$"FIRST-PARTY — FICTIONAL composition uses {a} rows of {b} equal small squares. How many squares total?";
                break;
            case "usccss.g2.u06.l06.two_trapezoids":
                value=2;
                prompt=$"FIRST-PARTY — Original source-compatible fact: how many equal trapezoid tiles may make one hexagon (teacher inspects real drawing)?";
                break;
            case "usccss.g2.u06.l06.four_square_tiles":
                value=4;
                prompt=$"FIRST-PARTY — How many equal small square tiles form a 2-by-2 larger square?";
                break;
            case "usccss.g2.u06.l06.composite_rectangle_perimeter":
                value=2*(a+b);
                prompt=$"FIRST-PARTY — FICTIONAL rectangle of unit squares has {a} units across and {b} units down; outer perimeter in unit edges?";
                break;
            case "usccss.g2.u06.l07.halves_in_whole":
                value=2;
                prompt=$"FIRST-PARTY — One whole partitioned into equal halves has how many equal pieces?";
                break;
            case "usccss.g2.u06.l07.thirds_in_whole":
                value=3;
                prompt=$"FIRST-PARTY — One whole partitioned into equal thirds has how many equal pieces?";
                break;
            case "usccss.g2.u06.l07.fourths_in_whole":
                value=4;
                prompt=$"FIRST-PARTY — One whole partitioned into equal fourths has how many equal pieces?";
                break;
            case "usccss.g2.u06.l07.pieces_in_groups":
                value=3*a;
                prompt=$"FIRST-PARTY — An independent rectangle group has {a} shapes, each split into THREE equal-area shares. How many thirds pieces altogether?";
                break;
            case "usccss.g2.u06.l08.halve_equal_area":
                value=2;
                prompt=$"FIRST-PARTY — How many equal-area pieces must represent halves of one square?";
                break;
            case "usccss.g2.u06.l08.quarter_equal_area":
                value=4;
                prompt=$"FIRST-PARTY — How many equal-area pieces must represent fourths of a square?";
                break;
            case "usccss.g2.u06.l08.three_wholes_in_fourths":
                value=4*a;
                prompt=$"FIRST-PARTY — FIRST-PARTY {a} rectangles each partitioned into4 equal quarters. Total quarter pieces?";
                break;
            case "usccss.g2.u06.l08.number_talk_difference":
                value=10*a-5;
                prompt=$"FIRST-PARTY — FIRST-PARTY tens-and-fives arithmetic: {10*a+5}−10 = ?";
                break;
            case "usccss.g2.u06.l09.thirds_rebuild_whole":
                value=3;
                prompt=$"FIRST-PARTY — One pizza cut in THREE EQUAL parts: how many thirds make the whole?";
                break;
            case "usccss.g2.u06.l09.halves_rebuild_whole":
                value=2;
                prompt=$"FIRST-PARTY — How many halves make ONE whole equal-area pizza?";
                break;
            case "usccss.g2.u06.l09.fourths_rebuild_whole":
                value=4;
                prompt=$"FIRST-PARTY — How many fourths make a whole?";
                break;
            case "usccss.g2.u06.l09.thirds_remaining":
                value=Math.Max(0,3-c%4);
                prompt=$"FIRST-PARTY — FIRST-PARTY pizza has3 equal slices; pretend {c%4} slices eaten. How many WHOLE third-slices remain?";
                break;
            case "usccss.g2.u06.l10.quarter_groups":
                value=25*a;
                prompt=$"FIRST-PARTY — FIRST-PARTY groups of 25: {a} equal groups represent what total?";
                break;
            case "usccss.g2.u06.l10.two_quarter_groups":
                value=50;
                prompt=$"FIRST-PARTY — Two groups of25 objects each make how many objects?";
                break;
            case "usccss.g2.u06.l10.four_quarter_groups":
                value=100;
                prompt=$"FIRST-PARTY — Four groups of25 objects each make how many?";
                break;
            case "usccss.g2.u06.l10.optional_center_tens":
                value=10*(a+b);
                prompt=$"FIRST-PARTY — FICTIONAL optional center: add {10*a} and {10*b}. What total?";
                break;
            case "usccss.g2.u06.l11.quarter_past_minute":
                value=15;
                prompt=$"FIRST-PARTY — At QUARTER PAST an hour, how many minutes after the hour?";
                break;
            case "usccss.g2.u06.l11.half_past_minute":
                value=30;
                prompt=$"FIRST-PARTY — At HALF PAST an hour, how many minutes after the hour?";
                break;
            case "usccss.g2.u06.l11.quarter_to_minute":
                value=45;
                prompt=$"FIRST-PARTY — At QUARTER TO the NEXT hour, what minute number is shown?";
                break;
            case "usccss.g2.u06.l11.hour_mark_to_minutes":
                value=5*a;
                prompt=$"FIRST-PARTY — FICTIONAL minute hand points at numeral {a}. How many minutes past the hour if labels are5 minutes apart?";
                break;
            case "usccss.g2.u06.l12.clock_five_steps":
                value=5*a;
                prompt=$"FIRST-PARTY — FICTIONAL clock minute hand at number {a}; elapsed minutes since12?";
                break;
            case "usccss.g2.u06.l12.half_clock_minutes":
                value=30;
                prompt=$"FIRST-PARTY — Minute hand pointing to6 shows how many minutes?";
                break;
            case "usccss.g2.u06.l12.clock_minute_nine":
                value=45;
                prompt=$"FIRST-PARTY — At number9 on an analog minute hand, how many minutes past?";
                break;
            case "usccss.g2.u06.l12.minute_label_05":
                value=5;
                prompt=$"FIRST-PARTY — The digital clock minute field for hand at1 is05. What is its numeric MINUTES value?";
                break;
            case "usccss.g2.u06.l13.clock_minute_at_three":
                value=15;
                prompt=$"FIRST-PARTY — Clock minute hand at3: how many minutes?";
                break;
            case "usccss.g2.u06.l13.clock_minute_at_six":
                value=30;
                prompt=$"FIRST-PARTY — Clock minute hand at6: how many minutes?";
                break;
            case "usccss.g2.u06.l13.clock_minute_at_nine":
                value=45;
                prompt=$"FIRST-PARTY — Clock minute hand at9: how many minutes?";
                break;
            case "usccss.g2.u06.l13.clock_count_by_five":
                value=5*a;
                prompt=$"FIRST-PARTY — FIRST-PARTY clock minute hand at {a}: numerical minutes after the hour? Family routine AM/PM requires teacher explanation.";
                break;
            case "usccss.g2.u06.l14.center_addition":
                value=a*10+b*5;
                prompt=$"FIRST-PARTY — FICTIONAL optional center: {a*10}+{b*5} = ?";
                break;
            case "usccss.g2.u06.l14.center_three_addends":
                value=a+b+c;
                prompt=$"FIRST-PARTY — FICTIONAL center three-card values {a},{b},{c}: sum?";
                break;
            case "usccss.g2.u06.l14.center_tens":
                value=10*(a+b);
                prompt=$"FIRST-PARTY — FICTIONAL mental sum {10*a}+{10*b} = ?";
                break;
            case "usccss.g2.u06.l14.polygon_side_count":
                value=a+2;
                prompt=$"FIRST-PARTY — FICTIONAL completed {a+2}-sided polygon: how many straight sides?";
                break;
            case "usccss.g2.u06.l15.penny_value":
                value=p;
                prompt=$"FIRST-PARTY — FICTIONAL {p} US pennies each worth1 cent. Total cents?";
                break;
            case "usccss.g2.u06.l15.nickel_value":
                value=5*n;
                prompt=$"FIRST-PARTY — FICTIONAL {n} US nickels each worth5 cents. Total cents?";
                break;
            case "usccss.g2.u06.l15.dime_value":
                value=10*d;
                prompt=$"FIRST-PARTY — FICTIONAL {d} US dimes each worth10 cents. Total cents?";
                break;
            case "usccss.g2.u06.l15.three_coins_total":
                value=p+5*n+10*d;
                prompt=$"FIRST-PARTY — FICTIONAL collection has {p} pennies, {n} nickels and {d} dimes. Total cents?";
                break;
            case "usccss.g2.u06.l16.quarter_value":
                value=25*q;
                prompt=$"FIRST-PARTY — FICTIONAL {q} quarters at25 cents each. Total cents?";
                break;
            case "usccss.g2.u06.l16.mixed_coin_total":
                value=p+5*n+10*d+25*q;
                prompt=$"FIRST-PARTY — FICTIONAL {q} quarters, {d} dimes, {n} nickels, {p} pennies. Total cents?";
                break;
            case "usccss.g2.u06.l16.quarters_and_dimes":
                value=25*q+10*d;
                prompt=$"FIRST-PARTY — FICTIONAL {q} quarters and {d} dimes. Combined cents?";
                break;
            case "usccss.g2.u06.l16.make_66_fewest":
                value=5;
                prompt=$"FIRST-PARTY — With US coins25,10,5,1 cents, fewest coins for66 cents is how many? Multiple actual combinations accepted in source drawings.";
                break;
            case "usccss.g2.u06.l17.four_quarters_one_dollar":
                value=4;
                prompt=$"FIRST-PARTY — How many 25-cent quarters make100 cents (one dollar)?";
                break;
            case "usccss.g2.u06.l17.ten_dimes_one_dollar":
                value=10;
                prompt=$"FIRST-PARTY — How many 10-cent dimes make100 cents?";
                break;
            case "usccss.g2.u06.l17.nickels_to_dollar":
                value=20;
                prompt=$"FIRST-PARTY — How many 5-cent nickels make100 cents?";
                break;
            case "usccss.g2.u06.l17.remaining_to_dollar":
                value=100-25*q-10*d-5*n-p;
                prompt=$"FIRST-PARTY — FICTIONAL jar contains {q} quarters, {d} dimes, {n} nickels, {p} pennies. How many cents are missing to make exactly100?";
                break;
            case "usccss.g2.u06.l18.source_cost_pencils":
                value=75;
                prompt=$"FIRST-PARTY — Official ORIGINAL price table: pack of pencils costs75 cents. How many cents?";
                break;
            case "usccss.g2.u06.l18.fictional_money_remaining":
                value=paid-price;
                prompt=$"FIRST-PARTY — FICTIONAL child starts with {paid} cents and buys item costing {price} cents. How many cents remain?";
                break;
            case "usccss.g2.u06.l18.fictional_money_combined":
                value=25*q+10*d+5*n+p;
                prompt=$"FIRST-PARTY — FICTIONAL coin values: {q} quarters, {d} dimes, {n} nickels and {p} pennies. How many cents total?";
                break;
            case "usccss.g2.u06.l18.add_two_shop_items":
                value=35+18;
                prompt=$"FIRST-PARTY — Publisher source price table: sharpener35c and pens18c; how many cents for BOTH? This separate add-on question is EDULYTICS.";
                break;
            case "usccss.g2.u06.l19.source_whole_part_gap":
                value=68-39;
                prompt=$"FIRST-PARTY — Source tape model total68 cents and known part39 cents; what unknown remaining part?";
                break;
            case "usccss.g2.u06.l19.two_step_add_subtract":
                value=paid-price+d;
                prompt=$"FIRST-PARTY — FICTIONAL learner owns {paid} cents, spends {price} cents, then receives {d} cents. How many cents now?";
                break;
            case "usccss.g2.u06.l19.mental_make_ten":
                value=10*a+b;
                prompt=$"FIRST-PARTY — FICTIONAL mental addition {10*a}+{b} = ?";
                break;
            case "usccss.g2.u06.l19.two_step_money_gap":
                value=paid-price-5;
                prompt=$"FIRST-PARTY — FICTIONAL {paid} cents, spend {price} cents and then5 cents. Remainder?";
                break;
            case "usccss.g2.u06.l20.optional_center_sum":
                value=10*a+5*b;
                prompt=$"FIRST-PARTY — FICTIONAL center sums {10*a} and {5*b}. What total?";
                break;
            case "usccss.g2.u06.l20.optional_coin_comparison":
                value=25*q+10*d;
                prompt=$"FIRST-PARTY — FICTIONAL {q} quarters and {d} dimes: cents total?";
                break;
            case "usccss.g2.u06.l20.center_make_ten":
                value=10*a+10*b;
                prompt=$"FIRST-PARTY — FICTIONAL center: {10*a}+{10*b} =?";
                break;
            case "usccss.g2.u06.l20.money_remaining":
                value=paid-price;
                prompt=$"FIRST-PARTY — FICTIONAL buy item costing{price} cents with{paid} cents. Change in cents?";
                break;
            case "usccss.g2.u06.l21.four_pattern_blocks":
                value=4;
                prompt=$"FIRST-PARTY — Original optional puzzle must contain exactly how many physical pattern blocks?";
                break;
            case "usccss.g2.u06.l21.at_least_hexagon":
                value=1;
                prompt=$"FIRST-PARTY — Original puzzle requires AT LEAST how many hexagonal blocks?";
                break;
            case "usccss.g2.u06.l21.fictional_block_cost":
                value=2*a+3*b;
                prompt=$"FIRST-PARTY — FICTIONAL Edulytics store: {a} small triangles at2 cents and {b} rhombuses at3 cents. Cost in cents? NOT publisher price table.";
                break;
            case "usccss.g2.u06.l21.swap_price_difference":
                value=2*a;
                prompt=$"FIRST-PARTY — FICTIONAL swap {a} block(s) of price5 cents for substitute3 cents each. How many cents saved? NOT original source price list.";
                break;
            default:throw new InvalidOperationException("Unknown G2 Unit6 question family");
        }
        return new(family,prompt,$"Edulytics-only exact solution {value}. Original open figures, games and drawings require teacher.",AssessmentItemType.Numeric,data);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> data)
    {
        if(!Valid(family,data))throw new InvalidOperationException("U06 numeric parameters invalid");
        int a=data["a"],b=data["b"],c=data["c"],p=data["p"],n=data["n"],d=data["d"],
            q=data["q"],paid=data["paid"],price=data["price"];
        int answer=family switch{
            "usccss.g2.u06.l01.triangle_sides"=>3,
            "usccss.g2.u06.l01.quadrilateral_vertices"=>4,
            "usccss.g2.u06.l01.pentagon_sides"=>5,
            "usccss.g2.u06.l01.hexagon_vertices"=>6,
            "usccss.g2.u06.l02.close_quadrilateral"=>4,
            "usccss.g2.u06.l02.close_pentagon"=>5,
            "usccss.g2.u06.l02.close_hexagon"=>6,
            "usccss.g2.u06.l02.vertices_for_sides"=>a+2,
            "usccss.g2.u06.l03.perimeter_square"=>4*a,
            "usccss.g2.u06.l03.rectangle_perimeter"=>2*(a+b),
            "usccss.g2.u06.l03.triangle_three_sides"=>a+b+(a+b-1),
            "usccss.g2.u06.l03.two_same_sides"=>2*a,
            "usccss.g2.u06.l04.cube_faces"=>6,
            "usccss.g2.u06.l04.cube_vertices"=>8,
            "usccss.g2.u06.l04.cube_edges"=>12,
            "usccss.g2.u06.l04.triangular_prism_faces"=>5,
            "usccss.g2.u06.l05.sum_tens_and_ones"=>10*a+b,
            "usccss.g2.u06.l05.tens_exchange"=>10*a,
            "usccss.g2.u06.l05.polygon_sides"=>a+2,
            "usccss.g2.u06.l05.add_place_value"=>100*a+10*b+c,
            "usccss.g2.u06.l06.equal_square_tiles"=>a*b,
            "usccss.g2.u06.l06.two_trapezoids"=>2,
            "usccss.g2.u06.l06.four_square_tiles"=>4,
            "usccss.g2.u06.l06.composite_rectangle_perimeter"=>2*(a+b),
            "usccss.g2.u06.l07.halves_in_whole"=>2,
            "usccss.g2.u06.l07.thirds_in_whole"=>3,
            "usccss.g2.u06.l07.fourths_in_whole"=>4,
            "usccss.g2.u06.l07.pieces_in_groups"=>3*a,
            "usccss.g2.u06.l08.halve_equal_area"=>2,
            "usccss.g2.u06.l08.quarter_equal_area"=>4,
            "usccss.g2.u06.l08.three_wholes_in_fourths"=>4*a,
            "usccss.g2.u06.l08.number_talk_difference"=>10*a-5,
            "usccss.g2.u06.l09.thirds_rebuild_whole"=>3,
            "usccss.g2.u06.l09.halves_rebuild_whole"=>2,
            "usccss.g2.u06.l09.fourths_rebuild_whole"=>4,
            "usccss.g2.u06.l09.thirds_remaining"=>Math.Max(0,3-c%4),
            "usccss.g2.u06.l10.quarter_groups"=>25*a,
            "usccss.g2.u06.l10.two_quarter_groups"=>50,
            "usccss.g2.u06.l10.four_quarter_groups"=>100,
            "usccss.g2.u06.l10.optional_center_tens"=>10*(a+b),
            "usccss.g2.u06.l11.quarter_past_minute"=>15,
            "usccss.g2.u06.l11.half_past_minute"=>30,
            "usccss.g2.u06.l11.quarter_to_minute"=>45,
            "usccss.g2.u06.l11.hour_mark_to_minutes"=>5*a,
            "usccss.g2.u06.l12.clock_five_steps"=>5*a,
            "usccss.g2.u06.l12.half_clock_minutes"=>30,
            "usccss.g2.u06.l12.clock_minute_nine"=>45,
            "usccss.g2.u06.l12.minute_label_05"=>5,
            "usccss.g2.u06.l13.clock_minute_at_three"=>15,
            "usccss.g2.u06.l13.clock_minute_at_six"=>30,
            "usccss.g2.u06.l13.clock_minute_at_nine"=>45,
            "usccss.g2.u06.l13.clock_count_by_five"=>5*a,
            "usccss.g2.u06.l14.center_addition"=>a*10+b*5,
            "usccss.g2.u06.l14.center_three_addends"=>a+b+c,
            "usccss.g2.u06.l14.center_tens"=>10*(a+b),
            "usccss.g2.u06.l14.polygon_side_count"=>a+2,
            "usccss.g2.u06.l15.penny_value"=>p,
            "usccss.g2.u06.l15.nickel_value"=>5*n,
            "usccss.g2.u06.l15.dime_value"=>10*d,
            "usccss.g2.u06.l15.three_coins_total"=>p+5*n+10*d,
            "usccss.g2.u06.l16.quarter_value"=>25*q,
            "usccss.g2.u06.l16.mixed_coin_total"=>p+5*n+10*d+25*q,
            "usccss.g2.u06.l16.quarters_and_dimes"=>25*q+10*d,
            "usccss.g2.u06.l16.make_66_fewest"=>5,
            "usccss.g2.u06.l17.four_quarters_one_dollar"=>4,
            "usccss.g2.u06.l17.ten_dimes_one_dollar"=>10,
            "usccss.g2.u06.l17.nickels_to_dollar"=>20,
            "usccss.g2.u06.l17.remaining_to_dollar"=>100-25*q-10*d-5*n-p,
            "usccss.g2.u06.l18.source_cost_pencils"=>75,
            "usccss.g2.u06.l18.fictional_money_remaining"=>paid-price,
            "usccss.g2.u06.l18.fictional_money_combined"=>25*q+10*d+5*n+p,
            "usccss.g2.u06.l18.add_two_shop_items"=>35+18,
            "usccss.g2.u06.l19.source_whole_part_gap"=>68-39,
            "usccss.g2.u06.l19.two_step_add_subtract"=>paid-price+d,
            "usccss.g2.u06.l19.mental_make_ten"=>10*a+b,
            "usccss.g2.u06.l19.two_step_money_gap"=>paid-price-5,
            "usccss.g2.u06.l20.optional_center_sum"=>10*a+5*b,
            "usccss.g2.u06.l20.optional_coin_comparison"=>25*q+10*d,
            "usccss.g2.u06.l20.center_make_ten"=>10*a+10*b,
            "usccss.g2.u06.l20.money_remaining"=>paid-price,
            "usccss.g2.u06.l21.four_pattern_blocks"=>4,
            "usccss.g2.u06.l21.at_least_hexagon"=>1,
            "usccss.g2.u06.l21.fictional_block_cost"=>2*a+3*b,
            "usccss.g2.u06.l21.swap_price_difference"=>2*a,
            _=>throw new InvalidOperationException("Unknown exact family")
        };
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> data,string? actual)=>
        Valid(family,data)&&string.Equals(actual,Solve(family,data),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> data)
    {
        if(family is null||!Supports(family)||data.Count!=10||
            !data.TryGetValue("lesson",out var l)||l!=Lesson(family)||
            !data.TryGetValue("a",out var a)||a<1||a>6||
            !data.TryGetValue("b",out var b)||b<2||b>8||
            !data.TryGetValue("c",out var c)||c<0||c>9||
            !data.TryGetValue("p",out var p)||p<0||p>4||
            !data.TryGetValue("n",out var n)||n<0||n>3||
            !data.TryGetValue("d",out var d)||d<0||d>2||
            !data.TryGetValue("q",out var q)||q<0||q>2||
            !data.TryGetValue("paid",out var paid)||paid<60||paid>100||
            !data.TryGetValue("price",out var price)||price<10||price>50)
            return false;
        return true;
    }
}
