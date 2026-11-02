using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Checks maths only, not actual rulers, line drawings, number cards or game winners.</summary>
public static class UsCcssG2Unit3ThirteenthLessonCenters
{
 public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L13";
 public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
 public static readonly IReadOnlyList<string> TeacherRubric=[
  "Number Talk58+10=68,58+12=70,58+13=71,67+14=81; explain compose-ten",
  "Target Measurements Stage1: choose whole inch target≤10in or whole centimeter target≤30cm",
  "One physical partner draws with straightedge, other calls STOP; measure with matching standard ruler aligned to zero",
  "Score absolute actual-versus-target length difference for STOP partner in SAME unit",
  "Authentic turn exchange for EIGHT rounds, sum separate player scores, lower total wins",
  "Number Puzzles Stage4 uses actual digit cards and original gameboard to make equations true WITH composing/decomposing",
  "Center choice includes prior Estimate and Measure or Target Measurements; teacher observes real work"
 ];
 private static bool N(string? s,int lo,int hi,out int n)=>int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=lo&&n<=hi;
 public static Review Talk(string? a,string? b,string? c,string? d){bool ok=N(a,0,100,out var x)&&x==68&&N(b,0,100,out var y)&&y==70&&N(c,0,100,out var z)&&z==71&&N(d,0,100,out var w)&&w==81;return new(ok,true,ok?"Warm-up values68,70,71,81; teacher checks make-ten mental strategy.":"58+10=68;58+12=70;58+13=71;67+14=81.");}
 public static Review Measure(string? target,string? actual,string? unit,string? score){bool inches=unit=="in",cm=unit=="cm";var good=(inches||cm)&&N(target,1,inches?10:30,out var t)&&N(actual,0,100,out var a)&&N(score,0,100,out var s)&&s==Math.Abs(a-t);return new(good,true,good?"Correct same-unit absolute deviation; teacher checks real ruler placement, partner STOP and eight rounds.":"Use target1..10 inches OR1..30 centimeters, measure same unit and score absolute difference.");}
 public static Review Puzzles(string? sum,string? difference){bool ok=N(sum,0,100,out var a)&&a==72&&N(difference,0,100,out var b)&&b==35;return new(ok,true,ok?"Original Edulytics47+25=72,63−28=35 are mathematically true; official cards need teacher.":"47+25=72 composes a ten;63−28=35 decomposes a ten.");}
 public static Review Exit(string? difference,string? sum){bool ok=N(difference,0,100,out var d)&&d==3&&N(sum,0,100,out var s)&&s==82;return new(ok,false,ok?"Independent12cm/15cm deviation3cm and54+28=82.":"Compute |15−12|=3cm and54+28=82.");}
}
