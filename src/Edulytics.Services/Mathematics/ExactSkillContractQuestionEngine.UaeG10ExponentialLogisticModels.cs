using System.Globalization;
using System.Numerics;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10ExponentialLogisticModels(string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int a=random.Next(2,9),b=random.Next(2,9),c=random.Next(12,51)*100,h=random.Next(2,13)*100,t=random.Next(2,13),v=random.Next(2,9),r=random.Next(2,9),q=random.Next(1,r),n=random.Next(1,31),rate=random.Next(250,901),p=random.Next(5,251)*100,mode=random.Next(4);
        string R(int x)=>((decimal)x/100).ToString(CultureInfo.InvariantCulture);
        string ratePercent=((decimal)rate/100).ToString(CultureInfo.InvariantCulture),round="قرّب إلى أربعة أجزاء عشرية.";
        string model=$"P(t)={c}/(1+{a}e^(-{R(r)}t))";
        string target=mode switch{0=>$"{c}/({a}+2)",1=>$"{c}",2=>$"{c}×{v}/({v}+1)",_=>$"{c}/({a}+1)"};
        string prompt=family switch{
            UaeG10ExponentialLogisticResponse.HalfRate=>$"عمر نصف مادة افتراضية {h} سنة. أوجد k في y=ae^(-kt) بوحدة سنة⁻¹ إلى ثماني منازل عشرية.",
            UaeG10ExponentialLogisticResponse.DecayAmount=>$"عمر النصف {h} سنة وكتلة البداية {p} mg. أوجد المتبقي بعد {100*t} سنة. {round}",
            UaeG10ExponentialLogisticResponse.DecayAge=>$"عمر نصف مادة افتراضية {h} سنة. تبقى {10*v}% من الكمية الأصلية. أوجد العمر بالسنوات. {round}",
            UaeG10ExponentialLogisticResponse.HalfFromLoss=>$"اضمحل {100-10*v}% من مادة خلال {100*t} سنة. أوجد عمر النصف بالسنوات. {round}",
            UaeG10ExponentialLogisticResponse.GrowthRate=>$"في نموذج y=y₀e^(kt)، ارتفع العدد من {100*a} إلى {100*a+10*b} في {t} سنوات. أوجد k إلى ست منازل عشرية.",
            UaeG10ExponentialLogisticResponse.GrowthForecast=>$"ارتفع العدد من {100*a} إلى {100*a+10*b} خلال {t} سنوات في نموذج أسي مستمر. احسب العدد عند {2*t+n} سنة من البداية. {round}",
            UaeG10ExponentialLogisticResponse.GrowthTarget=>$"ارتفع العدد من {100*a} إلى {100*a+10*b} خلال {t} سنوات في نموذج أسي مستمر. كم سنة من البداية حتى يصل إلى {2*(100*a+10*b)}؟ {round}",
            UaeG10ExponentialLogisticResponse.Crossing=>$"A(t)={100*a}e^({R(r)}t)، B(t)={100*a+10*b}e^({R(q)}t). أوجد زمن التقاطع الذي بعده A>B، بالسنوات. {round}",
            UaeG10ExponentialLogisticResponse.LogisticInitial=>$"في النموذج اللوجستي {model} أوجد P(0). {round}",
            UaeG10ExponentialLogisticResponse.LogisticLimit=>$"في النموذج اللوجستي {model} أوجد قيمة خط التقارب الأفقي.",
            UaeG10ExponentialLogisticResponse.LogisticValue=>$"في النموذج اللوجستي {model} أوجد P({t}). {round}",
            UaeG10ExponentialLogisticResponse.LogisticTime=>$"في النموذج اللوجستي {model} أوجد الزمن عندما P(t)={c}×{v}/({v}+1). {round}",
            UaeG10ExponentialLogisticResponse.LogisticRate=>$"في P(t)={c}/(1+{a}e^(-kt)) وجدنا P({t})={c}×{v}/({v}+1). أوجد k إلى ست منازل عشرية.",
            UaeG10ExponentialLogisticResponse.LogisticReachable=>$"في {model} والمجال t≥0، هل يمكن بلوغ P={target} في زمن منتهٍ؟ اكتب 1 لنعم أو 0 للا.",
            UaeG10ExponentialLogisticResponse.ModelRelation=>$"من النموذجين A(t)=({c}/({a}+1))e^({R(r)}t) وL(t)={c}/(1+{a}e^(-{R(r)}t))، أيهما أكبر عند t={t}>0؟ اكتب 1 للأسي أو 2 للوجستي أو 0 للتساوي.",
            UaeG10ExponentialLogisticResponse.LogisticCrossing=>$"قارن A(t)=({c}/({2*(a+1)}))e^({R(r)}t) وL(t)={c}/(1+{a}e^(-{R(q)}t)). أوجد زمن تقاطعهما للمجال t≥0 إلى أربعة أجزاء عشرية.",
            UaeG10ExponentialLogisticResponse.SpreadsheetBalance=>$"إيداع أو دين افتراضي بلا دفعات قدره {p} AED بمعدل {ratePercent}% مركب سنويًا مدة {n} سنوات. أوجد الرصيد بالدقة الكاملة ثم قرّب إلى فلس.",
            UaeG10ExponentialLogisticResponse.SpreadsheetInterest=>$"إيداع {p} AED بمعدل {ratePercent}% مركب سنويًا مدة {n} سنوات. أوجد الربح خلال السنة الأخيرة فقط، إلى أقرب فلس.",
            _=>throw new InvalidOperationException()
        };
        return Problem(family,prompt,"حدد النموذج ووحدة الزمن والنسبة المتبقية؛ اعزل المجهول باللوغاريتم وافحص الحد والمجال قبل التقريب.",AssessmentItemType.Numeric,
            ("a",a),("b",b),("c",c),("h",h),("t",t),("v",v),("r",r),("q",q),("n",n),("rate",rate),("p",p),("mode",mode));
    }
}
internal static class UaeG10ExponentialLogisticResponse
{
    internal const string Prefix="uae.g10.general.u05.l08.";
    internal const string HalfRate=Prefix+"half_life_constant",DecayAmount=Prefix+"half_life_amount",DecayAge=Prefix+"decay_age",HalfFromLoss=Prefix+"half_life_from_loss",GrowthRate=Prefix+"continuous_growth_rate",GrowthForecast=Prefix+"continuous_growth_forecast",GrowthTarget=Prefix+"continuous_growth_target",Crossing=Prefix+"continuous_crossing",LogisticInitial=Prefix+"logistic_initial",LogisticLimit=Prefix+"logistic_limit",LogisticValue=Prefix+"logistic_value",LogisticTime=Prefix+"logistic_time",LogisticRate=Prefix+"logistic_rate",LogisticReachable=Prefix+"logistic_reachable",ModelRelation=Prefix+"exponential_logistic_relation",SpreadsheetBalance=Prefix+"spreadsheet_balance",SpreadsheetInterest=Prefix+"spreadsheet_interest",LogisticCrossing=Prefix+"exponential_logistic_crossing";
    internal static bool Supports(string? f)=>f is HalfRate or DecayAmount or DecayAge or HalfFromLoss or GrowthRate or GrowthForecast or GrowthTarget or Crossing or LogisticInitial or LogisticLimit or LogisticValue or LogisticTime or LogisticRate or LogisticReachable or ModelRelation or SpreadsheetBalance or SpreadsheetInterest or LogisticCrossing;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> a)=>Supports(f)&&a.Count==12&&new[]{"a","b","c","h","t","v","r","q","n","rate","p","mode"}.All(a.ContainsKey)&&a["a"] is >=2 and <=8&&a["b"] is >=2 and <=8&&a["c"] is >=1200 and <=5000&&a["c"]%100==0&&a["h"] is >=200 and <=1200&&a["h"]%100==0&&a["t"] is >=2 and <=12&&a["v"] is >=2 and <=8&&a["r"] is >=2 and <=8&&a["q"]>=1&&a["q"]<a["r"]&&a["n"] is >=1 and <=30&&a["rate"] is >=250 and <=900&&a["p"] is >=500 and <=25000&&a["p"]%100==0&&a["mode"] is >=0 and <=3;
    private static int D(string f)=>f==HalfRate?8:f is GrowthRate or LogisticRate?6:f is SpreadsheetBalance or SpreadsheetInterest?2:f is LogisticLimit or LogisticReachable or ModelRelation?0:4;
    private static double V(string f,IReadOnlyDictionary<string,int> x)
    {
        double a=x["a"],b=x["b"],c=x["c"],h=x["h"],t=x["t"],v=x["v"],r=x["r"]/100.0,q=x["q"]/100.0,n=x["n"],p=x["p"],ratio=(100*a+10*b)/(100*a),k=Math.Log(ratio)/t;
        return f switch{HalfRate=>Math.Log(2)/h,DecayAmount=>p*Math.Pow(.5,100*t/h),DecayAge=>-h*Math.Log(v/10)/Math.Log(2),HalfFromLoss=>-100*t*Math.Log(2)/Math.Log(v/10),GrowthRate=>k,GrowthForecast=>100*a*Math.Exp(k*(2*t+n)),GrowthTarget=>Math.Log(2*ratio)/k,Crossing=>Math.Log(ratio)/(r-q),
            LogisticInitial=>c/(1+a),LogisticLimit=>c,LogisticValue=>c/(1+a*Math.Exp(-r*t)),LogisticTime=>Math.Log(a*v)/r,LogisticRate=>Math.Log(a*v)/t,LogisticReachable=>x["mode"]>=2?1:0,ModelRelation=>1,LogisticCrossing=>CrossTime(a,r,q),_=>throw new InvalidOperationException()};
    }
    // A/L=(exp(rt)+a exp((r-q)t))/(2(a+1)) is strictly increasing for r>q>0.
    // The ratio begins at 1/2, and tends to infinity. Bisection finds the unique crossing.
    private static double CrossTime(double a,double r,double q)
    {
        double lo=0,hi=Math.Log(2*(a+1))/r;
        for(int i=0;i<80;i++)
        {
            double mid=(lo+hi)/2;
            if(Math.Exp(r*mid)+a*Math.Exp((r-q)*mid)<2*(a+1))lo=mid;
            else hi=mid;
        }
        return (lo+hi)/2;
    }
    private static (BigInteger N,BigInteger D) Compound(string f,IReadOnlyDictionary<string,int> a)
    {
        int n=a["n"],p=a["p"],rate=a["rate"];BigInteger factor=10000+rate;
        // Promote the principal to BigInteger before multiplying by the basis-point rate.
        BigInteger cents=100*(BigInteger)p;
        return f==SpreadsheetBalance?(cents*BigInteger.Pow(factor,n),BigInteger.Pow(10000,n)):(cents*rate*BigInteger.Pow(factor,n-1),BigInteger.Pow(10000,n));
    }
    private static string R(double x,int d)=>Math.Round(x,d,MidpointRounding.AwayFromZero).ToString("F"+d,CultureInfo.InvariantCulture);
    private static string Norm(string answer)=>string.Concat(answer.Trim().Select(ch=>ch switch{>= '٠' and <= '٩'=>(char)('0'+ch-'٠'),>= '۰' and <= '۹'=>(char)('0'+ch-'۰'),'−' or '–'=>'-','٫'=>'.',_=>ch})).Replace(" ","");
    private static bool Num(string s,int d,out decimal x)
    {x=0;if(!decimal.TryParse(s,NumberStyles.AllowLeadingSign|NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out x)||x<0||x>100000000m)return false;decimal z=x*(decimal)Math.Pow(10,d);return z==decimal.Truncate(z);}
    internal static string Format(string f,IReadOnlyDictionary<string,int> a)
    {
        if(!Valid(f,a))throw new InvalidOperationException("Invalid bounded model parameters.");
        if(f is SpreadsheetBalance or SpreadsheetInterest){var z=Compound(f,a);BigInteger cents=(2*z.N+z.D)/(2*z.D);return ((decimal)cents/100).ToString("0.00",CultureInfo.InvariantCulture);}
        return R(V(f,a),D(f));
    }
    private static bool Bucket(string f,IReadOnlyDictionary<string,int> x,double y)
    {
        double a=x["a"],b=x["b"],c=x["c"],h=x["h"],t=x["t"],v=x["v"],r=x["r"]/100.0,q=x["q"]/100.0,n=x["n"],p=x["p"],ratio=(100*a+10*b)/(100*a);
        double F(double z)=>f switch{
            HalfRate=>Math.Exp(-z*h)-.5,DecayAmount=>z-p*Math.Exp(-100*t*Math.Log(2)/h),DecayAge=>Math.Exp(-z*Math.Log(2)/h)-v/10,
            HalfFromLoss=>z<=0?double.NegativeInfinity:Math.Exp(-100*t*Math.Log(2)/z)-v/10,GrowthRate=>t*z-Math.Log(ratio),
            GrowthForecast=>z<=0?double.NegativeInfinity:Math.Log(z/(100*a))-(2*t+n)*Math.Log(ratio)/t,
            GrowthTarget=>z*Math.Log(ratio)/t-Math.Log(2*ratio),Crossing=>(r-q)*z-Math.Log(ratio),
            LogisticInitial=>z*(1+a)-c,LogisticValue=>z*(1+a*Math.Exp(-r*t))-c,
            LogisticTime=>a*Math.Exp(-r*z)-1/v,LogisticRate=>a*Math.Exp(-z*t)-1/v,
            LogisticCrossing=>c*Math.Exp(r*z)/(2*(a+1))-c/(1+a*Math.Exp(-q*z)),_=>double.NaN};
        double e=.5/Math.Pow(10,D(f)),l=F(y-e),u=F(y+e);return (l<=0&&u>=0)||(l>=0&&u<=0);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> a,string? answer)
    {
        if(!Valid(f,a)||string.IsNullOrWhiteSpace(answer)||!Num(Norm(answer),D(f),out decimal x))return false;
        if(f is SpreadsheetBalance or SpreadsheetInterest){var z=Compound(f,a);BigInteger cents=(BigInteger)(x*100);return (2*cents-1)*z.D<=2*z.N&&2*z.N<(2*cents+1)*z.D;}
        if(f==LogisticLimit)return x==a["c"];
        if(f==LogisticReachable)
        {
            // Compare target with initial and limiting values as exact fractions of capacity.
            int num=a["mode"]==2?a["v"]:1,den=a["mode"] switch{0=>a["a"]+2,1=>1,2=>a["v"]+1,_=>a["a"]+1};
            return x==(num*(a["a"]+1)>=den&&num<den?1:0);
        }
        if(f==ModelRelation)return x==1&&Math.Exp(a["r"]*a["t"]/100.0)>1;
        return Bucket(f,a,(double)x);
    }
}
