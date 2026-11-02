using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>IM2021 G4 U01 L07 numeric checks ONLY; first-party statements and physical Stage2 game remain teacher-reviewed.</summary>
internal static class UsCcssG4Unit1SeventhLessonQuestions
{
 public const string Factor="usccss.g4.u01.l07.is_factor";
 public const string Multiple="usccss.g4.u01.l07.is_multiple";
 private static readonly int[] SourceTargets=[10,7,50,16,35,20,19,6];
 public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f is Factor or Multiple;
 public static Problem Build(string f,Random random)
 {
  if(!Supports(f))throw new InvalidOperationException("Unknown G4 L07 family");
  int target=SourceTargets[random.Next(SourceTargets.Length)];
  int candidate=random.Next(1,101);
  var p=new Dictionary<string,int>(StringComparer.Ordinal){{"target",target},{"candidate",candidate}};
  string prompt=f==Factor
  ?$"Is {candidate} a positive whole-number factor of {target}? Enter 1=yes, 0=no."
  :$"Is {candidate} a positive whole-number multiple of {target}? Enter 1=yes, 0=no.";
  return new Problem(f,prompt,"A divides B exactly iff B is a multiple of A.",AssessmentItemType.Numeric,p);
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p))throw new InvalidOperationException("Invalid original source bounds");
  var t=p["target"];var c=p["candidate"];
  int answer=f==Factor?(t%c==0?1:0):(c%t==0?1:0);
  return answer.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)=>
  Valid(f,p)&&string.Equals(Solve(f,p),answer,StringComparison.Ordinal);
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)=>
  Supports(f)&&p.Count==2&&p.TryGetValue("target",out var t)&&SourceTargets.Contains(t)
  &&p.TryGetValue("candidate",out var c)&&c>=1&&c<=100;
}
