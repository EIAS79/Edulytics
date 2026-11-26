using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// Selected source 8-3, physical pages 21–30. Bounded exact questions;
// open graph constructions, model comparison and regression remain teacher-assessed.
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10LogarithmicGraph(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int b=random.Next(2,6),m=random.Next(1,4),n=random.Next(-4,5),
            h=random.Next(-4,5),k=random.Next(-4,5),a=random.Next(1,4)*(random.Next(0,2)==0?-1:1),
            dir=random.Next(0,2),slot=random.Next(0,4),period=random.Next(1,7);
        string basis=UaeG10LogarithmicGraphResponse.PowerText(b,dir==0?-m:m);
        string arg=UaeG10LogarithmicGraphResponse.PowerText(b,n);
        string model=$"د(س)=({a})×لوغ_({basis})(س−({h}))+({k})";
        int xNumerator=h*period+UaeG10LogarithmicGraphResponse.Pow(b,Math.Abs(n));
        string xProbe=UaeG10LogarithmicGraphResponse.RationalText(xNumerator,period);
        string exponent=UaeG10LogarithmicGraphResponse.RationalText(dir==1?n:-n,m);
        string prompt=family switch
        {
            UaeG10LogarithmicGraphResponse.Evaluate=>$"احسب لوغ_({basis})({arg}) في صورة عدد صحيح أو كسر مبسط.",
            UaeG10LogarithmicGraphResponse.ToExponential=>$"حوّل لوغ_({basis})({arg})={exponent} إلى صيغة أسية. {UaeG10LogarithmicGraphResponse.Options(b,m,n,dir,slot,true)}",
            UaeG10LogarithmicGraphResponse.ToLogarithmic=>$"حوّل ({basis})^({exponent})={arg} إلى صيغة لوغاريتمية. {UaeG10LogarithmicGraphResponse.Options(b,m,n,dir,slot,false)}",
            UaeG10LogarithmicGraphResponse.Domain=>$"في {model}، المجال س>هـ. اكتب الحد العددي هـ فقط.",
            UaeG10LogarithmicGraphResponse.DomainMember=>$"في {model}، هل س={h+n} ينتمي إلى المجال؟ اكتب 1 لنعم أو 0 للا.",
            UaeG10LogarithmicGraphResponse.Asymptote=>$"في {model}، اكتب قيمة س للمقارب الرأسي.",
            UaeG10LogarithmicGraphResponse.Monotonic=>$"في {model}، هل الدالة متزايدة على مجالها؟ اكتب 1 لنعم أو 0 للا.",
            UaeG10LogarithmicGraphResponse.GraphAnchor=>$"في {model}، عند ص={k} ما إحداثي س للنقطة المرجعية؟",
            UaeG10LogarithmicGraphResponse.GraphValue=>$"لدالة د(س)=({a})×لوغ_({UaeG10LogarithmicGraphResponse.PowerText(b,dir==0?-1:1)})({period}×(س−({h})))+({k})، احسب د({xProbe}).",
            UaeG10LogarithmicGraphResponse.InverseShift=>$"معكوس د(س)={b}^(س−({h})) هو لوغ_{b}(س)+ج. أوجد ج.",
            UaeG10LogarithmicGraphResponse.ScaleRatio=>dir==1?$"قياس لوغاريتمي للأساس {b} يزداد بمقدار {Math.Abs(n)} درجات. كم مرة تصبح الكمية الأصلية؟":$"في نموذج تعويض الضوء ن=لوغ_2(1/ف)، إذا ن={Math.Abs(n)} فاكتب مقام نسبة الضوء ف=1/م.",
            _=>dir==1?$"عدد خلايا افتراضي يتضاعف كل {period} ساعات. إذا أصبحت الكمية {UaeG10LogarithmicGraphResponse.Pow(2,Math.Abs(n)+1)} أمثال البداية، فكم ساعة مرت؟":$"زاد عدد خلايا افتراضي إلى {UaeG10LogarithmicGraphResponse.Pow(2,Math.Abs(n)+1)} أمثال البداية خلال {period*(Math.Abs(n)+1)} ساعات. ما زمن الجيل بالساعات؟"
        };
        return Problem(family,prompt,"تحقق من الحجة والأساس، ثم استعمل التكافؤ بين الصيغتين الأسية واللوغاريتمية وخصائص التحويل.",family==UaeG10LogarithmicGraphResponse.Evaluate?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
            ("b",b),("m",m),("n",n),("h",h),("k",k),("a",a),("dir",dir),("slot",slot),("period",period));
    }
}
internal static class UaeG10LogarithmicGraphResponse
{
    internal const string Prefix="uae.g10.general.u05.l03.";
    internal const string Evaluate=Prefix+"evaluate_rational",ToExponential=Prefix+"convert_exponential",ToLogarithmic=Prefix+"convert_logarithmic",
        Domain=Prefix+"domain_boundary",DomainMember=Prefix+"domain_membership",Asymptote=Prefix+"vertical_asymptote",
        Monotonic=Prefix+"is_increasing",GraphAnchor=Prefix+"graph_anchor",GraphValue=Prefix+"graph_value",
        InverseShift=Prefix+"inverse_shift",ScaleRatio=Prefix+"scale_ratio",ModelTime=Prefix+"doubling_time";
    internal static bool Supports(string? f)=>f is Evaluate or ToExponential or ToLogarithmic or Domain or DomainMember or Asymptote or Monotonic or GraphAnchor or GraphValue or InverseShift or ScaleRatio or ModelTime;
    internal static int Pow(int b,int n){int v=1;for(int i=0;i<n;i++)v*=b;return v;}
    internal static string PowerText(int b,int n)=>n<0?$"1/{Pow(b,-n)}":Pow(b,n).ToString(CultureInfo.InvariantCulture);
    internal static string RationalText(int num,int den)
    {
        int g=Gcd(Math.Abs(num),den);num/=g;den/=g;
        return den==1?num.ToString(CultureInfo.InvariantCulture):$"{num}/{den}";
    }
    private static int OptionNumerator(int correct,int slot,int i)=>
        i==slot?correct:correct+(i<slot?i+1:i);
    internal static string Options(int b,int m,int n,int dir,int slot,bool exponential)
    {
        string basis=PowerText(b,dir==1?m:-m),arg=PowerText(b,n);
        var options=Enumerable.Range(0,4).Select(i=>{
            string e=RationalText(OptionNumerator(dir==1?n:-n,slot,i),m);
            return exponential?$"{i+1}) ({basis})^({e})={arg}":$"{i+1}) لوغ_({basis})({arg})={e}";
        });
        return "اختر الرقم: "+string.Join("؛ ",options);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>
        Supports(f)&&p.Count==9&&new[]{"b","m","n","h","k","a","dir","slot","period"}.All(p.ContainsKey)&&
        p["b"] is >=2 and <=5&&p["m"] is >=1 and <=3&&p["n"] is >=-4 and <=4&&
        p["h"] is >=-4 and <=4&&p["k"] is >=-4 and <=4&&p["a"] is >=-3 and <=3&&p["a"]!=0&&
        p["dir"] is 0 or 1&&p["slot"] is >=0 and <=3&&p["period"] is >=1 and <=6;
    private static int IntegerAnswer(string f,IReadOnlyDictionary<string,int> p)=>f switch{
        ToExponential or ToLogarithmic=>p["slot"]+1,
        Domain or Asymptote or InverseShift=>p["h"],
        DomainMember=>p["n"]>0?1:0,
        Monotonic=>(p["dir"]==1)==(p["a"]>0)?1:0,
        GraphAnchor=>p["h"]+1,
        GraphValue=>p["a"]*Math.Abs(p["n"])*(p["dir"]==1?1:-1)+p["k"],
        ScaleRatio=>Pow(p["dir"]==1?p["b"]:2,Math.Abs(p["n"])),
        ModelTime=>p["dir"]==1?p["period"]*(Math.Abs(p["n"])+1):p["period"],
        _=>throw new InvalidOperationException()
    };
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid bounded logarithm parameters.");
        if(f!=Evaluate)return IntegerAnswer(f,p).ToString(CultureInfo.InvariantCulture);
        int num=p["n"]*(p["dir"]==1?1:-1),den=p["m"];
        int g=Gcd(Math.Abs(num),den);num/=g;den/=g;
        return den==1?num.ToString(CultureInfo.InvariantCulture):$"{num}/{den}";
    }
    private static int Gcd(int a,int b){while(b!=0){int t=a%b;a=b;b=t;}return a;}
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(answer))return false;
        string raw=string.Concat(answer.Trim().Select(ch=>ch switch{
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),>= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-', '⁄' or '∕'=>'/',_=>ch}));
        if(f==Evaluate)
        {
            var parts=raw.Split('/');
            if(parts.Length is <1 or >2||!int.TryParse(parts[0],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int num))return false;
            int den=1;
            if(parts.Length==2&&(!int.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out den)||den<=0))return false;
            // Substitute into (b^(±m))^(num/den)=b^n, exact integer exponent equality.
            return (long)(p["dir"]==1?p["m"]:-p["m"])*num==(long)p["n"]*den;
        }
        if(!int.TryParse(raw,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int v))return false;
        // Check the original source relation/constraint rather than comparing to a stored answer.
        return f switch{
            ToExponential or ToLogarithmic=>v is >=1 and <=4&&OptionNumerator(p["dir"]==1?p["n"]:-p["n"],p["slot"],v-1)*(p["dir"]==1?1:-1)==p["n"],
            Domain or Asymptote=>v-p["h"]==0,
            DomainMember=>v is 0 or 1&&(v==1)==(p["h"]+p["n"]>p["h"]),
            Monotonic=>v is 0 or 1&&(v==1)==(p["dir"]==1?p["a"]>0:p["a"]<0),
            GraphAnchor=>v-p["h"]==1,
            GraphValue=>(long)v-p["k"]==(long)p["a"]*(p["dir"]==1?Math.Abs(p["n"]):-Math.Abs(p["n"])),
            InverseShift=>v==p["h"],
            ScaleRatio=>v==Pow(p["dir"]==1?p["b"]:2,Math.Abs(p["n"])),
            ModelTime=>p["dir"]==1?v>=0&&v%p["period"]==0&&v/p["period"]==Math.Abs(p["n"])+1:v>0&&v*(long)(Math.Abs(p["n"])+1)==p["period"]*(long)(Math.Abs(p["n"])+1),
            _=>false
        };
    }
}
