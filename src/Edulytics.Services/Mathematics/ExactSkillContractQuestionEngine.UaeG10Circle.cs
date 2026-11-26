using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// Source-specific families extend the existing ExactSkillContractQuestionEngine.
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10Circle(string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        if(UaeG10CircleResponse.IsTangent(family))
        {
            int k=random.Next(1,9),x=random.Next(2,16),a=random.Next(2,10),b=a+random.Next(2,7),c=random.Next(2,12),mode=random.Next(0,5),yes=random.Next(0,2);
            int distance=mode switch{0=>a+b+1,1=>a+b,2=>b,3=>b-a,_=>b-a-1};
            string prompt=family switch
            {
                UaeG10CircleResponse.TangentCount=>$"دائرتان مختلفتان نصفا قطريهما {a} و{b} سم والمسافة بين مركزيهما {distance} سم. كم مماسًا مشتركًا لهما؟",
                UaeG10CircleResponse.TangentCheck=>$"T نقطة على دائرة مركزها O. إذا كان OT={3*k} وPT={4*k} وOP={5*k+(1-yes)} سم، فهل PT مماس؟ اكتب 1 لنعم أو 0 للا.",
                UaeG10CircleResponse.TangentLength=>$"نصف قطر دائرة {3*k} سم وبُعد نقطة خارجية عن المركز {5*k} سم. أوجد طول قطعة المماس.",
                UaeG10CircleResponse.TangentRadius=>$"مماس طوله {4*k} سم من P خارج دائرة. يمر OP بأقرب نقطة A من الدائرة، وPA={2*k} سم. أوجد نصف القطر.",
                UaeG10CircleResponse.TangentCenter=>$"طول المماس من P إلى دائرة {4*k} سم ونصف قطرها {3*k} سم. أوجد بُعد P عن المركز.",
                UaeG10CircleResponse.TangentEqual=>$"قطعتان مماستان من نقطة خارج دائرة، طول إحداهما {a*k} سم. أوجد طول الأخرى.",
                UaeG10CircleResponse.TangentExpression=>$"طولا مماسين من النقطة الخارجية نفسها هما {a}س+{c} و{a*x+c}. أوجد س.",
                UaeG10CircleResponse.TangentPerimeter=>$"مثلث محيط بدائرة؛ أطوال قطع المماس من رؤوسه الثلاثة إلى نقاط التماس المجاورة {a} و{b} و{c} سم. أوجد محيط المثلث.",
                _=>$"رباعي محيط بدائرة ABCD حيث AB={a+b} وBC={b+c} وCD={c+k} سم. أوجد DA."
            };
            return Problem(family,prompt,"المماس عمودي على نصف القطر عند التماس؛ القطعتان من نقطة خارجية واحدة متساويتان؛ اجمع قطع المماس المتساوية للمحيط.",AssessmentItemType.Numeric,("k",k),("x",x),("a",a),("b",b),("c",c),("mode",mode),("yes",yes));
        }
        if(UaeG10CircleResponse.IsInscribed(family))
        {
            int v=random.Next(20,81),x=random.Next(2,16),k=random.Next(1,9),a=random.Next(2,5),b=random.Next(1,6);
            string prompt=family switch
            {
                UaeG10CircleResponse.InscribedHalf=>$"زاوية محيطية تحصر قوسًا قياسه {2*v}°. أوجد قياس الزاوية.",
                UaeG10CircleResponse.InscribedDouble=>$"زاوية محيطية قياسها {v}°. أوجد قياس قوسها المحصور.",
                UaeG10CircleResponse.InscribedEqual=>$"زاويتان محيطيتان تحصران القوس نفسه، قياس إحداهما {v}°. أوجد الأخرى.",
                UaeG10CircleResponse.InscribedRight=>$"مثلث محاط بدائرة، أحد أضلاعه قطر، وقياس إحدى الزاويتين الحادتين {v}°. أوجد الحادة الأخرى.",
                UaeG10CircleResponse.InscribedOpposite=>$"في رباعي محاط بدائرة قياس زاوية {2*v}°. أوجد قياس الزاوية المقابلة لها.",
                UaeG10CircleResponse.InscribedAlgebra=>$"زاويتان محيطيتان تحصران القوس نفسه، قياسهما ({a}س+{b})° و({a*x+b})°. أوجد س.",
                UaeG10CircleResponse.InscribedCyclicAlgebra=>$"زاويتان متقابلتان في رباعي دائري قياسهما ({a}س+{b})° و({180-a*x-b})°. أوجد س.",
                _=>$"مثلث محاط بدائرة، وتره قطرها وطولا ضلعي الزاوية القائمة {3*k} و{4*k} سم. أوجد قطر الدائرة."
            };
            return Problem(family,prompt,"الزاوية المحيطية نصف القوس المحصور؛ زاوية القطر قائمة؛ الزاويتان المتقابلتان في الرباعي الدائري مجموعهما 180°.",AssessmentItemType.Numeric,("v",v),("x",x),("k",k),("a",a),("b",b));
        }
        if(UaeG10CircleResponse.IsChord(family))
        {
            int k=random.Next(1,9), arc=2*random.Next(10,86), x=random.Next(2,21), a=random.Next(2,7), b=random.Next(1,10), radius=random.Next(6,31), half=random.Next(1,radius);
            string prompt=family switch
            {
                UaeG10CircleResponse.ChordEqualArc=>$"في دائرة، وتران متطابقان. قياس القوس الأصغر المقابل للأول {arc}°. أوجد قياس القوس الأصغر المقابل للثاني.",
                UaeG10CircleResponse.ChordHalfArc=>$"قطر عمودي على وتر ينصف القوس الأصغر المقابل له، الذي قياسه {arc}°. أوجد قياس نصف القوس.",
                UaeG10CircleResponse.ChordLength=>$"نصف قطر دائرة {5*k} سم، والمسافة العمودية من المركز إلى وتر {3*k} سم. أوجد طول الوتر.",
                UaeG10CircleResponse.ChordDistance=>$"نصف قطر دائرة {5*k} سم وطول وتر فيها {8*k} سم. أوجد بعد الوتر عن المركز.",
                UaeG10CircleResponse.ChordAlgebra=>$"وتران متطابقان في دائرة، طول أحدهما {a}س + {b} وطول الآخر {a*x+b}. أوجد س.",
                UaeG10CircleResponse.ChordEqualDistance=>$"وتران متطابقان في دائرة. بعد الوتر الأول عن المركز {3*k} سم. أوجد بعد الثاني عن المركز.",
                _=>$"نصف قطر دائرة {radius} سم، وطول وتر فيها {2*half} سم. أوجد البعد العمودي للوتر عن المركز مقربًا لأقرب جزء من مائة."
            };
            return Problem(family,prompt,"الأوتار المتطابقة تقابل أقواسًا متطابقة وتبعد المسافة نفسها عن المركز. العمود من المركز ينصف الوتر والقوس؛ استخدم فيثاغورس.",family==UaeG10CircleResponse.ChordApprox?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("k",k),("arc",arc),("x",x),("a",a),("b",b),("radius",radius),("half",half));
        }
        if(UaeG10CircleResponse.IsArc(family))
        {
            int a1=random.Next(10,171),a2=random.Next(10,171),r=random.Next(2,31),angle=random.Next(10,351),percent=5*random.Next(1,20);
            string prompt=family switch
            {
                UaeG10CircleResponse.Central=>$"ثلاث زوايا مركزية غير متداخلة تكمل دورة. قياس اثنتين {a1}° و{a2}°. أوجد الثالثة بالدرجات.",
                UaeG10CircleResponse.Major=>$"قوس صغير قياسه {a1}°. أوجد قياس القوس الكبير المتمم للدورة بالدرجات.",
                UaeG10CircleResponse.Pie=>$"قطاع يمثل {percent}٪ من بيانات مخطط دائري. أوجد قياس زاويته المركزية بالدرجات.",
                UaeG10CircleResponse.ArcSum=>$"قوسان متجاوران غير متداخلين قياساهما {a1}° و{a2}°. أوجد قياس القوس المكون منهما.",
                UaeG10CircleResponse.ArcCoeff=>$"دائرة نصف قطرها {r} سم، وقوس قياسه {angle}°. اكتب معامل π في طول القوس (عددًا أو كسرًا).",
                UaeG10CircleResponse.ArcApprox=>$"دائرة نصف قطرها {r} سم، وقوس قياسه {angle}°. أوجد طول القوس بالسنتيمتر مقربًا لأقرب جزء من مائة.",
                _=>$"قوس طوله {percent}/100 من محيط الدائرة. أوجد قياس القوس بالدرجات."
            };
            return Problem(family,prompt,"مجموع دورة كاملة 360°؛ طول القوس = (قياسه/360)×2π×نصف القطر، والقياس الزاوي يختلف عن الطول.",family is UaeG10CircleResponse.ArcCoeff or UaeG10CircleResponse.ArcApprox?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("a1",a1),("a2",a2),("r",r),("angle",angle),("percent",percent));
        }
        int a=random.Next(2,31),b=random.Next(2,31),distance=random.Next(0,61),k=random.Next(1,11);
        if(family==UaeG10CircleResponse.Intersection&&distance==0&&a==b)distance=1;
        if(family==UaeG10CircleResponse.Overlap)distance=random.Next(Math.Abs(a-b)+1,a+b);
        string prompt=family switch
        {
            UaeG10CircleResponse.Diameter=>$"نصف قطر دائرة {a} سم. أوجد قطرها بالسنتيمتر.",
            UaeG10CircleResponse.Radius=>$"قطر دائرة {a} سم. أوجد نصف قطرها، واكتب عددًا صحيحًا أو كسرًا.",
            UaeG10CircleResponse.Circumference=>$"نصف قطر دائرة {a} سم. إذا كان محيطها م×π سم، فأوجد المعامل م.",
            UaeG10CircleResponse.Approx=>$"قطر دائرة {a} سم. أوجد محيطها بالسنتيمتر باستخدام π، مقربًا إلى أقرب جزء من مائة.",
            UaeG10CircleResponse.Overlap=>$"دائرتان نصفا قطريهما {a} و{b} سم والمسافة بين مركزيهما {distance} سم. أوجد طول الجزء المشترك من خط المركزين بالسنتيمتر.",
            UaeG10CircleResponse.Intersection=>$"دائرتان مختلفتان نصفا قطريهما {a} و{b} سم والمسافة بين مركزيهما {distance} سم. كم نقطة تقاطع مشتركة على المحيطين؟",
            UaeG10CircleResponse.Rectangle=>$"مستطيل ضلعاه {3*k} و{4*k} سم، رؤوسه الأربعة على دائرة. إذا كان محيط الدائرة م×π سم، فأوجد م.",
            _=>$"محيط دائرة {a}π سم. أوجد نصف قطرها، واكتب عددًا صحيحًا أو كسرًا."
        };
        return Problem(family,prompt,"حدد نصف القطر والقطر. المحيط π×القطر. قارن مسافة المركزين بمجموع نصفي القطرين والفرق بينهما؛ قطر الدائرة المحيطة بمستطيل يساوي قطر المستطيل.",family is UaeG10CircleResponse.Radius or UaeG10CircleResponse.Inverse or UaeG10CircleResponse.Approx?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("a",a),("b",b),("distance",distance),("k",k));
    }
}
internal static class UaeG10CircleResponse
{
    internal const string Diameter="uae.g10.general.u13.l01.diameter_from_radius";
    internal const string Radius="uae.g10.general.u13.l01.radius_from_diameter";
    internal const string Circumference="uae.g10.general.u13.l01.circumference_pi_coefficient";
    internal const string Approx="uae.g10.general.u13.l01.circumference_rounded";
    internal const string Overlap="uae.g10.general.u13.l01.center_line_overlap";
    internal const string Intersection="uae.g10.general.u13.l01.intersection_count";
    internal const string Rectangle="uae.g10.general.u13.l01.circumscribed_rectangle_pi_coefficient";
    internal const string Inverse="uae.g10.general.u13.l01.radius_from_circumference_pi";
    internal const string Central="uae.g10.general.u13.l02.central_remaining";
    internal const string Major="uae.g10.general.u13.l02.major_arc";
    internal const string Pie="uae.g10.general.u13.l02.pie_angle";
    internal const string ArcSum="uae.g10.general.u13.l02.adjacent_arc_sum";
    internal const string ArcCoeff="uae.g10.general.u13.l02.arc_length_pi_coefficient";
    internal const string ArcApprox="uae.g10.general.u13.l02.arc_length_rounded";
    internal const string ArcInverse="uae.g10.general.u13.l02.angle_from_arc_fraction";
    internal static bool IsArc(string? f)=>f is Central or Major or Pie or ArcSum or ArcCoeff or ArcApprox or ArcInverse;
    internal const string ChordEqualArc="uae.g10.general.u13.l03.equal_chord_arc";
    internal const string ChordHalfArc="uae.g10.general.u13.l03.perpendicular_bisects_arc";
    internal const string ChordLength="uae.g10.general.u13.l03.chord_length_from_radius_distance";
    internal const string ChordDistance="uae.g10.general.u13.l03.center_distance_from_chord";
    internal const string ChordAlgebra="uae.g10.general.u13.l03.equal_chord_expression";
    internal const string ChordEqualDistance="uae.g10.general.u13.l03.equal_chords_equal_distances";
    internal const string ChordApprox="uae.g10.general.u13.l03.center_distance_rounded";
    internal static bool IsChord(string? f)=>f is ChordEqualArc or ChordHalfArc or ChordLength or ChordDistance or ChordAlgebra or ChordEqualDistance or ChordApprox;
    internal const string InscribedHalf="uae.g10.general.u13.l04.angle_from_intercepted_arc";
    internal const string InscribedDouble="uae.g10.general.u13.l04.arc_from_inscribed_angle";
    internal const string InscribedEqual="uae.g10.general.u13.l04.equal_angles_same_arc";
    internal const string InscribedRight="uae.g10.general.u13.l04.diameter_right_triangle_angle";
    internal const string InscribedOpposite="uae.g10.general.u13.l04.cyclic_opposite_angle";
    internal const string InscribedAlgebra="uae.g10.general.u13.l04.same_arc_angle_expression";
    internal const string InscribedCyclicAlgebra="uae.g10.general.u13.l04.cyclic_opposite_expression";
    internal const string InscribedDiameter="uae.g10.general.u13.l04.right_triangle_diameter";
    internal static bool IsInscribed(string? f)=>f is InscribedHalf or InscribedDouble or InscribedEqual or InscribedRight or InscribedOpposite or InscribedAlgebra or InscribedCyclicAlgebra or InscribedDiameter;
    internal const string TangentCount="uae.g10.general.u13.l05.common_tangent_count";
    internal const string TangentCheck="uae.g10.general.u13.l05.tangent_right_triangle_check";
    internal const string TangentLength="uae.g10.general.u13.l05.tangent_length";
    internal const string TangentRadius="uae.g10.general.u13.l05.radius_from_external_segment";
    internal const string TangentCenter="uae.g10.general.u13.l05.center_distance";
    internal const string TangentEqual="uae.g10.general.u13.l05.equal_tangent_length";
    internal const string TangentExpression="uae.g10.general.u13.l05.equal_tangent_expression";
    internal const string TangentPerimeter="uae.g10.general.u13.l05.circumscribed_triangle_perimeter";
    internal const string TangentQuad="uae.g10.general.u13.l05.circumscribed_quadrilateral_side";
    internal static bool IsTangent(string? f)=>f is TangentCount or TangentCheck or TangentLength or TangentRadius or TangentCenter or TangentEqual or TangentExpression or TangentPerimeter or TangentQuad;
    internal static bool Supports(string? f)=>IsTangent(f)||IsInscribed(f)||IsChord(f)||IsArc(f)||f is Diameter or Radius or Circumference or Approx or Overlap or Intersection or Rectangle or Inverse;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>IsTangent(f)?
        p.Count==7&&new[]{"k","x","a","b","c","mode","yes"}.All(p.ContainsKey)&&p["k"] is >=1 and <=8&&p["x"] is >=2 and <=15&&p["a"] is >=2 and <=9&&p["b"]-p["a"] is >=2 and <=6&&p["c"] is >=2 and <=11&&p["mode"] is >=0 and <=4&&p["yes"] is >=0 and <=1:
        IsInscribed(f)?
        p.Count==5&&new[]{"v","x","k","a","b"}.All(p.ContainsKey)&&p["v"] is >=20 and <=80&&p["x"] is >=2 and <=15&&p["k"] is >=1 and <=8&&p["a"] is >=2 and <=4&&p["b"] is >=1 and <=5:
        IsChord(f)?
        p.Count==7&&new[]{"k","arc","x","a","b","radius","half"}.All(p.ContainsKey)&&p["k"] is >=1 and <=8&&p["arc"] is >=20 and <=170&&p["arc"]%2==0&&p["x"] is >=2 and <=20&&p["a"] is >=2 and <=6&&p["b"] is >=1 and <=9&&p["radius"] is >=6 and <=30&&p["half"]>=1&&p["half"]<p["radius"]:
        IsArc(f)?
        p.Count==5&&new[]{"a1","a2","r","angle","percent"}.All(p.ContainsKey)&&p["a1"] is >=10 and <=170&&p["a2"] is >=10 and <=170&&p["r"] is >=2 and <=30&&p["angle"] is >=10 and <=350&&p["percent"] is >=5 and <=95&&p["percent"]%5==0:
        Supports(f)&&p.Count==4&&new[]{"a","b","distance","k"}.All(p.ContainsKey)&&p["a"] is >=2 and <=30&&p["b"] is >=2 and <=30&&p["distance"] is >=0 and <=60&&p["k"] is >=1 and <=10&&
        (f!=Intersection||p["distance"]!=0||p["a"]!=p["b"])&&(f!=Overlap||p["distance"]>Math.Abs(p["a"]-p["b"])&&p["distance"]<p["a"]+p["b"]);
    private static int Count(int a,int b,int d)=>d>a+b||d<Math.Abs(a-b)?0:d==a+b||d==Math.Abs(a-b)?1:2;
    private static (long N,long D) Answer(string f,IReadOnlyDictionary<string,int> p)
    {
        if(IsTangent(f))
        {
            int k=p["k"],x=p["x"],a=p["a"],b=p["b"],c=p["c"];
            return f switch{TangentCount=>(4-p["mode"],1),TangentCheck=>(p["yes"],1),TangentLength=>(4*k,1),TangentRadius=>(3*k,1),TangentCenter=>(5*k,1),TangentEqual=>(a*k,1),TangentExpression=>(x,1),TangentPerimeter=>(2*(a+b+c),1),TangentQuad=>(a+k,1),_=>throw new InvalidOperationException()};
        }
        if(IsInscribed(f))
        {
            int v=p["v"],x=p["x"],k=p["k"];
            return f switch{InscribedHalf or InscribedEqual=>(v,1),InscribedDouble=>(2*v,1),InscribedRight=>(90-v,1),InscribedOpposite=>(180-2*v,1),InscribedAlgebra or InscribedCyclicAlgebra=>(x,1),InscribedDiameter=>(5*k,1),_=>throw new InvalidOperationException()};
        }
        if(IsChord(f))
        {
            int k=p["k"], arc=p["arc"], x=p["x"], r=p["radius"], h=p["half"];
            return f switch{ChordEqualArc=>(arc,1),ChordHalfArc=>(arc/2,1),ChordLength=>(8*k,1),ChordDistance or ChordEqualDistance=>(3*k,1),ChordAlgebra=>(x,1),ChordApprox=>((long)Math.Round(Math.Sqrt(r*r-h*h)*100,MidpointRounding.AwayFromZero),100),_=>throw new InvalidOperationException()};
        }
        if(IsArc(f))
        {
            int a1=p["a1"],a2=p["a2"],r=p["r"],angle=p["angle"],percent=p["percent"];
            return f switch{Central=>(360-a1-a2,1),Major=>(360-a1,1),Pie or ArcInverse=>(percent*18/5,1),ArcSum=>(a1+a2,1),ArcCoeff=>(r*angle,180),ArcApprox=>((long)Math.Round(r*angle*Math.PI/180*100,MidpointRounding.AwayFromZero),100),_=>throw new InvalidOperationException()};
        }
        int a=p["a"],b=p["b"],d=p["distance"],k=p["k"];
        return f switch {Diameter=>(2*a,1),Radius or Inverse=>(a,2),Circumference=>(2*a,1),Approx=>((long)Math.Round(a*Math.PI*100,MidpointRounding.AwayFromZero),100),Overlap=>(a+b-d,1),Intersection=>(Count(a,b,d),1),Rectangle=>(5*k,1),_=>throw new InvalidOperationException()};
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid G10 circle parameters");
        var(n,d)=Answer(f,p);
        if(f is Approx or ArcApprox or ChordApprox)return (n/100m).ToString("0.00",CultureInfo.InvariantCulture);
        if(f==ArcCoeff){long a=n,b=d;while(b!=0)(a,b)=(b,a%b);n/=a;d/=a;}
        return n%d==0?(n/d).ToString(CultureInfo.InvariantCulture):n.ToString(CultureInfo.InvariantCulture)+"/"+d.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(raw)||raw.Length>40)return false;
        string s=string.Concat(raw.Trim().Where(c=>!char.IsWhiteSpace(c)).Select(c=>c switch{>= '٠' and <= '٩'=>(char)('0'+c-'٠'),>= '۰' and <= '۹'=>(char)('0'+c-'۰'),'٫'=>'.','⁄'=>'/',_=>c}));
        string[] pair=s.Split('/');
        long n,d=1;
        if(pair.Length==2){if(!long.TryParse(pair[0],NumberStyles.None,CultureInfo.InvariantCulture,out n)||!long.TryParse(pair[1],NumberStyles.None,CultureInfo.InvariantCulture,out d)||n>1000000||d<1||d>1000000)return false;}
        else if(pair.Length==1&&decimal.TryParse(s,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal value)&&value>=0&&value<=1000000)
        {decimal scaled=value;while(scaled!=decimal.Truncate(scaled)&&d<10000){scaled*=10;d*=10;}if(scaled!=decimal.Truncate(scaled))return false;n=(long)scaled;}
        else return false;
        if(IsTangent(f))
        {
            long k=p["k"],x=p["x"],a=p["a"],b=p["b"],c=p["c"],mode=p["mode"],yes=p["yes"];
            if(f==TangentCount){long dist=mode switch{0=>a+b+1,1=>a+b,2=>b,3=>b-a,_=>b-a-1};long count=dist>a+b?4:dist==a+b?3:dist>b-a?2:dist==b-a?1:0;return n==count*d;}
            if(f==TangentCheck){long op=5*k+1-yes;bool tangent=op*op==25*k*k;return n==(tangent?1:0)*d;}
            if(f==TangentLength)return n*n==16*k*k*d*d;
            if(f==TangentRadius)return 4*k*n==12*k*k*d;
            if(f==TangentCenter)return n*n==25*k*k*d*d;
            if(f==TangentEqual)return n==a*k*d;
            if(f==TangentExpression)return a*n+c*d==(a*x+c)*d;
            if(f==TangentPerimeter)return n==((a+b)+(b+c)+(c+a))*d;
            return f==TangentQuad&&n+(b+c)*d==((a+b)+(c+k))*d;
        }
        if(IsInscribed(f))
        {
            long v=p["v"],x=p["x"],k=p["k"],aa=p["a"],bb=p["b"];
            return f switch{InscribedHalf=>2*n==2*v*d,InscribedEqual=>n==v*d,InscribedDouble=>n==2*v*d,InscribedRight=>n+v*d==90*d,InscribedOpposite=>n+2*v*d==180*d,InscribedAlgebra=>aa*n+bb*d==(aa*x+bb)*d,InscribedCyclicAlgebra=>aa*n+bb*d+(180-aa*x-bb)*d==180*d,InscribedDiameter=>n==5*k*d,_=>false};
        }
        if(IsChord(f))
        {
            long k=p["k"],arc=p["arc"],x=p["x"],a0=p["a"],b0=p["b"],r=p["radius"],h=p["half"];
            if(f==ChordEqualArc)return n==arc*d;
            if(f==ChordHalfArc)return 2*n==arc*d;
            if(f==ChordLength)return n==8*k*d;
            if(f is ChordDistance or ChordEqualDistance)return n==3*k*d;
            if(f==ChordAlgebra)return a0*n+b0*d==(a0*x+b0)*d;
            double actual=Math.Sqrt(r*r-h*h);
            return f==ChordApprox&&n*100%d==0&&(double)n/d-0.005<=actual&&actual<(double)n/d+0.005;
        }
        if(IsArc(f))
        {
            long a1=p["a1"],a2=p["a2"],r=p["r"],angle=p["angle"],percent=p["percent"];
            if(f==Central)return n+d*(a1+a2)==360*d;
            if(f==Major)return n+a1*d==360*d;
            if(f is Pie or ArcInverse)return 100*n==360*percent*d;
            if(f==ArcSum)return n==(a1+a2)*d;
            if(f==ArcCoeff)return 180*n==r*angle*d;
            double actual=r*angle*Math.PI/180;
            return f==ArcApprox&&n*100%d==0&&(double)n/d-0.005<=actual&&actual<(double)n/d+0.005;
        }
        long a=p["a"],b=p["b"],distance=p["distance"],k=p["k"];
        if(f is Radius or Inverse)return 2*n==a*d;
        if(f is Diameter or Circumference)return n==2*a*d;
        if(f==Approx)return n*100%d==0&&((double)n/d-0.005)<=a*Math.PI&&a*Math.PI<((double)n/d+0.005);
        if(f==Overlap)return n+d*distance==d*(a+b);
        if(f==Intersection)return n==Count((int)a,(int)b,(int)distance)*d;
        return f==Rectangle&&n==5*k*d&& (5*k)*(5*k)==(3*k)*(3*k)+(4*k)*(4*k);
    }
}
