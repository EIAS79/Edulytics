using System.Globalization;
using System.Numerics;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>Source-shaped bounded variants for G11-001. Reuses the existing
/// exact engine, answer normalization and exposure mechanism. Formula
/// parameters are never an answer. Expressions use signed tenths; fractions
/// are returned exactly. Open modeling/drawing is not auto-graded.</summary>
internal static partial class UaeG11U01L01ExpressionQuestions
{
    internal const string SourceExpression = "uae.g11.unified.u01.l01.source_expression";
    internal const string SourceFormula = "uae.g11.unified.u01.l01.source_formula";
    internal const string RoundedFormula = "uae.g11.unified.u01.l01.rounded_formula";

    private const int ExpressionModes = 29;
    private const int FormulaModes = 23;
    private const int RoundedModes = 4;

    private static bool IsSourceVariant(string? family) =>
        family is SourceExpression or SourceFormula or RoundedFormula;

    private static bool ReadSourceParameters(string family, IReadOnlyDictionary<string,int>? p,
        out int mode, out long a, out long b, out long c, out long d)
    {
        mode = 0; a = b = c = d = 0;
        if (p is null || p.Count != 5 || !p.TryGetValue("mode",out mode) ||
            !p.TryGetValue("a",out int ai) || !p.TryGetValue("b",out int bi) ||
            !p.TryGetValue("c",out int ci) || !p.TryGetValue("d",out int di)) return false;
        a=ai; b=bi; c=ci; d=di;
        if (family == SourceExpression)
            return mode >= 0 && mode < ExpressionModes &&
                a is >= -40 and <= 40 && b is >= -40 and <= 40 &&
                c is >= -40 and <= 40 && d is >= -40 and <= 40;
        if (family != SourceFormula && family != RoundedFormula) return false;
        return mode >= 0 && mode < (family == SourceFormula ? FormulaModes : RoundedModes) &&
            a is >= 1 and <= 12 && b is >= 1 and <= 12 &&
            c is >= 1 and <= 12 && d is >= 1 and <= 12;
    }

    // Solver: integer numerator and denominator derived for each of the
    // displayed mathematical expressions. a,b,c,d are tenths.
    private static (long N,long D) ExpressionRatio(int m,long a,long b,long c,long d) => m switch
    {
        0 => (a-3*b+2*c,10),
        1 => (30*a+(b+20)*(b+20),100),
        2 => (10*a+2*(b*b-10*(a+c)),100),
        3 => (4*c-3*(b-a+c),10),
        4 => (3*(2*a+4*b)-c,10),
        5 => (a*a+30*c,10*(2*b+3*a)),
        6 => (b*b*b+10*a*c,10*(a*b+3*b*c)),
        7 => (2*b+3*a,60-c),
        8 => (10*(2*a-3*c),5*a*b),
        9 => (a+b-c,10),
        10 => (2*d-6*a+b,20),
        11 => (5*(a-d),10),
        12 => (4*(b+a),10),
        13 => (7*c-3*b+2*d,10),
        14 => (2*b-5*c+a,10),
        15 => (a+b,c-d),
        16 => (10*(a-b),b*d),
        17 => (a*c,10*(d+b)),
        18 => (b*b*c*c,100*a*d),
        19 => (b+40,3*(d+c)),
        20 => (400*(d+a),3*a*b*b),
        21 => (10*a*a*a+3*b*b*b*b,10000),
        22 => ((a-4*c)*(a-4*c),100),
        23 => (3*(5*d-2*b)-7*c,10),
        24 => ((b+c)*(b+c),a*d),
        25 => (10*(10*d-3*b),c*c),
        26 => (d*a+b*c,d*a-b*c),
        27 => (3*a-b-2*c,c-b-a),
        28 => (a*a*a*a-200*b*c,10*c*c*c+2000*b),
        _ => (0,0)
    };

    // Formula parameters are positive integers. Prompts expose the formula
    // and ask for one numerical target, using coefficients of pi where noted.
    private static (long N,long D) FormulaRatio(int m,long a,long b,long c,long d) => m switch
    {
        0 => (100*(a-b), a+b+c),                     // net percentage
        1 => (a*(10*b+c),10),                        // fuel distance
        2 => (5*(40+5*a-32),9),                      // F to C
        3 => (a*(a+b),1),                            // triangle
        4 => (a*1000,b*100),                         // earnings per share
        5 => (2*a,1),                                // circumference / pi
        6 => (2*a,b),                                // period / pi
        7 => (a*b,1),                                // base area
        8 => (a*b*c,3),                              // pyramid
        9 => (a*a*c,3),                              // cone from diameter / pi
        10 => (20+8*a,100),                          // weighted score
        11 => (60+a,5),                             // rate model
        12 => (100*(5*(4*a+2*c+10*d-20*b)+3*(a+b+c+d)),30*(a+b+c+d)), // four scaled rate terms
        13 => (a*a+12*a+500,200),                    // quadratic model
        14 => (a*a*a-20*a*a+300*a+3000,1000),        // cubic model
        15 => ((long)Math.Sqrt(6*a*3*a*2*a*a),1),      // Heron s=6a, sides 3a,4a,5a
        16 => (3*a,1),                               // radical with exact right-triangle ratio
        17 => (a*a*b,1),                             // cylinder / pi
        18 => (a*d+c*b,2*b*d),                       // midpoint
        19 => (a*a*b*b,1),                           // square scaling
        20 => (c+1,1),                               // first table month
        21 => (90+a,1),                              // angle bisectors
        22 => (100*a+100*b*c-10*b*c,1),               // linear height at t=c
        _ => (0,0)
    };

    private static string ExpressionText(int m) => m switch
    {
        0=>"أ−3ب+2ج", 1=>"3أ+(ب+2)²", 2=>"أ+2[ب²−(أ+ج)]",
        3=>"4ج−3[(ب−أ)+ج]", 4=>"3(2أ+4ب)−ج",
        5=>"(أ²+3ج)/(2ب+3أ)", 6=>"(ب³+أج)/(أب+3بج)",
        7=>"(2ب+3أ)/(6−ج)", 8=>"(2أ−3ج)/(5أب)",
        9=>"أ+ب−ج", 10=>"د−3أ+ب/2", 11=>"5(أ−د)", 12=>"4(ب+أ)",
        13=>"7ج−3ب+2د", 14=>"2ب−5ج+أ", 15=>"(أ+ب)/(ج−د)",
        16=>"(أ−ب)/(بد)", 17=>"أج/(د+ب)", 18=>"ب²ج²/(أد)",
        19=>"(ب+4)/(3(د+ج))", 20=>"4(د+أ)/(3أب²)",
        21=>"أ³+3ب⁴", 22=>"(أ−4ج)²", 23=>"3(5د−2ب)−7ج",
        24=>"(ب+ج)²/(أد)", 25=>"(10د−3ب)/ج²", 26=>"(دأ+بج)/(دأ−بج)",
        27=>"(3أ−ب−2ج)/(ج−ب−أ)", 28=>"(أ⁴−2بج)/(ج³+2ب)",
        _=>throw new InvalidOperationException("Unknown source expression variant.")
    };

    private static string FormulaText(int m,long a,long b,long c,long d) => m switch
    {
        0=>$"نجح لاعب في {a} محاولة وارتكب {b} أخطاء من {a+b+c} محاولة. احسب النسبة الصافية P=100(s−e)/n.",
        1=>$"كفاءة المركبة {a} كم/لتر، والوقود {Number(b+c/10m)} لتر. احسب المسافة D=FL.",
        2=>$"حوّل {40+5*a}°F إلى مئوية باستخدام C=5(F−32)/9.",
        3=>$"مثلث ارتفاعه {2*a} وقاعدته {a}+{b}. احسب A=bh/2.",
        4=>$"ربح {a*1000} درهم موزع بالتساوي على {b*100} سهم. ما ربح السهم؟",
        5=>$"مدار دائري نصف قطره {a} كم. أوجد معامل π في C=2πr.",
        6=>$"مدار نصف قطره {a} كم وسرعته {b} كم/ساعة. أوجد معامل π في T=2πr/v.",
        7=>$"قاعدة هرم مستطيلة {a} م × {b} م. احسب مساحة القاعدة.",
        8=>$"هرم قاعدته {a}×{b} وارتفاعه {c}. احسب V=Bh/3.",
        9=>$"مخروط قطره {2*a} وارتفاعه {c}. احسب معامل π في V=πr²h/3.",
        10=>$"احسب المؤشر I=0.2+0.8L عندما L={Number(a/10m)}.",
        11=>$"نموذج تجريبي للحرارة T=12+n/5. احسب T عند n={a}.",
        12=>$"مؤشر أداء افتراضي: I=( (C/A−0.25)/0.25 + (Y/A−2)/5 + (T/A)/0.1 + (0.1−E/A)/0.05 )×100/6. احسبه عند A={a+b+c+d}، C={a}، Y={10*c}، T={d}، E={b}.",
        13=>$"احسب P(t)=t²/200+3t/50+2.5 عند t={a}.",
        14=>$"احسب Q(t)=t³/1000−t²/50+3t/10+3 عند t={a}.",
        15=>$"مثلث أضلاعه {3*a}، {4*a}، {5*a}. احسب مساحته بصيغة هيرون A=√[s(s−x)(s−y)(s−z)].",
        16=>$"احسب √[r²(1−x²/h²)] عند r={5*a}، x=4، h=5.",
        17=>$"أسطوانة نصف قطرها {a} وارتفاعها {b}. احسب معامل π في V=πr²h.",
        18=>$"أوجد منتصف المسافة بين {a}/{b} و{c}/{d}.",
        19=>$"مساحة مربع {a*a}، وطول ضلع مربع آخر يساوي {b} أمثال ضلع الأول. احسب المساحة الجديدة.",
        20=>$"عدد العملاء في الشهر 1 هو {a*10}، والزيادة الشهرية ثابتة {b*10}. في أي شهر يصل العدد أول مرة إلى {a*10+b*10*c-((d-1)%(b*10))} على الأقل؟",
        21=>$"زاوية رأس مثلث {2*a}°. أوجد الزاوية بين منصفَي زاويتي القاعدة.",
        22=>$"ارتفاع البداية {100*a+100*b*c} م، والهبوط {10*b} م/ث. احسب h(t)=h₀−vt بعد {c} ثوان.",
        _=>throw new InvalidOperationException("Unknown source formula variant.")
    };

    private static string RoundedText(int m,long a,long b,long c,long d) => m switch
    {
        0=>$"احسب √[r²(1−x²/h²)] عند r={a+4}، x={b}، h={b+c}، وقرّب لأقرب عُشر.",
        1=>FormulaText(12,a,b,c,d)+" قرّب لأقرب عُشر.",
        2=>$"مدار نصف قطره {a} مليون كم وسرعته {b*1000} كم/ساعة. احسب زمن الدورة بالأيام T=2πr/(24v) وقرّب لأقرب عُشر.",
        3=>$"مثلث أضلاعه {a+b} و{b+c} و{c+a}. احسب مساحته بصيغة هيرون وقرّب لأقرب عُشر.",
        _=>throw new InvalidOperationException("Unknown rounded source formula.")
    };

    private static Item BuildSourceVariant(string family,Random random)
    {
        int count=family==SourceExpression?ExpressionModes:family==SourceFormula?FormulaModes:RoundedModes;
        int m=random.Next(count),a,b,c,d;
        for(int i=0;;i++)
        {
            if(i>=100)throw new InvalidOperationException("No valid bounded parameters.");
            int lo=family==SourceExpression?-40:1,hi=family==SourceExpression?41:13;
            a=random.Next(lo,hi); b=random.Next(lo,hi); c=random.Next(lo,hi); d=random.Next(lo,hi);
            if(family!=SourceExpression || ExpressionRatio(m,a,b,c,d).D!=0)break;
        }
        var p=new Dictionary<string,int>{{"mode",m},{"a",a},{"b",b},{"c",c},{"d",d}};
        string prompt=family==SourceExpression
            ? $"إذا كان أ={Number(a/10m)}، ب={Number(b/10m)}، ج={Number(c/10m)}، د={Number(d/10m)}، أوجد قيمة {ExpressionText(m)}."
            : family==SourceFormula?FormulaText(m,a,b,c,d):RoundedText(m,a,b,c,d);
        return new Item(family,prompt+(family==RoundedFormula?" اكتب الناتج العشري.":" اكتب عددًا صحيحًا أو كسرًا دقيقًا."),
            "حدّد الصيغة، عوّض بالقيم، طبّق ترتيب العمليات، وافحص المقام والوحدات. لا تقرّب إلا عند الطلب.",
            family==RoundedFormula?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,p);
    }

    private static string ReduceRatio(long n,long d)
    {
        if(d==0)throw new InvalidOperationException("Zero denominator.");
        if(d<0){n=-n;d=-d;}
        long g=Gcd(n,d);n/=g;d/=g;
        return d==1?Number(n):$"{n.ToString(CultureInfo.InvariantCulture)}/{d.ToString(CultureInfo.InvariantCulture)}";
    }

    private static decimal RoundTenth(decimal x) => decimal.Round(x,1,MidpointRounding.AwayFromZero);

    private static decimal RoundedValue(int m,long a,long b,long c,long d) => m switch
    {
        0=>RoundTenth((decimal)Math.Sqrt((double)((a+4)*(a+4)*(1m-(decimal)(b*b)/((b+c)*(b+c)))))),
        1=>RoundTenth(100m*(5*(4*a+2*c+10*d-20*b)+3*(a+b+c+d))/(30*(a+b+c+d))),
        2=>RoundTenth((decimal)(2*Math.PI)*a*1000/(24*b)),
        3=>RoundTenth((decimal)Math.Sqrt((a+b+c)*a*b*c)),
        _=>throw new InvalidOperationException("Invalid rounded mode.")
    };

    private static string SolveSourceVariant(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!ReadSourceParameters(family,p,out int m,out long a,out long b,out long c,out long d))
            throw new InvalidOperationException("Invalid source variant.");
        if(family==RoundedFormula)return Number(RoundedValue(m,a,b,c,d));
        var ratio=family==SourceExpression?ExpressionRatio(m,a,b,c,d):FormulaRatio(m,a,b,c,d);
        return ReduceRatio(ratio.N,ratio.D);
    }

    // Independent verifier: evaluates the mathematical numerator and
    // denominator in decimal variables (exact finite decimals), rather
    // than reading or comparing the solver's output or integer formulas.
    private static (decimal N,decimal D) VerifyExpression(int m,decimal a,decimal b,decimal c,decimal d) => m switch
    {
        0=>(a-3*b+2*c,1), 1=>(3*a+(b+2)*(b+2),1),
        2=>(a+2*(b*b-a-c),1), 3=>(4*c-3*(b-a+c),1),
        4=>(6*a+12*b-c,1), 5=>(a*a+3*c,2*b+3*a),
        6=>(b*b*b+a*c,a*b+3*b*c), 7=>(2*b+3*a,6-c),
        8=>(2*a-3*c,5*a*b), 9=>(a+b-c,1),
        10=>(d-3*a+b/2,1), 11=>(5*a-5*d,1), 12=>(4*b+4*a,1),
        13=>(7*c-3*b+2*d,1), 14=>(2*b-5*c+a,1),
        15=>(a+b,c-d), 16=>(a-b,b*d),17=>(a*c,d+b),
        18=>((b*c)*(b*c),a*d),19=>(b+4,3*(d+c)),
        20=>(4*(d+a),3*a*b*b),21=>(a*a*a+3*b*b*b*b,1),
        22=>(a*a-8*a*c+16*c*c,1),23=>(15*d-6*b-7*c,1),
        24=>(b*b+2*b*c+c*c,a*d),25=>(10*d-3*b,c*c),
        26=>(d*a+b*c,d*a-b*c),27=>(3*a-b-2*c,c-b-a),
        28=>(a*a*a*a-2*b*c,c*c*c+2*b),_ => (0,0)
    };

    private static (decimal N,decimal D) VerifyFormula(int m,decimal a,decimal b,decimal c,decimal d) => m switch
    {
        0=>((a-b)*100,a+b+c),1=>(a*(b+c/10),1),
        2=>(5*(8+5*a),9),3=>((2*a)*(a+b),2),4=>(10*a,b),
        5=>(a+a,1),6=>(2*a,b),7=>(a*b,1),8=>(a*b*c,3),
        9=>((2*a/2)*(2*a/2)*c,3),10=>(0.2m+0.8m*a/10,1),
        11=>(12+a/5,1),12=>FourRateRatio(a,b,c,d),
        13=>(a*a/200+3*a/50+2.5m,1),14=>(a*a*a/1000-a*a/50+3*a/10+3,1),
        // independent geometry: 3a,4a,5a is a right triangle.
        15=>(3*a*4*a,2),16=>(3*a,1),17=>(a*a*b,1),
        18=>(a*d+c*b,2*b*d),19=>((a*b)*(a*b),1),
        20=>(1+decimal.Ceiling((10*b*c-((d-1)%(10*b)))/(10*b)),1),21=>(180-(180-2*a)/2,1),
        22=>(100*a+90*b*c,1),_ => (0,0)
    };

    // Independent expansion of each of the four normalized ratios;
    // all quantities are exact before the final division by the total.
    private static (decimal N,decimal D) FourRateRatio(decimal a,decimal b,decimal c,decimal d)
    {
        decimal s=a+b+c+d;
        decimal n=(a-0.25m*s)/0.25m+(10*c-2*s)/5+
            d/0.1m+(0.1m*s-b)/0.05m;
        return (100*n,6*s);
    }

    private static decimal IndependentHeronSquared(decimal a,decimal b,decimal c)
    {
        decimal x=a+b,y=b+c,z=c+a;
        decimal xx=x*x,yy=y*y,zz=z*z;
        return (2*(xx*yy+yy*zz+zz*xx)-(xx*xx+yy*yy+zz*zz))/16;
    }

    private static bool VerifySourceVariant(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!ReadSourceParameters(family,p,out int m,out long a,out long b,out long c,out long d)
            || string.IsNullOrWhiteSpace(response)||response.Length>40||
            !TryResponse(response,family!=RoundedFormula,out long n,out long den))return false;
        if(family==RoundedFormula)
        {
            decimal answer=(decimal)n/den;
            if(answer*10!=decimal.Truncate(answer*10))return false;
            if(m==3)
            {
                if(answer<0)return false;
                decimal square=IndependentHeronSquared(a,b,c);
                decimal low=Math.Max(0m,answer-0.05m), high=answer+0.05m;
                return square>=low*low && square<high*high;
            }
            var rate=FourRateRatio(a,b,c,d);
            decimal actual=m switch
            {
                0=>(a+4)*(decimal)Math.Sqrt((double)((2*b*c+c*c)/(decimal)((b+c)*(b+c)))),
                1=>rate.N/rate.D,
                2=>(decimal)Math.PI*a*1000/(12*b),
                _=>throw new InvalidOperationException("Invalid rounded mode.")
            };
            return answer==decimal.Round(actual,1,MidpointRounding.AwayFromZero);
        }
        var pair=family==SourceExpression?VerifyExpression(m,a/10m,b/10m,c/10m,d/10m):VerifyFormula(m,a,b,c,d);
        if(pair.D==0)return false;
        // Exact scale includes every finite decimal in these bounded formulas.
        const decimal scale=100000m;
        if(pair.N*scale!=decimal.Truncate(pair.N*scale)||pair.D*scale!=decimal.Truncate(pair.D*scale))return false;
        return (BigInteger)n*(BigInteger)(pair.D*scale)==(BigInteger)den*(BigInteger)(pair.N*scale);
    }
}
