using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Web.GameRouting;

public enum LessonPracticePresentationKind
{
    SpecializedGame = 1,
    ServerVerifiedExact = 2
}

public sealed record LessonPracticePresentationRoute(
    LessonPracticePresentationKind Kind,
    string RendererKey,
    GameLessonRoute? SpecializedGameRoute,
    LessonPracticeContract PracticeContract);

/// <summary>
/// One presentation authority for lesson-launched Practice.
/// Practice capability comes from LessonPracticeCapabilityResolver; the
/// specialized game route is optional. If no specialized game exists, the
/// lesson still uses the dedicated server-verified Lesson Practice shell
/// rather than the Personal Practice surface.
/// </summary>
public static class LessonPracticePresentationResolver
{
    public const string ServerVerifiedRendererKey =
        "lesson-server-verified";

    public static bool TryResolve(
        string lessonCode,
        string unitTitle,
        string lessonTitle,
        string? lessonContext,
        bool requireLessonGrounding,
        bool enableMathematicsV2Pilot,
        out LessonPracticePresentationRoute? presentation)
    {
        presentation = null;

        if (!LessonPracticeCapabilityResolver.TryResolve(
                lessonCode,
                out var contract) ||
            contract is null)
        {
            return false;
        }

        var gameRoute =
            GameLessonRouteResolver.Resolve(
                lessonCode,
                unitTitle,
                lessonTitle,
                lessonContext,
                requireLessonGrounding,
                enableMathematicsV2Pilot);

        if (gameRoute.IsPlayable &&
            gameRoute.RendererKey is not null)
        {
            presentation =
                new LessonPracticePresentationRoute(
                    LessonPracticePresentationKind.SpecializedGame,
                    gameRoute.RendererKey,
                    gameRoute,
                    contract);
            return true;
        }

        presentation =
            new LessonPracticePresentationRoute(
                LessonPracticePresentationKind.ServerVerifiedExact,
                ServerVerifiedRendererKey,
                null,
                contract);
        return true;
    }
}
