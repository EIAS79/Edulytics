using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.ExamGeneration;
using Edulytics.Core.MathematicsGeneration;

namespace Edulytics.Services.ExamGeneration;

public sealed class ExamGenerationEngine
{
    public const string EngineVersion = "phase34-v1";

    public GeneratedExamDraft CreateDraft(ExamGenerationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Blueprint);
        ArgumentNullException.ThrowIfNull(request.GeneratedItems);

        var title = request.Title?.Trim() ?? string.Empty;
        if (title.Length is < 1 or > 200)
        {
            throw new InvalidOperationException(
                "Generated exam requires a title between 1 and 200 characters.");
        }

        var kind = request.Blueprint.Purpose switch
        {
            AssessmentPurpose.TeacherAssessment => GeneratedExamKind.FormalTeacherAssessment,
            AssessmentPurpose.StudentPersonalTest => GeneratedExamKind.StudentPersonalTest,
            _ => throw new InvalidOperationException(
                "Exam generation accepts only formal Teacher Assessment or Student Personal Test blueprints.")
        };

        ValidateContext(kind, request.FormalContext);
        ValidateBatch(request.Blueprint, request.GeneratedItems);

        var questions = request.GeneratedItems.Items
            .Select((x, i) => new GeneratedExamQuestion(
                i + 1,
                1m,
                x))
            .ToArray();

        var now = DateTime.UtcNow;
        var status = kind == GeneratedExamKind.FormalTeacherAssessment
            ? GeneratedExamStatus.Draft
            : GeneratedExamStatus.ReadyToStart;

        return new GeneratedExamDraft(
            Guid.NewGuid(),
            kind,
            status,
            title,
            request.Blueprint,
            request.FormalContext,
            questions,
            [Audit("Generated", status)],
            now,
            now);
    }

    public GeneratedExamDraft MarkReviewed(GeneratedExamDraft draft)
    {
        ValidateFormalDraft(draft);
        if (draft.Status != GeneratedExamStatus.Draft)
        {
            throw new InvalidOperationException(
                "Only a formal Draft can be marked reviewed.");
        }

        return Transition(draft, GeneratedExamStatus.Reviewed, "TeacherReviewed");
    }

    public GeneratedExamDraft ReplaceQuestion(
        GeneratedExamDraft draft,
        int order,
        GeneratedMathematicsItem replacement)
    {
        ValidateFormalDraft(draft);
        ArgumentNullException.ThrowIfNull(replacement);

        if (draft.Status is not GeneratedExamStatus.Draft and not GeneratedExamStatus.Reviewed)
        {
            throw new InvalidOperationException(
                "Questions can be replaced only before formal approval.");
        }

        var current = draft.Questions.SingleOrDefault(x => x.Order == order)
            ?? throw new InvalidOperationException("Exam question order was not found.");

        ValidateReplacement(draft, current, replacement);

        var questions = draft.Questions
            .Select(x => x.Order == order
                ? new GeneratedExamQuestion(x.Order, x.MaxScore, replacement)
                : x)
            .OrderBy(x => x.Order)
            .ToArray();

        var now = DateTime.UtcNow;
        return draft with
        {
            Status = GeneratedExamStatus.Draft,
            Questions = questions,
            AuditTrail = [.. draft.AuditTrail, Audit($"QuestionReplaced:{order}", GeneratedExamStatus.Draft)],
            UpdatedAtUtc = now
        };
    }

    public GeneratedExamDraft Approve(GeneratedExamDraft draft)
    {
        ValidateFormalDraft(draft);
        if (draft.Status != GeneratedExamStatus.Reviewed)
        {
            throw new InvalidOperationException(
                "Formal generated assessments require Teacher Review before approval.");
        }

        return Transition(draft, GeneratedExamStatus.Approved, "TeacherApproved");
    }

    public GeneratedExamDraft Publish(GeneratedExamDraft draft)
    {
        ValidateFormalDraft(draft);
        if (draft.Status != GeneratedExamStatus.Approved)
        {
            throw new InvalidOperationException(
                "Formal generated assessments never auto-publish and must be approved first.");
        }

        return Transition(draft, GeneratedExamStatus.Published, "TeacherPublished");
    }

    public FormalAssessmentMaterialization MaterializeFormalAssessment(
        GeneratedExamDraft draft)
    {
        ValidateFormalDraft(draft);
        if (draft.Status is not GeneratedExamStatus.Approved and not GeneratedExamStatus.Published)
        {
            throw new InvalidOperationException(
                "Only an approved or published formal exam can be materialized.");
        }

        var context = draft.FormalContext!;
        var now = DateTime.UtcNow;
        var assessment = new Assessment
        {
            Id = draft.Id,
            SchoolId = draft.Blueprint.SchoolId,
            SubjectId = context.SubjectId,
            ClassGroupId = context.ClassGroupId,
            AcademicYearId = context.AcademicYearId,
            TermId = context.TermId,
            Title = draft.Title,
            AssessmentDate = context.AssessmentDate,
            MaxScore = draft.Questions.Sum(x => x.MaxScore),
            Status = draft.Status == GeneratedExamStatus.Published
                ? AssessmentStatus.Open
                : AssessmentStatus.Draft,
            CreatedByUserId = context.TeacherUserId,
            CreatedAtUtc = draft.CreatedAtUtc,
            UpdatedAtUtc = now
        };

        var questions = new List<AssessmentQuestion>(draft.Questions.Count);
        var outcomeMappings = new List<QuestionLearningOutcome>(draft.Questions.Count);
        var items = new List<AssessmentItem>(draft.Questions.Count);
        var itemOutcomes = new List<AssessmentItemOutcome>(draft.Questions.Count);

        foreach (var question in draft.Questions.OrderBy(x => x.Order))
        {
            var generated = question.GeneratedItem;
            var item = generated.Item;

            // Shared identity is deliberate: it gives a stable reconstructable
            // one-to-one bridge between a formal AssessmentQuestion and the exact
            // generated AssessmentItem without introducing a duplicate Question Bank.
            questions.Add(new AssessmentQuestion
            {
                Id = item.Id,
                SchoolId = assessment.SchoolId,
                AssessmentId = assessment.Id,
                Prompt = item.Prompt,
                MaxScore = question.MaxScore,
                Order = question.Order
            });

            outcomeMappings.Add(new QuestionLearningOutcome
            {
                Id = Guid.NewGuid(),
                SchoolId = assessment.SchoolId,
                AssessmentQuestionId = item.Id,
                LearningOutcomeId = generated.OutcomeLink.LearningOutcomeId
            });

            items.Add(item);
            itemOutcomes.Add(generated.OutcomeLink);
        }

        return new FormalAssessmentMaterialization(
            assessment,
            questions,
            outcomeMappings,
            items,
            itemOutcomes);
    }

    private static GeneratedExamDraft Transition(
        GeneratedExamDraft draft,
        GeneratedExamStatus status,
        string eventName)
    {
        var now = DateTime.UtcNow;
        return draft with
        {
            Status = status,
            AuditTrail = [.. draft.AuditTrail, Audit(eventName, status)],
            UpdatedAtUtc = now
        };
    }

    private static string Audit(string eventName, GeneratedExamStatus status) =>
        $"{EngineVersion}|{eventName}|{status}";

    private static void ValidateContext(
        GeneratedExamKind kind,
        FormalExamContext? context)
    {
        if (kind == GeneratedExamKind.StudentPersonalTest)
        {
            if (context is not null)
            {
                throw new InvalidOperationException(
                    "Student Personal Tests are distinct from formal school assessments and cannot carry formal publication context.");
            }

            return;
        }

        if (context is null ||
            context.TeacherUserId == Guid.Empty ||
            context.ClassGroupId == Guid.Empty ||
            context.SubjectId == Guid.Empty ||
            context.AcademicYearId == Guid.Empty ||
            context.TermId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "Formal generated assessments require explicit Teacher, Class, Subject, Academic Year and Term context.");
        }
    }

    private static void ValidateBatch(
        AssessmentBlueprint blueprint,
        MathematicsGenerationBatch batch)
    {
        if (batch.SchoolId != blueprint.SchoolId ||
            batch.CurriculumAdoptionId != blueprint.CurriculumAdoptionId ||
            !string.Equals(batch.CurriculumLevelKey, blueprint.CurriculumLevelKey, StringComparison.Ordinal) ||
            batch.Items.Count != blueprint.QuestionCount)
        {
            throw new InvalidOperationException(
                "Generated Mathematics batch does not match the assessment blueprint scope or item count.");
        }

        var fingerprints = new HashSet<string>(StringComparer.Ordinal);
        var excluded = blueprint.ExcludedExposureFingerprints.ToHashSet(StringComparer.Ordinal);

        foreach (var generated in batch.Items)
        {
            var item = generated.Item;
            if (item.SchoolId != blueprint.SchoolId ||
                item.CurriculumAdoptionId != blueprint.CurriculumAdoptionId ||
                item.Source != AssessmentItemSource.SystemGenerated ||
                generated.OutcomeLink.SchoolId != blueprint.SchoolId ||
                generated.OutcomeLink.AssessmentItemId != item.Id ||
                string.IsNullOrWhiteSpace(item.Prompt) ||
                string.IsNullOrWhiteSpace(item.CorrectAnswer) ||
                string.IsNullOrWhiteSpace(item.Solution) ||
                string.IsNullOrWhiteSpace(item.GenerationFamily) ||
                string.IsNullOrWhiteSpace(item.GenerationParametersJson) ||
                string.IsNullOrWhiteSpace(item.ValidationMetadataJson) ||
                string.IsNullOrWhiteSpace(item.ExposureFingerprint))
            {
                throw new InvalidOperationException(
                    "Generated exam contains an invalid or non-reconstructable item.");
            }

            if (excluded.Contains(item.ExposureFingerprint) ||
                !fingerprints.Add(item.ExposureFingerprint))
            {
                throw new InvalidOperationException(
                    "Generated exam contains a previously excluded or duplicate exposure.");
            }
        }

        ValidateAllocations(blueprint, batch);
    }

    private static void ValidateAllocations(
        AssessmentBlueprint blueprint,
        MathematicsGenerationBatch batch)
    {
        foreach (var allocation in blueprint.OutcomeAllocations)
        {
            if (batch.Items.Count(x => x.OutcomeLink.LearningOutcomeId == allocation.LearningOutcomeId) != allocation.ItemCount)
                throw new InvalidOperationException("Generated exam Outcome allocation does not match its blueprint.");
        }

        foreach (var allocation in blueprint.DifficultyAllocations)
        {
            if (batch.Items.Count(x => x.Item.Difficulty == allocation.Difficulty) != allocation.ItemCount)
                throw new InvalidOperationException("Generated exam difficulty allocation does not match its blueprint.");
        }

        foreach (var allocation in blueprint.QuestionFamilyAllocations)
        {
            if (batch.Items.Count(x => x.BlueprintFamily == allocation.Family) != allocation.ItemCount)
                throw new InvalidOperationException("Generated exam question-family allocation does not match its blueprint.");
        }

        foreach (var allocation in blueprint.ItemTypeAllocations)
        {
            if (batch.Items.Count(x => x.Item.ItemType == allocation.ItemType) != allocation.ItemCount)
                throw new InvalidOperationException("Generated exam item-type allocation does not match its blueprint.");
        }
    }

    private static void ValidateFormalDraft(GeneratedExamDraft draft)
    {
        ArgumentNullException.ThrowIfNull(draft);
        if (draft.Kind != GeneratedExamKind.FormalTeacherAssessment ||
            draft.FormalContext is null ||
            draft.Blueprint.Purpose != AssessmentPurpose.TeacherAssessment)
        {
            throw new InvalidOperationException(
                "This operation is available only for formal Teacher Assessments.");
        }
    }

    private static void ValidateReplacement(
        GeneratedExamDraft draft,
        GeneratedExamQuestion current,
        GeneratedMathematicsItem replacement)
    {
        var old = current.GeneratedItem;
        var item = replacement.Item;

        if (item.SchoolId != draft.Blueprint.SchoolId ||
            item.CurriculumAdoptionId != draft.Blueprint.CurriculumAdoptionId ||
            replacement.OutcomeLink.SchoolId != draft.Blueprint.SchoolId ||
            replacement.OutcomeLink.AssessmentItemId != item.Id ||
            replacement.OutcomeLink.LearningOutcomeId != old.OutcomeLink.LearningOutcomeId ||
            item.Difficulty != old.Item.Difficulty ||
            item.ItemType != old.Item.ItemType ||
            replacement.BlueprintFamily != old.BlueprintFamily ||
            string.IsNullOrWhiteSpace(item.ExposureFingerprint) ||
            string.IsNullOrWhiteSpace(item.ValidationMetadataJson) ||
            string.IsNullOrWhiteSpace(item.GenerationParametersJson) ||
            string.IsNullOrWhiteSpace(item.Solution))
        {
            throw new InvalidOperationException(
                "Replacement item must preserve Outcome, difficulty, item type, question family and curriculum scope.");
        }

        if (draft.Blueprint.ExcludedExposureFingerprints.Contains(item.ExposureFingerprint, StringComparer.Ordinal) ||
            draft.Questions.Any(x =>
                x.Order != current.Order &&
                string.Equals(x.GeneratedItem.Item.ExposureFingerprint, item.ExposureFingerprint, StringComparison.Ordinal)) ||
            string.Equals(old.Item.ExposureFingerprint, item.ExposureFingerprint, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "Replacement item must be a new, non-exposed question.");
        }
    }
}
