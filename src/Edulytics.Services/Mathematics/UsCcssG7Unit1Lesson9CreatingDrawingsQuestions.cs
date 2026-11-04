using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 7 U01:L09 first-party exact scale plans: original measured lengths,
/// drawing lengths and differently-sized representations of the same object.
/// The learner's actual floor-plan construction is still teacher-reviewed.
/// </summary>
internal static class UsCcssG7Unit1Lesson9CreatingDrawingsQuestions
{
    public const string Family="usccss.g7.u01.l09.create_drawing_scale_lengths";
    // L13 optional human-reviewed capstone: numeric paper-size and proportional
    // dimensions complement (but cannot certify) the physical floor-plan task.
    public const string FloorPlan="usccss.g7.u01.l13.floor_plan_scale_fit";
    internal sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family) =>
        family is Family or FloorPlan;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G7 scale drawing family.");
        if(family==FloorPlan) return BuildFloorPlan(random);
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["kind"]=random.Next(0,4),
            ["feature"]=random.Next(0,4),
            ["style"]=random.Next(0,2)
        };
        int k=p["kind"],f=p["feature"],s=p["style"];
        string prompt=k switch
        {
            0 => "A first-party bedroom floor plan represents 4 metres " +
                "by 16 centimetres at the initial scale. At the " +
                (s==0 ? "same scale (4 cm on drawing per real metre)"
                      : "reduced scale (2 cm on drawing per real metre)") +
                ", how many centimetres long is " +
                new[]{"a 4-metre wall","a 2.5-metre wall","a 2.75-metre wall",
                      "a 3.75-metre wall"}[f] + "?",
            1 => "An original outline of a region has " +
                new[]{"width 270 miles","height 350 miles",
                      "notch width 110 miles","notch height 70 miles"}[f] +
                ". Create a scale drawing at " + (s==0?"1 cm to 50 miles":"1 cm to 75 miles")+
                ". How many centimetres long is that feature?",
            2 => "A drawing of a book has a width of " + (s==0?"1.5":"3")+
                " inches, while its real width is 9 inches. " +
                (f switch
                {
                    0 => "A corresponding diagram height is " +
                        (s==0?"3":"6") + " inches. What is the REAL book height in inches?",
                    1 => "How many REAL inches correspond to 1 drawing inch?",
                    2 => "How many drawing inches represent the REAL width of 9 inches?",
                    _ => "How many drawing inches represent a REAL height of 18 inches?"
                }),
            _ => "A 6-foot-wide flag with three stripes (top 2 feet tall; " +
                 "middle 1 foot; bottom 1 foot) is to be drawn at " +
                 (s==0?"1 cm to 2 feet":"2 cm to 1 foot")+". How many drawing " +
                 "centimetres represent "+new[]{"its 6-foot width","the 2-foot top stripe",
                 "the 1-foot middle stripe","the 1-foot bottom stripe"}[f]+"?"
        };
        return new(Family,prompt+" Enter the exact value in the requested units; " +
            "a reduced equivalent fraction is allowed.",
            "A scale is a ratio between DRAWING length and ACTUAL length in " +
            "different stated units; use one ratio for every corresponding " +
            "feature. The same real object is drawn smaller at a scale that " +
            "represents more actual length per cm. The produced diagram, " +
            "placement of parts, annotations and peer critique remain teacher-reviewed.",
            AssessmentItemType.ShortAnswer,p);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G7 L09 parameters.");
        var (n,d)=family==FloorPlan?ExpectedFloorPlan(p):Expected(p);
        return Fraction(n,d);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)
            return false;
        if(!TryFraction(answer.Trim(),out long n,out long d))
            return false;
        var (wantN,wantD)=family==FloorPlan?ExpectedFloorPlan(p):Expected(p);
        return n*wantD==wantN*d;
    }

    private static (long N,long D) Expected(IReadOnlyDictionary<string,int> p)
    {
        int kind=p["kind"],feature=p["feature"],style=p["style"];
        if(kind==0)
        {
            long[] quarterMetres=[16,10,11,15];
            return (quarterMetres[feature]*(style==0?4:2),4);
        }
        if(kind==1)
        {
            long[] miles=[270,350,110,70];
            return (miles[feature],style==0?50:75);
        }
        if(kind==2)
        {
            int inchesPerDrawInch=style==0?6:3;
            return feature switch
            {
                0 => (18,1),
                1 => (inchesPerDrawInch,1),
                2 => (9,inchesPerDrawInch),
                _ => (18,inchesPerDrawInch)
            };
        }
        long[] feet=[6,2,1,1];
        return (feet[feature]*(style==0?1:2),style==0?2:1);
    }


    private static Problem BuildFloorPlan(Random random)
    {
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["kind"]=random.Next(0,4),
            ["feature"]=random.Next(0,4),
            ["style"]=random.Next(0,2)
        };
        int kind=p["kind"],feature=p["feature"],style=p["style"];
        string question=kind switch
        {
            0 => "An original Edulytiks classroom is 8 m by 6 m with a 1-m "+
                 "door opening and a 3-m wall board. Use 1 cm on drawing to "+
                 (style==0?"1":"2")+" actual m. How many centimetres on "+
                 "the PLAN should "+new[]{"the 8-m length","the 6-m width",
                         "the 1-m door opening","the 3-m board"}[feature]+
                 " measure? Give an exact numeric answer.",
            1 => "A rectangular 10-by-8-m room must fit within "+
                 new[]{"20 by 15","21 by 29","15 by 15","10 by 10"}[feature]+
                 " centimetres of usable paper. Is a plan at 1 drawing cm "+
                 "to "+(style==0?"0.5":"1")+" actual m feasible "+
                 "WITHOUT cropping either dimension? Enter 1=yes or 0=no. "+
                 "A teacher must review the real student's layout.",
            2 => "A floor covering uses square tiles with sides "+
                 new[]{"5","8","10","12"}[feature]+" inches. "+
                 "Switch to tiles with DOUBLE the side length. "+
                 (style==0?"What FRACTION of the old tile count covers the SAME AREA "+
                            "ignoring cutting and waste?":
                           "How many times the ORIGINAL AREA does ONE new tile cover?")+
                 " Give an exact fraction or integer.",
            _ => "A real classroom is 8 by 6 metres, drawn at 1 plan cm to "+
                 (style==0?"1":"2")+" actual m. "+
                 new[]{"What is the REAL room area in square meters?",
                       "What is the DRAWN floor-plan area in square centimetres?",
                       "How many actual square meters does 1 drawn square centimetre represent?",
                       "What is the DRAWN floor-plan perimeter in centimetres?"}[feature]+
                 " Explain why its measured geometry still needs teacher review."
        };
        return new(FloorPlan,question,
            "Record actual dimensions first, choose one consistent scale "+
            "that fits the paper, and check each corresponding measured feature. "+
            "Doubling tile side length quadruples tile area. Numeric estimates "+
            "do not certify drawing accuracy, placements, handwriting or peer critique.",
            AssessmentItemType.ShortAnswer,p);
    }

    private static (long N,long D) ExpectedFloorPlan(IReadOnlyDictionary<string,int> p)
    {
        int k=p["kind"],f=p["feature"],s=p["style"];
        if(k==0)
        {
            long[] lengths=[8,6,1,3];
            return (lengths[f],s==0?1:2);
        }
        if(k==1)
        {
            int[][] available=[[20,15],[21,29],[15,15],[10,10]];
            int drawnLong=s==0?20:10,drawnShort=s==0?16:8;
            return (drawnLong<=available[f][0]&&drawnShort<=available[f][1]?1:0,1);
        }
        if(k==2)return s==0?(1,4):(4,1);
        return f switch
        {
            0 => (48,1),
            1 => s==0?(48,1):(12,1),
            2 => s==0?(1,1):(4,1),
            _ => s==0?(28,1):(14,1)
        };
    }

    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p) =>
        Supports(family)&&p is not null&&p.Count==3 &&
        p.TryGetValue("kind",out int kind)&&kind is >=0 and <=3 &&
        p.TryGetValue("feature",out int feature)&&feature is >=0 and <=3 &&
        p.TryGetValue("style",out int style)&&style is 0 or 1;

    private static string Fraction(long n,long d)
    {
        long g=Gcd(n,d);n/=g;d/=g;
        return d==1?n.ToString(CultureInfo.InvariantCulture):
            n.ToString(CultureInfo.InvariantCulture)+"/"+
            d.ToString(CultureInfo.InvariantCulture);
    }
    private static long Gcd(long a,long b)
    {
        while(b!=0){long t=a%b;a=b;b=t;}
        return a;
    }

    private static bool TryFraction(string value,out long n,out long d)
    {
        n=0;d=1;
        var split=value.Split('/');
        if(split.Length==2)
            return long.TryParse(split[0],NumberStyles.None,
                       CultureInfo.InvariantCulture,out n) &&
                   long.TryParse(split[1],NumberStyles.None,
                       CultureInfo.InvariantCulture,out d) &&
                   n>=0 && n<=1000000 && d>0 && d<=1000000;
        if(split.Length!=1 || !decimal.TryParse(value,NumberStyles.AllowDecimalPoint,
              CultureInfo.InvariantCulture,out decimal x) || x<0m || x>1000000m)
            return false;
        for(int i=0;i<=6;i++)
        {
            long denom=(long)Math.Pow(10,i);
            decimal scaled=x*denom;
            if(scaled==decimal.Truncate(scaled))
            {
                n=(long)scaled;d=denom;return true;
            }
        }
        return false;
    }
}
