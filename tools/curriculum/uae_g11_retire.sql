-- Owner-directed retirement of UAE Ministry Grade 11 only. Execute once on
-- verified production after exact candidate CI and deployment. No new backup.
-- Historical assessment/evidence, student and teacher records are retained.
DO $g11$
DECLARE
    lesson_ids uuid[];
    node_ids uuid[];
    adoption_ids uuid[];
    class_ids uuid[];
    n integer;
    lessons_before bigint;
    metadata_before bigint;
    state_id uuid;
BEGIN
    IF current_database() <> 'neondb' THEN
        RAISE EXCEPTION 'G11 retirement: unexpected database';
    END IF;
    LOCK TABLE "CurriculumPedagogicalLessons", "CurriculumLessonContents", "CurriculumPackContentNodes", "CurriculumPackImportStates", "SchoolCurriculumAdoptions", "ClassGroups" IN SHARE ROW EXCLUSIVE MODE;
    SELECT array_agg("Id") INTO lesson_ids FROM "CurriculumPedagogicalLessons" WHERE "Code" LIKE 'PED:UAE-MOE-MATH:L11:%';
    SELECT array_agg("Id") INTO node_ids FROM "CurriculumPackContentNodes" WHERE "FrameworkCode"='UAE-MOE-MATH' AND "LogicalLevelFrom"=11 AND "LogicalLevelTo"=11 AND "NodeKind" IN ('Reference','SourceCatalog');
    SELECT array_agg("Id") INTO adoption_ids FROM "SchoolCurriculumAdoptions" WHERE "CurriculumLevelKey" IN ('UAE-MOE-MATH:L11:GENERAL','UAE-MOE-MATH:L11:ADVANCED');
    SELECT array_agg("Id") INTO class_ids FROM "ClassGroups" WHERE "CurriculumAdoptionId"=ANY(adoption_ids);
    SELECT count(*) INTO lessons_before FROM "CurriculumPedagogicalLessons";
    SELECT count(*) INTO metadata_before FROM "CurriculumLessonContents";
    IF coalesce(cardinality(lesson_ids),0)=0 AND coalesce(cardinality(node_ids),0)=0 THEN
        IF EXISTS(SELECT 1 FROM "SchoolCurriculumAdoptions" WHERE "Id"=ANY(adoption_ids) AND "IsActive") OR EXISTS(SELECT 1 FROM "ClassGroups" WHERE "Id"=ANY(class_ids) AND "Status"<>2) OR NOT EXISTS(SELECT 1 FROM "CurriculumPackImportStates" WHERE "FrameworkCode"='UAE-MOE-MATH' AND "VersionCode"='MOE-2026-2027-T1' AND "NodeCount"=1105 AND "OfficialNodeCount"=1057 AND "ContentDigest"='e9ea360836a934efbeeb253fa1c0403da51d4a3e10359483f8b79508f3013be6') THEN RAISE EXCEPTION 'G11 partial prior retirement'; END IF;
        RETURN;
    END IF;
    IF cardinality(lesson_ids) IS DISTINCT FROM 123 OR cardinality(node_ids) IS DISTINCT FROM 129 OR cardinality(adoption_ids) IS DISTINCT FROM 4 OR cardinality(class_ids) IS DISTINCT FROM 4 OR lessons_before<>5066 OR metadata_before<>5066 THEN RAISE EXCEPTION 'G11 retirement: unexpected exact baseline'; END IF;
    IF (SELECT encode(sha256(convert_to(string_agg("Code",chr(10) ORDER BY "Code" COLLATE "C"),'UTF8')),'hex') FROM "CurriculumPedagogicalLessons" WHERE "Id"=ANY(lesson_ids)) IS DISTINCT FROM 'd058eb29f085df8de09ebb46ba6404696fc44388c0bd96afd6e8f1282b5305e3' OR (SELECT encode(sha256(convert_to(string_agg("Code",chr(10) ORDER BY "Code" COLLATE "C"),'UTF8')),'hex') FROM "CurriculumPackContentNodes" WHERE "Id"=ANY(node_ids)) IS DISTINCT FROM '3931c8676aedc61e28b209bb594bd1b1c0a6b021abd6635c5ed05417c808ba66' THEN RAISE EXCEPTION 'G11 retirement: identity digest mismatch'; END IF;
    IF (SELECT count(*) FROM "CurriculumLessonContents" WHERE "LessonNodeId"=ANY(lesson_ids) AND "Status"=3)<>123 OR (SELECT count(*) FROM "CurriculumLessonContents" WHERE "LessonNodeId"=ANY(lesson_ids))<>123 OR (SELECT count(*) FROM "CurriculumPackContentNodes" WHERE "FrameworkCode"='UAE-MOE-MATH')<>1234 OR (SELECT count(*) FROM "CurriculumPedagogicalLessons" WHERE "Code" LIKE 'PED:UAE-MOE-MATH:L10:GENERAL:%')<>148 THEN RAISE EXCEPTION 'G11 retirement: content baseline mismatch'; END IF;
    IF EXISTS(SELECT 1 FROM "CurriculumLessonContentTranslations" t JOIN "CurriculumLessonContents" m ON m."Id"=t."CurriculumLessonContentId" WHERE m."LessonNodeId"=ANY(lesson_ids)) OR EXISTS(SELECT 1 FROM "CurriculumPedagogicalLessonOutcomes" WHERE "PedagogicalLessonId"=ANY(lesson_ids) OR "OutcomeNodeId"=ANY(node_ids)) OR EXISTS(SELECT 1 FROM "AssessmentItems" WHERE "CurriculumPedagogicalLessonId"=ANY(lesson_ids)) OR EXISTS(SELECT 1 FROM "PracticeAttempts" WHERE "CurriculumPedagogicalLessonId"=ANY(lesson_ids)) OR EXISTS(SELECT 1 FROM "AdaptivePracticeSessions" WHERE "CurriculumPedagogicalLessonId"=ANY(lesson_ids)) OR EXISTS(SELECT 1 FROM "AdaptivePracticeShadowObservations" WHERE "CurriculumPedagogicalLessonId"=ANY(lesson_ids)) OR EXISTS(SELECT 1 FROM "CurriculumPackNodeLinks" WHERE "FromNodeId"=ANY(node_ids) OR "ToNodeId"=ANY(node_ids)) OR EXISTS(SELECT 1 FROM "LearningOutcomes" WHERE "OfficialContentNodeId"=ANY(node_ids)) OR EXISTS(SELECT 1 FROM "CurriculumPackContentNodes" WHERE "ParentId"=ANY(node_ids) AND NOT ("Id"=ANY(node_ids))) OR EXISTS(SELECT 1 FROM "CurriculumPedagogicalLessons" WHERE "OfficialLessonNodeId"=ANY(node_ids) AND NOT ("Id"=ANY(lesson_ids))) THEN RAISE EXCEPTION 'G11 retirement: unexpected dependent records; no data removed'; END IF;
    SELECT "Id" INTO STRICT state_id FROM "CurriculumPackImportStates" WHERE "FrameworkCode"='UAE-MOE-MATH' AND "VersionCode"='MOE-2026-2027-T1' AND ( ("NodeCount"=1532 AND "OfficialNodeCount"=1420 AND "ContentDigest"='ff6200142b1eea8f12a7d5ff0fd172259582044359c642aeb3c045e5e8886372') OR ("NodeCount"=1234 AND "OfficialNodeCount"=1180 AND "ContentDigest"='51eddf58a55de58b6a1861b618ec7d6dc1ceb8a08f3f04a2ccb5d78d8f94768b'));
    UPDATE "SchoolCurriculumAdoptions" SET "IsActive"=false,"UpdatedAtUtc"=CURRENT_TIMESTAMP WHERE "Id"=ANY(adoption_ids) AND "IsActive";
    UPDATE "ClassGroups" SET "Status"=2 WHERE "Id"=ANY(class_ids) AND "Status"=1;
    DELETE FROM "CurriculumLessonContents" WHERE "LessonNodeId"=ANY(lesson_ids);
    GET DIAGNOSTICS n=ROW_COUNT; IF n<>123 THEN RAISE EXCEPTION 'G11 metadata delete count mismatch'; END IF;
    DELETE FROM "CurriculumPedagogicalLessons" WHERE "Id"=ANY(lesson_ids);
    GET DIAGNOSTICS n=ROW_COUNT; IF n<>123 THEN RAISE EXCEPTION 'G11 lesson delete count mismatch'; END IF;
    DELETE FROM "CurriculumPackContentNodes" WHERE "Id"=ANY(node_ids);
    GET DIAGNOSTICS n=ROW_COUNT; IF n<>129 THEN RAISE EXCEPTION 'G11 source-node delete count mismatch'; END IF;
    UPDATE "CurriculumPackImportStates" SET "NodeCount"=1105,"OfficialNodeCount"=1057,"UnitCount"=0,"LessonCount"=0,"LinkCount"=0,"SourceDigest"='fc15730084f266a4384265c5382697edfaa52b5333bee70a69a52b757aa311d6',"ContentDigest"='e9ea360836a934efbeeb253fa1c0403da51d4a3e10359483f8b79508f3013be6',"IsComplete"=true,"ImportedAtUtc"=CURRENT_TIMESTAMP WHERE "Id"=state_id;
    IF (SELECT count(*) FROM "CurriculumPedagogicalLessons")<>lessons_before-123 OR (SELECT count(*) FROM "CurriculumLessonContents")<>metadata_before-123 OR (SELECT count(*) FROM "CurriculumPackContentNodes" WHERE "FrameworkCode"='UAE-MOE-MATH')<>1105 OR EXISTS(SELECT 1 FROM "CurriculumPedagogicalLessons" WHERE "Code" LIKE 'PED:UAE-MOE-MATH:L11:%') OR EXISTS(SELECT 1 FROM "SchoolCurriculumAdoptions" WHERE "Id"=ANY(adoption_ids) AND "IsActive") OR EXISTS(SELECT 1 FROM "ClassGroups" WHERE "Id"=ANY(class_ids) AND "Status"<>2) THEN RAISE EXCEPTION 'G11 retirement postcondition failed'; END IF;
END $g11$;
