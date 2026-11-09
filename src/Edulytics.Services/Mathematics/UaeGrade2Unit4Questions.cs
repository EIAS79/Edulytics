using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// Grade 2 Unit 4 source-specific addition/subtraction question extension.
// Reuses the existing ExactSkillContractQuestionEngine. Tests deferred to Stage D.
internal static class UaeGrade2Unit4Questions
{
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static int Type(string? family)=>family switch
    {
        "uae.g2.u04.l01.two_digit_fact_families"=>1,
        "uae.g2.u04.l02.subtract_decompose"=>2,
        "uae.g2.u04.l03.regroup_tens_for_subtraction"=>3,
        "uae.g2.u04.l04.subtract_one_digit_from_two"=>4,
        "uae.g2.u04.l05.subtract_two_digits"=>5,
        "uae.g2.u04.l06.vertical_subtract_two_digits"=>6,
        "uae.g2.u04.l07.verify_subtraction_add"=>7,
        "uae.g2.u04.l08.write_story_equation"=>8,
        "uae.g2.u04.l09.two_step_word_problems"=>9,
        _=>0
    };
    public static bool Supports(string? family)=>Type(family)>0;
    public static Item Build(string family,Random r)
    {
        var type=Type(family);
        if(type==0)throw new InvalidOperationException("Unknown UAE G2 U04 exact question family");
        if(type==1)
        {
            var a=r.Next(10,35);var b=r.Next(5,35);var total=a+b;
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"total",total},{"result",b}};
            return new Item(family,
                $"مثلث حقائق: الكل {total}، وأحد الجزأين {a}. ما الجزء المفقود في {total}−{a}=□؟",
                "أحدد الكل والجزأين، وأطرح الجزء المعروف من الكل. أتحقق بجمعهما.",
                AssessmentItemType.Numeric,p);
        }
        if(type==7)
        {
            var a=r.Next(30,96);var b=r.Next(10,a);
            var diff=a-b;
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"diff",diff},{"result",b}};
            return new Item(family,$"تحقق من الطرح: {a}−{b}={diff}. أكمل جملة الجمع: {diff}+□={a}. ما العدد الناقص؟",
                "أجمع الفرق والعدد المطروح لأصل إلى العدد الأصلي.",
                AssessmentItemType.Numeric,p);
        }
        if(type==9)
        {
            var a=r.Next(30,90);var b=r.Next(5,22);var c=r.Next(3,14);
            var mode=r.Next(0,2);var result=mode==0?a-b+c:a+c-b;
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"mode",mode},{"result",result}};
            var prompt=mode==0
                ? $"في المكتبة {a} كتابًا. أُعير منها {b} كتابًا، ثم وصل {c} كتابًا. كم كتابًا الآن؟ اكتب جملتين."
                : $"لدى ليلى {a} بطاقة، ثم جمعت {c} بطاقة أخرى وأعطت {b} بطاقة. كم بقي؟ اكتب عمليتين.";
            return new Item(family,prompt,"أرتب أحداث القصة، أحسب ناتج الخطوة الأولى ثم أكمل الخطوة الثانية.",
                AssessmentItemType.Numeric,p);
        }
        int x,y;
        if(type==2)
        {
            var tens=r.Next(3,10);var ones=r.Next(2,10);
            var subtractTens=r.Next(1,tens);
            var subtractOnes=r.Next(1,ones+1);
            x=10*tens+ones;y=10*subtractTens+subtractOnes;
        }
        else if(type==3)
        {
            var tens=r.Next(1,10);var ones=r.Next(0,8);
            x=tens*10+ones;y=r.Next(ones+1,10);
        }
        else if(type==4)
        {
            x=r.Next(20,100);y=r.Next(1,10);
        }
        else
        {
            x=r.Next(30,100);y=r.Next(10,x);
        }
        var ans=x-y;
        var pars=new Dictionary<string,int>{{"a",x},{"b",y},{"result",ans}};
        var question=type switch
        {
            2=>$"فكّك العدد {y} إلى عشرات وآحاد، ثم احسب {x}−{y}=؟",
            3=>$"بقطع العشرات والآحاد: أبدل عشرة واحدة بعشر آحاد ثم احسب {x}−{y}=؟",
            4=>$"أوجد {x}−{y}، وضع العدد ذي الرقم الواحد في عمود الآحاد.",
            5=>$"اطرح العددين ذا الرقمين مع تنظيم العشرات والآحاد: {x}−{y}=؟",
            6=>$"اكتب المسألة رأسيًا واطرح الآحاد ثم العشرات: {x}−{y}=؟",
            _=>$"في صندوق {x} كرة، أخرجت منه {y} كرة. اكتب جملة عددية مناسبة لإيجاد الباقي ثم احسبه."
        };
        return new Item(family,question,
            "أحاذي الآحاد والعشرات، وأستبدل عشرة واحدة عند الحاجة، وأراجع بالجمع.",
            AssessmentItemType.Numeric,pars);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p)
    {
        var type=Type(family);
        if(type==0||!p.TryGetValue("result",out var ans))return false;
        if(type==1)
            return p.Count==4 && p.TryGetValue("a",out var fa)&&fa is >=10 and <=34 &&
                p.TryGetValue("b",out var fb)&&fb is >=5 and <=34 &&
                p.TryGetValue("total",out var ft)&&ft==fa+fb && ans==fb;
        if(type==7)
            return p.Count==4 && p.TryGetValue("a",out var a7)&&a7 is >=30 and <=95 &&
                p.TryGetValue("b",out var b7)&&b7 is >=10 and <=94 && b7<a7 &&
                p.TryGetValue("diff",out var d7)&&d7==a7-b7&&ans==b7;
        if(type==9)
            return p.Count==5 && p.TryGetValue("a",out var a9)&&a9 is >=30 and <=89 &&
                p.TryGetValue("b",out var b9)&&b9 is >=5 and <=21 &&
                p.TryGetValue("c",out var c9)&&c9 is >=3 and <=13 &&
                p.TryGetValue("mode",out var m9)&&m9 is 0 or 1 &&
                ans==a9-b9+c9;
        if(p.Count!=3||!p.TryGetValue("a",out var x)||!p.TryGetValue("b",out var y)
           ||ans!=x-y)return false;
        if(type==2)
            return x is >=32 and <=99 && y is >=11 and <=89 &&
                y/10>=1 && y/10<x/10 && y%10<=x%10 && y%10>=1;
        if(type==3)
            return x is >=10 and <=97 && x%10<=7 &&
                y is >=1 and <=9 && y>x%10;
        if(type==4)return x is >=20 and <=99 && y is >=1 and <=9;
        return x is >=30 and <=99 && y is >=10 and <=98 && y<x;
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid UAE G2 U04 exact contract");
        return p["result"].ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer))return false;
        var ascii=new string(answer.Trim().Select(c=>
            c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
            c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(ascii,NumberStyles.Integer,
            CultureInfo.InvariantCulture,out var value)&&value==p["result"];
    }
}
