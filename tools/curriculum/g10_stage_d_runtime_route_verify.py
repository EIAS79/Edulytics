#!/usr/bin/env python3
"""Audit actual same-run Grade 10 authenticated evidence, never forge per-lesson grades.

The shared manual-scoring provider can be checked with a representative
persisted grade, while all 124 distinct lesson entry pages are exercised.
This is not a claim that every student's answers were graded or mathematically correct.
"""
import json
import os
from pathlib import Path
import sys

PREFIX="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:"
COUNTS=(22,15,16,11,18,17,14,11)
ROOT=Path(__file__).resolve().parents[2]

def verify(inventory_file, rubric_file, candidate):
    inventory=json.loads(Path(inventory_file).read_text(encoding="utf8"))
    rubric=json.loads(Path(rubric_file).read_text(encoding="utf8"))
    blueprint=json.loads((ROOT/"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-geometry-im1e.lesson-blueprint.json").read_text(encoding="utf8"))
    expected={x["LessonCode"] for x in blueprint["Lessons"]}
    assert len(expected)==124
    assert all(code.startswith(PREFIX) for code in expected)
    for unit,count in enumerate(COUNTS,1):
        for lesson in range(1,count+1):
            assert PREFIX+f"U{unit:02d}:L{lesson:02d}" in expected
    assert inventory.get("candidateSha")==candidate,("candidate SHA mismatch",inventory.get("candidateSha"),candidate)
    actual=inventory["lessons"]
    assert isinstance(actual,list) and len(actual)==124
    assert {x.get("lessonCode") for x in actual}==expected
    assert all(x.get("status")=="AUTHENTICATED_RENDERED" and x.get("actualBodyLength",0)>0 for x in actual)
    assert inventory.get("checked")==124 and inventory.get("passed")==124
    assert inventory.get("rubricEntryPagesRendered")==124
    assert inventory.get("crossSchoolDenial")=="PASS"
    assert inventory.get("technicalStatus")=="PASS"
    assert not inventory.get("failures")
    assert rubric.get("course")=="G10 Geometry"
    assert rubric.get("lessonCode") in expected
    assert rubric.get("studentSubmitted") is True
    assert rubric.get("teacherScorePersisted") is True
    assert rubric.get("studentReadBack") is True
    assert rubric.get("wrongSchoolTeacherDenied") is True
    assert rubric.get("teacherAwardedScore")==3
    assert rubric.get("academicCorrectnessVerified") is False
    assert rubric.get("all124Verified") is False
    output={"candidateSha":candidate,"source":"same-run temporary PostgreSQL 18 HTTPS",
      "lessonEntryPagesVerified":124,"lessonTeacherRubricEntryPagesVerified":124,
      "distinctLessonCodes":len(expected),"persistedManualTeacherGradeSamples":1,
      "schoolIsolationChecks":"PASS","academicReviewDecision":"OWNER_APPROVAL_IS_SEPARATE",
      "digitalPracticeFamilyCoverage":"NOT_PROVEN", "all124ScoresPersisted":False,
      "technicalDecision":"SHARED_HUMAN_REVIEW_ROUTE_ACCEPTED_WITH_REPRESENTATIVE_ROUND_TRIP"}
    return output

if __name__=="__main__":
    if len(sys.argv)!=5:
        raise SystemExit("Expected inventory.json rubric.json exactSha output.json")
    result=verify(sys.argv[1],sys.argv[2],sys.argv[3])
    Path(sys.argv[4]).write_text(json.dumps(result,indent=2)+"\n",encoding="utf8")
    print("G10_124_LESSON_ROUTES_WITH_ONE_REAL_SAVED_MANUAL_GRADE",result["candidateSha"])
