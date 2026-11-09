using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// Seven source-scoped arithmetic families extending the existing exact engine.
// Compiled/tested during Stage D, not Stage A-B-C.
internal static class UaeGrade2Unit3Questions
{
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static int Type(string? f)=>f switch
    {
        "uae.g2.u03.l01.two_digit_decomposition"=>1,
        "uae.g2.u03.l02.regroup_ones_ten"=>2,
        "uae.g2.u03.l03.one_digit_two_digit_regroup"=>3,
        "uae.g2.u03.l04.add_two_digit_numbers"=>4,
        "uae.g2.u03.l05.vertical_add_two_digits"=>5,
        "uae.g2.u03.l06.add_three_four_two_digits"=>6,
        "uae.g2.u03.l07.model_two_digit_story"=>7,
        _=>0
    };
    public static bool Supports(string? f)=>Type(f)>0;
    public static Item Build(string f,Random r)
    {
        var type=Type(f);
        if(type==0)throw new InvalidOperationException("Unknown G2 U3 exact family");
        if(type==6)
        {
            var count=r.Next(3,5);
            var a=r.Next(10,26);
            var b=r.Next(10,26);
            var c=r.Next(10,26);
            var d=count==4?r.Next(10,26):0;
            var sum=a+b+c+d;
            var p=new Dictionary<string,int>{
                ["count"]=count,["a"]=a,["b"]=b,["c"]=c,["d"]=d,["result"]=sum};
            var terms=count==3?$"{a}+{b}+{c}":$"{a}+{b}+{c}+{d}";
            return new Item(f,$"اجمع {count} أعداد مكونة من رقمين عموديًا: {terms} = ؟",
                "أجمع الآحاد كلها أولًا، ثم العشرات مع إعادة التجميع إذا لزم.",
                AssessmentItemType.Numeric,p);
        }
        int x,y;
        if(type is 2 or 3)
        {
            var tens=r.Next(1,9);
            var ones=r.Next(5,10);
            x=tens*10+ones;
            y=r.Next(10-ones,10);
        }
        else if(type==1)
        {
            x=r.Next(12,51);y=r.Next(12,51);
        }
        else
        {
            x=r.Next(11,51);y=r.Next(11,51);
        }
        var pars=new Dictionary<string,int>{{"a",x},{"b",y},{"result",x+y}};
        var question=type switch
        {
            1=>$"فكّك {x} و{y} إلى عشرات وآحاد، ثم أوجد {x}+{y} = ؟",
            2=>$"استعمل قطع العشرات والآحاد. اجمع {x}+{y} وأبدل عشر آحاد بعشرة واحدة. ما المجموع؟",
            3=>$"اجمع العدد من رقم واحد {y} مع العدد ذي الرقمين {x} رأسيا، وأعد تجميع الآحاد. ما الناتج؟",
            4=>$"اجمع {x}+{y}، واكتب خطوات إضافة الآحاد ثم العشرات.",
            5=>$"اكتب الجمع رأسيا مع محاذاة المنازل:\n{x,3}\n+{y,2}\nــــ\nكم المجموع؟",
            _=>$"لدى مريم {x} بطاقات، ثم أضافت {y} بطاقات. ارسم نموذج شريطين للعشرات والآحاد؛ كم بطاقة أصبح لديها؟"
        };
        return new Item(f,question,
            "أضع الآحاد تحت الآحاد والعشرات تحت العشرات. أجمع وأعيد تجميع عشرة آحاد إذا لزم.",
            AssessmentItemType.Numeric,pars);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p)
    {
        var type=Type(family);
        if(type==0||!p.TryGetValue("result",out var total))return false;
        if(type==6)
        {
            return p.Count==6 &&
                p.TryGetValue("count",out var n) && n is 3 or 4 &&
                p.TryGetValue("a",out var a) && a is >=10 and <=25 &&
                p.TryGetValue("b",out var b) && b is >=10 and <=25 &&
                p.TryGetValue("c",out var c) && c is >=10 and <=25 &&
                p.TryGetValue("d",out var d) && (n==4?d is >=10 and <=25:d==0) &&
                total==a+b+c+d;
        }
        if(p.Count!=3||!p.TryGetValue("a",out var x)||!p.TryGetValue("b",out var y))
            return false;
        if(type is 2 or 3)
            return x is >=15 and <=89 && x%10 is >=5 and <=9 &&
                y>=10-x%10 && y<=9 && total==x+y;
        if(type==1)return x is >=12 and <=50 && y is >=12 and <=50 && total==x+y;
        return x is >=11 and <=50 && y is >=11 and <=50 && total==x+y;
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("UAE Grade2 Unit3 invalid question source");
        return p["result"].ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string a)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(a))return false;
        var ascii=new string(a.Trim().Select(c=>
            c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
            c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(ascii,NumberStyles.Integer,
            CultureInfo.InvariantCulture,out var n) && n==p["result"];
    }
}
