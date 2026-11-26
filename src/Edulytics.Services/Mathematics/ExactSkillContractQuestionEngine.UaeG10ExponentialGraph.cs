using System.Globalization;
using System.Numerics;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade10 General G10-U05L01 from owner Applied 2022–2023 Volume2,
/// source printed pages294–300. Exponential growth/decay graph features.
/// Bounded integer exact-probes only; fractional table and real-world plotting
/// remain original authored material and teacher-assessed in Stage B.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10ExponentialGraph(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int b=random.Next(2,6),coeff=random.Next(1,5);
        int direction=random.Next(0,2); // 1 growth, 0 decay
        int h=random.Next(-2,3),k=random.Next(-4,5);
        int step=random.Next(0,3);
        // A small exact answer subset: decay a=coefficient*b^2 so
        // a*b^-n remains an integer for n<=2; growth a=coefficient.
        int a=direction==1?coeff:coeff*b*b;
        int probe=h+step;
        string factor=direction==1?$"{b}":"(1/"+b.ToString(CultureInfo.InvariantCulture)+")";
        string func=$"د(س)={a}×{factor}^(س−({h}))+({k})";
        string prompt=family switch
        {
            UaeG10ExponentialGraphResponse.Value=>
                $"للدالة الأسية {func}، أوجد د({probe}) واكتب القيمة الصحيحة بعد التعويض.",
            UaeG10ExponentialGraphResponse.Growth=>
                $"للدالة الأسية {func}، هل هي دالة نمو أسي عندما المعامل الموجب ثابت؟ أجب ١ للنمو أو ٠ للاضمحلال.",
            UaeG10ExponentialGraphResponse.Asymptote=>
                $"للدالة الأسية {func}، ما قيمة ص التي يقترب منها التمثيل البياني أفقيًا دون بلوغها؟",
            _=>$"للدالة الأسية {func}، أوجد د({h}) عند الإزاحة الأفقية هـ={h}، وهي قيمة بداية المضاعف قبل انتقال الخط البياني."
        };
        return Problem(family,prompt,
            "للصيغة د(س)=أ×ب^(س−هـ)+ك: حدد النمو من ب>١ أو الاضمحلال من ٠<ب<١، والمقارب الأفقي ص=ك، والتعويض يُحسب بأسس صحيحة وحدها مع مراعاة الإزاحة.",
            AssessmentItemType.Numeric,("base",b),("coeff",coeff),
            ("direction",direction),("h",h),("k",k),("step",step));
    }
}
internal static class UaeG10ExponentialGraphResponse
{
    internal const string Value="uae.g10.general.u05.l01.value_at_x";
    internal const string Growth="uae.g10.general.u05.l01.is_growth";
    internal const string Asymptote="uae.g10.general.u05.l01.horizontal_asymptote";
    internal const string ShiftAnchor="uae.g10.general.u05.l01.value_at_shift";
    internal static bool Supports(string? f) =>
        f is Value or Growth or Asymptote or ShiftAnchor;

    private static bool Expected(string family,IReadOnlyDictionary<string,int> p,out int value)
    {
        value=0;
        if(!Supports(family)||p.Count!=6||
            !p.ContainsKey("base")||!p.ContainsKey("coeff")||
            !p.ContainsKey("direction")||!p.ContainsKey("h")||
            !p.ContainsKey("k")||!p.ContainsKey("step"))return false;
        int b=p["base"],c=p["coeff"],dir=p["direction"],h=p["h"],k=p["k"],n=p["step"];
        if(b is < 2 or > 5||c is < 1 or > 4||dir is not (0 or 1)||
           h is < -2 or > 2||k is < -4 or > 4||n is < 0 or > 2)return false;
        int a=dir==1?c:c*b*b;
        int y=dir==1?a*IntPow(b,n)+k:a/IntPow(b,n)+k;
        value=family switch
        {
            Value=>y,
            Growth=>dir,
            Asymptote=>k,
            _=>a+k
        };
        return true;
    }
    private static int IntPow(int b,int n)
    {
        int r=1;for(int i=0;i<n;i++)r*=b;return r;
    }
    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Expected(family,p,out int value))
            throw new InvalidOperationException("Invalid bounded G10 exponential graph parameters.");
        return value.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Expected(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string raw=string.Concat(answer.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-',_=>ch
        }));
        return int.TryParse(raw,NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out int result)&&result==expected;
    }
}


// U05L02: extension of the shared exact lesson engine, not a new engine.
// Selected owner source PDF 150–155, extension 157. Stage D NOT RUN.
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10ExponentialEquation(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        const string hint="وحّد الأساس الموجب، وقارن الأسين مع عكس جهة المتباينة عند الحاجة؛ تحقق من الحد والاتجاه. في النمذجة استعمل عامل الفترة ولا تقرّب قبل النهاية.";
        if(family is UaeG10ExponentialEquationResponse.Equation or UaeG10ExponentialEquationResponse.Inequality)
        {
            int b=random.Next(2,6),denominator=random.Next(1,b),lp,rp,m,r,n=random.Next(-8,9),s=random.Next(-8,9);
            int nz(){int v;do v=random.Next(-8,9);while(v==0);return v;}
            do{lp=nz();rp=nz();m=nz();r=random.Next(-8,9);}while(lp*m==rp*r);
            int op=random.Next(0,4);
            string left=$"{UaeG10ExponentialEquationResponse.BaseText(b,denominator,lp)}^(({m})س+({n}))";
            string right=$"{UaeG10ExponentialEquationResponse.BaseText(b,denominator,rp)}^(({r})س+({s}))";
            string relation=family==UaeG10ExponentialEquationResponse.Equation?"=":UaeG10ExponentialEquationResponse.Relation(op);
            string instruction=family==UaeG10ExponentialEquationResponse.Equation
                ?"اكتب قيمة س على هيئة عدد صحيح أو كسر."
                :"اكتب الحل في صورة س ثم رمز المتباينة ثم الحد العددي، ويمكن أن يكون الحد كسرًا.";
            return Problem(family,$"حل {left} {relation} {right}. {instruction}",hint,
                AssessmentItemType.ShortAnswer,("base",b),("baseDen",denominator),("lp",lp),("rp",rp),("m",m),("r",r),("n",n),("s",s),("op",op));
        }
        if(family is UaeG10ExponentialEquationResponse.Factor or UaeG10ExponentialEquationResponse.Forecast or UaeG10ExponentialEquationResponse.Model)
        {
            int start=random.Next(100,1001),end;
            do end=random.Next(50,1501);while(end==start);
            int observed=random.Next(2,7),target=random.Next(observed+1,observed+5);
            string prompt=family==UaeG10ExponentialEquationResponse.Model
                ?$"كمية تتغير أسيًا من {start} عند ز=0 إلى {end} عند ز={observed}. اكتب معاملي النموذج د(ز)=أ×ب^ز، في زوج مرتب (أ،ب)، قرّب ب إلى أقرب جزء من ألف."
                :family==UaeG10ExponentialEquationResponse.Factor
                ?$"كمية تتغير أسيًا من {start} إلى {end} خلال {observed} فترات. أوجد عامل التغير لكل فترة مقربًا إلى أقرب جزء من ألف."
                :$"كمية تتغير أسيًا من {start} إلى {end} خلال {observed} فترات. توقع الكمية بعد {target} فترات من البداية، إلى أقرب عدد صحيح. لا تقرب عامل التغير مبكرًا.";
            return Problem(family,prompt,hint,family is UaeG10ExponentialEquationResponse.Factor or UaeG10ExponentialEquationResponse.Model?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
                ("start",start),("end",end),("observed",observed),("target",target));
        }
        if(family==UaeG10ExponentialEquationResponse.Compound)
        {
            int initial=100*random.Next(1,251),rate=random.Next(100,1401),years=random.Next(1,21);
            int[] frequencies=[1,2,4,6,12,24,26,52];int frequency=frequencies[random.Next(frequencies.Length)];
            string rateText=(rate/100m).ToString("0.00",CultureInfo.InvariantCulture);
            return Problem(family,$"أودع {initial} درهمًا بمعدل سنوي اسمي {rateText}٪ يركب {frequency} مرة في السنة. احسب الرصيد بعد {years} سنة إلى أقرب جزء من مائة درهم.",hint,
                AssessmentItemType.ShortAnswer,("initial",initial),("rate",rate),("frequency",frequency),("years",years));
        }
        if(family==UaeG10ExponentialEquationResponse.Period)
        {
            int percent=random.Next(1,6);int[] choices=[2,4,12];int periods=choices[random.Next(choices.Length)];
            decimal annual=1;for(int i=0;i<periods;i++)annual*=1+percent/100m;
            return Problem(family,$"عامل النمو السنوي الفعلي {annual.ToString(CultureInfo.InvariantCulture)}. أوجد عامل الفترة المكافئ عندما تقسم السنة إلى {periods} فترات متساوية، واكتبه كسرًا أو عددًا عشريًا.",hint,
                AssessmentItemType.ShortAnswer,("percent",percent),("periods",periods));
        }
        if(family is UaeG10ExponentialEquationResponse.Cooling or UaeG10ExponentialEquationResponse.Threshold)
        {
            int ambient=random.Next(16,26),delta=16*random.Next(2,6),period=random.Next(5,13),step=random.Next(1,5);
            string prompt=family==UaeG10ExponentialEquationResponse.Cooling
                ?$"درجة الحرارة د(ز)={ambient}+{delta}×(1/2)^(ز/{period})، والزمن بالدقائق. أوجد د({period*step})."
                :$"درجة الحرارة د(ز)={ambient}+{delta}×(1/2)^(ز/{period}) للزمن ز≥0. ما أول زمن بالدقائق تصبح فيه الحرارة {ambient+delta/(1<<step)} أو أقل؟";
            return Problem(family,prompt,hint,AssessmentItemType.Numeric,
                ("ambient",ambient),("delta",delta),("period",period),("step",step));
        }
        if(family is UaeG10ExponentialEquationResponse.PowerLaw or UaeG10ExponentialEquationResponse.PowerCoefficient)
        {
            int den=random.Next(2,4),num,a=random.Next(2,7),b,coeff=random.Next(1,7);
            do num=random.Next(1,5);while(num==den);
            do b=random.Next(2,7);while(b==a);
            string prompt=family==UaeG10ExponentialEquationResponse.PowerCoefficient
                ?$"كمية ك تتناسب طرديًا مع م^({num}/{den})؛ ك={coeff*UaeG10ExponentialEquationResponse.Pow(a,num)} عند م={UaeG10ExponentialEquationResponse.Pow(a,den)}. أوجد ثابت التناسب ث في ك=ث×م^({num}/{den})."
                :$"كمية ك تتناسب طرديًا مع م^({num}/{den}). إذا كانت ك={coeff*UaeG10ExponentialEquationResponse.Pow(a,num)} عند م={UaeG10ExponentialEquationResponse.Pow(a,den)}، أوجد ك عند م={UaeG10ExponentialEquationResponse.Pow(b,den)}.";
            return Problem(family,prompt,hint,
                AssessmentItemType.Numeric,("den",den),("num",num),("a",a),("b",b),("coeff",coeff));
        }
        if(family is UaeG10ExponentialEquationResponse.RoundedCooling or UaeG10ExponentialEquationResponse.CoolingProbe)
        {
            int ambient=random.Next(15,26),delta=random.Next(40,81),rate=random.Next(20,101),time=random.Next(5,31),threshold=ambient+random.Next(10,delta);
            string baseText=(1+rate/1000m).ToString("0.000",CultureInfo.InvariantCulture);
            string model=$"د(ز)={ambient}+{delta}×({baseText})^(−ز)";
            string prompt=family==UaeG10ExponentialEquationResponse.RoundedCooling
                ?$"درجة حرارة سائل وفق {model}، والزمن بالدقائق. أوجد درجة الحرارة بعد {time} دقيقة إلى أقرب عُشر."
                :$"درجة حرارة سائل وفق {model}، والزمن بالدقائق. هل د({time})≤{threshold}؟ اكتب 1 إذا نعم أو 0 إذا لا.";
            return Problem(family,prompt,hint,family==UaeG10ExponentialEquationResponse.RoundedCooling?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
                ("ambient",ambient),("delta",delta),("rate",rate),("time",time),("threshold",threshold));
        }
        if(family is UaeG10ExponentialEquationResponse.RoundedPowerCoefficient or UaeG10ExponentialEquationResponse.RoundedPowerPrediction)
        {
            (int num,int den)[] powers=[(131,100),(3,2),(3,4),(13,10),(2,3)];
            var (num,den)=powers[random.Next(powers.Length)];
            int m0=random.Next(10,61),initial=random.Next(50,201),m1=random.Next(20,151),convert=random.Next(0,2);
            string question=family==UaeG10ExponentialEquationResponse.RoundedPowerCoefficient
                ?"أوجد ثابت التناسب ث في ك=ث×م^("+num+"/"+den+") إلى أقرب جزء من مائة."
                :convert==1?$"أوجد ك عند كتلة {m1} رطلًا، علمًا بأن الرطل =0.454 كغ في هذه المسألة. قرّب إلى أقرب جزء من مائة."
                :$"أوجد ك عند م={m1} كغ إلى أقرب جزء من مائة.";
            return Problem(family,$"تتناسب كمية ك طرديًا مع م^({num}/{den})، حيث م بالكيلوغرام. إذا كانت ك={initial} عند م={m0} كغ، {question}",hint,AssessmentItemType.ShortAnswer,
                ("num",num),("den",den),("m0",m0),("initial",initial),("m1",m1),("convert",convert));
        }
        if(family==UaeG10ExponentialEquationResponse.RoundedPeriod)
        {
            int[] frequencies=[2,4,12];int periods=frequencies[random.Next(frequencies.Length)],direction=random.Next(0,2);
            int rate=direction==0?random.Next(100,801):random.Next(10,81);
            string factor=(1+rate/10000m).ToString("0.0000",CultureInfo.InvariantCulture);
            string prompt=direction==0
                ?$"عامل النمو السنوي الفعلي {factor}. أوجد عامل الفترة المكافئ لسنة مقسمة إلى {periods} فترات متساوية، إلى أقرب جزء من عشرة آلاف."
                :$"عامل النمو لكل فترة {factor}، وعدد الفترات في السنة {periods}. أوجد عامل النمو السنوي الفعلي إلى أقرب جزء من عشرة آلاف.";
            return Problem(family,prompt,hint,AssessmentItemType.ShortAnswer,("periods",periods),("direction",direction),("rate",rate));
        }
        int baseNumber=random.Next(2,6),exponent=random.Next(2,7),multiplicity=random.Next(1,3);
        return Problem(family,$"جمعنا {UaeG10ExponentialEquationResponse.Pow(baseNumber,multiplicity)} حدًا متطابقًا، قيمة كل منها {baseNumber}^{exponent}. إذا كان المجموع {baseNumber}^س فأوجد س.",hint,
            AssessmentItemType.Numeric,("base",baseNumber),("exponent",exponent),("multiplicity",multiplicity));
    }
}

internal static class UaeG10ExponentialEquationResponse
{
    internal const string Equation="uae.g10.general.u05.l02.equation_rational";
    internal const string Inequality="uae.g10.general.u05.l02.inequality_rational_boundary";
    internal const string Factor="uae.g10.general.u05.l02.model_factor_rounded";
    internal const string Model="uae.g10.general.u05.l02.model_parameters";
    internal const string PowerCoefficient="uae.g10.general.u05.l02.power_law_coefficient";
    internal const string RoundedCooling="uae.g10.general.u05.l02.cooling_rounded";
    internal const string CoolingProbe="uae.g10.general.u05.l02.cooling_threshold_probe";
    internal const string RoundedPowerCoefficient="uae.g10.general.u05.l02.power_law_rounded_coefficient";
    internal const string RoundedPowerPrediction="uae.g10.general.u05.l02.power_law_rounded_prediction";
    internal const string RoundedPeriod="uae.g10.general.u05.l02.equivalent_period_rounded";
    internal const string Forecast="uae.g10.general.u05.l02.model_forecast_rounded";
    internal const string Compound="uae.g10.general.u05.l02.compound_balance";
    internal const string Period="uae.g10.general.u05.l02.equivalent_period_factor";
    internal const string Cooling="uae.g10.general.u05.l02.cooling_value";
    internal const string Threshold="uae.g10.general.u05.l02.cooling_threshold";
    internal const string PowerLaw="uae.g10.general.u05.l02.power_law_prediction";
    internal const string Sum="uae.g10.general.u05.l02.like_power_sum";
    internal static bool Supports(string? f)=>f is Equation or Inequality or Factor or Forecast or Model or Compound or Period or Cooling or Threshold or PowerLaw or PowerCoefficient or Sum or RoundedCooling or CoolingProbe or RoundedPowerCoefficient or RoundedPowerPrediction or RoundedPeriod;
    internal static string Relation(int op)=>op switch{0=>">",1=>"≥",2=>"<",3=>"≤",_=>throw new InvalidOperationException()};
    internal static int Pow(int b,int n){int result=1;for(int i=0;i<n;i++)result*=b;return result;}
    internal static string BaseText(int b,int denominator,int n)
    {
        int a=Pow(n>0?b:denominator,Math.Abs(n)),d=Pow(n>0?denominator:b,Math.Abs(n));
        string f=Fraction(a,d);
        return f.Contains('/')?"("+f+")":f;
    }
    private static bool Keys(IReadOnlyDictionary<string,int> p,params string[] keys)=>p.Count==keys.Length&&keys.All(p.ContainsKey);
    private static bool In(int value,int lo,int hi)=>value>=lo&&value<=hi;
    private static bool NonZero(int value)=>In(value,-8,8)&&value!=0;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)
    {
        if(f is Equation or Inequality)return
            (Keys(p,"base","lp","rp","m","r","n","s","op") ||
            (Keys(p,"base","baseDen","lp","rp","m","r","n","s","op")&&In(p["baseDen"],1,p["base"]-1)))&&
            In(p["base"],2,5)&&NonZero(p["lp"])&&NonZero(p["rp"])&&NonZero(p["m"])&&In(p["r"],-8,8)&&
            In(p["n"],-8,8)&&In(p["s"],-8,8)&&In(p["op"],0,3)&&p["lp"]*p["m"]!=p["rp"]*p["r"];
        if(f is Factor or Forecast or Model)return Keys(p,"start","end","observed","target")&&In(p["start"],100,1000)&&
            In(p["end"],50,1500)&&p["start"]!=p["end"]&&In(p["observed"],2,6)&&In(p["target"],p["observed"]+1,p["observed"]+4);
        if(f==Compound)return Keys(p,"initial","rate","frequency","years")&&In(p["initial"],100,25000)&&p["initial"]%100==0&&
            In(p["rate"],100,1400)&&(p["frequency"] is 1 or 2 or 4 or 6 or 12 or 24 or 26 or 52)&&In(p["years"],1,20);
        if(f==Period)return Keys(p,"percent","periods")&&In(p["percent"],1,5)&&(p["periods"] is 2 or 4 or 12);
        if(f is Cooling or Threshold)return Keys(p,"ambient","delta","period","step")&&In(p["ambient"],16,25)&&In(p["delta"],32,80)&&
            p["delta"]%16==0&&In(p["period"],5,12)&&In(p["step"],1,4);
        if(f is PowerLaw or PowerCoefficient)return Keys(p,"den","num","a","b","coeff")&&In(p["den"],2,3)&&In(p["num"],1,4)&&p["num"]!=p["den"]&&
            In(p["a"],2,6)&&In(p["b"],2,6)&&p["a"]!=p["b"]&&In(p["coeff"],1,6);
        if(f is RoundedCooling or CoolingProbe)return Keys(p,"ambient","delta","rate","time","threshold")&&In(p["ambient"],15,25)&&In(p["delta"],40,80)&&In(p["rate"],20,100)&&In(p["time"],5,30)&&In(p["threshold"],p["ambient"]+10,p["ambient"]+p["delta"]-1);
        if(f is RoundedPowerCoefficient or RoundedPowerPrediction)return Keys(p,"num","den","m0","initial","m1","convert")&&
            ((p["num"],p["den"]) is (131,100) or (3,2) or (3,4) or (13,10) or (2,3))&&
            In(p["m0"],10,60)&&In(p["initial"],50,200)&&In(p["m1"],20,150)&&In(p["convert"],0,1);
        if(f==RoundedPeriod)return Keys(p,"periods","direction","rate")&&(p["periods"] is 2 or 4 or 12)&&
            In(p["direction"],0,1)&&(p["direction"]==0?In(p["rate"],100,800):In(p["rate"],10,80));
        return f==Sum&&Keys(p,"base","exponent","multiplicity")&&In(p["base"],2,5)&&In(p["exponent"],2,6)&&In(p["multiplicity"],1,2);
    }
    private static long Gcd(long a,long b){a=Math.Abs(a);b=Math.Abs(b);while(b!=0)(a,b)=(b,a%b);return a;}
    private static string Fraction(long n,long d)
    {
        if(d<0){n=-n;d=-d;}long g=Gcd(n,d);n/=g;d/=g;
        return d==1?n.ToString(CultureInfo.InvariantCulture):n.ToString(CultureInfo.InvariantCulture)+"/"+d.ToString(CultureInfo.InvariantCulture);
    }
    private static string Normalize(string answer)=>string.Concat(answer.Trim().Where(c=>!char.IsWhiteSpace(c)).Select(c=>c switch
    {
        >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
        >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
        '−' or '–'=>'-', '⁄'=>'/', '٫'=>'.','،'=>',','س'=>'x','X'=>'x',_=>c
    })).Replace(">=","≥",StringComparison.Ordinal).Replace("<=","≤",StringComparison.Ordinal);
    private static bool ParseNumber(string s,out long n,out long d)
    {
        n=0;d=1;
        if(s.Length==0||s.Length>32)return false;
        string[] fraction=s.Split('/');
        if(fraction.Length==2)
            return long.TryParse(fraction[0],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out n)&&
                long.TryParse(fraction[1],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out d)&&
                n>=-1000000000&&n<=1000000000&&d>0&&d<=1000000000;
        if(fraction.Length!=1)return false;
        if(long.TryParse(s,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out n))
            return n>=-1000000000&&n<=1000000000;
        if(!decimal.TryParse(s,NumberStyles.AllowLeadingSign|NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal value)||
            value<-1000000||value>1000000)return false;
        decimal scaled=value;
        while(scaled!=decimal.Truncate(scaled)&&d<1000000){scaled*=10;d*=10;}
        if(scaled!=decimal.Truncate(scaled)||scaled<-1000000000||scaled>1000000000)return false;
        n=(long)scaled;return true;
    }
    private static bool Holds(long left,long right,int op)=>op switch{0=>left>right,1=>left>=right,2=>left<right,3=>left<=right,_=>false};
    private static (BigInteger Numerator,BigInteger Denominator) CompoundRatio(IReadOnlyDictionary<string,int> p)
    {
        BigInteger denom=10000*p["frequency"],num=denom+p["rate"];
        int periods=p["frequency"]*p["years"];
        return (p["initial"]*BigInteger.Pow(num,periods),BigInteger.Pow(denom,periods));
    }
    private static long CompoundCents(IReadOnlyDictionary<string,int> p)
    {
        var (n,d)=CompoundRatio(p);
        return (long)((200*n+d)/(2*d)); // positive, nearest cent, ties up
    }
    private static bool FactorBucket(long k,IReadOnlyDictionary<string,int> p)
    {
        if(k<=0||k>1000000000)return false;
        int n=p["observed"];
        BigInteger scaled=BigInteger.Pow(2000,n)*p["end"];
        return BigInteger.Pow(2*(BigInteger)k-1,n)*p["start"]<=scaled&&
            scaled<BigInteger.Pow(2*(BigInteger)k+1,n)*p["start"];
    }
    private static bool ForecastBucket(long k,IReadOnlyDictionary<string,int> p)
    {
        if(k<0||k>1000000000)return false;
        int n=p["observed"],t=p["target"];
        BigInteger leftFactor=BigInteger.Pow(p["start"],t-n),right=BigInteger.Pow(p["end"],t)*BigInteger.Pow(2,n);
        if(k==0)return right<leftFactor; // a positive forecast below 0.5 rounds to zero
        return BigInteger.Pow(2*(BigInteger)k-1,n)*leftFactor<=right&&
            right<BigInteger.Pow(2*(BigInteger)k+1,n)*leftFactor;
    }
    // Positive root comparison without floating-point tolerance: y=(n/d)^(1/root).
    // For nearest 1/scale, the allowed bucket is [k−1/2,k+1/2) / scale.
    private static bool RootBucket(BigInteger n,BigInteger d,int root,int scale,long k)
    {
        if(n<=0||d<=0||root<=0||scale<=0||k<0||k>1000000000)return false;
        BigInteger scaled=n*BigInteger.Pow(2*scale,root);
        if(k==0)return scaled<d;
        return d*BigInteger.Pow(2*(BigInteger)k-1,root)<=scaled&&
            scaled<d*BigInteger.Pow(2*(BigInteger)k+1,root);
    }
    private static (BigInteger N,BigInteger D,int Root,int Scale,double Approximate) RoundedSource(string f,IReadOnlyDictionary<string,int> p)
    {
        if(f is RoundedCooling or CoolingProbe)
        {
            BigInteger d=BigInteger.Pow(1000+p["rate"],p["time"]);
            BigInteger n=p["ambient"]*d+p["delta"]*BigInteger.Pow(1000,p["time"]);
            return (n,d,1,10,p["ambient"]+p["delta"]*Math.Pow(1000d/(1000+p["rate"]),p["time"]));
        }
        if(f is RoundedPowerCoefficient or RoundedPowerPrediction)
        {
            int num=p["num"],den=p["den"];
            int mn=f==RoundedPowerCoefficient?1:p["convert"]==1?p["m1"]*454:p["m1"];
            int md=f==RoundedPowerPrediction&&p["convert"]==1?1000:1;
            BigInteger n=BigInteger.Pow(p["initial"],den)*BigInteger.Pow(mn,num);
            BigInteger d=BigInteger.Pow(md*p["m0"],num);
            return (n,d,den,100,p["initial"]*Math.Pow((double)mn/(md*p["m0"]),(double)num/den));
        }
        if(f==RoundedPeriod)
        {
            int rate=p["rate"],periods=p["periods"];
            if(p["direction"]==0)return (10000+rate,10000,periods,10000,Math.Pow(1+rate/10000d,1d/periods));
            return (BigInteger.Pow(10000+rate,periods),BigInteger.Pow(10000,periods),1,10000,Math.Pow(1+rate/10000d,periods));
        }
        throw new InvalidOperationException("Unsupported rounded source family.");
    }
    private static long RoundedCandidate(double approximate,Func<long,bool> bucket)
    {
        long k=(long)Math.Round(approximate,MidpointRounding.AwayFromZero);
        if(bucket(k))return k;
        if(bucket(k-1))return k-1;
        if(bucket(k+1))return k+1;
        throw new InvalidOperationException("Rounded candidate outside exact rounding bucket.");
    }
    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid Grade10 exponential equation/model parameters.");
        if(family is Equation or Inequality)
        {
            long n=p["rp"]*p["s"]-p["lp"]*p["n"],d=p["lp"]*p["m"]-p["rp"]*p["r"];
            string boundary=Fraction(n,d);
            return family==Equation?boundary:"x"+Relation(d>0?p["op"]:p["op"]^2)+boundary;
        }
        if(family is Factor or Model)
        {
            string b=(RoundedCandidate(1000*Math.Pow((double)p["end"]/p["start"],1d/p["observed"]),k=>FactorBucket(k,p))/1000m).ToString("0.000",CultureInfo.InvariantCulture);
            return family==Model?$"({p["start"]},{b})":b;
        }
        if(family==Forecast)return RoundedCandidate(p["start"]*Math.Pow((double)p["end"]/p["start"],(double)p["target"]/p["observed"]),k=>ForecastBucket(k,p)).ToString(CultureInfo.InvariantCulture);
        if(family==Compound)return (CompoundCents(p)/100m).ToString("0.00",CultureInfo.InvariantCulture);
        if(family==Period)return Fraction(100+p["percent"],100);
        if(family is RoundedCooling or CoolingProbe or RoundedPowerCoefficient or RoundedPowerPrediction or RoundedPeriod)
        {
            var (n,d,root,scale,approx)=RoundedSource(family,p);
            if(family==CoolingProbe)return n<=p["threshold"]*d?"1":"0";
            long k=RoundedCandidate(approx*scale,v=>RootBucket(n,d,root,scale,v));
            return (k/(decimal)scale).ToString(scale==10?"0.0":scale==100?"0.00":"0.0000",CultureInfo.InvariantCulture);
        }
        long value=family switch{Cooling=>p["ambient"]+p["delta"]/(1<<p["step"]),Threshold=>p["period"]*p["step"],
            PowerLaw=>p["coeff"]*Pow(p["b"],p["num"]),PowerCoefficient=>p["coeff"],Sum=>p["exponent"]+p["multiplicity"],_=>throw new InvalidOperationException()};
        return value.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>64)return false;
        string raw=Normalize(answer);int relation=-1;
        if(family==Model)
        {
            if(raw.Length<5||raw[0]!='('||raw[^1]!=')')return false;
            string[] pair=raw[1..^1].Split(',');
            return pair.Length==2&&ParseNumber(pair[0],out long an,out long ad)&&an==(long)p["start"]*ad&&
                ParseNumber(pair[1],out long bn,out long bd)&&bn*1000%bd==0&&FactorBucket(bn*1000/bd,p);
        }
        if(family==Equation&&raw.StartsWith("x=",StringComparison.Ordinal))raw=raw[2..];
        if(family==Inequality)
        {
            // Accept the stated inequality form, with Arabic or Latin variable.
            if(raw.Length<3||raw[0]!='x')return false;
            relation=raw[1] switch{'>'=>0,'≥'=>1,'<'=>2,'≤'=>3,_=>-1};
            if(relation<0)return false;raw=raw[2..];
        }
        if(!ParseNumber(raw,out long n,out long d))return false;
        if(family is Equation or Inequality)
        {
            // Substitute the proposed boundary into both original exponents.
            long Left(long x)=>p["lp"]*(p["m"]*x+p["n"]*d);
            long Right(long x)=>p["rp"]*(p["r"]*x+p["s"]*d);
            if(Left(n)!=Right(n))return false;
            if(family==Equation)return true;
            // Directly verify strictness and both sides of the claimed boundary.
            foreach(long probe in new[]{n-d,n,n+d})
                if(Holds(Left(probe),Right(probe),p["op"])!=Holds(probe,n,relation))return false;
            return true;
        }
        if(family is RoundedCooling or CoolingProbe or RoundedPowerCoefficient or RoundedPowerPrediction or RoundedPeriod)
        {
            var (rn,rd,root,scale,_)=RoundedSource(family,p);
            if(family==CoolingProbe)return n==(rn<=p["threshold"]*rd?1L:0L)*d;
            return n>=0&&n*scale%d==0&&RootBucket(rn,rd,root,scale,n*scale/d);
        }
        if(family==Factor)return n*1000%d==0&&FactorBucket(n*1000/d,p);
        if(family==Forecast)return n%d==0&&ForecastBucket(n/d,p);
        if(family==Compound)
        {
            if(n<0||n*100%d!=0)return false;
            BigInteger k=n*100/d;var (num,den)=CompoundRatio(p);
            return (2*k-1)*den<=200*num&&200*num<(2*k+1)*den;
        }
        if(family==Period)
        {
            int periods=p["periods"];
            return n>0&&BigInteger.Pow(n,periods)*BigInteger.Pow(100,periods)==
                BigInteger.Pow(d,periods)*BigInteger.Pow(100+p["percent"],periods);
        }
        if(n%d!=0)return false;
        long value=n/d;
        if(family==Cooling)return (value-p["ambient"])*(1<<p["step"])==p["delta"];
        if(family==Threshold)return value>=0&&value%p["period"]==0&&value/p["period"]==p["step"];
        if(family is PowerLaw or PowerCoefficient)
        {
            if(value<=0||value>1000000)return false;
            if(family==PowerCoefficient)
                return value*Pow(p["a"],p["num"])==(long)p["coeff"]*Pow(p["a"],p["num"]);
            int den=p["den"],num=p["num"];
            int initial=p["coeff"]*Pow(p["a"],num),m0=Pow(p["a"],den),m1=Pow(p["b"],den);
            return BigInteger.Pow(value,den)*BigInteger.Pow(m0,num)==BigInteger.Pow(initial,den)*BigInteger.Pow(m1,num);
        }
        return family==Sum&&value>=0&&value<=20&&BigInteger.Pow(p["base"],(int)value)==
            BigInteger.Pow(p["base"],p["multiplicity"])*BigInteger.Pow(p["base"],p["exponent"]);
    }
}
