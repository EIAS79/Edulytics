using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Exact Arabic Grade-8 objective-specific practice for gaps identified in the
/// 76-lesson runtime audit. Reuse of existing engines remains the default.
/// These short, machine-graded tasks do not claim coverage of source diagrams,
/// open proofs, or teacher-assessed explanations.
/// </summary>
internal static class UaeGrade8SupplementalQuestions
{
    internal const string MonomialMultiply = "uae.g8.u01.l03.monomial_multiply";
    internal const string MonomialDivide = "uae.g8.u01.l03.monomial_divide";
    internal const string MonomialPower = "uae.g8.u01.l04.monomial_power";
    internal const string NegativeExponent = "uae.g8.u01.l05.negative_integer_exponent";
    internal const string SquareRootExact = "uae.g8.u01.l08.square_root_exact";
    internal const string CubeRootExact = "uae.g8.u01.l08.cube_root_exact";
    internal const string SquareRootBounds = "uae.g8.u01.l09.square_root_bounds";
    internal const string CubeRootBounds = "uae.g8.u01.l09.cube_root_bounds";
    internal const string WriteTwoStepEquation = "uae.g8.u02.l03.write_two_step_equation";
    internal const string DirectVariation = "uae.g8.u03.l03.direct_variation";
    internal const string SegmentAddition = "uae.g8.u10.l02.segment_addition";
    internal const string SegmentMidpoint = "uae.g8.u10.l03.midpoint_coordinate";
    internal const string SegmentProof = "uae.g8.u10.l04.segment_reason";
    internal const string ParallelogramTest = "uae.g8.u11.l03.parallelogram_test";
    internal const string RectangleTest = "uae.g8.u11.l04.rectangle_test";
    internal const string RhombusDiagonals = "uae.g8.u11.l05.rhombus_diagonals";
    internal const string SystemSolutionCount = "uae.g8.u03.l07.system_solution_count";
    internal const string ScatterDirection = "uae.g8.u09.l01.scatter_direction";
    internal const string DistributionShape = "uae.g8.u09.l06.distribution_shape";

    internal sealed record Item(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    internal static bool Supports(string? family) => family is
        MonomialMultiply or MonomialDivide or MonomialPower or NegativeExponent or
        SquareRootExact or CubeRootExact or SquareRootBounds or CubeRootBounds or
        WriteTwoStepEquation or DirectVariation or SegmentAddition or SegmentMidpoint or
        SegmentProof or ParallelogramTest or RectangleTest or RhombusDiagonals or
        SystemSolutionCount or ScatterDirection or DistributionShape;

    private static int Pow(int a, int e)
    {
        var result = 1;
        for (var i = 0; i < e; i++) result = checked(result * a);
        return result;
    }

    private static string Mono(int a, int exponent) => $"{a}x^{exponent}";

    internal static Item Build(string family, Random random)
    {
        if (!Supports(family)) throw new InvalidOperationException("Unknown G8 supplemental family.");
        var p = new Dictionary<string, int>();
        string prompt, solution;
        var itemType = AssessmentItemType.Numeric;
        switch (family)
        {
            case MonomialMultiply:
            {
                var a = random.Next(2, 10); var b = random.Next(2, 10);
                var m = random.Next(1, 6); var n = random.Next(1, 6);
                p["a"] = a; p["b"] = b; p["m"] = m; p["n"] = n;
                prompt = $"اضرب أحاديتي الحد ({Mono(a,m)}) × ({Mono(b,n)}). اكتب الناتج بصيغة معامل متبوعًا بـx^الأس من دون مسافات، مثل 6x^3.";
                solution = "اضرب المعاملين، واجمع الأسين عند ضرب قوتين لهما الأساس نفسه x.";
                itemType = AssessmentItemType.ShortAnswer;
                break;
            }
            case MonomialDivide:
            {
                var a = random.Next(2, 7); var q = random.Next(2, 9);
                var m = random.Next(1, 6); var n = random.Next(1, 5);
                p["a"] = a; p["q"] = q; p["m"] = m; p["n"] = n;
                prompt = $"اقسم ({Mono(a*q,m+n)}) ÷ ({Mono(a,m)}). اكتب الناتج بصيغة معامل متبوعًا بـx^الأس، مثل 6x^3.";
                solution = "اقسم المعاملين، واطرح أس المقسوم عليه من أس المقسوم عندما الأساس واحد.";
                itemType = AssessmentItemType.ShortAnswer;
                break;
            }
            case MonomialPower:
            {
                var a = random.Next(2, 6); var m = random.Next(1, 5);
                var n = random.Next(2, 5);
                p["a"] = a; p["m"] = m; p["n"] = n;
                prompt = $"أوجد ({Mono(a,m)})^{n}. اكتب الناتج بصيغة معامل متبوعًا بـx^الأس، مثل 9x^6.";
                solution = "ارفع المعامل إلى القوة، واضرب أس المتغير في الأس الخارجي.";
                itemType = AssessmentItemType.ShortAnswer;
                break;
            }
            case NegativeExponent:
            {
                var a = random.Next(2, 7); var e = random.Next(1, 5);
                p["a"] = a; p["e"] = e;
                prompt = $"أعد كتابة {a}^(-{e}) في صورة كسر بأس موجب. اكتب 1/المقام دون مسافات.";
                solution = "قانون الأس السالب: a^(-n)=1/a^n عندما a لا يساوي صفرًا.";
                itemType = AssessmentItemType.ShortAnswer;
                break;
            }
            case SquareRootExact:
            case CubeRootExact:
            {
                var k = random.Next(2, 17);
                p["k"] = k;
                prompt = family == SquareRootExact
                    ? $"أوجد الجذر التربيعي الموجب للعدد {k*k}."
                    : $"أوجد الجذر التكعيبي الحقيقي للعدد {k*k*k}.";
                solution = family == SquareRootExact
                    ? "الجذر التربيعي الموجب لعدد مربع كامل هو طول ضلع مربعه."
                    : "الجذر التكعيبي لعدد مكعب كامل هو العدد الذي يساوي مكعبه العدد المعطى.";
                break;
            }
            case SquareRootBounds:
            case CubeRootBounds:
            {
                var k = random.Next(2, 12);
                var max = family == SquareRootBounds
                    ? 2*k : 3*k*k+3*k;
                var extra = random.Next(1, Math.Min(max, 25)+1);
                p["k"] = k; p["extra"] = extra;
                var n = family == SquareRootBounds ? k*k+extra : k*k*k+extra;
                prompt = family == SquareRootBounds
                    ? $"الجذر التربيعي للعدد {n} يقع بين عددين صحيحين متتاليين. ما العدد الأصغر منهما؟"
                    : $"الجذر التكعيبي للعدد {n} يقع بين عددين صحيحين متتاليين. ما العدد الأصغر منهما؟";
                solution = family == SquareRootBounds
                    ? "احصر العدد بين مربعين كاملين متتاليين؛ الجذر يقع بين أساسَيهما."
                    : "احصر العدد بين مكعبين كاملين متتاليين؛ الجذر التكعيبي يقع بين أساسَيهما.";
                break;
            }
            case WriteTwoStepEquation:
            {
                var a = random.Next(2, 15); var b = random.Next(2, 31);
                p["a"] = a; p["b"] = b;
                prompt = $"رسوم خدمة ثابتة قدرها {b} درهمًا، ويضاف {a} دراهم عن كل حصة. إذا كان x عدد الحصص وy التكلفة الكلية، فاكتب معادلة y بدلالة x بصيغة y=ax+b من دون مسافات.";
                solution = "التكلفة الكلية تساوي الرسم الثابت زائد تكلفة الحصة مضروبة في عدد الحصص.";
                itemType = AssessmentItemType.ShortAnswer;
                break;
            }
            case DirectVariation:
            {
                var a = random.Next(1, 10);
                var x = random.Next(-9, 10);
                p["a"] = a; p["x"] = x;
                prompt = $"العلاقة التناسبية المباشرة y={a}x. إذا كان x={x}، فأوجد قيمة y.";
                solution = "في التناسب المباشر يساوي الناتج ثابت التناسب مضروبًا في المدخل، ولا يوجد حد ثابت.";
                break;
            }
            case SegmentAddition:
            {
                var a = random.Next(2, 31); var b = random.Next(2, 31);
                p["a"] = a; p["b"] = b;
                prompt = $"النقطة ب تقع بين أ وج على خط مستقيم. إذا كان طول أ ب={a} سم وطول ب ج={b} سم، فما طول أ ج بالسنتيمتر؟";
                solution = "بحسب مسلمة جمع القطع المستقيمة، أ ج = أ ب + ب ج.";
                break;
            }
            case SegmentMidpoint:
            {
                var x = random.Next(-12, 13); var y = random.Next(-12, 13);
                var dx = random.Next(1, 9); var dy = random.Next(1, 9);
                var component = random.Next(0, 2);
                p["x"] = x; p["y"] = y; p["dx"] = dx; p["dy"] = dy; p["component"] = component;
                prompt = $"أ({x-dx}، {y-dy}) وب({x+dx}، {y+dy}) هما طرفا قطعة مستقيمة. ما الإحداثي {(component==0?"السيني":"الصادي")} لنقطة منتصف أ ب؟";
                solution = "إحداثيا نقطة المنتصف هما متوسطا إحداثيات طرفي القطعة.";
                break;
            }
            case SegmentProof:
            {
                var mode = random.Next(0, 3);
                p["mode"] = mode;
                prompt = (mode switch
                {
                    0 => "إذا كانت ب بين أ وج، فلماذا أ ب + ب ج = أ ج؟ ",
                    1 => "إذا كان أ ب = ج د، وج د = هـ و، فلماذا أ ب = هـ و؟ ",
                    _ => "إذا كانت م نقطة منتصف أ ب، فلماذا أ م = م ب؟ "
                }) + "اختر السبب: 1 مسلمة جمع القطع، 2 خاصية التعدي، 3 تعريف نقطة المنتصف.";
                solution = "اختر المسلمة أو التعريف الذي يبرر الانتقال، وليس نتيجة أخرى.";
                break;
            }
            case ParallelogramTest:
            {
                var mode = random.Next(0, 5);
                p["mode"] = mode;
                prompt = (mode switch
                {
                    0 => "رباعي فيه كل ضلعين متقابلين متساويان في الطول. ",
                    1 => "رباعي ينصف كل قطر فيه القطر الآخر. ",
                    2 => "رباعي له زوج واحد من الأضلاع المتقابلة متوازٍ ومتساوٍ في الطول. ",
                    3 => "رباعي فيه زوج واحد فقط من الأضلاع المتقابلة متوازٍ، دون معطيات أخرى. ",
                    _ => "رباعي قطرَاه متساويان في الطول دون معطيات أخرى. "
                }) + "هل هذه المعطيات كافية لإثبات أن الرباعي متوازي أضلاع؟ اكتب 1 لنعم أو 0 للا.";
                solution = "لا يكفي تساوي القطرين وحده أو توازي زوج واحد؛ أما تنصيف القطرين بعضهما، أو تساوي كل زوج متقابل، أو زوج متقابل متوازٍ ومتساوٍ فيكفي.";
                break;
            }
            case RectangleTest:
            {
                var mode = random.Next(0, 4);
                p["mode"] = mode;
                prompt = (mode switch
                {
                    0 => "متوازي أضلاع قطراه متساويان في الطول. ",
                    1 => "متوازي أضلاع قطرَاه متعامدان، دون معطيات أخرى. ",
                    2 => "متوازي أضلاع له زاوية قائمة واحدة. ",
                    _ => "متوازي أضلاع له ضلعان متقابلان متساويان، دون معطيات أخرى. "
                }) + "هل المعطيات كافية لإثبات أنه مستطيل؟ اكتب 1 لنعم أو 0 للا.";
                solution = "متوازي الأضلاع ذو الزاوية القائمة مستطيل، وكذلك ذو القطرين المتساويين. أما تعامد القطرين أو تساوي الضلعين المتقابلين وحده فلا يكفي.";
                break;
            }
            case RhombusDiagonals:
            {
                var a = random.Next(3, 18); var b = random.Next(3, 18);
                var mode = random.Next(0, 3);
                p["a"] = a; p["b"] = b; p["mode"] = mode;
                prompt = mode switch
                {
                    0 => $"معين طول قطره الأول {2*a} سم، ويتقاطع قطراه في نقطة المنتصف م. ما طول نصف هذا القطر بالسنتيمتر؟",
                    1 => $"معين طول قطره الثاني {2*b} سم، ويتقاطع قطراه في نقطة المنتصف م. ما طول نصف هذا القطر بالسنتيمتر؟",
                    _ => $"معين قطراه المتعامدان طولهما {2*a} سم و{2*b} سم. ما مساحة المعين بالسنتيمتر المربع؟"
                };
                solution = "قطرا المعين متعامدان وينصف كل منهما الآخر، ومساحته نصف حاصل ضرب طوليهما.";
                break;
            }
            case SystemSolutionCount:
            {
                var mode=random.Next(0,3); var a=random.Next(2,16);
                var b=random.Next(2,15);
                p["mode"]=mode;p["a"]=a;p["b"]=b;
                prompt=mode switch
                {
                    0 => $"النظام x+y={a} وx-y={b}. كم حلًا مختلفًا له؟ اكتب 1 لحل واحد، 0 لعدم وجود حل، أو 2 لعدد لا نهائي من الحلول.",
                    1 => $"النظام x+y={a} و2x+2y={2*a+1}. كم حلًا له؟ اكتب 1 لحل واحد، 0 لعدم وجود حل، أو 2 لعدد لا نهائي.",
                    _ => $"النظام x+y={a} و2x+2y={2*a}. كم حلًا له؟ اكتب 1 لحل واحد، 0 لعدم وجود حل، أو 2 لعدد لا نهائي."
                };
                solution="إذا تقاطع المستقيمان كان هناك حل واحد، وإذا توازيا واختلفا لم يوجد حل، وإذا تطابقا كان عدد الحلول لا نهائيًا.";
                break;
            }
            case ScatterDirection:
            {
                var mode=random.Next(0,3);
                p["mode"]=mode;
                prompt=(mode switch
                {
                    0 => "نقاط الانتشار (1،3)،(2،6)،(3،9)،(4،12)،(5،15). ",
                    1 => "نقاط الانتشار (1،15)،(2،12)،(3،9)،(4،6)،(5،3). ",
                    _ => "نقاط الانتشار (1،2)،(2،9)،(3،5)،(4،9)،(5،2). "
                })+"صنّف اتجاه الارتباط الخطي: اكتب 1 لموجب، أو 2 لسالب، أو 0 لعدم وجود اتجاه خطي واضح.";
                solution="إذا تزداد الصادات بزيادة السينات فالارتباط موجب، وإذا تتناقص فهو سالب، والتوزيع المتماثل دون اتجاه صاعد أو هابط خطيًا لا يُظهر ارتباطًا خطيًا واضحًا.";
                break;
            }
            case DistributionShape:
            {
                var mode=random.Next(0,5);
                p["mode"]=mode;
                int[][] frequencies=[
                    [3,3,3,3,3],[1,3,5,3,1],[8,5,3,2,1],[1,2,3,5,8],[6,2,1,2,6]
                ];
                prompt=$"لقيم البيانات المرتبة 1،2،3،4،5 التكرارات المناظرة {string.Join("،",frequencies[mode])}. صنّف شكل التوزيع: 1 منتظم، 2 متناظر شبيه بالجرس، 3 ملتوي لليمين، 4 ملتوي لليسار، 5 ثنائي القمة. اكتب رقم الاختيار.";
                solution="قارن تكرارات الأطراف والوسط: التساوي يعني انتظامًا، وزيادة الوسط بتناظر تعني شكلًا جرسيًا، وانحراف الذيل يحدد اتجاه الالتواء، ووجود قمتين يفيد ثنائية القمة.";
                break;
            }
            default: throw new InvalidOperationException("Unknown G8 supplemental family.");
        }
        return new Item(family, prompt, solution, itemType, p);
    }

    private static bool ExactKeys(IReadOnlyDictionary<string, int> p, params string[] keys) =>
        p.Count == keys.Length && keys.All(p.ContainsKey);

    internal static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Supports(family)) throw new InvalidOperationException("Unsupported family.");
        int expected;
        string? symbolic = null;
        switch (family)
        {
            case MonomialMultiply when ExactKeys(p,"a","b","m","n") &&
                p["a"] is >=2 and <=9 && p["b"] is >=2 and <=9 &&
                p["m"] is >=1 and <=5 && p["n"] is >=1 and <=5:
                symbolic=Mono(p["a"]*p["b"],p["m"]+p["n"]); break;
            case MonomialDivide when ExactKeys(p,"a","q","m","n") &&
                p["a"] is >=2 and <=6 && p["q"] is >=2 and <=8 &&
                p["m"] is >=1 and <=5 && p["n"] is >=1 and <=4:
                symbolic=Mono(p["q"],p["n"]); break;
            case MonomialPower when ExactKeys(p,"a","m","n") &&
                p["a"] is >=2 and <=5 && p["m"] is >=1 and <=4 && p["n"] is >=2 and <=4:
                symbolic=Mono(Pow(p["a"],p["n"]),p["m"]*p["n"]); break;
            case NegativeExponent when ExactKeys(p,"a","e") &&
                p["a"] is >=2 and <=6 && p["e"] is >=1 and <=4:
                symbolic=$"1/{Pow(p["a"],p["e"])}"; break;
            case SquareRootExact:
            case CubeRootExact:
                if (!ExactKeys(p,"k") || p["k"] is <2 or >16) throw new InvalidOperationException("Invalid root parameters.");
                expected=p["k"]; return expected.ToString(CultureInfo.InvariantCulture);
            case SquareRootBounds:
            case CubeRootBounds:
                if (!ExactKeys(p,"k","extra") || p["k"] is <2 or >11 ||
                    p["extra"]<1 || p["extra"]>Math.Min(
                        family==SquareRootBounds?2*p["k"]:3*p["k"]*p["k"]+3*p["k"],25))
                    throw new InvalidOperationException("Invalid root bounds.");
                return p["k"].ToString(CultureInfo.InvariantCulture);
            case WriteTwoStepEquation when ExactKeys(p,"a","b") &&
                p["a"] is >=2 and <=14 && p["b"] is >=2 and <=30:
                symbolic=$"y={p["a"]}x+{p["b"]}"; break;
            case DirectVariation when ExactKeys(p,"a","x") &&
                p["a"] is >=1 and <=9 && p["x"] is >=-9 and <=9:
                expected=p["a"]*p["x"]; return expected.ToString(CultureInfo.InvariantCulture);
            case SegmentAddition when ExactKeys(p,"a","b") &&
                p["a"] is >=2 and <=30 && p["b"] is >=2 and <=30:
                expected=p["a"]+p["b"]; return expected.ToString(CultureInfo.InvariantCulture);
            case SegmentMidpoint when ExactKeys(p,"x","y","dx","dy","component") &&
                p["x"] is >=-12 and <=12 && p["y"] is >=-12 and <=12 &&
                p["dx"] is >=1 and <=8 && p["dy"] is >=1 and <=8 &&
                p["component"] is 0 or 1:
                expected=p["component"]==0?p["x"]:p["y"];
                return expected.ToString(CultureInfo.InvariantCulture);
            case SegmentProof when ExactKeys(p,"mode") && p["mode"] is >=0 and <=2:
                expected=p["mode"]+1; return expected.ToString(CultureInfo.InvariantCulture);
            case ParallelogramTest when ExactKeys(p,"mode") && p["mode"] is >=0 and <=4:
                expected=p["mode"]<=2?1:0; return expected.ToString(CultureInfo.InvariantCulture);
            case RectangleTest when ExactKeys(p,"mode") && p["mode"] is >=0 and <=3:
                expected=p["mode"] is 0 or 2?1:0;
                return expected.ToString(CultureInfo.InvariantCulture);
            case RhombusDiagonals when ExactKeys(p,"a","b","mode") &&
                p["a"] is >=3 and <=17 && p["b"] is >=3 and <=17 &&
                p["mode"] is >=0 and <=2:
                expected=p["mode"] switch {0=>p["a"],1=>p["b"],_=>2*p["a"]*p["b"]};
                return expected.ToString(CultureInfo.InvariantCulture);
            case SystemSolutionCount when ExactKeys(p,"mode","a","b") &&
                p["mode"] is >=0 and <=2 && p["a"] is >=2 and <=15 &&
                p["b"] is >=2 and <=14:
                expected=p["mode"] switch{0=>1,1=>0,_=>2};
                return expected.ToString(CultureInfo.InvariantCulture);
            case ScatterDirection when ExactKeys(p,"mode") && p["mode"] is >=0 and <=2:
                expected=p["mode"] switch{0=>1,1=>2,_=>0};
                return expected.ToString(CultureInfo.InvariantCulture);
            case DistributionShape when ExactKeys(p,"mode") && p["mode"] is >=0 and <=4:
                expected=p["mode"]+1;
                return expected.ToString(CultureInfo.InvariantCulture);
            default: throw new InvalidOperationException("Invalid G8 supplemental question parameters.");
        }
        return symbolic ?? throw new InvalidOperationException("No G8 symbolic solution.");
    }

    internal static bool Verify(string family, IReadOnlyDictionary<string,int> parameters, string? answer)
    {
        if (string.IsNullOrWhiteSpace(answer) || answer.Length>100) return false;
        string expected;
        try { expected=Solve(family,parameters); }
        catch (InvalidOperationException) { return false; }
        var got=string.Concat(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩' => (char)('0'+c-'٠'),
            >= '۰' and <= '۹' => (char)('0'+c-'۰'),
            '−' => '-',
            _ => c
        })).Replace(" ","",StringComparison.Ordinal);
        return string.Equals(got,expected,StringComparison.OrdinalIgnoreCase);
    }
}
