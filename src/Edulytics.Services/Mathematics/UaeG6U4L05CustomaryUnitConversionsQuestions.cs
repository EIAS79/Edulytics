using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exact Grade6 Arabic customary unit ratio adapter: lengths, mass and capacity.
/// Graph and free-text explanations remain teacher reviewed. Existing engines are unchanged.</summary>
internal static class UaeG6U4L05CustomaryUnitConversionsQuestions
{
    internal const string Smaller="uae.g6.u04.l05.to_smaller_exact";
    internal const string Larger="uae.g6.u04.l05.to_larger_exact";
    private static readonly (string Large,string Small,int Rate)[] Ratios=[
        ("yd","ft",3),("ft","in",12),("mi","ft",5280),
        ("T","lb",2000),("lb","oz",16),("gal","qt",4),
        ("qt","pt",2),("pt","c",2),("c","fl oz",8)
    ];
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Smaller or Larger;
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported unit ratio");
        int index=random.Next(Ratios.Length),count=random.Next(1,21);
        int n=family==Smaller?2*count+random.Next(2):count,d=family==Smaller?2:1;
        var pair=Ratios[index];
        string value=d==1?$"{n}":$"{n}/2";
        string from=family==Smaller?pair.Large:pair.Small;
        string to=family==Smaller?pair.Small:pair.Large;
        return new Item(family,$"حوّل {value} {from} إلى {to}. اكتب عددًا صحيحًا أو كسرًا مبسطًا (بسط/مقام).",
            $"١ {pair.Large} = {pair.Rate} {pair.Small}. اختر اتجاه النسبة بحيث تُلغى الوحدة القديمة.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string,int>{{"pair",index},{"n",n},{"d",d}});
    }
    private static int Gcd(int a,int b){while(b!=0){int t=a%b;a=b;b=t;}return a;}
    private static bool TryResult(string family,IReadOnlyDictionary<string,int>? p,out int a,out int b)
    {
        a=b=0;
        if(!Supports(family)||p is null||p.Count!=3||
           !p.TryGetValue("pair",out int index)||index<0||index>=Ratios.Length||
           !p.TryGetValue("n",out int n)||n is <1 or >41||
           !p.TryGetValue("d",out int d)||d is not (1 or 2)||
           (family==Larger&&d!=1))return false;
        int rate=Ratios[index].Rate;
        a=family==Smaller?checked(n*rate):n;
        b=family==Smaller?d:checked(d*rate);
        int g=Gcd(a,b);a/=g;b/=g;return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!TryResult(family,p,out int n,out int d))
            throw new InvalidOperationException("Invalid conversion operands");
        return d==1?n.ToString(CultureInfo.InvariantCulture):$"{n}/{d}";
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!TryResult(family,p,out int n,out int d)||
           string.IsNullOrWhiteSpace(answer)||answer.Length>64)return false;
        string normalized=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch))
            .Select(ch=>ch switch{
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                '⁄'=>'/',_=>ch}).ToArray());
        string[] parts=normalized.Split('/');
        if(d==1&&parts.Length==1)return int.TryParse(parts[0],
            NumberStyles.None,CultureInfo.InvariantCulture,out int x)&&x==n;
        return parts.Length==2&&int.TryParse(parts[0],NumberStyles.None,
            CultureInfo.InvariantCulture,out int a)&&int.TryParse(parts[1],
            NumberStyles.None,CultureInfo.InvariantCulture,out int b)&&
            a>0&&b>0&&Gcd(a,b)==1&&a==n&&b==d;
    }
}
