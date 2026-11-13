using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit10DecimalPlaceValueQuestions
{
    internal const string PlaceValueDecimal="uae.g4.u10.l01.place_value_decimal";
    internal const string HundredthsCount="uae.g4.u10.l01.hundredths_count";
    internal const string TenthsDigit="uae.g4.u10.l01.tenths_digit";
    internal const string TenthsDecimal="uae.g4.u10.l02.tenths_decimal";
    internal const string TenthsCount="uae.g4.u10.l02.tenths_count";
    internal const string HundredthsDecimal="uae.g4.u10.l03.hundredths_decimal";
    internal const string HundredthsFraction="uae.g4.u10.l03.hundredths_fraction_numerator";
    internal const string DecimalToHundredths="uae.g4.u10.l04.decimal_to_hundredths";
    internal const string DecimalToTenths="uae.g4.u10.l04.decimal_to_tenths";
    internal const string Grade5HundredthsPercent="uae.g5.u01.l03.hundredths_to_percent";
    internal const string Grade5TenthsPercent="uae.g5.u01.l03.tenths_to_percent";
    internal const string Grade5ThousandthsDecimal="uae.g5.u01.l04.thousandths_decimal";
    internal const string Grade5ThousandthsNumerator="uae.g5.u01.l04.thousandths_numerator";
    internal const string Grade5DigitPlaceValue="uae.g5.u01.l05.decimal_digit_place_value";
    internal const string Grade5PlaceFactor="uae.g5.u01.l05.place_factor_ten";
    internal const string Grade5ExpandedDecimal="uae.g5.u01.l06.expanded_decimal_to_standard";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is PlaceValueDecimal or HundredthsCount or TenthsDigit
        or TenthsDecimal or TenthsCount or HundredthsDecimal or HundredthsFraction
        or DecimalToHundredths or DecimalToTenths or Grade5HundredthsPercent or Grade5TenthsPercent
        or Grade5ThousandthsDecimal or Grade5ThousandthsNumerator
        or Grade5DigitPlaceValue or Grade5PlaceFactor or Grade5ExpandedDecimal;
    private static bool IsDecimal(string family)=>family is PlaceValueDecimal or TenthsDecimal or HundredthsDecimal;
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported decimal place-value family");
        if (family is Grade5ThousandthsDecimal or Grade5ThousandthsNumerator)
            return BuildGrade5Thousandths(family,random);
        if (family is Grade5DigitPlaceValue or Grade5PlaceFactor)
            return BuildGrade5DecimalPositions(family,random);
        if (family==Grade5ExpandedDecimal)
            return BuildGrade5ExpandedDecimal(random);
        int whole=family is PlaceValueDecimal or HundredthsCount or TenthsDigit ? random.Next(0,5):0;
        // Stage C extension for G5: include 0% and 100% without changing G4 output.
        if ((family is Grade5HundredthsPercent or Grade5TenthsPercent) && random.Next(10)==0)
            whole=1;
        int tenths=(family is Grade5HundredthsPercent or Grade5TenthsPercent) && whole==1
            ? 0 : random.Next(0,10);
        int hundredths=family is TenthsDecimal or TenthsCount or DecimalToTenths or Grade5TenthsPercent
            ? 0 : (family==Grade5HundredthsPercent && whole==1 ? 0 : random.Next(0,10));
        var p=new Dictionary<string,int>{{"whole",whole},{"tenths",tenths},{"hundredths",hundredths}};
        string prompt=family switch
        {
            PlaceValueDecimal=>$"اكتب العدد الذي يحتوي على {whole} آحاد و{tenths} أعشار و{hundredths} أجزاء من مئة بصيغة عشرية.",
            HundredthsCount=>$"كم جزءًا من مئة في {whole} آحاد و{tenths} أعشار و{hundredths} أجزاء من مئة؟",
            TenthsDigit=>$"ما رقم منزلة الأعشار في عدد مكوّن من {whole} آحاد و{tenths} أعشار و{hundredths} أجزاء من مئة؟",
            TenthsDecimal=>$"اكتب {tenths} أعشار في صورة عدد عشري.",
            TenthsCount=>$"كم جزءًا من عشرة في {tenths} أعشار؟",
            HundredthsDecimal=>$"اكتب {tenths*10+hundredths} جزءًا من مئة في صورة عدد عشري.",
            HundredthsFraction=>$"ما بسط الكسر الذي مقامه ١٠٠ ويمثل {tenths} أعشار و{hundredths} أجزاء من مئة؟",
            DecimalToHundredths=>$"كم جزءًا من مئة في العدد ٠٫{tenths}{hundredths}؟",
            Grade5HundredthsPercent=>$"لوّنت {whole*100+tenths*10+hundredths} مربعًا من شبكة مؤلفة من ١٠٠ مربع؛ ما النسبة المئوية؟ اكتب العدد فقط.",
            Grade5TenthsPercent=>$"ظلّلت {whole*10+tenths} أجزاء من شريط مقسم إلى ١٠ أجزاء متساوية؛ اكتب النسبة المئوية عددًا فقط.",
            _=>$"اكتب ٠٫{tenths} على صورة كسر مقامه ١٠. ما البسط؟"
        };
        return new Item(family,prompt,"ضع كل رقم في منزلته: آحاد، أعشار، أجزاء مئة؛ كل عشر يساوي عشرة أجزاء من مئة.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out decimal answer)
    {
        answer=0m;
        if(!Supports(family)||p.Count!=3||!p.TryGetValue("whole",out int whole)||
           !p.TryGetValue("tenths",out int t)||!p.TryGetValue("hundredths",out int h)||
           whole is <0 or >6||t is <0 or >9||h is <0 or >9||
           (family is TenthsDecimal or TenthsCount or DecimalToTenths or Grade5TenthsPercent && h!=0)||
           (family is HundredthsDecimal or HundredthsFraction or DecimalToHundredths or DecimalToTenths && whole!=0)||
           ((family is Grade5HundredthsPercent or Grade5TenthsPercent) &&
            (whole>1 || (whole==1 && (t!=0 || h!=0)))))return false;
        answer=family switch
        {
            PlaceValueDecimal=>whole+t/10m+h/100m,
            HundredthsCount=>whole*100+t*10+h,
            TenthsDigit=>t,
            TenthsDecimal=>t/10m,
            TenthsCount=>t,
            HundredthsDecimal=>t/10m+h/100m,
            HundredthsFraction or DecimalToHundredths=>t*10+h,
            Grade5HundredthsPercent=>whole*100+t*10+h,
            Grade5TenthsPercent=>(whole*10+t)*10,
            _=>t
        };
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if (family is Grade5ThousandthsDecimal or Grade5ThousandthsNumerator)
            return SolveGrade5Thousandths(family,p);
        if (family is Grade5DigitPlaceValue or Grade5PlaceFactor)
            return SolveGrade5DecimalPositions(family,p);
        if (family==Grade5ExpandedDecimal)
            return SolveGrade5ExpandedDecimal(p);
        if(!Valid(family,p,out decimal result))throw new InvalidOperationException("Invalid decimal parameters");
        string fmt=family switch {TenthsDecimal=>"0.0",PlaceValueDecimal or HundredthsDecimal=>"0.00",_=>"0"};
        return result.ToString(fmt,CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if (family is Grade5ThousandthsDecimal or Grade5ThousandthsNumerator)
            return VerifyGrade5Thousandths(family,p,answer);
        if (family is Grade5DigitPlaceValue or Grade5PlaceFactor)
            return VerifyGrade5DecimalPositions(family,p,answer);
        if (family==Grade5ExpandedDecimal)
            return VerifyGrade5ExpandedDecimal(p,answer);
        if(!Valid(family,p,out decimal expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '٫'=>'.',
            _=>c
        }).Where(c=>c is not ' ' and not '٬' and not '\u00a0').ToArray());
        if(normalized.Length>20||!normalized.All(c=>c is >= '0' and <= '9' or '.'))return false;
        var style=IsDecimal(family)?NumberStyles.AllowDecimalPoint:NumberStyles.None;
        return decimal.TryParse(normalized,style,CultureInfo.InvariantCulture,out decimal actual)&&actual==expected;
    }
    // Extend this same reviewed decimal-place engine to the thousandths place.
    // Separate parameter shape prevents any Grade 4 family from changing.
    private static Item BuildGrade5Thousandths(string family, Random random)
    {
        int numerator = random.Next(0,1001);
        var parameters = new Dictionary<string,int>
        { ["numerator"]=numerator,["denominator"]=1000 };
        string prompt = family==Grade5ThousandthsDecimal
            ? $"اكتب الكسر {numerator}/١٠٠٠ في صورة عدد عشري حتى أجزاء الألف."
            : $"العدد العشري ٠٫{numerator:000} يمثل كسرًا مقامه ١٠٠٠. ما البسط؟";
        if (numerator==1000 && family==Grade5ThousandthsNumerator)
            prompt="اكتب بسط الكسر الذي مقامه ١٠٠٠ ويساوي ١٫٠٠٠.";
        return new Item(family,prompt,
            "ضع ثلاثة أرقام بعد الفاصلة، ويمكنك استعمال أصفار على اليسار؛ ٩٥/١٠٠٠ = ٠٫٠٩٥.",
            AssessmentItemType.Numeric,parameters);
    }

    private static bool ValidGrade5Thousandths(string family,
        IReadOnlyDictionary<string,int> p,out int numerator)
    {
        numerator=0;
        return family is Grade5ThousandthsDecimal or Grade5ThousandthsNumerator &&
            p.Count==2 && p.TryGetValue("numerator",out numerator) &&
            p.TryGetValue("denominator",out int denominator) &&
            denominator==1000 && numerator is >=0 and <=1000;
    }

    private static string SolveGrade5Thousandths(string family,
        IReadOnlyDictionary<string,int> p)
    {
        if(!ValidGrade5Thousandths(family,p,out int numerator))
            throw new InvalidOperationException("Invalid Grade 5 thousandths parameters.");
        return family==Grade5ThousandthsDecimal
            ? (numerator/1000m).ToString("0.000",CultureInfo.InvariantCulture)
            : numerator.ToString(CultureInfo.InvariantCulture);
    }

    private static bool VerifyGrade5Thousandths(string family,
        IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidGrade5Thousandths(family,p,out int numerator) ||
           string.IsNullOrWhiteSpace(answer)) return false;
        string normalized = new(answer.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '٫'=>'.',
            _=>ch
        }).Where(ch=>ch is not ' ' and not '٬' and not '\u00A0' and not '\u202F').ToArray());
        if(normalized.Length>20 || normalized.Count(ch=>ch=='.')>1 ||
           !normalized.All(ch=>ch is >= '0' and <= '9' or '.')) return false;

        if(family==Grade5ThousandthsNumerator)
            return int.TryParse(normalized,NumberStyles.None,
                CultureInfo.InvariantCulture,out int n) && n==numerator;
        // Verify independently from the fraction's rational numerator and
        // denominator; never reuse SolveGrade5Thousandths.
        return decimal.TryParse(normalized,NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,out decimal actual) &&
            actual*1000m==numerator;
    }
    private static Item BuildGrade5DecimalPositions(string family,Random random)
    {
        if (family==Grade5PlaceFactor)
        {
            int digit=random.Next(1,10),largerPlace=random.Next(1,3);
            int smallerPlace=random.Next(largerPlace+1,4);
            var p=new Dictionary<string,int>
            { ["digit"]=digit,["largerPlace"]=largerPlace,["smallerPlace"]=smallerPlace };
            string larger=largerPlace==1?"الأعشار":"أجزاء المئة";
            string smaller=smallerPlace==2?"أجزاء المئة":"أجزاء الألف";
            return new Item(family,
                $"كم ضعفًا قيمة الرقم {digit} في منزلة {larger} مقارنة بقيمته في منزلة {smaller}؟",
                "كل منزلة على يسار الأخرى قيمتها عشرة أضعافها.",
                AssessmentItemType.Numeric,p);
        }
        int numerator=random.Next(1,1000),place=random.Next(1,4);
        string placeName=place==1?"الأعشار":place==2?"أجزاء المئة":"أجزاء الألف";
        var parameters=new Dictionary<string,int>
        { ["numerator"]=numerator,["denominator"]=1000,["place"]=place };
        return new Item(family,
            $"ما قيمة الرقم في منزلة {placeName} للعدد ٠٫{numerator:000}؟ اكتب عددًا عشريًا.",
            "قيمة الرقم تعتمد على منزلته: أعشار، أجزاء المئة أو أجزاء الألف.",
            AssessmentItemType.Numeric,parameters);
    }

    private static bool ValidGrade5Positions(string family,
        IReadOnlyDictionary<string,int> p,out decimal expected)
    {
        expected=0m;
        if(family==Grade5DigitPlaceValue)
        {
            if(p.Count!=3||!p.TryGetValue("numerator",out int n)||
               n is <0 or >999||!p.TryGetValue("denominator",out int d)||
               d!=1000||!p.TryGetValue("place",out int place)||
               place is <1 or >3) return false;
            int digit=place==1?n/100:place==2?n/10%10:n%10;
            expected=place==1?digit/10m:place==2?digit/100m:digit/1000m;
            return true;
        }
        if(family==Grade5PlaceFactor)
        {
            if(p.Count!=3||!p.TryGetValue("digit",out int digit)||
               digit is <1 or >9||!p.TryGetValue("largerPlace",out int high)||
               !p.TryGetValue("smallerPlace",out int low)||
               high is <1 or >2||low is <2 or >3||low<=high) return false;
            expected=low-high==1?10m:100m;
            return true;
        }
        return false;
    }

    private static string SolveGrade5DecimalPositions(string family,
        IReadOnlyDictionary<string,int> p)
    {
        if(!ValidGrade5Positions(family,p,out decimal value))
            throw new InvalidOperationException("Invalid Grade 5 decimal positions.");
        return family==Grade5PlaceFactor
            ? value.ToString("0",CultureInfo.InvariantCulture)
            : value.ToString("0.000",CultureInfo.InvariantCulture);
    }

    private static bool VerifyGrade5DecimalPositions(string family,
        IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidGrade5Positions(family,p,out _) ||
           string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '٫'=>'.',
            _=>ch
        }).Where(ch=>ch is not ' ' and not '٬' and not '\u00A0').ToArray());
        if(normalized.Length>20||normalized.Count(x=>x=='.')>1||
           !normalized.All(x=>x is >= '0' and <= '9' or '.'))return false;
        if(!decimal.TryParse(normalized,
            family==Grade5PlaceFactor?NumberStyles.None:NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,out decimal provided))return false;

        // Independent direct check from position and numerator, without Solve().
        if(family==Grade5PlaceFactor)
        {
            int high=p["largerPlace"],low=p["smallerPlace"],factor=1;
            for(int index=high;index<low;index++)factor*=10;
            return provided==factor;
        }
        int n=p["numerator"],position=p["place"];
        int scale=position==1?100:position==2?10:1;
        int divisor=position==1?10:position==2?100:1000;
        return provided==(n/scale%10)/(decimal)divisor;
    }
    private static Item BuildGrade5ExpandedDecimal(Random random)
    {
        int whole=random.Next(0,500), t=random.Next(0,10),
            h=random.Next(0,10),th=random.Next(0,10);
        var p=new Dictionary<string,int>
        { ["whole"]=whole,["tenths"]=t,["hundredths"]=h,["thousandths"]=th };
        return new Item(Grade5ExpandedDecimal,
            $"اكتب ( {whole} × ١ ) + ( {t} × ١/١٠ ) + ( {h} × ١/١٠٠ ) + ( {th} × ١/١٠٠٠ ) في الصيغة القياسية.",
            "ضع الآحاد والأعشار وأجزاء المئة والألف في مواضعها ثم تحقق من القيمة.",
            AssessmentItemType.Numeric,p);
    }
    private static bool ValidGrade5ExpandedDecimal(IReadOnlyDictionary<string,int> p,
        out int whole,out int t,out int h,out int th)
    {
        whole=t=h=th=0;
        return p.Count==4&&p.TryGetValue("whole",out whole)&&
            p.TryGetValue("tenths",out t)&&p.TryGetValue("hundredths",out h)&&
            p.TryGetValue("thousandths",out th)&&whole is >=0 and <500&&
            t is >=0 and <=9&&h is >=0 and <=9&&th is >=0 and <=9;
    }
    private static string SolveGrade5ExpandedDecimal(IReadOnlyDictionary<string,int> p)
    {
        if(!ValidGrade5ExpandedDecimal(p,out int whole,out int t,out int h,out int th))
            throw new InvalidOperationException("Invalid G5 decimal expansion.");
        decimal value=whole+t/10m+h/100m+th/1000m;
        return value.ToString("0.000",CultureInfo.InvariantCulture);
    }
    private static bool VerifyGrade5ExpandedDecimal(
        IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidGrade5ExpandedDecimal(p,out int whole,out int t,out int h,out int th)||
            string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '٫'=>'.',
            _=>ch
        }).Where(ch=>ch is not ' ' and not '٬' and not '\u00A0').ToArray());
        if(normalized.Length>20||normalized.Count(ch=>ch=='.')>1||
            !normalized.All(ch=>ch is >= '0' and <= '9' or '.'))return false;
        if(!decimal.TryParse(normalized,NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,out decimal value))return false;
        // Independent integer-thousandths recomposition vs Solve's decimal sum.
        int thousandthUnits=whole*1000+t*100+h*10+th;
        return value*1000m==thousandthUnits;
    }
}
