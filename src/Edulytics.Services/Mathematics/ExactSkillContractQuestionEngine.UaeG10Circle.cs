using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// Source-specific families extend the existing ExactSkillContractQuestionEngine.
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10Circle(string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        if(UaeG10CircleResponse.IsCircleArea(family))
        {
            int k=random.Next(1,7),r=6*k,angle=30*random.Next(1,12),inner=random.Next(1,r),s=r*r*angle/360;
            string prompt=family switch
            {
                UaeG10CircleResponse.AreaDisk=>$"دائرة نصف قطرها {r} سم. اكتب معامل π في مساحتها.",
                UaeG10CircleResponse.AreaDiameter=>$"دائرة قطرها {2*r} سم. اكتب معامل π في مساحتها.",
                UaeG10CircleResponse.AreaRadius=>$"مساحة دائرة {r*r}π سم². أوجد نصف قطرها.",
                UaeG10CircleResponse.AreaCircumference=>$"محيط دائرة {2*r}π سم. اكتب معامل π في مساحتها.",
                UaeG10CircleResponse.AreaSector=>$"قطاع دائري نصف قطره {r} سم وزاويته {angle}°. اكتب معامل π في مساحته.",
                UaeG10CircleResponse.AreaSectorAngle=>$"مساحة قرص {r*r}π سم² ومساحة قطاع منه {s}π سم². أوجد زاوية القطاع بالدرجات.",
                UaeG10CircleResponse.AreaSectorRadius=>$"مساحة قطاع زاويته {angle}° تساوي {s}π سم². أوجد نصف قطره.",
                UaeG10CircleResponse.AreaAnnulus=>$"دائرتان متحدتا المركز نصفا قطريهما {r} و{inner} سم. اكتب معامل π في مساحة المنطقة بينهما.",
                UaeG10CircleResponse.AreaDiskApprox=>$"دائرة نصف قطرها {r} سم. أوجد المساحة مقربة لأقرب جزء من مائة.",
                UaeG10CircleResponse.AreaSectorApprox=>$"قطاع نصف قطره {r} سم وزاويته {angle}°. أوجد المساحة لأقرب جزء من مائة.",
                UaeG10CircleResponse.AreaSegmentApprox=>$"وتر في دائرة نصف قطرها {r} سم يقابل زاوية مركزية 90°. أوجد مساحة القطعة الدائرية الصغرى لأقرب جزء من مائة.",
                _=>$"دائرة تمس أضلاع مربع ضلعه {2*r} سم. أوجد مساحة المربع خارج الدائرة لأقرب جزء من مائة."
            };
            return Problem(family,prompt,"مساحة الدائرة πr²؛ القطاع نسبة زاويته إلى 360 من مساحة الدائرة؛ القطعة الصغرى قطاع ناقص مثلث.",UaeG10CircleResponse.IsAreaApprox(family)?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("k",k),("angle",angle),("inner",inner));
        }
        if(UaeG10CircleResponse.IsCircleEquation(family))
        {
            int h=random.Next(-8,9),k=random.Next(-8,9),m=random.Next(1,7),mode=random.Next(0,3),yes=random.Next(0,2);
            int r=5*m,d=-2*h,e=-2*k,f=h*h+k*k-r*r,line=k+(mode==0?3*m:mode==1?r:r+1);
            string prompt=family switch
            {
                UaeG10CircleResponse.EquationCenterX=>$"معادلة دائرة س²+ص²+({d})س+({e})ص+({f})=0. أوجد الإحداثي السيني للمركز.",
                UaeG10CircleResponse.EquationCenterY=>$"معادلة دائرة س²+ص²+({d})س+({e})ص+({f})=0. أوجد الإحداثي الصادي للمركز.",
                UaeG10CircleResponse.EquationRadius=>$"معادلة دائرة (س−({h}))²+(ص−({k}))²={r*r}. أوجد نصف قطرها.",
                UaeG10CircleResponse.EquationPointRadiusSquared=>$"دائرة مركزها ({h}،{k}) وتمر بالنقطة ({h+3*m}،{k+4*m}). أوجد مربع نصف القطر.",
                UaeG10CircleResponse.EquationGeneralRadius=>$"معادلة دائرة س²+ص²+({d})س+({e})ص+({f})=0. أوجد نصف القطر.",
                UaeG10CircleResponse.EquationConstant=>$"دائرة مركزها ({h}،{k}) ونصف قطرها {r}. في المعادلة العامة س²+ص²+({d})س+({e})ص+ج=0، أوجد ج.",
                UaeG10CircleResponse.EquationDiameterCenter=>$"طرفا قطر دائرة هما ({h-r}،{k}) و({h+r}،{k}). أوجد الإحداثي السيني للمركز.",
                UaeG10CircleResponse.EquationPointTest=>$"دائرة مركزها ({h}،{k}) ونصف قطرها {r}. هل تقع النقطة ({h+3*m}،{k+4*m+1-yes}) على المحيط؟ اكتب 1 لنعم أو 0 للا.",
                UaeG10CircleResponse.EquationLineCount=>$"دائرة (س−({h}))²+(ص−({k}))²={r*r} ومستقيم ص={line}. كم نقطة تقاطع؟",
                UaeG10CircleResponse.EquationLineX=>$"دائرة (س−({h}))²+(ص−({k}))²={r*r} ومستقيم ص={k+3*m}. أوجد الإحداثي السيني لنقطة التقاطع اليمنى.",
                UaeG10CircleResponse.EquationThreePoints=>$"دائرة تمر بالنقاط ({h-r}،{k}) و({h+r}،{k}) و({h}،{k+r}). أوجد نصف قطرها.",
                _=>$"دائرة س²+ص²={2*m*m} ومستقيم ص=س. أوجد الإحداثي السيني الموجب لنقطة التقاطع."
            };
            return Problem(family,prompt,"استخدم الصيغة القياسية وقانون المسافة، وأكمل المربع أو حل المعادلتين بالتعويض.",AssessmentItemType.Numeric,("h",h),("k",k),("m",m),("mode",mode),("yes",yes));
        }
        if(UaeG10CircleResponse.IsSpecialSegment(family))
        {
            int k=random.Next(1,9),x=random.Next(2,12),b=random.Next(2,10),e=random.Next(2,12),i=random.Next(2,16);
            string prompt=family switch
            {
                UaeG10CircleResponse.SegmentChord=>$"وتران يتقاطعان داخل دائرة؛ جزآ الأول {3*k} و{8*k} سم، وأحد جزأي الثاني {4*k} سم. أوجد الجزء الآخر.",
                UaeG10CircleResponse.SegmentChordAlgebra=>$"جزآ وتر متقاطع طولهما س و(س+{b}) سم، وجزآ الآخر {x} و{x+b} سم. أوجد قيمة س الموجبة.",
                UaeG10CircleResponse.SegmentSecantWhole=>$"قاطع من نقطة خارج دائرة، خارجيه {3*k} وطوله الكامل {12*k} سم. قاطع ثانٍ خارجيه {4*k} سم. أوجد طوله الكامل.",
                UaeG10CircleResponse.SegmentSecantInner=>$"قاطع خارجيه {3*k} وطوله الداخلي {9*k} سم. قاطع آخر من النقطة نفسها خارجيه {4*k} سم. أوجد طوله الداخلي.",
                UaeG10CircleResponse.SegmentTangent=>$"قاطع خارجيه {4*k} وطوله الكامل {9*k} سم، ومماس من النقطة نفسها. أوجد طول المماس.",
                UaeG10CircleResponse.SegmentTangentExternal=>$"مماس طوله {6*k} سم، وقاطع من النقطة نفسها طوله الكامل {9*k} سم. أوجد الجزء الخارجي.",
                UaeG10CircleResponse.SegmentTangentQuadratic=>$"مماس طوله {6*k} سم، والجزء الداخلي لقاطع من النقطة نفسها {9*k} سم. أوجد طول الجزء الخارجي.",
                UaeG10CircleResponse.SegmentSagitta=>$"قوس دائري فوق وتر طوله {18*k} سم. ارتفاع القوس فوق منتصف الوتر {3*k} سم. أوجد نصف قطر الدائرة.",
                _=>$"قاطع من نقطة خارج دائرة خارجيه {e} سم وداخليه {i} سم. أوجد طول المماس من النقطة نفسها مقربًا لأقرب جزء من مائة."
            };
            // 6k tangent and inner 9k imply e=3k, since 3k*(3k+9k)=36k².
            return Problem(family,prompt,"وتران داخل: جزء×جزء؛ قاطعان خارج: خارجي×كامل؛ مماس وقاطع: مربع المماس=خارجي×كامل.",family==UaeG10CircleResponse.SegmentApprox?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("k",k),("x",x),("b",b),("e",e),("i",i));
        }
        if(UaeG10CircleResponse.IsSecantAngle(family))
        {
            int a=2*random.Next(10,61),b=2*random.Next(10,61),v=2*random.Next(10,81),x=2*random.Next(2,16),k=random.Next(2,5);
            if(k*x+a+b>=360||k*x+2*a>=360||2*a+b>=360){a=60;b=60;}
            int far=a+b,near=a;
            string prompt=family switch
            {
                UaeG10CircleResponse.SecantInside=>$"وتران يتقاطعان داخل دائرة؛ القوسان المحصوران بالزاوية ومقابلتها بالرأس {a}° و{b}°. أوجد قياس الزاوية.",
                UaeG10CircleResponse.SecantInsideArc=>$"زاوية بين وترين داخل دائرة قياسها {(a+b)/2}°؛ أحد القوسين المحصورين {a}°. أوجد القوس الآخر.",
                UaeG10CircleResponse.SecantAdjacent=>$"قوسا زاوية داخلية ومقابلتها بالرأس {a}° و{b}°. أوجد قياس الزاوية المجاورة.",
                UaeG10CircleResponse.SecantOn=>$"مماس ووتر عند نقطة التماس يحصران قوسًا قياسه {v}°. أوجد الزاوية.",
                UaeG10CircleResponse.SecantOnArc=>$"زاوية مماس ووتر عند التماس قياسها {v/2}°. أوجد القوس المحصور.",
                UaeG10CircleResponse.SecantOutside=>$"قاطعان من نقطة خارج دائرة، القوس البعيد {far}° والقريب {near}°. أوجد قياس الزاوية.",
                UaeG10CircleResponse.SecantOutsideArc=>$"زاوية خارجية بين قاطع ومماس قياسها {b/2}°، والقوس القريب {near}°. أوجد القوس البعيد.",
                UaeG10CircleResponse.SecantTwoTangents=>$"مماسان من نقطة خارج دائرة؛ قياس القوس الأصغر بين نقطتي التماس {v}°. أوجد الزاوية بين المماسين.",
                UaeG10CircleResponse.SecantTangentArc=>$"زاوية مماسين من نقطة خارج دائرة قياسها {180-v}°. أوجد قياس القوس الأصغر.",
                UaeG10CircleResponse.SecantInsideAlgebra=>$"القوسان المحصوران بزاوية داخلية ومقابلتها قياسهما ({k}س+{a})° و{b}°، وقياس الزاوية {(k*x+a+b)/2}°. أوجد س.",
                _=>$"القوس البعيد لزاوية خارجية ({k}س+{a})° والقريب {a}°؛ قياس الزاوية {k*x/2}°. أوجد س."
            };
            return Problem(family,prompt,"حدد موضع الرأس: على المحيط نصف القوس، داخل الدائرة نصف المجموع، خارجها نصف الفرق.",AssessmentItemType.Numeric,("a",a),("b",b),("v",v),("x",x),("k",k));
        }
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
        {
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
    internal const string SecantInside="uae.g10.general.u13.l06.inside_angle";
    internal const string SecantInsideArc="uae.g10.general.u13.l06.inside_missing_arc";
    internal const string SecantAdjacent="uae.g10.general.u13.l06.inside_adjacent_angle";
    internal const string SecantOn="uae.g10.general.u13.l06.tangent_chord_angle";
    internal const string SecantOnArc="uae.g10.general.u13.l06.tangent_chord_arc";
    internal const string SecantOutside="uae.g10.general.u13.l06.outside_angle";
    internal const string SecantOutsideArc="uae.g10.general.u13.l06.outside_missing_far_arc";
    internal const string SecantTwoTangents="uae.g10.general.u13.l06.two_tangent_angle";
    internal const string SecantTangentArc="uae.g10.general.u13.l06.two_tangent_minor_arc";
    internal const string SecantInsideAlgebra="uae.g10.general.u13.l06.inside_arc_expression";
    internal const string SecantOutsideAlgebra="uae.g10.general.u13.l06.outside_arc_expression";
    internal static bool IsSecantAngle(string? f)=>f is SecantInside or SecantInsideArc or SecantAdjacent or SecantOn or SecantOnArc or SecantOutside or SecantOutsideArc or SecantTwoTangents or SecantTangentArc or SecantInsideAlgebra or SecantOutsideAlgebra;
    internal const string SegmentChord="uae.g10.general.u13.l07.intersecting_chord_missing";
    internal const string SegmentChordAlgebra="uae.g10.general.u13.l07.intersecting_chord_quadratic";
    internal const string SegmentSecantWhole="uae.g10.general.u13.l07.two_secants_whole";
    internal const string SegmentSecantInner="uae.g10.general.u13.l07.two_secants_inner";
    internal const string SegmentTangent="uae.g10.general.u13.l07.tangent_length";
    internal const string SegmentTangentExternal="uae.g10.general.u13.l07.tangent_external";
    internal const string SegmentTangentQuadratic="uae.g10.general.u13.l07.tangent_secant_quadratic";
    internal const string SegmentSagitta="uae.g10.general.u13.l07.chord_sagitta_radius";
    internal const string SegmentApprox="uae.g10.general.u13.l07.tangent_rounded";
    internal static bool IsSpecialSegment(string? f)=>f is SegmentChord or SegmentChordAlgebra or SegmentSecantWhole or SegmentSecantInner or SegmentTangent or SegmentTangentExternal or SegmentTangentQuadratic or SegmentSagitta or SegmentApprox;
    internal const string EquationCenterX="uae.g10.general.u13.l08.center_x_from_general";
    internal const string EquationCenterY="uae.g10.general.u13.l08.center_y_from_general";
    internal const string EquationRadius="uae.g10.general.u13.l08.radius_from_standard";
    internal const string EquationPointRadiusSquared="uae.g10.general.u13.l08.radius_squared_from_point";
    internal const string EquationGeneralRadius="uae.g10.general.u13.l08.radius_from_general";
    internal const string EquationConstant="uae.g10.general.u13.l08.general_constant";
    internal const string EquationDiameterCenter="uae.g10.general.u13.l08.diameter_midpoint_x";
    internal const string EquationPointTest="uae.g10.general.u13.l08.point_membership";
    internal const string EquationLineCount="uae.g10.general.u13.l08.horizontal_line_intersection_count";
    internal const string EquationLineX="uae.g10.general.u13.l08.right_intersection_x";
    internal const string EquationThreePoints="uae.g10.general.u13.l08.three_point_radius";
    internal const string EquationDiagonal="uae.g10.general.u13.l08.diagonal_positive_intersection";
    internal static bool IsCircleEquation(string? f)=>f is EquationCenterX or EquationCenterY or EquationRadius or EquationPointRadiusSquared or EquationGeneralRadius or EquationConstant or EquationDiameterCenter or EquationPointTest or EquationLineCount or EquationLineX or EquationThreePoints or EquationDiagonal;
    internal const string AreaDisk="uae.g10.general.u13.l09.disk_pi_coefficient";
    internal const string AreaDiameter="uae.g10.general.u13.l09.disk_from_diameter";
    internal const string AreaRadius="uae.g10.general.u13.l09.radius_from_area";
    internal const string AreaCircumference="uae.g10.general.u13.l09.area_from_circumference";
    internal const string AreaSector="uae.g10.general.u13.l09.sector_pi_coefficient";
    internal const string AreaSectorAngle="uae.g10.general.u13.l09.sector_central_angle";
    internal const string AreaSectorRadius="uae.g10.general.u13.l09.sector_radius";
    internal const string AreaAnnulus="uae.g10.general.u13.l09.annulus_pi_coefficient";
    internal const string AreaDiskApprox="uae.g10.general.u13.l09.disk_rounded";
    internal const string AreaSectorApprox="uae.g10.general.u13.l09.sector_rounded";
    internal const string AreaSegmentApprox="uae.g10.general.u13.l09.quarter_segment_rounded";
    internal const string AreaCompositeApprox="uae.g10.general.u13.l09.square_outside_disk_rounded";
    internal static bool IsCircleArea(string? f)=>f is AreaDisk or AreaDiameter or AreaRadius or AreaCircumference or AreaSector or AreaSectorAngle or AreaSectorRadius or AreaAnnulus or AreaDiskApprox or AreaSectorApprox or AreaSegmentApprox or AreaCompositeApprox;
    internal static bool IsAreaApprox(string? f)=>f is AreaDiskApprox or AreaSectorApprox or AreaSegmentApprox or AreaCompositeApprox;
    internal static bool Supports(string? f)=>IsCircleArea(f)||IsCircleEquation(f)||IsSpecialSegment(f)||IsSecantAngle(f)||IsTangent(f)||IsInscribed(f)||IsChord(f)||IsArc(f)||f is Diameter or Radius or Circumference or Approx or Overlap or Intersection or Rectangle or Inverse;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>IsCircleArea(f)?
        p.Count==3&&new[]{"k","angle","inner"}.All(p.ContainsKey)&&p["k"] is >=1 and <=6&&p["angle"] is >=30 and <=330&&p["angle"]%30==0&&p["inner"]>=1&&p["inner"]<6*p["k"]:
        IsCircleEquation(f)?
        p.Count==5&&new[]{"h","k","m","mode","yes"}.All(p.ContainsKey)&&p["h"] is >=-8 and <=8&&p["k"] is >=-8 and <=8&&p["m"] is >=1 and <=6&&p["mode"] is >=0 and <=2&&p["yes"] is >=0 and <=1:
        IsSpecialSegment(f)?
        p.Count==5&&new[]{"k","x","b","e","i"}.All(p.ContainsKey)&&p["k"] is >=1 and <=8&&p["x"] is >=2 and <=11&&p["b"] is >=2 and <=9&&p["e"] is >=2 and <=11&&p["i"] is >=2 and <=15:
        IsSecantAngle(f)?
        p.Count==5&&new[]{"a","b","v","x","k"}.All(p.ContainsKey)&&p["a"] is >=20 and <=120&&p["a"]%2==0&&p["b"] is >=20 and <=120&&p["b"]%2==0&&p["v"] is >=20 and <=160&&p["v"]%2==0&&p["x"] is >=4 and <=30&&p["x"]%2==0&&p["k"] is >=2 and <=4&&p["k"]*p["x"]+p["a"]+p["b"]<360&&p["k"]*p["x"]+2*p["a"]<360&&2*p["a"]+p["b"]<360:
        IsTangent(f)?
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
        if(IsCircleArea(f))
        {
            int r=6*p["k"],angle=p["angle"],inner=p["inner"],sq=r*r,s=sq*angle/360;
            return f switch{AreaDisk or AreaDiameter or AreaCircumference=>(sq,1),AreaRadius or AreaSectorRadius=>(r,1),AreaSector=>(s,1),AreaSectorAngle=>(angle,1),AreaAnnulus=>(sq-inner*inner,1),AreaDiskApprox=>((long)Math.Round(sq*Math.PI*100,MidpointRounding.AwayFromZero),100),AreaSectorApprox=>((long)Math.Round(s*Math.PI*100,MidpointRounding.AwayFromZero),100),AreaSegmentApprox=>((long)Math.Round((sq*Math.PI/4-sq/2.0)*100,MidpointRounding.AwayFromZero),100),AreaCompositeApprox=>((long)Math.Round((4*sq-sq*Math.PI)*100,MidpointRounding.AwayFromZero),100),_=>throw new InvalidOperationException()};
        }
        if(IsCircleEquation(f))
        {
            int h=p["h"],k=p["k"],m=p["m"],r=5*m;
            return f switch{EquationCenterX or EquationDiameterCenter=>(h,1),EquationCenterY=>(k,1),EquationRadius or EquationGeneralRadius or EquationThreePoints=>(r,1),EquationPointRadiusSquared=>(r*r,1),EquationConstant=>(h*h+k*k-r*r,1),EquationPointTest=>(p["yes"],1),EquationLineCount=>(2-p["mode"],1),EquationLineX=>(h+4*m,1),EquationDiagonal=>(m,1),_=>throw new InvalidOperationException()};
        }
        if(IsSpecialSegment(f))
        {
            int k=p["k"],x=p["x"],e=p["e"],i=p["i"];
            return f switch{SegmentChord=>(6*k,1),SegmentChordAlgebra=>(x,1),SegmentSecantWhole=>(9*k,1),SegmentSecantInner=>(5*k,1),SegmentTangent=>(6*k,1),SegmentTangentExternal=>(4*k,1),SegmentTangentQuadratic=>(3*k,1),SegmentSagitta=>(15*k,1),SegmentApprox=>((long)Math.Round(Math.Sqrt(e*(e+i))*100,MidpointRounding.AwayFromZero),100),_=>throw new InvalidOperationException()};
        }
        if(IsSecantAngle(f))
        {
            int a=p["a"],b=p["b"],v=p["v"],x=p["x"];
            return f switch{SecantInside=>((a+b)/2,1),SecantInsideArc=>(b,1),SecantAdjacent=>(180-(a+b)/2,1),SecantOn=>(v/2,1),SecantOnArc=>(v,1),SecantOutside=>(b/2,1),SecantOutsideArc=>(a+b,1),SecantTwoTangents=>(180-v,1),SecantTangentArc=>(v,1),SecantInsideAlgebra or SecantOutsideAlgebra=>(x,1),_=>throw new InvalidOperationException()};
        }
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
        {
            int a=p["a"],b=p["b"],d=p["distance"],k=p["k"];
            return f switch {Diameter=>(2*a,1),Radius or Inverse=>(a,2),Circumference=>(2*a,1),Approx=>((long)Math.Round(a*Math.PI*100,MidpointRounding.AwayFromZero),100),Overlap=>(a+b-d,1),Intersection=>(Count(a,b,d),1),Rectangle=>(5*k,1),_=>throw new InvalidOperationException()};
        }
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid G10 circle parameters");
        var(n,d)=Answer(f,p);
        if((f is Approx or ArcApprox or ChordApprox or SegmentApprox)||IsAreaApprox(f))return (n/100m).ToString("0.00",CultureInfo.InvariantCulture);
        if(f==ArcCoeff){long a=n,b=d;while(b!=0)(a,b)=(b,a%b);n/=a;d/=a;}
        return n%d==0?(n/d).ToString(CultureInfo.InvariantCulture):n.ToString(CultureInfo.InvariantCulture)+"/"+d.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(raw)||raw.Length>40)return false;
        string s=string.Concat(raw.Trim().Where(c=>!char.IsWhiteSpace(c)).Select(c=>c switch{>= '٠' and <= '٩'=>(char)('0'+c-'٠'),>= '۰' and <= '۹'=>(char)('0'+c-'۰'),'٫'=>'.','⁄'=>'/',_=>c}));
        if(IsCircleEquation(f))
        {
            if(!decimal.TryParse(s.Replace('−','-'),NumberStyles.AllowLeadingSign|NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal signedValue)||signedValue!=decimal.Truncate(signedValue)||signedValue < -1000000||signedValue > 1000000)return false;
            long q=(long)signedValue,h=p["h"],k=p["k"],m=p["m"],r=5*m,mode=p["mode"],yes=p["yes"];
            if(f==EquationCenterX)return 2*q==2*h;
            if(f==EquationCenterY)return 2*q==2*k;
            if(f==EquationRadius)return q>0&&q*q==r*r;
            if(f==EquationPointRadiusSquared)return q==9*m*m+16*m*m;
            if(f==EquationGeneralRadius)return q>0&&q*q==h*h+k*k-(h*h+k*k-r*r);
            if(f==EquationConstant)return h*h+k*k-q==r*r;
            if(f==EquationDiameterCenter)return 2*q==(h-r)+(h+r);
            if(f==EquationPointTest){long dy=4*m+1-yes;return q==(9*m*m+dy*dy==r*r?1:0);}
            if(f==EquationLineCount){long dist=mode==0?3*m:mode==1?r:r+1;return q==(dist<r?2:dist==r?1:0);}
            if(f==EquationLineX)return q>h&&(q-h)*(q-h)+9*m*m==r*r;
            if(f==EquationThreePoints)return q>0&&q*q==((h+r)-(h-r))*((h+r)-(h-r))/4;
            return f==EquationDiagonal&&q>0&&2*q*q==2*m*m;
        }
        string[] pair=s.Split('/');
        long n,d=1;
        if(pair.Length==2){if(!long.TryParse(pair[0],NumberStyles.None,CultureInfo.InvariantCulture,out n)||!long.TryParse(pair[1],NumberStyles.None,CultureInfo.InvariantCulture,out d)||n>1000000||d<1||d>1000000)return false;}
        else if(pair.Length==1&&decimal.TryParse(s,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal value)&&value>=0&&value<=1000000)
        {decimal scaled=value;while(scaled!=decimal.Truncate(scaled)&&d<10000){scaled*=10;d*=10;}if(scaled!=decimal.Truncate(scaled))return false;n=(long)scaled;}
        else return false;
        if(IsCircleArea(f))
        {
            long r=6*p["k"],angle=p["angle"],inner=p["inner"],sq=r*r,sectorCoefficient=sq*angle/360;
            if(f is AreaDisk or AreaDiameter or AreaCircumference)return n==sq*d;
            if(f==AreaRadius)return n>0&&n*n==sq*d*d;
            if(f==AreaSector)return 360*n==sq*angle*d;
            if(f==AreaSectorAngle)return n*sq==360*sectorCoefficient*d;
            if(f==AreaSectorRadius)return n>0&&angle*n*n==360*sectorCoefficient*d*d;
            if(f==AreaAnnulus)return n+inner*inner*d==sq*d;
            double actual=f switch{AreaDiskApprox=>sq*Math.PI,AreaSectorApprox=>sq*angle*Math.PI/360,AreaSegmentApprox=>sq*(Math.PI-2)/4,AreaCompositeApprox=>sq*(4-Math.PI),_=>double.NaN};
            return IsAreaApprox(f)&&n*100%d==0&&(double)n/d-0.005<=actual&&actual<(double)n/d+0.005;
        }
        if(IsSpecialSegment(f))
        {
            long k=p["k"],x=p["x"],b=p["b"],e=p["e"],i=p["i"];
            if(n<=0)return false;
            if(f==SegmentChord)return 4*k*n==24*k*k*d;
            if(f==SegmentChordAlgebra)return n*(n+b*d)==x*(x+b)*d*d;
            if(f==SegmentSecantWhole)return 4*k*n==36*k*k*d&&n>4*k*d;
            if(f==SegmentSecantInner)return 4*k*(4*k*d+n)==36*k*k*d;
            if(f==SegmentTangent)return n*n==36*k*k*d*d;
            if(f==SegmentTangentExternal)return 9*k*n==36*k*k*d&&n<9*k*d;
            if(f==SegmentTangentQuadratic)return n*(n+9*k*d)==36*k*k*d*d;
            if(f==SegmentSagitta)return 3*k*(2*n-3*k*d)==81*k*k*d;
            double actual=Math.Sqrt(e*(e+i));
            return f==SegmentApprox&&n*100%d==0&&(double)n/d-0.005<=actual&&actual<(double)n/d+0.005;
        }
        if(IsSecantAngle(f))
        {
            long a=p["a"],b=p["b"],v=p["v"],x=p["x"],k=p["k"];
            return f switch{SecantInside=>2*n==(a+b)*d,SecantInsideArc=>n+a*d==(a+b)*d,SecantAdjacent=>2*n+(a+b)*d==360*d,SecantOn=>2*n==v*d,SecantOnArc=>n==2*(v/2)*d,SecantOutside=>2*n==((a+b)-a)*d,SecantOutsideArc=>n-a*d==b*d,SecantTwoTangents=>n+v*d==180*d,SecantTangentArc=>n+(180-v)*d==180*d,SecantInsideAlgebra=>k*n+(a+b)*d==(k*x+a+b)*d,SecantOutsideAlgebra=>k*n+a*d-a*d==k*x*d,_=>false};
        }
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
        {
            long a=p["a"],b=p["b"],distance=p["distance"],k=p["k"];
            if(f is Radius or Inverse)return 2*n==a*d;
            if(f is Diameter or Circumference)return n==2*a*d;
            if(f==Approx)return n*100%d==0&&((double)n/d-0.005)<=a*Math.PI&&a*Math.PI<((double)n/d+0.005);
            if(f==Overlap)return n+d*distance==d*(a+b);
            if(f==Intersection)return n==Count((int)a,(int)b,(int)distance)*d;
            return f==Rectangle&&n==5*k*d&& (5*k)*(5*k)==(3*k)*(3*k)+(4*k)*(4*k);
        }
    }
}
