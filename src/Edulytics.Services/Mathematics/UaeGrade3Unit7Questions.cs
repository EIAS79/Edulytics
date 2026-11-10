using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Extends the existing exact Practice engine with source-scoped G3-U07 mathematics.
// Stage D unit tests are written separately and have NOT_RUN during A-B-C.
internal static class UaeGrade3Unit7Questions
{
 internal const string Mul3="uae.g3.u07.l01.multiply_by_three";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f==Mul3;
 public static Item Build(string f,Random rng)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported Grade3 U07 family");
  int b=rng.Next(0,11),mode=rng.Next(3),total=3*b;
  string prompt=mode switch{
   0=>$"أوجد حاصل ٣ × {b} باستخدام مجموعات متساوية.",
   1=>$"يوجد {b} صناديق، في كل صندوق ثلاث قطع. كم قطعة بالمجموع؟",
   _=>$"اقفز {b} قفزات طول كل قفزة ٣ من الصفر. ما نقطة الوصول على خط الأعداد؟"
  };
  var p=new Dictionary<string,int>{{"a",3},{"b",b},{"mode",mode},{"result",total}};
  return new Item(f,prompt,"أمثل الضرب في ثلاثات بمصفوفة أو قفزات وأتحقق من الناتج.",AssessmentItemType.Numeric,p);
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int expected)
 {
  expected=0;
  if(!Supports(f)||p.Count!=4||!p.TryGetValue("a",out var a)||!p.TryGetValue("b",out var b)||
     !p.TryGetValue("mode",out var m)||!p.TryGetValue("result",out var result)||
     a!=3||b<0||b>10||m<0||m>2)return false;
  expected=3*b;
  return result==expected;
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p,out var n))throw new InvalidOperationException("Invalid G3 U07 numeric parameters");
  return n.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
  string normalized=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
  return int.TryParse(normalized,NumberStyles.Integer,CultureInfo.InvariantCulture,out int val)&&val==expected;
 }
}
