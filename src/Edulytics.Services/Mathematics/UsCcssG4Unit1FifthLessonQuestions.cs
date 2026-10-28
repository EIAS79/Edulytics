using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exact numeric subskills of Grade4 IM2021 Unit1 Lesson5; source image and open justification remain teacher-reviewed.</summary>
internal static class UsCcssG4Unit1FifthLessonQuestions
{
 public const string Exact="usccss.g4.u01.l05.exact_seating";
 public const string Minimum="usccss.g4.u01.l05.minimum_packages";
 public const string Shared="usccss.g4.u01.l05.shared_multiples";
 // Source-goal-preserving parameter pools: more than ten distinct, grade-appropriate
 // mathematical instances per family. The earlier 8/4/5-item pools were unable
 // to meet the mandatory ten-question non-repetition session contract.
 private static readonly int[] ExactPeople = [18,24,30,32,36,40,42,48,50,54,56,60,64,72,80,84,90,96];
 private static readonly int[] MinimumNeeds = [24,30,36,40,42,48,50,60,64,72,80,84,90,96,100,108];
 private static readonly int[] SharedCounts = [20,24,30,32,36,40,48,50,56,60,64,70,72,80,88,90,96,100,104,110,112,120,128,140,144,160,168,180,200];
 public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f is Exact or Minimum or Shared;
 public static Problem Build(string f,Random random)
 {
  if(!Supports(f))throw new InvalidOperationException("Unknown G4L05 family");
  var p=new Dictionary<string,int>(StringComparer.Ordinal);
  string prompt;
  if(f==Exact)
  {
   int people=ExactPeople[random.Next(ExactPeople.Length)],size=random.Next(2)==0?6:8;
   p["need"]=people;p["size"]=size;
   prompt=$"Seat {people} people using ONLY {size}-seat tables with NO empty seats. Is this possible? Answer 1=yes, 0=no.";
  }
  else if(f==Minimum)
  {
   int need=MinimumNeeds[random.Next(MinimumNeeds.Length)],size=random.Next(2)==0?8:10;
   p["need"]=need;p["size"]=size;
   prompt=$"You need AT LEAST {need} items sold only in packs of {size}. What minimum number of whole packs covers that need?";
  }
  else
  {
   int n=SharedCounts[random.Next(SharedCounts.Length)];p["need"]=n;p["size"]=0;
   prompt=$"Can EXACTLY {n} hotdogs from packs of 10 AND exactly {n} buns from packs of 8 be bought with no extras? Answer 1=yes, 0=no.";
  }
  return new Problem(f,prompt,"Use exact integer division for exact situations; ceiling for at-least packages.",AssessmentItemType.Numeric,p);
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p))throw new InvalidOperationException("Invalid G4L05 parameters");
  int n=p["need"],s=p["size"];
  int answer=f switch
  {
   Exact=>n%s==0?1:0,
   Minimum=>(n+s-1)/s,
   Shared=>n%8==0&&n%10==0?1:0,
   _=>throw new InvalidOperationException("Unknown source family")
  };
  return answer.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)=>
  Valid(f,p)&&string.Equals(Solve(f,p),answer,StringComparison.Ordinal);
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)
 {
  if(!Supports(f)||p.Count!=2||!p.TryGetValue("need",out var n)||!p.TryGetValue("size",out var s))return false;
  return f switch
  {
   Exact=>ExactPeople.Contains(n)&&(s is 6 or 8),
   Minimum=>MinimumNeeds.Contains(n)&&(s is 8 or 10),
   Shared=>SharedCounts.Contains(n)&&s==0,
   _=>false
  };
 }
}
