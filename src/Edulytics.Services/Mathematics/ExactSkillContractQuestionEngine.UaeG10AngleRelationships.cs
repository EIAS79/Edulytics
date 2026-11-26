using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10AngleRelationships(string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int a=random.Next(10,76),b=random.Next(5,61),x=random.Next(2,21),p=random.Next(0,31),
            m=random.Next(2,5),mode=random.Next(1,9),variant=random.Next(4);
        int total=variant%2==0?90:180, angle=a+b;
        int left=a-m*x,right=total-a-(m+1)*x,
            first=angle-m*x,second=angle-(m+1)*x;
        string choices="اكتب رقم المبرر: 1=انعكاس التطابق، 2=تماثل التطابق، 3=تعدي التطابق، 4=التقابل بالرأس، 5=تطابق المكملتين، 6=تطابق المتممتين، 7=تطابق الزوايا القائمة، 8=زوج خطي متطابق.";
        string prompt=family switch{
            UaeG10AngleRelationshipsResponse.Protractor=>$"على التدريج نفسه في نصف المستوى، قراءتا الشعاعين {(variant%2==0?p:p+a+b)}° و{(variant%2==0?p+a+b:p)}°. ما قياس الزاوية بينهما بالدرجات؟",
            UaeG10AngleRelationshipsResponse.Addition=>variant%2==0?$"الشعاع BD داخل ∠ABC، وقياس ∠ABD={a}° و∠DBC={b}°. أوجد قياس ∠ABC.":$"الشعاع BD داخل ∠ABC وقياس ∠ABC={a+b}° و∠ABD={a}°. أوجد قياس ∠DBC.",
            UaeG10AngleRelationshipsResponse.Complement=>variant%2==0?$"زاويتان متتامتان إحداهما {a}°. أوجد الأخرى بالدرجات.":$"في الربع نفسه يصنع خط النظر {a}° مع العمودي؛ الأفقي متعامد مع العمودي. ما زاويته مع الأفقي بالدرجات؟",
            UaeG10AngleRelationshipsResponse.Supplement=>$"زاويتان تشكلان زوجًا خطيًا. قياس إحداهما {(variant%2==0?a:180-a)}°. أوجد الأخرى.",
            UaeG10AngleRelationshipsResponse.RightPartition=>variant%2==0?$"ثلاث زوايا متجاورة تكوّن زاوية مستقيمة. الطرفيتان متطابقتان والوسطى {180-2*a}°. ما قياس كل طرفية؟":$"ثلاث زوايا متجاورة على مستقيم؛ الوسطى قائمة والطرفيتان متطابقتان. أوجد قياس كل طرفية.",
            UaeG10AngleRelationshipsResponse.Linear=>$"زاويتان متجاورتان تشكلان زاوية {(total==90?"قائمة":"مستقيمة")}. قياساهما ({m}س+({left}))° و({m+1}س+({right}))°. أوجد س.",
            UaeG10AngleRelationshipsResponse.VerticalEquation=>$"زاويتان متقابلتان بالرأس قياساهما ({m}س+({first}))° و({m+1}س+({second}))°. أوجد {(variant%2==0?"س":"قياس كل زاوية بالدرجات")}.",
            UaeG10AngleRelationshipsResponse.VerticalMeasure=>$"يتقاطع مستقيمان، وقياس ∠1={angle}°، والزوايا 1،2،3،4 متتابعة حول التقاطع. أوجد قياس ∠{(variant%2==0?3:2)}.",
            UaeG10AngleRelationshipsResponse.SameReference=>variant<2?$"الزاويتان أ وب {(total==90?"متممتان":"مكملتان")} للزاوية ج التي قياسها {a}°. أوجد قياس ب.":$"∠أ و∠ب {(total==90?"متممتان":"مكملتان")} لزاويتين متطابقتين، على الترتيب. قياس ∠أ={a}°. أوجد قياس ∠ب.",
            UaeG10AngleRelationshipsResponse.Bisector=>variant switch{0=>$"زاوية قياسها {2*a}° ينصفها شعاع. أوجد قياس أحد النصفين.",1=>$"شعاع ينصف زاوية وقياس أحد النصفين {a}°. أوجد قياس الزاوية الكلية.",2=>$"زاوية قياسها {2*a}° ينصفها شعاع، والزاوية 3 تقابل أحد النصفين بالرأس. أوجد قياس 3.",_=>$"في نموذج ذراع مهتز، زاوية الحركة 90° والشعاع الوسطي منصف. أوجد قياس كل نصف."},
            UaeG10AngleRelationshipsResponse.AroundPoint=>$"أربع زوايا متتابعة حول نقطة، قياسات ثلاث منها {a}° و{90+b}° و{90-a}°. أوجد الرابعة.",
            UaeG10AngleRelationshipsResponse.Reason=>$"{UaeG10AngleRelationshipsResponse.ReasonStatement(mode)} {choices}",
            UaeG10AngleRelationshipsResponse.Perpendicular=>$"{UaeG10AngleRelationshipsResponse.PerpendicularStatement(variant)} هل تكفي المعطيات وحدها لإثبات تعامد المستقيمين؟ اكتب 1=نعم أو 0=لا.",
            UaeG10AngleRelationshipsResponse.Transfer=>$"في تقاطعين مستقلين، ∠1 تقابل ∠2 بالرأس، و∠3 تقابل ∠4 بالرأس، و∠1≅∠3. قياس ∠1={angle}°. أوجد قياس {(variant%2==0?"∠4":"زاوية مجاورة لـ∠4 في زوج خطي")}.",
            _=>throw new InvalidOperationException("Unknown angle relationship family.")
        };
        return Problem(family,prompt,"حدد المعطيات وعلاقة الزوايا، ثم طبّق مسلمة أو نظرية مناسبة وافحص القياس. البرهان الكامل والرسم الحر يراجعهما المعلم.",
            AssessmentItemType.Numeric,("a",a),("b",b),("x",x),("p",p),("m",m),("mode",mode),("variant",variant));
    }
}

internal static class UaeG10AngleRelationshipsResponse
{
    internal const string Prefix="uae.g10.general.u18.l03.";
    internal const string Protractor=Prefix+"protractor_difference",Addition=Prefix+"angle_addition",
        Complement=Prefix+"complementary_measure",Supplement=Prefix+"supplementary_measure",
        RightPartition=Prefix+"equal_partition",Linear=Prefix+"adjacent_angle_equation",
        VerticalEquation=Prefix+"vertical_angle_equation",VerticalMeasure=Prefix+"intersection_measure",
        SameReference=Prefix+"same_reference_congruence",Bisector=Prefix+"bisector_vertical",
        AroundPoint=Prefix+"around_point",Reason=Prefix+"congruence_reason",
        Perpendicular=Prefix+"perpendicular_sufficient",Transfer=Prefix+"two_crossing_transfer";
    internal static bool Supports(string? f)=>f is Protractor or Addition or Complement or Supplement or RightPartition or Linear or VerticalEquation or VerticalMeasure or SameReference or Bisector or AroundPoint or Reason or Perpendicular or Transfer;
    internal static string ReasonStatement(int mode)=>mode switch{
        1=>"∠A≅∠A دون معطيات إضافية.",
        2=>"من ∠A≅∠B نستنتج ∠B≅∠A.",
        3=>"من ∠A≅∠B و∠B≅∠C نستنتج ∠A≅∠C.",
        4=>"عند تقاطع مستقيمين نستنتج تطابق زاويتين متقابلتين بالرأس.",
        5=>"∠A مكملة لـ∠P، و∠B مكملة لـ∠Q، و∠P≅∠Q؛ نستنتج ∠A≅∠B.",
        6=>"∠A متممة لـ∠P، و∠B متممة لـ∠Q، و∠P≅∠Q؛ نستنتج ∠A≅∠B.",
        7=>"زاويتان قائمتان في رسمين منفصلين، فنستنتج تطابقهما.",
        8=>"زاويتان متطابقتان تشكلان زاوية مستقيمة، فنستنتج أنهما قائمتان.",
        _=>throw new InvalidOperationException()
    };
    internal static string PerpendicularStatement(int variant)=>variant switch{
        0=>"يتقاطع مستقيمان، وإحدى الزوايا الناتجة قائمة.",
        1=>"يتقاطع مستقيمان، وزاويتان متجاورتان ناتجتان عنهما متطابقتان.",
        2=>"يتقاطع مستقيمان، وزاويتان متقابلتان بالرأس متطابقتان.",
        3=>"يتقاطع مستقيمان، وزاويتان متجاورتان ناتجتان عنهما متكاملتان.",
        _=>throw new InvalidOperationException()
    };
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>
        Supports(f)&&p.Count==7&&new[]{"a","b","x","p","m","mode","variant"}.All(p.ContainsKey)&&
        p["a"] is >=10 and <=75&&p["b"] is >=5 and <=60&&p["x"] is >=2 and <=20&&
        p["p"] is >=0 and <=30&&p["m"] is >=2 and <=4&&p["mode"] is >=1 and <=8&&p["variant"] is >=0 and <=3;
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid bounded geometry angle parameters.");
        int a=p["a"],b=p["b"],x=p["x"],v=p["variant"],t=v%2==0?90:180;
        int result=f switch{
            Protractor=>a+b,Addition=>v%2==0?a+b:b,Complement=>90-a,Supplement=>v%2==0?180-a:a,
            RightPartition=>v%2==0?a:45,Linear=>x,VerticalEquation=>v%2==0?x:a+b,
            VerticalMeasure or Transfer=>v%2==0?a+b:180-a-b,SameReference=>v<2?t-a:a,
            Bisector=>v switch{0 or 2=>a,1=>2*a,_=>45},AroundPoint=>180-b,
            Reason=>p["mode"],Perpendicular=>v<2?1:0,_=>throw new InvalidOperationException()
        };
        return result.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>24)return false;
        string s=UaeG10TrigScalar.Normalize(answer);
        if(!int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int z)||z>360)return false;
        int a=p["a"],b=p["b"],x=p["x"],v=p["variant"],m=p["m"],start=p["p"],t=v%2==0?90:180;
        if(f==Linear)
        {
            long l=m*(long)z+a-m*x,r=(m+1L)*z+t-a-(m+1L)*x;
            return l>0&&r>0&&l+r==t;
        }
        if(f==VerticalEquation)
        {
            long leftOffset=a+b-m*x,rightOffset=a+b-(m+1L)*x;
            if(v%2==0){long l=m*(long)z+leftOffset,r=(m+1L)*z+rightOffset;return l==r&&l>0&&l<180;}
            // A proposed measure must produce a common root of both original expressions.
            return z>0&&z<180&&(z-leftOffset)*(m+1L)==(z-rightOffset)*m;
        }
        if(f==Reason)
        {
            // Fixed source theorem situations; choices 1..8 refer to distinct explicit premises.
            return z is >=1 and <=8&&z==p["mode"];
        }
        if(f==Perpendicular)
        {
            // One right angle or an equal linear pair forces 90 degrees; vertical equality or a
            // linear pair alone also occurs at 40/140 degrees, and is insufficient.
            return z is 0 or 1&&(z==1)==(v is 0 or 1);
        }
        if(z<=0)return false;
        return f switch{
            Protractor=>z==Math.Abs(start-(start+a+b)),
            Addition=>v%2==0?z-a==b:a+z==a+b,
            Complement=>a+z==90,
            Supplement=>z+(v%2==0?a:180-a)==180,
            RightPartition=>2*z+(v%2==0?180-2*a:90)==180,
            VerticalMeasure or Transfer=>v%2==0?z+a+b+2*(180-a-b)==360:z+a+b==180,
            SameReference=>v<2?z+a==t:z+(t-a)==t,
            Bisector=>v switch{0 or 2=>2*z==2*a,1=>z==a+a,_=>2*z==90},
            AroundPoint=>z+a+(90+b)+(90-a)==360,
            _=>false
        };
    }
}
