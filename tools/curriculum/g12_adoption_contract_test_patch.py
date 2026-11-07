#!/usr/bin/env python3
"""Author future Stage D static contracts for G12 course choice; do NOT RUN."""
from pathlib import Path
root=Path(__file__).resolve().parents[2]
t=root/"tests/Edulytics.Tests/Phase29/Phase29ExplicitCurriculumLevelIdentityTests.cs"
s=t.read_text(encoding="utf8")
a='''        Assert.Equal(9, CurriculumLevelIdentityRegistry.SourceBackedAmericanHighSchoolCourses.Count);'''
b='''        // Nine historical HS courses remain available at 10–13.
        // New source-backed PRECALC is intentionally selectable at Grade 12 only.
        Assert.Equal(10, CurriculumLevelIdentityRegistry.SourceBackedAmericanHighSchoolCourses.Count);'''
assert s.count(a)==1;s=s.replace(a,b)
a='''            Assert.Equal(9, options.Length);
            Assert.Equal(9, options.Select(x => x.Key).Distinct().Count());
            foreach (var source in CurriculumLevelIdentityRegistry.SourceBackedAmericanHighSchoolCourses)'''
b='''            var expectedCount = logical == 13 ? 10 : 9;
            Assert.Equal(expectedCount, options.Length);
            Assert.Equal(expectedCount, options.Select(x => x.Key).Distinct().Count());
            var expectedCourses = CurriculumLevelIdentityRegistry.SourceBackedAmericanHighSchoolCourses
                .Where(course => course.Code != "PRECALC" || logical == 13);
            foreach (var source in expectedCourses)'''
assert s.count(a)==1;s=s.replace(a,b)
a='''        Assert.Equal(9, files.Length);'''
b='''        Assert.Equal(10, files.Length);'''
assert s.count(a)==1;s=s.replace(a,b)
a='''    [Fact]
    public void AmericanHighSchoolCourseSelectionPreservesAllNonAmericanLevelKeys()'''
b='''    [Fact]
    public void Grade12PrecalculusCourseIsExplicitAndDoesNotAppearInOtherGradeSelectors()
    {
        var code = MathematicsCurriculumPackRegistry.CommonCoreCode;
        var level = Assert.Single(CurriculumLevelIdentityRegistry.ForPack(code),
            item => item.CourseCode == "PRECALC");
        Assert.Equal(13, level.LogicalLevel);
        Assert.Equal("Advanced Precalculus", level.Pathway);
        Assert.Equal("Precalculus", level.Label);
        Assert.Equal(
            CurriculumLevelIdentityRegistry.BuildKey(code, 13, "Advanced Precalculus", "PRECALC"),
            level.Key);
        Assert.Same(level, CurriculumLevelIdentityRegistry.Find(level.Key));
        Assert.DoesNotContain(CurriculumLevelIdentityRegistry.ForPack(code),
            item => item.CourseCode == "PRECALC" && item.LogicalLevel != 13);
    }

    [Fact]
    public void AmericanHighSchoolCourseSelectionPreservesAllNonAmericanLevelKeys()'''
assert s.count(a)==1;s=s.replace(a,b)
t.write_text(s,encoding="utf8")

# This Grade12 test must verify an actual embedded course definition; no tests are run in A–B–C.
t=root/"tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
s=t.read_text(encoding="utf8")
a='''    [Fact]
    public void FirstPartySection11ContentHasFifteenDistinctMathematicalExamples()'''
b='''    [Fact]
    public void IndependentPrecalculusBlueprintIsSourceOrderedAndNotGrade11()
    {
        var blueprint = Assert.Single(PedagogicalLessonBlueprintRegistry.LoadEmbeddedDocuments(),
            b => b.BlueprintCode == "US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017");
        Assert.Equal(2, blueprint.SchemaVersion);
        Assert.Equal("PRECALC", blueprint.CourseCode);
        Assert.Equal("Advanced Precalculus", blueprint.Pathway);
        Assert.Equal(13, blueprint.LogicalLevelFrom);
        Assert.Equal(13, blueprint.LogicalLevelTo);
        Assert.Equal(12, blueprint.Units.Count);
        Assert.Equal(73, blueprint.Lessons.Count);
        Assert.Equal(73, blueprint.Lessons.Select(x => x.SourceLessonCode).Distinct().Count());
        Assert.Equal(Code, blueprint.Lessons[0].LessonCode);
        Assert.Equal("Functions and Function Notation", blueprint.Lessons[0].Title);
        Assert.EndsWith(":U12:L04", blueprint.Lessons[^1].LessonCode,
            StringComparison.Ordinal);
        Assert.Equal(73, blueprint.Lessons.Select(x => x.LessonCode).Distinct().Count());
        Assert.Equal(2, blueprint.Lessons[0].FormalTargets.Count);
        Assert.All(blueprint.Lessons.Skip(1), lesson =>
            Assert.Empty(lesson.FormalTargets)); // Fail-closed: other standards not yet source-reviewed.
    }

    [Fact]
    public void FirstPartySection11ContentHasFifteenDistinctMathematicalExamples()'''
assert s.count(a)==1;s=s.replace(a,b)
a='''        Assert.True(lesson.KeyConcepts.Count >= 8);'''
b='''        Assert.True(lesson.KeyConcepts.Count >= 8);
        Assert.Equal(5, lesson.Visuals.Count);
        Assert.Contains(lesson.Visuals, visual => visual.Title.Contains("Vertical-line test",
            StringComparison.Ordinal));
        Assert.Contains(lesson.Visuals, visual => visual.Title.Contains("Horizontal-line test",
            StringComparison.Ordinal));'''
assert s.count(a)==1;s=s.replace(a,b)
t.write_text(s,encoding="utf8")
print("G12_AUTHOR_2_FUTURE_TEST_FILES_NOT_RUN; GRADE12_ONLY_COURSE_IDENTITY_PROVEN_BY_DRAFT_ASSERTIONS")
