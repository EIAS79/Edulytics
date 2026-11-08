using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Stage A–B–C authored targeted regression; WRITTEN_NOT_RUN.
public sealed class UaeGrade1ArabicMakeTenAdditionTests
{
    [Fact]
    public void MakeTen_UsesExactLessonRoute_AndVerifierChecksParts()
    {
        const string code = "PED:UAE-MOE-MATH:L1:COMMON:04:07";
        const string family = "uae.g1.u04.l07.make_ten_add";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(family, Assert.Single(contract!.AllowedQuestionFamilies));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var qs = new ExactSkillContractQuestionEngine().Generate(
            "stage18", code, new[] { family },
            ExactSkillQuestionDifficulty.Standard, 8, 40007, Array.Empty<string>());
        Assert.NotEmpty(qs);
        Assert.All(qs, q =>
        {
            Assert.InRange(q.Parameters["a"], 6, 9);
            Assert.InRange(q.Parameters["b"], 2, 9);
            Assert.Equal(10 - q.Parameters["a"], q.Parameters["toTen"]);
            Assert.Equal(q.Parameters["b"] - q.Parameters["toTen"], q.Parameters["remainder"]);
            Assert.Equal(q.Parameters["a"] + q.Parameters["b"], q.Parameters["total"]);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, "99"));
        });
        var p = new Dictionary<string,int> { ["a"]=8, ["b"]=5, ["toTen"]=2, ["remainder"]=3, ["total"]=13, ["scene"]=0 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(family, p, "١٣"));
        p["remainder"]=4;
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, "١٣"));
    }
}
