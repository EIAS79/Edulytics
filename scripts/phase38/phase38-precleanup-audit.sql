-- Edulytics — Phase 38 pre-cleanup audit
-- READ-ONLY by design. This script performs no writes and ends with ROLLBACK.
-- Baseline prepared after Phase43 main SHA 60c69918477070210a4c455331c71ea9b8247ffc.

BEGIN TRANSACTION ISOLATION LEVEL REPEATABLE READ READ ONLY;

-- -----------------------------------------------------------------------------
-- 1. Hard preservation guards.
-- -----------------------------------------------------------------------------
DO $$
DECLARE
    global_frameworks bigint;
    school_owned_frameworks bigint;
    global_superadmins bigint;
    roles_count bigint;
    migrations_count bigint;
BEGIN
    SELECT count(*)
      INTO global_frameworks
      FROM public."CurriculumFrameworks"
     WHERE "OwnerSchoolId" IS NULL;

    SELECT count(*)
      INTO school_owned_frameworks
      FROM public."CurriculumFrameworks"
     WHERE "OwnerSchoolId" IS NOT NULL;

    SELECT count(DISTINCT u."Id")
      INTO global_superadmins
      FROM public."AspNetUsers" u
      JOIN public."AspNetUserRoles" ur ON ur."UserId" = u."Id"
      JOIN public."AspNetRoles" r ON r."Id" = ur."RoleId"
     WHERE u."SchoolId" IS NULL
       AND u."IsActive" = true
       AND r."Name" = 'SuperAdmin';

    SELECT count(*) INTO roles_count
      FROM public."AspNetRoles";

    SELECT count(*) INTO migrations_count
      FROM public."__EFMigrationsHistory";

    IF global_frameworks <> 5 THEN
        RAISE EXCEPTION
            'PRESERVATION_GUARD_FAILED: expected 5 global curriculum frameworks, found %',
            global_frameworks;
    END IF;

    IF school_owned_frameworks <> 0 THEN
        RAISE EXCEPTION
            'PRESERVATION_GUARD_FAILED: found % school-owned curriculum frameworks; cleanup scope must be reviewed manually',
            school_owned_frameworks;
    END IF;

    IF global_superadmins <> 1 THEN
        RAISE EXCEPTION
            'PRESERVATION_GUARD_FAILED: expected exactly 1 active global SuperAdmin, found %',
            global_superadmins;
    END IF;

    IF roles_count <> 5 THEN
        RAISE EXCEPTION
            'PRESERVATION_GUARD_FAILED: expected 5 system roles, found %',
            roles_count;
    END IF;

    IF migrations_count < 21 THEN
        RAISE EXCEPTION
            'PRESERVATION_GUARD_FAILED: migration history unexpectedly short (% rows)',
            migrations_count;
    END IF;
END $$;

-- -----------------------------------------------------------------------------
-- 2. Official/system corpus — MUST survive final cleanup.
-- -----------------------------------------------------------------------------
SELECT *
FROM (
    SELECT 'AspNetRoles' AS item, count(*)::bigint AS row_count
      FROM public."AspNetRoles"
    UNION ALL
    SELECT 'CurriculumFrameworks', count(*)
      FROM public."CurriculumFrameworks"
    UNION ALL
    SELECT 'CurriculumFrameworkVersions', count(*)
      FROM public."CurriculumFrameworkVersions"
    UNION ALL
    SELECT 'CurriculumPackContentNodes', count(*)
      FROM public."CurriculumPackContentNodes"
    UNION ALL
    SELECT 'CurriculumPackNodeLinks', count(*)
      FROM public."CurriculumPackNodeLinks"
    UNION ALL
    SELECT 'CurriculumPedagogicalLessons', count(*)
      FROM public."CurriculumPedagogicalLessons"
    UNION ALL
    SELECT 'CurriculumPedagogicalLessonOutcomes', count(*)
      FROM public."CurriculumPedagogicalLessonOutcomes"
    UNION ALL
    SELECT 'CurriculumLessonContents', count(*)
      FROM public."CurriculumLessonContents"
    UNION ALL
    SELECT 'CurriculumLessonContentTranslations', count(*)
      FROM public."CurriculumLessonContentTranslations"
    UNION ALL
    SELECT '__EFMigrationsHistory', count(*)
      FROM public."__EFMigrationsHistory"
) preserved
ORDER BY item;

-- Framework identity guard: all five official frameworks are global.
SELECT "Code", "Name", "OwnerSchoolId"
FROM public."CurriculumFrameworks"
ORDER BY "Code";

-- -----------------------------------------------------------------------------
-- 3. Tenant inventory — operational data candidates for final cleanup.
--    This reports scope only; it does not remove anything.
-- -----------------------------------------------------------------------------
SELECT
    s."Id" AS school_id,
    s."Name" AS school_name,
    s."Status" AS school_status,
    (SELECT count(*) FROM public."AspNetUsers" u WHERE u."SchoolId" = s."Id") AS users,
    (SELECT count(*) FROM public."AcademicYears" x WHERE x."SchoolId" = s."Id") AS academic_years,
    (SELECT count(*) FROM public."AcademicPrograms" x WHERE x."SchoolId" = s."Id") AS academic_programs,
    (SELECT count(*) FROM public."AcademicYearProgramOfferings" x WHERE x."SchoolId" = s."Id") AS program_offerings,
    (SELECT count(*) FROM public."SchoolCurriculumAdoptions" x WHERE x."SchoolId" = s."Id") AS curriculum_adoptions,
    (SELECT count(*) FROM public."GradeLevels" x WHERE x."SchoolId" = s."Id") AS grade_levels,
    (SELECT count(*) FROM public."Subjects" x WHERE x."SchoolId" = s."Id") AS subjects,
    (SELECT count(*) FROM public."Terms" x WHERE x."SchoolId" = s."Id") AS terms,
    (SELECT count(*) FROM public."ClassGroups" x WHERE x."SchoolId" = s."Id") AS classes,
    (SELECT count(*) FROM public."CurriculumTopics" x WHERE x."SchoolId" = s."Id") AS curriculum_topics,
    (SELECT count(*) FROM public."LearningOutcomes" x WHERE x."SchoolId" = s."Id") AS learning_outcomes,
    (SELECT count(*) FROM public."StudentProfiles" x WHERE x."SchoolId" = s."Id") AS students,
    (SELECT count(*) FROM public."StudentEnrollments" x WHERE x."SchoolId" = s."Id") AS enrollments,
    (SELECT count(*) FROM public."TeacherAssignments" x WHERE x."SchoolId" = s."Id") AS teacher_assignments,
    (SELECT count(*) FROM public."SubjectSupervisorAssignments" x WHERE x."SchoolId" = s."Id") AS supervisor_assignments,
    (SELECT count(*) FROM public."Assessments" x WHERE x."SchoolId" = s."Id") AS assessments,
    (SELECT count(*) FROM public."AssessmentItems" x WHERE x."SchoolId" = s."Id") AS assessment_items,
    (SELECT count(*) FROM public."AssessmentResults" x WHERE x."SchoolId" = s."Id") AS assessment_results,
    (SELECT count(*) FROM public."PracticeAttempts" x WHERE x."SchoolId" = s."Id") AS practice_attempts,
    (SELECT count(*) FROM public."LearningEvidence" x WHERE x."SchoolId" = s."Id") AS learning_evidence,
    (SELECT count(*) FROM public."StudentOutcomeMasteries" x WHERE x."SchoolId" = s."Id") AS student_masteries,
    (SELECT count(*) FROM public."ReportExportJobs" x WHERE x."SchoolId" = s."Id") AS report_exports,
    (SELECT count(*) FROM public."ImportBatches" x WHERE x."SchoolId" = s."Id") AS imports,
    (SELECT count(*) FROM public."UserNotifications" x WHERE x."SchoolId" = s."Id") AS notifications,
    (SELECT count(*) FROM public."OutboxMessages" x WHERE x."SchoolId" = s."Id") AS outbox_messages,
    (SELECT count(*) FROM public."AuditLogs" x WHERE x."SchoolId" = s."Id") AS audit_logs,
    (SELECT count(*) FROM public."SchoolSubscriptions" x WHERE x."SchoolId" = s."Id") AS subscriptions,
    (SELECT count(*) FROM public."SchoolTrials" x WHERE x."SchoolId" = s."Id") AS trials,
    (SELECT count(*) FROM public."SchoolBillingProfiles" x WHERE x."SchoolId" = s."Id") AS billing_profiles
FROM public."Schools" s
ORDER BY s."Name", s."Id";

-- User scope summary without exposing password hashes or other credentials.
SELECT
    CASE WHEN u."SchoolId" IS NULL THEN 'GLOBAL' ELSE 'SCHOOL' END AS scope,
    u."IsActive" AS is_active,
    COALESCE(r."Name", '(no role)') AS role,
    count(DISTINCT u."Id") AS users
FROM public."AspNetUsers" u
LEFT JOIN public."AspNetUserRoles" ur ON ur."UserId" = u."Id"
LEFT JOIN public."AspNetRoles" r ON r."Id" = ur."RoleId"
GROUP BY 1, 2, 3
ORDER BY 1, 2 DESC, 3;

-- -----------------------------------------------------------------------------
-- 4. School deletion rules.
--    A direct DELETE FROM Schools is intentionally NOT safe: most child FKs are
--    RESTRICT, so the final cleanup must use a dependency-ordered transaction.
-- -----------------------------------------------------------------------------
SELECT
    tc.table_name,
    kcu.column_name,
    rc.delete_rule
FROM information_schema.table_constraints tc
JOIN information_schema.key_column_usage kcu
  ON tc.constraint_name = kcu.constraint_name
 AND tc.constraint_schema = kcu.constraint_schema
JOIN information_schema.constraint_column_usage ccu
  ON ccu.constraint_name = tc.constraint_name
 AND ccu.constraint_schema = tc.constraint_schema
JOIN information_schema.referential_constraints rc
  ON rc.constraint_name = tc.constraint_name
 AND rc.constraint_schema = tc.constraint_schema
WHERE tc.constraint_type = 'FOREIGN KEY'
  AND tc.table_schema = 'public'
  AND ccu.table_name = 'Schools'
ORDER BY tc.table_name, kcu.column_name;

-- -----------------------------------------------------------------------------
-- 5. Zero-activity checks useful before final cleanup.
-- -----------------------------------------------------------------------------
SELECT *
FROM (
    SELECT 'AssessmentItems' AS item, count(*)::bigint AS row_count FROM public."AssessmentItems"
    UNION ALL SELECT 'AssessmentResults', count(*) FROM public."AssessmentResults"
    UNION ALL SELECT 'StudentAnswers', count(*) FROM public."StudentAnswers"
    UNION ALL SELECT 'PracticeAttempts', count(*) FROM public."PracticeAttempts"
    UNION ALL SELECT 'PracticeAttemptItems', count(*) FROM public."PracticeAttemptItems"
    UNION ALL SELECT 'PracticeResponses', count(*) FROM public."PracticeResponses"
    UNION ALL SELECT 'LearningEvidence', count(*) FROM public."LearningEvidence"
    UNION ALL SELECT 'StudentOutcomeMasteries', count(*) FROM public."StudentOutcomeMasteries"
    UNION ALL SELECT 'ClassOutcomeSummaries', count(*) FROM public."ClassOutcomeSummaries"
    UNION ALL SELECT 'ClassTopicSummaries', count(*) FROM public."ClassTopicSummaries"
    UNION ALL SELECT 'ReportExportJobs', count(*) FROM public."ReportExportJobs"
    UNION ALL SELECT 'ImportBatches', count(*) FROM public."ImportBatches"
    UNION ALL SELECT 'ImportValidationErrors', count(*) FROM public."ImportValidationErrors"
) activity
ORDER BY item;

-- No changes can survive this audit.
ROLLBACK;
