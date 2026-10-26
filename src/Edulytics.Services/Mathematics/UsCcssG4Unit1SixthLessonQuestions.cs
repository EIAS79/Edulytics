using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UsCcssG4Unit1SixthLessonQuestions
{
 public const string Touch="usccss.g4.u01.l06.student_touches";
 public const string Count="usccss.g4.u01.l06.touch_count";
 public const string Open="usccss.g4.u01.l06.end_open";
 public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f is Touch or Count or Open;
 public static Problem Build(string f,Random random)
 {
  if(!Supports(f))throw new InvalidOperationException("Unknown locker family");
  int locker=random.Next(1,21);
  var p=new Dictionary<string,int>(StringComparer.Ordinal){{"locker",locker}};
  string prompt;
  if(f==Touch)
  {
   int student=random.Next(1,21);p["student"]=student;
   prompt=$"Does student {student} touch locker {locker}? Enter 1 for yes,0 for no.";
  }
  else if(f==Count)prompt=$"How many distinct students touch locker {locker} after all twenty turns?";
  else prompt=$"All lockers start closed. Does locker {locker} end OPEN after all twenty turns? Enter 1 for yes,0 for no.";
  return new Problem(f,prompt,"Find all positive factors and use parity for final state.",AssessmentItemType.Numeric,p);
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p))throw new InvalidOperationException("Invalid G4 L06 parameters");
  int n=p["locker"],count=Enumerable.Range(1,n).Count(x=>n%x==0);
  int answer=f switch{Touch=>n%p["student"]==0?1:0,Count=>count,Open=>count%2,_=>throw new InvalidOperationException("Unknown family")};
  return answer.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)=>Valid(f,p)&&string.Equals(Solve(f,p),answer,StringComparison.Ordinal);
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)
 {
  if(!Supports(f)||!p.TryGetValue("locker",out int n)||n<1||n>20)return false;
  if(f==Touch)return p.Count==2&&p.TryGetValue("student",out int s)&&s>=1&&s<=20;
  return p.Count==1;
 }
}
