using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10Postulates(string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int a=random.Next(2,31),b=random.Next(2,31),m=random.Next(1,8),n=random.Next(3,13);
        int x=random.Next(1,n),v=random.Next(10,81),w=random.Next(10,81);
        string labels="اكتب رقم الاختيار: 1=ضروري فقط، 2=كافٍ فقط، 3=ضروري وكافٍ، 4=لا أي منهما.";
        string prompt=family switch{
            UaeG10PostulatesResponse.Necessary=>$"في الأعداد الصحيحة، الشرط أ: س≥{a}، والنتيجة ب: س≥{b}. صنف أ بالنسبة لب. {labels}",
            UaeG10PostulatesResponse.Postulate=>$"في نموذج هندسي يضم نقاطًا مرقمة (ومنها {a} و{b+31})، حدد رقم المسلمة 1-j المناسبة: {UaeG10PostulatesResponse.Statement(m)} اكتب j من 1 إلى 7.",
            UaeG10PostulatesResponse.Classify=>$"في الفضاء الإقليدي، صنف العبارة التالية (نموذج {a}): {UaeG10PostulatesResponse.Claim(m)} اكتب 1=دائمًا، 2=أحيانًا، 3=أبدًا.",
            UaeG10PostulatesResponse.Midpoint=>$"م منتصف القطعة أ ب، وأ م={a} سم. أوجد طول أ ب بالسنتيمتر.",
            UaeG10PostulatesResponse.SharedMidpoint=>$"م منتصف أ ب وج د، والقطعتان أ ب وج د متطابقتان. إذا أ ب={2*a} سم، فأوجد طول ج م بالسنتيمتر.",
            UaeG10PostulatesResponse.Pairs=>$"بطولة تضم {n} فرق؛ يلعب كل فريق مع كل فريق آخر مرة واحدة. كم مباراة؟",
            UaeG10PostulatesResponse.Wheels=>$"عدد الدراجات ذات العجلتين والألواح ذات الأربع عجلات {n}، وعدد العجلات {2*n+2*x}. كم لوحًا؟",
            UaeG10PostulatesResponse.Travel=>$"مسار أ طوله {a} كم بسرعة ثابتة {v} كم/س، ومسار ب طوله {b} كم بسرعة {w} كم/س. أيهما أقل زمنًا؟ اكتب 1=أ، 2=ب، 3=متساويان.",
            UaeG10PostulatesResponse.Slope=>$"في نموذج تصميم تعليمي يلزم ارتفاع 4 سم لكل متر على الأقل. سقف يرتفع {a} سم خلال {b} متر. هل يحقق الشرط؟ اكتب 1=نعم، 0=لا.",
            _=>throw new InvalidOperationException()
        };
        return Problem(family,prompt,"تحقق من الفرضيات واتجاه الاستلزام، أو استخدم المسلمة أو العلاقة الرياضية المناسبة. البرهان الحر الكامل يراجعه المعلم.",
            AssessmentItemType.Numeric,("a",a),("b",b),("m",m),("n",n),("x",x),("v",v),("w",w));
    }
}
internal static class UaeG10PostulatesResponse
{
    internal const string Prefix="uae.g10.general.u18.l01.";
    internal const string Necessary=Prefix+"necessary_sufficient_threshold",Postulate=Prefix+"incidence_postulate",Classify=Prefix+"always_sometimes_never",Midpoint=Prefix+"midpoint_total",SharedMidpoint=Prefix+"shared_midpoint_half",Pairs=Prefix+"unordered_pair_count",Wheels=Prefix+"wheel_count_reasoning",Travel=Prefix+"travel_time_comparison",Slope=Prefix+"slope_requirement";
    internal static bool Supports(string? f)=>f is Necessary or Postulate or Classify or Midpoint or SharedMidpoint or Pairs or Wheels or Travel or Slope;
    internal static string Statement(int m)=>m switch{1=>"نقطتان مختلفتان تعينان مستقيمًا وحيدًا.",2=>"ثلاث نقاط غير مستقيمة تعين مستوى وحيدًا.",3=>"كل مستقيم يحوي نقطتين على الأقل.",4=>"كل مستوى يحوي ثلاث نقاط غير مستقيمة على الأقل.",5=>"إذا وقعت نقطتان مختلفتان من مستقيم في مستوى فإن المستقيم كله فيه.",6=>"إذا تقاطع مستقيمان مختلفان فتقاطعهما نقطة واحدة فقط.",7=>"إذا تقاطع مستويان مختلفان فتقاطعهما مستقيم.",_=>throw new InvalidOperationException()};
    internal static string Claim(int m)=>m switch{1=>"نقطتان مختلفتان تعينان مستقيمًا وحيدًا.",2=>"ثلاث نقاط مختلفة تعين مستوى وحيدًا.",3=>"مستقيمان مختلفان يشتركان في نقطتين مختلفتين.",4=>"مستقيمان مختلفان في الفضاء يتقاطعان.",5=>"نقطتان مختلفتان من مستقيم في مستوى تعنيان وقوع المستقيم كله فيه.",6=>"يتقاطع مستويان مختلفان في نقطة واحدة فقط.",7=>"ثلاث نقاط غير مستقيمة تقع في مستوى واحد فقط.",_=>throw new InvalidOperationException()};
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==7&&new[]{"a","b","m","n","x","v","w"}.All(p.ContainsKey)&&p["a"] is >=2 and <=30&&p["b"] is >=2 and <=30&&p["m"] is >=1 and <=7&&p["n"] is >=3 and <=12&&p["x"]>=1&&p["x"]<p["n"]&&p["v"] is >=10 and <=80&&p["w"] is >=10 and <=80;
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid bounded postulates parameters.");
        int a=p["a"],b=p["b"],m=p["m"],n=p["n"];
        int value=f switch{Necessary=>a==b?3:a<b?1:2,Postulate=>m,Classify=>m is 1 or 5 or 7?1:m is 2 or 4?2:3,Midpoint=>2*a,SharedMidpoint=>a,Pairs=>n*(n-1)/2,Wheels=>p["x"],Travel=>a*p["w"]==b*p["v"]?3:a*p["w"]<b*p["v"]?1:2,Slope=>a>=4*b?1:0,_=>throw new InvalidOperationException()};
        return value.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>24)return false;
        string raw=string.Concat(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c));
        if(!int.TryParse(raw,NumberStyles.None,CultureInfo.InvariantCulture,out int k)||k>1000)return false;
        int a=p["a"],b=p["b"],m=p["m"],n=p["n"];
        switch(f)
        {
            case Necessary:
                // The finite witnesses include both threshold boundaries and both tails.
                bool ab=true,ba=true;for(int z=1;z<=31;z++){if(z>=a&&z<b)ab=false;if(z>=b&&z<a)ba=false;}
                return k==(ab&&ba?3:ba?1:ab?2:4);
            case Postulate: return k>=1&&k<=7&&Statement(k)==Statement(m);
            case Classify:
                // 1,5,7 follow directly from the incidence axioms; 2,4 admit examples and counterexamples;
                // 3 and 6 contradict the unique line / plane-intersection axioms.
                return k==1&&(m==1||m==5||m==7)||k==2&&(m==2||m==4)||k==3&&(m==3||m==6);
            case Midpoint:return k-a==a;
            case SharedMidpoint:return k+k==2*a;
            case Pairs:int count=0;for(int i=0;i<n;i++)for(int j=i+1;j<n;j++)count++;return k==count;
            case Wheels:return k<=n&&4*k+2*(n-k)==2*n+2*p["x"];
            case Travel:decimal t1=(decimal)a/p["v"],t2=(decimal)b/p["w"];return k==(t1==t2?3:t1<t2?1:2);
            case Slope:return k==(a/b>=4?1:0);
            default:return false;
        }
    }
}
