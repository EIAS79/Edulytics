#!/usr/bin/env python3
"""G12 U01 L01: minimum first-party canonical content + exact existing Practice family reuse.

A-B-C authoring only: no tests, no CI, no publishing, no source-PDF writes.
Run in an isolated PR #626 checkout after the original PDF/source evidence is verified.
"""
from pathlib import Path
import datetime, hashlib, json

ROOT = Path(__file__).resolve().parents[2]
BASE = ROOT / "artifacts/curriculum-delivery/US-CCSS-MATH/G12"
SOURCE_SHA = "f0a83745e1ea5b4200bac371afb64615742528e4d31e1e4d241623a38e454e7d"
LESSON = "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L01"
now = datetime.datetime.now(datetime.timezone.utc).isoformat().replace("+00:00", "Z")
read = lambda p: json.loads(p.read_text(encoding="utf-8"))
write = lambda p, obj: p.write_text(json.dumps(obj, indent=2, ensure_ascii=False)+"\n", encoding="utf-8")
rich_path = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l01.rich-lesson-v2.json"
source = read(rich_path)
assert source["PackCode"] == "US-CCSS-MATH"
lesson, = source["Lessons"]
assert lesson["LessonCode"] == LESSON
assert len(lesson["WorkedExamples"]) == 15
assert len(lesson["KeyConcepts"]) >= 8

paragraphs = lesson["ExplanationParagraphs"]
translation = {
    "CultureCode": "en",
    "Title": lesson["Title"],
    "Explanation": "\n\n".join(paragraphs),
    "KeyConceptsAndRules": "\n\n".join(
        f'{c["Title"]}: {c["Definition"]}\nRule: {c["Rule"]}\nExample: {c["Example"]}'
        for c in lesson["KeyConcepts"]
    ),
    "WorkedExamples": "\n\n".join(
        f'{i}. {e["Title"]}\nTask: {e["Question"]}\nMethod: {e["Method"]}\n'
        + "\n".join(f'Step {k}: {step}' for k, step in enumerate(e["Steps"], 1))
        + f'\nAnswer: {e["Answer"]}\nCheck: {e["Check"]}'
        for i, e in enumerate(lesson["WorkedExamples"], 1)
    ),
    "StepByStepSolutions": "\n\n".join(
        f'{i}. {e["Title"]}: ' + " ".join(e["Steps"]) + f' Final: {e["Answer"]}'
        for i, e in enumerate(lesson["WorkedExamples"], 1)
    ),
    "CommonMistakes": "\n\n".join(
        f'{c["Mistake"]}\nWhy incorrect: {c["WhyWrong"]}\nCorrection: {c["Correction"]}'
        for c in lesson["CommonMistakes"]
    ),
    "QuickSummary": "\n".join(lesson["SummaryPoints"])
}
assert all(translation.values())
body_sha = hashlib.sha256("\n".join(translation[k] for k in (
    "Title", "Explanation", "KeyConceptsAndRules", "WorkedExamples",
    "StepByStepSolutions", "CommonMistakes", "QuickSummary"
)).encode("utf-8")).hexdigest()
candidate = {
    "PackCode": "US-CCSS-MATH",
    "VersionCode": "CCSSM-2010",
    "ContentVersion": "g12-precalculus-u01-l01-abc-20261106",
    "AcademicLanguage": "en",
    "CurriculumTranslationRequired": False,
    "TargetCurriculumPeriod": "2026-2027",
    "SourceCurriculumPeriod": "CCSS Mathematics 2010",
    "SourceVersionLabel": "CCSSM-2010",
    "SourceAuthority": "NGA Center / CCSSO",
    "SourceUrl": "https://corestandards.org/wp-content/uploads/2023/09/ADA-Compliant-Math-Standards.pdf",
    "SourceCheckedAtUtc": "2026-11-06T10:52:00Z",
    "SourceResolution": "CurrentOfficial",
    "FallbackReason": "",
    "ReviewMethod": "Edulytics first-party authoring against local OpenStax 2017 Section 1.1, source parity incomplete.",
    "SourcePolicyVersion": 2,
    "PedagogicalSourceType": "OpenEducationalResource",
    "PedagogicalSourceTitle": "Precalculus",
    "PedagogicalSourcePublisher": "OpenStax / Rice University",
    "PedagogicalSourceEdition": "Local 2017 revision PR-2015-002(03/17)-BW",
    "PedagogicalSourceUrl": "https://openstax.org/details/books/precalculus",
    "PedagogicalSourceCheckedAtUtc": "2026-11-06T10:52:00Z",
    "PedagogicalSourceSelectionReason": "Owner-selected American Grade 12 Precalculus textbook, source reviewed locally.",
    "PedagogicalSourceSelectionEvidence": f"Read-only PDF SHA-256 {SOURCE_SHA}; Section 1.1 PDF pages 18-37.",
    "PedagogicalSourceRightsNote": "CC BY 4.0: provide required attribution separately in credits. All authored lesson figures are first-party; external figures pending individual review.",
    "Status": "Draft",
    "ReviewedBy": "",
    "ReviewEvidence": "A-C INCOMPLETE; not Stage D accepted or published.",
    "Lessons": [{
        "LessonCode": LESSON,
        "TitleProvenance": "PedagogicalSource",
        "TitleSourceReference": f"OpenStax Precalculus 2017 Section 1.1, local PDF SHA256 {SOURCE_SHA}",
        "OutcomeCodes": ["CCSS:HSF-IF.A.1", "CCSS:HSF-IF.A.2"],
        "OfficialReferenceCode": None,
        "IsSupporting": False,
        "SourceUrl": "https://openstax.org/details/books/precalculus",
        "SourceLocator": "Section 1.1, PDF pages 18-37",
        "SourceTitle": "Precalculus",
        "SourcePublisher": "OpenStax / Rice University",
        "SourceEdition": "Local 2017 revision PR-2015-002(03/17)-BW",
        "SourceRights": "CC BY 4.0; first-party Edulytics math presentation and examples, no copied source figure.",
        "SourceSha256": SOURCE_SHA,
        "CanonicalBodySha256": body_sha,
        "SourceVerifiedAtUtc": "2026-11-06T10:52:00Z",
        "RetrievalUrl": "https://openstax.org/details/books/precalculus",
        "RetrievalChannel": "USER_LOCAL_READ_ONLY",
        "RetrievalTimestamp": "2026-11-06T10:52:00Z",
        "AdaptationStatus": "FIRST_PARTY_DRAFT_SOURCE_ALIGNMENT_PENDING",
        "Translations": [translation]
    }]
}
canonical = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g12-precalculus-u01-l01-abc-draft.lesson-content-pack.json"
if canonical.exists():
    raise RuntimeError("Existing canonical G12 lesson content must be reviewed, never overwritten blindly")
write(canonical, candidate)

contracts_path = ROOT / "src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs"
text = contracts_path.read_text(encoding="utf-8")
needle = "        return byLesson.Values\n"
assert text.count(needle) == 1
assert LESSON not in text
insert = '''        // OpenStax Precalculus 2017 Section 1.1 (Grade 12): reuse only four
        // independently implemented function family mechanics. This is a
        // PARTIAL skill-scoped digital route, NOT an assertion that all 92
        // original exercises, graph drawings, answers or teacher rubrics are
        // digitally graded or that the lesson passed Stage D.
        byLesson["PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L01"] =
            new LessonPracticeContract(
                "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L01",
                "supporting.functions.core",
                "PRECALC_FUNCTION_RELATIONS_NOTATION_DOMAIN_TOOLKIT_PARTIAL",
                [
                    "functions.relations.unique_output.classify",
                    "functions.notation.evaluate_context",
                    "functions.domain_range.context_bounds",
                    "functions.absolute_value.translate_evaluate"
                ],
                "G12_SECTION_1_1_EXISTING_FAMILY_REUSE_ONLY_NOT_FULL_SOURCE_PARITY",
                "READY_VERIFIED",
                Version)
            {
                SkillIds = ["supporting.functions.core", "functions.evaluate.exact"]
            };

'''
contracts_path.write_text(text.replace(needle, insert + needle), encoding="utf-8")

test_path = ROOT / "tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
test_text = test_path.read_text(encoding="utf-8")
assert "Grade12ExactPartialReuseIsRestrictedToFourRealFamilies" not in test_text
test_text = test_text.replace("using Edulytics.Core.Curriculum;", "using Edulytics.Core.Curriculum;\nusing Edulytics.Core.Mathematics.Practice;")
marker = "    [Fact]\n    public void IndependentWorkedExamplesHaveCorrectMathGoldenValues()"
assert test_text.count(marker) == 1
addition = '''    [Fact]
    public void Grade12ExactPartialReuseIsRestrictedToFourRealFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Code, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Equal(new[] { "supporting.functions.core", "functions.evaluate.exact" },
            contract.SkillIds);
        Assert.Equal(new[]
        {
            UsCcssAlg1Unit4FunctionQuestions.FunctionRelationFamily,
            UsCcssAlg1Unit4FunctionQuestions.FunctionNotationFamily,
            UsCcssAlg1Unit4FunctionQuestions.FunctionDomainRangeFamily,
            UsCcssAlg1Unit4FunctionQuestions.FunctionAbsoluteTranslateFamily
        }, contract.AllowedQuestionFamilies);
        Assert.DoesNotContain(contract.AllowedQuestionFamilies,
            family => family.Contains("graph", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(contract.AllowedQuestionFamilies,
            family => family.Contains("one_to_one", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Grade12ReuseExactPositiveWrongAndInvalidAnswers()
    {
        var relation = new Dictionary<string, int>
        {
            ["x"] = 2, ["a"] = 5, ["b"] = 8, ["repeats"] = 1, ["variant"] = 0
        };
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(
            UsCcssAlg1Unit4FunctionQuestions.FunctionRelationFamily, relation, "no"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(
            UsCcssAlg1Unit4FunctionQuestions.FunctionRelationFamily, relation, "yes"));
        var notation = new Dictionary<string, int>
        {
            ["initial"] = 20, ["rate"] = 9, ["input"] = 2, ["variant"] = 0
        };
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(
            UsCcssAlg1Unit4FunctionQuestions.FunctionNotationFamily, notation, "38"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(
            UsCcssAlg1Unit4FunctionQuestions.FunctionNotationFamily, notation, "37"));
        notation.Remove("rate");
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(
            UsCcssAlg1Unit4FunctionQuestions.FunctionNotationFamily, notation, "38"));
    }

'''
test_path.write_text(test_text.replace(marker, addition + marker), encoding="utf-8")
cp_path = BASE / "u01-l01-abc-checkpoint.json"
checkpoint = read(cp_path)
checkpoint["CanonicalDraftPath"] = "src/Edulytics.Core/Curriculum/LessonContent/Packs/" + canonical.name
checkpoint["CanonicalDraftStatus"] = "AUTHORED_DRAFT_NOT_PUBLISHED"
checkpoint["PartialPracticeCodeContract"] = "READY_VERIFIED_FOUR_EXISTING_FAMILIES_ONLY_STAGE_D_NOT_TESTED"
checkpoint["ExerciseRoutingRuntimeBound"] = 0
checkpoint["SourceExercisesMatchedInEdulytics"] = 0
checkpoint["ReadyForStageD"] = False
checkpoint["Status"] = "A_C_EXPLICIT_GAP"
checkpoint["Tests"] = "WRITTEN_NOT_RUN"
checkpoint["UpdatedUtc"] = now
write(cp_path, checkpoint)
progress_path = BASE / "full-grade-progress.json"
progress = read(progress_path)
entry = progress["LessonEntries"][0]
entry["CanonicalDraftPath"] = checkpoint["CanonicalDraftPath"]
entry["CanonicalDraftStatus"] = checkpoint["CanonicalDraftStatus"]
entry["SelectedLessonPracticeStatus"] = checkpoint["PartialPracticeCodeContract"]
entry["PracticeVerified"] = False
entry["ReadyForStageD"] = False
entry["UpdatedUtc"] = now
progress["ReadyForStageD"] = 0
progress["FirstIncompleteLessonOrdinal"] = 1
progress["LastClosedLessonOrdinal"] = 0
write(progress_path, progress)
instructions_path = BASE / "grade12-abc-execution-instructions.md"
ins = instructions_path.read_text(encoding="utf-8")
ins = ins.replace(
    "Draft course now authored: PRECALC/Advanced Precalculus selector at logical Level 12 only, V2 source identity inventory of 73 lessons.",
    "Draft course now authored: PRECALC/Advanced Precalculus selector at American Grade 12 logical Level 13 only, V2 source identity inventory of 73 lessons."
)
instructions_path.write_text(ins, encoding="utf-8")
handoff = BASE / "grade12-a-c-handoff.md"
h = handoff.read_text(encoding="utf-8")
h += (
    "\n## L01 runtime authoring continuation — partial reuse; not lesson closure\n\n"
    "- Added Grade 12 first-party **canonical Draft** course content pack sourced from the existing 15 Rich V2 worked examples, objectives, teacher notes and concepts. The Draft is not learner-published.\n"
    "- Added an exact G12-only lesson Practice contract reusing four pre-existing families: unique-output classification, numeric function notation, contextual domain/range and absolute-value evaluation. These support only a subset of Section 1.1, not all 92 original exercises, graphical interpretations, or teacher-led assessment.\n"
    "- Wrote focused Stage D contract/positive/wrong/invalid regression tests, **not run**, and did not initiate CI or E2E. Original grade-level evidence still requires runtime testing, full mapping, graph/source rights, teacher-scored gradebook, and unit resources.\n"
    "- Corrected the stale contradictory Grade 12 placement line in execution instructions: American Grade 12 = logical Level 13.\n"
    "- **U01:L01 remains A_C_EXPLICIT_GAP; 0/73 READY_FOR_STAGE_D.**\n"
)
handoff.write_text(h, encoding="utf-8")
print(json.dumps({"canonical":str(canonical.relative_to(ROOT)),"bodySha":body_sha,
"fourReuse":["relations","notation","domain_range","absolute_value"],
"expectedChanges":7,"ready":0}, indent=2))
