using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>Bounded Grade6 Arabic positive/negative reference-position adapter.
/// The existing exact practice pipeline performs numeric grading. Freehand
/// number-line drawings and contextual explanations remain teacher-reviewed.</summary>
internal static class UaeG6U5L01SignedNumbersQuestions
{
    internal const string Position="uae.g6.u05.l01.signed_position";
    internal const string Opposite="uae.g6.u05.l01.opposite_integer";
    internal const string GraphSetExtremum="uae.g6.u05.l02.signed_plot_set_extremum";
    internal const string AbsoluteDistance="uae.g6.u05.l03.absolute_distance";
    internal const string AbsoluteExpression="uae.g6.u05.l04.absolute_expression";
    internal const string SignedCompare="uae.g6.u05.l05.compare_signed_integers";
    internal const string SignedOrderSet="uae.g6.u05.l05.order_integer_set";
    internal const string WorkBackwardTemperature="uae.g6.u05.l06.work_backward_temperature";
    internal const string WorkBackwardSchedule="uae.g6.u05.l06.work_backward_schedule";
    internal const string SignedRationalCompare="uae.g6.u05.l07.compare_signed_rationals";
    internal const string SignedRationalSide="uae.g6.u05.l07.rational_relative_zero";
    internal const string MixedRationalCompare="uae.g6.u05.l09.compare_mixed_signed_rationals";
    internal const string FourRationalOrder="uae.g6.u05.l09.order_four_signed_rationals";
    internal const string CoordinateRegion="uae.g6.u05.l10.coordinate_region";
    internal const string ReflectAcrossX="uae.g6.u05.l10.reflect_across_x";
    internal const string ReflectAcrossY="uae.g6.u05.l10.reflect_across_y";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Position or Opposite or GraphSetExtremum or AbsoluteDistance or AbsoluteExpression or SignedCompare or SignedOrderSet or WorkBackwardTemperature or WorkBackwardSchedule or SignedRationalCompare or SignedRationalSide or MixedRationalCompare or FourRationalOrder or CoordinateRegion or ReflectAcrossX or ReflectAcrossY;
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported signed-number family.");

        ArgumentNullException.ThrowIfNull(random);
        if(family is CoordinateRegion or ReflectAcrossX or ReflectAcrossY)
        {
            int x2=random.Next(-10,11),y2=random.Next(-10,11);
            var p=new Dictionary<string,int>{{"x2",x2},{"y2",y2}};
            string at=$"({FormatHalf(x2)},{FormatHalf(y2)})";
            string prompt=family switch
            {
                CoordinateRegion=>$"حدد موقع النقطة {at}. اكتب الأول أو الثاني أو الثالث أو الرابع أو المحور السيني أو المحور الصادي أو الأصل.",
                ReflectAcrossX=>$"اكتب صورة النقطة {at} عند الانعكاس حول المحور السيني x بالصيغة (س،ص).",
                _=>$"اكتب صورة النقطة {at} عند الانعكاس حول المحور الصادي y بالصيغة (س،ص)."
            };
            string hint=family switch
            {
                CoordinateRegion=>"إشارتا س وص تحددان الربع؛ المحاور والأصل خارج الأرباع.",
                ReflectAcrossX=>"احتفظ بس وغيّر إشارة ص فقط.",
                _=>"احتفظ بص وغيّر إشارة س فقط."
            };
            return new Item(family,prompt,hint,AssessmentItemType.ShortAnswer,p);
        }
        if(family is MixedRationalCompare or FourRationalOrder)
        {
            int[] denominators=[2,4,5,8,10,12,16,20,25,100];
            static string Format(int n,int d)=>d is 10 or 100?
                (n/(decimal)d).ToString("0.##",CultureInfo.InvariantCulture):$"{n}/{d}";
            if(family==MixedRationalCompare)
            {
                int d1=denominators[random.Next(denominators.Length)];
                int d2=denominators[random.Next(denominators.Length)];
                int n1=random.Next(-3*d1,3*d1+1),n2=random.Next(-3*d2,3*d2+1);
                var parameters=new Dictionary<string,int>{{"n1",n1},{"d1",d1},{"n2",n2},{"d2",d2}};
                return new Item(family,
                    $"قارن {Format(n1,d1)} و{Format(n2,d2)}، واكتب < أو > أو =.",
                    "يمكن توحيد المقامين والمقارنة بضرب تبادلي دقيق؛ لا تُقرب الأعداد الدورية.",
                    AssessmentItemType.ShortAnswer,parameters);
            }
            var entries=new List<(int N,int D)>();
            while(entries.Count<4)
            {
                int d=denominators[random.Next(denominators.Length)],n=random.Next(-3*d,3*d+1);
                if(entries.Any(x=>(long)x.N*d==(long)n*x.D))continue;
                entries.Add((n,d));
            }
            int mode=random.Next(2);
            var p=new Dictionary<string,int>{{"mode",mode}};
            for(int i=0;i<4;i++){p["n"+(i+1)]=entries[i].N;p["d"+(i+1)]=entries[i].D;}
            string labels=string.Join("؛ ",entries.Select((x,i)=>$"{i+1}) {Format(x.N,x.D)}"));
            return new Item(family,
                $"رتب الأعداد {labels} من {(mode==0?"الأصغر إلى الأكبر":"الأكبر إلى الأصغر")}. اكتب ترتيب أرقام العناصر ١ إلى ٤ مفصولة بفواصل.",
                "استخدم كسرًا مكافئًا أو قسمة دقيقة، ثم رتب القيم على خط الأعداد. الجواب تسلسل أرقام العناصر.",
                AssessmentItemType.ShortAnswer,p);
        }
        if(family==SignedRationalCompare)
        {
            int[] denominators=[2,4,5,8,10,12];
            int b=denominators[random.Next(denominators.Length)];
            int d=denominators[random.Next(denominators.Length)];
            int a=random.Next(-2*b,2*b+1),c=random.Next(-2*d,2*d+1);
            return new Item(family,
                $"قارن {a}/{b} و{c}/{d} على خط الأعداد. اكتب الرمز الصحيح (< أو > أو =).",
                "مثّل العددين بمقياس متساوٍ حول الصفر، أو قارن البسطين بعد توحيد المقامين؛ العدد الأيمن أكبر.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"d",d}});
        }
        if(family==SignedRationalSide)
        {
            int[] denominators=[2,4,5,8,10];
            int d=denominators[random.Next(denominators.Length)];
            int n=random.Next(-2*d,2*d+1);
            return new Item(family,
                $"على خط أعداد مرجعه صفر، هل {n}/{d} أكبر من الصفر أو أصغر منه أو يساويه؟ اكتب > أو < أو =.",
                "البسط السالب يعني نقطة يسار الصفر، والموجب يمينه؛ البسط صفر عند المرجع.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string,int>{{"n",n},{"d",d}});
        }
        if(family==WorkBackwardTemperature)
        {
            int final=random.Next(-15,16),change=random.Next(-25,26);
            if(change==0)change=5;
            return new Item(family,
                $"بعد تغير درجة الحرارة بمقدار {change} درجات أصبحت {final} درجة. كم كانت في البداية؟ أوجدها بالعمل بترتيب عكسي.",
                "للرجوع إلى البداية نطرح التغير من الدرجة النهائية ثم نتحقق بإضافته.",
                AssessmentItemType.ShortAnswer,new Dictionary<string,int>{{"final",final},{"change",change}});
        }
        if(family==WorkBackwardSchedule)
        {
            int end=random.Next(96,121)*5,travel=random.Next(10,26),
                food=random.Next(20,36),prepare=random.Next(5,21),message=random.Next(5,21);
            var p=new Dictionary<string,int>{{"endMinute",end},{"travel",travel},
                {"food",food},{"prepare",prepare},{"message",message}};
            return new Item(family,
                $"موعد النهاية {(end/60):D2}:{(end%60):D2} صباحًا. قبلها السفر {travel} دقيقة، والفطور {food} دقيقة، والاستعداد {prepare} دقيقة، والرسائل {message} دقيقة. متى تبدأ؟ اكتب الوقت بنظام 24 ساعة HH:MM.",
                "ابدأ من الوقت النهائي واطرح المدد بالعكس، ثم اجمعها للتحقق.",
                AssessmentItemType.ShortAnswer,p);
        }
        if(family==SignedCompare)
        {
            int a=random.Next(-40,41),b=random.Next(-40,41);
            return new Item(family,$"ضع الرمز الصحيح (< أو > أو =) بين {a} و{b}.",
                "على خط الأعداد العدد الواقع يمين العدد الآخر هو الأكبر، والأعداد الموجبة أكبر من الصفر والسالبة.",
                AssessmentItemType.ShortAnswer,new Dictionary<string,int>{{"a",a},{"b",b}});
        }
        if(family==SignedOrderSet)
        {
            var set=new HashSet<int>();
            while(set.Count<4)set.Add(random.Next(-40,41));
            var n=set.ToArray();int mode=random.Next(2);
            return new Item(family,
                $"رتب الأعداد {{{string.Join("، ",n)}}} من {(mode==0?"الأصغر إلى الأكبر":"الأكبر إلى الأصغر")}. اكتب القيم مفصولة بفواصل.",
                "مثّل الأعداد بمقياس واحد، ثم اقرأها من اليسار أو اليمين بحسب الترتيب المطلوب.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string,int>{{"a",n[0]},{"b",n[1]},{"c",n[2]},{"d",n[3]},{"mode",mode}});
        }
        if(family==AbsoluteExpression)
        {
            int a=random.Next(-25,26),b=random.Next(-25,26),operation=random.Next(2);
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"operation",operation}};
            string op=operation==0?"+":"−";
            return new Item(family,
                $"أوجد قيمة التعبير |{a}| {op} |{b}|. اكتب الناتج عددًا صحيحًا.",
                "أوجد المسافة من الصفر لكل عدد أولًا، ثم اجمع أو اطرح المقدارين حسب رمز التعبير.",
                AssessmentItemType.ShortAnswer,p);
        }
        if(family==AbsoluteDistance)
        {
            int n=random.Next(-40,41);
            return new Item(family,
                $"احسب المسافة بين العدد الصحيح {n} والصفر على خط الأعداد. اكتب المقدار فقط.",
                "القيمة المطلقة تقيس المسافة من الصفر وهي غير سالبة.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string,int>{{"n",n}});
        }
        if(family==GraphSetExtremum)
        {
            var numbers=new HashSet<int>();
            while(numbers.Count<4)numbers.Add(random.Next(-20,21));
            var entries=numbers.ToArray();
            int mode=random.Next(2);
            var pp=new Dictionary<string,int>{
                ["a"]=entries[0],["b"]=entries[1],["c"]=entries[2],
                ["d"]=entries[3],["mode"]=mode
            };
            return new Item(family,
                $"مثّل الأعداد {{{string.Join("، ",entries)}}} على خط الأعداد، وحدد العدد الموجود في أقصى {(mode==0?"اليسار":"اليمين")}. اكتب العدد فقط.",
                "ثبّت الصفر ووحدات متساوية، ووضّح موضع كل عدد. يفحص المعلم الرسم ويصحّح النظام إجابة الطرف.",
                AssessmentItemType.ShortAnswer,pp);
        }
        int magnitude=random.Next(family==Opposite?0:1,501);
        int sign=random.Next(2)==0?-1:1;
        var p=new Dictionary<string,int>{{"magnitude",magnitude},{"sign",sign}};
        if(family==Position)
        {
            string place=sign==1?"فوق مستوى سطح البحر":"تحت مستوى سطح البحر";
            return new Item(family,$"يقع موضع على بعد {magnitude} قدم {place}. اكتب العدد الصحيح بإشارته بالنسبة إلى المستوى المرجعي صفر.",
                "الموضع الأعلى من المرجع موجب، والأدنى منه سالب. الصفر يمثّل مستوى المرجع.",
                AssessmentItemType.ShortAnswer,p);
        }
        int original=sign*magnitude;
        return new Item(family,$"اكتب العدد الصحيح المقابل للعدد {original}. ما موقعه من الصفر؟",
            "العددان المتقابلان على بعد متساوٍ من الصفر وفي اتجاهين متعاكسين؛ مقابل الصفر هو الصفر.",
            AssessmentItemType.ShortAnswer,p);
    }




    // Signed Cartesian pairs use integer half-units for exact grading.
    private static string FormatHalf(int halves) =>
        (halves/2m).ToString("0.#",CultureInfo.InvariantCulture);
    private static bool TryCoordinateAnswer(string family,IReadOnlyDictionary<string,int>? p,out string result)
    {
        result="";
        if(family is not (CoordinateRegion or ReflectAcrossX or ReflectAcrossY)||
           p is null||p.Count!=2||!p.TryGetValue("x2",out int x2)||
           !p.TryGetValue("y2",out int y2)||
           x2 is <-10 or >10||y2 is <-10 or >10)return false;
        if(family==CoordinateRegion)
        {
            result=x2==0&&y2==0?"الأصل":y2==0?"المحور السيني":
                x2==0?"المحور الصادي":x2>0&&y2>0?"الأول":
                x2<0&&y2>0?"الثاني":x2<0&&y2<0?"الثالث":"الرابع";
            return true;
        }
        result=$"({FormatHalf(family==ReflectAcrossY?-x2:x2)},{FormatHalf(family==ReflectAcrossX?-y2:y2)})";
        return true;
    }

    // U05L09 scoped exact comparison of positive and negative fractions, decimals, mixed values.
    // Denominators remain positive and all comparison uses integer cross-products.
    private static bool TryMixedRationalCompare(IReadOnlyDictionary<string,int>? p,out string result)
    {
        result="";
        if(p is null||p.Count!=4||!p.TryGetValue("n1",out int n1)||
           !p.TryGetValue("d1",out int d1)||!p.TryGetValue("n2",out int n2)||
           !p.TryGetValue("d2",out int d2)||d1 is <2 or >100||d2 is <2 or >100||
           n1 < -3*d1||n1>3*d1||n2 < -3*d2||n2>3*d2)return false;
        long a=(long)n1*d2,b=(long)n2*d1;
        result=a<b?"<":a>b?">":"=";
        return true;
    }
    private static bool TryFourRationalOrder(IReadOnlyDictionary<string,int>? p,out string result)
    {
        result="";
        if(p is null||p.Count!=9||!p.TryGetValue("mode",out int mode)||
           mode is not (0 or 1))return false;
        var fractions=new (int N,int D,int Position)[4];
        for(int i=0;i<4;i++)
        {
            if(!p.TryGetValue("n"+(i+1),out int n)||
               !p.TryGetValue("d"+(i+1),out int d)||d is <2 or >100||
               n < -3*d||n>3*d)return false;
            fractions[i]=(n,d,i+1);
        }
        for(int i=0;i<4;i++)
            for(int j=i+1;j<4;j++)
                if((long)fractions[i].N*fractions[j].D==
                   (long)fractions[j].N*fractions[i].D)return false;
        Array.Sort(fractions,(a,b)=>((long)a.N*b.D).CompareTo((long)b.N*a.D));
        if(mode==1)Array.Reverse(fractions);
        result=string.Join(",",fractions.Select(x=>x.Position.ToString(CultureInfo.InvariantCulture)));
        return true;
    }

    // Reuse the G6 signed-number engine for bounded rational positions and comparisons.
    // Only exact symbols are auto-graded. Student-drawn scales and points remain teacher-reviewed.
    private static bool TrySignedRationalCompare(IReadOnlyDictionary<string,int>? p,out string symbol)
    {
        symbol="";
        if(p is null||p.Count!=4||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||!p.TryGetValue("c",out int c)||
           !p.TryGetValue("d",out int d)||b is <2 or >12||d is <2 or >12||
           a < -2*b||a>2*b||c < -2*d||c>2*d)return false;
        long left=(long)a*d,right=(long)c*b;
        symbol=left<right?"<":left>right?">":"=";
        return true;
    }
    private static bool TrySignedRationalSide(IReadOnlyDictionary<string,int>? p,out string symbol)
    {
        symbol="";
        if(p is null||p.Count!=2||!p.TryGetValue("n",out int n)||
           !p.TryGetValue("d",out int d)||d is <2 or >10||
           n < -2*d||n>2*d)return false;
        symbol=n<0?"<":n>0?">":"=";
        return true;
    }

    // U05L06 teacher-led reverse-step inquiry; only temperature and schedule numeric components are auto-scored.
    private static bool TryBackTemperature(IReadOnlyDictionary<string,int>? p,out int start)
    {
        start=0;
        if(p is null||p.Count!=2||!p.TryGetValue("final",out int end)||
           !p.TryGetValue("change",out int change)||
           end is <-15 or >15||change is <-25 or >25||change==0)return false;
        start=end-change;
        return true;
    }
    private static bool TryBackSchedule(IReadOnlyDictionary<string,int>? p,out string start)
    {
        start="";
        if(p is null||p.Count!=5||!p.TryGetValue("endMinute",out int end)||
           !p.TryGetValue("travel",out int travel)||!p.TryGetValue("food",out int food)||
           !p.TryGetValue("prepare",out int prep)||!p.TryGetValue("message",out int message)||
           end is <480 or >600||end%5!=0||travel is <10 or >25||
           food is <20 or >35||prep is <5 or >20||message is <5 or >20)return false;
        int minute=end-travel-food-prep-message;
        if(minute<0)return false;
        start=$"{minute/60:D2}:{minute%60:D2}";
        return true;
    }

    // Scoped G6 U05L05 exact comparison and 4-number ordering; reuse existing signed-number adapter.
    private static bool TryCompare(IReadOnlyDictionary<string,int>? p,out string symbol)
    {
        symbol="";
        if(p is null||p.Count!=2||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||a is <-40 or >40||b is <-40 or >40)return false;
        symbol=a<b?"<":a>b?">":"=";
        return true;
    }
    private static bool TryOrder(IReadOnlyDictionary<string,int>? p,out string values)
    {
        values="";
        if(p is null||p.Count!=5||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||!p.TryGetValue("c",out int c)||
           !p.TryGetValue("d",out int d)||!p.TryGetValue("mode",out int mode)||
           mode is not (0 or 1)||new[]{a,b,c,d}.Any(n=>n is <-40 or >40))return false;
        var ordered=mode==0?new[]{a,b,c,d}.OrderBy(n=>n):new[]{a,b,c,d}.OrderByDescending(n=>n);
        values=string.Join(",",ordered.Select(n=>n.ToString(CultureInfo.InvariantCulture)));
        return true;
    }
    private static string NormalizeSignedAnswer(string answer)
    {
        return new string(answer.Where(ch=>!char.IsWhiteSpace(ch))
            .Select(ch=>ch switch{
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                '−' or '–'=>'-','،'=>',','٫'=>'.',_=>ch}).ToArray());
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int>? p,out int answer)
    {

        answer=0;
        if(family==AbsoluteExpression)
        {
            if(p is null||p.Count!=3||!p.TryGetValue("a",out int a)||
               !p.TryGetValue("b",out int b)||!p.TryGetValue("operation",out int op)||
               op is not (0 or 1)||a is <-25 or >25||b is <-25 or >25)
                return false;
            answer=op==0?Math.Abs(a)+Math.Abs(b):Math.Abs(a)-Math.Abs(b);
            return true;
        }
        if(family==AbsoluteDistance)
        {
            if(p is null||p.Count!=1||!p.TryGetValue("n",out int n)||
                n is <-40 or >40)return false;
            answer=Math.Abs(n);
            return true;
        }
        if(family==GraphSetExtremum)
        {
            if(p is null||p.Count!=5||!p.TryGetValue("a",out var a)||
               !p.TryGetValue("b",out var b)||!p.TryGetValue("c",out var c)||
               !p.TryGetValue("d",out var d)||!p.TryGetValue("mode",out var mode)||
               mode is not (0 or 1)||new[]{a,b,c,d}.Any(n=>n is <-20 or >20))
                return false;
            answer=mode==0?Math.Min(Math.Min(a,b),Math.Min(c,d)):
                Math.Max(Math.Max(a,b),Math.Max(c,d));
            return true;
        }
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("magnitude",out int magnitude)||
           !p.TryGetValue("sign",out int sign)||sign is not (-1 or 1)||
           magnitude<0||magnitude>500||
           (family==Position&&magnitude==0))return false;
        int signed=checked(sign*magnitude);
        answer=family==Position?signed:-signed;
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(family is CoordinateRegion or ReflectAcrossX or ReflectAcrossY)
            return TryCoordinateAnswer(family,p,out string result)?result:
                throw new InvalidOperationException("Invalid coordinate parameters.");
        if(family==MixedRationalCompare)
            return TryMixedRationalCompare(p,out string symbol)?symbol:
                throw new InvalidOperationException("Invalid mixed-rational comparison.");
        if(family==FourRationalOrder)
            return TryFourRationalOrder(p,out string sequence)?sequence:
                throw new InvalidOperationException("Invalid rational ordering.");
        if(family==SignedRationalCompare)
            return TrySignedRationalCompare(p,out string rationalSymbol)?rationalSymbol:
                throw new InvalidOperationException("Invalid signed rational comparison.");
        if(family==SignedRationalSide)
            return TrySignedRationalSide(p,out string zeroSymbol)?zeroSymbol:
                throw new InvalidOperationException("Invalid rational zero comparison.");
        if(family==WorkBackwardTemperature)
            return TryBackTemperature(p,out int n)?n.ToString(CultureInfo.InvariantCulture):
                throw new InvalidOperationException("Invalid reverse-temperature parameters.");
        if(family==WorkBackwardSchedule)
            return TryBackSchedule(p,out string clock)?clock:
                throw new InvalidOperationException("Invalid reverse-schedule parameters.");
        if(family==SignedCompare)
            return TryCompare(p,out string symbol)?symbol:throw new InvalidOperationException("Invalid signed comparison.");
        if(family==SignedOrderSet)
            return TryOrder(p,out string ordered)?ordered:throw new InvalidOperationException("Invalid signed ordering.");
        if(!Valid(family,p,out int signed))
            throw new InvalidOperationException("Invalid signed-number parameters.");
        return signed.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(family is CoordinateRegion or ReflectAcrossX or ReflectAcrossY)
        {
            if(string.IsNullOrWhiteSpace(answer)||answer.Length>80||
                !TryCoordinateAnswer(family,p,out string result))return false;
            return string.Equals(NormalizeSignedAnswer(answer.Trim()),result,StringComparison.Ordinal);
        }
        if(family is MixedRationalCompare or FourRationalOrder)
        {
            if(string.IsNullOrWhiteSpace(answer)||answer.Length>80)return false;
            string expected;
            if(family==MixedRationalCompare){
                if(!TryMixedRationalCompare(p,out expected))return false;
            }else if(!TryFourRationalOrder(p,out expected))return false;
            return string.Equals(NormalizeSignedAnswer(answer.Trim()),expected,StringComparison.Ordinal);
        }
        if(family is SignedRationalCompare or SignedRationalSide)
        {
            if(string.IsNullOrWhiteSpace(answer)||answer.Length>24)return false;
            string symbol;
            if(family==SignedRationalCompare){
                if(!TrySignedRationalCompare(p,out symbol))return false;
            }else if(!TrySignedRationalSide(p,out symbol))return false;
            return string.Equals(answer.Trim(),symbol,StringComparison.Ordinal);
        }
        if(family==WorkBackwardTemperature)
        {
            if(!TryBackTemperature(p,out int n)||string.IsNullOrWhiteSpace(answer)||
               answer.Length>48)return false;
            return int.TryParse(NormalizeSignedAnswer(answer.Trim()),
                NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int actual)&&actual==n;
        }
        if(family==WorkBackwardSchedule)
        {
            if(!TryBackSchedule(p,out string clock)||string.IsNullOrWhiteSpace(answer)||
               answer.Length>40)return false;
            return string.Equals(NormalizeSignedAnswer(answer.Trim()),clock,StringComparison.Ordinal);
        }
        if(family is SignedCompare or SignedOrderSet)
        {
            if(string.IsNullOrWhiteSpace(answer)||answer.Length>100)return false;
            string expected;
            if(family==SignedCompare){
                if(!TryCompare(p,out expected))return false;
            }else if(!TryOrder(p,out expected))return false;
            return string.Equals(NormalizeSignedAnswer(answer.Trim()),expected,StringComparison.Ordinal);
        }
        if(!Valid(family,p,out int expected)||
           string.IsNullOrWhiteSpace(answer)||answer.Length>36)return false;
        string normalized=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch))
            .Select(ch=>ch switch{
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                '−' or '–'=>'-',_=>ch
            }).ToArray());
        return int.TryParse(normalized,NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}
