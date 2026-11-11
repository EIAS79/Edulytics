using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit7Lesson03SequencesQuestions
{
    internal const string Multiply="uae.g4.u07.l03.multiply_sequence";
    internal const string Divide="uae.g4.u07.l03.divide_sequence";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Multiply or Divide;
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported sequence");
        int first=r.Next(2,10),factor=r.Next(2,6),index=r.Next(3,5);
        var list=new List<int>();
        for(int i=0;i<index;i++)
        {
            int power=(int)Math.Pow(factor,f==Multiply?i:index-1-i);
            list.Add(first*power);
        }
        return new Item(f,$"أكمل المتتالية ذات القاعدة {(f==Multiply?"الضرب":"القسمة")} في {factor}: {string.Join("، ",list)}، □.",
            "أتبع القاعدة ذاتها على كل حد متتالٍ وأتحقق بعملية عكسية.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"first",first},{"factor",factor},{"index",index}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("first",out int a)||
           !p.TryGetValue("factor",out int b)||!p.TryGetValue("index",out int k)||
           a is <2 or >9||b is <2 or >5||k is <3 or >4)return false;
        result=f==Multiply?a*(int)Math.Pow(b,k):a;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out var x))throw new InvalidOperationException("Invalid sequence");
        return x.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out var x)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==x;
    }
}
