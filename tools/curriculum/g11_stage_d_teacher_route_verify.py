#!/usr/bin/env python3
"""Fail-closed verifier for actual G11 CI HTTPS and persisted teacher-review evidence.
A course-wide teacher-marked route is not machine-graded Practice capability.
"""
import json,sys,os
from pathlib import Path
assert len(sys.argv)==5, "usage: verify.py rendered.json teacher.json exact_sha output.json"
assert os.environ.get("GITHUB_ACTIONS")=="true" and os.environ.get("GITHUB_RUN_ID","").isdigit(), "must run inside GitHub Actions"
render=json.loads(Path(sys.argv[1]).read_text(encoding="utf8"))
review=json.loads(Path(sys.argv[2]).read_text(encoding="utf8"))
expected_sha=sys.argv[3]
prefix="PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:"
assert len(expected_sha)==40 and all(c in "0123456789abcdef" for c in expected_sha)
assert render.get("candidateSha")==expected_sha, "runtime candidate SHA mismatch"
assert render.get("expected")==120
assert render.get("checked")==120
assert render.get("passed")==120
assert render.get("rubricEntryPagesRendered")==120, "missing 120 rubric entry pages"
assert render.get("crossSchoolDenial")=="PASS", "student tenant boundary failed"
assert render.get("technicalRenderingStatus")=="PASS"
assert render.get("failures")==[], "uncleared student lesson response failures"
rows=render.get("lessons")
assert isinstance(rows,list) and len(rows)==120
assert len({x["lessonCode"] for x in rows})==120
assert all(x["lessonCode"].startswith(prefix) and
           x["status"]=="AUTHENTICATED_RICHV2_RENDER" for x in rows)
assert review.get("lessonCode")==prefix+"U01:L01"
assert review.get("studentSubmitted") is True
assert review.get("studentsSubmittedCount")==120
assert review.get("teacherScoresPersistedCount")==120
assert review.get("studentReadBackCount")==120
assert review.get("databaseRowsVerified")==120
assert review.get("duplicateWriteDenied") is True
assert len(review.get("gradedLessonCodes",[]))==120
assert set(review["gradedLessonCodes"])=={x["lessonCode"] for x in rows}

assert review.get("teacherAwardedScore")==3
assert review.get("teacherScorePersisted") is True
assert review.get("studentReadBack") is True
assert review.get("wrongSchoolTeacherDenied") is True
assert review.get("academicCorrectnessVerified") is False
out={
  "candidateSha":expected_sha,
  "actionsRunId":int(os.environ["GITHUB_RUN_ID"]),
  "studentRichV2Verified":120,
  "perLessonTeacherRubricEntryVerified":120,
  "teacherSubmissionAndReviewPersistedOnSampleLesson":True,
  "teacherSubmissionAndReviewPersistedForAll120":True,
  "wrongSchoolTeacherAndStudentDenied":True,
  "coverage":"120 eligible teacher-reviewed entry routes; one representative persisted grade, NOT 120 graded student attempts",
  "academicOrSourceParityProven":False,
  "digitalPracticeQuestionFamiliesVerifiedHere":0,
  "status":"TEACHER_REVIEW_RUNTIME_ROUTE_PASS"
}
Path(sys.argv[4]).write_text(json.dumps(out,indent=2)+"\n",encoding="utf8")
print(json.dumps(out,indent=2))
