using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Digitally gradeable source subskills; card sort, rectangles, classroom discussion and open explanations stay teacher-reviewed.</summary>
internal static class UsCcssG4Unit1ThirdLessonQuestions
{
 public const string Classification="usccss.g4.u01.l03.classify_area";
 public const string PairCount="usccss.g4.u01.l03.count_factor_pairs";
 public const string SideFactor="usccss.g4.u01.l03.valid_factor_side";
 private static readonly int[] SourceAreas=[1,2,10,48,11,21,23,60,32,42,31,56];
 public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f is Classification or PairCount or SideFactor;
 public static Problem Build(string family,Random random)
 {
  if(!Supports(family))throw new InvalidOperationException("Unknown G4 U01 L03 source family");
  var area=SourceAreas[random.Next(SourceAreas.Length)];
  var p=new Dictionary<string,int>(StringComparer.Ordinal){{"area",area}};
  string prompt;
  if(family==Classification)
    prompt=$"A whole-number rectangle has area {area} square units. Classify its area: 0 for neither, 1 for prime, 2 for composite.";
  else if(family==PairCount)
    prompt=$"How many DISTINCT unordered positive whole-number factor pairs (rotations counted once) make area {area} square units?";
  else
  {
    var candidate=random.Next(1,Math.Min(area+2,13));
    p["side"]=candidate;
    prompt=$"Can a whole-number-sided rectangle with area {area} square units have a side of length {candidate} units? 1 for yes, 0 for no.";
  }
  return new Problem(family,prompt,"Compute exact factor pairs and check whole-number divisibility.",AssessmentItemType.Numeric,p);
 }
 public static string Solve(string family,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(family,p))throw new InvalidOperationException("Invalid G4 L03 parameters");
  var a=p["area"],count=Enumerable.Range(1,(int)Math.Sqrt(a)).Count(x=>a%x==0);
  var answer=family switch
  {
    Classification => a==1?0:count==1?1:2,
    PairCount => count,
    SideFactor => a%p["side"]==0?1:0,
    _ => throw new InvalidOperationException("Bad G4 L03 family")
  };
  return answer.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
   Valid(family,p)&&string.Equals(Solve(family,p),answer,StringComparison.Ordinal);
 private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
 {
  if(!Supports(family)||!p.TryGetValue("area",out var a)||!SourceAreas.Contains(a))return false;
  return family==SideFactor
    ?p.Count==2&&p.TryGetValue("side",out var s)&&s>=1&&s<=Math.Min(a+1,12)
    :p.Count==1;
 }
}
