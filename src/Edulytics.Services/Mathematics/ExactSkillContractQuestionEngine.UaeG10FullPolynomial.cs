using System.Globalization;
using System.Text;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Bounded whole-expression exact response for G10 U01L01, reused through
/// ExactSkillContractQuestionEngine. Not a new engine or an unrestricted CAS.
/// </summary>
internal static class UaeG10PolynomialResponse
{
    private static bool Valid(IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["a2","a1","a0","b2","b1","b0","subtract"];
        return p.Count==keys.Length && keys.All(p.ContainsKey) &&
            keys.Take(6).All(k=>p[k] is >= -9 and <= 9) &&
            p["subtract"] is 0 or 1;
    }
    private static int[] Expected(IReadOnlyDictionary<string,int> p)
    {
        int sign=p["subtract"]==1?-1:1;
        return [p["a0"]+sign*p["b0"],
                p["a1"]+sign*p["b1"],
                p["a2"]+sign*p["b2"]];
    }
    public static string Format(IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(p))throw new InvalidOperationException("Bad polynomial parameters.");
        var coeff=Expected(p);
        var builder=new StringBuilder();
        for(int degree=2;degree>=0;degree--)
        {
            int c=coeff[degree];
            if(c==0)continue;
            builder.Append(builder.Length>0?(c<0?"−":"+"):(c<0?"−":""));
            int abs=Math.Abs(c);
            if(degree==0||abs!=1)builder.Append(abs.ToString(CultureInfo.InvariantCulture));
            if(degree==2)builder.Append("س²");
            if(degree==1)builder.Append('س');
        }
        return builder.Length==0?"0":builder.ToString();
    }
    public static bool Verify(IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(p)||!TryParse(answer,out var actual))return false;
        return actual.SequenceEqual(Expected(p));
    }
    private static bool TryParse(string? answer,out int[] result)
    {
        result=[0,0,0];
        if(string.IsNullOrWhiteSpace(answer)||answer.Length>128)return false;
        var builder=new StringBuilder(answer.Length+1);
        foreach(char orig in answer.Trim())
        {
            char c=orig switch
            {
                >= '٠' and <= '٩'=>(char)('0'+orig-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+orig-'۰'),
                '−' or '–'=>'-',
                'س' or 'X'=>'x',
                _=>orig
            };
            if(char.IsWhiteSpace(c))continue;
            if(c=='²'){builder.Append("^2");continue;}
            if(c is not ('+' or '-' or '^' or 'x') &&
               (c<'0'||c>'9'))return false;
            builder.Append(c);
        }
        string expression=builder.ToString();
        if(expression.Length==0)return false;
        if(expression[0] is not ('+' or '-'))expression="+"+expression;
        int pos=0,count=0,previousDegree=3;
        while(pos<expression.Length)
        {
            char sign=expression[pos];
            if(sign is not ('+' or '-'))return false;
            int start=++pos;
            while(pos<expression.Length && expression[pos] is not ('+' or '-'))pos++;
            if(start==pos||++count>12)return false;
            string term=expression[start..pos];
            int variable=term.IndexOf('x'),degree=0,absolute;
            if(variable==-1)
            {
                if(!int.TryParse(term,NumberStyles.None,
                     CultureInfo.InvariantCulture,out absolute))return false;
            }
            else
            {
                degree=1;
                string tail=term[(variable+1)..];
                if(tail=="^2")degree=2;
                else if(tail is not ("" or "^1"))return false;
                string prefix=term[..variable];
                if(prefix.Length==0)absolute=1;
                else if(!int.TryParse(prefix,NumberStyles.None,
                        CultureInfo.InvariantCulture,out absolute))return false;
            }
            if(absolute>1000)return false;
            // Lesson 1-1 explicitly asks for the standard polynomial form:
            // powers must descend, with no repeated degrees or zero-padding
            // terms. A zero polynomial may be entered as a sole 0.
            if(degree>=previousDegree ||
               (absolute==0 && !(count==1 && pos==expression.Length && degree==0)))
                return false;
            previousDegree=degree;
            int sum=result[degree]+(sign=='-'?-absolute:absolute);
            if(sum is < -1000 or > 1000)return false;
            result[degree]=sum;
        }
        return count>0;
    }
}
