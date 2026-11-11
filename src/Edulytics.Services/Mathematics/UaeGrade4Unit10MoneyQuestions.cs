using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit10MoneyQuestions
{
 internal const string Total="uae.g4.u10.l10.coin_total_fils";
 internal const string Difference="uae.g4.u10.l10.coin_wallet_difference";
 internal const string Change="uae.g4.u10.l11.change_fils";
 internal const string NextDirham="uae.g4.u10.l11.count_up_to_next_dirham";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 internal static bool Supports(string? f)=>f is Total or Difference or Change or NextDirham;
 private static int CoinSum(IReadOnlyDictionary<string,int> p,string prefix)
     =>p[prefix+"25"]*25+p[prefix+"10"]*10+p[prefix+"5"]*5+p[prefix+"1"];
 internal static Item Build(string f,Random random)
 {
    if(!Supports(f))throw new InvalidOperationException("Unsupported money question");
    if(f is Total or Difference)
    {
        var p=new Dictionary<string,int>();
        foreach(string prefix in new[]{"a","b"})
            foreach(int coin in new[]{25,10,5,1})
                p[prefix+coin]=random.Next(0,4);
        string prompt=f==Total?
          $"في محفظة {p["a25"]} عملات من فئة ٢٥ فلسًا و{p["a10"]} من فئة ١٠ فلوس و{p["a5"]} من فئة ٥ فلوس و{p["a1"]} من فئة فلس واحد. ما قيمتها بالفلس؟":
          $"هناك محفظتان: الأولى قيمتها {CoinSum(p,"a")} فلسًا، والثانية {CoinSum(p,"b")} فلسًا. ما الفرق بينهما بالفلس؟";
        return new Item(f,prompt,"اضرب عدد كل فئة في قيمتها بالفلس ثم اجمع القيم؛ استخدم الطرح لإيجاد الفرق.",
            AssessmentItemType.Numeric,p);
    }
    int paid=random.Next(1,6)*100,cost=random.Next(1,paid);
    var q=new Dictionary<string,int>{{"paidFils",paid},{"costFils",cost}};
    string question=f==Change?
       $"ثمن سلعة {cost} فلسًا، ودفع المشتري {paid} فلسًا. كم فلسًا يجب ردّه؟":
       $"ثمن سلعة {cost} فلسًا. كم فلسًا نضيف إلى هذا الثمن للوصول إلى أقرب درهم كامل تالٍ؟";
    return new Item(f,question,"ابدأ من الثمن وعدّ تصاعديًا حتى المبلغ المدفوع أو الدرهم الكامل التالي.",
        AssessmentItemType.Numeric,q);
 }
 private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int answer)
 {
    answer=0;
    if(!Supports(f))return false;
    if(f is Total or Difference)
    {
        if(p.Count!=8)return false;
        foreach(string prefix in new[]{"a","b"})
            foreach(int coin in new[]{25,10,5,1})
                if(!p.TryGetValue(prefix+coin,out int count)||count is <0 or >5)return false;
        int a=CoinSum(p,"a"),b=CoinSum(p,"b");
        answer=f==Total?a:Math.Abs(a-b);
        return true;
    }
    if(p.Count!=2||!p.TryGetValue("paidFils",out int paid)||!p.TryGetValue("costFils",out int cost)||
       paid<100||paid>500||paid%100!=0||cost<1||cost>=paid)return false;
    answer=f==Change?paid-cost:(100-cost%100)%100;
    return true;
 }
 internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
   if(!Valid(f,p,out int ans))throw new InvalidOperationException("Invalid money exercise");
   return ans.ToString(CultureInfo.InvariantCulture);
 }
 internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
   if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
   string normalized=new(answer.Trim().Select(c=>c switch
   {
       >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
       >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
       _=>c
   }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
   return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
 }
}
