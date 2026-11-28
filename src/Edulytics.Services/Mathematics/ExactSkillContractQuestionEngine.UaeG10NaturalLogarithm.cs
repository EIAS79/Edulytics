using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10NaturalLogarithm(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int b=random.Next(2,8),c=random.Next(2,9),m=random.Next(1,4)*(random.Next(2)==0?1:-1),n=random.Next(-4,5),h=random.Next(0,5),op=random.Next(4),dir=random.Next(2);
        int p=random.Next(10,51)*100,v=random.Next(2,8),r=random.Next(2,9),t=random.Next(3,13),z=random.Next(1,5);
        string rel=UaeG10NaturalLogarithmResponse.Relation(op),round="قرّب إلى أقرب جزء من عشرة آلاف.",money="قرّب إلى أقرب 0.01 AED.";
        int u=20+10*b,ambient=20,delta=u-ambient,first=ambient+delta*z/5;
        string prompt=family switch{
            UaeG10NaturalLogarithmResponse.Convert=>dir==0?$"حوّل e^({m}x)={v} إلى الصيغة اللوغاريتمية المكافئة. اكتب ln(الحجة)=الأس باستعمال x.":$"حوّل ln(x)={m} إلى الصيغة الأسية المكافئة. اكتب e^(الأس)=x.",
            UaeG10NaturalLogarithmResponse.Identity=>dir==0?$"احسب ln(e^({m})) بالضبط.":$"احسب e^(ln {v}) بالضبط.",
            UaeG10NaturalLogarithmResponse.Value=>$"احسب ln({v}/{b}). {round}",
            UaeG10NaturalLogarithmResponse.Condense=>$"اكتب {Math.Abs(m)}ln({b})+ln({c})−ln({v}) على صورة ln(A). اكتب A كعدد صحيح أو كسر.",
            UaeG10NaturalLogarithmResponse.Expand=>$"لـ x,y>0، وسّع ln({b}×x^{Math.Abs(m)}/y^{z}) على صورة A ln({b})+B ln x+C ln y. اكتب (A,B,C).",
            UaeG10NaturalLogarithmResponse.Equation=>$"حل {b}e^({m}x)+({n})={b*v+n}. {round}",
            UaeG10NaturalLogarithmResponse.Possible=>$"هل للمعادلة {b}e^({m}x)+({n})={n+(dir==0?b*v:-b*v)} حل حقيقي؟ اكتب 1 لنعم أو 0 للا.",
            UaeG10NaturalLogarithmResponse.LogEq=>$"حل {m}ln({b}(x+{h}))={n}. {round}",
            UaeG10NaturalLogarithmResponse.ExpIneq=>$"حل {b}e^({m}x)+({n}) {rel} {b*v+n}. اكتب فترة الحل بالحد المقرب لأربع منازل.",
            UaeG10NaturalLogarithmResponse.LogIneq=>$"حل {m}ln({b}(x+{h})) {rel} {n}. اكتب فترة الحل بالحد المقرب لأربع منازل، مع مراعاة المجال.",
            UaeG10NaturalLogarithmResponse.Even=>$"حل ln(x^{2*z}) {rel} {v}. اكتب اتحاد الفترتين بفاصل ∪، والحدود إلى أربع منازل. تذكر x≠0.",
            UaeG10NaturalLogarithmResponse.Reciprocal=>$"حل −{b}/ln(x−{h})≤{c}. اكتب اتحاد الفترات (∪) والحد إلى أربع منازل.",
            UaeG10NaturalLogarithmResponse.Balance=>$"في نموذج المرابحة المستمرة A=Pe^(rt)، إذا P={p} AED وr={r}% سنويًا وt={t} سنوات فأوجد A. {money}",
            UaeG10NaturalLogarithmResponse.Principal=>$"أوجد رأس المال P ليصبح الرصيد {p*v} AED بعد {t} سنوات بمعدل {r}% مستمر. {money}",
            UaeG10NaturalLogarithmResponse.Time=>$"استثمر {p} AED بمعدل {r}% مستمر. كم سنة حتى يصل الرصيد إلى {p*v} AED؟ {round}",
            UaeG10NaturalLogarithmResponse.Rate=>$"في نموذج المرابحة المستمرة، أصبح {p} AED مبلغ {p*v} AED خلال {t} سنوات. أوجد المعدل السنوي بالمئة. {round}",
            UaeG10NaturalLogarithmResponse.Decay=>$"قيمة سيارة تعليمية V={p*5}e^(-{(r/100m).ToString(CultureInfo.InvariantCulture)}t) AED، t بالسنوات. أوجد قيمتها بعد {6*z} شهرًا. {money}",
            UaeG10NaturalLogarithmResponse.Half=>$"في نموذج V={p*5}e^(-{(r/100m).ToString(CultureInfo.InvariantCulture)}t)، متى تنخفض القيمة إلى النصف؟ {round}",
            UaeG10NaturalLogarithmResponse.Threshold=>$"في نموذج V={p*5}e^(-{(r/100m).ToString(CultureInfo.InvariantCulture)}t)، أوجد زمن المساواة V={p}؛ بعدها تصبح القيمة أقل. {round}",
            UaeG10NaturalLogarithmResponse.CoolingK=>$"جسم حرارته {u}°C في محيط 20°C؛ عند {t} دقائق أصبحت {first}°C. في T=20+({delta})e^(-kt)، أوجد k إلى ست منازل.",
            UaeG10NaturalLogarithmResponse.CoolingTemp=>$"جسم حرارته {u}°C في محيط 20°C؛ عند {t} دقائق أصبحت {first}°C. وفق نموذج نيوتن أوجد T بعد {2*t+z} دقيقة إلى أقرب جزء من مائة.",
            UaeG10NaturalLogarithmResponse.CoolingTime=>$"جسم حرارته {u}°C في محيط 20°C؛ عند {t} دقائق أصبحت {first}°C. وفق نموذج نيوتن متى يصبح T=20+{delta}/{v+2} °C؟ {round}",
            UaeG10NaturalLogarithmResponse.Substitution=>$"حل {b}^(2x)−({v-1})×{b}^x−{v}=0 باستبدال u={b}^x>0. {round}",
            _=>throw new InvalidOperationException()
        };
        bool structured=family is UaeG10NaturalLogarithmResponse.Convert or UaeG10NaturalLogarithmResponse.Condense or UaeG10NaturalLogarithmResponse.Expand or UaeG10NaturalLogarithmResponse.ExpIneq or UaeG10NaturalLogarithmResponse.LogIneq or UaeG10NaturalLogarithmResponse.Even or UaeG10NaturalLogarithmResponse.Reciprocal;
        return Problem(family,prompt,"اعزل e أو ln، تحقق من الحجة والمقام وإشارة المعامل، واستعمل القيمة غير المقربة حتى آخر خطوة.",structured?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
            ("b",b),("c",c),("m",m),("n",n),("h",h),("op",op),("dir",dir),("p",p),("v",v),("r",r),("t",t),("z",z));
    }
}
internal static class UaeG10NaturalLogarithmResponse
{
    internal const string Prefix="uae.g10.general.u05.l07.";
    internal const string Convert=Prefix+"convert_equivalent",Identity=Prefix+"inverse_identity",Value=Prefix+"ln_value",Condense=Prefix+"ln_condense_ratio",Expand=Prefix+"ln_expand_vector",Equation=Prefix+"natural_exponential_equation",Possible=Prefix+"positive_rhs_possible",LogEq=Prefix+"natural_log_equation",ExpIneq=Prefix+"natural_exponential_interval",LogIneq=Prefix+"natural_log_interval",Even=Prefix+"even_power_interval",Reciprocal=Prefix+"reciprocal_ln_interval",Balance=Prefix+"continuous_balance",Principal=Prefix+"continuous_principal",Time=Prefix+"continuous_time",Rate=Prefix+"continuous_rate",Decay=Prefix+"continuous_depreciation",Half=Prefix+"continuous_half_time",Threshold=Prefix+"continuous_threshold",CoolingK=Prefix+"cooling_constant",CoolingTemp=Prefix+"cooling_temperature",CoolingTime=Prefix+"cooling_time",Substitution=Prefix+"exponential_substitution";
    internal static bool Supports(string? f)=>f is Convert or Identity or Value or Condense or Expand or Equation or Possible or LogEq or ExpIneq or LogIneq or Even or Reciprocal or Balance or Principal or Time or Rate or Decay or Half or Threshold or CoolingK or CoolingTemp or CoolingTime or Substitution;
    internal static string Relation(int op)=>op switch{0=>"<",1=>"≤",2=>">",_=>"≥"};
    private static bool Valid(string f,IReadOnlyDictionary<string,int> a)=>Supports(f)&&a.Count==12&&new[]{"b","c","m","n","h","op","dir","p","v","r","t","z"}.All(a.ContainsKey)&&a["b"] is >=2 and <=7&&a["c"] is >=2 and <=8&&a["m"] is >=-3 and <=3&&a["m"]!=0&&a["n"] is >=-4 and <=4&&a["h"] is >=0 and <=4&&a["op"] is >=0 and <=3&&a["dir"] is 0 or 1&&a["p"] is >=1000 and <=5000&&a["p"]%100==0&&a["v"] is >=2 and <=7&&a["r"] is >=2 and <=8&&a["t"] is >=3 and <=12&&a["z"] is >=1 and <=4;
    private static int D(string f)=>f is Balance or Principal or Decay or CoolingTemp?2:f==CoolingK?6:4;
    private static string R(double x,int d=4)=>Math.Round(x,d,MidpointRounding.AwayFromZero).ToString("F"+d,CultureInfo.InvariantCulture);
    private static string Norm(string answer)=>string.Concat(answer.Trim().Select(ch=>ch switch{>= '٠' and <= '٩'=>(char)('0'+ch-'٠'),>= '۰' and <= '۹'=>(char)('0'+ch-'۰'),'−' or '–'=>'-','،' or '؛'=>',','٫'=>'.','س'=>'x',_=>ch})).Replace(" ","").ToLowerInvariant().Replace("+inf","∞").Replace("inf","∞").Replace("+∞","∞").Replace("union","∪");
    private static bool Num(string s,int d,out double x)
    {
        x=0;if(!decimal.TryParse(s,NumberStyles.AllowLeadingSign|NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal y)||y < -10000000m||y>10000000m)return false;
        decimal z=y*(decimal)Math.Pow(10,d);if(z!=decimal.Truncate(z))return false;x=(double)y;return true;
    }
    private static int IntPow(int b,int n){int v=1;for(int i=0;i<n;i++)v*=b;return v;}
    private static string Fraction(int n,int d){int a=n,b=d;while(b!=0){int t=a%b;a=b;b=t;}return d/a==1?(n/a).ToString(CultureInfo.InvariantCulture):$"{n/a}/{d/a}";}
    private static double V(string f,IReadOnlyDictionary<string,int> a)
    {
        double b=a["b"],m=a["m"],n=a["n"],h=a["h"],p=a["p"],v=a["v"],r=a["r"]/100.0,t=a["t"],z=a["z"],k=-Math.Log(z/5)/t;
        return f switch{Identity=>a["dir"]==0?m:v,Value=>Math.Log(v/b),Equation or ExpIneq=>Math.Log(v)/m,Possible=>a["dir"]==0?1:0,LogEq or LogIneq=>Math.Exp(n/m)/b-h,Even=>Math.Exp(v/(2*z)),Reciprocal=>h+Math.Exp(-b/a["c"]),Balance=>p*Math.Exp(r*t),Principal=>p*v*Math.Exp(-r*t),Time=>Math.Log(v)/r,Rate=>100*Math.Log(v)/t,Decay=>5*p*Math.Exp(-r*z/2),Half=>Math.Log(2)/r,Threshold=>Math.Log(5)/r,CoolingK=>k,CoolingTemp=>20+10*b*Math.Exp(-k*(2*t+z)),CoolingTime=>Math.Log(v+2)/k,Substitution=>Math.Log(v)/Math.Log(b),_=>throw new InvalidOperationException()};
    }
    private static string ConvertText(IReadOnlyDictionary<string,int> a)=>a["dir"]==0?$"ln({a["v"]})={a["m"]}x":$"e^({a["m"]})=x";
    private static string Interval(string f,IReadOnlyDictionary<string,int> a)
    {
        string e=R(V(f,a));int op=a["m"]<0?a["op"]^2:a["op"];int h=a["h"];
        if(f==ExpIneq)return op switch{0=>$"(-∞,{e})",1=>$"(-∞,{e}]",2=>$"({e},∞)",_=>$"[{e},∞)"};
        if(f==LogIneq)return op switch{0=>$"({-h},{e})",1=>$"({-h},{e}]",2=>$"({e},∞)",_=>$"[{e},∞)"};
        if(f==Even)return a["op"] switch{0=>$"(-{e},0)∪(0,{e})",1=>$"[-{e},0)∪(0,{e}]",2=>$"(-∞,-{e})∪({e},∞)",_=>$"(-∞,-{e}]∪[{e},∞)"};
        return $"({h},{e}]∪({h+1},∞)";
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> a)
    {
        if(!Valid(f,a))throw new InvalidOperationException("Invalid bounded natural log parameters.");
        if(f==Convert)return ConvertText(a);
        if(f==Condense)return Fraction(IntPow(a["b"],Math.Abs(a["m"]))*a["c"],a["v"]);
        if(f==Expand)return $"(1,{Math.Abs(a["m"])},{-a["z"]})";
        if(f is ExpIneq or LogIneq or Even or Reciprocal)return Interval(f,a);
        if(f is Identity or Possible)return ((int)V(f,a)).ToString(CultureInfo.InvariantCulture);
        return R(V(f,a),D(f));
    }
    // Independent residual signs in original model, evaluated at rounding bucket edges.
    private static bool Bucket(string f,IReadOnlyDictionary<string,int> a,double x)
    {
        double b=a["b"],m=a["m"],n=a["n"],h=a["h"],p=a["p"],v=a["v"],r=a["r"]/100.0,t=a["t"],z=a["z"],k=-Math.Log(z/5)/t;
        double F(double q)=>f switch{
            Value=>Math.Exp(q)*b-v,Equation or ExpIneq=>m*q-Math.Log(v),
            LogEq or LogIneq=>q<=-h?double.NegativeInfinity:m>0?Math.Log(b*(q+h))*m-n:n-Math.Log(b*(q+h))*m,
            Even=>q<=0?double.NegativeInfinity:2*z*Math.Log(q)-v,Reciprocal=>q<=h?double.NegativeInfinity:a["c"]*Math.Log(q-h)+b,
            Balance=>q<=0?double.NegativeInfinity:Math.Log(q/p)-r*t,Principal=>q<=0?double.NegativeInfinity:Math.Log(q/(p*v))+r*t,
            Time=>r*q-Math.Log(v),Rate=>q*t/100-Math.Log(v),Decay=>q<=0?double.NegativeInfinity:Math.Log(q/(5*p))+r*z/2,
            Half=>r*q-Math.Log(2),Threshold=>r*q-Math.Log(5),CoolingK=>Math.Exp(-q*t)-z/5,
            CoolingTemp=>q-20-10*b*Math.Pow(z/5,(2*t+z)/t),CoolingTime=>q*k-Math.Log(v+2),
            Substitution=>q*Math.Log(b)-Math.Log(v),_=>double.NaN};
        double e=.5/Math.Pow(10,D(f)),l=F(x-e),u=F(x+e);return (l<=0&&u>=0)||(l>=0&&u<=0);
    }
    private static bool VerifyInterval(string f,IReadOnlyDictionary<string,int> a,string raw)
    {
        // Parse and verify all endpoints and bracket topology independently.
        var parts=raw.Split('∪');int op=a["m"]<0?a["op"]^2:a["op"],h=a["h"];
        bool Match(string s,char l,char r,Func<string,bool> lo,Func<string,bool> hi)
        {if(s.Length<5||s[0]!=l||s[^1]!=r)return false;var x=s[1..^1].Split(',');return x.Length==2&&lo(x[0])&&hi(x[1]);}
        bool Edge(string s)=>Num(s,4,out double x)&&Bucket(f,a,x);
        bool NegEdge(string s)=>Num(s,4,out double x)&&x<0&&Bucket(f,a,-x);
        bool Fixed(string s,int k)=>Num(s,4,out double x)&&x==k;
        if(f==ExpIneq||f==LogIneq)
        {
            if(parts.Length!=1)return false;
            if(op<2)return Match(parts[0],'(',(op&1)==1?']':')',s=>f==ExpIneq?s=="-∞":Fixed(s,-h),Edge);
            return Match(parts[0],(op&1)==1?'[':'(',')',Edge,s=>s=="∞");
        }
        if(parts.Length!=2)return false;
        if(f==Reciprocal)return Match(parts[0],'(',']',s=>Fixed(s,h),Edge)&&Match(parts[1],'(',')',s=>Fixed(s,h+1),s=>s=="∞");
        op=a["op"];
        if(op<2)return Match(parts[0],(op&1)==1?'[':'(',')',NegEdge,s=>Fixed(s,0))&&Match(parts[1],'(',(op&1)==1?']':')',s=>Fixed(s,0),Edge);
        return Match(parts[0],'(',(op&1)==1?']':')',s=>s=="-∞",NegEdge)&&Match(parts[1],(op&1)==1?'[':'(',')',Edge,s=>s=="∞");
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> a,string? answer)
    {
        if(!Valid(f,a)||string.IsNullOrWhiteSpace(answer))return false;string raw=Norm(answer);
        if(f==Convert){var sides=raw.Split('=');if(sides.Length!=2)return false;string exp=a["dir"]==0?$"ln({a["v"]})":$"e^({a["m"]})",other=a["dir"]==0?$"{a["m"]}x":"x";return sides[0]==exp&&sides[1]==other||sides[1]==exp&&sides[0]==other;}
        if(f==Condense){var s=raw.Split('/');if(s.Length is <1 or >2||!long.TryParse(s[0],out long n)||n<=0||n>1000000)return false;long d=1;if(s.Length==2&&(!long.TryParse(s[1],out d)||d<=0||d>1000000))return false;return n*a["v"]==d*IntPow(a["b"],Math.Abs(a["m"]))*a["c"];}
        if(f==Expand){if(raw.Length<5||raw[0]!='('||raw[^1]!=')')return false;var s=raw[1..^1].Split(',');return s.Length==3&&Num(s[0],0,out double x)&&x==1&&Num(s[1],0,out double y)&&y==Math.Abs(a["m"])&&Num(s[2],0,out double z)&&z==-a["z"];}
        if(f is ExpIneq or LogIneq or Even or Reciprocal)return VerifyInterval(f,a,raw);
        if(f==Identity)return Num(raw,0,out double v0)&&(a["dir"]==0?v0==a["m"]:v0==a["v"]);
        if(f==Possible)return Num(raw,0,out double v1)&&v1==(a["dir"]==0?1:0);
        return Num(raw,D(f),out double x0)&&Bucket(f,a,x0);
    }
}
