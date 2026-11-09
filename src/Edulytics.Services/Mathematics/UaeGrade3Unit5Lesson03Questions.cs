using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Grade 3 repeated subtraction family uses current ExactSkillContractQuestionEngine; no new engine.
internal static class UaeGrade3Unit5Lesson03Questions
{
 internal const string Family="uae.g3.u05.l03.repeated_subtraction_division";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f==Family;
 public static Item Build(string f,Random rng)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported G3 repeated subtraction family");
  int divisor=rng.Next(2,11),quotient=rng.Next(2,11),total=divisor*quotient,mode=rng.Next(3);
  string prompt=mode switch {
   0=>$"ابدأ من {total} واطرح {divisor} حتى تصل إلى الصفر. كم مرة ستطرح؟",
   1=>$"على خط الأعداد، اقفز إلى الخلف من {total} إلى صفر بخطوات مقدارها {divisor}. كم قفزة؟",
   _=>$"أكمل القسمة بالطرح المتكرر: {total} ÷ {divisor} = ؟"
  };
  return new Item(f,prompt,
      "عدد خطوات الطرح المتساوية إلى الصفر هو ناتج القسمة؛ أتحقق بضرب عددها في حجم الخطوة.",
      AssessmentItemType.Numeric,new Dictionary<string,int>{{"divisor",divisor},{"quotient",quotient},{"total",total},{"mode",mode}});
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int result)
 {
  result=0;
  if(!Supports(f)||p.Count!=4||!p.TryGetValue("divisor",out int divisor)||divisor is <2 or >10||
    !p.TryGetValue("quotient",out int supplied)||supplied is <2 or >10||
    !p.TryGetValue("total",out int total)||total!=divisor*supplied||
    !p.TryGetValue("mode",out int mode)||mode is <0 or >2)return false;
  result=total/divisor;return result==supplied;
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p,out int result))throw new InvalidOperationException("Invalid subtract-to-zero parameters");
  return result.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!Valid(f,p,out int correct)||string.IsNullOrWhiteSpace(answer))return false;
  string digits=new(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
    c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
  return int.TryParse(digits,NumberStyles.Integer,CultureInfo.InvariantCulture,out int num)&&num==correct;
 }
}
