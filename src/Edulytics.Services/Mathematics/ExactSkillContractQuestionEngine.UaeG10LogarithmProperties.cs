using System.Globalization;
using System.Numerics;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10LogarithmProperties(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int b=random.Next(2,6),u=random.Next(-3,4),v=random.Next(-3,4),w=random.Next(-3,4),mode=random.Next(5);
        if(u==0&&v==0&&w==0)u=1;
        int p=random.Next(2,10),q=random.Next(2,10),m=random.Next(1,4),ph=random.Next(20,91),digit=random.Next(1,10);
        int rate=random.Next(50,201),balance=100*random.Next(10,31),decay=random.Next(2,9);
        string B=b.ToString(CultureInfo.InvariantCulture);
        string pay=UaeG10LogarithmPropertiesResponse.DecimalText(balance*rate/100+5000+1000*digit,2);
        string offered=UaeG10LogarithmPropertiesResponse.DecimalText(balance*rate/100+10*u,2);
        string prompt=family switch{
            UaeG10LogarithmPropertiesResponse.Approx=>$"باستعمال لوغ_4(2)=0.5، لوغ_4(3)≈0.7925، لوغ_4(5)≈1.1610، احسب لوغ_4(2^({u})×3^({v})×5^({w})) باستعمال القيم المعطاة، إلى أربع منازل عشرية.",
            UaeG10LogarithmPropertiesResponse.Expand=>$"س،ص،ع موجبة. وسّع لوغ_{B}(س^({u})×ص^({v})) + لوغ_{B}(س^({w})/ع^{m}) على صورة أ لوغ_{B}س + ج لوغ_{B}ص + د لوغ_{B}ع. اكتب (أ،ج،د) بالأرقام.",
            UaeG10LogarithmPropertiesResponse.Condense=>$"س،ص،ع موجبة. اختصر ({u})لوغ_{B}س + ({v})لوغ_{B}ص − {m}لوغ_{B}ع + ({w})لوغ_{B}س على صورة لوغ_{B}(س^أ×ص^ج×ع^د)، واكتب (أ،ج،د).",
            UaeG10LogarithmPropertiesResponse.Truth=>$"هل العبارة التالية صحيحة للأعداد الموجبة المعطاة؟ {UaeG10LogarithmPropertiesResponse.Identity(b,p,q,m,mode)}. اكتب 1 للصواب و0 للخطأ.",
            UaeG10LogarithmPropertiesResponse.Equation=>$"حل {UaeG10LogarithmPropertiesResponse.EquationText(b,p,q,m,mode)}، واكتب الحل الموجب بعد فحص المجال.",
            UaeG10LogarithmPropertiesResponse.Ph=>$"في النموذج pH=−لوغ_10([H⁺])، إذا pH={(ph/10m).ToString("0.0",CultureInfo.InvariantCulture)}، أوجد تركيز [H⁺] بالمول/لتر، مقربًا إلى 12 منزلة عشرية.",
            UaeG10LogarithmPropertiesResponse.Pressure=>$"في النموذج هـ=15500(5−لوغ_10 ض)، أوجد الضغط ض بالباسكال عند ارتفاع هـ={1550*digit} مترًا، إلى أقرب باسكال.",
            UaeG10LogarithmPropertiesResponse.Benford=>$"حسب قانون بنفورد P(د)=لوغ_10(1+1/د)، أوجد احتمال الرقم الأول {digit} كنسبة مئوية مقربة إلى أقرب عُشر بالمئة. اكتب العدد دون ٪.",
            UaeG10LogarithmPropertiesResponse.BenfordInverse=>$"وفق قانون بنفورد P(د)=لوغ_10(1+1/د)، إذا كان الاحتمال {UaeG10LogarithmPropertiesResponse.BenfordTenths(digit)/10m:0.0}٪ إلى أقرب عُشر، فما الرقم الأول د من 1 إلى 9؟",
            UaeG10LogarithmPropertiesResponse.Decay=>$"كمية ابتدائية {balance} تتناقص {decay}٪ سنويًا. ما أول سنة صحيحة تقل عندها الكمية عن {balance}/{m+1} من وحدات الكمية؟ لا تقرّب قبل المقارنة.",
            UaeG10LogarithmPropertiesResponse.Sound=>mode%2==0?$"إذا تغير مستوى الصوت بمقدار {10*m} ديسيبل في ل=10لوغ_10 ن، كم مرة تضاعفت الشدة النسبية؟":$"إذا ازدادت الشدة النسبية {UaeG10LogarithmPropertiesResponse.PowInt(10,m)} مرة، فما زيادة الديسيبل؟",
            UaeG10LogarithmPropertiesResponse.Payment=>$"دين افتراضي {balance} درهم، معدل شهري {(rate/100m).ToString("0.00",CultureInfo.InvariantCulture)}٪، يسدد بدفعات شهرية متساوية في {12*m} شهرًا دون رسوم. أوجد القسط إلى أقرب 0.01 درهم.",
            UaeG10LogarithmPropertiesResponse.Payable=>$"دين افتراضي {balance} درهم، معدل شهري {(rate/100m).ToString("0.00",CultureInfo.InvariantCulture)}٪، ودفعة شهرية {offered} درهم دون رسوم. هل ينخفض الرصيد بالدفع؟ 1 لنعم أو 0 للا.",
            UaeG10LogarithmPropertiesResponse.Minimum=>$"دين افتراضي {balance} درهم، معدل شهري {(rate/100m).ToString("0.00",CultureInfo.InvariantCulture)}٪. ما أصغر قسط شهري بأجزاء 0.01 درهم يخفض الرصيد، دون رسوم أو قيود تعاقدية؟",
            UaeG10LogarithmPropertiesResponse.Months=>$"دين افتراضي {balance} درهم، معدل شهري {(rate/100m).ToString("0.00",CultureInfo.InvariantCulture)}٪، وقسط شهري {pay} درهم. ما أول عدد صحيح من الأشهر يكفي للسداد، مع تسوية الدفعة الأخيرة ودون رسوم؟",
            _=>throw new InvalidOperationException()
        };
        bool numeric=family is UaeG10LogarithmPropertiesResponse.Truth or UaeG10LogarithmPropertiesResponse.Equation or UaeG10LogarithmPropertiesResponse.Pressure or UaeG10LogarithmPropertiesResponse.BenfordInverse or UaeG10LogarithmPropertiesResponse.Decay or UaeG10LogarithmPropertiesResponse.Sound or UaeG10LogarithmPropertiesResponse.Payable or UaeG10LogarithmPropertiesResponse.Months;
        return Problem(family,prompt,"استعمل خواص الضرب والقسمة والقوة مع حجج موجبة؛ تحقق بالتعويض، واستخدم تقريبًا نهائيًا فقط في النماذج.",numeric?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,
            ("b",b),("u",u),("v",v),("w",w),("mode",mode),("p",p),("q",q),("m",m),("ph",ph),("digit",digit),("rate",rate),("balance",balance),("decay",decay));
    }
}
internal static class UaeG10LogarithmPropertiesResponse
{
    internal const string Prefix="uae.g10.general.u05.l05.";
    internal const string Approx=Prefix+"approx_from_given",Expand=Prefix+"expand_coefficients",Condense=Prefix+"condense_powers",Truth=Prefix+"property_truth",Equation=Prefix+"solve_property_equation",
        Ph=Prefix+"ph_concentration",Pressure=Prefix+"pressure_rounded",Benford=Prefix+"benford_percent",BenfordInverse=Prefix+"benford_digit",Decay=Prefix+"first_decay_year",
        Sound=Prefix+"relative_sound",Payment=Prefix+"repayment_payment",Payable=Prefix+"repayment_possible",Minimum=Prefix+"repayment_minimum",Months=Prefix+"repayment_months";
    internal static bool Supports(string? f)=>f is Approx or Expand or Condense or Truth or Equation or Ph or Pressure or Benford or BenfordInverse or Decay or Sound or Payment or Payable or Minimum or Months;
    internal static int PowInt(int b,int n){int r=1;for(int i=0;i<n;i++)r*=b;return r;}
    internal static string Identity(int b,int p,int q,int m,int mode)=>mode switch{
        0=>$"لوغ_{b}({p*q})=لوغ_{b}({p})+لوغ_{b}({q})",
        1=>$"لوغ_{b}({p+q+1})=لوغ_{b}({p})+لوغ_{b}({q})",
        2=>$"لوغ_{b}({PowInt(p,m+1)})=({m+1})لوغ_{b}({p})",
        3=>$"لوغ_{b}({p}/{q})=لوغ_{b}({q})−لوغ_{b}({p})",
        _=>$"لوغ_{b}(1)=1"
    };
    internal static string EquationText(int b,int p,int q,int m,int mode)=>mode switch{
        0=>$"لوغ_{b}س + لوغ_{b}(س+({q-p})) = لوغ_{b}({p*q})",
        1=>$"لوغ_{b}({p*q})−لوغ_{b}س=لوغ_{b}({q})",
        2=>$"{m+1}لوغ_{b}س=لوغ_{b}({PowInt(p,m+1)})",
        3=>$"لوغ_{b}({q}س)+لوغ_{b}({m+1})=لوغ_{b}({p*q*(m+1)})",
        _=>$"{m}لوغ_{b}({q})+{m}لوغ_{b}س=لوغ_{b}({PowInt(p*q,m)})"
    };
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==13&&
        new[]{"b","u","v","w","mode","p","q","m","ph","digit","rate","balance","decay"}.All(p.ContainsKey)&&
        p["b"] is >=2 and <=5&&p["u"] is >=-3 and <=3&&p["v"] is >=-3 and <=3&&p["w"] is >=-3 and <=3&&p["mode"] is >=0 and <=4&&
        p["p"] is >=2 and <=9&&p["q"] is >=2 and <=9&&p["m"] is >=1 and <=3&&p["ph"] is >=20 and <=90&&p["digit"] is >=1 and <=9&&
        p["rate"] is >=50 and <=200&&p["balance"] is >=1000 and <=3000&&p["balance"]%100==0&&p["decay"] is >=2 and <=8;
    private static string Norm(string answer)=>string.Concat(answer.Trim().Select(ch=>ch switch{
        >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),>= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
        '−' or '–'=>'-','،' or '؛'=>',','٫'=>'.',_=>ch})).Replace(" ","");
    internal static string DecimalText(long scaled,int places)=>((decimal)scaled/(decimal)BigInteger.Pow(10,places)).ToString("F"+places,CultureInfo.InvariantCulture);
    private static bool TryScaled(string s,int places,out long n)
    {
        n=0;if(!decimal.TryParse(s,NumberStyles.AllowLeadingSign|NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal value)||Math.Abs(value)>1000000000m)return false;
        decimal a=value*(decimal)BigInteger.Pow(10,places);if(a<long.MinValue||a>long.MaxValue||a!=decimal.Truncate(a))return false;
        n=(long)a;return true;
    }
    private static long RoundTenPower(int exponentTenths,int places)=>(long)Math.Round(Math.Pow(10,exponentTenths/10.0)*Math.Pow(10,places),MidpointRounding.AwayFromZero);
    private static bool TenPowerBucket(long k,int exponentTenths,int places)
    {
        if(k<1)return false;
        BigInteger scale=2*BigInteger.Pow(10,places),low=BigInteger.Pow(2*(BigInteger)k-1,10),high=BigInteger.Pow(2*(BigInteger)k+1,10),actual=BigInteger.Pow(scale,10);
        if(exponentTenths>=0)actual*=BigInteger.Pow(10,exponentTenths);
        else{var x=BigInteger.Pow(10,-exponentTenths);low*=x;high*=x;}
        return low<=actual&&actual<high;
    }
    internal static int BenfordTenths(int d)=>(int)Math.Round(1000*Math.Log10(1+1.0/d),MidpointRounding.AwayFromZero);
    private static bool BenfordBucket(int d,long k)
    {
        if(d is <1 or >9||k is <1 or >400)return false;
        BigInteger lhs=BigInteger.Pow(d+1,2000),den=BigInteger.Pow(d,2000);
        return den*BigInteger.Pow(10,(int)(2*k-1))<=lhs&&lhs<den*BigInteger.Pow(10,(int)(2*k+1));
    }
    private static bool Below(IReadOnlyDictionary<string,int> p,int years)=>
        (p["m"]+1)*BigInteger.Pow(100-p["decay"],years)<BigInteger.Pow(100,years);
    private static int DecayYears(IReadOnlyDictionary<string,int> p){int t=1;while(!Below(p,t))t++;return t;}
    private static int InterestCents(IReadOnlyDictionary<string,int> p)=>p["balance"]*p["rate"]/100;
    private static int OfferCents(IReadOnlyDictionary<string,int> p)=>InterestCents(p)+10*p["u"];
    private static int PaymentCents(IReadOnlyDictionary<string,int> p)=>InterestCents(p)+5000+1000*p["digit"];
    private static (BigInteger Num,BigInteger Den) ExactPayment(IReadOnlyDictionary<string,int> p)
    {
        BigInteger a=BigInteger.Pow(10000+p["rate"],12*p["m"]),b=BigInteger.Pow(10000,12*p["m"]);
        return (p["balance"]*p["rate"]*a,100*(a-b)); // cents
    }
    private static BigInteger Residual(IReadOnlyDictionary<string,int> p,int payment,int months)
    {
        BigInteger a=BigInteger.Pow(10000+p["rate"],months),b=BigInteger.Pow(10000,months);
        return (BigInteger)p["balance"]*100*p["rate"]*a-(BigInteger)payment*10000*(a-b);
    }
    private static int MonthsNeeded(IReadOnlyDictionary<string,int> p){int t=1;while(Residual(p,PaymentCents(p),t)>0)t++;return t;}
    private static bool TruthValue(IReadOnlyDictionary<string,int> p)=>p["mode"] switch{0 or 2=>true,1=>p["p"]+p["q"]+1==p["p"]*p["q"],3=>p["p"]==p["q"],_=>false};
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid bounded logarithm properties parameters.");
        if(f==Approx)return DecimalText(5000L*p["u"]+7925L*p["v"]+11610L*p["w"],4);
        if(f is Expand or Condense)return $"({p["u"]+p["w"]},{p["v"]},{-p["m"]})";
        if(f==Ph)return DecimalText(RoundTenPower(-p["ph"],12),12);
        if(f==Pressure)return RoundTenPower(50-p["digit"],0).ToString(CultureInfo.InvariantCulture);
        if(f==Benford)return DecimalText(BenfordTenths(p["digit"]),1);
        if(f==Payment){var (n,d)=ExactPayment(p);return DecimalText((long)((2*n+d)/(2*d)),2);}
        if(f==Minimum)return DecimalText(InterestCents(p)+1,2);
        int value=f switch{Truth=>TruthValue(p)?1:0,Equation=>p["p"],BenfordInverse=>p["digit"],Decay=>DecayYears(p),Sound=>p["mode"]%2==0?PowInt(10,p["m"]):10*p["m"],Payable=>OfferCents(p)>InterestCents(p)?1:0,Months=>MonthsNeeded(p),_=>throw new InvalidOperationException()};
        return value.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(answer))return false;string raw=Norm(answer);
        if(f==Approx)return TryScaled(raw,4,out long val)&&val==5000L*p["u"]+7925L*p["v"]+11610L*p["w"];
        if(f is Expand or Condense)
        {
            raw=raw.Trim('(',')');var parts=raw.Split(',');if(parts.Length!=3)return false;
            var coeff=new int[3];for(int i=0;i<3;i++)if(!int.TryParse(parts[i],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out coeff[i]))return false;
            // Each variable's exponent is checked independently, using additivity of source exponents.
            return coeff[0]-p["w"]==p["u"]&&coeff[1]==p["v"]&&coeff[2]==-p["m"];
        }
        if(f==Ph)return TryScaled(raw,12,out long concentration)&&concentration<=1000000000000&&TenPowerBucket(concentration,-p["ph"],12);
        if(f==Pressure)return TryScaled(raw,0,out long pressure)&&pressure<=100000&&TenPowerBucket(pressure,50-p["digit"],0);
        if(f==Benford)return TryScaled(raw,1,out long k)&&BenfordBucket(p["digit"],k);
        if(f==Payment){if(!TryScaled(raw,2,out long cents)||cents<0||cents>10000000)return false;var (n,d)=ExactPayment(p);return (2*cents-1)*d<=2*n&&2*n<(2*cents+1)*d;}
        if(f==Minimum)return TryScaled(raw,2,out long min)&&min==InterestCents(p)+1;
        if(!int.TryParse(raw,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int x))return false;
        if(f==Equation)
        {
            if(x is <1 or >1000)return false;int a=p["p"],q=p["q"],m=p["m"];
            return p["mode"] switch{0=>x+q-a>0&&(long)x*(x+q-a)==a*q,1=>(long)x*q==a*q,2=>BigInteger.Pow(x,m+1)==BigInteger.Pow(a,m+1),3=>(long)q*x*(m+1)==a*q*(m+1),_=>BigInteger.Pow(q*x,m)==BigInteger.Pow(a*q,m)};
        }
        if(f==Truth)return x is 0 or 1&&(x==1)==TruthValue(p);
        if(f==BenfordInverse)return x is >=1 and <=9&&BenfordBucket(x,BenfordTenths(p["digit"]));
        if(f==Decay)return x is >=1 and <=200&&Below(p,x)&&!Below(p,x-1);
        if(f==Sound)return p["mode"]%2==0?x==PowInt(10,p["m"]):x==10*p["m"];
        if(f==Payable)return x is 0 or 1&&(x==1)==(OfferCents(p)>InterestCents(p));
        return f==Months&&x is >=1 and <=240&&Residual(p,PaymentCents(p),x)<=0&&Residual(p,PaymentCents(p),x-1)>0;
    }
}
