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
 internal const string G5ShareTwo="uae.g5.u11.l07.equal_share_two_groups";
 internal const string G5ShareThree="uae.g5.u11.l07.equal_share_three_groups";
 internal const string G5ShareMissing="uae.g5.u11.l07.equal_share_missing_group";
 internal const string G5MeanFive="uae.g5.u11.l08.arithmetic_mean_five";
 internal const string G5MeanMissing="uae.g5.u11.l08.missing_value_given_mean";
 internal const string G5MeanHalf="uae.g5.u11.l08.mean_half_decimal";
 internal const string G5MedianOdd="uae.g5.u11.l09.median_odd_five";
 internal const string G5MedianEven="uae.g5.u11.l09.median_even_four";
 internal const string G5ModeSingle="uae.g5.u11.l09.single_mode";
 internal const string G5ModeTwo="uae.g5.u11.l09.two_modes_count";
 internal const string G5ModeNone="uae.g5.u11.l09.no_mode_count";
 internal const string G5DotFrequency="uae.g5.u11.l10.dot_frequency";
 internal const string G5DotTotal="uae.g5.u11.l10.dot_total";
 internal const string G5DotMedian="uae.g5.u11.l10.dot_median";
 internal const string G5DotMode="uae.g5.u11.l10.dot_mode";
 internal const string G5DotRange="uae.g5.u11.l10.dot_range";
 internal const string Picture="uae.g3.u12.l02.scaled_picture_graph";
 internal const string Bars="uae.g3.u12.l03.bar_graph";
 internal const string Mixed="uae.g3.u12.l04.picture_bar_graph_compare";
 internal const string Line="uae.g3.u12.l05.line_plot";
 internal const string Pattern="uae.g3.u12.l06.number_sense_pattern";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 private static bool IsG5Tally(string? f)=>f is G5TallyFrequency or G5TallyTotal or G5TallyRange;
 private static bool IsG5Share(string? f)=>f is G5ShareTwo or G5ShareThree or G5ShareMissing;
 private static bool IsG5Mean(string? f)=>f is G5MeanFive or G5MeanMissing or G5MeanHalf;
 private static bool IsG5MedianMode(string? f)=>f is G5MedianOdd or G5MedianEven or G5ModeSingle or G5ModeTwo or G5ModeNone;
 private static bool IsG5DotPlot(string? f)=>f is G5DotFrequency or G5DotTotal or G5DotMedian or G5DotMode or G5DotRange;
 public static bool Supports(string? f)=>f is Tally or Picture or Bars or Mixed or Line or Pattern || IsG5Tally(f) || IsG5Share(f) || IsG5Mean(f) || IsG5MedianMode(f) || IsG5DotPlot(f);
 public static Item Build(string f,Random rng)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported Grade3 data question");
  if(IsG5DotPlot(f))return BuildG5DotPlot(f,rng);
  if(IsG5MedianMode(f))return BuildG5MedianMode(f,rng);
  if(IsG5Mean(f))return BuildG5Mean(f,rng);
  if(IsG5Share(f))return BuildG5Share(f,rng);
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
  if(IsG5DotPlot(f))return SolveG5DotPlot(f,p);
  if(IsG5MedianMode(f))return SolveG5MedianMode(f,p);
  if(IsG5Mean(f))return SolveG5Mean(f,p);
  if(IsG5Share(f))return SolveG5Share(f,p);
  if(IsG5Tally(f))return SolveG5Tally(f,p);
  if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid data problem parameters");
  return n.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(IsG5DotPlot(f))return VerifyG5DotPlot(f,p,answer);
  if(IsG5MedianMode(f))return VerifyG5MedianMode(f,p,answer);
  if(IsG5Mean(f))return VerifyG5Mean(f,p,answer);
  if(IsG5Share(f))return VerifyG5Share(f,p,answer);
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


 // Grade 5 U11 lesson 7: physical equal-share model of arithmetic mean.
 // The teacher inspects actual counters/redistribution; numeric answer is exact.
 // No expected "result" is placed in the client-facing parameters.
 private static Item BuildG5Share(string family,Random random)
 {
  int mean=random.Next(9,21),x=random.Next(1,5),y=random.Next(1,5);
  int a=mean+x,b=mean-x,c=mean-x-y;
  var p=new Dictionary<string,int>();
  string prompt=family switch
  {
   G5ShareTwo=>$"في مجموعتين من القطع النقدية، الأولى {a} والثانية {b}. إذا وزعنا جميع القطع بالتساوي على مجموعتين، كم قطعة في كل مجموعة؟",
   G5ShareThree=>$"ثلاث مجموعات من الكرات تحتوي {mean+x} و{mean+y} و{mean-x-y} كرة. انقل الكرات بينها حتى تتساوى. كم كرة في كل مجموعة؟",
   G5ShareMissing=>$"نريد التوزيع بالتساوي على ثلاث مجموعات بمعدل {mean} قطع لكل مجموعة. لدى المجموعة الأولى {mean+x} وللثانية {mean+y}. كم قطعة يجب أن تضمها المجموعة الثالثة؟",
   _=>throw new InvalidOperationException("Unsupported mean-model family")
  };
  if(family==G5ShareTwo)
  {
   p.Add("a",a);p.Add("b",b);p.Add("n",2);
  }
  else if(family==G5ShareThree)
  {
   p.Add("a",mean+x);p.Add("b",mean+y);p.Add("c",c);p.Add("n",3);
  }
  else
  {
   p.Add("a",mean+x);p.Add("b",mean+y);p.Add("m",mean);p.Add("n",3);
  }
  return new Item(family,prompt,
    "أجمع العناصر من جميع المجموعات ثم أقسمها على عدد المجموعات. في سؤال العدد الناقص أضرب المتوسط في عدد المجموعات ثم أطرح الأعداد المعلومة. يمكن التحقق بإعادة توزيع حقيقية.",
    AssessmentItemType.Numeric,p);
 }
 private static bool ValidG5Share(string family,IReadOnlyDictionary<string,int> p,out int expected)
 {
  expected=0;
  if(!IsG5Share(family)||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
     !p.TryGetValue("n",out int n)||a is <1 or >30||b is <1 or >30)return false;
  if(family==G5ShareTwo)
  {
   if(p.Count!=3||n!=2||(a+b)%2!=0)return false;
   expected=(a+b)/2;return expected is >=5 and <=20;
  }
  if(family==G5ShareThree)
  {
   if(p.Count!=4||n!=3||!p.TryGetValue("c",out int c)||c is <0 or >20||
      (a+b+c)%3!=0)return false;
   expected=(a+b+c)/3;return expected is >=5 and <=20;
  }
  if(p.Count!=4||n!=3||!p.TryGetValue("m",out int m)||m is <5 or >20)return false;
  expected=3*m-a-b;
  return expected is >=0 and <=20;
 }
 private static string SolveG5Share(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!ValidG5Share(f,p,out int answer))
   throw new InvalidOperationException("Invalid Grade5 fair-share dataset");
  return answer.ToString(CultureInfo.InvariantCulture);
 }
 private static bool VerifyG5Share(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!ValidG5Share(f,p,out _)||string.IsNullOrWhiteSpace(answer)||answer.Length>24)return false;
  string s=new(answer.Trim().Select(ch=>ch switch
  {
   >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
   >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
   _=>ch
  }).Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ',' and not '٬').ToArray());
  if(!int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int value))return false;
  return value==(f switch
  {
   G5ShareTwo=>(p["a"]+p["b"])/2,
   G5ShareThree=>(p["a"]+p["b"]+p["c"])/3,
   _=>3*p["m"]-(p["a"]+p["b"])
  });
 }


 // UAE Grade 5 U11 L08 arithmetic mean. Existing statistics contract
 // extension keeps G3 and L07 families stable; no shared engine duplicated.
 private static Item BuildG5Mean(string f,Random rng)
 {
  int m=rng.Next(10,26),x=rng.Next(1,5),y=rng.Next(1,5);
  var p=new Dictionary<string,int>();
  string prompt;
  if(f==G5MeanFive)
  {
   int a=m-x,b=m-y,c=m,d=m+y,e=m+x;
   p.Add("a",a);p.Add("b",b);p.Add("c",c);p.Add("d",d);p.Add("e",e);
   prompt=$"سجل خمسة طلاب عدد الدقائق التالية في نشاط القراءة: {a}، {b}، {c}، {d}، {e}. ما المتوسط الحسابي للدقائق؟";
  }
  else if(f==G5MeanMissing)
  {
   int a=m+2*x,b=m+y,c=m-x,d=m-y;
   p.Add("a",a);p.Add("b",b);p.Add("c",c);p.Add("d",d);p.Add("m",m);
   prompt=$"متوسط خمسة قياسات هو {m}. القياسات الأربعة المعروفة {a}، {b}، {c}، {d}. ما القياس الخامس المفقود؟";
  }
  else if(f==G5MeanHalf)
  {
   int a=m-1,b=m+1,c=m+2,d=m;
   p.Add("a",a);p.Add("b",b);p.Add("c",c);p.Add("d",d);
   prompt=$"درجات أربعة طلاب هي {a}، {b}، {c}، {d}. احسب المتوسط الحسابي بدقة، وقد تكون النتيجة عشرية.";
  }
  else throw new InvalidOperationException("Unsupported G5 mean family");
  return new Item(f,prompt,
     "أجمع القيم وأقسم على عددها. للعنصر الناقص أضرب المتوسط في عدد العناصر ثم أطرح القيم المعروفة. أتحقق بإعادة حساب المتوسط.",
     AssessmentItemType.Numeric,p);
 }
 private static bool ValidG5Mean(string f,IReadOnlyDictionary<string,int> p,out decimal result)
 {
  result=0;
  if(!IsG5Mean(f)||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
     !p.TryGetValue("c",out int c)||!p.TryGetValue("d",out int d)||
     a is <1 or >50||b is <1 or >50||c is <1 or >50||d is <1 or >50)return false;
  if(f==G5MeanFive)
  {
   if(p.Count!=5||!p.TryGetValue("e",out int e)||e is <1 or >50)return false;
   result=(a+b+c+d+e)/5m;
  }
  else if(f==G5MeanMissing)
  {
   if(p.Count!=5||!p.TryGetValue("m",out int mean)||mean is <5 or >30)return false;
   result=5*mean-a-b-c-d;
   if(result is <0 or >50)return false;
  }
  else
  {
   if(p.Count!=4)return false;
   result=(a+b+c+d)/4m;
  }
  return true;
 }
 private static string SolveG5Mean(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!ValidG5Mean(f,p,out decimal value))
   throw new InvalidOperationException("Invalid Grade5 mean dataset");
  return value.ToString("0.##",CultureInfo.InvariantCulture);
 }
 private static bool VerifyG5Mean(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!ValidG5Mean(f,p,out _)||string.IsNullOrWhiteSpace(answer)||answer.Length>24)return false;
  string s=new(answer.Trim().Select(ch=>ch switch
  {
   >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
   >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
   '٫'=>'.',
   _=>ch
  }).Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ',' and not '٬').ToArray());
  if(!decimal.TryParse(s,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal submitted))return false;
  int a=p["a"],b=p["b"],c=p["c"],d=p["d"];
  decimal independentlyCalculated=f switch
  {
   G5MeanFive=>(a+b+c+d+p["e"])/5m,
   G5MeanMissing=>5*p["m"]-a-b-c-d,
   _=>(a+b+c+d)/4m
  };
  return submitted==independentlyCalculated;
 }


 // Grade 5 Unit 11 lesson 9: source-bound median and mode in the existing
 // statistics engine. Every answer is derived from the supplied data, never
 // a client-submitted expected result. Free-form justifications remain teacher-reviewed.
 private static Item BuildG5MedianMode(string family,Random rng)
 {
  int n=rng.Next(9,40);
  var p=new Dictionary<string,int>();
  int[] values;
  if(family==G5MedianOdd)
  {
   values=new[]{n-5,n-1,n,n+4,n+8};
   p.Add("a",values[0]);p.Add("b",values[1]);p.Add("c",values[2]);
   p.Add("d",values[3]);p.Add("e",values[4]);
  }
  else if(family==G5MedianEven)
  {
   values=new[]{n-4,n,n+1,n+7};
   p.Add("a",values[0]);p.Add("b",values[1]);p.Add("c",values[2]);p.Add("d",values[3]);
  }
  else if(family==G5ModeSingle)
  {
   values=new[]{n,n,n,n+2,n+4,n+6};
   p.Add("a",n);p.Add("b",n+2);p.Add("c",n+4);p.Add("d",n+6);
  }
  else if(family==G5ModeTwo)
  {
   values=new[]{n,n,n+2,n+2,n+4,n+6};
   p.Add("a",n);p.Add("b",n+2);p.Add("c",n+4);p.Add("d",n+6);
  }
  else if(family==G5ModeNone)
  {
   values=new[]{n,n+2,n+4,n+6,n+8};
   p.Add("a",values[0]);p.Add("b",values[1]);p.Add("c",values[2]);
   p.Add("d",values[3]);p.Add("e",values[4]);
  }
  else throw new InvalidOperationException("Unsupported Grade5 median-mode family");
  var scrambled=values.OrderBy(_=>rng.Next()).ToArray();
  string prompt=$"البيانات: {string.Join("، ",scrambled)}. ";
  prompt+=family switch
  {
   G5MedianOdd or G5MedianEven=>"ما الوسيط؟ رتب البيانات أولًا، ثم اكتب الناتج بدقة.",
   G5ModeSingle=>"ما قيمة المنوال؟",
   _=>"كم عدد المنوالات المتميزة في هذه المجموعة؟ إذا لم يوجد منوال فاكتب ٠."
  };
  return new Item(family,prompt,
   "لإيجاد الوسيط أرتب القيم وأختار الأوسط أو متوسط الأوسطين. ولإيجاد المنوال أحصي تكرار كل قيمة وأقارن التكرارات؛ لا يوجد منوال عند تساوي جميع التكرارات.",
   AssessmentItemType.Numeric,p);
 }
 private static bool ValidG5MedianMode(string f,IReadOnlyDictionary<string,int> p,out decimal result)
 {
  result=0;
  if(!IsG5MedianMode(f)||p.Count!=(f is G5MedianOdd or G5ModeNone?5:4)||
     !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
     !p.TryGetValue("c",out int c)||!p.TryGetValue("d",out int d))return false;
  int e=0;
  if((f is G5MedianOdd or G5ModeNone) && !p.TryGetValue("e",out e))return false;
  int[] data=f switch
  {
   G5MedianOdd or G5ModeNone=>new[]{a,b,c,d,e},
   G5MedianEven=>new[]{a,b,c,d},
   G5ModeSingle=>new[]{a,a,a,b,c,d},
   G5ModeTwo=>new[]{a,a,b,b,c,d},
   _=>throw new InvalidOperationException("Unexpected family")
  };
  if(data.Any(x=>x is <0 or >100))return false;
  if((f is G5ModeSingle or G5ModeTwo) && new[]{a,b,c,d}.Distinct().Count()!=4)return false;
  if(f==G5ModeNone && data.Distinct().Count()!=data.Length)return false;
  int[] sorted=data.OrderBy(x=>x).ToArray();
  if(f==G5MedianOdd)result=sorted[2];
  else if(f==G5MedianEven)result=(sorted[1]+sorted[2])/2m;
  else if(f==G5ModeSingle)result=a;
  else if(f==G5ModeTwo)result=data.GroupBy(x=>x).Count(g=>g.Count()==2);
  else result=0;
  return true;
 }
 private static string SolveG5MedianMode(string family,IReadOnlyDictionary<string,int> p)
 {
  if(!ValidG5MedianMode(family,p,out decimal expected))
   throw new InvalidOperationException("Invalid Grade5 median/mode dataset");
  return expected.ToString("0.##",CultureInfo.InvariantCulture);
 }
 private static bool VerifyG5MedianMode(string family,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!ValidG5MedianMode(family,p,out _)||string.IsNullOrWhiteSpace(answer)||answer.Length>24)return false;
  string normalized=new(answer.Trim().Select(ch=>ch switch
  {
   >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
   >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
   '٫'=>'.',
   _=>ch
  }).Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ',' and not '٬').ToArray());
  if(!decimal.TryParse(normalized,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal got))return false;
  int a=p["a"],b=p["b"],c=p["c"],d=p["d"];
  decimal independent=family switch
  {
   G5MedianOdd=>new[]{a,b,c,d,p["e"]}.OrderBy(x=>x).ElementAt(2),
   G5MedianEven=>(new[]{a,b,c,d}.OrderBy(x=>x).ElementAt(1)+
                 new[]{a,b,c,d}.OrderBy(x=>x).ElementAt(2))/2m,
   G5ModeSingle=>a,
   G5ModeTwo=>new[]{a,a,b,b,c,d}.GroupBy(x=>x).Count(g=>g.Count()==2),
   G5ModeNone=>0,
   _=>throw new InvalidOperationException("Unknown Grade5 median/mode family")
  };
  return got==independent;
 }


 // Grade 5 U11 L10: numerical reading of dot plots; drawing the
 // actual axis and narrative about outliers remain teacher-reviewed.
 // All answers are reconstructed from five source data frequencies.
 private static Item BuildG5DotPlot(string family,Random rng)
 {
  int start=rng.Next(8,41),step=rng.Next(1,4);
  int[] frequencies=Enumerable.Range(0,5).Select(_=>rng.Next(1,5)).ToArray();
  if(family==G5DotMode)frequencies[2]=6; // The unique highest count.
  var p=new Dictionary<string,int>{{"a",start},{"s",step}};
  for(int i=0;i<5;i++)p.Add($"f{i}",frequencies[i]);
  string plot=string.Join("؛ ",Enumerable.Range(0,5).Select(i=>
      $"{start+i*step}: {new string('X',frequencies[i])}"));
  string prompt="يمثل كل X مشاهدة واحدة في مخطط النقاط: "+plot+". ";
  prompt+=family switch
  {
   G5DotFrequency=>$"كم مشاهدة عند القيمة {start+2*step}؟",
   G5DotTotal=>"ما إجمالي المشاهدات؟",
   G5DotMedian=>"ما وسيط جميع البيانات عند عد العلامات المكررة؟",
   G5DotMode=>"ما القيمة الأكثر تكرارًا (المنوال)؟",
   G5DotRange=>"ما مدى البيانات؟",
   _=>throw new InvalidOperationException("Unsupported Grade5 dot-plot family")
  };
  return new Item(family,prompt,
   "كل علامة X مشاهدة واحدة. لقراءة التكرار أعد العلامات، ولإيجاد الوسيط أرتب جميع المشاهدات مع تكرارها، وللمنوال أختار أعلى تكرار، وللمدى أطرح أصغر قيمة من أكبرها.",
   AssessmentItemType.Numeric,p);
 }
 private static bool ValidG5DotPlot(string family,IReadOnlyDictionary<string,int> p,out decimal result)
 {
  result=0;
  if(!IsG5DotPlot(family)||p.Count!=7||!p.TryGetValue("a",out int a)||
     !p.TryGetValue("s",out int step)||a is <0 or >80||step is <1 or >4||a+4*step>100)return false;
  int[] counts=new int[5];
  for(int i=0;i<5;i++)
  {
   if(!p.TryGetValue($"f{i}",out int count)||count is <1 or >6)return false;
   counts[i]=count;
  }
  if(family==G5DotMode && counts.Count(x=>x==counts.Max())!=1)return false;
  int[] data=Enumerable.Range(0,5)
    .SelectMany(i=>Enumerable.Repeat(a+i*step,counts[i])).OrderBy(x=>x).ToArray();
  if(data.Length<5||data.Length>30)return false;
  result=family switch
  {
   G5DotFrequency=>counts[2],
   G5DotTotal=>data.Length,
   G5DotMedian=>(data[(data.Length-1)/2]+data[data.Length/2])/2m,
   G5DotMode=>a+Array.IndexOf(counts,counts.Max())*step,
   G5DotRange=>data[^1]-data[0],
   _=>throw new InvalidOperationException("Unexpected Grade5 dot family")
  };
  return true;
 }
 private static string SolveG5DotPlot(string family,IReadOnlyDictionary<string,int> p)
 {
  if(!ValidG5DotPlot(family,p,out decimal answer))
   throw new InvalidOperationException("Invalid Grade5 dot plot");
  return answer.ToString("0.##",CultureInfo.InvariantCulture);
 }
 private static bool VerifyG5DotPlot(string family,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!ValidG5DotPlot(family,p,out _)||string.IsNullOrWhiteSpace(answer)||answer.Length>24)return false;
  string value=new(answer.Trim().Select(ch=>ch switch
  {
   >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
   >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
   '٫'=>'.',
   _=>ch
  }).Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ',' and not '٬').ToArray());
  if(!decimal.TryParse(value,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal submitted))return false;
  int first=p["a"],step=p["s"];
  int[] counts=Enumerable.Range(0,5).Select(i=>p[$"f{i}"]).ToArray();
  int[] values=Enumerable.Range(0,5).SelectMany(i=>Enumerable.Repeat(first+i*step,counts[i]))
    .OrderBy(x=>x).ToArray();
  decimal independent=family switch
  {
   G5DotFrequency=>values.Count(x=>x==first+2*step),
   G5DotTotal=>values.Length,
   G5DotMedian=>(values[(values.Length-1)/2]+values[values.Length/2])/2m,
   G5DotMode=>values.GroupBy(x=>x).OrderByDescending(g=>g.Count()).First().Key,
   G5DotRange=>values.Max()-values.Min(),
   _=>throw new InvalidOperationException("Unknown Grade5 dot family")
  };
  return submitted==independent;
 }

}
