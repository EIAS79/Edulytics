using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit7AlgebraRulesQuestions
{
    internal const string Add="uae.g4.u07.l05.input_add_rule";
    internal const string Subtract="uae.g4.u07.l05.input_subtract_rule";
    internal const string Multiply="uae.g4.u07.l06.input_multiply_rule";
    internal const string Divide="uae.g4.u07.l06.input_divide_rule";
    internal const string Order="uae.g4.u07.l07.order_multiply_before_add";
    internal const string TwoStep="uae.g4.u07.l08.two_step_rule";
    internal const string MultiStep="uae.g4.u07.l09.multi_step_rule";
    // Grade 5 Arabic bar-model numeric expressions reuse the same exact
    // parameter/solve/verify algebra engine, without changing Grade 4.
    internal const string G5Equal="uae.g5.u06.l01.equal_group_bar_total";
    internal const string G5Two="uae.g5.u06.l01.two_group_bar_total";
    internal const string G5Paired="uae.g5.u06.l01.paired_group_expression_value";
    private static bool IsG5Bar(string? family)=>family is G5Equal or G5Two or G5Paired;
    internal const string G5OrderBasic="uae.g5.u06.l02.multiply_before_add";
    internal const string G5OrderNested="uae.g5.u06.l02.nested_parentheses";
    internal const string G5OrderPowers="uae.g5.u06.l02.exponents_before_products";
    private static bool IsG5Order(string? f)=>f is G5OrderBasic or G5OrderNested or G5OrderPowers;
    internal const string G5WriteMultiplySubtract="uae.g5.u06.l03.write_multiply_then_subtract";
    internal const string G5WriteSumDivide="uae.g5.u06.l03.write_sum_then_divide";
    internal const string G5WriteGroupSum="uae.g5.u06.l03.write_group_times_sum";
    private static bool IsG5Writing(string? f)=>f is G5WriteMultiplySubtract or G5WriteSumDivide or G5WriteGroupSum;
    internal const string G5SubBracket="uae.g5.u06.l05.substitute_bracket_add";
    internal const string G5SubSquare="uae.g5.u06.l05.substitute_squared_subtract";
    internal const string G5SubDecimal="uae.g5.u06.l05.substitute_decimal_values";
    internal const string G5ModelAdd="uae.g5.u06.l06.model_unknown_plus";
    internal const string G5ModelSubtract="uae.g5.u06.l06.model_unknown_minus";
    internal const string G5ModelGroups="uae.g5.u06.l06.model_repeated_group";
    private static bool IsG5Model(string? f)=>f is G5ModelAdd or G5ModelSubtract or G5ModelGroups;
    private static bool IsG5SubInt(string? f)=>f is G5SubBracket or G5SubSquare;
    private static bool IsG5SubDecimal(string? f)=>f==G5SubDecimal;
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Add or Subtract or Multiply or Divide or Order or TwoStep or MultiStep || IsG5Bar(f) || IsG5Order(f) || IsG5Writing(f) || IsG5Model(f) || IsG5SubInt(f) || IsG5SubDecimal(f);
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported algebra rule family");
        if(IsG5Model(f))return BuildG5Model(f,r);
        if(IsG5SubInt(f)||IsG5SubDecimal(f))return BuildG5Sub(f,r);
        if(IsG5Writing(f))return BuildG5Writing(f,r);
        if(IsG5Order(f))return BuildG5Order(f,r);
        if(IsG5Bar(f))return BuildG5Bar(f,r);
        int n=r.Next(15,76),change=r.Next(2,10),factor=r.Next(2,7);
        var p=new Dictionary<string,int>{{"input",n},{"change",change},{"factor",factor}};
        string question=f switch
        {
            Add=>$"في جدول المدخل والمخرج، القاعدة y = x + {change}. إذا كان x = {n}، فما قيمة y؟",
            Subtract=>$"في جدول المدخل والمخرج، القاعدة y = x − {change}. إذا كان x = {n}، فما قيمة y؟",
            Multiply=>$"في جدول المدخل والمخرج، القاعدة y = {factor} × x. إذا كان x = {n}، فما قيمة y؟",
            Divide=>$"القاعدة y = x ÷ {factor}. إذا كان x = {n*factor}، فما قيمة y؟",
            Order=>$"أوجد قيمة التعبير {n} + {change} × {factor} وفق ترتيب العمليات.",
            TwoStep=>$"أكمل جدول المعادلة y = (x + {change}) × {factor} عندما يكون x = {n}.",
            _=>$"أوجد المخرج y في المعادلة y = x × {factor} + {change}، حيث x = {n}."
        };
        return new Item(f,question,"أستبدل المدخل بقيمته وأطبق العمليات وفق ترتيبها؛ ثم أتحقق بعكس العملية أو إعادة الحساب.",
            AssessmentItemType.Numeric,p);
    }

    private static Item BuildG5Bar(string family,Random rng)
    {
        int n=rng.Next(2,7),a=rng.Next(3,13);
        int m=family==G5Equal?0:family==G5Paired?n:rng.Next(2,7);
        int b=family==G5Equal?0:rng.Next(3,13);
        string prompt=family switch
        {
            G5Equal=>$"في شريط يوجد {n} أجزاء متساوية، قيمة كل جزء {a}. اكتب تعبير ضرب يمثل الشريط، ثم احسب إجمالي الأجزاء.",
            G5Two=>$"يمثل شريط {n} مجموعات كل منها {a} وأيضًا {m} مجموعات كل منها {b}. اكتب تعبيرًا باستعمال الضرب والجمع ثم احسب قيمة الشريط.",
            _=>$"يتكرر في الرسم جزء قيمته {a} وجزء قيمته {b} في {n} أزواج. احسب قيمة تعبير مكافئ: {n} × ({a} + {b})."
        };
        return new Item(family,prompt,
            "أحدد الأجزاء المتساوية وأكتب المجموع بالجمع المتكرر أو الضرب وأبرر تكافؤ التعبيرين.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"n",n},{"a",a},{"m",m},{"b",b}});
    }
    private static bool ValidG5Bar(string? f,IReadOnlyDictionary<string,int> p,out int value)
    {
        value=0;
        if(!IsG5Bar(f)||p.Count!=4||!p.TryGetValue("n",out int n)||
            !p.TryGetValue("a",out int a)||!p.TryGetValue("m",out int m)||
            !p.TryGetValue("b",out int b)||n is <2 or >6||a is <3 or >12)
            return false;
        if(f==G5Equal)
        {
            if(m!=0||b!=0)return false;
        }
        else
        {
            if(m is <2 or >6||b is <3 or >12|| (f==G5Paired&&m!=n))
                return false;
        }
        value=checked(n*a+m*b);
        return true;
    }


    private static int Grade5Pow(int n,int exponent)
    {
        int value=1;
        for(int i=0;i<exponent;i++)value=checked(value*n);
        return value;
    }
    private static Item BuildG5Order(string f,Random r)
    {
        int a,b,c,d;
        string question;
        if(f==G5OrderBasic)
        {
            a=r.Next(15,71);b=r.Next(2,10);c=r.Next(2,10);d=0;
            question=$"احسب {a} + {b} × {c}. ابدأ بالضرب ثم الجمع.";
        }
        else if(f==G5OrderNested)
        {
            b=r.Next(2,13);d=r.Next(2,9);int q=r.Next(2,13);
            c=d*q;a=b+q+r.Next(5,35);
            question=$"أوجد قيمة {a} − {{ {b} + ({c} ÷ {d}) }} مبتدئًا بالأقواس الداخلية.";
        }
        else
        {
            a=r.Next(2,7);b=r.Next(2,4);c=r.Next(2,12);d=r.Next(2,12);
            question=$"احسب {a} أس {b} + {c} × {d} وفق ترتيب العمليات.";
        }
        return new Item(f,question,
            "أحسب الأقواس ثم الأسس ثم الضرب والقسمة من اليسار لليمين ثم الجمع والطرح من اليسار لليمين.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"d",d}});
    }
    private static bool ValidG5Order(string? f,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!IsG5Order(f)||p.Count!=4||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("c",out int c)||!p.TryGetValue("d",out int d))
            return false;
        if(f==G5OrderBasic)
        {
            if(a is <15 or >70||b is <2 or >9||c is <2 or >9||d!=0)return false;
            result=checked(a+b*c);
        }
        else if(f==G5OrderNested)
        {
            if(b is <2 or >12||d is <2 or >8||c%d!=0||
               c/d is <2 or >12||a<b+c/d+5||a>b+c/d+34)return false;
            result=checked(a-(b+c/d));
        }
        else
        {
            if(a is <2 or >6||b is <2 or >3||c is <2 or >11||d is <2 or >11)return false;
            result=checked(Grade5Pow(a,b)+c*d);
        }
        return result>=0;
    }


    // Writing expressions needs exact symbolic answers rather than a numeric
    // answer disguised as the target skill. The teacher reviews other equivalent
    // expressions beyond the two normalized variants accepted below.
    private static Item BuildG5Writing(string f,Random r)
    {
        int a=r.Next(2,7),b=r.Next(3,13),c;
        string question;
        if(f==G5WriteMultiplySubtract)
        {
            c=r.Next(1,Math.Min(6,a*b));
            question=$"اكتب تعبيرًا عدديًا دون إيجاد قيمته: اشترى {a} أشخاص تذاكر ثمن الواحدة {b} دراهم، ثم خُصم {c} دراهم من المجموع.";
        }
        else if(f==G5WriteSumDivide)
        {
            c=r.Next(2,7);
            question=$"اكتب تعبيرًا عدديًا: اجمع {a} و{b} أولًا ثم اقسم المجموع على {c}. لا تحسب القيمة.";
        }
        else
        {
            c=r.Next(3,13);
            question=$"اكتب تعبيرًا عدديًا: اشترى كل واحد من {a} أصدقاء غرضًا بسعر {b} درهمًا وآخر بسعر {c} دراهم. لا تحسب الإجمالي.";
        }
        return new Item(f,question,
            "أقرأ تسلسل الكلمات: ثم، مجموع، خصم، ولكل؛ أستخدم الأقواس إذا كان الجمع أو الطرح يجب إجراؤه قبل الضرب أو القسمة.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}});
    }
    private static bool ValidG5Writing(string f,IReadOnlyDictionary<string,int> p,
        out int a,out int b,out int c)
    {
        a=b=c=0;
        if(!IsG5Writing(f)||p.Count!=3||
            !p.TryGetValue("a",out a)||!p.TryGetValue("b",out b)||
            !p.TryGetValue("c",out c)||a is <2 or >6||b is <3 or >12)
            return false;
        if(f==G5WriteMultiplySubtract)
            return c>=1&&c<=5&&c<a*b;
        if(f==G5WriteSumDivide)
            return c is >=2 and <=6;
        return c is >=3 and <=12;
    }
    private static string SolveG5Writing(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5Writing(f,p,out int a,out int b,out int c))
            throw new InvalidOperationException("Invalid numeric-expression authoring parameters.");
        return f switch
        {
            G5WriteMultiplySubtract=>$"{a}×{b}−{c}",
            G5WriteSumDivide=>$"({a}+{b})÷{c}",
            _=>$"{a}×({b}+{c})"
        };
    }
    private static bool VerifyG5Writing(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5Writing(f,p,out int a,out int b,out int c)||
           string.IsNullOrWhiteSpace(answer)||answer.Length>120)return false;
        string normalized=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch)&&ch!='\u00a0')
            .Select(ch=>ch switch
            {
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                '*'=>'×','/'=>'÷','-'=>'−',_=>ch
            }).ToArray());
        string[] accepted=f switch
        {
            G5WriteMultiplySubtract=>
                [$"{a}×{b}−{c}",$"{b}×{a}−{c}"],
            G5WriteSumDivide=>
                [$"({a}+{b})÷{c}",$"({b}+{a})÷{c}"],
            _=>[$"{a}×({b}+{c})",$"{a}×({c}+{b})",
                 $"({b}+{c})×{a}",$"({c}+{b})×{a}"]
        };
        return accepted.Contains(normalized,StringComparer.Ordinal);
    }


    // UAE Grade 5 Arabic student U06 L06 printed 437–440.
    // Original model-to-expression tasks reuse this exact symbolic engine
    // rather than introducing a separate solver or generic signed algebra.
    private static Item BuildG5Model(string f,Random r)
    {
        int n=r.Next(2,10);
        int groups=f==G5ModelGroups?r.Next(2,6):0;
        string prompt=f switch
        {
            G5ModelAdd=>$"يوجد صندوق مغلق به x قطع غير معلومة، وإلى جانبه {n} قطع ظاهرة. اكتب تعبيرًا جبريًا يمثل إجمالي القطع. لا تفترض أن الصندوق فارغ.",
            G5ModelSubtract=>$"كان في صندوق x قطعة، ثم أخذنا منه {n} قطع. اكتب تعبيرًا جبريًا لعدد القطع الباقية. انتبه لترتيب الطرح.",
            _=>$"لديك {groups} مجموعات متشابهة، في كل مجموعة صندوق فيه x قطع غير معلومة ومعه {n} قطع ظاهرة. اكتب تعبيرًا جبريًا للمجموع الكلي دون إيجاد قيمته."
        };
        return new Item(f,prompt,
            "أحدد العدد المجهول x وأمثله بصندوق، ثم أترجم الإضافة أو الإزالة أو تكرار المجموعة مع الحفاظ على الأقواس.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string,int>{{"n",n},{"groups",groups}});
    }
    private static bool ValidG5Model(string f,IReadOnlyDictionary<string,int> p,
        out int n,out int groups)
    {
        n=groups=0;
        return IsG5Model(f)&&p.Count==2&&p.TryGetValue("n",out n)&&
            p.TryGetValue("groups",out groups)&&n is >=2 and <=9&&
            (f==G5ModelGroups?groups is >=2 and <=5:groups==0);
    }
    private static string SolveG5Model(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5Model(f,p,out int n,out int groups))
            throw new InvalidOperationException("Invalid source model expression parameters");
        return f switch
        {
            G5ModelAdd=>$"x+{n}",
            G5ModelSubtract=>$"x−{n}",
            _=>$"{groups}×(x+{n})"
        };
    }
    private static bool VerifyG5Model(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5Model(f,p,out int n,out int groups)||
           string.IsNullOrWhiteSpace(answer)||answer.Length>80)return false;
        string value=new(answer.Trim()
            .Where(ch=>!char.IsWhiteSpace(ch)&&ch!='\u00a0')
            .Select(ch=>ch switch
            {
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                'س' or 'X'=>'x',
                '*' or '·'=>'×',
                '-'=>'−',
                _=>ch
            }).ToArray());
        string[] accepted=f switch
        {
            G5ModelAdd=>[$"x+{n}",$"{n}+x"],
            G5ModelSubtract=>[$"x−{n}"],
            _=>[
                $"{groups}(x+{n})",
                $"{groups}×(x+{n})",
                $"{groups}×({n}+x)",
                $"(x+{n})×{groups}",
                $"({n}+x)×{groups}",
                $"{groups}x+{groups*n}",
                $"{groups}×x+{groups*n}",
                $"{groups*n}+{groups}x",
                $"{groups*n}+{groups}×x"
            ]
        };
        return accepted.Contains(value,StringComparer.Ordinal);
    }

    private static Item BuildG5Sub(string f,Random r)
    {
        if(IsG5SubDecimal(f))
        {
            int a=r.Next(10,291),b=r.Next(100,1201),c=r.Next(a,1501);
            var p=new Dictionary<string,int>{{"aHundredths",a},{"bHundredths",b},{"cHundredths",c}};
            string prompt=$"إذا كان a={a/100m:0.##} وb={b/100m:0.##} وc={c/100m:0.##}، أوجد قيمة (c − a) + b.";
            return new Item(f,prompt,"أستبدل المتغيرات بقيمها وأجمع وأطرح الأعداد العشرية مع محاذاة الفواصل.",
                AssessmentItemType.Numeric,p);
        }
        int x=r.Next(2,11),y=r.Next(x,20),k=r.Next(2,10);
        // Upper bound is exclusive: never generate k >= a*a, which the validator rejects.
        if(f==G5SubSquare)k=r.Next(2,Math.Min(10,x*x));
        string promptInt=f==G5SubBracket
            ?$"إذا كان a={x} وb={y}، أوجد قيمة (b − a) + {k}."
            :$"إذا كان a={x}، أوجد قيمة a × a − {k}.";
        return new Item(f,promptInt,"استبدل الحرف بقيمته العددية، ثم احسب الأقواس أو الضرب قبل الطرح.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"a",x},{"b",y},{"c",k}});
    }
    private static bool ValidG5SubInt(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5SubInt(f)||p.Count!=3||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("c",out int k)||a is <2 or >10||
           b<a||b>19||k is <2 or >9)return false;
        if(f==G5SubSquare&&k>=a*a)return false;
        expected=f==G5SubBracket?b-a+k:a*a-k;
        return true;
    }
    private static bool ValidG5SubDecimal(IReadOnlyDictionary<string,int> p,
        out int a,out int b,out int c)
    {
        a=b=c=0;
        return p.Count==3&&
           p.TryGetValue("aHundredths",out a)&&p.TryGetValue("bHundredths",out b)&&
           p.TryGetValue("cHundredths",out c)&&
           a is >=10 and <=290&&b is >=100 and <=1200&&c>=a&&c<=1500;
    }
    private static string SolveG5SubDecimal(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!IsG5SubDecimal(f)||!ValidG5SubDecimal(p,out int a,out int b,out int c))
            throw new InvalidOperationException("Invalid decimal substitution parameters");
        return ((c-a+b)/100m).ToString("0.##",CultureInfo.InvariantCulture);
    }
    private static bool VerifyG5SubDecimal(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!IsG5SubDecimal(f)||!ValidG5SubDecimal(p,out int a,out int b,out int c)||
           string.IsNullOrWhiteSpace(answer)||answer.Length>24)return false;
        string s=new(answer.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '٫'=>'.',_=>ch
        }).Where(ch=>ch is not ' ' and not '٬' and not '\u00a0').ToArray());
        if(!s.All(ch=>ch is >= '0' and <= '9' or '.'))return false;
        return decimal.TryParse(s,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,
            out decimal actual)&&actual*100m==c-a+b;
    }

    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int x)
    {
        x=0;
        if(IsG5SubInt(f))return ValidG5SubInt(f,p,out x);
        if(IsG5Order(f))return ValidG5Order(f,p,out x);
        if(IsG5Bar(f))return ValidG5Bar(f,p,out x);
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("input",out int n)||
           !p.TryGetValue("change",out int c)||!p.TryGetValue("factor",out int k)||
           n is <15 or >75||c is <2 or >9||k is <2 or >6)return false;
        x=f switch
        {
            Add=>n+c,
            Subtract=>n-c,
            Multiply=>n*k,
            Divide=>n,
            Order=>n+c*k,
            TwoStep=>(n+c)*k,
            _=>n*k+c
        };
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(IsG5Model(f))return SolveG5Model(f,p);
        if(IsG5SubDecimal(f))return SolveG5SubDecimal(f,p);
        if(IsG5Writing(f))return SolveG5Writing(f,p);
        if(!Valid(f,p,out int x))throw new InvalidOperationException("Invalid input-output rule");
        return x.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(IsG5Model(f))return VerifyG5Model(f,p,answer);
        if(IsG5SubDecimal(f))return VerifyG5SubDecimal(f,p,answer);
        if(IsG5Writing(f))return VerifyG5Writing(f,p,answer);
        if(!Valid(f,p,out int x)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&x==actual;
    }
}
