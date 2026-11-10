using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

// Reuses the existing ExactSkillContractQuestionEngine with G3 U07 lesson-specific families.
// Stage D tests are WRITTEN_NOT_RUN; this source file does not certify runtime execution.
internal static class UaeGrade3Unit7Questions
{
 internal const string Mul3="uae.g3.u07.l01.multiply_by_three";
 internal const string Div3="uae.g3.u07.l02.divide_by_three";
 internal const string Double="uae.g3.u07.l03.double_known_fact";
 internal const string Mul4="uae.g3.u07.l04.multiply_by_four";
 internal const string Div4="uae.g3.u07.l05.divide_by_four";
 internal const string Info="uae.g3.u07.l06.information_sufficient";
 internal const string Mul01="uae.g3.u07.l07.multiply_zero_one";
 internal const string Div01="uae.g3.u07.l08.divide_zero_one";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f is Mul3 or Div3 or Double or Mul4 or Div4 or Info or Mul01 or Div01;
 public static Item Build(string f, Random rng)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported G3 U07 family");
  int a=0,b=0,mode=0,answer=0; string prompt; string solution;
  switch(f)
  {
   case Mul3:
    a=3;b=rng.Next(0,11);mode=rng.Next(3);answer=3*b;
    prompt=mode switch{
     0=>$"أوجد ناتج ٣ × {b} بمجموعات متساوية.",
     1=>$"لدى {b} صناديق، في كل صندوق ثلاث قطع. ما عدد القطع؟",
     _=>$"على خط الأعداد قفزت {b} قفزات طول كل منها ٣ من الصفر؛ ما نقطة الوصول؟"};
    solution="أعد ثلاثات وأتحقق بمصفوفة أو جمع متكرر."; break;
   case Div3:
    a=3;b=rng.Next(0,11);mode=rng.Next(2);answer=b;
    prompt=mode==0?$"قسّم {3*b} قطعة على ٣ مجموعات متساوية. كم قطعة في كل مجموعة؟":
       $"لديك {3*b} قطعة، تضع ثلاث قطع في كل كيس. كم كيسًا تملأ؟";
    solution="أستعمل حقيقة ٣ × عدد المجموعات، وأراجع بالتوزيع.";break;
   case Double:
    a=4;b=rng.Next(1,11);mode=rng.Next(2);answer=4*b;
    prompt=mode==0?$"أعرف أن ٢ × {b} = {2*b}. ما ناتج ٤ × {b} بمضاعفة الحقيقة المعروفة؟":
       $"صنعت مصفوفتين، كل منهما صفان من {b} نقاط. كم نقطة في المصفوفتين؟";
    solution="أكرر الحقيقة ٢×ب مرتين وأحصل على ٤×ب.";break;
   case Mul4:
    a=4;b=rng.Next(0,11);mode=rng.Next(3);answer=4*b;
    prompt=mode switch{
     0=>$"ما ناتج ٤ × {b}؟",
     1=>$"يوجد {b} أكياس، بكل كيس أربع كرات. كم كرة؟",
     _=>$"ضاعف ٢ × {b} لتحصل على ٤ × {b}. ما الناتج؟"};
    solution="أجمع أربع مرات أو أضاعف حقيقة ضرب في اثنين.";break;
   case Div4:
    a=4;b=rng.Next(0,11);mode=rng.Next(2);answer=b;
    prompt=mode==0?$"احسب {4*b} ÷ ٤ بمجموعات متساوية.":
       $"توجد {4*b} ملصقًا ونضع أربعة في كل بطاقة. كم بطاقة نستخدم؟";
    solution="أستعمل ٤ × عدد المجموعات = المقسوم وأتحقق.";break;
   case Info:
    a=0;b=rng.Next(2,10);mode=rng.Next(2);answer=mode==0?1:0;
    prompt=mode==0?$"لدينا {b} علب، في كل علبة ٣ أقلام. لون العلب أزرق. هل المعطيات كافية لحساب عدد الأقلام؟ اكتب ١ لنعم، ٠ للا.":
       $"لدينا {b} علب أقلام ولم نعرف عدد الأقلام في العلبة. هل المعطيات كافية لحساب المجموع؟ اكتب ١ لنعم، ٠ للا.";
    solution="أحدد المطلوب وأميز المعطيات الضرورية من الزائدة أو الناقصة.";break;
   case Mul01:
    a=rng.Next(2);b=rng.Next(0,21);mode=rng.Next(2);answer=a*b;
    prompt=mode==0?$"احسب {a} × {b} باستعمال خاصية الصفر أو العنصر المحايد.":
       $"ما عدد القطع في {a} مجموعة، في كل مجموعة {b} قطعة؟";
    solution="أي عدد مضروب في الصفر يساوي صفرًا، ومضروب في ١ يساوي نفسه.";break;
   case Div01:
    mode=rng.Next(3);
    if(mode==0){a=rng.Next(1,13);b=0;answer=0;prompt=$"ما ناتج ٠ ÷ {a} حيث المقسوم عليه غير صفري؟";}
    else if(mode==1){a=1;b=rng.Next(0,21);answer=b;prompt=$"ما ناتج {b} ÷ ١؟";}
    else {a=rng.Next(1,13);b=a;answer=1;prompt=$"ما ناتج {a} ÷ {a}؟";}
    solution="القسمة على صفر غير معرفة؛ صفر على عدد غير صفري يساوي صفرًا، وعدد على نفسه غير الصفري يساوي ١.";break;
   default:throw new InvalidOperationException("Unsupported G3 U07 family");
  }
  var values=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode},{"result",answer}};
  return new Item(f,prompt,solution,AssessmentItemType.Numeric,values);
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int expected)
 {
  expected=0;
  if(!Supports(f)||p.Count!=4||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
     !p.TryGetValue("mode",out int m)||!p.TryGetValue("result",out int got))return false;
  bool ok=f switch{
   Mul3=>a==3&&b>=0&&b<=10&&m>=0&&m<=2,
   Div3=>a==3&&b>=0&&b<=10&&m>=0&&m<=1,
   Double=>a==4&&b>=1&&b<=10&&m>=0&&m<=1,
   Mul4=>a==4&&b>=0&&b<=10&&m>=0&&m<=2,
   Div4=>a==4&&b>=0&&b<=10&&m>=0&&m<=1,
   Info=>a==0&&b>=2&&b<=9&&m>=0&&m<=1,
   Mul01=>a>=0&&a<=1&&b>=0&&b<=20&&m>=0&&m<=1,
   Div01=>m switch{
    0=>a>=1&&a<=12&&b==0,
    1=>a==1&&b>=0&&b<=20,
    2=>a>=1&&a<=12&&a==b,
    _=>false},
   _=>false};
  if(!ok)return false;
  expected=f switch{
   Mul3 or Double or Mul4 or Mul01=>a*b,
   Div3 or Div4=>b,
   Info=>m==0?1:0,
   Div01=>m==0?0:m==1?b:1,
   _=>throw new InvalidOperationException("Invalid family")};
  return got==expected;
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p,out int answer))throw new InvalidOperationException("Invalid G3 U07 parameters");
  return answer.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
  string normalized=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
   c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
  return int.TryParse(normalized,NumberStyles.Integer,CultureInfo.InvariantCulture,out int val)&&val==expected;
 }
}
