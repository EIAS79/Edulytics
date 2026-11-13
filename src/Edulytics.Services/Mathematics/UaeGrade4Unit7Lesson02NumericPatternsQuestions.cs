using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit7Lesson02NumericPatternsQuestions
{
    internal const string Constant="uae.g4.u07.l02.constant_numeric_step";
    internal const string Alternating="uae.g4.u07.l02.alternating_numeric_rule";
    private const string G5Sequence="uae.g5.u05.l07.decimal_factor_ten_pattern",G5Arithmetic="uae.g5.u05.l07.arithmetic_step_pattern",
      G5Table="uae.g5.u05.l07.table_double_minus_one",G5Discount="uae.g5.u05.l07.ten_percent_discount_pattern";
    private static bool IsG5(string? f)=>f is G5Sequence or G5Arithmetic or G5Table or G5Discount;
    internal const string G5U6Squares="uae.g5.u06.l07.matchstick_adjacent_squares";
    internal const string G5U6Diagonal="uae.g5.u06.l07.matchstick_square_diagonal";
    internal const string G5U6Triangles="uae.g5.u06.l07.matchstick_shared_triangles";
    internal const string G5U6Compare="uae.g5.u06.l07.compare_three_six_growth";
    private static bool IsG5U6(string? f)=>f is G5U6Squares or G5U6Diagonal or G5U6Triangles or G5U6Compare;
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Constant or Alternating || IsG5(f) || IsG5U6(f) || IsG5L08(f);
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported numeric pattern");
        if(IsG5L08(f))return BuildG5L08(f,r);
        if(IsG5U6(f))return BuildG5U6(f,r);
        if(IsG5(f))return BuildG5(f,r);
        int start=r.Next(20,100),step=r.Next(2,10),index=r.Next(4,9);
        var values=new List<int>{start};
        for(int i=1;i<index;i++)
            values.Add(f==Constant?values[^1]+step:
                values[^1]+(i%2==1?-step:step+2));
        string sequence=string.Join("، ",values.Take(index-1));
        return new Item(f,$"أكمل النمط العددي {sequence}، □. ما العدد التالي؟",
            f==Constant?"أبحث عن الفرق الثابت بين الأعداد وأطبقه.":"أبحث عن قاعدتين تتناوبان بين طرح عدد وإضافة عدد مختلف.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"start",start},{"step",step},{"index",index}});
    }


    // UAE G5 U06 L07 Arabic hands-on toothpick patterns (source printed 443–448).
    // Only the numeric subskills are auto-verified; model drawings need a teacher rubric.
    private static Item BuildG5U6(string family,Random random)
    {
        int index=random.Next(4,10);
        string prompt=family switch
        {
            G5U6Squares=>$"تُبنى مربعات متجاورة بأعواد: الشكل الأول ٤، والثاني ٧، والثالث ١٠ أعواد. كم عودًا يحتاج الشكل {index} إذا اشترك كل مربع جديد في ضلع؟",
            G5U6Diagonal=>$"نمط من مربعات متصلة مع قطر إضافي في كل مربع: الأشكال الأولى تستخدم ٤، ٨، ١٢ عودًا. كم عودًا للشكل {index} إذا استمر النمط؟",
            G5U6Triangles=>$"تُرتب مثلثات متجاورة وتتشارك في ضلع: ٣، ٥، ٧ أعواد للأشكال الأولى. كم عودًا للشكل {index}؟",
            _=>$"يبدأ النمطان أ وب بأربعة أعواد في الشكل الأول. يضيف أ ٣ أعواد في كل شكل، ويضيف ب ٦ أعواد. كم يزيد عدد أعواد ب عن أ في الشكل {index}؟"
        };
        return new Item(family,prompt,
            "أعدّ الأعواد التي أضيفت في كل خطوة وأحسب عدد الانتقالات من الشكل الأول إلى الشكل المطلوب، ثم أتحقق بجدول.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"index",index}});
    }
    private static bool ValidG5U6(string f,IReadOnlyDictionary<string,int> parameters,out int result)
    {
        result=0;
        if(!IsG5U6(f)||parameters.Count!=1||
            !parameters.TryGetValue("index",out int n)||n is <4 or >9)
            return false;
        result=f switch
        {
            G5U6Squares=>4+3*(n-1),
            G5U6Diagonal=>4*n,
            G5U6Triangles=>3+2*(n-1),
            _=>3*(n-1)
        };
        return true;
    }
    private static string SolveG5U6(string f,IReadOnlyDictionary<string,int> parameters)
    {
        if(!ValidG5U6(f,parameters,out int result))
            throw new InvalidOperationException("Invalid Grade 5 source toothpick-pattern parameters.");
        return result.ToString(CultureInfo.InvariantCulture);
    }
    private static bool VerifyG5U6(string f,IReadOnlyDictionary<string,int> parameters,string? answer)
    {
        if(!ValidG5U6(f,parameters,out int expected)||
            string.IsNullOrWhiteSpace(answer)||answer.Length>24)return false;
        string value=new(answer.Trim()
            .Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ',' and not '٬')
            .Select(ch=>ch switch
            {
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                _=>ch
            }).ToArray());
        return int.TryParse(value,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&
            actual==expected;
    }

    
    // Grade 5 Unit 6 Lesson 8: numeric subskills of additive/multiplicative
    // patterns and comparisons. Reuse G4 engine; do not alter G4 constraints.
    internal const string G5L08Add="uae.g5.u06.l08.next_additive_term";
    internal const string G5L08Multiply="uae.g5.u06.l08.next_multiplicative_term";
    internal const string G5L08Compare="uae.g5.u06.l08.compare_weekly_growth";
    private static bool IsG5L08(string? f)=>f is G5L08Add or G5L08Multiply or G5L08Compare;

    private static Item BuildG5L08(string f, Random random)
    {
        if(f==G5L08Compare)
        {
            int weeks=random.Next(4,9),a=3,b=6;
            return new Item(f,$"تسير هدى ٣ كيلومترات إضافية كل أسبوع، وتسير ريم ٦ كيلومترات إضافية كل أسبوع. بدأتا من المسافة نفسها. بعد {weeks} أسابيع، كم تزيد المسافة التراكمية لريم عن هدى؟",
                "قارن زيادتي النمطين عند عدد الأسابيع نفسه.",AssessmentItemType.Numeric,
                new Dictionary<string,int>{{"weeks",weeks},{"a",a},{"b",b}});
        }
        int start=random.Next(2,12),step=f==G5L08Add?random.Next(2,10):random.Next(2,4);
        int[] values=new int[4];values[0]=start;
        for(int i=1;i<values.Length;i++)values[i]=f==G5L08Add?values[i-1]+step:values[i-1]*step;
        return new Item(f,$"أكمل المتتالية {string.Join("، ",values)}، □.",
            f==G5L08Add?"أحسب الفرق الثابت بين كل حدين.":"أحدد عامل الضرب الثابت بين الحدود.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"start",start},{"step",step},{"terms",4}});
    }

    private static bool ValidG5L08(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!IsG5L08(f))return false;
        if(f==G5L08Compare)
        {
            if(p.Count!=3||!p.TryGetValue("weeks",out int n)||
               !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
               n is <4 or >8||a!=3||b!=6)return false;
            answer=(b-a)*n;return true;
        }
        if(p.Count!=3||!p.TryGetValue("start",out int s)||
           !p.TryGetValue("step",out int d)||!p.TryGetValue("terms",out int t)||
           s is <2 or >11||t!=4||
           (f==G5L08Add&&(d is <2 or >9))||
           (f==G5L08Multiply&&(d is <2 or >3)))return false;
        answer=s;
        for(int i=0;i<t;i++)answer=f==G5L08Add?answer+d:checked(answer*d);
        return true;
    }

    private static string SolveG5L08(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5L08(f,p,out int expected))
            throw new InvalidOperationException("Invalid Grade5 U06 L08 numeric-pattern parameters");
        return expected.ToString(CultureInfo.InvariantCulture);
    }
    private static bool VerifyG5L08(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5L08(f,p,out _)||string.IsNullOrWhiteSpace(answer)||answer.Length>24)return false;
        string normalized=new(answer.Trim().Where(ch=>ch is not ',' and not '٬' &&
            !char.IsWhiteSpace(ch)).Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            _=>ch
        }).ToArray());
        if(!int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual))return false;
        int expected;
        if(f==G5L08Compare)expected=(p["b"]-p["a"])*p["weeks"];
        else
        {
            expected=p["start"];
            for(int i=0;i<4;i++)
                expected=f==G5L08Add?expected+p["step"]:expected*p["step"];
        }
        return actual==expected;
    }

    private static decimal PowerTen(int exp)
    {
        decimal result=1m;for(int i=0;i<exp;i++)result*=10m;return result;
    }
    private static Item BuildG5(string family,Random random)
    {
        IReadOnlyDictionary<string,int> p;
        string prompt;
        if(family==G5Sequence)
        {
            int milli=random.Next(10,900001),dir=random.Next(2)==0?-1:1;
            decimal value=milli/1000m;
            string sequence=string.Join("، ",Enumerable.Range(0,4).Select(i=>
                (value*(dir==1?PowerTen(i):1m/PowerTen(i)))
                  .ToString("0.#######",CultureInfo.InvariantCulture)));
            p=new Dictionary<string,int>{{"seedMilli",milli},{"direction",dir},{"length",4}};
            prompt=$"اكتشف النمط وأكمل: {sequence}، □.";
        }
        else if(family==G5Arithmetic)
        {
            int start=random.Next(3,101),step=random.Next(2,11);
            p=new Dictionary<string,int>{{"start",start},{"step",step},{"length",3}};
            prompt=$"أكمل {start}، {start+step}، {start+2*step}، □.";
        }
        else if(family==G5Table)
        {
            int input=random.Next(5,51);
            p=new Dictionary<string,int>{{"input",input}};
            prompt=$"إذا كان ٥→٩ و١٠→١٩ و١٥→٢٩، فما المخرج للمدخل {input}؟";
        }
        else
        {
            int cents=random.Next(20,901)*10;
            p=new Dictionary<string,int>{{"priceCents",cents}};
            prompt=$"سعر سلعة {(cents/100m).ToString("0.00",CultureInfo.InvariantCulture)} درهمًا. ما سعرها بعد خصم ١٠٪؟";
        }
        return new Item(family,prompt,"أحدد القاعدة وأطبقها ثم أتحقق من الحدود السابقة.",
            AssessmentItemType.Numeric,p);
    }
    private static bool ValidG5(string f,IReadOnlyDictionary<string,int> p,out decimal result)
    {
        result=0m;if(!IsG5(f))return false;
        if(f==G5Sequence)
        {
            if(p.Count!=3||!p.TryGetValue("seedMilli",out int s)||
                !p.TryGetValue("direction",out int d)||!p.TryGetValue("length",out int len)||
                s is <10 or >900000||(d!=1&&d!=-1)||len!=4)return false;
            result=s/1000m*(d==1?PowerTen(len):1m/PowerTen(len));return true;
        }
        if(f==G5Arithmetic)
        {
            if(p.Count!=3||!p.TryGetValue("start",out int s)||!p.TryGetValue("step",out int d)||
                !p.TryGetValue("length",out int len)||s is <3 or >100||d is <2 or >10||len!=3)return false;
            result=s+d*len;return true;
        }
        if(f==G5Table)
        {
            if(p.Count!=1||!p.TryGetValue("input",out int x)||x is <5 or >50)return false;
            result=2*x-1;return true;
        }
        if(p.Count!=1||!p.TryGetValue("priceCents",out int cents)||
           cents is <200 or >9000||cents%10!=0)return false;
        result=cents*9/1000m;return true;
    }
    private static string SolveG5(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5(f,p,out decimal result))throw new InvalidOperationException("Invalid G5 pattern");
        return result.ToString("0.#######",CultureInfo.InvariantCulture);
    }
    private static bool VerifyG5(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5(f,p,out _)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Select(c=>c switch{
          >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
          >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
          '٫'=>'.',_=>c
        }).Where(c=>c is not ' ' and not '٬' and not '\u00a0').ToArray());
        if(s.Length>30||!s.All(c=>c is >= '0' and <= '9' or '.'))return false;
        if(!decimal.TryParse(s,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal provided))return false;
        decimal expected=f switch{
          G5Sequence=>p["seedMilli"]/1000m*(p["direction"]==1?PowerTen(4):1m/PowerTen(4)),
          G5Arithmetic=>p["start"]+3*p["step"],
          G5Table=>2*p["input"]-1,
          _=>p["priceCents"]*9/1000m
        };
        return provided==expected;
    }

    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("start",out int start)||
           !p.TryGetValue("step",out int step)||!p.TryGetValue("index",out int index)||
           start is <20 or >99||step is <2 or >9||index is <4 or >8)return false;
        result=start;
        for(int i=1;i<index;i++)result+=f==Constant?step:(i%2==1?-step:step+2);
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(IsG5L08(f))return SolveG5L08(f,p);
        if(IsG5U6(f))return SolveG5U6(f,p);
        if(IsG5(f))return SolveG5(f,p);
        if(!Valid(f,p,out int x))throw new InvalidOperationException("Invalid pattern");
        return x.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(IsG5L08(f))return VerifyG5L08(f,p,answer);
        if(IsG5U6(f))return VerifyG5U6(f,p,answer);
        if(IsG5(f))return VerifyG5(f,p,answer);
        if(!Valid(f,p,out var x)||string.IsNullOrWhiteSpace(answer))return false;
        var s=new string(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out var y)&&x==y;
    }
}
