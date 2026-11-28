using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine {
    private static ExactProblem BuildG10SolidViews(string family,Random random,ExactSkillQuestionDifficulty difficulty){
        int a=random.Next(1,5),b=random.Next(0,5),c=random.Next(0,5),d=random.Next(0,5),k=random.Next(0,6);
        string view=family==UaeG10SolidViewsResponse.Front?"الأمامي":family==UaeG10SolidViewsResponse.Side?"الجانبي":"العلوي";
        string prompt=family==UaeG10SolidViewsResponse.Cross?UaeG10SolidViewsResponse.CrossPrompt(k):$"أعمدة من مكعبات وحدة في شبكة 2×2؛ الصف الأمامي من اليسار إلى اليمين ({a}،{b}) والخلفي ({c}،{d}). لا فراغ داخل أي عمود. كم مربع وحدة في المنظر {view} المتعامد؟";
        return Problem(family,prompt,"حدد اتجاه النظر أو اتجاه مستوى القطع قبل حساب المقطع أو الإسقاط.",AssessmentItemType.Numeric,("a",a),("b",b),("c",c),("d",d),("k",k));
    }
}
internal static class UaeG10SolidViewsResponse {
    internal const string Front="uae.g10.general.u15.l01.front_view_count",Side="uae.g10.general.u15.l01.side_view_count",Top="uae.g10.general.u15.l01.top_view_count",Cross="uae.g10.general.u15.l01.cross_section_choice";
    internal static bool Supports(string? f)=>f is Front or Side or Top or Cross;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==5&&p.TryGetValue("a",out int a)&&a>=1&&a<=4&&p.TryGetValue("b",out int b)&&b>=0&&b<=4&&p.TryGetValue("c",out int c)&&c>=0&&c<=4&&p.TryGetValue("d",out int d)&&d>=0&&d<=4&&p.TryGetValue("k",out int k)&&k>=0&&k<6;
    internal static string CrossPrompt(int k){
        string q=k switch{0=>"منشور ثلاثي قائم، قطع موازٍ للقاعدة",1=>"أسطوانة قائمة، قطع عمودي على المحور",2=>"أسطوانة قائمة، قطع عبر المحور",3=>"هرم قاعدته مربعة، قطع موازٍ للقاعدة",4=>"كرة، قطع يمر بالمركز",_=>"هرم رباعي، قطع يمر بالرأس ويقطع القاعدة في قطعة مستقيمة داخلها"};
        return $"{q}. اختر نوع المقطع: 1) مثلث، 2) دائرة، 3) مستطيل، 4) مربع. اختر الشكل الذي تضمنه المعطيات دون افتراض تساوي أضلاع إضافية.";
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){
        if(!Valid(f,p))throw new InvalidOperationException("Invalid solid-view parameters");
        int a=p["a"],b=p["b"],c=p["c"],d=p["d"];
        int n=f switch{Front=>Math.Max(a,c)+Math.Max(b,d),Side=>Math.Max(a,b)+Math.Max(c,d),Top=>new[]{a,b,c,d}.Count(v=>v>0),_=>new[]{1,2,3,4,2,1}[p["k"]]};
        return UaeG10TrigScalar.Rational(n,1);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        if(f==Cross){int expected=p["k"] switch{0 or 5=>1,1 or 4=>2,2=>3,_=>4};return UaeG10TrigScalar.Matches(expected,1,raw);}
        int[,] h={{p["a"],p["b"]},{p["c"],p["d"]}};
        var projected=new HashSet<(int,int)>();
        for(int y=0;y<2;y++)for(int x=0;x<2;x++)for(int z=0;z<h[y,x];z++)projected.Add(f==Front?(x,z):f==Side?(y,z):(x,y));
        return UaeG10TrigScalar.Matches(projected.Count,1,raw);
    }
}
