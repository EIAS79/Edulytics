using System.Globalization;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Arabic learner prompts for shared exact question families only when invoked
/// by a UAE Grade 9 General lesson identity. Mathematical parameters, numeric
/// solution, independent verifier and original other-curriculum prompts remain
/// completely unchanged. No publisher exercise wording is reproduced.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem LocalizeUaeGrade9GeneralArabic(ExactProblem problem)
    {
        // Existing hand-authored exact G9 prompts have already been localized
        // per-lesson in Generate. Never replace those lesson-specific contexts.
        if (problem.Prompt.Any(c => c is >= '\u0600' and <= '\u06ff'))
            return problem;

        var p = problem.Parameters;
        static int Value(IReadOnlyDictionary<string, int> x, string key) =>
            x.TryGetValue(key, out var number) ? number
            : throw new InvalidOperationException(
                "Grade9 Arabic exact family is missing required parameter: " + key);
        static string Signed(int n) =>
            n < 0 ? $"− {Math.Abs((long)n)}" : $"+ {n}";
        static string AlgebraTerm(int n, string variable = "x") =>
            n == 1 ? variable : n == -1 ? "−" + variable : n.ToString(CultureInfo.InvariantCulture) + variable;
        string P(string key) => Value(p, key).ToString(CultureInfo.InvariantCulture);

        var localized = problem.Family switch
        {
            "algebra.linear.ax_plus_b_equals_c" =>
                problem with
                {
                    Prompt = $"حل المعادلة {AlgebraTerm(Value(p,"coefficient"))} {Signed(Value(p,"offset"))} = {P("right")}. أوجد قيمة x وتحقق بالتعويض.",
                    Solution = "اجمع الحدود الثابتة على جهة واحدة، ثم اقسم على معامل المتغير غير الصفري وتحقق بالتعويض."
                },

            "functions.relations.unique_output.classify" =>
                TranslateRelationClassification(problem),

            "functions.notation.evaluate_context" =>
                problem with
                {
                    Prompt = $"تعطي الدالة H(t) قيمة مقاسة بالأمتار بعد t ساعة وفق القاعدة H(t)={P("initial")}+({P("rate")})t. احسب H({P("input")}). اكتب الناتج العددي الصحيح فقط، وناقش معنى التمثيل البياني مع المعلم.",
                    Solution = "عوض قيمة t المطلوبة في قاعدة الدالة، واحسب المقدار العددي قبل تفسيره في السياق."
                },

            "geometry.coordinate.y_intercept_from_rule" =>
                problem with
                {
                    Prompt = $"للمستقيم ص = ({P("gradient")})س + ({P("intercept")})، أوجد المقطع الصادي، أي قيمة ص عندما س=٠.",
                    Solution = "عند س=٠ يبقى الحد الثابت وحده، وهو المقطع الصادي."
                },

            "supporting.algebra.equations.mixed" =>
                problem with
                {
                    Prompt = $"حل المعادلة {AlgebraTerm(Value(p,"a"))} {Signed(Value(p,"b"))} = {P("right")}. اكتب قيمة x.",
                    Solution = "اعزل حد المتغير بإجراء العملية العكسية على الطرفين، ثم تحقق بالتعويض."
                },

            "supporting.algebra.expand" =>
                problem with
                {
                    Prompt = $"استخدم خاصية التوزيع لتوسيع التعبير {P("a")}(x {Signed(Value(p,"b"))}). اكتب التعبير الجبري المكافئ.",
                    Solution = "وزّع العامل خارج القوس على كل حد داخله، مع الاحتفاظ بإشارة الحد الثابت."
                },

            "algebra.linear.variables_both_sides" =>
                problem with
                {
                    Prompt = $"حل المعادلة {AlgebraTerm(Value(p,"leftCoefficient"))} {Signed(Value(p,"leftOffset"))} = {AlgebraTerm(Value(p,"rightCoefficient"))} {Signed(Value(p,"rightOffset"))}. أوجد قيمة x.",
                    Solution = "اجمع حدود x في طرف والثوابت في الطرف المقابل، ثم تحقق من الحل في المعادلة الأصلية."
                },

            "ratio.unit_rate.direct" =>
                problem with
                {
                    Prompt = $"تُنتج آلة {P("total")} قطعة خلال {P("quantity")} دقائق بمعدل ثابت. كم قطعة تُنتج في دقيقة واحدة؟ اكتب العدد.",
                    Solution = "اقسم عدد القطع المنتجة على المدة بالدقائق للحصول على معدل إنتاج الدقيقة الواحدة."
                },

            "statistics.center_spread.mean" =>
                problem with
                {
                    Prompt = "أوجد المتوسط الحسابي للقيم " +
                        string.Join("، ", Enumerable.Range(1, Value(p, "count"))
                            .Select(i => P("v" + i))) + ". اكتب الناتج العددي.",
                    Solution = "اجمع جميع القيم ثم اقسم المجموع على عددها."
                },

            "geometry.coordinate.evaluate_linear_rule" =>
                problem with
                {
                    Prompt = $"للمستقيم ص = ({P("gradient")})س + ({P("intercept")})، احسب قيمة ص عندما س = {P("x")}.",
                    Solution = "عوّض قيمة س في قاعدة المستقيم واضرب ثم اجمع الحد الثابت."
                },

            "geometry.coordinate.gradient_between_points" =>
                problem with
                {
                    Prompt = $"أوجد ميل المستقيم المار بالنقطتين ({P("x1")}،{P("y1")}) و({P("x2")}،{P("y2")}). اكتب قيمة الميل العددية.",
                    Solution = "الميل يساوي فرق الإحداثيين الصاديين مقسومًا على فرق الإحداثيين السينيين مع ثبات ترتيب الطرح."
                },

            "ratio.unit_rate.equivalent_ratio" =>
                problem with
                {
                    Prompt = $"أُنتجت {P("baseFirst")} قطع خلال {P("baseSecond")} دقيقة. عند ثبات معدل الوحدة، كم قطعة تُنتج خلال {P("targetSecond")} دقائق؟",
                    Solution = "استخرج معدل الإنتاج في الدقيقة ثم اضربه في عدد الدقائق الجديد."
                },

            "sequences.core.arithmetic_nth_term" =>
                problem with
                {
                    Prompt = $"متتالية حسابية تبدأ بالحدود {P("first")}، {Value(p,"first")+Value(p,"difference")}، {Value(p,"first")+2L*Value(p,"difference")}، ... . أوجد قيمة الحد رقم {P("n")}.",
                    Solution = "استخدم قاعدة الحد النوني للمتتالية الحسابية: الحد الأول زائد حاصل ضرب (رقم الحد ناقص ١) في الفرق المشترك."
                },

            "sequences.core.find_position_arithmetic" =>
                problem with
                {
                    Prompt = $"متتالية حسابية تبدأ بالحدود {P("first")}، {Value(p,"first")+Value(p,"difference")}، {Value(p,"first")+2L*Value(p,"difference")}، ... . ما رقم الحد الذي قيمته {P("target")}؟",
                    Solution = "ساوِ الحد العام بالقيمة المستهدفة، وحل لإيجاد رقم الحد الصحيح الموجب."
                },

            "algebra.linear.inequality.ax_plus_b_relation_c" =>
                TranslateInequality(problem),

            "supporting.powers10.multiply" =>
                problem with
                {
                    Prompt = $"احسب ناتج {P("value")} × 10^{P("exponent")}. اكتب الناتج العددّي.",
                    Solution = "احسب القوة العشرية أولًا ثم اضربها في العدد المعطى."
                },

            "supporting.powers10.evaluate" =>
                problem with
                {
                    Prompt = $"احسب قيمة 10^{P("exponent")}. اكتب الناتج العددّي.",
                    Solution = "قوة العشرة الموجبة تمثل العدد ١ متبوعًا بعدد من الأصفار يساوي الأس."
                },

            "supporting.powers10.divide" =>
                problem with
                {
                    Prompt = $"احسب {P("dividend")} ÷ 10^{P("exponent")}. اكتب الناتج العددّي.",
                    Solution = "قسمة عدد على قوة موجبة للعشرة تعادل إزاحة الفاصلة العشرية يسارًا بعدد من المنازل يساوي الأس."
                },

            "supporting.powers10.missing_exponent" =>
                problem with
                {
                    Prompt = $"إذا كان {P("value")} × 10^n = {P("result")}، فما قيمة الأس n؟ اكتب عددًا صحيحًا.",
                    Solution = "اقسم الطرف الأيمن على العامل الثابت لتحديد قوة العشرة، ثم حدد أسها."
                },

            "sequences.core.geometric_nth_term" =>
                problem with
                {
                    Prompt = $"متتالية هندسية تبدأ بالحدود {P("first")}، {(long)Value(p,"first")*Value(p,"ratio")}، {(long)Value(p,"first")*Value(p,"ratio")*Value(p,"ratio")}، ... . أوجد قيمة الحد رقم {P("n")}.",
                    Solution = "اضرب الحد الأول في النسبة المشتركة مرفوعة للقوة (رقم الحد ناقص ١)."
                },

            "usccss.g7.u07.vertical_angle" =>
                problem with
                {
                    Prompt = $"تقاطَع مستقيمان فنتجت زاويتان متقابلتان بالرأس. إذا كان قياس إحداهما {P("known")}°، فما قياس الأخرى؟",
                    Solution = "الزاويتان المتقابلتان بالرأس متساويتان في القياس."
                },

            "usccss.g7.u07.complementary_angle" =>
                problem with
                {
                    Prompt = $"زاويتان متتامتان، قياس إحداهما {P("known")}°. أوجد قياس الزاوية الأخرى.",
                    Solution = "مجموع الزاويتين المتتامتين يساوي ٩٠°؛ اطرح قياس الزاوية المعروفة."
                },

            "usccss.g4.u07.l03.dot_grid_parallel_intersect" =>
                problem with
                {
                    Prompt = $"على شبكة نقاط مؤلفة من ٥ صفوف و٥ أعمدة، يمر المستقيم الأول بالنقطتين (صف {P("r1")}، عمود {P("c1")}) و(صف {P("r2")}، عمود {P("c2")}). ويمر المستقيم الثاني بالنقطتين (صف {P("r3")}، عمود {P("c3")}) و(صف {P("r4")}، عمود {P("c4")}). عند مد المستقيمين في الاتجاهين، هل هما متوازيان أم متقاطعان؟ أجب بالرقم ١ للمتوازيين أو ٢ للمتقاطعين.",
                    Solution = "قارن اتجاه المستقيمين بعد مدّهما: إذا كان لهما الاتجاه نفسه ولا يلتقيان فهما متوازيان، وإلا فهما متقاطعان. الرسم التفصيلي يراجعه المعلم."
                },

            "geometry.angles.parallel_lines" =>
                TranslateParallelAngles(problem),

            "geometry.angles.supplementary" =>
                problem with
                {
                    Prompt = $"زاويتان متجاورتان تكوّنان زاوية مستقيمة، وقياس إحداهما {P("knownAngle")}°. أوجد قياس الأخرى.",
                    Solution = "مجموع الزاويتين على خط مستقيم يساوي ١٨٠°؛ اطرح القياس المعطى."
                },

            "usccss.g4.u07.l10.perpendicular_right_angle_judgment" =>
                problem with
                {
                    Prompt = $"يتقاطع مستقيمان، وقياس إحدى الزوايا عند نقطة التقاطع {P("angleDegrees")}°. هل المستقيمان متعامدان؟ أجب بالرمز ١ إذا كانا متعامدين وبالرمز ٢ إذا لم يكونا متعامدين. راجع التمثيل بالرسم مع المعلم.",
                    Solution = "المستقيمان متعامدان إذا كانت زاوية التقاطع ٩٠°. يراجع المعلم رسم المستقيمين منفصلًا عن التصحيح العددي."
                },

            "geometry.congruence.identify_criterion" =>
                TranslateCongruenceCriterion(problem),

            _ => throw new InvalidOperationException(
                "Unlocalized UAE Grade9 General Arabic exact Practice family: " +
                problem.Family)
        };
        if (!localized.Prompt.Any(c => c is >= '\u0600' and <= '\u06ff'))
            throw new InvalidOperationException(
                "UAE Grade9 General Arabic localization did not produce Arabic learner instructions for " +
                problem.Family);
        return localized;
    }

    private static ExactProblem TranslateRelationClassification(ExactProblem original)
    {
        const string head = "Is the relation ";
        const string end = " a function of its first coordinate?";
        if (!original.Prompt.StartsWith(head, StringComparison.Ordinal))
            throw new InvalidOperationException("Unexpected function-relation prompt template.");
        var index = original.Prompt.IndexOf(end, head.Length, StringComparison.Ordinal);
        if (index < 0)
            throw new InvalidOperationException("Unexpected function-relation prompt delimiter.");
        var orderedPairs = original.Prompt[head.Length..index];
        return original with
        {
            Prompt = $"هل العلاقة التي تمثلها الأزواج المرتبة {orderedPairs} دالة بالنسبة للإحداثي الأول؟ اكتب yes إذا كانت دالة أو no إذا لم تكن دالة. ناقش الرسم وتفسير المخرجات مع المعلم.",
            Solution = "الدالة لا تعطي القيمة الأولى نفسها ناتجين مختلفين. قارن المخرجات عند تكرار قيمة المدخل."
        };
    }

    private static ExactProblem TranslateInequality(ExactProblem original)
    {
        const string head = "Solve ";
        const string end = ". Give your answer as an inequality in x.";
        if (!original.Prompt.StartsWith(head, StringComparison.Ordinal) ||
            !original.Prompt.EndsWith(end, StringComparison.Ordinal))
            throw new InvalidOperationException("Unexpected linear inequality prompt template.");
        var expression = original.Prompt[head.Length..^end.Length];
        return original with
        {
            Prompt = $"حل المتباينة {expression}. اكتب الإجابة على صورة متباينة بدلالة x، واعكس اتجاه المتباينة عند القسمة على عدد سالب.",
            Solution = "اجمع الحدود المتشابهة واستخدم العمليات العكسية، ثم اعكس علامة المتباينة إذا قسمت على عدد سالب. تحقق من الحدود بالمتباينة الأصلية."
        };
    }

    private static ExactProblem TranslateParallelAngles(ExactProblem original)
    {
        var p = original.Parameters;
        if (!p.TryGetValue("knownAngle", out var angle) ||
            !p.TryGetValue("relationship", out var kind))
            throw new InvalidOperationException("Missing parallel-line angle parameters.");
        return kind switch
        {
            0 => original with
            {
                Prompt = $"مستقيمان متوازيان يقطعهما قاطع. قياس زاوية {angle}°. أوجد قياس الزاوية المناظرة لها.",
                Solution = "الزاويتان المتناظرتان عند قطع مستقيمين متوازيين متساويتان."
            },
            1 => original with
            {
                Prompt = $"مستقيمان متوازيان يقطعهما قاطع. قياس زاوية داخلية {angle}°. أوجد قياس الزاوية الداخلية المجاورة لها على المستقيم نفسه.",
                Solution = "مجموع الزاويتين المتجاورتين على خط مستقيم ١٨٠°."
            },
            _ => throw new InvalidOperationException("Unknown parallel-line angle relationship.")
        };
    }

    private static ExactProblem TranslateCongruenceCriterion(ExactProblem original)
    {
        var p = original.Parameters;
        static int V(IReadOnlyDictionary<string, int> x, string key) =>
            x.TryGetValue(key, out var n) ? n
            : throw new InvalidOperationException("Missing congruence parameter: " + key);
        var criterion = V(p, "criterion");
        string prompt;
        string solution;
        switch (criterion)
        {
            case 0:
                prompt = $"في مثلثين تساوت أطوال الأضلاع المتناظرة الثلاثة {V(p,"sideA")} و{V(p,"sideB")} و{V(p,"sideC")}. أي معيار يثبت التطابق؟ اختر SSS أو SAS أو ASA أو RHS.";
                solution = "تطابق أزواج الأضلاع الثلاثة يحقق مسلمة ضلع–ضلع–ضلع SSS.";
                break;
            case 1:
                prompt = $"في مثلثين تساوى ضلعان متناظران طولهما {V(p,"sideA")} و{V(p,"sideB")}، وتساوت الزاوية المحصورة بينهما وقياسها {V(p,"angleA")}°. أي معيار يثبت التطابق؟ اختر SSS أو SAS أو ASA أو RHS.";
                solution = "تساوي ضلعين والزاوية المحصورة بينهما يحقق مسلمة ضلع–زاوية–ضلع SAS.";
                break;
            case 2:
                prompt = $"في مثلثين تساوت زاويتان متناظرتان قياساهما {V(p,"angleA")}° و{V(p,"angleB")}° وتساوى الضلع المحصور بينهما وطوله {V(p,"sideA")}. أي معيار يثبت التطابق؟ اختر SSS أو SAS أو ASA أو RHS.";
                solution = "تساوي زاويتين والضلع المحصور بينهما يحقق معيار زاوية–ضلع–زاوية ASA.";
                break;
            case 3:
                prompt = $"مثلثان قائمان تساوى وتر كل منهما وطوله {V(p,"sideC")} وتساوى ضلع قائم متناظر وطوله {V(p,"sideA")}. أي معيار يثبت التطابق؟ اختر SSS أو SAS أو ASA أو RHS.";
                solution = "الزاوية القائمة مع تساوي الوتر وضلع قائم متناظر تثبت التطابق بمعيار RHS.";
                break;
            default:
                throw new InvalidOperationException("Unknown triangle congruence criterion.");
        }
        return original with { Prompt = prompt, Solution = solution };
    }
}
