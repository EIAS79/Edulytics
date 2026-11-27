-- Execute only inside a SERIALIZABLE transaction after a verified rollback
-- snapshot. The isolated CI rehearsal must use this exact SQL.
-- Preserve class, student, enrollment, teacher and official reference IDs.
-- Retire only the two unused, exact historical placeholder topic/outcome
-- pairs belonging to the owner-retired Advanced adoptions; never CASCADE
-- or invent a General official alignment.
DO $guard$
BEGIN
    IF current_setting('edulytics.g10_transition_scope', true)
       IS DISTINCT FROM 'UAE-MOE-MATH:L10:ADVANCED_TO_GENERAL:2'
       OR current_setting('transaction_isolation') <> 'serializable' THEN
        RAISE EXCEPTION 'G10 transition requires explicit scope and serializable transaction';
    END IF;
END
$guard$;
SELECT pg_advisory_xact_lock(27500029);

CREATE TEMP TABLE g10_transition_pairs ON COMMIT DROP AS
SELECT a."Id" AS old_id, g."Id" AS new_id, a."SchoolId" AS school_id,
       a."AcademicYearId" AS year_id, a."AcademicProgramId" AS program_id
FROM "SchoolCurriculumAdoptions" a
JOIN "SchoolCurriculumAdoptions" g
 ON g."SchoolId"=a."SchoolId"
 AND g."AcademicYearId" IS NOT DISTINCT FROM a."AcademicYearId"
 AND g."AcademicProgramId"=a."AcademicProgramId"
 AND g."GradeLevelId"=a."GradeLevelId"
 AND g."SubjectId"=a."SubjectId"
 AND g."FrameworkVersionId"=a."FrameworkVersionId"
JOIN "CurriculumFrameworkVersions" v ON v."Id"=a."FrameworkVersionId"
JOIN "CurriculumFrameworks" f ON f."Id"=v."FrameworkId"
WHERE f."Code"='UAE-MOE-MATH' AND v."VersionCode"='MOE-2026-2027-T1'
 AND a."CurriculumLevelKey"='UAE-MOE-MATH:L10:ADVANCED'
 AND a."CurriculumPathway"='Advanced' AND a."CurriculumLogicalLevel"=10
 AND a."IsPrimary" AND a."IsActive"
 AND g."CurriculumLevelKey"='UAE-MOE-MATH:L10:GENERAL'
 AND g."CurriculumPathway"='General' AND g."CurriculumLogicalLevel"=10
 AND g."IsPrimary" AND g."IsActive";

CREATE TEMP TABLE g10_transition_classes ON COMMIT DROP AS
SELECT c."Id" AS id, c."SchoolId" AS school_id, p.new_id
FROM "ClassGroups" c JOIN g10_transition_pairs p
 ON c."SchoolId"=p.school_id AND c."CurriculumAdoptionId"=p.old_id;

CREATE TEMP TABLE g10_transition_topics ON COMMIT DROP AS
SELECT t."Id" AS id, t."SchoolId" AS school_id
FROM "CurriculumTopics" t JOIN g10_transition_pairs p
 ON t."SchoolId"=p.school_id AND t."CurriculumAdoptionId"=p.old_id;

CREATE TEMP TABLE g10_transition_outcomes ON COMMIT DROP AS
SELECT o."Id" AS id, o."SchoolId" AS school_id
FROM "LearningOutcomes" o JOIN g10_transition_pairs p
 ON o."SchoolId"=p.school_id AND o."CurriculumAdoptionId"=p.old_id;

DO $transition$
DECLARE
    changed integer;
    n integer;
BEGIN
    SELECT count(*) INTO n FROM "SchoolCurriculumAdoptions"
    WHERE "CurriculumLevelKey"='UAE-MOE-MATH:L10:ADVANCED';

    -- Repeat only after the exact expected end state; zero by itself is not
    -- evidence that a migration was completed.
    IF n=0 THEN
        IF (SELECT count(*) FROM "SchoolCurriculumAdoptions"
            WHERE "CurriculumLevelKey"='UAE-MOE-MATH:L10:GENERAL'
              AND "CurriculumLogicalLevel"=10 AND "CurriculumPathway"='General'
              AND "IsActive" AND "IsPrimary")<>2
           OR (SELECT count(*) FROM "ClassGroups" c
               JOIN "SchoolCurriculumAdoptions" a
                 ON a."Id"=c."CurriculumAdoptionId" AND a."SchoolId"=c."SchoolId"
               WHERE a."CurriculumLevelKey"='UAE-MOE-MATH:L10:GENERAL'
                 AND c."Code"='L10-GENERAL-B'
                 AND c."Name"='Grade 10 — General — B')<>2 THEN
            RAISE EXCEPTION 'G10 replay requires exact migrated General B classes';
        END IF;
        IF (SELECT count(*) FROM "StudentEnrollments" e
              JOIN "ClassGroups" c ON e."ClassGroupId"=c."Id" AND e."SchoolId"=c."SchoolId"
              JOIN "SchoolCurriculumAdoptions" a ON a."Id"=c."CurriculumAdoptionId" AND a."SchoolId"=c."SchoolId"
              WHERE a."CurriculumLevelKey"='UAE-MOE-MATH:L10:GENERAL' AND c."Code"='L10-GENERAL-B')<>50
           OR (SELECT count(*) FROM "TeacherAssignments" t
              JOIN "ClassGroups" c ON t."ClassGroupId"=c."Id" AND t."SchoolId"=c."SchoolId"
              JOIN "SchoolCurriculumAdoptions" a ON a."Id"=c."CurriculumAdoptionId" AND a."SchoolId"=c."SchoolId"
              WHERE a."CurriculumLevelKey"='UAE-MOE-MATH:L10:GENERAL' AND c."Code"='L10-GENERAL-B')<>2 THEN
            RAISE EXCEPTION 'G10 replay learner/teacher count drift';
        END IF;
        RETURN;
    END IF;

    IF n<>2 OR (SELECT count(*) FROM g10_transition_pairs)<>2
      OR (SELECT count(DISTINCT old_id) FROM g10_transition_pairs)<>2
      OR (SELECT count(DISTINCT new_id) FROM g10_transition_pairs)<>2
      OR (SELECT count(*) FROM "SchoolCurriculumAdoptions"
          WHERE "CurriculumLevelKey"='UAE-MOE-MATH:L10:GENERAL')<>2
      OR (SELECT count(*) FROM g10_transition_classes)<>2
      OR (SELECT count(*) FROM g10_transition_topics)<>2
      OR (SELECT count(*) FROM g10_transition_outcomes)<>2 THEN
        RAISE EXCEPTION 'G10 exact audited 2-to-2 adoption inventory drift';
    END IF;

    IF EXISTS (SELECT 1 FROM "ClassGroups" c JOIN g10_transition_classes s
        ON c."Id"=s.id AND c."SchoolId"=s.school_id
        WHERE c."Name"<>'Grade 10 — Advanced — A' OR c."Code"<>'L10-ADVANCED-A')
      OR EXISTS (SELECT 1 FROM "ClassGroups" c JOIN g10_transition_pairs p
        ON c."SchoolId"=p.school_id AND c."AcademicYearId" IS NOT DISTINCT FROM p.year_id
        AND c."AcademicProgramId"=p.program_id
        WHERE c."NormalizedCode"='L10-GENERAL-B'
        OR (c."CurriculumAdoptionId"=p.new_id AND c."NormalizedName"='GRADE 10 — GENERAL — B'))
      OR (SELECT count(*) FROM "ClassGroups" c JOIN g10_transition_pairs p
        ON c."SchoolId"=p.school_id AND c."CurriculumAdoptionId"=p.new_id
        WHERE c."Name"='Grade 10 — General — A' AND c."Code"='L10-GENERAL-A')<>2 THEN
        RAISE EXCEPTION 'G10 class identity or destination-name collision';
    END IF;

    IF EXISTS (SELECT 1 FROM "CurriculumTopics" t JOIN g10_transition_topics s
        ON t."Id"=s.id AND t."SchoolId"=s.school_id
        WHERE t."Name"<>'UAE MoE standards group MAT.2.05')
      OR (SELECT count(*) FROM "LearningOutcomes" o
        JOIN g10_transition_outcomes s ON o."Id"=s.id AND o."SchoolId"=s.school_id
        JOIN g10_transition_topics t ON o."TopicId"=t.id AND o."SchoolId"=t.school_id
        JOIN "CurriculumPackContentNodes" n ON n."Id"=o."OfficialContentNodeId"
        WHERE o."Code"='MAT.2.05.02'
          AND o."Description"='Official historical UAE MoE code accepted for current lesson alignment only under the Product Owner EXACT historical-outcome rule; historical year is not user-facing.'
          AND n."Code"='UAE:STD:MAT.2.05.02' AND n."Pathway"='Advanced'
          AND n."NodeKind"='Standard' AND n."IsActive")<>2 THEN
        RAISE EXCEPTION 'G10 old outcomes are not the exact unused historical placeholders';
    END IF;

    IF (SELECT count(*) FROM "StudentEnrollments" e JOIN g10_transition_classes c
        ON e."ClassGroupId"=c.id AND e."SchoolId"=c.school_id)<>50
      OR (SELECT count(*) FROM "TeacherAssignments" e JOIN g10_transition_classes c
        ON e."ClassGroupId"=c.id AND e."SchoolId"=c.school_id)<>2
      OR EXISTS(SELECT 1 FROM "Assessments" x JOIN g10_transition_classes c
        ON x."ClassGroupId"=c.id AND x."SchoolId"=c.school_id)
      OR EXISTS(SELECT 1 FROM "ClassAssessmentTrends" x JOIN g10_transition_classes c
        ON x."ClassGroupId"=c.id AND x."SchoolId"=c.school_id)
      OR EXISTS(SELECT 1 FROM "StudentOutcomeMasteries" x JOIN g10_transition_classes c
        ON x."ClassGroupId"=c.id AND x."SchoolId"=c.school_id)
      OR EXISTS(SELECT 1 FROM "ClassOutcomeSummaries" x JOIN g10_transition_classes c
        ON x."ClassGroupId"=c.id AND x."SchoolId"=c.school_id)
      OR EXISTS(SELECT 1 FROM "ClassTopicSummaries" x JOIN g10_transition_classes c
        ON x."ClassGroupId"=c.id AND x."SchoolId"=c.school_id)
      OR EXISTS(SELECT 1 FROM "AssessmentItems" x JOIN g10_transition_pairs p
        ON x."CurriculumAdoptionId"=p.old_id AND x."SchoolId"=p.school_id)
      OR EXISTS(SELECT 1 FROM "PracticeAttempts" x JOIN g10_transition_pairs p
        ON x."CurriculumAdoptionId"=p.old_id AND x."SchoolId"=p.school_id)
      OR EXISTS(SELECT 1 FROM "AdaptivePracticeSessions" x JOIN g10_transition_pairs p
        ON x."CurriculumAdoptionId"=p.old_id AND x."SchoolId"=p.school_id)
      OR EXISTS(SELECT 1 FROM "StudentMisconceptionStates" x JOIN g10_transition_pairs p
        ON x."CurriculumAdoptionId"=p.old_id AND x."SchoolId"=p.school_id)
      OR EXISTS(SELECT 1 FROM "AdaptivePracticeShadowObservations" x JOIN g10_transition_pairs p
        ON x."CurriculumAdoptionId"=p.old_id AND x."SchoolId"=p.school_id) THEN
        RAISE EXCEPTION 'G10 historical student/assessment evidence or learner inventory drift';
    END IF;

    -- These two exact historical placeholder outcomes/topics have no dependent
    -- evidence. Explicit FK RESTRICT is a second independent safety check.
    IF EXISTS(SELECT 1 FROM "LearningLessons" x JOIN g10_transition_topics t
        ON x."TopicId"=t.id AND x."SchoolId"=t.school_id)
      OR EXISTS(SELECT 1 FROM "AssessmentItems" x JOIN g10_transition_topics t
        ON x."CurriculumTopicId"=t.id AND x."SchoolId"=t.school_id)
      OR EXISTS(SELECT 1 FROM "ClassTopicSummaries" x JOIN g10_transition_topics t
        ON x."CurriculumTopicId"=t.id AND x."SchoolId"=t.school_id)
      OR EXISTS(SELECT 1 FROM "LearningEvidence" x JOIN g10_transition_outcomes o
        ON x."LearningOutcomeId"=o.id AND x."SchoolId"=o.school_id)
      OR EXISTS(SELECT 1 FROM "LearningLessonOutcomes" x JOIN g10_transition_outcomes o
        ON x."LearningOutcomeId"=o.id AND x."SchoolId"=o.school_id)
      OR EXISTS(SELECT 1 FROM "QuestionLearningOutcomes" x JOIN g10_transition_outcomes o
        ON x."LearningOutcomeId"=o.id AND x."SchoolId"=o.school_id)
      OR EXISTS(SELECT 1 FROM "AssessmentItemOutcomes" x JOIN g10_transition_outcomes o
        ON x."LearningOutcomeId"=o.id AND x."SchoolId"=o.school_id)
      OR EXISTS(SELECT 1 FROM "StudentOutcomeMasteries" x JOIN g10_transition_outcomes o
        ON x."LearningOutcomeId"=o.id AND x."SchoolId"=o.school_id)
      OR EXISTS(SELECT 1 FROM "ClassOutcomeSummaries" x JOIN g10_transition_outcomes o
        ON x."LearningOutcomeId"=o.id AND x."SchoolId"=o.school_id) THEN
        RAISE EXCEPTION 'G10 placeholder is not unreferenced; refuse removal';
    END IF;

    UPDATE "ClassGroups" c SET "CurriculumAdoptionId"=s.new_id,
      "Name"='Grade 10 — General — B', "NormalizedName"='GRADE 10 — GENERAL — B',
      "Code"='L10-GENERAL-B', "NormalizedCode"='L10-GENERAL-B'
    FROM g10_transition_classes s WHERE c."Id"=s.id AND c."SchoolId"=s.school_id;
    GET DIAGNOSTICS changed = ROW_COUNT;
    IF changed<>2 THEN RAISE EXCEPTION 'G10 class transition row count'; END IF;

    DELETE FROM "LearningOutcomes" o USING g10_transition_outcomes s
      WHERE o."Id"=s.id AND o."SchoolId"=s.school_id;
    GET DIAGNOSTICS changed = ROW_COUNT;
    IF changed<>2 THEN RAISE EXCEPTION 'G10 retired placeholder outcome count'; END IF;
    DELETE FROM "CurriculumTopics" t USING g10_transition_topics s
      WHERE t."Id"=s.id AND t."SchoolId"=s.school_id;
    GET DIAGNOSTICS changed = ROW_COUNT;
    IF changed<>2 THEN RAISE EXCEPTION 'G10 retired placeholder topic count'; END IF;
    DELETE FROM "SchoolCurriculumAdoptions" a USING g10_transition_pairs p
      WHERE a."Id"=p.old_id AND a."SchoolId"=p.school_id;
    GET DIAGNOSTICS changed = ROW_COUNT;
    IF changed<>2 THEN RAISE EXCEPTION 'G10 retired Advanced adoption count'; END IF;

    IF EXISTS(SELECT 1 FROM "SchoolCurriculumAdoptions"
        WHERE "CurriculumLevelKey"='UAE-MOE-MATH:L10:ADVANCED')
      OR (SELECT count(*) FROM "ClassGroups" c JOIN g10_transition_classes s
         ON c."Id"=s.id AND c."SchoolId"=s.school_id AND c."CurriculumAdoptionId"=s.new_id)<>2
      OR (SELECT count(*) FROM "StudentEnrollments" e JOIN g10_transition_classes c
        ON e."ClassGroupId"=c.id AND e."SchoolId"=c.school_id)<>50
      OR (SELECT count(*) FROM "TeacherAssignments" e JOIN g10_transition_classes c
        ON e."ClassGroupId"=c.id AND e."SchoolId"=c.school_id)<>2 THEN
        RAISE EXCEPTION 'G10 postcondition failed; rollback';
    END IF;
END
$transition$;
