#!/usr/bin/env python3
"""Exercise the actual scoped tenant transition on disposable PostgreSQL only."""
import hashlib
import json
import os
import pathlib
import re
import subprocess

ROOT = pathlib.Path(__file__).resolve().parents[2]
MIGRATION = ROOT / "tools/curriculum/uae_g10_tenant_transition.sql"
OUT = pathlib.Path("/tmp/grade10-tenant-transition.json")
PSQL = ["psql", "-X", "-h", "127.0.0.1", "-U", "postgres",
        "-d", "edulytics_clean_ci", "-Atq", "-v", "ON_ERROR_STOP=1"]
ADV = "UAE-MOE-MATH:L10:ADVANCED"
GEN = "UAE-MOE-MATH:L10:GENERAL"


def guard():
    for key, value in {
        "GITHUB_ACTIONS": "true", "EDULYTICS_CI_REHEARSAL": "true",
        "EDULYTICS_CI_REHEARSAL_PROGRAMME": "UAE-G10",
        "EDULYTICS_CLEAN_DATABASE_NAME": "edulytics_clean_ci",
        "ASPNETCORE_ENVIRONMENT": "Staging",
    }.items():
        if os.environ.get(key) != value:
            raise RuntimeError("G10 tenant rehearsal refuses non-disposable target: " + key)
    assert scalar("SELECT current_database()") == "edulytics_clean_ci"
    assert re.fullmatch(r"[a-f0-9]{40}", os.environ["GITHUB_SHA"])


def scalar(query):
    return subprocess.check_output(PSQL + ["-c", query], text=True).strip()


def rows(query):
    return json.loads(scalar("SELECT COALESCE(json_agg(row_to_json(q)),'[]') FROM (" + query + ") q"))


def ids_sql(ids):
    assert ids and all(re.fullmatch(r"[a-f0-9-]{36}", x) for x in ids)
    return ",".join("'" + x + "'::uuid" for x in ids)


def digest(table, where="true"):
    return rows('SELECT count(*) AS count,md5(COALESCE(string_agg(to_jsonb(t)::text,chr(10) ORDER BY to_jsonb(t)::text),\'\')) AS sha FROM "' + table + '" t WHERE ' + where)[0]


def scope():
    adoptions = rows('SELECT "Id"::text AS id,"SchoolId"::text AS school,"CurriculumLevelKey" AS key FROM "SchoolCurriculumAdoptions" WHERE "CurriculumLevelKey" IN (\'' + ADV + '\',\'' + GEN + '\') ORDER BY "Id"')
    adv = [x["id"] for x in adoptions if x["key"] == ADV]
    assert len(adv) == 2 and len(adoptions) == 4
    filt = '"CurriculumAdoptionId" IN (' + ids_sql(adv) + ')'
    classes = rows('SELECT * FROM "ClassGroups" WHERE ' + filt + ' ORDER BY "Id"')
    topics = rows('SELECT * FROM "CurriculumTopics" WHERE ' + filt + ' ORDER BY "Id"')
    outcomes = rows('SELECT * FROM "LearningOutcomes" WHERE ' + filt + ' ORDER BY "Id"')
    assert len(classes) == len(topics) == len(outcomes) == 2
    return {"adoptions": adoptions, "classes": classes, "topics": topics, "outcomes": outcomes}


def stable_snapshot(before):
    modified = {
        "SchoolCurriculumAdoptions": [x["id"] for x in before["adoptions"] if x["key"] == ADV],
        "ClassGroups": [x["Id"] for x in before["classes"]],
        "CurriculumTopics": [x["Id"] for x in before["topics"]],
        "LearningOutcomes": [x["Id"] for x in before["outcomes"]],
    }
    tables = [
        "Schools", "AspNetUsers", "StudentProfiles", "StudentEnrollments",
        "TeacherAssignments", "AcademicYears", "AcademicPrograms", "GradeLevels",
        "Subjects", "Assessments", "AssessmentItems", "AssessmentAttempts",
        "AssessmentItemOutcomes", "PracticeAttempts", "LearningLessons",
        "LearningEvidence", "LearningLessonOutcomes", "QuestionLearningOutcomes",
        "StudentOutcomeMasteries", "ClassOutcomeSummaries", "ClassTopicSummaries",
        "ClassAssessmentTrends", "AdaptivePracticeSessions", "StudentMisconceptionStates",
        "AdaptivePracticeShadowObservations", "CurriculumFrameworks",
        "CurriculumFrameworkVersions", "CurriculumPackContentNodes",
        "CurriculumPackNodeLinks", "CurriculumPackImportStates",
        "CurriculumPedagogicalLessons", "CurriculumLessonContents",
        "CurriculumPedagogicalLessonOutcomes", *modified,
    ]
    return {t: digest(t, '"Id" NOT IN (' + ids_sql(modified[t]) + ')' if t in modified else "true")
            for t in tables}


def all_snapshot(before):
    return {
        "stable": stable_snapshot(before),
        "adoptions": digest("SchoolCurriculumAdoptions"),
        "classes": digest("ClassGroups"),
        "topics": digest("CurriculumTopics"),
        "outcomes": digest("LearningOutcomes"),
    }


def apply(*, bad_placeholder=False, fail_after=False):
    args = PSQL + ["-1", "-c", "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE",
                   "-c", "SET LOCAL edulytics.g10_transition_scope='UAE-MOE-MATH:L10:ADVANCED_TO_GENERAL:2'"]
    if bad_placeholder:
        args += ["-c", 'UPDATE "CurriculumTopics" SET "Name"=\'G10_REHEARSAL_NEGATIVE_CONTROL\' WHERE "Id"=(SELECT t."Id" FROM "CurriculumTopics" t JOIN "SchoolCurriculumAdoptions" a ON a."Id"=t."CurriculumAdoptionId" AND a."SchoolId"=t."SchoolId" WHERE a."CurriculumLevelKey"=\'' + ADV + '\' ORDER BY t."Id" LIMIT 1)']
    args += ["-f", str(MIGRATION)]
    if fail_after:
        args += ["-c", "DO $$ BEGIN RAISE EXCEPTION 'G10_ROLLBACK_NEGATIVE_CONTROL'; END $$;"]
    result = subprocess.run(args, text=True, capture_output=True)
    if bad_placeholder or fail_after:
        assert result.returncode != 0, "Negative control did not fail closed"
        expected = "not the exact unused historical placeholders" if bad_placeholder else "G10_ROLLBACK_NEGATIVE_CONTROL"
        assert expected in result.stderr, result.stderr[-1500:]
    else:
        assert result.returncode == 0, result.stderr[-1500:]


def run():
    guard()
    before = scope()
    stable_before = stable_snapshot(before)
    initial = all_snapshot(before)
    apply(bad_placeholder=True)
    assert all_snapshot(before) == initial, "Bad placeholder attempt changed the DB"
    apply(fail_after=True)
    assert all_snapshot(before) == initial, "Rollback after DML did not restore the DB"
    apply()
    class_ids = [x["Id"] for x in before["classes"]]
    moved = rows('SELECT c."Id"::text AS id,c."CurriculumAdoptionId"::text AS adoption,c."Name" AS name,c."Code" AS code,a."CurriculumLevelKey" AS key FROM "ClassGroups" c JOIN "SchoolCurriculumAdoptions" a ON a."Id"=c."CurriculumAdoptionId" AND a."SchoolId"=c."SchoolId" WHERE c."Id" IN (' + ids_sql(class_ids) + ') ORDER BY c."Id"')
    assert {x["id"] for x in moved} == set(class_ids)
    assert all(x["key"] == GEN and x["name"] == "Grade 10 — General — B" and x["code"] == "L10-GENERAL-B" for x in moved)
    assert {x["adoption"] for x in moved} == {x["id"] for x in before["adoptions"] if x["key"] == GEN}
    adv_after = int(scalar('SELECT count(*) FROM "SchoolCurriculumAdoptions" WHERE "CurriculumLevelKey"=\'' + ADV + '\''))
    assert adv_after == 0
    enrollments = int(scalar('SELECT count(*) FROM "StudentEnrollments" WHERE "ClassGroupId" IN (' + ids_sql(class_ids) + ')'))
    teachers = int(scalar('SELECT count(*) FROM "TeacherAssignments" WHERE "ClassGroupId" IN (' + ids_sql(class_ids) + ')'))
    assert enrollments == 50 and teachers == 2
    stable_after = stable_snapshot(before)
    assert stable_before == stable_after, "Unexpected unrelated or preserved rows changed"
    after = all_snapshot(before)
    apply()
    assert all_snapshot(before) == after, "Idempotent replay altered persisted rows"
    report = {
        "candidateSha": os.environ["GITHUB_SHA"], "status": "PASS",
        "scope": "UAE-G10-GENERAL", "productionWritten": False,
        "migrationSha256": hashlib.sha256(MIGRATION.read_bytes()).hexdigest(),
        "advancedAdoptionsBefore": 2, "advancedAdoptionsAfter": adv_after,
        "classesRehomed": len(moved), "enrollmentsPreserved": enrollments,
        "teacherAssignmentsPreserved": teachers,
        "retiredUnusedPlaceholderTopics": len(before["topics"]),
        "retiredUnusedPlaceholderOutcomes": len(before["outcomes"]),
        "historicalOfficialNodesPreserved": stable_before["CurriculumPackContentNodes"] == stable_after["CurriculumPackContentNodes"],
        "unrelatedRowsPreserved": stable_before == stable_after,
        "rollbackNegativeControlPassed": True, "idempotentReplayPassed": True,
        "before": before, "afterClasses": moved, "preservedTableDigests": stable_after,
    }
    OUT.write_text(json.dumps(report, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print("G10_TENANT_TRANSITION_ROLLBACK_REPLAY_PASS")


if __name__ == "__main__":
    run()
