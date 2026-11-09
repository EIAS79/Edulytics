using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

// UAE Grade 2 Unit 6 source-bounded arithmetic extension to EXISTING exact engine.
// Runtime tests are prepared but NOT RUN until Stage D.
internal static class UaeGrade2Unit6Questions
{
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static int T(string? f)=>f switch
    {
        "uae.g2.u06.l01.make_hundred_add"=>1,
        "uae.g2.u06.l02.add_hundreds"=>2,
        "uae.g2.u06.l03.add_ten_hundred_mental"=>3,
        "uae.g2.u06.l04.regroup_ones_add"=>4,
        "uae.g2.u06.l05.regroup_tens_add"=>5,
        "uae.g2.u06.l06.add_three_digits"=>6,
        "uae.g2.u06.l07.vertical_three_digit_add"=>7,
        "uae.g2.u06.l08.guess_check_revision"=>8,
        _=>0
    };
    public static bool Supports(string? f)=>T(f)>0;
    public static Item Build(string f,Random r)
    {
        var t=T(f);
        if(t==0)throw new InvalidOperationException("Unknown G2 U06 family");
        var p=new Dictionary<string,int>();string prompt;
        if(t==1)
        {
            var h=r.Next(1,4);var near=h*100-1;
            var b=r.Next(12,90);
            p["a"]=near;p["b"]=b;p["result"]=near+b;
            prompt=$"كوّن مئة أو مئتين أو ثلاثمئة لتجمع {near}+{b}. ما المجموع؟";
        }
        else if(t==2)
        {
            var a=r.Next(1,8);var b=r.Next(1,11-a);
            p["a"]=a;p["b"]=b;p["result"]=(a+b)*100;
            prompt=$"اجمع المئات: {a*100}+{b*100} = ؟";
        }
        else if(t==3)
        {
            var a=r.Next(100,851);
            var d=r.Next(2)==0?10:100;
            p["a"]=a;p["d"]=d;p["result"]=a+d;
            prompt=$"أضف العدد {d} ذهنيًا إلى {a}. ما الناتج؟";
        }
        else if(t is 4 or 5)
        {
            int a,b;
            if(t==4)
            {
                var tensA=r.Next(0,5);var tensB=r.Next(0,5);
                var onesA=r.Next(5,10);var onesB=r.Next(10-onesA,10);
                a=r.Next(2,6)*100+tensA*10+onesA;
                b=r.Next(1,3)*100+tensB*10+onesB;
            }
            else
            {
                var tensA=r.Next(6,10);var tensB=r.Next(10-tensA,10);
                var onesA=r.Next(0,5);var onesB=r.Next(0,5);
                a=r.Next(1,5)*100+tensA*10+onesA;
                b=r.Next(1,3)*100+tensB*10+onesB;
            }
            p["a"]=a;p["b"]=b;p["result"]=a+b;
            prompt=t==4?$"اجمع {a}+{b} مع إعادة تجميع الآحاد إلى عشرات."
                :$"اجمع {a}+{b} مع إعادة تجميع العشرات إلى مئة.";
        }
        else if(t is 6 or 7)
        {
            var a=r.Next(101,500);var b=r.Next(101,500);
            p["a"]=a;p["b"]=b;p["result"]=a+b;
            prompt=t==6?$"اجمع العددين المكونين من ثلاثة أرقام {a}+{b}."
                :$"أعد كتابة {a}+{b} رأسيًا بمحاذاة المئات والعشرات والآحاد، ثم أوجد المجموع.";
        }
        else
        {
            var start=r.Next(100,300);var change=r.Next(40,201);
            p["start"]=start;p["change"]=change;p["result"]=change;
            prompt=$"أستخدم التخمين والتحقق: لدي {start} بطاقة، وأريد أن يصبح العدد {start+change}. كم بطاقة أضيف؟ جرّب تخمينًا ثم راجعه.";
        }
        return new Item(f,prompt,
            "أنظم العشرات والمئات، وأجمع، وأتحقق من الحمل أو من التخمين.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)
    {
        var t=T(f);
        if(t==0||!p.TryGetValue("result",out var v))return false;
        if(t==1)return p.Count==3&&p.TryGetValue("a",out var a1)&&a1 is 99 or 199 or 299&&
            p.TryGetValue("b",out var b1)&&b1 is >=12 and <=89&&v==a1+b1;
        if(t==2)return p.Count==3&&p.TryGetValue("a",out var a2)&&a2 is >=1 and <=7&&
            p.TryGetValue("b",out var b2)&&b2>=1&&a2+b2<=10&&v==(a2+b2)*100;
        if(t==3)return p.Count==3&&p.TryGetValue("a",out var a3)&&a3 is >=100 and <=850&&
            p.TryGetValue("d",out var d3)&&d3 is 10 or 100&&v==a3+d3;
        if(t==8)return p.Count==3&&p.TryGetValue("start",out var s8)&&s8 is >=100 and <=299&&
            p.TryGetValue("change",out var c8)&&c8 is >=40 and <=200&&v==c8;
        if(p.Count!=3||!p.TryGetValue("a",out var a)||!p.TryGetValue("b",out var b)||
           v!=a+b)return false;
        if(t==4)return a/100 is >=2 and <=5&&b/100 is >=1 and <=2&&
            a%100/10 is >=0 and <=4&&b%100/10 is >=0 and <=4&&
            a%10 is >=5 and <=9&&b%10 is >=1 and <=9&&(a%10+b%10)>=10;
        if(t==5)return a/100 is >=1 and <=4&&b/100 is >=1 and <=2&&
            a%100/10 is >=6 and <=9&&b%100/10 is >=1 and <=9&&
            a%10 is >=0 and <=4&&b%10 is >=0 and <=4&&
            (a%100/10+b%100/10)>=10;
        return a is >=101 and <=499&&b is >=101 and <=499;
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid Grade2 U06 source contract");
        return p["result"].ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(answer))return false;
        var s=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?
            (char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.Integer,CultureInfo.InvariantCulture,out var n)&&n==p["result"];
    }
}
