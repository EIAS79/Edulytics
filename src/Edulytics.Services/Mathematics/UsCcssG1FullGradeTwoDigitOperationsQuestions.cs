using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// G1 new U04 L11-L15, ordinals 76-80. These are gradeable parts only.
/// Partner centers, concrete base-ten models and oral reasoning remain teacher-assessed.
/// L14 uses comparison WORDS; L15 introduces the formal symbols.
/// </summary>
public static class UsCcssG1FullGradeTwoDigitOperationsQuestions
{
    public const string AddTens="usccss.g1.u04.l11.add_tens.two_digit";
    public const string TenMoreLess="usccss.g1.u04.l12.mental.ten_more_less";
    public const string CenterReview="usccss.g1.u04.l13.center.place_value_mix";
    public const string CompareWords="usccss.g1.u04.l14.compare.two_digits_words";
    public const string CompareSymbols="usccss.g1.u04.l15.compare.symbols";
    public static readonly IReadOnlyList<string> Families=
        [AddTens,TenMoreLess,CenterReview,CompareWords,CompareSymbols];
    public sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is not null&&Families.Contains(family,StringComparer.Ordinal);
    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=4||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("mode",out int mode)||!p.TryGetValue("unused",out int z)||
           z!=0||mode<0||mode>12||(mode>3&&family!=AddTens&&family!=CenterReview&&family!=CompareWords))return false;
        return family switch
        {
            AddTens=>mode<=3?
                (a>=20&&a<=79&&b>=10&&b<=30&&b%10==0&&a+b<=99):
                mode==4?
                ((a==37&&b==20)||(a==60&&b==23)||(a==48&&b==50)||
                 (a==54&&b==20)||(a==30&&b==65)):
                mode==5?(a==80&&b==5):
                mode==6?(a==70&&b==1):
                mode==7?(a==20&&b==6):
                mode==8?(a==46&&b==30):
                mode==9?(a==45&&b==20):
                mode==10?(a==43&&b==20):
                mode==11?(a==56&&b==40):
                (a==3&&b==30),
            TenMoreLess=>mode switch
            {
                0 or 1=>a>=20&&a<=89&&b==10,
                2=>b==30&&(a==67||a==99),
                3=>b==20&&a==45,
                _=>false
            },
            CompareWords or CompareSymbols=>a>=20&&a<=99&&b>=20&&b<=99,
            CenterReview=>mode switch
            {
                4=>a==70&&b==20,
                5=>a==8&&b==7,
                6=>a==9&&b==6,
                0=>a>=11&&a<=17&&b>=1&&a+b<=20,
                1=>a>=11&&a<=20&&b>=1&&b<=9&&b<a,
                2=>a>=20&&a<=69&&b>=10&&b<=30&&b%10==0&&a+b<=99,
                _=>a>=1&&a<=9&&b>=0&&b<=9
            },
            _=>false
        };
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G1 two-digit source family");
        int mode=random.Next(family==AddTens?13:family==CenterReview?7:4),a,b;
        if(family==AddTens)
        {
            if(mode==4)
            {
                var tasks=new[]{(37,20),(60,23),(48,50),(54,20),(30,65)};
                (a,b)=tasks[random.Next(tasks.Length)];
            }
            else if(mode==5){a=80;b=5;}
            else if(mode==6){a=70;b=1;}
            else if(mode==7){a=20;b=6;}
            else if(mode==8){a=46;b=30;}
            else if(mode==9){a=45;b=20;}
            else if(mode==10){a=43;b=20;}
            else if(mode==11){a=56;b=40;} // Public original illustration ALT description: 56 + [smudged tens]0 = 96.
            else if(mode==12){a=3;b=30;} // Public original ALT description: [smudged tens]3 + 30 = 74 is false.
            else{a=random.Next(20,80);b=random.Next(1,Math.Min(3,(99-a)/10)+1)*10;}
        }
        else if(family==TenMoreLess)
        {
            if(mode==2){a=random.Next(2)==0?67:99;b=30;}
            else if(mode==3){a=45;b=20;}
            else{a=random.Next(20,90);b=10;}
        }
        else if(family is CompareWords or CompareSymbols)
        {
            a=random.Next(20,100);b=mode==0?a:random.Next(20,100);
        }
        else
        {
            if(family==CenterReview&&mode==4){a=70;b=20;}
            else if(family==CenterReview&&mode==5){a=8;b=7;}
            else if(family==CenterReview&&mode==6){a=9;b=6;}
            else if(mode==0){a=random.Next(11,18);b=random.Next(1,21-a);}
            else if(mode==1){a=random.Next(11,21);b=random.Next(1,10);}
            else if(mode==2){a=random.Next(20,70);b=random.Next(1,4)*10;}
            else {a=random.Next(1,10);b=random.Next(0,10);}
        }
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
            {["a"]=a,["b"]=b,["mode"]=mode,["unused"]=0};
        if(!Valid(family,p))throw new InvalidOperationException("Source family generated invalid grade 1 operands");
        string prompt=family switch
        {
            AddTens=>mode==4?
                (a==54?
                    "EDULYTICS ORIGINAL-SOURCE-ALIGNED NUMERIC TASK: Fill the left box in □=54+20. Write TOTAL=number;ONES=number.":
                    $"EDULYTICS SOURCE-GOAL NUMERIC TASK: Find {a}+{b}, combining complete tens and preserving or identifying leftover ones. Write TOTAL=number;ONES=number. Drawings need teacher review.") :
                mode==5?
                "ORIGINAL 2021 TRUE/FALSE WARM-UP: Is 80+5=5+80 true? Evaluate BOTH sides. Write TRUE=YES/NO;LEFT=number;RIGHT=number.":
                mode==6?
                "ORIGINAL 2021 TRUE/FALSE WARM-UP: Is 70+1=80+1 true? Evaluate BOTH sides. Write TRUE=YES/NO;LEFT=number;RIGHT=number.":
                mode==7?
                "ORIGINAL 2021 TRUE/FALSE WARM-UP: Is 20+6=6+30 true? Evaluate BOTH sides. Write TRUE=YES/NO;LEFT=number;RIGHT=number.":
                mode==8?
                "EDULYTICS ORIGINAL EXIT CHECK (not the inaccessible publisher Cool-down): Add 46 and 30. Four tens plus three tens gives seven tens; six ones remain. Write TOTAL=number;ONES=number. Teacher reviews drawings.":
                mode==9?
                "EDULYTICS ORIGINAL MISSING-DIGIT TASK, not reconstructed from the publisher image: 4□+20=65 is true. Which ONES digit belongs in the box? Write DIGIT=number;EQUATION=number+number=number;TRUE=YES/NO.":
                mode==10?
                "EDULYTICS ORIGINAL FALSE-EQUATION CHECK, not reconstructed from the publisher image: Could 4□+20=75 ever be true for a single missing ONES digit 0–9? Identify the greatest possible total and judge the claim. Write TRUE=YES/NO;MAX=number;TARGET=number.":
                mode==11?
                "SOURCE-ALIGNED FIRST-PARTY DIGIT PUZZLE (public 2021 source image accessibility text, NOT reused image): 56 plus some WHOLE tens equals 96: 56+□0=96. What TENS digit does □ represent? Write DIGIT=number;EQUATION=number+number=number;TRUE=YES/NO. A teacher reviews a drawing.":
                mode==12?
                "SOURCE-ALIGNED FIRST-PARTY FALSE-EQUATION PUZZLE (public 2021 image accessibility text, NOT reused image): □3+30=74 is said to be NOT TRUE. Could ANY TENS digit in □ make the equation true? Explain by comparing ones digits 3 and 4. Write TRUE=YES/NO;ONES_LEFT=number;ONES_RIGHT=number. Teacher reviews words or drawings.":
                $"Add a multiple of ten to a two-digit number: {a}+{b}. Tens join tens and ONES stay the same. Write TOTAL=number;ONES=number.",
            TenMoreLess=>mode switch
            {
                2=>a==67?
                    "SOURCE-ALIGNED NUMERIC SEQUENCE: Start at 67 and add 10 three times: 67+10+10+10. Write RESULT=number. Explain the tens pattern to a teacher.":
                    "SOURCE-ALIGNED NUMERIC SEQUENCE: Start at 99 and subtract 10 three times: 99-10-10-10. Write RESULT=number. Explain the tens pattern to a teacher.",
                3=>"SOURCE-ALIGNED NUMERIC SEQUENCE: Start at 45. Find 45+10+10, 45-10-10, and 45+10-10. Write PLUS=number;MINUS=number;RETURN=number. Explain why the last sequence returns to 45.",
                0=>$"What is TEN MORE than {a}? Write RESULT=number.",
                _=>$"What is TEN LESS than {a}? Write RESULT=number."
            },
            CenterReview=>mode switch
            {
                4=>"SOURCE-ALIGNED CHECK IT OFF CENTER: Starting with 70 and 20, find the addition and subtraction results. Write ADD=number;SUBTRACT=number. Actual partner play needs teacher review.",
                5=>"SOURCE-ALIGNED HOW CLOSE? CENTER: Select exactly three DISTINCT cards from the hand 8,4,1,7,5 to total 20. One valid selection uses 8,7,5. Write TOTAL=number;CARDS=8+7+5. Real center play needs teacher review.",
                6=>"SOURCE-ALIGNED NUMBER TALK: Find 9+6 and 15-6; explain how the operations relate. Write ADD=number;SUBTRACT=number. Oral explanation needs teacher review.",
                0=>$"Teacher-led center numeric portion: calculate {a}+{b} within 20. Write RESULT=number.",
                1=>$"Teacher-led center numeric portion: calculate {a}-{b} within 20. Write RESULT=number.",
                2=>$"Teacher-led place value center numeric portion: calculate {a}+{b}. Write RESULT=number.",
                _=>$"Teacher-led counting center numeric portion: count {a} tens and {b} loose ones. Write RESULT=number."
            },
            CompareWords=>$"Compare the value of {a} and {b} by tens, then ones. Is the FIRST number GREATER, LESS or EQUAL? Write RELATION=GREATER/LESS/EQUAL.",
            CompareSymbols=>$"Compare {a} and {b}. Write the mathematical comparison SYMBOL between the two numbers, one of >, < or =. Write SYMBOL=sign.",
            _=>throw new InvalidOperationException()
        };
        return new(family,prompt,
          "Show tens/ones reasoning. Oral responses and real classroom center participation cannot be certified through one numeric answer.",
          AssessmentItemType.ShortAnswer,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Source family parameters out of range");
        int a=p["a"],b=p["b"],mode=p["mode"];
        return family switch
        {
            AddTens=>mode==5?
                "TRUE=YES;LEFT=85;RIGHT=85":
                mode==6?
                "TRUE=NO;LEFT=71;RIGHT=81":
                mode==7?
                "TRUE=NO;LEFT=26;RIGHT=36":
                mode==9?
                "DIGIT=5;EQUATION=45+20=65;TRUE=YES":
                mode==10?
                "TRUE=NO;MAX=69;TARGET=75":
                mode==11?
                "DIGIT=4;EQUATION=56+40=96;TRUE=YES":
                mode==12?
                "TRUE=NO;ONES_LEFT=3;ONES_RIGHT=4":
                $"TOTAL={a+b};ONES={(a+b)%10}",
            TenMoreLess=>mode==3?
                "PLUS=65;MINUS=25;RETURN=45":
                $"RESULT={(mode==2?(a==67?a+30:a-30):mode==0?a+10:a-10)}",
            CenterReview=>mode switch
            {
                4=>"ADD=90;SUBTRACT=50",
                5=>"TOTAL=20;CARDS=8+7+5",
                6=>"ADD=15;SUBTRACT=9",
                _=>$"RESULT={(mode==0?a+b:mode==1?a-b:mode==2?a+b:10*a+b)}"
            }
            CompareWords=>$"RELATION={(a>b?"GREATER":a<b?"LESS":"EQUAL")}",
            CompareSymbols=>$"SYMBOL={(a>b?">":a<b?"<":"=")}",
            _=>throw new InvalidOperationException()
        };
    }
    private static string Norm(string? s)=>
        string.Concat((s??"").Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();
    public static bool GradePersistedAnswer(string? family,string? expected,string? submitted)=>
        Supports(family)&&!string.IsNullOrWhiteSpace(expected)&&!string.IsNullOrWhiteSpace(submitted)&&
        Norm(expected)==Norm(submitted);
    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? answer)=>
        Valid(family,p)&&GradePersistedAnswer(family,Solve(family!,p),answer);
}
