using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit11EstimationQuestions
{
 internal const string Sum="uae.g4.u11.l02.estimate_sum_whole";
 internal const string Difference="uae.g4.u11.l02.estimate_difference_whole";
 internal const string ApproxCost="uae.g4.u11.l03.approximate_ticket_cost";
 internal const string ExactCost="uae.g4.u11.l03.exact_ticket_cost_fils";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 internal static bool Supports(string? f)=>f is Sum or Difference or ApproxCost or ExactCost;
 private static int RoundWhole(int fils)=>(int)Math.Round(fils/100m,0,MidpointRounding.AwayFromZero);
 internal static Item Build(string f,Random random)
 {
    if(!Supports(f))throw new InvalidOperationException("Unsupported estimation question family");
    if(f is ApproxCost or ExactCost)
    {
       int price=random.Next(75,1501),count=random.Next(2,6);
       var p=new Dictionary<string,int>{{"priceFils",price},{"count",count}};
       string prompt=f==ApproxCost?
        $"سعر التذكرة {(price/100m).ToString("0.00",CultureInfo.InvariantCulture)} درهم واشترت الأسرة {count} تذاكر. قدّر تكلفة التذاكر إلى الدراهم بعد تقريب سعر التذكرة إلى أقرب درهم. اكتب عدد الدراهم.":
        $"سعر التذكرة {(price/100m).ToString("0.00",CultureInfo.InvariantCulture)} درهم واشترت الأسرة {count} تذاكر. ما التكلفة الدقيقة بالفلس؟";
       return new Item(f,prompt,"للتقدير قرب سعر التذكرة أولًا، وللحساب الدقيق اضرب الثمن بالفلس في العدد.",AssessmentItemType.Numeric,p);
    }
    int a=random.Next(200,10001),b=random.Next(100,f==Difference?a:10001);
    var q=new Dictionary<string,int>{{"firstFils",a},{"secondFils",b}};
    string prefix=f==Sum?"مجموع":"فرق";
    string operation=f==Sum?"+":"−";
    string question=$"قدّر {prefix} {(a/100m).ToString("0.00",CultureInfo.InvariantCulture)} {operation} {(b/100m).ToString("0.00",CultureInfo.InvariantCulture)} بعد تقريب كل عدد لأقرب درهم. اكتب عدد الدراهم.";
    return new Item(f,question,"قرب كل قيمة إلى العدد الكلي الأقرب ثم اجمع أو اطرح.",AssessmentItemType.Numeric,q);
 }
 private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int answer)
 {
   answer=0;
   if(!Supports(f))return false;
   if(f is ApproxCost or ExactCost)
   {
      if(p.Count!=2||!p.TryGetValue("priceFils",out int price)||!p.TryGetValue("count",out int count)||
          price is <1 or >10000||count is <1 or >30)return false;
      answer=f==ApproxCost?RoundWhole(price)*count:price*count;
      return true;
   }
   if(p.Count!=2||!p.TryGetValue("firstFils",out int a)||!p.TryGetValue("secondFils",out int b)||
       a is <0 or >100000||b is <0 or >100000||(f==Difference&&b>a))return false;
   answer=f==Sum?RoundWhole(a)+RoundWhole(b):RoundWhole(a)-RoundWhole(b);
   return true;
 }
 internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
   if(!Valid(f,p,out int ans))throw new InvalidOperationException("Invalid estimation problem");
   return ans.ToString(CultureInfo.InvariantCulture);
 }
 internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
   if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
   string normalized=new(answer.Trim().Select(c=>c switch{
     >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
     >= '۰' and <= '۹'=>(char)('0'+c-'۰'),_=>c
   }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
   return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
 }
}
