using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Reuses the exact numeric engine, unit-scoped mathematics; Stage D NOT_RUN.
internal static class UaeGrade3Unit8Questions
{
 internal const string M6="uae.g3.u08.l01.multiply_six";
 internal const string M7="uae.g3.u08.l02.multiply_seven";
 internal const string D67="uae.g3.u08.l03.divide_six_seven";
 internal const string M8="uae.g3.u08.l04.multiply_eight";
 internal const string M9="uae.g3.u08.l05.multiply_nine";
 internal const string D89="uae.g3.u08.l06.divide_eight_nine";
 internal const string List="uae.g3.u08.l07.organized_list_combinations";
 internal const string M1112="uae.g3.u08.l08.multiply_eleven_twelve";
 internal const string D1112="uae.g3.u08.l09.divide_eleven_twelve";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f is M6 or M7 or D67 or M8 or M9 or D89 or List or M1112 or D1112;
 public static Item Build(string f, Random rng)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported G3 U08 family");
  int a=0,b=0,mode=0,result=0;
  string prompt,solution;
  switch(f)
  {
   case M6:case M7:case M8:case M9:case M1112:
    a=f switch{M6=>6,M7=>7,M8=>8,M9=>9,_=>rng.Next(11,13)};
    b=rng.Next(0,13);mode=rng.Next(3);result=a*b;
    prompt=mode switch{
     0=>$"ما حاصل ضرب {a} × {b}؟ أظهر كيف تعدّ مجموعات متساوية.",
     1=>$"هناك {b} أكياس فيها {a} قطع لكل كيس؛ كم قطعة بالمجموع؟",
     _=>$"ارسم مصفوفة من {a} صفوف و{b} أعمدة؛ ما عدد العناصر الكلي؟"};
    solution="أحسب حاصل ضرب عدد المجموعات في حجم الواحدة وأتحقق بالمصفوفة.";break;
   case D67:case D89:case D1112:
    a=f switch{D67=>rng.Next(6,8),D89=>rng.Next(8,10),_=>rng.Next(11,13)};
    b=rng.Next(0,13);mode=rng.Next(2);result=b;
    prompt=mode==0?$"كم يساوي {a*b} ÷ {a}؟ استعمل حقيقة ضرب معروفة.":
        $"لديك {a*b} قطعة؛ ضع {a} قطع في كل علبة. كم علبة؟";
    solution="أتحقق من أن العدد المطلوب في المقسوم عليه يساوي المقسوم.";break;
   case List:
    a=rng.Next(2,5);b=rng.Next(2,5);mode=rng.Next(2);result=a*b;
    prompt=mode==0?$"لديك {a} أنواع حلوى و{b} أنواع عصير. كم اختيارًا مختلفًا يمكن تكوينه إذا أخذت واحدًا من كل؟":
       $"يمكن اختيار قميص من {a} ألوان وبنطال من {b} ألوان. كم تنسيقًا مختلفًا؟ رتب القائمة دون تكرار.";
    solution="أكتب قائمة منظمة لكل اختيار أول مع كل اختيار ثانٍ وأتحقق بالضرب.";break;
   default:throw new InvalidOperationException("Unsupported G3 U08 family");
  }
  var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode},{"result",result}};
  return new Item(f,prompt,solution,AssessmentItemType.Numeric,p);
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int expected)
 {
  expected=0;
  if(!Supports(f)||p.Count!=4||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
     !p.TryGetValue("mode",out int mode)||!p.TryGetValue("result",out int got))return false;
  bool ok=f switch{
   M6=>a==6&&b>=0&&b<=12&&mode>=0&&mode<=2,
   M7=>a==7&&b>=0&&b<=12&&mode>=0&&mode<=2,
   M8=>a==8&&b>=0&&b<=12&&mode>=0&&mode<=2,
   M9=>a==9&&b>=0&&b<=12&&mode>=0&&mode<=2,
   M1112=>a>=11&&a<=12&&b>=0&&b<=12&&mode>=0&&mode<=2,
   D67=>a>=6&&a<=7&&b>=0&&b<=12&&mode>=0&&mode<=1,
   D89=>a>=8&&a<=9&&b>=0&&b<=12&&mode>=0&&mode<=1,
   D1112=>a>=11&&a<=12&&b>=0&&b<=12&&mode>=0&&mode<=1,
   List=>a>=2&&a<=4&&b>=2&&b<=4&&mode>=0&&mode<=1,
   _=>false};
  if(!ok)return false;
  expected=f is D67 or D89 or D1112?b:a*b;
  return got==expected;
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid Grade3 Unit8 parameters");
  return n.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
  var normalized=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
    c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
  return int.TryParse(normalized,NumberStyles.Integer,CultureInfo.InvariantCulture,out int n)&&n==expected;
 }
}
