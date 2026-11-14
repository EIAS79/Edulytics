using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit4Lesson01MultiplesOfPowersOfTenQuestions
{
    internal const string Multiply="uae.g4.u04.l01.multiply_power_of_ten";
    internal const string Unknown="uae.g4.u04.l01.unknown_factor_power_ten";
    internal const string Compare="uae.g4.u04.l01.compare_powers_ten";
    internal const string G5Power="uae.g5.u02.l03.evaluate_integer_power";
    internal const string G5Count="uae.g5.u02.l03.count_repeated_factor";
    internal const string G5Cube="uae.g5.u02.l03.cube_volume";
    internal const string G5PowerTen="uae.g5.u02.l04.multiply_by_power_ten";
    internal const string G5TrailingZeros="uae.g5.u02.l04.product_with_trailing_zeros";
    internal const string G5MissingPowerTen="uae.g5.u02.l04.find_missing_power_ten";
    private static bool IsG5Multiply(string? f)=>f is G5PowerTen or G5TrailingZeros or G5MissingPowerTen;
    private static bool IsG5(string? f)=>f is G5Power or G5Count or G5Cube;
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Multiply or Unknown or Compare || IsG5(f) || IsG5Multiply(f);
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown power question family");
        if(IsG5(f))return BuildG5(f,random);
        if(IsG5Multiply(f))return BuildG5Multiply(f,random);
        int factor=random.Next(2,10),baseDigit=random.Next(2,10);
        int zeros=random.Next(1,4),place=(int)Math.Pow(10,zeros),quantity=baseDigit*place;
        string prompt=f switch
        {
            Multiply=>$"أوجد حاصل ضرب {factor} × {quantity} باستخدام نمط مضاعفات ١٠ و١٠٠ و١٠٠٠.",
            Unknown=>$"أكمل جملة الضرب: {factor} × □ = {factor*quantity}. ما قيمة المجهول؟",
            _=>$"أي الناتجين أكبر: {factor} × {quantity} أم {factor} × {quantity/10}؟ اكتب ١ إذا كان الأول أكبر و٠ خلاف ذلك."
        };
        return new Item(f,prompt,"أضرب الأرقام الأساسية ثم أستخدم القيمة المكانية لمضاعفات عشرة.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"factor",factor},{"baseDigit",baseDigit},{"zeros",zeros}});
    }

    // Reuses the G4 exact powers-of-ten engine, extending to arbitrary
    // positive integer bases and exponent interpretation for G5.
    private static int Power(int number,int exponent)
    {
        int result=1;
        for(int i=0;i<exponent;i++)result=checked(result*number);
        return result;
    }

    private static int PowTen(int n)
    {
        int result=1;
        for(int i=0;i<n;i++)result=checked(result*10);
        return result;
    }
    private static Item BuildG5Multiply(string f,Random rng)
    {
        string prompt;
        IReadOnlyDictionary<string,int> parameters;
        if(f==G5PowerTen)
        {
            int factor=rng.Next(2,501),power=rng.Next(1,5);
            prompt=$"احسب ذهنيًا {factor} × ١٠ أس {power}.";
            parameters=new Dictionary<string,int>{{"factor",factor},{"pow",power},{"mode",0}};
        }
        else if(f==G5TrailingZeros)
        {
            int a=rng.Next(2,21),b=rng.Next(2,21);
            int za=rng.Next(1,3),zb=rng.Next(1,4);
            prompt=$"أوجد حاصل ضرب {a*PowTen(za)} × {b*PowTen(zb)} باستخدام الأرقام غير الصفرية والأصفار.";
            parameters=new Dictionary<string,int>{{"a",a},{"b",b},{"za",za},{"zb",zb},{"mode",1}};
        }
        else
        {
            int factor=rng.Next(2,201),power=rng.Next(1,5),product=factor*PowTen(power);
            prompt=$"أكمل القوة: {factor} × ١٠ أس □ = {product}. ما الأس المجهول؟";
            parameters=new Dictionary<string,int>{{"factor",factor},{"product",product},{"mode",2}};
        }
        return new Item(f,prompt,"افصل ضرب الأرقام الأساسية عن قوى العشرة ثم تحقق بالقيمة المكانية.",
            AssessmentItemType.Numeric,parameters);
    }
    private static bool ValidG5Multiply(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5Multiply(f)||!p.TryGetValue("mode",out int mode))return false;
        if(f==G5PowerTen)
        {
            if(p.Count!=3||mode!=0||!p.TryGetValue("factor",out int factor)||
               !p.TryGetValue("pow",out int power)||factor is <2 or >500||power is <1 or >4)
                return false;
            expected=checked(factor*PowTen(power));return true;
        }
        if(f==G5TrailingZeros)
        {
            if(p.Count!=5||mode!=1||!p.TryGetValue("a",out int a)||
               !p.TryGetValue("b",out int b)||!p.TryGetValue("za",out int za)||
               !p.TryGetValue("zb",out int zb)||a is <2 or >20||b is <2 or >20||
               za is <1 or >2||zb is <1 or >3)return false;
            expected=checked(a*b*PowTen(za+zb));return true;
        }
        if(p.Count!=3||mode!=2||!p.TryGetValue("factor",out int x)||
           !p.TryGetValue("product",out int product)||x is <2 or >200||
           product<=x||product>2000000||product%x!=0)return false;
        int quotient=product/x,powerCount=0;
        while(quotient>1&&quotient%10==0){quotient/=10;powerCount++;}
        if(quotient!=1||powerCount is <1 or >4)return false;
        expected=powerCount;return true;
    }

    private static Item BuildG5(string f,Random random)
    {
        int baseValue=random.Next(2,13),exponent=random.Next(2,6);
        IReadOnlyDictionary<string,int> parameters;
        string prompt;
        if(f==G5Count)
        {
            int value=Power(baseValue,exponent);
            parameters=new Dictionary<string,int>
            {{"base",baseValue},{"product",value},{"mode",1}};
            prompt=$"اكتب الأس المناسب: {string.Join(" × ",Enumerable.Repeat(baseValue,exponent))} = {value}. كم مرة ظهر الأساس كعامل؟";
        }
        else if(f==G5Cube)
        {
            baseValue=random.Next(2,19);
            parameters=new Dictionary<string,int>{{"base",baseValue},{"mode",2}};
            prompt=$"صندوق مكعب طول حرفه {baseValue} وحدة. احسب {baseValue} أس ٣ لإيجاد عدد مكعبات الوحدة في حجمه.";
        }
        else
        {
            parameters=new Dictionary<string,int>{{"base",baseValue},{"exp",exponent},{"mode",0}};
            prompt=$"احسب قيمة {baseValue} أس {exponent} بوصفها ضربًا متكررًا.";
        }
        return new Item(f,prompt,"الأساس هو العامل المكرر، والأس هو عدد مرات ضربه؛ تحقق بضرب العوامل.",AssessmentItemType.Numeric,parameters);
    }
    private static bool ValidG5(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5(f)||!p.TryGetValue("base",out int baseValue)||
            !p.TryGetValue("mode",out int mode))return false;
        if(f==G5Cube)
        {
            if(p.Count!=2||mode!=2||baseValue is <2 or >18)return false;
            expected=Power(baseValue,3);return true;
        }
        if(f==G5Power)
        {
            if(p.Count!=3||mode!=0||baseValue is <2 or >12||
               !p.TryGetValue("exp",out int exp)||exp is <2 or >5)return false;
            expected=Power(baseValue,exp);return true;
        }
        if(p.Count!=3||mode!=1||baseValue is <2 or >12||
           !p.TryGetValue("product",out int product)||product<4||product>248832)
            return false;
        int count=0,n=product;
        while(n%baseValue==0&&n>1){n/=baseValue;count++;}
        if(n!=1||count is <2 or >5)return false;
        expected=count;return true;
    }

    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(IsG5(f))return ValidG5(f,p,out expected);
        if(IsG5Multiply(f))return ValidG5Multiply(f,p,out expected);
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("factor",out var a)||
           !p.TryGetValue("baseDigit",out var b)||!p.TryGetValue("zeros",out var z)||
           a is <2 or >9||b is <2 or >9||z is <1 or >3)return false;
        expected=f switch {Multiply=>a*b*(int)Math.Pow(10,z),Unknown=>b*(int)Math.Pow(10,z),_=>1};
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out var n))throw new InvalidOperationException("Invalid powers of ten parameters");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out var n)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out var x)&&x==n;
    }
}
