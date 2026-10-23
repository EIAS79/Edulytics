using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C contracts are written, deliberately not executed.</summary>
public sealed class Grade3SourceCompanionGradingTests
{
    [Fact]
    public void OriginalRoadSignDataHasExact2021SourceCountsAndIndependentRenderingKey()
    {
        var signs=Grade3SourceCompanionCatalogue.Local2021RoadSignGraph;
        Assert.Equal(new[] {2,1,3,5},signs.Select(x=>x.SymbolCount));
        Assert.Equal(new[] {4,2,6,10},signs.Select(x=>x.TotalSigns));
        Assert.Equal(2,Grade3SourceCompanionCatalogue.SourceGraphSignsPerSymbol);
        Assert.Equal(22,signs.Sum(x=>x.TotalSigns));
    }

    [Fact]
    public void SourceRoadSignFigureHasDiscreteSquaresAndLessonNineKeepsAllPublicTasks()
    {
        // Written only: no Stage D test execution here.
        DirectoryInfo? dir = new(AppContext.BaseDirectory);
        string? path = null;
        while (dir is not null)
        {
            var p = Path.Combine(dir.FullName, "src", "Edulytics.Web",
                "Views", "Grade3SourceCompanion", "Index.cshtml");
            if (File.Exists(p)) { path = p; break; }
            dir = dir.Parent;
        }
        Assert.NotNull(path);
        var view = File.ReadAllText(path!);
        Assert.Contains("symbol<category.SymbolCount", view);
        Assert.Contains("source-pictogram", view);
        Assert.DoesNotContain("category.SymbolCount * 20", view);
        Assert.Contains("40 + 35, 45 + 35, 45 + 36, and 34 + 58", view);
        Assert.Contains("three groups of two", view);
        Assert.Contains("five groups of two", view);
        Assert.Contains("Four people, each wearing two shoes", view);
        Assert.Contains("Two boxes containing ten markers", view);
        Assert.Contains("Three basketball teams with five players", view);
    }

    [Fact]
    public void LessonTenPreservesDistinctTapeGraphAndTeacherOnlyOriginalCardSort()
    {
        // Authored-only Stage C static contract.
        DirectoryInfo? dir = new(AppContext.BaseDirectory);
        string? file = null;
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, "src", "Edulytics.Web",
                "Views", "Grade3SourceCompanion", "Index.cshtml");
            if (File.Exists(candidate)) { file = candidate; break; }
            dir = dir.Parent;
        }
        Assert.NotNull(file);
        var view = File.ReadAllText(file!);
        Assert.Contains("six pairs of socks", view);
        Assert.Contains("three equal tape-diagram sections labelled 2", view);
        Assert.Contains("four equal groups of two strawberries", view);
        Assert.Contains("four equal groups of five fingers", view);
        Assert.Contains("original card-sort blackline materials", view);
        Assert.Contains("not been independently inspected here", view);
    }

    [Theory]
    [InlineData("3x10",true)]
    [InlineData("3 × 10",true)]
    [InlineData("3*10=30",true)]
    [InlineData("30",false)]
    [InlineData("10x3",false)]
    [InlineData("3x10=31",false)]
    [InlineData("3+10",false)]
    [InlineData("3x11",false)]
    public void LessonElevenRequiresActualGroupSizedExpressionNotOnlyProduct(
        string submission,bool correct)
    {
        // Contracts authored for Stage C. Tests intentionally NOT RUN.
        var q=new Grade3OriginalQuestion {
            Prompt="Three groups of ten pencils: write the expression.",
            AnswerType="multiplication-expression",
            Expected="3x10"
        };
        var feedback=Grade3SourceCompanionCatalogue.Grade(q,submission);
        Assert.Equal(correct,feedback.NumericCorrect);
        Assert.True(feedback.TeacherReview);
    }

    [Fact]
    public void LessonElevenBankTargetsExpressionsAndSourceModels()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U01:L11",out var lesson));
        Assert.Equal("multiplication-expression",lesson!.IndependentPractice!.AnswerType);
        Assert.Equal("3x10",lesson.IndependentPractice.Expected);
        Assert.Equal("multiplication-expression",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.Equal("4x6",lesson.OriginalEdulyticsCoolDown.Expected);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(
            lesson.IndependentPractice,"3 × 10").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(
            lesson.IndependentPractice,"30").NumericCorrect);
    }

    [Fact]
    public void LessonTwelveContainsAllSixPublicSourceNumericContextsAndKeepsModelsHuman()
    {
        // Authoring-only Stage C tests, not run.
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U01:L12",out var lesson));
        Assert.Equal(6,lesson!.SourceSubtasks.Count);
        var expected=new[]{15,8,14,50,20,30};
        for(var i=0;i<expected.Length;i++){
            var q=lesson.SourceSubtasks[i];
            Assert.Equal("integer",q.AnswerType);
            Assert.False(string.IsNullOrWhiteSpace(q.Id));
            var good=Grade3SourceCompanionCatalogue.Grade(q,expected[i].ToString());
            var wrong=Grade3SourceCompanionCatalogue.Grade(q,(expected[i]+1).ToString());
            Assert.True(good.NumericCorrect);
            Assert.False(wrong.NumericCorrect);
            Assert.True(good.TeacherReview);
        }
        Assert.False(lesson.ReadyForStageD);
    }

    [Theory]
    [InlineData("2x10=20",true)]
    [InlineData("20 = 2 × 10",true)]
    [InlineData("2*10 = 20",true)]
    [InlineData("20",false)]
    [InlineData("2x10",false)]
    [InlineData("2x10=21",false)]
    [InlineData("10x2=20",false)]
    [InlineData("2+10=20",false)]
    public void MultiplicationEquationsRequireFactorsEqualityAndCorrectProduct(
        string answer,bool correct)
    {
        var q=new Grade3OriginalQuestion {
            AnswerType="multiplication-equation", Expected="2x10=20"
        };
        var result=Grade3SourceCompanionCatalogue.Grade(q,answer);
        Assert.Equal(correct,result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void LessonThirteenCapturesNineMatchesAndSevenEquationWritingContexts()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U01:L13",out var lesson));
        Assert.Equal(16,lesson!.SourceSubtasks.Count);
        Assert.Equal(2,lesson.SourceSubtasks.Count(q=>q.AnswerType=="teacher"));
        foreach(var q in lesson.SourceSubtasks.Where(q=>q.AnswerType=="multiplication-equation")){
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q,q.Expected).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q,"999").NumericCorrect);
        }
        Assert.Equal("multiplication-equation",lesson.IndependentPractice!.AnswerType);
        Assert.Equal("multiplication-equation",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.False(lesson.ReadyForStageD);
    }

    [Fact]
    public void LessonFourteenDistinguishesUnknownGroupCountFromProductAndBlocksUnreadDiagrams()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U01:L14",out var lesson));
        Assert.Equal(10,lesson!.SourceSubtasks.Count);
        Assert.Equal(3,lesson.SourceSubtasks.Count(q=>q.AnswerType=="teacher"));
        Assert.Contains("□ × 5 = 35",lesson.IndependentPractice!.Prompt);
        Assert.DoesNotContain("5 × □ = 35",lesson.IndependentPractice.Prompt);
        var packs=lesson.SourceSubtasks.Single(q=>q.Id=="G3-U01-L14-A2-02");
        Assert.True(Grade3SourceCompanionCatalogue.Grade(packs,"9").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(packs,"5").NumericCorrect);
        var unknownProduct=lesson.SourceSubtasks.Single(q=>q.Id=="G3-U01-L14-A2-03");
        Assert.True(Grade3SourceCompanionCatalogue.Grade(unknownProduct,"10").NumericCorrect);
        var incompletePicture=lesson.SourceSubtasks.Single(q=>q.Id=="G3-U01-L14-A2-01");
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(incompletePicture,"30").NumericCorrect);
        Assert.False(lesson.ReadyForStageD);
    }

    [Fact]
    public void LessonFifteenKeepsAllTwelveSourceProblemsAndThreeUnknownRoles()
    {
        // Unexecuted Stage C source contracts.
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U01:L15",out var lesson));
        Assert.Equal(12,lesson!.SourceSubtasks.Count);
        var expected=new[]{10,20,30,40,3,12,6,8,16,2,45,10};
        for(var i=0;i<expected.Length;i++){
            var q=lesson.SourceSubtasks[i];
            Assert.Equal("integer",q.AnswerType);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q,expected[i].ToString()).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q,(expected[i]+1).ToString()).NumericCorrect);
        }
        Assert.Equal("8",lesson.SourceSubtasks.Single(q=>q.Id=="G3-U01-L15-A2-01").Expected);
        Assert.Equal("16",lesson.SourceSubtasks.Single(q=>q.Id=="G3-U01-L15-A2-02").Expected);
        Assert.Equal("2",lesson.SourceSubtasks.Single(q=>q.Id=="G3-U01-L15-A2-03").Expected);
        Assert.False(lesson.ReadyForStageD);
    }

    [Fact]
    public void LessonSixteenConstructionCannotPassAsAnUnrelatedInteger()
    {
        // Authored-only, never run here.
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U01:L16",out var lesson));
        Assert.Equal(7,lesson!.SourceSubtasks.Count);
        Assert.All(lesson.SourceSubtasks,q=>Assert.Equal("teacher",q.AnswerType));
        Assert.Equal("teacher",lesson.IndependentPractice!.AnswerType);
        Assert.Equal("teacher",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(lesson.IndependentPractice,"20").NumericCorrect);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(lesson.OriginalEdulyticsCoolDown,"24").NumericCorrect);
        Assert.False(lesson.ReadyForStageD);
    }

    [Fact]
    public void LessonSeventeenPreservesArrayGeometryAndWrittenPeerRevision()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U01:L17",out var lesson));
        Assert.Equal(6,lesson!.SourceSubtasks.Count);
        Assert.All(lesson.SourceSubtasks,q=>Assert.Equal("teacher",q.AnswerType));
        Assert.Equal("teacher",lesson.IndependentPractice!.AnswerType);
        Assert.Equal("teacher",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(lesson.IndependentPractice,"12").NumericCorrect);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(lesson.OriginalEdulyticsCoolDown,"16").NumericCorrect);
        Assert.False(lesson.ReadyForStageD);
    }

    [Fact]
    public void LessonEighteenSeparatesWrittenExpressionsFromTeacherObservedArrayDrawings()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U01:L18",out var lesson));
        Assert.Equal(10,lesson!.SourceSubtasks.Count);
        var expected=new[]{"3x5","4x5","2x6","2x10"};
        var expressionTasks=lesson.SourceSubtasks.Where(q=>q.AnswerType=="multiplication-expression").ToArray();
        Assert.Equal(4,expressionTasks.Length);
        for(var i=0;i<expressionTasks.Length;i++){
            Assert.Equal(expected[i],expressionTasks[i].Expected);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(expressionTasks[i],expected[i]).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(expressionTasks[i],
                (i+1==3?"12":"15")).NumericCorrect);
        }
        Assert.Equal("multiplication-expression",lesson.IndependentPractice!.AnswerType);
        Assert.Equal("teacher",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(lesson.OriginalEdulyticsCoolDown,"28").NumericCorrect);
        Assert.False(lesson.ReadyForStageD);
    }

    [Fact]
    public void LessonNineteenRequiresCompleteEquationsForTreeArrays()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U01:L19",out var lesson));
        Assert.Equal(9,lesson!.SourceSubtasks.Count);
        Assert.Equal("multiplication-equation",lesson.IndependentPractice!.AnswerType);
        var originalTree=lesson.SourceSubtasks.Single(q=>q.Id=="G3-U01-L19-A2-01");
        var backyardTree=lesson.SourceSubtasks.Single(q=>q.Id=="G3-U01-L19-A2-02");
        Assert.True(Grade3SourceCompanionCatalogue.Grade(originalTree,"5 × 9 = 45").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(backyardTree,"8 = 2 × 4").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(originalTree,"45").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(backyardTree,"8").NumericCorrect);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(
            lesson.SourceSubtasks.Single(q=>q.Id=="G3-U01-L19-A1-COCRAFT"),"35").NumericCorrect);
        Assert.False(lesson.ReadyForStageD);
    }

    [Theory]
    [InlineData("2x5=10;5x2=10",true)]
    [InlineData("5 × 2 = 10; 2 × 5 = 10",true)]
    [InlineData("2x5=10,5x2=10",true)]
    [InlineData("10",false)]
    [InlineData("2x5=10",false)]
    [InlineData("2x5=10;2x5=10",false)]
    [InlineData("2x5=10;5x2=11",false)]
    [InlineData("2x5=10;5x3=15",false)]
    public void CommutativeSourceRequiresTwoSwappedEquationsNotOneProduct(
        string response,bool correct)
    {
        var q=new Grade3OriginalQuestion {
            AnswerType="commutative-equation-pair",Expected="2x5=10;5x2=10"
        };
        var result=Grade3SourceCompanionCatalogue.Grade(q,response);
        Assert.Equal(correct,result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void LessonTwentyMapsFourSubtractionsTwoSwapsAndTeacherDiscussion()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U01:L20",out var lesson));
        Assert.Equal(9,lesson!.SourceSubtasks.Count);
        Assert.Equal(4,lesson.SourceSubtasks.Count(q=>q.Id.Contains("-WU-")));
        Assert.Equal(2,lesson.SourceSubtasks.Count(q=>q.AnswerType=="commutative-equation-pair"));
        Assert.Equal("commutative-equation-pair",lesson.IndependentPractice!.AnswerType);
        Assert.Equal("commutative-equation-pair",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(lesson.IndependentPractice,"21").NumericCorrect);
        Assert.False(lesson.ReadyForStageD);
    }

    [Fact]
    public void OptionalGameNightProjectNeverTreatsIllustrativeFortyTwoAsTheRequiredAnswer()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U01:L21",out var lesson));
        Assert.Equal(5,lesson!.SourceSubtasks.Count);
        Assert.All(lesson.SourceSubtasks,q=>Assert.Equal("teacher",q.AnswerType));
        Assert.Equal("teacher",lesson.IndependentPractice!.AnswerType);
        Assert.Equal("teacher",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(lesson.IndependentPractice,"42").NumericCorrect);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(lesson.SourceSubtasks[3],"42").NumericCorrect);
        Assert.False(lesson.ReadyForStageD);
        Assert.Equal(16,2*1+2*1+2*2+2*4);
        Assert.Equal(42,2*2+2*4+2*5+2*10);
        Assert.Equal(6,2*2+2);
        Assert.Equal(10,2*4+2);
    }

    [Fact]
    public void SecondUnitAreaIntroductionRequiresOriginalGeometryAndTeacherReview()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U02:L01",out var lesson));
        Assert.Equal(10,lesson!.SourceSubtasks.Count);
        Assert.Equal(2,lesson.SourceSubtasks.Count(q=>q.AnswerType=="integer"));
        Assert.Equal(8,lesson.SourceSubtasks.Count(q=>q.AnswerType=="teacher"));
        Assert.True(Grade3SourceCompanionCatalogue.Grade(
            lesson.SourceSubtasks[1],"12").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(
            lesson.SourceSubtasks[2],"20").NumericCorrect);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(
            lesson.SourceSubtasks[3],"1").NumericCorrect);
        Assert.Equal("teacher",lesson.IndependentPractice!.AnswerType);
        Assert.False(lesson.ReadyForStageD);
    }

    [Fact]
    public void UnitTwoLessonTwoKeepsSixPublisherFiguresUnscoredWithoutOriginalImages()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U02:L02",out var lesson));
        Assert.Equal(13,lesson!.SourceSubtasks.Count);
        Assert.Equal(6,lesson.SourceSubtasks.Count(q=>q.Id.Contains("-A2-")));
        Assert.All(lesson.SourceSubtasks,q=>Assert.Equal("teacher",q.AnswerType));
        Assert.All(lesson.SourceSubtasks,q=>
            Assert.Null(Grade3SourceCompanionCatalogue.Grade(q,"20").NumericCorrect));
        Assert.Equal("integer",lesson.IndependentPractice!.AnswerType);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(lesson.IndependentPractice,"12").NumericCorrect);
        Assert.False(lesson.ReadyForStageD);
    }

    [Fact]
    public void UnitTwoLessonThreeProtectsFourPartiallyTiledTasksAndRectangleCardSort()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U02:L03",out var lesson));
        Assert.Equal(14,lesson!.SourceSubtasks.Count);
        Assert.Equal(4,lesson.SourceSubtasks.Count(q=>q.Id.Contains("-A1-0")));
        Assert.Equal(4,lesson.SourceSubtasks.Count(q=>q.Id.Contains("-WU-")));
        Assert.All(lesson.SourceSubtasks,q=>Assert.Equal("teacher",q.AnswerType));
        var areaEight = lesson.SourceSubtasks.Single(q=>q.Id=="G3-U02-L03-A2-EIGHT");
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(areaEight,"8").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(
            lesson.SourceBasedCheck!,"8").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(
            lesson.IndependentPractice!,"24").NumericCorrect);
        Assert.False(lesson.ReadyForStageD);
    }

    [Fact]
    public void UnitTwoLessonFourNeverGuessesThirdAreaOrSpoilsHiddenRectangleTask()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U02:L04",out var lesson));
        Assert.Equal(11,lesson!.SourceSubtasks.Count);
        var first=lesson.SourceSubtasks.Single(q=>q.Id=="G3-U02-L04-A2-01");
        var second=lesson.SourceSubtasks.Single(q=>q.Id=="G3-U02-L04-A2-02");
        var unknown=lesson.SourceSubtasks.Single(q=>q.Id=="G3-U02-L04-A2-03");
        var fourth=lesson.SourceSubtasks.Single(q=>q.Id=="G3-U02-L04-A2-04");
        Assert.True(Grade3SourceCompanionCatalogue.Grade(first,"18").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(second,"30").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(fourth,"45").NumericCorrect);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(unknown,"30").NumericCorrect);
        Assert.Equal(2,lesson.SourceSubtasks.Count(q=>q.Id.Contains("-A1-")));
        Assert.False(lesson.ReadyForStageD);
    }

    [Fact]
    public void UnitTwoLessonFiveKeepsSevenCardMatchesTeacherOnlyAndFiveDrawingAreaChecks()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U02:L05",out var lesson));
        Assert.Equal(16,lesson!.SourceSubtasks.Count);
        Assert.Equal(7,lesson.SourceSubtasks.Count(q=>q.Id.Contains("-A1-")));
        Assert.Equal(5,lesson.SourceSubtasks.Count(q=>q.Id.Contains("-A2-")));
        Assert.All(lesson.SourceSubtasks.Where(q=>q.Id.Contains("-A1-")),
            q=>Assert.Null(Grade3SourceCompanionCatalogue.Grade(q,"20").NumericCorrect));
        Assert.Equal(new[]{"12","24","18","28","6"},
            lesson.SourceSubtasks.Where(q=>q.Id.Contains("-A2-")).Select(q=>q.Expected));
        foreach(var task in lesson.SourceSubtasks.Where(q=>q.Id.Contains("-A2-"))){
            Assert.True(Grade3SourceCompanionCatalogue.Grade(task,task.Expected).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(task,"999").NumericCorrect);
        }
        Assert.False(lesson.ReadyForStageD);
    }

    [Theory]
    [InlineData("21 cm²",true)]
    [InlineData("21 cm^2",true)]
    [InlineData("21 square centimeters",true)]
    [InlineData("21 square centimetres",true)]
    [InlineData("21",false)]
    [InlineData("21 cm",false)]
    [InlineData("21 in²",false)]
    [InlineData("24 cm²",false)]
    [InlineData("25 cm²",false)]
    public void FirstPartyAreaAnswerRequiresMatchingSquareUnit(
        string answer,bool good)
    {
        var q=new Grade3OriginalQuestion {
            AnswerType="area-with-unit",Expected="21 cm²"
        };
        var result=Grade3SourceCompanionCatalogue.Grade(q,answer);
        Assert.Equal(good,result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void UnitTwoLessonSixLeavesSourceGridMeasurementsUnderTeacherReview()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U02:L06",out var lesson));
        Assert.Equal(10,lesson!.SourceSubtasks.Count);
        Assert.All(lesson.SourceSubtasks,q=>Assert.Equal("teacher",q.AnswerType));
        Assert.Equal("area-with-unit",lesson.IndependentPractice!.AnswerType);
        Assert.Equal("area-with-unit",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(lesson.IndependentPractice,"21 cm²").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(lesson.IndependentPractice,"21 in²").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(lesson.OriginalEdulyticsCoolDown,"24 in²").NumericCorrect);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(lesson.SourceSubtasks[8],"25").NumericCorrect);
        Assert.False(lesson.ReadyForStageD);
    }

    [Fact]
    public void Only135StageCOriginalCompanionCandidatesRegistered()
    {
        Assert.Equal(135, Grade3SourceCompanionCatalogue.Codes.Count);
        Assert.False(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U01:L08", out _));
        Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U01:L09", out var l));
        Assert.Equal(9,l!.Ordinal);
        Assert.True(l.ReadyForStageD);
    }

    [Theory]
    [InlineData("integer","12","12","13",true)]
    [InlineData("integer","5","5","6",true)]
    [InlineData("comparison",">",">","<",true)]
    [InlineData("comparison","=","=","<",true)]
    [InlineData("rational","2/4","1/2","3/4",true)]
    [InlineData("rational","7/4","14/8","1/4",true)]
    public void EligibleNumbersAndEquivalentFractionsAreCheckedWithoutCertifyingReasoning(
        string kind,string expected,string good,string bad,bool review)
    {
        var q = new Grade3OriginalQuestion{Prompt="Test",AnswerType=kind,Expected=expected};
        var yes=Grade3SourceCompanionCatalogue.Grade(q,good);
        var no=Grade3SourceCompanionCatalogue.Grade(q,bad);
        Assert.True(yes.NumericCorrect);
        Assert.False(no.NumericCorrect);
        Assert.Equal(review,yes.TeacherReview);
        Assert.True(no.TeacherReview);
    }
    [Fact]
    public void TeacherOnlyTasksNeverYieldNumericPasses()
    {
        var q=new Grade3OriginalQuestion{AnswerType="teacher",Prompt="Draw a graph."};
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(q,"10").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q,"10").TeacherReview);
    }
    [Fact]
    public void FractionParserRejectsZeroDenominatorAndOutOfScopeNumbers()
    {
        var q=new Grade3OriginalQuestion{AnswerType="rational",Expected="3/4"};
        Assert.False(Grade3SourceCompanionCatalogue.Grade(q,"1/0").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(q,"100000000/4").NumericCorrect);
    }
    [Theory]
    [InlineData(9,6)]
    [InlineData(10,6)]
    [InlineData(11,12)]
    [InlineData(12,15)]
    [InlineData(13,10)]
    [InlineData(14,9)]
    [InlineData(15,3)]
    [InlineData(16,30)]
    [InlineData(17,15)]
    [InlineData(18,15)]
    [InlineData(19,35)]
    [InlineData(20,10)]
    public void SourceAlignedNumericalCheckDoesNotProveOriginalDiagramParity(int number, int expected)
    {
        var lessonCode=$"PED:US-CCSS-MATH:G3:U01:L{number:00}";
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(lessonCode,out var entry));
        Assert.NotNull(entry!.SourceBasedCheck);
        Assert.True(entry.ReviewedStudentFacingText);
        Assert.False(entry.SourceBasedCheckProvenance!.FigurePixelsInspected);
        var yes=Grade3SourceCompanionCatalogue.Grade(entry.SourceBasedCheck!, expected.ToString());
        var no=Grade3SourceCompanionCatalogue.Grade(entry.SourceBasedCheck!, (expected+1).ToString());
        Assert.True(yes.NumericCorrect);
        Assert.False(no.NumericCorrect);
        Assert.True(yes.TeacherReview);
    }

    [Fact]
    public void EverySourceReviewedCompanionRetainsThreeScopedContractsAndTeacherBoundary()
    {
        Assert.Equal(135, Grade3SourceCompanionCatalogue.Codes.Count);
        foreach (var code in Grade3SourceCompanionCatalogue.Codes)
        {
            Assert.True(Grade3SourceCompanionCatalogue.TryGet(code, out var lesson));
            Assert.NotNull(lesson);
            Assert.NotNull(lesson!.SourceBasedCheck);
            Assert.NotNull(lesson.IndependentPractice);
            Assert.NotNull(lesson.OriginalEdulyticsCoolDown);
            Assert.NotNull(lesson.SourceBasedCheckProvenance);
            Assert.False(lesson.SourceBasedCheckProvenance!.FigurePixelsInspected);
            Assert.True(lesson.ReviewedStudentFacingText);
            Assert.False(lesson.ReadyForStageD);

            foreach (var question in new[] { lesson.SourceBasedCheck!,
                       lesson.IndependentPractice!, lesson.OriginalEdulyticsCoolDown! })
            {
                Assert.False(string.IsNullOrWhiteSpace(question.Prompt));
                Assert.Contains(question.AnswerType, new[]
                    { "teacher", "integer", "rational", "comparison", "clock-time", "multiplication-expression", "multiplication-equation", "commutative-equation-pair", "area-with-unit" });
                if (question.AnswerType == "teacher")
                {
                    Assert.Null(question.Expected);
                    var review = Grade3SourceCompanionCatalogue.Grade(question, "42");
                    Assert.Null(review.NumericCorrect);
                    Assert.True(review.TeacherReview);
                    continue;
                }

                Assert.False(string.IsNullOrWhiteSpace(question.Expected));
                var valid=Grade3SourceCompanionCatalogue.Grade(question,question.Expected);
                Assert.True(valid.NumericCorrect);
                Assert.True(valid.TeacherReview);
                var malformed=Grade3SourceCompanionCatalogue.Grade(question,"not-an-answer");
                Assert.False(malformed.NumericCorrect);
                Assert.True(malformed.TeacherReview);

                string wrong=question.AnswerType switch
                {
                    "integer" => (int.Parse(question.Expected!) + 1).ToString(),
                    "rational" => "1/0",
                    "comparison" => question.Expected == ">" ? "<" : ">",
                    "clock-time" => "13:99 pm",
                    "multiplication-expression" => "1x1",
                    "multiplication-equation" => "1x1=1",
                    "commutative-equation-pair" => "1x2=2;2x1=2",
                    "area-with-unit" => "999 cm²",
                    _ => throw new InvalidOperationException("Unexpected type")
                };
                var rejected=Grade3SourceCompanionCatalogue.Grade(question,wrong);
                Assert.False(rejected.NumericCorrect);
                Assert.True(rejected.TeacherReview);
            }
        }
    }

    [Theory]
    [InlineData("4/8",8,"4/8",true)]
    [InlineData("4/8",8,"1/2",false)]
    [InlineData("6/4",4,"3/2",false)]
    [InlineData("6/4",4,"6/4",true)]
    [InlineData("10/2",2,"5",false)]
    [InlineData("10/2",2,"10/2",true)]
    public void RequiredDenominatorRequiresSpecificOriginalRepresentation(
        string expected,int denominator,string answer,bool correct)
    {
        var question=new Grade3OriginalQuestion {
            Prompt="Write a fraction in the specified unit parts.",
            AnswerType="rational",
            Expected=expected,
            RequiredDenominator=denominator
        };
        var result=Grade3SourceCompanionCatalogue.Grade(question,answer);
        Assert.Equal(correct,result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void EquivalentFractionWithoutSpecifiedDenominatorRemainsAccepted()
    {
        var question=new Grade3OriginalQuestion {
            Prompt="Write any equivalent fraction.",
            AnswerType="rational",Expected="1/2"
        };
        Assert.True(Grade3SourceCompanionCatalogue.Grade(question,"3/6").NumericCorrect);
    }

    [Fact]
    public void NoSourceAlignedQuestionForEarlierDedicatedLessonsIsDuplicatedInGenericBank()
    {
        for (var lessonNumber=1; lessonNumber<=8; lessonNumber++)
        {
            Assert.False(Grade3SourceCompanionCatalogue.TryGet(
                $"PED:US-CCSS-MATH:G3:U01:L{lessonNumber:00}",out _));
        }
    }

    [Fact]
    public void WholeNumberRequestsRejectFractionSyntaxEvenWhenNumericallyEquivalent()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U05:L08",out var l));
        var q=l!.SourceBasedCheck!;
        Assert.Equal("integer",q.AnswerType);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(q,"5/1").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q,"5").NumericCorrect);
    }

    [Fact]
    public void FractionalInchLengthsAcceptEquivalentFormsUnlessNamedPartsRequested()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U06:L03",out var l));
        var q=l!.SourceBasedCheck!;
        Assert.Equal("rational",q.AnswerType);
        Assert.Null(q.RequiredDenominator);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q,"18/4").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q,"9/2").NumericCorrect);
    }

    [Theory]
    [InlineData("7/4", "1 3/4")]
    [InlineData("9/2", "4 1/2")]
    [InlineData("9/2", "4 2/4")]
    [InlineData("22/4", "5 1/2")]
    [InlineData("5/2", "2 1/2")]
    public void EquivalentMixedInchLengthsMustReceiveNumericCredit(string fraction, string mixed)
    {
        var q=new Grade3OriginalQuestion { Prompt="State the measured length in inches.",
            AnswerType="rational", Expected=fraction };
        var result=Grade3SourceCompanionCatalogue.Grade(q,mixed);
        Assert.True(result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Theory]
    [InlineData("1 5/4")]
    [InlineData("1 1/0")]
    [InlineData("1 2/2")]
    [InlineData("4 2/3 2/4")]
    [InlineData("-1 1/2")]
    [InlineData("10001 1/2")]
    public void InvalidMixedAnswersCannotReceiveAutomaticCredit(string mixed)
    {
        var q=new Grade3OriginalQuestion { Prompt="Measure an inch length.",
            AnswerType="rational", Expected="3/2" };
        var result=Grade3SourceCompanionCatalogue.Grade(q,mixed);
        Assert.False(result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void NamedFourthPartsStillRequireFourthDenominatorForMixedAnswers()
    {
        var q=new Grade3OriginalQuestion { Prompt="Write it in fourths.",
            AnswerType="rational", Expected="7/4", RequiredDenominator=4 };
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q,"1 3/4").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(q,"1 1/2").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(q,"1 6/8").NumericCorrect);
    }

    [Theory]
    [InlineData("3:51 PM", "3:51 pm", true)]
    [InlineData("3:51 PM", "03:51 p.m.", true)]
    [InlineData("3:51 PM", "3:51 AM", false)]
    [InlineData("3:51 PM", "15:51", false)]
    [InlineData("3:51 PM", "3:61 p.m.", false)]
    [InlineData("5:18 PM", "5:18 p.m.", true)]
    [InlineData("2:36 PM", "2:36 pm", true)]
    public void FullClockTimeCheckerHonorsHoursMinutesAndMeridiem(
        string expected,string answer,bool correct)
    {
        var question=new Grade3OriginalQuestion{
            Prompt="Read the complete time.",AnswerType="clock-time",Expected=expected};
        var result=Grade3SourceCompanionCatalogue.Grade(question,answer);
        Assert.Equal(correct,result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void SourceTimeTasksCannotPassOnMinuteComponentAlone()
    {
        foreach(var pair in new[] {
            ("PED:US-CCSS-MATH:G3:U06:L09", "2:36 PM"),
            ("PED:US-CCSS-MATH:G3:U06:L10", "3:51 PM")
        }) {
            Assert.True(Grade3SourceCompanionCatalogue.TryGet(pair.Item1,out var lesson));
            Assert.Equal("clock-time",lesson!.SourceBasedCheck!.AnswerType);
            Assert.Equal(pair.Item2,lesson.SourceBasedCheck.Expected);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(
                lesson.SourceBasedCheck,"51").NumericCorrect);
        }
    }

}
