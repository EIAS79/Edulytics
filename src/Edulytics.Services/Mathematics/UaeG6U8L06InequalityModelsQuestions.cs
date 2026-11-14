using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exact Grade6 Arabic inequality boundary, direction and membership.
/// Source exploration freehand number lines remain teacher reviewed.</summary>
internal static class UaeG6U8L06InequalityModelsQuestions
{
    internal const string Membership="uae.g6.u08.l06.candidate_membership";
    internal const string Endpoint="uae.g6.u08.l06.endpoint_inclusion";
    internal const string Direction="uae.g6.u08.l06.graph_direction";
    internal sealed record Item(string Family,string Prompt,string Solution,
      AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Membership or Endpoint or Direction;
    private static string Symbol(int mode)=>mode switch{0=>"<",1=>"≤",2=>">",3=>"≥",
      _=>throw new ArgumentOutOfRangeException(nameof(mode))};
    private static bool TryExpected(string family,IReadOnlyDictionary<string,int>? p,out string answer)
    {
        answer="";
        if(!Supports(family)||p is null||!p.TryGetValue("op",out int op)||
           op is <0 or >3)return false;
        if(family==Membership)
        {
            if(p.Count!=3||!p.TryGetValue("bound",out int bound)||
               !p.TryGetValue("candidate",out int candidate)||
               bound is <-20 or >20||candidate is <-20 or >20)return false;
            bool yes=op switch{0=>candidate<bound,1=>candidate<=bound,
              2=>candidate>bound,_=>candidate>=bound};
            answer=yes?"نعم":"لا";
            return true;
        }
        if(p.Count!=1)return false;
        answer=family==Endpoint?(op is 0 or 2?"مفتوحة":"مغلقة"):
          (op is 0 or 1?"يسار":"يمين");
        return true;
    }
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported inequality");
        ArgumentNullException.ThrowIfNull(random);
        int op=random.Next(4),bound=random.Next(-12,13),candidate=random.Next(-15,16);
        var p=new Dictionary<string,int>{{"op",op}};
        string prompt;
        if(family==Membership)
        {
            p["bound"]=bound;p["candidate"]=candidate;
            prompt=$"هل العدد {candidate} حل للمتباينة س {Symbol(op)} {bound}؟ اكتب نعم أو لا.";
        }
        else if(family==Endpoint)
            prompt=$"في التمثيل البياني للمتباينة س {Symbol(op)} {bound}، هل دائرة الحد {bound} مفتوحة أم مغلقة؟";
        else
            prompt=$"في تمثيل س {Symbol(op)} {bound}، هل جهة التظليل يسار العدد {bound} أم يمينه؟ اكتب يسار أو يمين.";
        return new Item(family,prompt,
            "أحدد أولًا الرمز واتجاه القيم المقبولة ثم أختبر إدراج الحد.",
            AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)=>
        TryExpected(family,p,out var s)?s:throw new InvalidOperationException("Invalid inequality params");
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)=>
        !string.IsNullOrWhiteSpace(answer)&&answer.Length<=40&&
        TryExpected(family,p,out var wanted)&&
        string.Equals(answer.Trim(),wanted,StringComparison.Ordinal);
}
