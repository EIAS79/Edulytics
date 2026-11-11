using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit4Lesson03PlaceValueModelsQuestions
{
    internal const string Full="uae.g4.u04.l03.model_two_digit_product";
    internal const string Tens="uae.g4.u04.l03.model_tens_product";
    internal const string Ones="uae.g4.u04.l03.model_ones_product";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Full or Tens or Ones;
    internal static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported G4 model");
        int multiplier=rng.Next(2,6),tens=rng.Next(1,8),ones=rng.Next(1,10);
        string prompt=f switch
        {
            Full=>$"أمثل بالمكعبات {multiplier} مجموعات كل منها {tens*10+ones}. كم مكعبًا بالمجموع؟",
            Tens=>$"في {multiplier} مجموعات وكل مجموعة تضم {tens} عشرات و{ones} آحاد. كم مقدار العشرات فقط بعد الضرب؟",
            _=>$"في {multiplier} مجموعات وكل مجموعة تضم {tens} عشرات و{ones} آحاد. كم ناتج ضرب الآحاد فقط؟"
        };
        return new Item(f,prompt,"أنمذج عشرات وآحاد كل مجموعة، ثم أجمع نواتج الضرب الجزئية.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"multiplier",multiplier},{"tens",tens},{"ones",ones}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int v)
    {
        v=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("multiplier",out var a)||
           !p.TryGetValue("tens",out var t)||!p.TryGetValue("ones",out var u)||
           a is <2 or >5||t is <1 or >7||u is <1 or >9)return false;
        v=f switch {Full=>a*(10*t+u),Tens=>a*t*10,_=>a*u};
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int n))throw new InvalidOperationException("Bad G4 place value params");
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
