using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10CommonLogarithm(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int[] bases=[2,3,5,7];
        int b=bases[random.Next(bases.Length)],c;do c=bases[random.Next(bases.Length)];while(c==b);
        int m=random.Next(1,4),p=random.Next(1,4),n=random.Next(-4,5),q=random.Next(-4,5);
        int k=random.Next(0,4),v=random.Next(2,81),op=random.Next(4),dir=random.Next(2);
        int h=random.Next(2,10),t=random.Next(2,7);
        // Keep the entire bounded domain away from floating-point ambiguity at the half-unit.
        // Numeric answers are explicitly rounded; teacher tasks preserve the exact symbolic boundary.
        var data=new Dictionary<string,int>{{"b",b},{"c",c},{"m",m},{"p",p},{"n",n},{"q",q},{"k",k},{"v",v},{"op",op},{"dir",dir},{"h",h},{"t",t}};
        string B=dir==0?$"{b}":$"(1/{b})",rel=UaeG10CommonLogarithmResponse.Relation(op),round="قرّب إلى أقرب جزء من عشرة آلاف.";
        string prompt=family switch{
            UaeG10CommonLogarithmResponse.Common=>$"احسب لوغ العشري({v}/10^{k}). {round}",
            UaeG10CommonLogarithmResponse.Change=>$"استعمل تغيير الأساس لحساب لوغ_{B}({v}/10^{k}). {round}",
            UaeG10CommonLogarithmResponse.Equation=>$"حل {B}^({m}س+({n}))={v}. {round}",
            UaeG10CommonLogarithmResponse.TwoBases=>$"حل {B}^({m}س+({n}))={c}^({p}س+({q})). {round}",
            UaeG10CommonLogarithmResponse.Inequality=>$"حل {B}^({m}س+({n})) {rel} {(k==0?v.ToString(CultureInfo.InvariantCulture):$"{c}^({p}س+({q}))")}. اكتب فترة الحل بالحد التقريبي لأربع منازل؛ مثال (-∞,2.5000]، أو (2.5000,∞).",
            UaeG10CommonLogarithmResponse.Quadratic=>$"حل {b}^(س²)={v}. اكتب الجذرين المرتبين بين قوسين (السالب، الموجب)، كل منهما إلى أربع منازل.",
            UaeG10CommonLogarithmResponse.Sound=>$"في L=10log(I/I₀)، إذا L={50+h} ديسيبل وI₀=10^(-12) W/m²، احسب I بوحدة µW/m² (1 µW=10^(-6) W). {round}",
            UaeG10CommonLogarithmResponse.SoundReduction=>$"انخفض مستوى الصوت {h} ديسيبل. احسب النقص المئوي في الشدة اعتمادًا على I₂/I₁=10^(-{h}/10). {round}",
            UaeG10CommonLogarithmResponse.Energy=>$"في النموذج log E=11.8+1.5M، إذا M={(3+h/10m).ToString(CultureInfo.InvariantCulture)}، احسب E مقسومًا على 10^16 بوحدة النموذج. {round}",
            UaeG10CommonLogarithmResponse.Pet=>$"في النموذج ن={10*b}لوغ_{c}(ز+2) بالألف، احسب ن عند ز={v} سنة. {round}",
            UaeG10CommonLogarithmResponse.PetTime=>$"في النموذج ن={10*b}لوغ_{c}(ز+2) بالألف، عند أي زمن يصل ن إلى {(10*b*h/2m).ToString(CultureInfo.InvariantCulture)} ألفًا؟ {round}",
            UaeG10CommonLogarithmResponse.GrowthRate=>$"زاد عدد من {100*b} إلى {100*b+10*v} خلال {t} سنوات بمعدل ثابت. احسب المعدل السنوي المئوي. {round}",
            UaeG10CommonLogarithmResponse.GrowthTime=>$"زاد عدد من {100*b} إلى {100*b+10*v} خلال {t} سنوات بمعدل ثابت. كم سنة من المشاهدة الأخيرة حتى يبلغ {200*b+20*v}؟ {round}",
            UaeG10CommonLogarithmResponse.Cents=>$"احسب الفترة بالسنت عند الانتقال من {100*b} Hz إلى {100*b+v} Hz باستعمال C=1200log₂(f₂/f₁). {round}",
            UaeG10CommonLogarithmResponse.Frequency=>$"إذا كان التردد الابتدائي {100*b} Hz والفترة {(dir==0?h*25:-h*25)} سنت، احسب التردد النهائي باستعمال C=1200log₂(f₂/f₁). {round}",
            UaeG10CommonLogarithmResponse.Ph=>$"احسب pH=-log([H⁺]) عندما [H⁺]={v}×10^(-{h+3}) mol/L. {round}",
            UaeG10CommonLogarithmResponse.Ppm=>$"عينة رياضية كتلتها {t} kg تحتوي {(v/100m).ToString(CultureInfo.InvariantCulture)} mg من مادة. احسب التركيز mg/kg (ppm). {round}",
            UaeG10CommonLogarithmResponse.LabEq=>$"حل بيانيًا ثم جبريًا: log_{b}({b}(س+{h}))=log_{c}({c}(س+{h})). اكتب إحداثي س للتقاطع. {round}",
            UaeG10CommonLogarithmResponse.LabIneq=>$"حدّد مجال حل log_{b}({b}(س+{h})) < log_{c}({c}(س+{h})). اكتب الفترة، مع استبعاد نقاط عدم التعريف والمساواة.",
            _=>throw new InvalidOperationException("Unknown common log family.")
        };
        bool structured=family is UaeG10CommonLogarithmResponse.Inequality or UaeG10CommonLogarithmResponse.Quadratic or UaeG10CommonLogarithmResponse.LabIneq;
        return Problem(family,prompt,"تحقق من المجال وخذ لوغاريتم الطرفين أو غيّر الأساس. لا تقرب إلا في الخطوة الأخيرة؛ اختبر اتجاه المتباينة.",
            structured?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
            ("b",b),("c",c),("m",m),("p",p),("n",n),("q",q),("k",k),("v",v),("op",op),("dir",dir),("h",h),("t",t));
    }
}
internal static class UaeG10CommonLogarithmResponse
{
    internal const string Prefix="uae.g10.general.u05.l06.";
    internal const string Common=Prefix+"common_log_rounded",Change=Prefix+"change_base_rounded",Equation=Prefix+"exponential_equation",TwoBases=Prefix+"exponential_two_bases",Inequality=Prefix+"exponential_inequality",Quadratic=Prefix+"exponential_quadratic_roots",Sound=Prefix+"sound_intensity_micro",SoundReduction=Prefix+"sound_reduction_percent",Energy=Prefix+"energy_scaled",Pet=Prefix+"log_model_population",PetTime=Prefix+"log_model_time",GrowthRate=Prefix+"observed_growth_rate",GrowthTime=Prefix+"observed_growth_time",Cents=Prefix+"music_cents",Frequency=Prefix+"music_frequency",Ph=Prefix+"ph_from_concentration",Ppm=Prefix+"mass_ppm",LabEq=Prefix+"lab_intersection",LabIneq=Prefix+"lab_interval";
    internal static bool Supports(string? f)=>f is Common or Change or Equation or TwoBases or Inequality or Quadratic or Sound or SoundReduction or Energy or Pet or PetTime or GrowthRate or GrowthTime or Cents or Frequency or Ph or Ppm or LabEq or LabIneq;
    internal static string Relation(int op)=>op switch{0=>"<",1=>"≤",2=>">",_=>"≥"};
    private static bool Valid(string f,IReadOnlyDictionary<string,int> a)=>Supports(f)&&a.Count==12&&new[]{"b","c","m","p","n","q","k","v","op","dir","h","t"}.All(a.ContainsKey)&&
        a["b"] is 2 or 3 or 5 or 7&&a["c"] is 2 or 3 or 5 or 7&&a["b"]!=a["c"]&&a["m"] is >=1 and <=3&&a["p"] is >=1 and <=3&&a["n"] is >=-4 and <=4&&a["q"] is >=-4 and <=4&&a["k"] is >=0 and <=3&&a["v"] is >=2 and <=80&&a["op"] is >=0 and <=3&&a["dir"] is 0 or 1&&a["h"] is >=2 and <=9&&a["t"] is >=2 and <=6;
    private static string R(double x)=>Math.Round(x,4,MidpointRounding.AwayFromZero).ToString("0.0000",CultureInfo.InvariantCulture);
    private static double B(IReadOnlyDictionary<string,int> a)=>a["dir"]==0?a["b"]:1.0/a["b"];
    private static double Slope(IReadOnlyDictionary<string,int> a)=>a["m"]*Math.Log(B(a))-(a["k"]==0?0:a["p"]*Math.Log(a["c"]));
    private static double V(string f,IReadOnlyDictionary<string,int> a)
    {
        double b=B(a),c=a["c"],m=a["m"],p=a["p"],n=a["n"],q=a["q"],v=a["v"],h=a["h"],t=a["t"],arg=v/Math.Pow(10,a["k"]),ratio=(100*a["b"]+10*v)/(100*a["b"]);
        return f switch{
            Common=>Math.Log10(arg),Change=>Math.Log10(arg)/Math.Log10(b),Equation=>(Math.Log10(v)/Math.Log10(b)-n)/m,
            TwoBases=>(q*Math.Log10(c)-n*Math.Log10(b))/(m*Math.Log10(b)-p*Math.Log10(c)),Inequality=>a["k"]==0?(Math.Log10(v)/Math.Log10(b)-n)/m:(q*Math.Log10(c)-n*Math.Log10(b))/(m*Math.Log10(b)-p*Math.Log10(c)),
            Quadratic=>Math.Sqrt(Math.Log10(v)/Math.Log10(a["b"])),Sound=>Math.Pow(10,(50+h)/10-6),
            SoundReduction=>100*(1-Math.Pow(10,-h/10)),Energy=>Math.Pow(10,11.8+1.5*(3+h/10)-16),
            Pet=>10*a["b"]*Math.Log10(v+2)/Math.Log10(c),PetTime=>Math.Pow(c,h/2)-2,
            GrowthRate=>100*(Math.Pow(ratio,1/t)-1),GrowthTime=>t*Math.Log10(2)/Math.Log10(ratio),
            Cents=>1200*Math.Log10((100*a["b"]+v)/(100.0*a["b"]))/Math.Log10(2),Frequency=>100*a["b"]*Math.Pow(2,(a["dir"]==0?1:-1)*h*25/1200),
            Ph=>h+3-Math.Log10(v),Ppm=>v/(100*t),LabEq=>1-h,_=>throw new InvalidOperationException()
        };
    }
    private static string Interval(string f,IReadOnlyDictionary<string,int> a)
    {
        if(f==LabIneq)return a["b"]<a["c"]?$"({-a["h"]},{1-a["h"]})":$"({1-a["h"]},∞)";
        int op=Slope(a)<0?a["op"]^2:a["op"];string edge=R(V(f,a));return op switch{0=>$"(-∞,{edge})",1=>$"(-∞,{edge}]",2=>$"({edge},∞)",_=>$"[{edge},∞)"};
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> a)
    {
        if(!Valid(f,a))throw new InvalidOperationException("Invalid bounded common log parameters.");
        if(f is Inequality or LabIneq)return Interval(f,a);
        if(f==Quadratic){double x=V(f,a);return $"({R(-x)},{R(x)})";}
        return R(V(f,a));
    }
    private static string Norm(string answer)=>string.Concat(answer.Trim().Select(ch=>ch switch{
        >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),>= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
        '−' or '–'=>'-','،' or '؛'=>',','٫'=>'.',_=>ch})).Replace(" ","").ToLowerInvariant().Replace("+inf","∞").Replace("inf","∞").Replace("+∞","∞");
    private static bool Num(string s,out double x)
    {
        x=0;if(!decimal.TryParse(s,NumberStyles.AllowLeadingSign|NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal d)||Math.Abs(d)>10000000m||d*10000!=decimal.Truncate(d*10000))return false;
        x=(double)d;return true;
    }
    // Independent residual equations, using natural logs rather than the generation formula.
    // Compare the residual signs at both rounding-bucket edges, including negative values.
    private static bool Bucket(string f,IReadOnlyDictionary<string,int> a,double x)
    {
        double b=B(a),c=a["c"],m=a["m"],p=a["p"],n=a["n"],q=a["q"],v=a["v"],h=a["h"],t=a["t"],arg=v/Math.Pow(10,a["k"]),ratio=(100*a["b"]+10*v)/(100*a["b"]);
        double F(double z)=>f switch{
            Common=>z*Math.Log(10)-Math.Log(arg),Change=>z*Math.Log(b)-Math.Log(arg),
            Equation=>(m*z+n)*Math.Log(b)-Math.Log(v),TwoBases=>(m*z+n)*Math.Log(b)-(p*z+q)*Math.Log(c),Inequality=>(m*z+n)*Math.Log(b)-(a["k"]==0?Math.Log(v):(p*z+q)*Math.Log(c)),
            Quadratic=>z*z*Math.Log(a["b"])-Math.Log(v),Sound=>z<=0?double.NegativeInfinity:Math.Log(z)+(6-(50+h)/10)*Math.Log(10),
            SoundReduction=>z>=100?double.NegativeInfinity:Math.Log(1-z/100)+h/10*Math.Log(10),
            Energy=>z<=0?double.NegativeInfinity:Math.Log(z)+(16-11.8-1.5*(3+h/10))*Math.Log(10),
            Pet=>z*Math.Log(c)/(10*a["b"])-Math.Log(v+2),PetTime=>z<=-2?double.NegativeInfinity:Math.Log(z+2)-h/2*Math.Log(c),
            GrowthRate=>z<=-100?double.NegativeInfinity:t*Math.Log(1+z/100)-Math.Log(ratio),GrowthTime=>z*Math.Log(ratio)-t*Math.Log(2),
            Cents=>z*Math.Log(2)/1200-Math.Log((100*a["b"]+v)/(100.0*a["b"])),
            Frequency=>z<=0?double.NegativeInfinity:Math.Log(z/(100*a["b"]))-(a["dir"]==0?1:-1)*h*25/1200*Math.Log(2),
            Ph=>(z-h-3)*Math.Log(10)+Math.Log(v),Ppm=>z*100*t-v,LabEq=>z+h-1,_=>double.NaN
        };
        if(f==Quadratic&&x<=0)return false;
        if(f==SoundReduction&&(x<0||x>=100))return false;
        if(f==PetTime&&x<0)return false;
        double l=F(x-.00005),u=F(x+.00005);
        return (l<=0&&u>=0)||(l>=0&&u<=0);
    }
    private static bool VerifyInterval(string f,IReadOnlyDictionary<string,int> a,string raw)
    {
        if(raw.Length<5||raw[0] is not ('(' or '[')||raw[^1] is not (')' or ']'))return false;
        var t=raw[1..^1].Split(',');if(t.Length!=2)return false;
        if(f==LabIneq)
        {
            if(raw[0]!='('||raw[^1]!=')')return false;
            if(a["b"]<a["c"])return Num(t[0],out double l)&&l==-a["h"]&&Num(t[1],out double u)&&u==1-a["h"];
            return Num(t[0],out double z)&&z==1-a["h"]&&t[1]=="∞";
        }
        // Determine the direction from the original log-linear residual, not from formatted answer.
        double residual0=a["n"]*Math.Log(B(a))-(a["k"]==0?Math.Log(a["v"]):a["q"]*Math.Log(a["c"])),residual1=(a["m"]+a["n"])*Math.Log(B(a))-(a["k"]==0?Math.Log(a["v"]):(a["p"]+a["q"])*Math.Log(a["c"]));
        int op=residual1<residual0?a["op"]^2:a["op"];bool closed=(op&1)==1;
        if(op<2)return raw[0]=='('&&raw[^1]==(closed?']':')')&&t[0]=="-∞"&&Num(t[1],out double upper)&&Bucket(f,a,upper);
        return raw[0]==(closed?'[':'(')&&raw[^1]==')'&&t[1]=="∞"&&Num(t[0],out double lower)&&Bucket(f,a,lower);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> a,string? answer)
    {
        if(!Valid(f,a)||string.IsNullOrWhiteSpace(answer))return false;
        string raw=Norm(answer);
        if(f is Inequality or LabIneq)return VerifyInterval(f,a,raw);
        if(f==Quadratic)
        {
            if(raw.Length<5||raw[0]!='('||raw[^1]!=')')return false;var z=raw[1..^1].Split(',');
            return z.Length==2&&Num(z[0],out double l)&&Num(z[1],out double u)&&l<0&&u>0&&Bucket(f,a,-l)&&Bucket(f,a,u);
        }
        return Num(raw,out double x)&&Bucket(f,a,x);
    }
}
