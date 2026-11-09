using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

// Single shared ExactSkillContractQuestionEngine extension for G3 U06.
// Families are activated through per-lesson contracts and registry entries, one lesson at a time.
// Stage D test suite is intentionally NOT_RUN during A-B-C.
internal static class UaeGrade3Unit6Questions
{
 internal const string Table="uae.g3.u06.l01.multiplication_table_pattern";
 internal const string Mul2="uae.g3.u06.l02.multiply_by_two";
 internal const string Div2="uae.g3.u06.l03.divide_by_two";
 internal const string Mul5="uae.g3.u06.l04.multiply_by_five";
 internal const string Div5="uae.g3.u06.l05.divide_by_five";
 internal const string Pattern="uae.g3.u06.l06.find_mul_div_pattern";
 internal const string Mul10="uae.g3.u06.l07.multiply_by_ten";
 internal const string MulTens="uae.g3.u06.l08.multiply_by_tens";
 internal const string Div10="uae.g3.u06.l09.divide_by_ten";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f) => f is Table or Mul2 or Div2 or Mul5 or Div5 or Pattern or Mul10 or MulTens or Div10;

 public static Item Build(string family,Random random)
 {
    if(!Supports(family))throw new InvalidOperationException("Unknown Grade 3 U06 family");
    int a=0,b=0,mode=0,value=0;
    string prompt;
    switch(family)
    {
     case Table:
       mode=random.Next(3);a=random.Next(mode==1?1:0,11);b=random.Next(mode==2?1:0,mode==2?10:11);
       value=mode==0?a*b:mode==1?b:a*(b+1);
       prompt=mode switch{
        0=>$"في جدول الضرب من ٠ إلى ١٠، ما ناتج تقاطع الصف {a} والعمود {b}؟",
        1=>$"في جدول الضرب أوجد العامل المجهول: {a} × □ = {a*b}؟",
        _=>$"أكمل نمط صف {a} من جدول الضرب: {a*(b-1<0?0:b-1)}، {a*b}، □. أضف {a} للانتقال إلى العمود التالي."
       };
       break;
     case Mul2:
       a=2;b=random.Next(0,11);value=2*b;mode=random.Next(2);
       prompt=mode==0?$"ما ناتج ضرب {b} × ٢؟":$"توجد {b} مجموعات، في كل مجموعة قطعتان. كم قطعة في المجموع؟";
       break;
     case Div2:
       a=2;b=random.Next(0,11);value=b;mode=random.Next(2);
       prompt=mode==0?$"قسّم {2*b} إلى مجموعتين متساويتين. كم قطعة في كل مجموعة؟":$"لديك {2*b} قطعة، وتضع قطعتين في كل علبة. كم علبة تحتاج؟";
       break;
     case Mul5:
       a=5;b=random.Next(0,11);value=5*b;mode=random.Next(2);
       prompt=mode==0?$"احسب حاصل الضرب ٥ × {b}.":$"مع {b} مجموعات فيها خمس قطع لكل مجموعة، ما عدد القطع الكلي؟";
       break;
     case Div5:
       a=5;b=random.Next(0,11);value=b;mode=random.Next(2);
       prompt=mode==0?$"اقسم {5*b} على ٥: كم جزءًا في كل مجموعة؟":$"لديك {5*b} قطعة، ووضعت خمس قطع في كل علبة. كم علبة؟";
       break;
     case Pattern:
       mode=random.Next(3);a=new[]{2,5,10}[mode];b=random.Next(0,8);value=(b+3)*a;
       prompt=$"ابحث عن قاعدة هذا النمط ثم أكمله: {b*a}، {(b+1)*a}، {(b+2)*a}، □.";
       break;
     case Mul10:
       a=10;b=random.Next(0,11);value=10*b;mode=random.Next(2);
       prompt=mode==0?$"ما حاصل ضرب {b} × ١٠؟":$"لدى {b} مجموعات عشر قطع في كل مجموعة. كم عدد القطع؟";
       break;
     case MulTens:
       a=random.Next(2,10);b=random.Next(1,10)*10;value=a*b;mode=random.Next(2);
       prompt=mode==0?$"استخدم حقائق الضرب لإيجاد {a} × {b}.":$"يوجد {a} صفوف، وفي كل صف {b} قطعة. كم قطعة؟";
       break;
     case Div10:
       a=10;b=random.Next(0,11);value=b;mode=random.Next(2);
       prompt=mode==0?$"ما ناتج {10*b} ÷ ١٠؟":$"يوجد {10*b} قطعة، وتُوضع عشر قطع في كل علبة. كم علبة؟";
       break;
     default:throw new InvalidOperationException("Unsupported family");
    }
    var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode},{"result",value}};
    return new Item(family,prompt,"استخدم مجموعات متساوية وخط أعداد وجدول الضرب، ثم تحقق من علاقة الضرب بالقسمة.",AssessmentItemType.Numeric,p);
 }
 private static bool Valid(string? family,IReadOnlyDictionary<string,int> p,out int expected)
 {
    expected=0;
    if(!Supports(family)||p.Count!=4||
       !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
       !p.TryGetValue("mode",out int mode)||!p.TryGetValue("result",out int result))return false;
    bool okay=family switch
    {
     Table => a is >=0 and <=10 && b is >=0 and <=10 && mode is >=0 and <=2 &&
              (mode!=1||a>0)&&(mode!=2||b<10),
     Mul2 => a==2&&b is >=0 and <=10&&mode is >=0 and <=1,
     Div2 => a==2&&b is >=0 and <=10&&mode is >=0 and <=1,
     Mul5 => a==5&&b is >=0 and <=10&&mode is >=0 and <=1,
     Div5 => a==5&&b is >=0 and <=10&&mode is >=0 and <=1,
     Pattern => mode is >=0 and <=2 && a==(mode==0?2:mode==1?5:10)&&b is >=0 and <=7,
     Mul10 => a==10&&b is >=0 and <=10&&mode is >=0 and <=1,
     MulTens => a is >=2 and <=9 && b is >=10 and <=90 && b%10==0 && mode is >=0 and <=1,
     Div10 => a==10&&b is >=0 and <=10&&mode is >=0 and <=1,
     _ => false
    };
    if(!okay)return false;
    expected=family switch{
       Table => mode==0?a*b:mode==1?b:a*(b+1),
       Div2 or Div5 or Div10 => b,
       Pattern => (b+3)*a,
       _ => a*b
    };
    return result==expected;
 }
 public static string Solve(string family,IReadOnlyDictionary<string,int> p)
 {
    if(!Valid(family,p,out int answer))throw new InvalidOperationException("Invalid U06 numeric parameters");
    return answer.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)
 {
    if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
    var text=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
       c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
    return int.TryParse(text,NumberStyles.Integer,CultureInfo.InvariantCulture,out int val)&&val==expected;
 }
}
