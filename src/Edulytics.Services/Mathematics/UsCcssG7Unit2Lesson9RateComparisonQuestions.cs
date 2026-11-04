using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>
/// OUR/IM 2017-2019 G7 U02 L09; compare constant proportional
/// rates vs totals and solve rational race-time and information gaps.
/// Teacher evaluates communication, questioning and assumptions.
/// </summary>
internal static class UsCcssG7Unit2Lesson9RateComparisonQuestions
{
    public const string Family="usccss.g7.u02.l09.missing_info_rate_comparison";
    internal sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f==Family;
    public static Problem Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported G7 L09 family.");
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["mode"]=rng.Next(0,7),["feature"]=rng.Next(0,4),["variant"]=rng.Next(0,2)};
        int m=p["mode"],k=p["feature"],v=p["variant"];
        string q=m switch
        {
            0 => "A race participant travels at 156.25 meters/minute. "+
                 "Find the time in MINUTES needed for "+
                 new[]{"10000","5000","15625","6250"}[k]+
                 " meters. Explain the missing-information decision.",
            1 => "A rider moves at a constant "+
                 new[]{"12","15","20","18"}[k]+
                 " kilometers/hour. A separate card reports a distance of "+
                 new[]{"36","90","60","54"}[k]+
                 " kilometers. What is the duration in HOURS?",
            2 => "Four comment moderators are compared. Find the TOTAL "+
                 "comments completed by person "+(k+1)+
                 ". Person1: 50,000 in210 minutes. Person2: 1325 every5 minutes for200 minutes. "+
                 "Person3: c=331t for120 minutes. Person4: t=(3/800)c for150 minutes.",
            3 => "In the same four-moderator comparison, find the "+
                 "constant processing rate in comments PER MINUTE for person "+
                 (k+1)+". Express the exact fraction where applicable.",
            4 => "For person "+(k+1)+
                 ", find the rank (1=greatest) among four moderators by "+
                 (v==0?"TOTAL completed comments":"COMMENTS PER MINUTE")+
                 ". Totals: P1=50000,P2=53000,P3=39720,P4=40000. "+
                 "Rates: P1=5000/21,P2=265,P3=331,P4=800/3.",
            5 => "Could this scenario define a proportional relation "+
                 "over all relevant nonnegative quantities? "+
                 new[]{"A stack of identical paper sheets, weight vs count.",
                       "A mixed stack of books of different masses, weight vs count.",
                       "Each packaged toy includes exactly 2 batteries, battery count vs toys.",
                       "Two siblings whose ages always differ by three years, one age vs another."}[k]+
                 " Enter 1 yes (with the stated constant assumption), 0 no.",
            _ => "Each toy package contains 2 batteries. A supply "+
                 "has "+new[]{"2","12","50","100"}[k]+
                 " batteries. Find the number of complete toy packages "+
                 "that these batteries can supply."
        };
        return new(Family,q+" Give an exact nonnegative numeric answer.",
            "Information gaps require eliciting the unknown rate or length and "+
            "stating assumptions. Rank total output and hourly rate separately; "+
            "do not confuse absolute count with per-minute productivity.",
            AssessmentItemType.ShortAnswer,p);
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid G7 L09 parameters.");
        var(n,d)=Expected(p);long g=Gcd(n,d);n/=g;d/=g;
        return d==1?n.ToString(CultureInfo.InvariantCulture):
            n.ToString(CultureInfo.InvariantCulture)+"/"+d.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)return false;
        var a=answer.Trim().Split('/');
        decimal value;
        if(a.Length==2)
        {
            if(!decimal.TryParse(a[0],NumberStyles.None,
                    CultureInfo.InvariantCulture,out var n)||
               !decimal.TryParse(a[1],NumberStyles.None,
                    CultureInfo.InvariantCulture,out var d)||
                n<0||n>100000000||d<=0||d>100000000)return false;
            value=n/d;
        }
        else if(a.Length==1)
        {
            if(!decimal.TryParse(a[0],NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,out value)||
               value<0||value>100000000)return false;
        }
        else return false;
        var(en,ed)=Expected(p);return value*ed==en;
    }
    private static(long,long) Expected(IReadOnlyDictionary<string,int> p)
    {
        int m=p["mode"],k=p["feature"],v=p["variant"];
        if(m==0)return (new long[]{64,32,100,40}[k],1);
        if(m==1)return (new long[]{3,6,3,3}[k],1);
        if(m==2)return (new long[]{50000,53000,39720,40000}[k],1);
        if(m==3)return k switch
        {
            0=>(5000,21),1=>(265,1),2=>(331,1),_=>(800,3)
        };
        if(m==4)return(v==0
            ? new long[]{2,1,4,3}[k]
            : new long[]{4,3,1,2}[k],1);
        if(m==5)return (k is 0 or 2?1:0,1);
        return (new long[]{1,6,25,50}[k],1);
    }
    private static long Gcd(long a,long b)
    {while(b!=0){long q=a%b;a=b;b=q;}return a;}
    private static bool Valid(string? f,IReadOnlyDictionary<string,int>? p)=>
        Supports(f)&&p is not null&&p.Count==3&&
        p.TryGetValue("mode",out var m)&&m is >=0 and <=6&&
        p.TryGetValue("feature",out var k)&&k is >=0 and <=3&&
        p.TryGetValue("variant",out var v)&&v is 0 or 1;
}
