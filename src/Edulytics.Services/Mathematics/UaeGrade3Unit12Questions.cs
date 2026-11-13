using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Grade3 data handling; rendering graphs requires teacher verification.
// Exact engine is reused. Stage D tests are source only, NOT_RUN in A-B-C.
internal static class UaeGrade3Unit12Questions
{
 internal const string Tally="uae.g3.u12.l01.tally_record";
 internal const string G5TallyFrequency="uae.g5.u11.l01.tally_category_frequency";
 internal const string G5TallyTotal="uae.g5.u11.l01.tally_total_frequency";
 internal const string G5TallyRange="uae.g5.u11.l01.tally_frequency_range";
 internal const string Picture="uae.g3.u12.l02.scaled_picture_graph";
 internal const string Bars="uae.g3.u12.l03.bar_graph";
 internal const string Mixed="uae.g3.u12.l04.picture_bar_graph_compare";
 internal const string Line="uae.g3.u12.l05.line_plot";
 internal const string Pattern="uae.g3.u12.l06.number_sense_pattern";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 private static bool IsG5Tally(string? f)=>f is G5TallyFrequency or G5TallyTotal or G5TallyRange;
 public static bool Supports(string? f)=>f is Tally or Picture or Bars or Mixed or Line or Pattern || IsG5Tally(f);
 public static Item Build(string f,Random rng)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported Grade3 data question");
  if(IsG5Tally(f))return BuildG5Tally(f,rng);
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
  if(IsG5Tally(f))return SolveG5Tally(f,p);
  if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid data problem parameters");
  return n.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(IsG5Tally(f))return VerifyG5Tally(f,p,answer);
  if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
  string s=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
   c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
  return int.TryParse(s,NumberStyles.Integer,CultureInfo.InvariantCulture,out int n)&&n==expected;
 }

 // Grade 5 Arabic Unit 11 lesson 1. Extend the existing tally-table engine
 // without modifying legacy G3 behaviour or trusting a stored result parameter.
 // A real classroom survey remains a teacher-observed physical activity.
 private static Item BuildG5Tally(string family,Random rng)
 {
  int a=rng.Next(3,9),b=rng.Next(2,8),c=rng.Next(2,7),d=rng.Next(1,6);
  var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"d",d}};
  var responses=Enumerable.Repeat("القراءة",a)
    .Concat(Enumerable.Repeat("الرياضة",b))
    .Concat(Enumerable.Repeat("الرسم",c))
    .Concat(Enumerable.Repeat("البرمجة",d))
    .OrderBy(_=>rng.Next()).ToArray();
  string question="أُجري مسح عن النشاط المفضل. هذه الإجابات الخام: "+
    string.Join("، ",responses)+". ";
  question+=family switch
  {
   G5TallyFrequency=>"أنشئ جدول علامات إحصاء ثم اكتب تكرار اختيار الرسم.",
   G5TallyTotal=>"أنشئ جدول تكرار بأربع فئات. كم طالبًا شارك في المسح؟",
   G5TallyRange=>"بعد إنشاء جدول التكرار، ما الفرق بين أعلى تكرار وأقل تكرار؟",
   _=>throw new InvalidOperationException("Unknown Grade5 tally family")
  };
  return new Item(family,question,
   "أضع علامة واحدة لكل إجابة في فئتها، وأجمع العلامات في مجموعات من خمسة، ثم أتحقق من عدد المشاركين ومقارنة التكرارات.",
   AssessmentItemType.Numeric,p);
 }
 private static bool ValidG5Tally(string f,IReadOnlyDictionary<string,int> p,out int expected)
 {
  expected=0;
  if(!IsG5Tally(f)||p.Count!=4||
     !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
     !p.TryGetValue("c",out int c)||!p.TryGetValue("d",out int d)||
     a is <3 or >8||b is <2 or >7||c is <2 or >6||d is <1 or >5)
   return false;
  expected=f switch
  {
   G5TallyFrequency=>c,
   G5TallyTotal=>a+b+c+d,
   G5TallyRange=>new[]{a,b,c,d}.Max()-new[]{a,b,c,d}.Min(),
   _=>throw new InvalidOperationException("Unknown Grade5 tally family")
  };
  return true;
 }
 private static string SolveG5Tally(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!ValidG5Tally(f,p,out int result))
   throw new InvalidOperationException("Invalid Grade5 tally data");
  return result.ToString(CultureInfo.InvariantCulture);
 }
 private static bool VerifyG5Tally(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!ValidG5Tally(f,p,out _)||string.IsNullOrWhiteSpace(answer)||answer.Length>24)return false;
  string value=new(answer.Trim().Select(ch=>ch switch
  {
   >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
   >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
   _=>ch
  }).Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ',' and not '٬').ToArray());
  if(!int.TryParse(value,NumberStyles.None,CultureInfo.InvariantCulture,out int submitted))return false;
  int a=p["a"],b=p["b"],c=p["c"],d=p["d"];
  int independentlyExpected=f switch
  {
   G5TallyFrequency=>c,
   G5TallyTotal=>a+b+c+d,
   _=>Math.Max(Math.Max(a,b),Math.Max(c,d))-Math.Min(Math.Min(a,b),Math.Min(c,d))
  };
  return submitted==independentlyExpected;
 }

}
