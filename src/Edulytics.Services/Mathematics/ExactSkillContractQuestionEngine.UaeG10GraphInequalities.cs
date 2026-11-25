using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// UAE G10 General U03L02: a point belongs to the overlap of two linear
/// half-planes only if BOTH inequalities hold. Strict inequalities exclude
/// their boundary; weak inequalities include it. Graph drawing is teacher scored.
/// Stage C authored. Stage D tests and production routing are NOT active.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10GraphInequalities(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int bound=difficulty==ExactSkillQuestionDifficulty.Challenge?3:2;
        int m1=random.Next(-bound,bound+1),m2=random.Next(-bound,bound+1);
        int b1=random.Next(-5,6),b2=random.Next(-5,6);
        int t1=random.Next(0,4),t2=random.Next(0,4);
        int x=random.Next(-5,6);
        int y=family==UaeG10GraphInequalitiesResponse.BoundaryFamily
            ? m1*x+b1
            : random.Next(-7,8);
        string op1=UaeG10GraphInequalitiesResponse.Symbol(t1);
        string op2=UaeG10GraphInequalitiesResponse.Symbol(t2);
        string inequalities=$"ص {op1} ({m1})س+({b1})، ص {op2} ({m2})س+({b2})";
        string prompt=family==UaeG10GraphInequalitiesResponse.BoundaryFamily
            ? $"النقطة ({x}،{y}) تقع على حد المتباينة الأولى للنظام {inequalities}. هل تدخل في منطقة الحل المشتركة؟ اكتب 1 إذا كانت تحقق المتباينتين معًا، أو 0 إذا لا."
            : $"مثّل نظام المتباينتين {inequalities} على المستوى الإحداثي. هل النقطة ({x}،{y}) تقع في تقاطع منطقتي الحل؟ اكتب 1 إذا كانت تحقق المتباينتين معًا، أو 0 إذا لا.";
        return Problem(family,prompt,
            "اختبر كل متباينة مستقلًا بالتعويض في إحداثيي النقطة. بالنسبة لخط الحد يكون التظليل على الجانب الصحيح، ويُستبعد الخط المتقطع للمتباينات الصارمة < و >، ويُقبل الخط المتصل للمتباينات الشاملة ≤ و ≥. تقاطع المنطقتين فقط هو حل النظام.",
            AssessmentItemType.Numeric,
            ("m1",m1),("m2",m2),("b1",b1),("b2",b2),
            ("t1",t1),("t2",t2),("x",x),("y",y));
    }
}

internal static class UaeG10GraphInequalitiesResponse
{
    internal const string PointFamily="uae.g10.general.u03.l02.feasible_point_membership";
    internal const string BoundaryFamily="uae.g10.general.u03.l02.boundary_point_membership";
    internal static bool Supports(string? family)=>family is PointFamily or BoundaryFamily;

    // 0 = >, 1 = >=, 2 = <, 3 = <=
    internal static string Symbol(int kind)=>kind switch
    {
        0=>">",1=>"≥",2=>"<",3=>"≤",
        _=>throw new InvalidOperationException("Unsupported inequality relation.")
    };

    private static bool Valid(IReadOnlyDictionary<string,int> p)
    {
        string[] required=["m1","m2","b1","b2","t1","t2","x","y"];
        return p.Count==8 && required.All(p.ContainsKey) &&
            p["m1"] is >=-3 and <=3 && p["m2"] is >=-3 and <=3 &&
            p["b1"] is >=-5 and <=5 && p["b2"] is >=-5 and <=5 &&
            p["t1"] is >=0 and <=3 && p["t2"] is >=0 and <=3 &&
            p["x"] is >=-5 and <=5 && p["y"] is >=-20 and <=20;
    }

    private static bool Holds(int y,int rhs,int relation)=>relation switch
    {
        0=>y>rhs,1=>y>=rhs,2=>y<rhs,3=>y<=rhs,_=>false
    };

    private static bool TryExpected(string family,IReadOnlyDictionary<string,int> p,out int value)
    {
        value=0;
        if(!Supports(family)||!Valid(p))return false;
        int x=p["x"],y=p["y"];
        int line1=p["m1"]*x+p["b1"],line2=p["m2"]*x+p["b2"];
        if(family==BoundaryFamily&&y!=line1)return false;
        value=Holds(y,line1,p["t1"])&&Holds(y,line2,p["t2"])?1:0;
        return true;
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!TryExpected(family,p,out int answer))
            throw new InvalidOperationException("Invalid Grade10 inequality-system parameters.");
        return answer.ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!TryExpected(family,p,out int expected)||
            string.IsNullOrWhiteSpace(answer)||answer.Length>20)return false;
        string digits=string.Concat(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '−' or '–'=>'-',_=>c
        }));
        return int.TryParse(digits,NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}
