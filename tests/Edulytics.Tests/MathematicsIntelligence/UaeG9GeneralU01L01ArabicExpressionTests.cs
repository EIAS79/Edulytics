using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG9GeneralU01L01ArabicExpressionTests
{
    private const string Lesson =
        "PED:UAE-MOE-MATH:L9:GENERAL:01:01:VARIABLES-AND-EXPRESSIONS";

    private static readonly string[] Families =
    [
        "usccss.algebra.expression_coefficient",
        "usccss.algebra.expression_words"
    ];

    [Fact]
    public void Grade9GeneratesArabicPromptsWithExactCorrectAndIncorrectVerification()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var questions = engine.Generate(
            "UAE-G9-GENERAL", Lesson, Families,
            ExactSkillQuestionDifficulty.Standard, 8, 20261121,
            Array.Empty<string>());

        Assert.Equal(8, questions.Count);
        Assert.Contains(questions, q => q.Family == Families[0]);
        Assert.Contains(questions, q => q.Family == Families[1]);

        foreach (var question in questions)
        {
            Assert.Contains("x", question.Prompt);
            if (question.Family == Families[0])
                Assert.StartsWith("في التعبير", question.Prompt);
            else
                Assert.StartsWith("اكتب تعبيرًا جبريًا", question.Prompt);

            Assert.True(ExactSkillContractQuestionEngine.Verify(
                question.Family, question.Parameters, question.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                question.Family, question.Parameters, "-999"));
            Assert.DoesNotContain("Write an algebraic expression", question.Prompt);
            Assert.DoesNotContain("coefficient of x", question.Prompt);
        }
    }

    [Fact]
    public void NonUaeQuestionsKeepOriginalEnglishPrompts()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var questions = engine.Generate(
            "US-CCSS-MATH", "PED:US-CCSS-MATH:L9:GENERAL:01:01:NON-UAE",
            Families, ExactSkillQuestionDifficulty.Standard, 2, 20261121,
            Array.Empty<string>());

        Assert.Contains(questions, q => q.Prompt.StartsWith("In the expression"));
        Assert.Contains(questions, q => q.Prompt.StartsWith("Write an algebraic expression"));
        Assert.All(questions, q => Assert.True(
            ExactSkillContractQuestionEngine.Verify(q.Family, q.Parameters, q.CorrectAnswer)));
    }
}
