using System.Globalization;
using System.Numerics;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10ProbabilityOdds(string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int a=random.Next(2,9),b=random.Next(2,9),c=random.Next(2,9);
        int r=random.Next(2,6),u=random.Next(0,Math.Min(a,r)+1),v=random.Next(0,Math.Min(b,r-u)+1);
        // Ensure the selected composition is feasible, including zeros and boundary events.
        while(r-u-v>c){u=random.Next(0,Math.Min(a,r)+1);v=random.Next(0,Math.Min(b,r-u)+1);}
        int k=random.Next(0,r+1),dir=random.Next(2),mode=random.Next(4),pct=random.Next(0,1001),n=random.Next(4,11);
        string odds="اكتب النسبة بالترتيب المطلوب أ:ب، في أبسط صورة.";
        string prob="اكتب الاحتمال كعدد صحيح أو كسر.";
        string side=dir==0?"لصالح":"ضد";
        string single=mode switch{0=>$"كيس به {a} حمراء و{b} زرقاء و{c} خضراء. ما احتمال سحب حمراء أو خضراء؟",1=>$"كيس به {a} حمراء و{b} زرقاء و{c} خضراء. ما احتمال ألا تكون الكرة المسحوبة زرقاء؟",2=>$"كيس به {a} حمراء و{b} زرقاء و{c} خضراء. ما احتمال سحب صفراء؟",_=>$"كيس به {a} حمراء و{b} زرقاء و{c} خضراء. ما احتمال سحب كرة من الألوان الثلاثة؟"};
        string prompt=family switch{
            UaeG10ProbabilityOddsResponse.Single=>$"{single} {prob}",
            UaeG10ProbabilityOddsResponse.OddsCounts=>$"فضاء عينة متساوي الاحتمال يضم {(mode==2?0:a)} نواتج ملائمة و{(mode==3?0:b)} غير ملائمة. احسب الفرص {side} الحدث. {odds}",
            UaeG10ProbabilityOddsResponse.FromOdds=>$"الفرص لصالح حدث هي {(mode==2?0:a)}:{(mode==3?0:b)}. احسب احتمال {(dir==0?"وقوعه":"عدم وقوعه")}. {prob}",
            UaeG10ProbabilityOddsResponse.FromDecimal=>$"احتمال وقوع حدث هو {((decimal)pct/1000).ToString("0.000",CultureInfo.InvariantCulture)}. أوجد الفرص {side} الحدث. {odds}",
            UaeG10ProbabilityOddsResponse.ExactSample=>$"من {a+b} قطعة مميزة منها {a} حمراء اختر {r} دون إرجاع بانتظام. ما احتمال اختيار {k} حمراء بالضبط؟ {prob}",
            UaeG10ProbabilityOddsResponse.SampleOdds=>$"من {a+b} قطعة مميزة منها {a} حمراء اختر {r} دون إرجاع بانتظام. ما الفرص {side} اختيار {k} حمراء بالضبط؟ {odds}",
            UaeG10ProbabilityOddsResponse.AtLeast=>$"من {a+b} قطعة مميزة منها {a} حمراء اختر {r} دون إرجاع بانتظام. ما احتمال اختيار حمراء واحدة على الأقل؟ {prob}",
            UaeG10ProbabilityOddsResponse.Composition=>$"من {a} حمراء و{b} زرقاء و{c} خضراء اختر {r} دون إرجاع بانتظام. ما احتمال {u} حمراء و{v} زرقاء و{r-u-v} خضراء بالضبط؟ {prob}",
            UaeG10ProbabilityOddsResponse.CompositionOdds=>$"من {a} حمراء و{b} زرقاء و{c} خضراء اختر {r} دون إرجاع بانتظام. ما الفرص {side} اختيار {u} حمراء و{v} زرقاء و{r-u-v} خضراء بالضبط؟ {odds}",
            UaeG10ProbabilityOddsResponse.DifferentPair=>$"من {a} حمراء و{b} زرقاء و{c} خضراء اختر كرتين دون إرجاع بانتظام. ما الفرص {side} اختيار لونين مختلفين؟ {odds}",
            UaeG10ProbabilityOddsResponse.SuitPattern=>$"رزمة تعليمية بها {3+c%3} مجموعات منفصلة متساوية، كل مجموعة {n+3} بطاقات مميزة. نسحب 5 دون إرجاع بانتظام. أوجد الفرص {side} وجود 3 من مجموعة واحدة و2 من مجموعة أخرى. {odds}",
            UaeG10ProbabilityOddsResponse.Repeat=>$"مكعب تعليمي عادل ذو {n} نواتج متساوية يرمى {2+mode%2} مرات مستقلة. ما احتمال ظهور ناتج محدد في كل مرة؟ {prob}",
            UaeG10ProbabilityOddsResponse.Ordered=>$"نختار ترتيبًا من ثلاث خانات باستعمال {n} رموز متساوية الاحتمال، {(mode%2==0?"دون تكرار":"مع السماح بالتكرار")}. ما احتمال مطابقة ترتيب محدد من ثلاثة رموز مختلفة؟ {prob}",
            UaeG10ProbabilityOddsResponse.OrderedOdds=>$"نختار ترتيبًا من ثلاث خانات باستعمال {n} رموز متساوية الاحتمال، {(mode%2==0?"دون تكرار":"مع السماح بالتكرار")}. ما الفرص {side} مطابقة ترتيب محدد من ثلاثة رموز مختلفة؟ {odds}",
            UaeG10ProbabilityOddsResponse.Conditional=>$"تجربتان مستقلتان؛ في كل تجربة {a} نواتج نجاح و{b} فشل متساوية الاحتمال. ما احتمال نجاح التجربتين {(dir==0?"معًا؟":"إذا عُلم أن الأولى نجحت؟")} {prob}",
            UaeG10ProbabilityOddsResponse.Cut=>$"نختار نقطة بانتظام على قطعة طولها {10*(a+b)} سم. ما احتمال أن يكون الجزء الأطول على الأقل {n} أمثال الأقصر؟ {prob}",
            UaeG10ProbabilityOddsResponse.Data=>$"في سجل افتراضي لفئات منفصلة: أ={10*a}، ب={10*b}، ج={10*c}. احسب التكرار النسبي للفئتين أ أو ب. {prob}",
            UaeG10ProbabilityOddsResponse.DataOdds=>$"في سجل افتراضي لفئات منفصلة: أ={10*a}، ب={10*b}، ج={10*c}. أوجد الفرص {side} الفئة ج عند اختيار سجل واحد بانتظام. {odds}",
            _=>throw new InvalidOperationException()
        };
        return Problem(family,prompt,"حدد الفضاء المتساوي الاحتمال، احسب الملائم والمتمم؛ استعمل التوافيق دون ترتيب أو التباديل عند ترتيب النتائج. الاحتمال ملائم/كلي، والفرص نسبة ملائم إلى غير ملائم أو العكس.",
            AssessmentItemType.ShortAnswer,("a",a),("b",b),("c",c),("r",r),("u",u),("v",v),("k",k),("dir",dir),("mode",mode),("pct",pct),("n",n));
    }
}
internal static class UaeG10ProbabilityOddsResponse
{
    internal const string Prefix="uae.g10.general.u16.l05.";
    internal const string Single=Prefix+"single_event_boundary",OddsCounts=Prefix+"odds_from_counts",FromOdds=Prefix+"probability_from_odds",FromDecimal=Prefix+"odds_from_decimal",ExactSample=Prefix+"hypergeometric_probability",SampleOdds=Prefix+"hypergeometric_odds",AtLeast=Prefix+"at_least_one_probability",Composition=Prefix+"three_category_probability",CompositionOdds=Prefix+"three_category_odds",DifferentPair=Prefix+"different_pair_odds",SuitPattern=Prefix+"two_suit_pattern_odds",Repeat=Prefix+"repeated_result_probability",Ordered=Prefix+"ordered_pattern_probability",OrderedOdds=Prefix+"ordered_pattern_odds",Conditional=Prefix+"conditional_independent_probability",Cut=Prefix+"uniform_cut_probability",Data=Prefix+"data_union_probability",DataOdds=Prefix+"data_category_odds";
    internal static bool Supports(string? f)=>f is Single or OddsCounts or FromOdds or FromDecimal or ExactSample or SampleOdds or AtLeast or Composition or CompositionOdds or DifferentPair or SuitPattern or Repeat or Ordered or OrderedOdds or Conditional or Cut or Data or DataOdds;
    internal static bool IsOdds(string f)=>f is OddsCounts or FromDecimal or SampleOdds or CompositionOdds or DifferentPair or SuitPattern or OrderedOdds or DataOdds;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==11&&new[]{"a","b","c","r","u","v","k","dir","mode","pct","n"}.All(p.ContainsKey)&&
        p["a"] is >=2 and <=8&&p["b"] is >=2 and <=8&&p["c"] is >=2 and <=8&&p["r"] is >=2 and <=5&&p["r"]<=p["a"]+p["b"]&&p["u"]>=0&&p["u"]<=Math.Min(p["a"],p["r"])&&p["v"]>=0&&p["v"]<=Math.Min(p["b"],p["r"]-p["u"])&&p["r"]-p["u"]-p["v"]<=p["c"]&&p["k"]>=0&&p["k"]<=p["r"]&&p["dir"] is 0 or 1&&p["mode"] is >=0 and <=3&&p["pct"] is >=0 and <=1000&&p["n"] is >=4 and <=10;
    private static BigInteger Choose(int n,int k)
    {
        if(k<0||k>n)return 0;BigInteger result=1;
        for(int i=1;i<=k;i++)result=result*(n-i+1)/i;
        return result;
    }
    // Independent count via Pascal recurrence rather than the multiplicative formula.
    private static BigInteger Pascal(int n,int k)
    {
        if(k<0||k>n)return 0;
        var row=new BigInteger[k+1];row[0]=1;
        for(int i=1;i<=n;i++)for(int j=Math.Min(i,k);j>=1;j--)row[j]+=row[j-1];
        return row[k];
    }
    private static (BigInteger S,BigInteger T) Counts(string f,IReadOnlyDictionary<string,int> p,bool independent)
    {
        int a=p["a"],b=p["b"],c=p["c"],r=p["r"],u=p["u"],v=p["v"],k=p["k"],mode=p["mode"],n=p["n"];
        BigInteger s=0,t=0;
        Func<int,int,BigInteger> choose=independent?Pascal:Choose;
        switch(f)
        {
            case Single:t=a+b+c;s=mode switch{0=>a+c,1=>a+c,2=>0,_=>a+b+c};break;
            case OddsCounts:case FromOdds:s=mode==2?0:a;t=s+(mode==3?0:b);break;
            case FromDecimal:s=p["pct"];t=1000;break;
            case ExactSample:case SampleOdds:t=choose(a+b,r);s=choose(a,k)*choose(b,r-k);break;
            case AtLeast:
                t=choose(a+b,r);
                if(independent){for(int i=1;i<=r;i++)s+=choose(a,i)*choose(b,r-i);}
                else s=t-choose(b,r);
                break;
            case Composition:case CompositionOdds:t=choose(a+b+c,r);s=choose(a,u)*choose(b,v)*choose(c,r-u-v);break;
            case DifferentPair:
                t=choose(a+b+c,2);s=independent?t-choose(a,2)-choose(b,2)-choose(c,2):a*b+a*c+b*c;break;
            case SuitPattern:
                int g=3+c%3,h=n+3;t=choose(g*h,5);
                if(independent){for(int i=0;i<g;i++)for(int j=i+1;j<g;j++)s+=2*choose(h,3)*choose(h,2);}
                else s=g*(g-1)*choose(h,3)*choose(h,2);
                break;
            case Repeat:s=1;t=BigInteger.Pow(n,2+mode%2);break;
            case Ordered:case OrderedOdds:s=1;t=mode%2==0?n*(n-1)*(n-2):n*n*n;break;
            case Conditional:
                s=p["dir"]==0?a*a:a;t=p["dir"]==0?(a+b)*(a+b):a+b;break;
            case Cut:s=2;t=n+1;break;
            case Data:s=a+b;t=a+b+c;break;
            case DataOdds:s=c;t=a+b+c;break;
            default:throw new InvalidOperationException();
        }
        // Only these families explicitly ask for the complement or the reverse odds.
        if((IsOdds(f)||f==FromOdds)&&p["dir"]==1)s=t-s;
        return (s,t);
    }
    private static string Text(BigInteger a,BigInteger b,bool odds)
    {
        BigInteger g=BigInteger.GreatestCommonDivisor(a,b);a/=g;b/=g;
        if(odds)return a.ToString(CultureInfo.InvariantCulture)+":"+b.ToString(CultureInfo.InvariantCulture);
        return b==1?a.ToString(CultureInfo.InvariantCulture):a.ToString(CultureInfo.InvariantCulture)+"/"+b.ToString(CultureInfo.InvariantCulture);
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid bounded probability parameters.");
        var (s,t)=Counts(f,p,false);return Text(s,IsOdds(f)?t-s:t,IsOdds(f));
    }
    private static string Norm(string answer)=>string.Concat(answer.Trim().Select(ch=>ch switch{>= '٠' and <= '٩'=>(char)('0'+ch-'٠'),>= '۰' and <= '۹'=>(char)('0'+ch-'۰'),'∶' or '：'=>':','⁄' or '∕'=>'/',_=>ch})).Replace(" ","");
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>80)return false;
        string raw=Norm(answer);bool odds=IsOdds(f);var parts=raw.Split(odds?':':'/');
        if(odds?parts.Length!=2:parts.Length is <1 or >2)return false;
        if(!BigInteger.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out BigInteger num))return false;
        BigInteger den=1;if(parts.Length==2&&!BigInteger.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out den))return false;
        if(odds?num+den<=0:den<=0||num>den)return false;
        var (s,t)=Counts(f,p,true);BigInteger expectedDen=odds?t-s:t;
        return num*expectedDen==den*s;
    }
}
