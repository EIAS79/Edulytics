using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// UAE G2 Unit08 first-party data and chart families. Existing exact engine reused.
// Unit/contract tests written but not run until Stage D.
internal static class UaeGrade2Unit8Questions
{
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static int T(string? f)=>f switch
    {
        "uae.g2.u08.l01.survey_collect_count"=>1,
        "uae.g2.u08.l02.make_picture_graph"=>2,
        "uae.g2.u08.l03.analyze_picture_graph"=>3,
        "uae.g2.u08.l04.draw_bar_graph"=>4,
        "uae.g2.u08.l05.analyze_bar_graph"=>5,
        "uae.g2.u08.l06.make_data_table"=>6,
        "uae.g2.u08.l07.make_dot_plot"=>7,
        "uae.g2.u08.l08.analyze_dot_plot"=>8,
        _=>0
    };
    public static bool Supports(string? f)=>T(f)>0;
    private static string Bar(int count)=>new string('█',count);
    private static string Dots(int count)=>new string('×',count);
    public static Item Build(string f,Random r)
    {
        var t=T(f);
        if(t==0)throw new InvalidOperationException("Unrecognized Grade 2 Unit 8 data family");
        var p=new Dictionary<string,int>();string prompt;
        if(t is 1 or 6)
        {
            var a=r.Next(2,8);var b=r.Next(2,8);var c=r.Next(1,7);
            p["a"]=a;p["b"]=b;p["c"]=c;p["result"]=a+b+c;
            prompt=t==1
              ? $"أجريت استطلاعًا حول النشاط المفضل (اختيار واحد لكل طالب): القراءة {a}، الرسم {b}، الرياضة {c}. كم طالبًا شارك في الاستطلاع؟"
              : $"كوّن جدولًا بثلاثة صفوف: كتب {a}، أقلام {b}، مساطر {c}. ما مجموع الأشياء في الجدول؟";
        }
        else if(t==2)
        {
            var key=r.Next(1,3);var symbols=r.Next(2,8);var count=symbols*key;
            p["key"]=key;p["symbols"]=symbols;p["count"]=count;p["result"]=symbols;
            prompt=$"ارسم تمثيلًا بالصور لعدد {count} كتب. إذا كان مفتاح الرسم كل صورة كتاب = {key} كتابًا، فكم صورة ترسم؟";
        }
        else if(t==3)
        {
            var key=r.Next(1,3);var a=r.Next(3,9);var b=r.Next(1,a);
            p["key"]=key;p["a"]=a;p["b"]=b;p["result"]=(a-b)*key;
            prompt=$"تمثيل صور: التفاح {new string('●',a)}، البرتقال {new string('●',b)}. مفتاح ● = {key} ثمرة. كم ثمرة يزيد التفاح عن البرتقال؟";
        }
        else if(t==4)
        {
            var a=r.Next(2,9);var b=r.Next(2,9);
            p["a"]=a;p["b"]=b;p["result"]=a;
            prompt=$"جدول استطلاع: رسم {a} طلاب، كرة {b} طلاب. ارسم عمودًا للرسم على محور من0 إلى10، كم مربعًا يكون ارتفاعه؟";
        }
        else if(t==5)
        {
            var a=r.Next(1,9);var b=r.Next(1,9);var c=r.Next(1,9);
            p["a"]=a;p["b"]=b;p["c"]=c;p["result"]=Math.Max(a,Math.Max(b,c))-Math.Min(a,Math.Min(b,c));
            prompt=$"اقرأ رسم الأعمدة (كل █ يمثل طالبًا):\nقراءة {Bar(a)}\nرسم {Bar(b)}\nرياضة {Bar(c)}\nكم فرقًا بين أكثر الأنشطة وأقلها؟";
        }
        else if(t==7)
        {
            var a=r.Next(1,6);var b=r.Next(1,6);var c=r.Next(1,6);
            var target=r.Next(1,4);
            p["a"]=a;p["b"]=b;p["c"]=c;p["target"]=target;
            p["result"]=target==1?a:target==2?b:c;
            prompt=$"ارسم مخطط نقاط مجمعة على خط الأعداد من1 إلى3 للبيانات: القيمة1 تكررت {a} مرات، والقيمة2 تكررت {b} مرات، والقيمة3 تكررت {c} مرات. كم علامة × فوق القيمة {target}؟";
        }
        else
        {
            var a=r.Next(1,7);var b=r.Next(1,7);var c=r.Next(1,7);
            p["a"]=a;p["b"]=b;p["c"]=c;
            p["result"]=Math.Max(a,Math.Max(b,c))-Math.Min(a,Math.Min(b,c));
            prompt=$"مخطط نقاط مجمعة، كل × تمثل مشاهدة واحدة:\n1: {Dots(a)}\n2: {Dots(b)}\n3: {Dots(c)}\nما الفرق بين أكبر تكرار وأصغر تكرار؟";
        }
        return new Item(f,prompt,"أقرأ المفتاح والتكرار، وأنظم البيانات ثم أختار الجمع أو الطرح بما يناسب السؤال.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)
    {
        var t=T(f);
        if(t==0||!p.TryGetValue("result",out var result))return false;
        if(t is 1 or 6)
            return p.Count==4&&p.TryGetValue("a",out var a1)&&a1 is >=2 and <=7&&
                p.TryGetValue("b",out var b1)&&b1 is >=2 and <=7&&
                p.TryGetValue("c",out var c1)&&c1 is >=1 and <=6&&
                result==a1+b1+c1;
        if(t==2)
            return p.Count==4&&p.TryGetValue("key",out var k2)&&k2 is 1 or 2&&
                p.TryGetValue("symbols",out var s2)&&s2 is >=2 and <=7&&
                p.TryGetValue("count",out var c2)&&c2==k2*s2&&result==s2;
        if(t==3)
            return p.Count==4&&p.TryGetValue("key",out var k3)&&k3 is 1 or 2&&
                p.TryGetValue("a",out var a3)&&a3 is >=3 and <=8&&
                p.TryGetValue("b",out var b3)&&b3>=1&&b3<a3&&
                result==(a3-b3)*k3;
        if(t==4)
            return p.Count==3&&p.TryGetValue("a",out var a4)&&a4 is >=2 and <=8&&
                p.TryGetValue("b",out var b4)&&b4 is >=2 and <=8&&result==a4;
        if(t==5||t==8)
        {
            var min=t==5?1:1;var max=t==5?8:6;
            if(p.Count!=4||!p.TryGetValue("a",out var a5)||a5<min||a5>max||
               !p.TryGetValue("b",out var b5)||b5<min||b5>max||
               !p.TryGetValue("c",out var c5)||c5<min||c5>max)return false;
            return result==Math.Max(a5,Math.Max(b5,c5))-Math.Min(a5,Math.Min(b5,c5));
        }
        if(t==7)
        {
            if(p.Count!=5||!p.TryGetValue("a",out var a7)||a7 is <1 or >5||
               !p.TryGetValue("b",out var b7)||b7 is <1 or >5||
               !p.TryGetValue("c",out var c7)||c7 is <1 or >5||
               !p.TryGetValue("target",out var target)||target is <1 or >3)return false;
            return result==(target==1?a7:target==2?b7:c7);
        }
        return false;
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid UAE G2 Unit8 data contract");
        return p["result"].ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(answer))return false;
        var ascii=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?
            (char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(ascii,NumberStyles.Integer,CultureInfo.InvariantCulture,out var n)
            &&n==p["result"];
    }
}
