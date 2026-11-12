using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit12GraphsQuestions
{
 internal const string PlotValue="uae.g4.u12.l01.plot_read_value";
 internal const string PlotDifference="uae.g4.u12.l01.plot_difference";
 internal const string LineMax="uae.g4.u12.l02.line_graph_max";
 internal const string LineIncrease="uae.g4.u12.l02.line_graph_increase";
 internal const string PieCategory="uae.g4.u12.l03.pie_category_percent";
 internal const string PieRest="uae.g4.u12.l03.pie_rest_percent";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 internal static bool Supports(string? f)=>f is PlotValue or PlotDifference or LineMax or LineIncrease or PieCategory or PieRest;
 private static bool IsPie(string f)=>f is PieCategory or PieRest;
 internal static Item Build(string f,Random random)
 {
   if(!Supports(f))throw new InvalidOperationException("Unsupported grade4 graph family");
   Dictionary<string,int> p;
   if(IsPie(f))
   {
      int a=random.Next(10,40),b=random.Next(10,35),c=random.Next(10,90-a-b);
      p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"d",0},{"e",0}};
      string data=$"في تمثيل دائري: القراءة {a}%، الرياضة {b}%، الرسم {c}%، وبقية الوقت لأنشطة أخرى.";
      string question=f==PieCategory?
       $"{data} ما النسبة المئوية للرياضة؟":
       $"{data} ما النسبة المئوية لبقية الأنشطة؟";
      return new Item(f,question,"اجمع نسب الأجزاء المعطاة، والقطاع الكامل يساوي ١٠٠٪.",
        AssessmentItemType.Numeric,p);
   }
   int a0=random.Next(12,32),a1=a0+random.Next(2,7),a2=a1+random.Next(1,7),
       a3=a2+random.Next(1,7),a4=a3+random.Next(1,7);
   p=new Dictionary<string,int>{{"a",a0},{"b",a1},{"c",a2},{"d",a3},{"e",a4}};
   string table=$"قيم مخطط من خمس نقاط متتابعة هي {a0}، {a1}، {a2}، {a3}، {a4}.";
   string prompt=f switch
   {
       PlotValue=>$"{table} ما قيمة النقطة الثالثة؟",
       PlotDifference=>$"{table} ما مقدار الزيادة من النقطة الأولى إلى الخامسة؟",
       LineMax=>$"{table} ما أعلى قيمة في التمثيل البياني الخطي؟",
       _=>$"{table} كم ارتفعت القيمة من بداية الخط إلى نهايته؟"
   };
   return new Item(f,prompt,"اقرأ القيم من التدريج على محور البيانات؛ ولإيجاد التغير اطرح القيمة الأولى من الأخيرة.",
     AssessmentItemType.Numeric,p);
 }
 private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int ans)
 {
   ans=0;
   if(!Supports(f)||p.Count!=5||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
      !p.TryGetValue("c",out int c)||!p.TryGetValue("d",out int d)||!p.TryGetValue("e",out int e))return false;
   if(IsPie(f))
   {
       if(a is <1 or >80||b is <1 or >80||c is <1 or >80||a+b+c>100||d!=0||e!=0)return false;
       ans=f==PieCategory?b:100-a-b-c;return true;
   }
   if(a<0||b<a||c<b||d<c||e<d||e>150)return false;
   ans=f switch
   {
       PlotValue=>c,PlotDifference or LineIncrease=>e-a,_=>e
   };
   return true;
 }
 internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
   if(!Valid(f,p,out int result))throw new InvalidOperationException("Invalid graph data");
   return result.ToString(CultureInfo.InvariantCulture);
 }
 internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
   if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
   string normalized=new(answer.Trim().Select(c=>c switch
   {
      >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
      >= '۰' and <= '۹'=>(char)('0'+c-'۰'),_=>c
   }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
   return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
 }
}
