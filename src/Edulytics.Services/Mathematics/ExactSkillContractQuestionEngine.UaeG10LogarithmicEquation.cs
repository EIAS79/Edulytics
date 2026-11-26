using System.Globalization;
using System.Numerics;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10LogarithmicEquation(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int b=random.Next(2,6),m=random.Next(1,4),n=random.Next(-3,4),dir=random.Next(2),op=random.Next(4);
        int a=random.Next(1,4)*(random.Next(2)==0?-1:1),c=random.Next(-8,9),d=random.Next(1,4)*(random.Next(2)==0?-1:1),e=random.Next(-8,9);
        int r=random.Next(-5,5),s;do s=random.Next(-5,6);while(s==r);
        int z=random.Next(1,10);
        string basis=UaeG10LogarithmicEquationResponse.PowText(b,dir==1?m:-m);
        string parent=UaeG10LogarithmicEquationResponse.PowText(b,dir==1?1:-1);
        string rel=UaeG10LogarithmicEquationResponse.Relation(op);
        string left=$"({a})س+({c})",right=$"({d})س+({e})";
        string interval="اكتب فترة الحل بالأقواس، مثل (1/2,3] أو (−∞,3)، أو ∅ إذا لا حل. اكتب inf إذا لم تتوفر علامة اللانهاية.";
        string prompt=family switch{
            UaeG10LogarithmicEquationResponse.Direct=>op switch{
                0=>$"حل لوغ_({basis})(س)={n}/{m}. اكتب عددًا صحيحًا أو كسرًا.",
                1=>$"حل لوغ_({basis})({UaeG10LogarithmicEquationResponse.PowText(b,n)})=س. اكتب عددًا صحيحًا أو كسرًا.",
                2=>$"حل لوغ_س({UaeG10LogarithmicEquationResponse.PowText(b,m)})={(dir==1?m:-m)}/{1+z%3}. اكتب الأساس الموجب المختلف عن 1 كعدد صحيح أو كسر.",
                _=>$"احسب مجموع {string.Join(" + ",Enumerable.Range(1,5).Select(i=>$"لوغ_({UaeG10LogarithmicEquationResponse.PowText(b,dir==1?i:-i)})({UaeG10LogarithmicEquationResponse.PowText(b,n)})"))} كعدد صحيح أو كسر."
            },
            UaeG10LogarithmicEquationResponse.Quadratic=>dir==1?$"حل لوغ_{b}(س²+({a-r-s})س+({c+r*s}))=لوغ_{b}(({a})س+({c})). اكتب الجذور المقبولة بالأرقام مفصولة بفاصلة، أو ∅ إن لم يوجد حل.":$"حل لوغ_{b}(({a+1})س+({c-r}))=لوغ_{b}(({a})س+({c})). اكتب الجذور المقبولة بالأرقام، أو ∅ إن لم يوجد حل.",
            UaeG10LogarithmicEquationResponse.OneSide=>$"حل لوغ_({parent})({left}) {rel} {n}. {interval}",
            UaeG10LogarithmicEquationResponse.TwoSides=>$"حل لوغ_({parent})({left}) {rel} لوغ_({parent})({right}). {interval}",
            UaeG10LogarithmicEquationResponse.Wind=>dir==1?$"في نموذج تدريبي ع={10*b}لوغ(م)+{10*m}، ع سرعة بالميل/ساعة وم مسافة بالأميال. إذا ع={10*b*z+10*m}، أوجد م.":$"في النموذج التدريبي ع={10*b}لوغ(م)+{10*m}، احسب ع عندما م={10+13*z+7*Math.Abs(c)} ميلًا. قرّب إلى أقرب ميل/ساعة.",
            UaeG10LogarithmicEquationResponse.Ratio=>$"فرق قياسين لوغاريتميين للأساس 10 يساوي {(dir==1?z/10m:m).ToString("0.0",CultureInfo.InvariantCulture)}. كم تبلغ نسبة السعتين؟ قرّب إلى أقرب جزء من مائة.",
            UaeG10LogarithmicEquationResponse.Music=>$"في نموذج تردد ت={25*b}×2^((ن−1)/12) هرتز. أوجد ن عندما ت={25*b*(1<<Math.Abs(n))} هرتز.",
            UaeG10LogarithmicEquationResponse.Sound=>$"في النموذج β=10 لوغ(I/10^(-12))، أوجد β بالديسيبل عندما I=10^({n-6}) واط/م².",
            _=>throw new InvalidOperationException("Unknown logarithmic equation family.")
        };
        bool integer=family is UaeG10LogarithmicEquationResponse.Wind or UaeG10LogarithmicEquationResponse.Music or UaeG10LogarithmicEquationResponse.Sound;
        return Problem(family,prompt,"تحقق من موجبية كل حجة؛ حوّل إلى أس أو قارن الحجج مع عكس الاتجاه للأساس المقلوب. افحص الجذور والحدود في الأصل.",integer?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,
            ("b",b),("m",m),("n",n),("dir",dir),("op",op),("a",a),("c",c),("d",d),("e",e),("r",r),("s",s),("z",z));
    }
}

internal static class UaeG10LogarithmicEquationResponse
{
    internal const string Prefix="uae.g10.general.u05.l04.";
    internal const string Direct=Prefix+"direct_rational",Quadratic=Prefix+"quadratic_root_set",
        OneSide=Prefix+"one_side_interval",TwoSides=Prefix+"two_sides_interval",
        Wind=Prefix+"wind_distance",Ratio=Prefix+"rounded_amplitude_ratio",Music=Prefix+"music_key",Sound=Prefix+"sound_level";
    internal static bool Supports(string? f)=>f is Direct or Quadratic or OneSide or TwoSides or Wind or Ratio or Music or Sound;
    internal static int Pow(int b,int n){int v=1;for(int i=0;i<n;i++)v*=b;return v;}
    internal static string PowText(int b,int n)=>n<0?$"1/{Pow(b,-n)}":Pow(b,n).ToString(CultureInfo.InvariantCulture);
    internal static string Relation(int op)=>op switch{0=>"<",1=>"≤",2=>">",_=>"≥"};
    private readonly record struct Q(long N,long D):IComparable<Q>
    {
        internal static Q Make(long n,long d=1)
        {
            if(d==0)throw new ArgumentException("zero denominator");
            if(d<0){n=-n;d=-d;}long a=Math.Abs(n),b=d;
            while(b!=0){long t=a%b;a=b;b=t;}return new Q(n/a,d/a);
        }
        public int CompareTo(Q other)=>(new BigInteger(N)*other.D).CompareTo(new BigInteger(other.N)*D);
        internal Q Plus(Q x)=>Make(N*x.D+x.N*D,D*x.D);
        internal Q Shift(int x)=>Make(N+x*D,D);
        internal Q Half()=>Make(N,D*2);
        public override string ToString()=>D==1?N.ToString(CultureInfo.InvariantCulture):$"{N}/{D}";
    }
    private readonly record struct C(long A,long B,bool Closed)
    {
        internal bool Holds(Q x){var v=new BigInteger(A)*x.N+new BigInteger(B)*x.D;return Closed?v>=0:v>0;}
        internal Q Root()=>Q.Make(-B,A);
    }
    private readonly record struct I(Q? Low,Q? High,bool LC,bool HC,bool Empty)
    {
        internal bool Has(Q x)=>!Empty&&(!Low.HasValue||x.CompareTo(Low.Value)>0||(LC&&x.CompareTo(Low.Value)==0))&&
            (!High.HasValue||x.CompareTo(High.Value)<0||(HC&&x.CompareTo(High.Value)==0));
        internal string Text()=>Empty?"∅":(LC?"[":"(")+(Low?.ToString()??"-∞")+","+(High?.ToString()??"∞")+(HC?"]":")");
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==12&&
        new[]{"b","m","n","dir","op","a","c","d","e","r","s","z"}.All(p.ContainsKey)&&
        p["b"] is >=2 and <=5&&p["m"] is >=1 and <=3&&p["n"] is >=-3 and <=3&&p["dir"] is 0 or 1&&p["op"] is >=0 and <=3&&
        p["a"] is >=-3 and <=3&&p["a"]!=0&&p["d"] is >=-3 and <=3&&p["d"]!=0&&
        p["c"] is >=-8 and <=8&&p["e"] is >=-8 and <=8&&p["r"] is >=-5 and <=4&&p["s"] is >=-5 and <=5&&p["r"]!=p["s"]&&p["z"] is >=1 and <=9;
    private static Q Power(int b,int n)=>n>=0?Q.Make(Pow(b,n)):Q.Make(1,Pow(b,-n));
    private static C[] Conditions(string f,IReadOnlyDictionary<string,int> p)
    {
        int op=p["dir"]==1?p["op"]:p["op"]^2, sign=op>=2?1:-1;
        bool closed=(op&1)==1;
        int a=p["a"],c=p["c"];
        var result=new List<C>{new(a,c,false)};
        if(f==OneSide)
        {
            var limit=Power(p["b"],p["n"]*(p["dir"]==1?1:-1));
            result.Add(new(sign*a*limit.D,sign*(c*limit.D-limit.N),closed));
        }
        else
        {
            int d=p["d"],e=p["e"];
            result.Add(new(d,e,false));result.Add(new(sign*(a-d),sign*(c-e),closed));
        }
        return result.ToArray();
    }
    private static I Solve(C[] cs)
    {
        Q? lo=null,hi=null;bool lc=false,hc=false;
        foreach(var c in cs)
        {
            if(c.A==0){if(!c.Holds(Q.Make(0)))return new(null,null,false,false,true);continue;}
            Q x=c.Root();
            if(c.A>0)
            {
                if(!lo.HasValue||x.CompareTo(lo.Value)>0){lo=x;lc=c.Closed;}
                else if(x.CompareTo(lo.Value)==0)lc&=c.Closed;
            }
            else
            {
                if(!hi.HasValue||x.CompareTo(hi.Value)<0){hi=x;hc=c.Closed;}
                else if(x.CompareTo(hi.Value)==0)hc&=c.Closed;
            }
        }
        bool empty=lo.HasValue&&hi.HasValue&&(lo.Value.CompareTo(hi.Value)>0||(lo.Value.CompareTo(hi.Value)==0&&!(lc&&hc)));
        return new(lo,hi,lc,hc,empty);
    }
    private static string Norm(string answer)=>string.Concat(answer.Trim().Select(ch=>ch switch{
        >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),>= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
        '−' or '–'=>'-','⁄' or '∕'=>'/','،' or '؛'=>',','٫'=>'.',_=>ch})).Replace(" ","").ToLowerInvariant();
    private static bool TryQ(string s,out Q q)
    {
        q=default;var t=s.Split('/');
        if(t.Length is <1 or >2||!long.TryParse(t[0],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out long n)||n is <-1000000000 or >1000000000)return false;
        long d=1;if(t.Length==2&&(!long.TryParse(t[1],NumberStyles.None,CultureInfo.InvariantCulture,out d)||d is <1 or >1000000))return false;
        q=Q.Make(n,d);return true;
    }
    private static bool TryInterval(string s,out I interval)
    {
        interval=new(null,null,false,false,true);
        if(s is "∅" or "{}" or "empty")return true;
        if(s.Length<5||s[0] is not ('(' or '[')||s[^1] is not (')' or ']'))return false;
        var t=s[1..^1].Split(',');if(t.Length!=2)return false;
        Q? lo=null,hi=null;bool lc=s[0]=='[',hc=s[^1]==']';
        if(t[0] is "-∞" or "-inf"){if(lc)return false;}else{if(!TryQ(t[0],out Q q))return false;lo=q;}
        if(t[1] is "∞" or "+∞" or "inf" or "+inf"){if(hc)return false;}else{if(!TryQ(t[1],out Q q))return false;hi=q;}
        if(lo.HasValue&&hi.HasValue&&(lo.Value.CompareTo(hi.Value)>0||(lo.Value.CompareTo(hi.Value)==0&&!(lc&&hc))))return false;
        interval=new(lo,hi,lc,hc,false);return true;
    }
    private static bool VerifyInterval(I candidate,C[] cs)
    {
        var cuts=cs.Where(c=>c.A!=0).Select(c=>c.Root()).ToList();
        if(candidate.Low.HasValue)cuts.Add(candidate.Low.Value);
        if(candidate.High.HasValue)cuts.Add(candidate.High.Value);
        var xs=cuts.Distinct().OrderBy(x=>x).ToArray();
        var probes=new List<Q>();
        if(xs.Length==0)probes.Add(Q.Make(0));
        else
        {
            probes.Add(xs[0].Shift(-1));probes.Add(xs[^1].Shift(1));
            for(int i=0;i<xs.Length;i++){probes.Add(xs[i]);if(i+1<xs.Length)probes.Add(xs[i].Plus(xs[i+1]).Half());}
        }
        return probes.All(x=>candidate.Has(x)==cs.All(c=>c.Holds(x)));
    }
    private static bool RootHolds(int x,IReadOnlyDictionary<string,int> p)
    {
        long right=p["a"]*(long)x+p["c"];
        long left=p["dir"]==1?(long)x*x+(p["a"]-p["r"]-p["s"])*(long)x+p["c"]+p["r"]*p["s"]:(p["a"]+1)*(long)x+p["c"]-p["r"];
        return left>0&&right>0&&left==right;
    }
    private static int[] Roots(IReadOnlyDictionary<string,int> p)=>(p["dir"]==1?new[]{p["r"],p["s"]}:new[]{p["r"]}).Where(x=>p["a"]*x+p["c"]>0).OrderBy(x=>x).ToArray();
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid bounded logarithm equation parameters.");
        if(f==Direct)
        {
            int sign=p["dir"]==1?1:-1;
            return (p["op"] switch{0=>Power(p["b"],p["n"]*sign),1=>Q.Make(p["n"]*sign,p["m"]),2=>Power(p["b"],(1+p["z"]%3)*sign),_=>Q.Make(137*p["n"]*sign,60)}).ToString();
        }
        if(f==Quadratic){int[] roots=Roots(p);return roots.Length==0?"∅":string.Join(",",roots);}
        if(f is OneSide or TwoSides)return Solve(Conditions(f,p)).Text();
        if(f==Ratio)return Math.Pow(10,p["dir"]==1?p["z"]/10.0:p["m"]).ToString("0.00",CultureInfo.InvariantCulture);
        int v=f switch{Wind=>p["dir"]==1?Pow(10,p["z"]):(int)Math.Round(10*p["b"]*Math.Log10(10+13*p["z"]+7*Math.Abs(p["c"]))+10*p["m"],MidpointRounding.AwayFromZero),Music=>12*Math.Abs(p["n"])+1,Sound=>10*(p["n"]+6),_=>throw new InvalidOperationException()};
        return v.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(answer))return false;string raw=Norm(answer);
        if(f==Direct)
        {
            if(!TryQ(raw,out Q q))return false;
            int sign=p["dir"]==1?1:-1;
            if(p["op"]==1)return new BigInteger(q.N)*p["m"]*sign==new BigInteger(q.D)*p["n"];
            if(p["op"]==3){var sum=Q.Make(0);for(int i=1;i<=5;i++)sum=sum.Plus(Q.Make(p["n"]*sign,i));return q.CompareTo(sum)==0;}
            if(q.N<=0)return false;
            int k=(p["op"]==0?p["n"]:1+p["z"]%3)*sign;
            return k>=0?new BigInteger(q.N)==new BigInteger(q.D)*Pow(p["b"],k):new BigInteger(q.N)*Pow(p["b"],-k)==q.D;
        }
        if(f==Quadratic)
        {
            var expected=Enumerable.Range(-5,11).Where(x=>RootHolds(x,p)).OrderBy(x=>x).ToArray();
            if(raw is "∅" or "{}" or "empty")return expected.Length==0;
            if(raw.StartsWith('{')&&raw.EndsWith('}'))raw=raw[1..^1];
            var parts=raw.Split(',');if(parts.Length is <1 or >2)return false;
            var actual=new List<int>();foreach(var part in parts){if(!int.TryParse(part,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int x)||x is <-5 or >5||!RootHolds(x,p))return false;actual.Add(x);}
            return actual.Distinct().Count()==actual.Count&&actual.OrderBy(x=>x).SequenceEqual(expected);
        }
        if(f is OneSide or TwoSides)return TryInterval(raw,out I candidate)&&VerifyInterval(candidate,Conditions(f,p));
        if(f==Ratio)
        {
            if(!decimal.TryParse(raw,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal dec)||dec<1||dec>1000||dec*100!=decimal.Truncate(dec*100))return false;
            int k=(int)(dec*100);BigInteger actual=BigInteger.Pow(10,p["dir"]==1?p["z"]:10*p["m"])*BigInteger.Pow(200,10);
            return BigInteger.Pow(2*k-1,10)<=actual&&actual<BigInteger.Pow(2*k+1,10);
        }
        if(!int.TryParse(raw,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int v))return false;
        if(f==Wind)
        {
            if(p["dir"]==1)return v==Pow(10,p["z"]);
            if(v<10*p["m"]+1||v>250)return false;
            int a=10*p["b"],d=10+13*p["z"]+7*Math.Abs(p["c"]),k=2*(v-10*p["m"]);
            BigInteger exact=BigInteger.Pow(d,2*a);
            return BigInteger.Pow(10,k-1)<=exact&&exact<BigInteger.Pow(10,k+1);
        }
        if(f==Music)return v>=1&&(v-1)%12==0&&(v-1)/12==Math.Abs(p["n"]);
        return f==Sound&&v%10==0&&v/10-12==p["n"]-6;
    }
}
