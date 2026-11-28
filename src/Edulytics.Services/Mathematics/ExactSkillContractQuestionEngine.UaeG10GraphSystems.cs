using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Owner source Grade10 General U03L01: graph two straight lines, recover
/// their exact intersection, and recognize unique/parallel/coincident systems.
/// Stage C authored, not executed until Stage D.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10GraphSystems(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int limit=difficulty==ExactSkillQuestionDifficulty.Challenge?4:3;
        int m1=random.Next(-limit,limit+1),m2,b1,b2;
        bool classification=family==UaeG10GraphSystemsResponse.CountFamily;
        int mode=classification?random.Next(0,3):1; // 0 none, 1 unique, 2 infinite
        if(mode==1)
        {
            do m2=random.Next(-limit,limit+1);while(m2==m1);
            int x=random.Next(-5,6),y=random.Next(-5,6);
            b1=y-m1*x;
            b2=y-m2*x;
        }
        else
        {
            m2=m1;
            b1=random.Next(-10,11);
            int gap;
            do gap=random.Next(-3,4);while(gap==0);
            b2=mode==2?b1:b1+gap;
        }
        string system=$"ص=({m1})س+({b1})، وص=({m2})س+({b2})";
        string prompt=family switch
        {
            UaeG10GraphSystemsResponse.XFamily =>
                $"ارسم خطي النظام {system}. ما إحداثي س لنقطة التقاطع؟ أجب بعدد صحيح فقط.",
            UaeG10GraphSystemsResponse.YFamily =>
                $"ارسم خطي النظام {system}. ما إحداثي ص لنقطة التقاطع؟ أجب بعدد صحيح فقط.",
            _ =>
                $"مثّل المستقيمين في النظام {system}. صنّف عدد الحلول: اكتب 0 لعدم وجود حل (مستقيمان متوازيان)، أو 1 لحل واحد (نقطة تقاطع)، أو 2 لحلول غير منتهية (مستقيمان متطابقان)."
        };
        return Problem(family,prompt,
            "اكتب المستقيمين على هيئة ص=م س+ب. ميلان مختلفان يعنيان تقاطعًا وحيدًا؛ ميلان متساويان مع مقطعين مختلفين يعنيان مستقيمين متوازيين؛ التطابق التام يعني حلولًا غير منتهية. احسب س من تساوي التعبيرين ثم تحقق من ص في المعادلتين.",
            AssessmentItemType.Numeric,("m1",m1),("m2",m2),
            ("b1",b1),("b2",b2));
    }
}

internal static class UaeG10GraphSystemsResponse
{
    internal const string XFamily="uae.g10.general.u03.l01.graph_intersection_x";
    internal const string YFamily="uae.g10.general.u03.l01.graph_intersection_y";
    internal const string CountFamily="uae.g10.general.u03.l01.graph_solution_count";
    internal static bool Supports(string? family)=>
        family is XFamily or YFamily or CountFamily;
    private static bool Valid(IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["m1","m2","b1","b2"];
        return p.Count==4&&keys.All(p.ContainsKey)&&
            p["m1"] is >=-4 and <=4&&p["m2"] is >=-4 and <=4&&
            p["b1"] is >=-30 and <=30&&p["b2"] is >=-30 and <=30;
    }
    private static int Classification(IReadOnlyDictionary<string,int> p)=>
        p["m1"]!=p["m2"]?1:p["b1"]==p["b2"]?2:0;
    private static bool TryExpected(
        string family,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(family)||!Valid(p))return false;
        int kind=Classification(p);
        if(family==CountFamily){answer=kind;return true;}
        if(kind!=1)return false;
        int denominator=p["m1"]-p["m2"];
        int numerator=p["b2"]-p["b1"];
        if(numerator%denominator!=0)return false;
        int x=numerator/denominator;
        int y=p["m1"]*x+p["b1"];
        if(x is <-5 or >5||y is <-5 or >5||
            p["m2"]*x+p["b2"]!=y)return false;
        answer=family==XFamily?x:y;
        return true;
    }
    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!TryExpected(family,p,out var value))
            throw new InvalidOperationException("G10 graphical-system parameters are invalid.");
        return value.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!TryExpected(family,p,out var expected)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)return false;
        var normalized=string.Concat(answer.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-',
            _=>ch
        }));
        return int.TryParse(normalized,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out var submitted)
            &&submitted==expected;
    }
}
