using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original-goal digital numeric subset of G1 U04 L06–L10 (ordinals 71–75).
/// Physical collections, tactile towers, gallery walk, and live reading aloud
/// remain teacher-reviewed; typing numbers does not certify those activities.
/// </summary>
public static class UsCcssG1FullGradeTensAndOnesQuestions
{
    public const string MixedCollection="usccss.g1.u04.l06.collection.tens_ones";
    public const string ReadNumber="usccss.g1.u04.l07.read.two_digit";
    public const string MatchRepresentations="usccss.g1.u04.l08.match.tens_ones_expression";
    public const string ShowMultipleWays="usccss.g1.u04.l09.show.expanded_form";
    public const string WriteNumeral="usccss.g1.u04.l10.write.two_digit_numeral";
    public static readonly IReadOnlyList<string> Families=
        [MixedCollection,ReadNumber,MatchRepresentations,ShowMultipleWays,WriteNumeral];
    public sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is not null && Families.Contains(family,StringComparer.Ordinal);
    private static string Norm(string? s)=>
        string.Concat((s??"").Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();
    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)=>
        Supports(family)&&p.Count==4&&
        p.TryGetValue("tens",out var t)&&t>=1&&t<=9&&
        p.TryGetValue("ones",out var o)&&o>=0&&o<=9&&
        p.TryGetValue("mode",out var m)&&m>=0&&m<=3&&
        p.TryGetValue("unused",out var z)&&z==0;
    private static readonly string[] TensWords =
        ["","ten","twenty","thirty","forty","fifty","sixty","seventy","eighty","ninety"];
    private static readonly string[] OnesWords =
        ["","one","two","three","four","five","six","seven","eight","nine"];
    private static readonly string[] TeenWords =
        ["ten","eleven","twelve","thirteen","fourteen","fifteen","sixteen","seventeen","eighteen","nineteen"];
    public static string NumberWords(int tens,int ones)
    {
        if(tens<1||tens>9||ones<0||ones>9)throw new ArgumentOutOfRangeException(nameof(tens));
        return tens==1?TeenWords[ones] : ones==0?TensWords[tens]:TensWords[tens]+"-"+OnesWords[ones];
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G1 tens-and-ones source family");
        int tens=random.Next(2,10),ones=random.Next(0,10),mode=random.Next(0,4),total=10*tens+ones;
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
            {["tens"]=tens,["ones"]=ones,["mode"]=mode,["unused"]=0};
        if(!Valid(family,p))throw new InvalidOperationException("Invalid base-ten representation");
        string prompt=family switch
        {
            MixedCollection=>$"Count an organized collection with {tens} FULL groups of ten and {ones} loose objects. No object counted twice. Write TENS=number;ONES=number;TOTAL=number.",
            ReadNumber=>$"READ this two-digit number: {total}. Express its number NAME (for example 43 = forty-three). Write NAME=word.",
            MatchRepresentations=>$"Match {tens} ten-rods and {ones} ones with its numeral and expanded expression {10*tens}+{ones}. Write TENS=number;ONES=number;NUMBER=number.",
            ShowMultipleWays=>$"Show the number {total} using bundles of ten, loose cubes and expanded addition. Write EXPANDED=number+number;TENS=number;ONES=number. Teacher checks any actual drawing.",
            WriteNumeral=>$"Write the TWO-DIGIT numeral for {tens} tens and {ones} ones. Write NUMBER=number.",
            _=>throw new InvalidOperationException()
        };
        return new(family,prompt,
          "Ten ones make one ten; make place-value matches and explain representations. Reading aloud, drawing, classroom objects and gallery walks need their own classroom assessment.",
          AssessmentItemType.ShortAnswer,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G1 tens-and-ones source instance");
        int t=p["tens"],o=p["ones"],n=10*t+o;
        return family switch
        {
            MixedCollection=>$"TENS={t};ONES={o};TOTAL={n}",
            ReadNumber=>$"NAME={NumberWords(t,o)}",
            MatchRepresentations=>$"TENS={t};ONES={o};NUMBER={n}",
            ShowMultipleWays=>$"EXPANDED={10*t}+{o};TENS={t};ONES={o}",
            WriteNumeral=>$"NUMBER={n}",
            _=>throw new InvalidOperationException()
        };
    }
    public static bool GradePersistedAnswer(string? family,string? expected,string? response)
    {
        if(!Supports(family)||string.IsNullOrWhiteSpace(expected)||string.IsNullOrWhiteSpace(response))
            return false;
        var e=Norm(expected).Split(';');var r=Norm(response).Split(';');
        if(e.Length!=r.Length)return false;
        var correct=new Dictionary<string,string>(StringComparer.Ordinal);
        var actual=new Dictionary<string,string>(StringComparer.Ordinal);
        foreach(var (parts,map) in new[]{(e,correct),(r,actual)})
            foreach(var pair in parts)
            {
                int at=pair.IndexOf('=');
                if(at<=0||at>=pair.Length-1||!map.TryAdd(pair[..at],pair[(at+1)..]))return false;
            }
        return correct.Count==actual.Count && correct.All(x=>
            actual.TryGetValue(x.Key,out var value)&&value==x.Value);
    }
    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? candidate)=>
        Valid(family,p)&&GradePersistedAnswer(family,Solve(family!,p),candidate);
}
