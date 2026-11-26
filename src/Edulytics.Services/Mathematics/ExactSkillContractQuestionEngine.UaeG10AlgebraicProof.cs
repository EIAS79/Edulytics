using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10AlgebraicProof(string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int a=random.Next(2,7),b=random.Next(1,10),x=random.Next(1,16),k=random.Next(2,11),m=random.Next(1,10);
        int t=2*random.Next(1,7),u=random.Next(2,21),v=random.Next(2,21);
        int signed=m%2==0?x:-x;
        string choices="اكتب رقم الخاصية: 1=الجمع، 2=الطرح، 3=الضرب، 4=القسمة، 5=الانعكاس، 6=التماثل، 7=التعدي، 8=التعويض، 9=التوزيع.";
        string prompt=family switch{
            UaeG10AlgebraicProofResponse.Property=>$"{UaeG10AlgebraicProofResponse.PropertyStatement(m,a,b)} {choices}",
            UaeG10AlgebraicProofResponse.Linear=>$"حل {a}(س+{b})={a*(signed+b)}. أوجد س؛ برر بالخطوات في البرهان الذي يراجعه المعلم.",
            UaeG10AlgebraicProofResponse.Rational=>$"حل س/{a}+{b}={signed+b}. أوجد س.",
            UaeG10AlgebraicProofResponse.Segment=>$"أ ب≅ج د، أ ب={a+1}س+{b} وج د={a}س+{b+x}. أوجد س.",
            UaeG10AlgebraicProofResponse.Angle=>$"∠أ≅∠ب، قياس أ=({a+1}س+{b})° وقياس ب=({a}س+{b+x})°. أوجد س.",
            UaeG10AlgebraicProofResponse.Temperature=>$"C=5(F−32)/9. إذا C={5*x} درجة مئوية، أوجد F بالفهرنهايت.",
            UaeG10AlgebraicProofResponse.Motion=>$"d=(u+v)t/2؛ d={(u+v)*t/2} متر، v={v} م/ث، t={t} ث. أوجد u بالمتر/ث.",
            UaeG10AlgebraicProofResponse.Acceleration=>$"d=vt+at²/2؛ d={v*t+x*t*t/2} م، v={v} م/ث، t={t} ث. أوجد a بالمتر/ث².",
            UaeG10AlgebraicProofResponse.Heart=>$"في نموذج حسابي تعليمي H=0.75(220−a)، إذا H={165-3*x} أوجد قيمة a؛ هذه مسألة جبرية لا توصية طبية.",
            UaeG10AlgebraicProofResponse.Gas=>$"PV=nRT في نموذج الغاز المثالي، R=0.0821، P=1 atm، n={a} mol، V={(a*821*(200+10*x)/10000m).ToString("0.0000",CultureInfo.InvariantCulture)} L. أوجد T بالكلفن.",
            UaeG10AlgebraicProofResponse.Voltage=>$"V=P/I؛ القدرة {2*x*v} واط والتيار {v} أمبير. أوجد الجهد عند {(m%2==0?"مضاعفة التيار وثبات القدرة":"مضاعفة القدرة وثبات التيار")}.",
            UaeG10AlgebraicProofResponse.Cube=>$"مكعب ضلعه {a} وحدات. إذا ضربنا طول ضلعه في {k}، فما عامل تغير حجمه؟",
            UaeG10AlgebraicProofResponse.Pythagoras=>$"في مثلث قائم، طول الوتر {5*k} وحدة وأحد الضلعين القائمين {3*k} وحدات. أوجد طول الضلع الآخر.",
            UaeG10AlgebraicProofResponse.Equivalence=>$"صنف العلاقة على الأعداد الحقيقية: {UaeG10AlgebraicProofResponse.Relation(m,a)}. هل هي علاقة تكافؤ؟ اكتب 1=نعم أو 0=لا.",
            _=>throw new InvalidOperationException()
        };
        return Problem(family,prompt,"استخدم خاصية المساواة المناسبة مع شرط عدم القسمة على صفر، ثم تحقق بالتعويض. إثبات الخطوات كاملة يراجعه المعلم.",
            AssessmentItemType.Numeric,("a",a),("b",b),("x",x),("k",k),("m",m),("t",t),("u",u),("v",v));
    }
}
internal static class UaeG10AlgebraicProofResponse
{
    internal const string Prefix="uae.g10.general.u18.l02.";
    internal const string Property=Prefix+"equality_property",Linear=Prefix+"distributed_equation",Rational=Prefix+"rational_linear_equation",Segment=Prefix+"congruent_segment_equation",Angle=Prefix+"congruent_angle_equation",Temperature=Prefix+"temperature_formula",Motion=Prefix+"motion_formula",Acceleration=Prefix+"acceleration_formula",Heart=Prefix+"heart_rate_formula",Gas=Prefix+"ideal_gas_formula",Voltage=Prefix+"voltage_scaling",Cube=Prefix+"cube_volume_scaling",Pythagoras=Prefix+"positive_pythagorean_root",Equivalence=Prefix+"equivalence_relation";
    internal static bool Supports(string? f)=>f is Property or Linear or Rational or Segment or Angle or Temperature or Motion or Acceleration or Heart or Gas or Voltage or Cube or Pythagoras or Equivalence;
    internal static string PropertyStatement(int m,int a,int b)=>m switch{
        1=>$"من س=ص نستنتج س+{a}=ص+{a}.",2=>$"من س=ص نستنتج س−{a}=ص−{a}.",3=>$"من س=ص نستنتج {a}س={a}ص.",4=>$"من س=ص نستنتج س/{a}=ص/{a}.",
        5=>$"{a}+س={a}+س.",6=>$"من س={b} نستنتج {b}=س.",7=>$"من س=ص وص={b} نستنتج س={b}.",8=>$"إذا أ=س+{a} وس={b} نستنتج أ={b}+{a}.",9=>$"{a}(س+{b})={a}س+{a*b}.",_=>throw new InvalidOperationException()};
    internal static string Relation(int m,int a)=>m switch{1=>"س=ص",2=>"س≤ص",3=>"س>ص",4=>"س≠ص",5=>$"|س−ص|≤{a}",6=>$"س+{a}=ص+{a}",7=>$"{a}س={a}ص",8=>$"س−ص={a}",9=>$"س²=ص²",_=>throw new InvalidOperationException()};
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==8&&new[]{"a","b","x","k","m","t","u","v"}.All(p.ContainsKey)&&p["a"] is >=2 and <=6&&p["b"] is >=1 and <=9&&p["x"] is >=1 and <=15&&p["k"] is >=2 and <=10&&p["m"] is >=1 and <=9&&p["t"] is >=2 and <=12&&p["t"]%2==0&&p["u"] is >=2 and <=20&&p["v"] is >=2 and <=20;
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid bounded algebraic proof parameters.");
        int a=p["a"],x=p["x"],m=p["m"],k=p["k"];
        int value=f switch{Property=>m,Linear=>m%2==0?x:-x,Segment or Angle or Acceleration=>x,Rational=>a*(m%2==0?x:-x),Temperature=>9*x+32,Motion=>p["u"],Heart=>4*x,Gas=>200+10*x,Voltage=>m%2==0?x:4*x,Cube=>k*k*k,Pythagoras=>4*k,Equivalence=>m is 1 or 6 or 7 or 9?1:0,_=>throw new InvalidOperationException()};
        return value.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>24)return false;
        string raw=string.Concat(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c is '−' or '–'?'-':c));
        if(!int.TryParse(raw,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int z)||z is <-1000 or >1000)return false;
        int a=p["a"],b=p["b"],x=p["x"],k=p["k"],m=p["m"],t=p["t"],u=p["u"],v=p["v"];
        return f switch{
            Property=>z is >=1 and <=9&&PropertyStatement(z,a,b)==PropertyStatement(m,a,b),
            Linear=>a*(z+b)==a*((m%2==0?x:-x)+b),
            Rational=>z+a*b==a*((m%2==0?x:-x)+b),
            Segment or Angle=>(a+1)*z+b==a*z+b+x,
            Temperature=>5*(z-32)==45*x,
            Motion=>(z+v)*t==(u+v)*t,
            Acceleration=>2*v*t+z*t*t==2*v*t+x*t*t,
            Heart=>3*(220-z)==4*(165-3*x),
            Gas=>a*821*z==a*821*(200+10*x),
            Voltage=>m%2==0?z*2*v==2*x*v:z*v==4*x*v,
            Cube=>z*a*a*a==(a*k)*(a*k)*(a*k),
            Pythagoras=>z>0&&z*z+9*k*k==25*k*k,
            // Equality of an image under a function is an equivalence; the remaining relations fail a defining property.
            Equivalence=>z==1&&(m==1||m==6||m==7||m==9)||z==0&&(m==2||m==3||m==4||m==5||m==8),
            _=>false
        };
    }
}
