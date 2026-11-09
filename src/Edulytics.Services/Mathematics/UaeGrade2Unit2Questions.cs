using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// Source-scoped Grade 2 Unit 2 extension of the EXISTING exact-question engine.
// Runtime grading tests are written and deferred to Stage D, not executed in A-B-C.
internal static class UaeGrade2Unit2Questions
{
    private const string Prefix = "uae.g2.u02.l";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static int Lesson(string? family) =>
        family switch
        {
            "uae.g2.u02.l01.skip_count_hundred_chart" => 1,
            "uae.g2.u02.l02.skip_count_equal_groups" => 2,
            "uae.g2.u02.l03.find_number_pattern" => 3,
            "uae.g2.u02.l04.repeated_add_equal_groups" => 4,
            "uae.g2.u02.l05.repeated_add_arrays" => 5,
            "uae.g2.u02.l06.even_odd_classify" => 6,
            "uae.g2.u02.l07.equal_addends" => 7,
            _ => 0
        };
    public static bool Supports(string? family) => Lesson(family) > 0;
    public static Item Build(string family,Random r)
    {
        var lesson=Lesson(family);
        if(lesson==0)throw new InvalidOperationException("Unsupported UAE G2 Unit2 exact family");
        if(lesson is 1 or 3)
        {
            var step=new[]{2,5,10}[r.Next(3)];
            var start=r.Next(0,21);
            var jumps=r.Next(3,7);
            var result=start+step*jumps;
            var pars=new Dictionary<string,int>{
                ["step"]=step,["start"]=start,["jumps"]=jumps,["result"]=result};
            var terms=Enumerable.Range(0,jumps).Select(k=>(start+k*step).ToString(CultureInfo.InvariantCulture));
            var prompt=lesson==1
                ? $"لوحة المئة: ابدأ من {start} واقفز بمقدار {step} عدد {jumps} قفزات. ما العدد الذي تصل إليه؟"
                : $"اكتشف النمط: {string.Join("، ",terms)}، □. ما العدد المفقود؟";
            return new Item(family,prompt,"أضيف مقدار القفزة الثابت في كل انتقال وأتحقق من الفروق.",
                AssessmentItemType.Numeric,pars);
        }
        if(lesson==2)
        {
            var step=new[]{2,5,10}[r.Next(3)];
            var groups=r.Next(2,7);
            var p=new Dictionary<string,int>{{"step",step},{"groups",groups},{"result",step*groups}};
            return new Item(family,
                $"عندي {groups} مجموعات متساوية، بكل منها {step} عناصر. عُدّ بمقدار {step} في كل مرة؛ كم عنصرًا بالمجموع؟",
                "كل قفزة تضيف مجموعة كاملة مساوية للحجم الأول.",AssessmentItemType.Numeric,p);
        }
        if(lesson is 4 or 5)
        {
            var size=r.Next(2,7);
            var groups=r.Next(2,6);
            var p=new Dictionary<string,int>{{"size",size},{"groups",groups},{"result",size*groups}};
            var sum=string.Join(" + ",Enumerable.Repeat(size.ToString(CultureInfo.InvariantCulture),groups));
            var visual=string.Join("\n",Enumerable.Repeat(new string('●',size),groups));
            var prompt=lesson==4
                ? $"في {groups} مجموعات متساوية بكل منها {size} قطع. أكمل: {sum} = ؟"
                : $"مصفوفة فيها {groups} صفوف و{size} عناصر لكل صف:\n{visual}\nكم عنصرًا؟ {sum} = ؟";
            return new Item(family,prompt,"عدد عناصر المجموعة يتكرر مرة بعدد المجموعات.",
                AssessmentItemType.Numeric,p);
        }
        if(lesson==6)
        {
            var number=r.Next(1,21);
            var p=new Dictionary<string,int>{{"number",number},{"result",number%2==0?2:1}};
            return new Item(family,
                $"صنف العدد {number}. اكتب 2 إذا كان زوجيًا و1 إذا كان فرديًا. كوّن أزواجًا لتعرف الجواب.",
                "العدد الزوجي لا يتبقى منه عنصر عند تكوين الأزواج.",
                AssessmentItemType.Numeric,p);
        }
        var half=r.Next(1,11);
        var pars7=new Dictionary<string,int>{{"part",half},{"total",2*half},{"result",half}};
        return new Item(family,
            $"اكتب العدد {2*half} كمجموع حدّين متساويين: □ + □ = {2*half}. ما قيمة المربع؟",
            "أقسم العدد الكلي إلى جزأين متساويين ثم أتحقق بجمع الجزأين.",
            AssessmentItemType.Numeric,pars7);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p)
    {
        var lesson=Lesson(family);
        if(lesson==0 || !p.TryGetValue("result",out var result))return false;
        if(lesson is 1 or 3)
        {
            return p.Count==4 &&
                p.TryGetValue("step",out var stepA) && stepA is 2 or 5 or 10 &&
                p.TryGetValue("start",out var startA) && startA is >=0 and <=20 &&
                p.TryGetValue("jumps",out var jumpsA) && jumpsA is >=3 and <=6 &&
                result==startA+stepA*jumpsA && result<=100;
        }
        if(lesson==2)
        {
            return p.Count==3 &&
                p.TryGetValue("step",out var stepB) && stepB is 2 or 5 or 10 &&
                p.TryGetValue("groups",out var groupsB) && groupsB is >=2 and <=6 &&
                result==stepB*groupsB;
        }
        if(lesson is 4 or 5)
        {
            return p.Count==3 &&
                p.TryGetValue("size",out var sizeC) && sizeC is >=2 and <=6 &&
                p.TryGetValue("groups",out var groupsC) && groupsC is >=2 and <=5 &&
                result==sizeC*groupsC;
        }
        if(lesson==6)
        {
            return p.Count==2 &&
                p.TryGetValue("number",out var numberD) && numberD is >=1 and <=20 &&
                result==(numberD%2==0?2:1);
        }
        return p.Count==3 &&
            p.TryGetValue("part",out var halfE) && halfE is >=1 and <=10 &&
            p.TryGetValue("total",out var totalE) && totalE==2*halfE &&
            result==halfE;
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("UAE G2 Unit2 question outside exact contract");
        return p["result"].ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer))return false;
        var ascii=new string(answer.Trim().Select(c=>
            c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
            c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(ascii,NumberStyles.Integer,CultureInfo.InvariantCulture,out var actual)
            && actual==p["result"];
    }
}
