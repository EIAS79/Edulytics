using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>
/// G10 U01L04: square of sums/differences and conjugate product.
/// Reuses polynomial product convolution and canonical bounded full-response verifier.
/// Stage C authoring only. CI/student Practice execution belongs to Stage D.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10SpecialProduct(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int limit=difficulty==ExactSkillQuestionDifficulty.Challenge?7:5;
        int a=random.Next(1,limit+1),b=random.Next(1,limit+1);
        int shape=random.Next(0,3);
        int a0=shape==1?-b:b;
        int b0=shape==0?b:-b;
        string left=shape==1?$"({a}س−{b})":$"({a}س+{b})";
        string right=shape==0?left:$"({a}س−{b})";
        string prompt=shape switch
        {
            0=>$"بسّط مربع مجموع الحدين {left}² واكتب الناتج بالصورة القياسية، موضحًا الحد الأوسط.",
            1=>$"بسّط مربع فرق الحدين {left}² واكتب الناتج بالصورة القياسية، موضحًا الحد الأوسط.",
            _=>$"بسّط حاصل ضرب المترافقين {left}{right} واكتب الناتج بالصورة القياسية، موضحًا لماذا يختفي الحد الأوسط."
        };
        return Problem(family,prompt,
            "طبّق توزيع كل حد أو المتطابقات (أ±ب)²=أ²±2أب+ب²، (أ+ب)(أ−ب)=أ²−ب²، ثم تحقق بالتعويض.",
            AssessmentItemType.ShortAnswer,
            ("a2",0),("a1",a),("a0",a0),
            ("b2",0),("b1",a),("b0",b0),("degree",random.Next(0,3)));
    }
}
internal static class UaeG10SpecialProductsResponse
{
    internal const string Family="uae.g10.general.u01.l04.full_special_product";
    internal static bool Verify(IReadOnlyDictionary<string,int> p,string? answer)=>
        UaeG10PolynomialProductResponse.Verify(
            UaeG10PolynomialProductResponse.WholeFamily,p,answer);
    internal static string Format(IReadOnlyDictionary<string,int> p)=>
        UaeG10PolynomialProductResponse.Format(p);
}
