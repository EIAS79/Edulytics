using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Grade3 data handling; rendering graphs requires teacher verification.
// Exact engine is reused. Stage D tests are source only, NOT_RUN in A-B-C.
internal static class UaeGrade3Unit12Questions
{
 internal const string Tally="uae.g3.u12.l01.tally_record";
 internal const string Picture="uae.g3.u12.l02.scaled_picture_graph";
 internal const string Bars="uae.g3.u12.l03.bar_graph";
 internal const string Mixed="uae.g3.u12.l04.picture_bar_graph_compare";
 internal const string Line="uae.g3.u12.l05.line_plot";
 internal const string Pattern="uae.g3.u12.l06.number_sense_pattern";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f is Tally or Picture or Bars or Mixed or Line or Pattern;
 public static Item Build(string f,Random rng)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported Grade3 data question");
  int a=0,b=0,mode=rng.Next(2),result=0;string prompt,solution;
  switch(f)
  {
   case Tally:
    a=rng.Next(4,13);b=rng.Next(1,a+1);result=mode==0?a+b:a-b;
    prompt=mode==0?$"في جدول علامات العد يوجد {a} صوتًا للخيار الأول و{b} للخيار الثاني. كم صوتًا بالمجموع؟":
     $"لدى الخيار الأول {a} علامات عد وللثاني {b}. بكم صوتًا يزيد الأول؟";
    solution="أعد إشارات العد مجموعات من خمسة، ثم أجمع أو أطرح أعداد الخيارات.";break;
   case Picture:
    a=rng.Next(2,9);b=new[]{2,5,10}[rng.Next(3)];result=a*b;
    prompt=mode==0?$"في تمثيل بياني بالصور هناك {a} رموز، كل رمز يمثل {b} كتب. كم كتابًا؟":
     $"يوضح مفتاح التمثيل أن كل صورة تمثل {b} زهور؛ إذا ظهرت {a} صور، كم زهرة؟";
    solution="أقرأ مفتاح الرسم أولًا، ثم أضرب عدد الصور في قيمة كل صورة.";break;
   case Bars:
    a=rng.Next(3,16);b=rng.Next(1,13);result=mode==0?a+b:Math.Abs(a-b);
    prompt=mode==0?$"يمثل العمود الأول {a} مشاركًا والثاني {b}. كم مشاركًا في الفئتين؟":
     $"طول العمود الأول {a} وحدات والثاني {b}. ما الفرق بين القيمتين؟";
    solution="أقرأ محور القيم ومقياسه وأقارن ارتفاع الأعمدة، ثم أجمع أو أطرح.";break;
   case Mixed:
    a=rng.Next(2,9);b=new[]{2,5,10}[rng.Next(3)];result=a*b;
    prompt=mode==0?$"تظهر {a} صور في مخطط صور، وكل صورة تمثل {b} عناصر. ما ارتفاع العمود المناظر في مخطط أعمدة بوحدة عنصر واحد؟":
     $"لتحويل رسم الصور إلى عمود بياني، هناك {a} رموز، قيمة كل رمز {b}. كم وحدة على محور العمود؟";
    solution="أحول الصور إلى عدد عناصر حقيقي بحسب مفتاح الرسم، ثم أرسم العمود بالقيمة نفسها.";break;
   case Line:
    a=rng.Next(2,10);b=rng.Next(1,a+1);result=mode==0?a+b:a-b;
    prompt=mode==0?$"في تمثيل بالنقاط يوجد {a} علامات فوق القيمة ٥ و{b} علامات فوق القيمة ٦. كم مشاهدة في الفئتين؟":
     $"تمثل النقاط {a} مشاهدة عند العدد ٧ و{b} عند العدد ٨. ما فرق التكرارين؟";
    solution="كل علامة X تمثل مشاهدة واحدة؛ أعدّ العلامات لكل قيمة بدقة.";break;
   case Pattern:
    a=rng.Next(2,15);b=rng.Next(2,10);
    result=mode==0?a+4*b:a;
    prompt=mode==0?$"أكمل النمط المنتظم: {a}، {a+b}، {a+2*b}، {a+3*b}، □. ما العدد التالي؟":
     $"استنتج قاعدة الطرح وأكمل: {a+4*b}، {a+3*b}، {a+2*b}، {a+b}، □.";
    solution="أجد مقدار التغير الثابت بين الحدود، وأستخدمه للانتقال إلى الحد التالي.";break;
   default:throw new InvalidOperationException("Unknown");
  }
  return new Item(f,prompt,solution,AssessmentItemType.Numeric,
   new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode},{"result",result}});
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int expected)
 {
  expected=0;
  if(!Supports(f)||p.Count!=4||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
    !p.TryGetValue("mode",out int mode)||!p.TryGetValue("result",out int got)||
    mode<0||mode>1)return false;
  bool ok=f switch{
   Tally=>a>=4&&a<=12&&b>=1&&b<=a,
   Picture or Mixed=>a>=2&&a<=8&&(b==2||b==5||b==10),
   Bars=>a>=3&&a<=15&&b>=1&&b<=12,
   Line=>a>=2&&a<=9&&b>=1&&b<=a,
   Pattern=>a>=2&&a<=14&&b>=2&&b<=9,
   _=>false};
  if(!ok)return false;
  expected=f switch{
   Tally=>mode==0?a+b:a-b,
   Picture or Mixed=>a*b,
   Bars=>mode==0?a+b:Math.Abs(a-b),
   Line=>mode==0?a+b:a-b,
   Pattern=>mode==0?a+4*b:a,
   _=>throw new InvalidOperationException("Unsupported")};
  return got==expected;
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid data problem parameters");
  return n.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
  string s=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
   c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
  return int.TryParse(s,NumberStyles.Integer,CultureInfo.InvariantCulture,out int n)&&n==expected;
 }
}
