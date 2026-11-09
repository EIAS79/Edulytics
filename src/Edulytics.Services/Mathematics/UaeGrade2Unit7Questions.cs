using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

// Source-aligned UAE G2 Unit7 subtraction extension of existing exact engine.
// Compile and runtime test only in Stage D.
internal static class UaeGrade2Unit7Questions
{
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static int T(string? f)=>f switch
    {
        "uae.g2.u07.l01.decompose_hundred_subtract"=>1,
        "uae.g2.u07.l02.subtract_hundreds"=>2,
        "uae.g2.u07.l03.subtract_ten_hundred_mental"=>3,
        "uae.g2.u07.l04.regroup_tens_for_ones"=>4,
        "uae.g2.u07.l05.regroup_hundreds_for_tens"=>5,
        "uae.g2.u07.l06.subtract_three_digit"=>6,
        "uae.g2.u07.l07.vertical_three_digit_sub"=>7,
        "uae.g2.u07.l08.story_equation_sub"=>8,
        "uae.g2.u07.l09.subtract_with_zeros"=>9,
        _=>0
    };
    public static bool Supports(string? f)=>T(f)>0;
    public static Item Build(string f,Random r)
    {
        var t=T(f);if(t==0)throw new InvalidOperationException("Unknown UAE G2 U07 family");
        var p=new Dictionary<string,int>();int a,b;string prompt;
        if(t==1)
        {
            a=r.Next(3,10)*100+r.Next(10,90);b=new[]{97,98,99}[r.Next(3)];
        }
        else if(t==2)
        {
            a=r.Next(2,10)*100;b=r.Next(1,a/100)*100;
        }
        else if(t==3)
        {
            a=r.Next(200,1000);b=r.Next(2)==0?10:100;
        }
        else if(t==4)
        {
            var h=r.Next(3,10);var tens=r.Next(2,10);var ones=r.Next(0,6);
            var ht=r.Next(1,h);var tt=r.Next(0,tens);var ot=r.Next(ones+1,10);
            a=h*100+tens*10+ones;b=ht*100+tt*10+ot;
        }
        else if(t==5)
        {
            var h=r.Next(2,10);var tens=r.Next(0,6);var ones=r.Next(0,10);
            var ht=r.Next(1,h);var tt=r.Next(tens+1,10);var ot=r.Next(0,ones+1);
            a=h*100+tens*10+ones;b=ht*100+tt*10+ot;
        }
        else if(t==9)
        {
            a=r.Next(2,10)*100;b=r.Next(101,a);
        }
        else
        {
            a=r.Next(300,1000);b=r.Next(101,a);
        }
        p["a"]=a;p["b"]=b;p["result"]=a-b;
        prompt=t switch
        {
            1=>$"فكّك المئة لتطرح {b} من {a}. احسب {a}−{b} = ؟",
            2=>$"اطرح مجموعات المئات: {a}−{b}=؟",
            3=>$"اطرح {b} ذهنيًا من {a}. ما الفرق؟",
            4=>$"أعد تجميع عشرة واحدة إلى عشر آحاد ثم احسب {a}−{b}.",
            5=>$"أعد تجميع مئة واحدة إلى عشر عشرات ثم احسب {a}−{b}.",
            6=>$"اطرح العددين ذوي ثلاثة أرقام: {a}−{b}=؟",
            7=>$"أعد كتابة الطرح رأسيًا بمحاذاة المنازل ثم احسب: {a}−{b}.",
            8=>$"في المخزن {a} ورقة، استُخدمت {b}. اكتب جملة الطرح واحسب الباقي.",
            _=>$"في العدد {a} أصفار في العشرات والآحاد. اطرح {b} باستخدام الاستبدال. ما الفرق؟"
        };
        return new Item(f,prompt,"أحاذي القيم المكانية، وأستبدل عند الحاجة، ثم أتحقق بالجمع.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p)
    {
        var t=T(family);
        if(t==0||p.Count!=3||!p.TryGetValue("a",out var a)||
           !p.TryGetValue("b",out var b)||!p.TryGetValue("result",out var result)||
           result!=a-b||result<0)return false;
        if(t==1)return a is >=310 and <=989&&a/100 is >=3 and <=9&&b is 97 or 98 or 99;
        if(t==2)return a%100==0&&b%100==0&&a is >=200 and <=900&&
            b>=100&&b<a;
        if(t==3)return a is >=200 and <=999&&b is 10 or 100;
        if(t==4)return a/100 is >=3 and <=9&&b/100>=1&&b/100<a/100&&
            a%100/10 is >=2 and <=9&&b%100/10<a%100/10&&a%10 is >=0 and <=5&&
            b%10>a%10&&b%10<=9;
        if(t==5)return a/100 is >=2 and <=9&&b/100>=1&&b/100<a/100&&
            a%100/10 is >=0 and <=5&&b%100/10>a%100/10&&
            a%10 is >=0 and <=9&&b%10<=a%10;
        if(t==9)return a%100==0&&a is >=200 and <=900&&b>=101&&b<a;
        return a is >=300 and <=999&&b>=101&&b<a;
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("UAE Grade 2 U07 invalid contract");
        return p["result"].ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(answer))return false;
        var s=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?
            (char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.Integer,CultureInfo.InvariantCulture,out var value)
            &&value==p["result"];
    }
}
