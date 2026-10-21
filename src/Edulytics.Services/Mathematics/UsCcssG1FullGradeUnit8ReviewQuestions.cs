using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original G1 U08 L01-L10 digital calculation and interpretation subsets.
/// Student-created riddles, oral PLC and hands-on 120-count activities require
/// separate human evidence; auto-correct answers DO NOT certify authorship.
/// </summary>
public static class UsCcssG1FullGradeUnit8ReviewQuestions
{
    public static readonly IReadOnlyList<string> Families =
    [
        "usccss.g1.u08.l01.addition_fluency_within10",
        "usccss.g1.u08.l02.inverse_add_subtract",
        "usccss.g1.u08.l03.fluency_within20",
        "usccss.g1.u08.l04.change_unknown_story",
        "usccss.g1.u08.l05.missing_part_story",
        "usccss.g1.u08.l06.compare_difference_story",
        "usccss.g1.u08.l07.count_on_to120",
        "usccss.g1.u08.l08.same_number_tens_ones",
        "usccss.g1.u08.l09.solve_number_riddle",
        "usccss.g1.u08.l10.write_riddle_numeric_check"
    ];
    public sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static int Index(string? family)
    {
        if(family==null)return -1;
        for(int i=0;i<Families.Count;i++)
            if(Families[i]==family)return i;
        return -1;
    }
    public static bool Supports(string? family)=>Index(family)>=0;
    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        int n=Index(family);
        if(n<0||p.Count!=4||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("c",out int c)||!p.TryGetValue("mode",out int mode)||
           mode<0||mode>(n==1?13:n==2?8:n==3?11:n==4?12:n==5?7:n==7?5:n==8?9:n==9?8:3))return false;
        return n switch
        {
            0=>a>=1&&a<=9&&b>=1&&b<=9&&a+b<=10&&c==0,
            1=>mode>=4?c==0&&(mode switch
                {
                    4=>a==5&&b==4,
                    5=>a==4&&b==5,
                    6=>a==9&&b==4,
                    7=>a==9&&b==5,
                    8=>a==4&&b==5,
                    9=>a==8&&b==5,
                    10=>a==8&&b==3,
                    11=>a==7&&b==4,
                    12=>a==8&&b==2,
                    13=>a==8&&b==4,
                    _=>false
                }):
                a>=1&&a<=9&&b>=1&&b<=9&&a+b<=10&&c==0,
            2=>mode>=4?c==0&&(mode switch
            {
                4=>a==6&&b==7,
                5=>a==13&&b==7,
                6=>a==7&&b==8,
                7=>a==15&&b==7,
                8=>a==5&&b==9,
                _=>false
            }):a>=2&&a<=19&&b>=1&&b<=9&&a+b<=20&&a>=b&&c==0,
            3=>mode>=9?(mode switch
            {
                9=>a==10&&b==7&&c==0,
                10=>a==8&&b==2&&c==7,
                11=>a==10&&b==5&&c==2,
                _=>false
            }):c==0&&(mode switch
            {
                4=>a==8&&b==7,
                5=>a==17&&b==12,
                6=>a==20&&b==17,
                7=>a==6&&b==8,
                8=>a==16&&b==7,
                _=>a>=2&&a<=19&&b>=1&&b<=9&&a+b<=20
            }),
            4=>mode>=10?(mode switch
            {
                10=>a==57&&b==20&&c==59,
                11=>a==66&&b==4&&c==62,
                12=>a==17&&b==76&&c==59,
                _=>false
            }):c==0&&(mode switch
            {
                4=>a==7&&b==11,
                5=>a==13&&b==5,
                6=>a==6&&b==9,
                7=>a==12&&b==8,
                8=>a==9&&b==8,
                9=>a==11&&b==5,
                _=>a>=2&&a<=19&&b>=1&&b<=9&&a+b<=20
            }),
            5=>c==0&&(mode switch
            {
                4=>a==12&&b==7,
                5=>a==17&&b==7,
                6=>a==13&&b==9,
                7=>a==20&&b==14,
                _=>a>=7&&a<=20&&b>=1&&b<a
            }),
            6=>c==0&&(mode<=1?
                a>=1&&a<=119&&b>=1&&b<=120-a:
                a>=20&&a<=120&&b==0),
            7=>mode==5?a==84&&b==0&&c==0:
                mode==4?a>=20&&a<=100&&b==0&&c==0:
                a>=20&&a<=100&&b==0&&c==0,
            8=>mode switch
            {
                4=>a==65&&b==35&&c==0,
                5=>a==65&&b==65&&c==0,
                6=>a==65&&b==65&&c==0,
                7=>a==34&&b==44&&c==0,
                8=>a==24&&b==20&&c==0,
                9=>a==51&&b==32&&c==0,
                _=>a>=31&&a<=79&&a%10>0&&b==10&&c==0
            },
            9=>mode switch
            {
                4=>a==60&&b==70&&c==0,
                5=>a==70&&b==70&&c==0,
                6=>a==80&&b==69&&c==0,
                7=>a==47&&b==40&&c==0,
                _=>a>=1&&a<=120&&b>=0&&b<=a&&c==0
            },
            _=>false
        };
    }
    public static Problem Build(string family,Random random)
    {
        int n=Index(family);
        if(n<0)throw new InvalidOperationException("Unreviewed Grade1 U08 family");
        int mode=random.Next(n==1?14:n==2?9:n==3?12:n==4?13:n==5?8:n==7?6:n==8?10:n==9?9:4),a=0,b=0,c=0;
        for(int attempts=0;attempts<1000;attempts++)
        {
            if(n<=1){a=random.Next(1,10);b=random.Next(1,10);}
            else if(n<=4){a=random.Next(2,20);b=random.Next(1,10);}
            else if(n==5){a=random.Next(7,21);b=random.Next(1,a);}
            else if(n==6)
            {
                if(mode<=1)
                {
                    a=random.Next(1,120);
                    b=random.Next(1,Math.Min(6,121-a));
                }
                else
                {
                    a=random.Next(20,121);
                    b=0;
                }
            }
            else if(n==9){a=random.Next(1,121);b=random.Next(0,a+1);}
            else if(n==7){a=mode==5?84:random.Next(20,101);b=0;}
            else if(n==8){a=random.Next(3,8)*10+random.Next(1,10);b=10;}
            else{a=random.Next(20,101);b=n>=8?10:0;}
            if(n==1&&mode>=4)(a,b)=mode switch
            {
                4=>(5,4),5=>(4,5),6=>(9,4),7=>(9,5),8=>(4,5),
                9=>(8,5),10=>(8,3),11=>(7,4),12=>(8,2),13=>(8,4),
                _=>throw new InvalidOperationException("Unreviewed original related-facts example")
            };
            if(n==2&&mode>=4)(a,b)=mode switch
            {
                4=>(6,7),5=>(13,7),6=>(7,8),7=>(15,7),8=>(5,9),
                _=>throw new InvalidOperationException("Unreviewed original 2021 L03 mode")
            };
            if(n==3&&mode>=4&&mode<=8)(a,b)=mode switch
            {
                4=>(8,7),5=>(17,12),6=>(20,17),7=>(6,8),8=>(16,7),
                _=>throw new InvalidOperationException("Unexpected change-unknown story mode")
            };
            if(n==3&&mode>=9)(a,b,c)=mode switch
            {
                9=>(10,7,0),10=>(8,2,7),11=>(10,5,2),
                _=>throw new InvalidOperationException("Unreviewed aquarium mental warm-up")
            };
            if(n==4&&mode>=4&&mode<=9)(a,b)=mode switch
            {
                4=>(7,11),5=>(13,5),6=>(6,9),7=>(12,8),8=>(9,8),9=>(11,5),
                _=>throw new InvalidOperationException("Unexpected missing-part story mode")
            };
            if(n==4&&mode>=10)(a,b,c)=mode switch
            {
                10=>(57,20,59),11=>(66,4,62),12=>(17,76,59),
                _=>throw new InvalidOperationException("Unreviewed original planetarium number talk")
            };
            if(n==5&&mode>=4)(a,b)=mode switch
            {
                4=>(12,7),5=>(17,7),6=>(13,9),7=>(20,14),
                _=>throw new InvalidOperationException("Unexpected carnival story mode")
            };
            if(n==8&&mode>=4)(a,b)=mode switch
            {
                4=>(65,35),5=>(65,65),6=>(65,65),
                7=>(34,44),8=>(24,20),9=>(51,32),
                _=>throw new InvalidOperationException("Unreviewed original number-riddle source")
            };
            if(n==9&&(mode is >=4 and <=7))(a,b)=mode switch
            {
                4=>(60,70),5=>(70,70),6=>(80,69),7=>(47,40),
                _=>throw new InvalidOperationException("Unexpected source number-riddle mode")
            };
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"mode",mode}};
            if(!Valid(family,p))continue;
            var whole=a+b;
            string prompt=n switch
            {
                0=>$"Find the sum within ten: {a}+{b}. Write RESULT=number.",
                1=>mode>=9?
                    mode switch
                    {
                        9=>"ORIGINAL 2021 RELATED FACTS: □+5=8. Write MISSING=number.",
                        10=>"ORIGINAL 2021 RELATED FACTS: 8-3=□. Write MISSING=number.",
                        11=>"ORIGINAL 2021 RELATED FACTS: 7-4=□. Write MISSING=number.",
                        12=>"ORIGINAL 2021 RELATED FACTS: 8=2+□. Write MISSING=number.",
                        _=>"ORIGINAL 2021 RELATED FACTS: 8-□=4. Write MISSING=number."
                    }:
                    mode==8?
                    "ORIGINAL 2021 UNIT 8 LESSON 2 STUDENT MISSING NUMBERS: "+
                    "4+□=9 and 9-5=□. Write MISSING_ADD=number;MISSING_SUB=number. "+
                    "Teacher reviews connecting cubes and partner explanation.":
                    mode>=4?mode switch
                    {
                        4=>"ORIGINAL 2021 RELATED FACTS: Find 5+4. Write RESULT=number.",
                        5=>"ORIGINAL 2021 RELATED FACTS: Find 4+5. Write RESULT=number.",
                        6=>"ORIGINAL 2021 RELATED FACTS: Find 9-4. Write RESULT=number.",
                        _=>"ORIGINAL 2021 RELATED FACTS: Find 9-5. Write RESULT=number."
                    }:
                    mode<=1?
                    $"The two parts are {a} and {b}, with whole {whole}. If {a}+{b}={whole}, remove the {b} part from the whole. Write LEFT=number;WHOLE=number.":
                    mode==2?
                    $"ORIGINAL CUBE TOWERS: Two connecting-cube towers contain {a} and {b} cubes. Write ONE complete addition equation and ONE matching subtraction equation using the same whole, the two parts and the equals sign. Write ADD=number+number=number;SUB=number-number=number.":
                    $"ORIGINAL MISSING NUMBER: Find the TWO unknown values for {a} + □ = {whole} and {whole} - {b} = □. Write MISSING_ADD=number;MISSING_SUB=number.",
                2=>mode==8?
                    "ORIGINAL 2021 UNIT 8 LESSON 3 CHOOSE ONE OF EACH: "+
                    "ADDITION expressions 5+9, 4+7, 6+4+4; SUBTRACTION expressions "+
                    "14-8, 15-9, 13-6. Choose ANY one valid addition and ANY "+
                    "one valid subtraction, find both, and write ADD=equation;SUB=equation. "+
                    "Source circles, personal methods and partner discussion are teacher-assessed.":
                    mode>=4?
                    $"ORIGINAL 2021 UNIT 8 LESSON 3 MENTAL WARM-UP: "+
                    $"{a}{(mode%2==0?"+":"-")}{b}. Write RESULT=number.":
                    mode%2==0?$"Add within 20: {a}+{b}. Write RESULT=number.":
                    $"Subtract within 20: {a}-{b}. Write RESULT=number.",
                3=>mode>=9?
                    $"ORIGINAL 2021 AQUARIUM MENTAL WARM-UP: "+
                    $"{(mode==9?"10+7":mode==10?"8+2+7":"10+5+2")}. "+
                    "Write RESULT=number and explain how it equals seventeen.":
                    mode switch
                {
                    4=>"ORIGINAL 2021 AQUARIUM: Clare sees 8 sea turtles, then some more join; now 15. How many JOINED? Write CHANGE=number;FINAL=number.",
                    5=>"ORIGINAL 2021 AQUARIUM: 17 penguins sit on rocks, some swim away, and 5 remain. How many LEFT? Write CHANGE=number;FINAL=number.",
                    6=>"ORIGINAL 2021 AQUARIUM: Seals eat some of a bucket of 20 fish, leaving 3 fish. How many were EATEN? Write CHANGE=number;FINAL=number.",
                    7=>"ORIGINAL 2021 AQUARIUM: Tyler feels 6 stingrays, then some sea stars, until there are 14 animals in all. How many SEA STARS? Write CHANGE=number;FINAL=number.",
                    8=>"ORIGINAL 2021 AQUARIUM ANDRE: 16 bullfrogs at first, then some leave and 9 remain. Explain the missing-change subtraction and check by inverse addition. How many LEFT? Write CHANGE=number;FINAL=number. Real pupil strategy is teacher-reviewed.",
                    _=>mode%2==0?$"There are {a} counters and some JOIN. Now there are {whole}. How many joined? Write CHANGE=number;FINAL=number.":
                        $"There are {whole} counters. Some LEAVE. Now {a} remain. How many left? Write CHANGE=number;FINAL=number."
                },
                4=>mode>=10?
                    $"ORIGINAL 2021 UNIT 8 LESSON 5 TRUE/FALSE WARM-UP: "+
                    (mode==10?"57+20=59":mode==11?"66-4=62":"17+76=59")+
                    ". Write TRUE=YES/NO;ACTUAL=number;STATED=number; "+
                    "explain why the equality holds or fails.":
                    mode switch
                {
                    4=>"ORIGINAL PLANETARIUM: 7 first graders and some second graders make 18 altogether. How many second graders? Write MISSING=number;TOTAL=number.",
                    5=>"ORIGINAL PLANETARIUM: There are 18 stars and 13 are bright; the rest are dim. How many dim stars? Write MISSING=number;TOTAL=number.",
                    6=>"ORIGINAL PLANETARIUM: Diego saw 6 shooting stars; Diego and Tyler together saw 15. How many did Tyler see? Write MISSING=number;TOTAL=number.",
                    7=>"ORIGINAL PLANETARIUM: Elena bought 12 star stickers and some planet stickers, 20 total. How many planet stickers? Write MISSING=number;TOTAL=number.",
                    8=>"ORIGINAL NOAH TWO-EQUATION CHECK: Noah painted 9 white stars in a painting of 17 total stars. Circle TWO matching equations: A: 17-9=box. B: 9+17=box. C: 9+box=17. Write MATCH=first_letter,second_letter;MISSING=number.",
                    9=>"ORIGINAL KIRAN TWO-EQUATION CHECK: Kiran sees 11 stars among 16 sky objects; the rest are planets. Circle TWO matching equations: A: 11+box=16. B: 16+11=box. C: 16-11=box. Write MATCH=first_letter,second_letter;MISSING=number.",
                    _=>$"There are {whole} objects total; one part has {a} objects. How many are in the other part? Write MISSING=number;TOTAL=number."
                },
                5=>mode switch
                {
                    4=>"ORIGINAL SCHOOL CARNIVAL: Lin has 7 ride tickets, Mai has 12. How many MORE tickets does Mai have? Write DIFFERENCE=number.",
                    5=>"ORIGINAL SCHOOL CARNIVAL: 17 bags of BLUE cotton candy and 7 bags of PINK cotton candy. How many MORE blue? Write DIFFERENCE=number.",
                    6=>"ORIGINAL SCHOOL CARNIVAL: There are 13 toys and 9 are stuffed animals. How many FEWER stuffed animals than toys? Write DIFFERENCE=number.",
                    7=>"ORIGINAL SCHOOL CARNIVAL: Ferris wheel holds 20 people and the swings 14. How many FEWER people can ride the swings? Write DIFFERENCE=number. A separate authored task asks learners to write TWO ride questions for teacher review.",
                    _=>$"There are {a} blue cubes and {b} red cubes. How many MORE blue cubes than red cubes? Write DIFFERENCE=number."
                },
                6=>mode<=1?
                    $"Last Number Wins: start at {a} (not at one); count ON {b} hops up to 120. Write LAST=number.":
                    $"Count an organized collection of objects with tens bundles and loose ones. The display has NO double-counted cubes and contains between 20 and 120 objects. How many are there? Write TENS=number;ONES=number;TOTAL=number.",
                7=>mode==5?
                    "ORIGINAL 2021 UNIT 8 LESSON 8 NUMBER BOOK: Start from "+
                    "the teacher's example number 84. Write AT LEAST THREE "+
                    "DISTINCT correct addition expressions for eighty-four, "+
                    "such as two addends in each. Enter "+
                    "NUMBER=84;E1=x+y;E2=x+y;E3=x+y. "+
                    "Different mathematical decompositions are accepted. "+
                    "An authentic pupil book PAGE with visual tens/ones models "+
                    "and the classroom gallery walk must be assessed by a teacher.":
                    mode==4?
                    $"ORIGINAL 2021 NUMBER BOOK, FIRST-PARTY STRUCTURED RECORD: Choose number {a} from the source 20 through 100 range. Write NUMBER=number;TENS=number;ONES=number to show its standard tens-and-ones value. Real pupil book composition and illustrated representations require teacher review.":
                    mode<=1?
                    $"Show the number {a} in tens and ones (the source range includes 100). Then exchange ONE full ten-rod for ten loose ones without changing the total. Write NEW_TENS=number;NEW_ONES=number;TOTAL=number.":
                    mode==2?
                    $"ORIGINAL SHOW ME ALL THE WAYS: Start with {a/10} tens and {a%10} ones representing {a}. TRADE TWO full ten-rods for 20 more loose ones to show a SECOND different model. Write ALT_TENS=number;ALT_ONES=number;TOTAL=number.":
                    $"ORIGINAL FAVORITE NUMBER GALLERY: The number is {a}. Exchange any number of complete ten-rods for ten loose ones. How many DISTINCT tens-and-ones models exist when ZERO full tens is allowed, and how many loose ones remain in the all-loose model? Write WAYS=number;ZERO_TENS_ONES=number;TOTAL=number.",
                8=>mode>=4?mode switch
                    {
                        4=>"ORIGINAL 2021 TRUE/FALSE: Is 65>35 true? Write TRUE=YES/NO;LEFT=number;RIGHT=number.",
                        5=>"ORIGINAL 2021 TRUE/FALSE: Is 65=75-10 true? Write TRUE=YES/NO;LEFT=number;RIGHT=number.",
                        6=>"ORIGINAL 2021 TRUE/FALSE: Is 65>35+30 true? Write TRUE=YES/NO;LEFT=number;RIGHT=number.",
                        7=>"ORIGINAL 2021 FIRST THREE CLUES: The number is more than 30, less than 60, and has four ones. List ALL three candidates in ascending order as CANDIDATES=n1,n2,n3.",
                        8=>"ORIGINAL 2021 FINAL CLUE: From possible 34,44,54, choose the one equal to 24+20. Write NUMBER=number.",
                        _=>"ORIGINAL 2021 LESSON SYNTHESIS: Number has more than six tens, is less than 100, has eight tens, and equals 51+32. Write NUMBER=number."
                    }:
                    mode%2==0?
                    $"NUMBER RIDDLE: My secret number has {a%10} ones. It is GREATER than {(a/10)*10} and LESS than {(a/10+3)*10}. List ALL three possible numbers in order from smallest to largest. Write CANDIDATES=n1,n2,n3.":
                    $"NUMBER RIDDLE: My secret number has {a%10} ones. It is GREATER than {(a/10)*10} and LESS than {(a/10+3)*10}. First find the three possible numbers. FINAL clue: it is exactly 10 MORE than {a}. Which one number now satisfies ALL four clues? Write NUMBER=number.",
                9=>mode switch
                {
                    4=>"ORIGINAL 2021 TRUE/FALSE: Is 60=25+45 true? Write TRUE=YES/NO;LEFT=number;RIGHT=number.",
                    5=>"ORIGINAL 2021 TRUE/FALSE: Is 70=24+46 true? Write TRUE=YES/NO;LEFT=number;RIGHT=number.",
                    6=>"ORIGINAL 2021 TRUE/FALSE: Is 80=32+37 true? Write TRUE=YES/NO;LEFT=number;RIGHT=number.",
                    7 or 8=>$"FIRST-PARTY ORIGINAL-GOAL WRITING SCAFFOLD: Take secret number {a}. Author four CONCRETE clues: (1) choose a LOWER bound strictly smaller than {a}, (2) choose an UPPER bound strictly greater than {a}, (3) state full TENS and ONES for {a}, (4) write an ADDITION expression equal to {a}. Enter NUMBER=n;LOWER=n;UPPER=n;TENS=n;ONES=n;ADD=x+y. All four must be consistent. Student-chosen bounds and addition are accepted; a teacher reviews real written clues and partner riddle discussion.",
                    _=>$"Review a proposed FOUR-clue number riddle for a secret number within 1 to 120 (not proof of authorship): Clue 1: My number is more than {a-1}. Clue 2: My number is less than {a+1}. Clue 3: My number has {a/10} tens and {a%10} ones. Clue 4, an addition expression: {b} + {a-b+(mode%2)}. Decide whether ALL four clues are consistent. Write NUMBER=number;VALID=YES/NO."
                },
                _=>throw new InvalidOperationException()
            };
            return new(family,prompt,"Use first-grade addition, difference, grouping and place value reasoning. Actual student-authored riddles, drawings and PLC explanations require independent teacher evidence.",AssessmentItemType.ShortAnswer,p);
        }
        throw new InvalidOperationException("Could not sample original-scope Grade 1 review problem");
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid original-scope G1 U08 instance");
        int n=Index(family),a=p["a"],b=p["b"],mode=p["mode"];
        return n switch
        {
            0=>$"RESULT={a+b}",
            1=>mode>=9?$"MISSING={(mode==9?3:mode==10?5:mode==11?3:mode==12?6:4)}":
                mode==8?"MISSING_ADD=5;MISSING_SUB=4":
                mode>=4?$"RESULT={(mode==4||mode==5?9:mode==6?5:4)}":
                mode<=1?$"LEFT={a};WHOLE={a+b}":
                mode==2?$"ADD={a}+{b}={a+b};SUB={a+b}-{b}={a}":
                $"MISSING_ADD={b};MISSING_SUB={a}",
            2=>mode==8?"ADD=5+9=14;SUB=14-8=6":
                $"RESULT={(mode%2==0?a+b:a-b)}",
            3=>mode>=9?$"RESULT={a+b+p["c"]}":
                mode>=4?
                $"CHANGE={b};FINAL={(mode==4||mode==7?a+b:a-b)}":
                mode%2==0?$"CHANGE={b};FINAL={a+b}":$"CHANGE={b};FINAL={a}",
            4=>mode>=10?
                $"TRUE={(mode==11?"YES":"NO")};ACTUAL={(mode==10?77:mode==11?62:93)};STATED={p["c"]}":
                mode==8||mode==9?$"MATCH=A,C;MISSING={b}":
                $"MISSING={b};TOTAL={a+b}",
            5=>$"DIFFERENCE={a-b}",
            6=>mode<=1?$"LAST={a+b}":$"TENS={a/10};ONES={a%10};TOTAL={a}",
            7=>mode==5?"NUMBER=84;E1=20+64;E2=30+54;E3=40+44":
                mode==4?$"NUMBER={a};TENS={a/10};ONES={a%10}":
                mode<=1?$"NEW_TENS={a/10-1};NEW_ONES={a%10+10};TOTAL={a}":
                mode==2?$"ALT_TENS={a/10-2};ALT_ONES={a%10+20};TOTAL={a}":
                $"WAYS={a/10+1};ZERO_TENS_ONES={a};TOTAL={a}",
            8=>mode switch
            {
                4=>"TRUE=YES;LEFT=65;RIGHT=35",
                5=>"TRUE=YES;LEFT=65;RIGHT=65",
                6=>"TRUE=NO;LEFT=65;RIGHT=65",
                7=>"CANDIDATES=34,44,54",
                8=>"NUMBER=44",
                9=>"NUMBER=83",
                _=>mode%2==0?$"CANDIDATES={a},{a+10},{a+20}":$"NUMBER={a+10}"
            },
            9=>mode switch
            {
                4=>"TRUE=NO;LEFT=60;RIGHT=70",
                5=>"TRUE=YES;LEFT=70;RIGHT=70",
                6=>"TRUE=NO;LEFT=80;RIGHT=69",
                7 or 8=>$"NUMBER={a};LOWER={Math.Max(0,a-10)};UPPER={Math.Min(121,a+10)};TENS={a/10};ONES={a%10};ADD={b}+{a-b}",
                _=>$"NUMBER={a};VALID={(mode%2==0?"YES":"NO")}"
            },
            _=>throw new InvalidOperationException()
        };
    }
    private static string Normalize(string? s)=>
        string.Concat((s??"").Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();
    private static bool VerifyFourAuthoredClues(IReadOnlyDictionary<string,int> p,string? response)
    {
        string normalized=Normalize(response);
        var parts=normalized.Split(';');
        if(parts.Length!=6)return false;
        var fields=new Dictionary<string,string>(StringComparer.Ordinal);
        foreach(var part in parts)
        {
            int pos=part.IndexOf('=');
            if(pos<=0||pos==part.Length-1||
               !fields.TryAdd(part[..pos],part[(pos+1)..]))return false;
        }
        if(fields.Count!=6||
           !fields.TryGetValue("NUMBER",out var numberText)||
           !fields.TryGetValue("LOWER",out var lowerText)||
           !fields.TryGetValue("UPPER",out var upperText)||
           !fields.TryGetValue("TENS",out var tensText)||
           !fields.TryGetValue("ONES",out var onesText)||
           !fields.TryGetValue("ADD",out var additionText)||
           !int.TryParse(numberText,out int number)||number!=p["a"]||
           !int.TryParse(lowerText,out int lower)||lower<0||lower>=number||
           !int.TryParse(upperText,out int upper)||upper<=number||upper>121||
           !int.TryParse(tensText,out int tens)||tens!=number/10||
           !int.TryParse(onesText,out int ones)||ones!=number%10)return false;
        string[] addends=additionText.Split('+');
        return addends.Length==2&&
               int.TryParse(addends[0],out int first)&&first>=0&&
               int.TryParse(addends[1],out int second)&&second>=0&&
               first+second==number;
    }

    private static bool VerifyOriginalNumberBook(string? response)
    {
        var parts=Normalize(response).Split(';');
        if(parts.Length!=4)return false;
        var fields=new Dictionary<string,string>(StringComparer.Ordinal);
        foreach(var p in parts)
        {
            int at=p.IndexOf('=');
            if(at<=0||at==p.Length-1||!fields.TryAdd(p[..at],p[(at+1)..]))
                return false;
        }
        if(fields.Count!=4||!fields.TryGetValue("NUMBER",out string? number)||
            number!="84")return false;
        var distinct=new HashSet<string>(StringComparer.Ordinal);
        foreach(var key in new[]{"E1","E2","E3"})
        {
            if(!fields.TryGetValue(key,out var expression))return false;
            var x=expression.Split('+');
            if(x.Length!=2||!int.TryParse(x[0],out int a)||
                !int.TryParse(x[1],out int b)||a<0||b<0||a+b!=84)
                return false;
            if(!distinct.Add($"{Math.Min(a,b)},{Math.Max(a,b)}"))
                return false;
        }
        return true;
    }

    private static bool VerifySourceOneOfEach(string? response)
    {
        var candidate=Normalize(response);
        string[] add=["5+9=14","4+7=11","6+4+4=14"];
        string[] subtract=["14-8=6","15-9=6","13-6=7"];
        return add.Any(a=>subtract.Any(b=>
            candidate==$"ADD={a};SUB={b}"));
    }
    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? response)=>
        Valid(family,p)&&!string.IsNullOrWhiteSpace(response)&&
        (Index(family)==2&&p["mode"]==8?
            VerifySourceOneOfEach(response):
        Index(family)==7&&p["mode"]==5?
            VerifyOriginalNumberBook(response):
        Index(family)==9&&(p["mode"] is 7 or 8)?
            VerifyFourAuthoredClues(p,response):
            Normalize(Solve(family!,p))==Normalize(response));
}
