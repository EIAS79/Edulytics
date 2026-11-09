using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

// Original UAE G2 Unit5 mechanics, extending the EXISTING exact engine.
// Not compiled or exercised prior to Stage D.
internal static class UaeGrade2Unit5Questions
{
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static int T(string? f)=>f switch
    {
        "uae.g2.u05.l01.count_hundreds"=>1,
        "uae.g2.u05.l02.compose_hto"=>2,
        "uae.g2.u05.l03.place_value"=>3,
        "uae.g2.u05.l04.logical_hto"=>4,
        "uae.g2.u05.l05.read_number_words"=>5,
        "uae.g2.u05.l06.skip_5_10_100"=>6,
        "uae.g2.u05.l07.compare_3digit"=>7,
        "uae.g2.u05.l08.ordinal"=>8,
        _=>0
    };
    public static bool Supports(string? f)=>T(f)>0;
    private static readonly (string Name,int Number)[] Names={
        ("مئة",100),("مئتان",200),("ثلاثمئة وستة وعشرون",326),
        ("أربعمئة وسبعة",407),("خمسمئة وتسعة",509),("ستمئة وخمسة",605),
        ("سبعمئة وثلاثة عشر",713),("ثمانمئة واثنان وأربعون",842),
        ("تسعمئة وأربعة",904),("ألف",1000)
    };
    public static Item Build(string f,Random r)
    {
        var t=T(f);if(t==0)throw new InvalidOperationException("Unknown G2 U05 family");
        var p=new Dictionary<string,int>();
        string prompt;
        if(t==1)
        {
            var c=r.Next(1,11);p["c"]=c;p["result"]=c*100;
            prompt=$"لدي {c} مربعات مئة. كم آحادًا تمثل؟";
        }
        else if(t==2||t==4)
        {
            var h=r.Next(1,t==4?8:10);
            var tens=t==4?h+r.Next(1,3):r.Next(0,10);
            var ones=t==4?tens-r.Next(1,3):r.Next(0,10);
            p["h"]=h;p["t"]=tens;p["o"]=ones;p["result"]=h*100+tens*10+ones;
            prompt=t==2?$"اكتب العدد من {h} مئات و{tens} عشرات و{ones} آحاد."
                :$"لغز: المئات {h}، العشرات أكبر من المئات بمقدار {tens-h}، والآحاد أصغر من العشرات بمقدار {tens-ones}. اكتب العدد.";
        }
        else if(t==3)
        {
            var n=r.Next(100,1000);var pos=r.Next(0,3);
            p["number"]=n;p["pos"]=pos;
            p["result"]=pos==0?(n/100)*100:pos==1?(n/10%10)*10:n%10;
            prompt=$"في العدد {n} ما قيمة الرقم في منزلة {(pos==0?"المئات":pos==1?"العشرات":"الآحاد")}؟";
        }
        else if(t==5)
        {
            var ix=r.Next(Names.Length);p["ix"]=ix;p["result"]=Names[ix].Number;
            prompt=$"اكتب بالأرقام: {Names[ix].Name}.";
        }
        else if(t==6)
        {
            var step=new[]{5,10,100}[r.Next(3)];
            var start=r.Next(100,601);var jumps=r.Next(2,5);
            p["step"]=step;p["start"]=start;p["jumps"]=jumps;p["result"]=start+step*jumps;
            var terms=string.Join("، ",Enumerable.Range(0,jumps).Select(i=>(start+i*step).ToString(CultureInfo.InvariantCulture)));
            prompt=$"أكمل النمط: {terms}، □. ما العدد الناقص؟";
        }
        else if(t==7)
        {
            var a=r.Next(100,1000);var b=r.Next(100,1000);
            if(r.Next(4)==0)b=a;
            p["a"]=a;p["b"]=b;p["result"]=a>b?1:a<b?2:0;
            prompt=$"قارن {a} و{b}. اكتب 1 إذا كان الأول أكبر، 2 إذا أصغر و0 إذا متساويان. اختر أيضًا علامة > أو < أو =.";
        }
        else
        {
            var len=r.Next(7,16);var pos=r.Next(1,len+1);
            p["len"]=len;p["pos"]=pos;p["result"]=pos;
            var s=string.Join(" ",Enumerable.Range(1,len).Select(i=>i==pos?"★":"●"));
            prompt=$"ما ترتيب النجمة من اليسار؟ {s} اكتب ترتيبها رقمًا.";
        }
        return new Item(f,prompt,"أتحقق بالنموذج العددي أو بالقيمة المكانية وأراجع الناتج.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)
    {
        var t=T(f);if(t==0||!p.TryGetValue("result",out var v))return false;
        if(t==1)return p.Count==2&&p.TryGetValue("c",out var c)&&c is >=1 and <=10&&v==c*100;
        if(t==2||t==4)
        {
            if(p.Count!=4||!p.TryGetValue("h",out var h)||!p.TryGetValue("t",out var ten)||
               !p.TryGetValue("o",out var o)||h is <1 or >9||ten is <0 or >9||
               o is <0 or >9||v!=h*100+ten*10+o)return false;
            return t!=4||(h<=7&&ten-h is 1 or 2&&ten-o is 1 or 2);
        }
        if(t==3)
        {
            if(p.Count!=3||!p.TryGetValue("number",out var n)||n is <100 or >999||
               !p.TryGetValue("pos",out var ix)||ix is <0 or >2)return false;
            return v==(ix==0?(n/100)*100:ix==1?(n/10%10)*10:n%10);
        }
        if(t==5)return p.Count==2&&p.TryGetValue("ix",out var ix)&&
            ix>=0&&ix<Names.Length&&v==Names[ix].Number;
        if(t==6)return p.Count==4&&p.TryGetValue("start",out var st)&&st is >=100 and <=600&&
            p.TryGetValue("step",out var step)&&step is 5 or 10 or 100&&
            p.TryGetValue("jumps",out var j)&&j is >=2 and <=4&&
            v==st+step*j&&v<=1000;
        if(t==7)return p.Count==3&&p.TryGetValue("a",out var a)&&a is >=100 and <=999&&
            p.TryGetValue("b",out var b)&&b is >=100 and <=999&&v==(a>b?1:a<b?2:0);
        return p.Count==3&&p.TryGetValue("len",out var len)&&len is >=7 and <=15&&
            p.TryGetValue("pos",out var pos)&&pos>=1&&pos<=len&&v==pos;
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid U05 contract");
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
