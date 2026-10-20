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
        p.TryGetValue("mode",out var m)&&m>=0&&m<=6&&
        (m<=3||family==MixedCollection||family==ReadNumber)&&
        (family!=MixedCollection||m!=4||(t==5&&o==2))&&
        (family!=MixedCollection||m!=5||(t==4&&o==8))&&
        (family!=MixedCollection||m!=6||(t==3&&o==7))&&
        (family!=ReadNumber||m!=4||(t==3&&o==5))&&
        (family!=ReadNumber||m!=5||(t==8&&o==6))&&
        (family!=ReadNumber||m!=6||(t==4&&o==2))&&
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
        int mode=random.Next(0,family==MixedCollection||family==ReadNumber?7:4);
        int tens=random.Next(2,10),ones=random.Next(0,10);
        if(family==MixedCollection)
        {
            if(mode==4){tens=5;ones=2;} // Source 2021 actual collection of 52 cubes.
            else if(mode==5){tens=4;ones=8;} // Noah's 50 claim for 48 cubes.
            else if(mode==6){tens=3;ones=7;} // Original 37-cube closing representation.
        }
        if(family==ReadNumber)
        {
            if(mode==4){tens=3;ones=5;} // 3 and 5 number cards: 35 or 53 are both valid.
            else if(mode==5){tens=8;ones=6;} // Clare 68 / Diego 86 source dialogue.
            else if(mode==6){tens=4;ones=2;} // Tyler writes 24, but shows 42.
        }
        int total=10*tens+ones;
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
            {["tens"]=tens,["ones"]=ones,["mode"]=mode,["unused"]=0};
        if(!Valid(family,p))throw new InvalidOperationException("Invalid base-ten representation");
        string prompt=family switch
        {
            MixedCollection=>mode==4?
                "EDULYTICS SOURCE-GOAL TASK: A real classroom collection contains 52 separate cubes. Arrange every cube into full groups of ten and leftover ones. How many tens and ones? Write TENS=number;ONES=number;TOTAL=number. Teacher reviews actual grouped objects.":
                mode==5?
                "EDULYTICS REASONING TASK based on the public 2021 Noah scenario: Noah says a model with four complete 10-cube towers and a fifth INCOMPLETE 8-cube tower contains 50 cubes. Do you AGREE or DISAGREE? A full ten needs 10 cubes. Write DECISION=AGREE/DISAGREE;TENS=number;ONES=number;TOTAL=number;CLAIM=number. Explain why to your teacher.":
                mode==6?
                "EDULYTICS SOURCE-GOAL TASK: Represent the lesson's 3 complete towers of ten and 7 loose cubes. Read the number and write TENS=number;ONES=number;TOTAL=number. Teacher reviews physical representation and reading aloud.":
                $"Count an organized collection with {tens} FULL groups of ten and {ones} loose objects. No object counted twice. Write TENS=number;ONES=number;TOTAL=number.",
            ReadNumber=>mode==4?
                "EDULYTICS TWO-CARD REASONING (source public Make It context): One card shows digit 3, the other digit 5. Form either valid two-digit number, 35 OR 53. Describe tens and ones and its number name. Write DIGITS=3,5;NUMBER=number;TENS=number;ONES=number;NAME=word. Both orderings are accepted; teacher checks the physical cubes and reading aloud.":
                mode==5?
                "EDULYTICS SOURCE-GOAL CHECK: A model contains 8 COMPLETE tens and 6 single cubes, including ones shown before the tens. Clare claims 68 and Diego claims 86. Whom do you AGREE with? Write AGREE=CLARE/DIEGO;TENS=number;ONES=number;NUMBER=number;NAME=word. Explain how the numeral places tens first.":
                mode==6?
                "EDULYTICS SOURCE-GOAL CHECK: Tyler writes 24 but makes a drawing of FOUR complete tens and TWO loose ones. Do you agree? Write DECISION=AGREE/DISAGREE;DRAWN=number;CLAIM=number;TENS=number;ONES=number. Drawings require teacher review.":
                $"READ this two-digit number: {total}. Express its number NAME (for example 43 = forty-three). Write NAME=word.",
            MatchRepresentations=>$"Match {tens} ten-rods and {ones} ones with its numeral and expanded expression {10*tens}+{ones}. Write TENS=number;ONES=number;NUMBER=number.",
            ShowMultipleWays=>$"Original Show Me Your Number: represent {total} in TWO mathematically equivalent ways. First use {tens} tens and {ones} ones (= {10*tens}+{ones}). Then trade {Math.Min(tens,mode+1)} full ten-rods for loose ones. Fill both forms: EXPANDED=ten_value+ones;TENS=number;ONES=number;ALT_TENS=number;ALT_ONES=number. Drawings and gallery walk require teacher review.",
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
        int t=p["tens"],o=p["ones"],mode=p["mode"],n=10*t+o;
        return family switch
        {
            MixedCollection=>mode==5?
                "DECISION=DISAGREE;TENS=4;ONES=8;TOTAL=48;CLAIM=50":
                $"TENS={t};ONES={o};TOTAL={n}",
            ReadNumber=>mode==4?
                "DIGITS=3,5;NUMBER=35;TENS=3;ONES=5;NAME=thirty-five":
                mode==5?
                "AGREE=DIEGO;TENS=8;ONES=6;NUMBER=86;NAME=eighty-six":
                mode==6?
                "DECISION=DISAGREE;DRAWN=42;CLAIM=24;TENS=4;ONES=2":
                $"NAME={NumberWords(t,o)}",
            MatchRepresentations=>$"TENS={t};ONES={o};NUMBER={n}",
            ShowMultipleWays=>$"EXPANDED={10*t}+{o};TENS={t};ONES={o};ALT_TENS={t-Math.Min(t,mode+1)};ALT_ONES={o+10*Math.Min(t,mode+1)}",
            WriteNumeral=>$"NUMBER={n}",
            _=>throw new InvalidOperationException()
        };
    }
    public static bool GradePersistedAnswer(string? family,string? expected,string? response)
    {
        if(!Supports(family)||string.IsNullOrWhiteSpace(expected)||string.IsNullOrWhiteSpace(response))
            return false;
        // Source card 3/5 task admits either learner-chosen two-digit order.
        // The persisted-answer route has only the canonical answer text, so
        // apply the same two-way rule here, not solely in parameterized Verify.
        if(family==ReadNumber&&
           Norm(expected)=="DIGITS=3,5;NUMBER=35;TENS=3;ONES=5;NAME=THIRTY-FIVE"&&
           GradePersistedAnswer(family,
               "DIGITS=3,5;NUMBER=53;TENS=5;ONES=3;NAME=fifty-three",response))
            return true;
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
    // Original Show Me Your Number does not prescribe which ten-rods a
    // student trades. Accept any second, DIFFERENT mathematically equivalent
    // nonstandard tens-and-ones model, not only the one seeded by the engine.
    private static bool VerifyStudentChosenRegrouping(
        IReadOnlyDictionary<string,int> p,string? candidate)
    {
        var fields=new Dictionary<string,string>(StringComparer.Ordinal);
        var pieces=Norm(candidate).Split(';');
        if(pieces.Length!=5)return false;
        foreach(var piece in pieces)
        {
            var separatorIndex=piece.IndexOf('=');
            if(separatorIndex<=0||separatorIndex==piece.Length-1||
               !fields.TryAdd(piece[..separatorIndex],piece[(separatorIndex+1)..]))return false;
        }
        int t=p["tens"],o=p["ones"];
        if(fields.Count!=5||!fields.TryGetValue("EXPANDED",out var expanded)||
            expanded!=$"{t*10}+{o}"||
            !fields.TryGetValue("TENS",out var originalTens)||
            !fields.TryGetValue("ONES",out var originalOnes)||
            !fields.TryGetValue("ALT_TENS",out var alternateTens)||
            !fields.TryGetValue("ALT_ONES",out var alternateOnes)||
            !int.TryParse(originalTens,out var qt)||qt!=t||
            !int.TryParse(originalOnes,out var qo)||qo!=o||
            !int.TryParse(alternateTens,out var at)||at<0||at>=t||
            !int.TryParse(alternateOnes,out var ao)||ao<10)return false;
        return 10*at+ao==10*t+o;
    }

    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? candidate)=>
        Valid(family,p)&&
        (family==ShowMultipleWays?VerifyStudentChosenRegrouping(p,candidate):
            GradePersistedAnswer(family,Solve(family!,p),candidate));
}
