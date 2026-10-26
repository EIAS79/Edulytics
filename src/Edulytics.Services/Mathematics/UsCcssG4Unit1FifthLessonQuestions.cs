using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exact numeric subskills of Grade4 IM2021 Unit1 Lesson5; source image and open justification remain teacher-reviewed.</summary>
internal static class UsCcssG4Unit1FifthLessonQuestions
{
 public const string Exact="usccss.g4.u01.l05.exact_seating";
 public const string Minimum="usccss.g4.u01.l05.minimum_packages";
 public const string Shared="usccss.g4.u01.l05.shared_multiples";
 public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f is Exact or Minimum or Shared;
 public static Problem Build(string f,Random random)
 {
  if(!Supports(f))throw new InvalidOperationException("Unknown G4L05 family");
  var p=new Dictionary<string,int>(StringComparer.Ordinal);
  string prompt;
  if(f==Exact)
  {
   int people=new[]{18,30,48,50}[random.Next(4)],size=random.Next(2)==0?6:8;
   p["need"]=people;p["size"]=size;
   prompt=$"Seat {people} people using ONLY {size}-seat tables with NO empty seats. Is this possible? Answer 1=yes, 0=no.";
  }
  else if(f==Minimum)
  {
   int need=random.Next(2)==0?50:72,size=random.Next(2)==0?8:10;
   p["need"]=need;p["size"]=size;
   prompt=$"You need AT LEAST {need} items sold only in packs of {size}. What minimum number of whole packs covers that need?";
  }
  else
  {
   int n=new[]{40,48,50,72,80}[random.Next(5)];p["need"]=n;p["size"]=0;
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
   Exact=>new[]{18,30,48,50}.Contains(n)&&(s is 6 or 8),
   Minimum=>(n is 50 or 72)&&(s is 8 or 10),
   Shared=>new[]{40,48,50,72,80}.Contains(n)&&s==0,
   _=>false
  };
 }
}
