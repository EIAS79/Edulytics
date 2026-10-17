#!/usr/bin/env python3
"""Generate a truthful 20-cohort evidence/backlog ledger from already recorded facts.
Read-only inputs; no academic PASS awarded; no source-book text is copied.
"""
from pathlib import Path
import json, hashlib, argparse
root=Path(__file__).resolve().parents[2]
folder=root/"docs/curriculum/batches"
def load(name):return json.loads((folder/name).read_text(encoding="utf8"))
def build():
    intake=load("US-CCSS-B001-20-INTAKE.json")
    source=load("US-CCSS-B001-2021-SOURCE-PEDAGOGY-PACKETS-1-OF-2.json")
    fingerprint=load("US-CCSS-B001-IM2021-500-SOURCE-FINGERPRINTS-TRIAGE.json")
    studentpdf=load("US-CCSS-B001-20-ORIGINAL-STUDENT-PDF-TEXT-REVIEW.json")
    assert studentpdf["metrics"]["pdfTextsReadFirstHand"]==20
    assert studentpdf["metrics"]["academicallyAcceptedUnderV2"]==0
    assert studentpdf["metrics"]["releaseDecision"]=="NO_GO"
    assert len(studentpdf["lessons"])==20
    pdf_by_code={x["lessonCode"]:x for x in studentpdf["lessons"]}
    assert len(pdf_by_code)==20
    assert intake["planId"]=="EDULYTICS-US-CCSS-20-V2" and intake["total"]==20
    assert source["lessonCount"]==250 and fingerprint["batchId"]=="US-CCSS-B001"
    lookup1={x["code"]:x for x in source["lessons"]}
    lookup2={x["lessonCode"]:x for x in fingerprint["lessons"]}
    rows=[]
    for x in intake["lessons"]:
        code=x["lessonCode"]; ref=lookup1[code]; fingerprints=lookup2[code]
        assert ref["ordinal"]==x["programmeOrdinal"]==fingerprints["ordinal"]
        assert ref["title"]==x["title"]
        pdf=pdf_by_code[code]
        assert pdf["gradeOrdinal"]==x["gradeOrdinal"]
        assert pdf["firstHandPdfTextReviewed"] is True
        assert len(pdf["pdfSha256"])==64
        assert pdf["academicallyAccepted"] is False
        if x["programmeOrdinal"] in (2,3):stage="HISTORICAL_SHIPPED_G1_U01_L02_L03_REPAIR_CANDIDATE_REQUIRES_DRIFT_AND_FRESH_ACADEMIC_PROOF"
        elif x["programmeOrdinal"]<=15:stage="HISTORICAL_SHIPPED_REQUIRES_DRIFT_AND_FRESH_ACADEMIC_PROOF"
        elif x["programmeOrdinal"]<=18:stage="SOURCE_GUARDED_TEACHING_PATCH_IN_MAIN_REQUIRES_ACADEMIC_PROOF"
        else:stage="TWO_LESSON_SOURCE_GOAL_PRACTICE_REPAIR_CANDIDATE_NOT_MERGED"
        rows.append({
            "lessonCode":code, "gradeOrdinal":x["gradeOrdinal"],"lessonTitle":x["title"],
            "historicalPreviouslyShipped":x["historicalPreviouslyShipped"],
            "p3ImplementationStage":stage,
            "sourceTeacherPreparationSha256":fingerprints["sourcePreparationSha256"],
            "sourceTeacherLessonSha256":fingerprints["sourceFullLessonSha256"],
            "original2021StudentPdfSha256":pdf["pdfSha256"],
            "originalStudentPdfPageCount":pdf["pages"],
            "sourceStudentTaskTextReview":"FIRST_HAND_TEXT_REVIEWED",
            "sourceStudentTaskSummary":pdf["paraphrasedStudentTaskCoverage"],
            "sourceStudentTaskGraphicReview":"FULL_VISUAL_PARITY_PENDING",
            "studentActivityParityBlocker":pdf["academicOrInteractionEvidenceStillRequired"],
            "sourceActivityCountRecorded":len(ref["originalActivities"]),
            "sourceActivityNamesOnly":[a["name"] for a in ref["originalActivities"]],
            "sourceExtractionReviewFlags":ref["reviewFlags"],
            "sourceTeacherPurposeGoalPresent":bool(fingerprints["lessonPurposeExtracted"] and fingerprints["teacherLearningGoalsExtracted"]),
            "originalStudentActivityParity":"NOT_INDEPENDENTLY_APPROVED",
            "actualEffectiveLearnerBodyParity":"NOT_INDEPENDENTLY_APPROVED",
            "perLessonExactPracticeMathOracle":"NOT_INDEPENDENTLY_ACCEPTED",
            "historicalContentAndAttemptDrift":"NOT_VERIFIED" if x["historicalPreviouslyShipped"] else "NOT_APPLICABLE",
            "actualP4EvidenceOnReleaseCandidateSha":"NOT_RECONCILED_ON_CURRENT_P3_CANDIDATE",
            "p5AcademicAcceptance":"NOT_ACCEPTED",
            "p6LiveLessonReleaseAndUat":"NOT_VERIFIED",
        })
    assert len(rows)==20 and len({x["lessonCode"] for x in rows})==20
    assert sum(x["historicalPreviouslyShipped"] for x in rows)==15
    return {
        "schemaVersion":1,"planId":intake["planId"],"batchId":intake["batchId"],
        "cohortSize":20,"inputs":{
            "canonical20Intake":"docs/curriculum/batches/US-CCSS-B001-20-INTAKE.json",
            "historicSourcePacket":"docs/curriculum/batches/US-CCSS-B001-2021-SOURCE-PEDAGOGY-PACKETS-1-OF-2.json",
            "historicSourceFingerprintTriage":"docs/curriculum/batches/US-CCSS-B001-IM2021-500-SOURCE-FINGERPRINTS-TRIAGE.json",
            "firstHandOriginalStudentPdfReview":"docs/curriculum/batches/US-CCSS-B001-20-ORIGINAL-STUDENT-PDF-TEXT-REVIEW.json"},
        "assurance":"Machine-grounded status accounting only. Archived 2021 teacher/student-facing sources inspected previously. No academic PASS, source book text replication, new production release or live student UAT.",
        "counts":{
            "canonicalCodes":20,"historicallyShipped":15,
            "previouslyUnshippedWithMainTeachingCorrections":3,
            "previouslyUnshippedWithCurrentP3UnmergedCandidate":2,
            "historicallyShippedWithNewG1U01L02L03P3RepairCandidates":2,
            "totalFourSourceBoundRepairsInIntegratedDraft":4,
            "original2021StudentPdfFirstHandTextRead":20,
            "original2021StudentPdfFullGraphicsAccepted":0,
            "academicAcceptedUnderV2_20":0,"historicalDriftAccepted":0,
            "originalActivityAndLearnerParityAccepted":0,
            "releaseDecision":"NO_GO"},
        "outstandingGates":[
            "Original 20 student task PDFs have first-hand text review; full graphics, original activity equivalence, and student-produced work remain independently unaccepted",
            "Actual effective rendered lesson body SHA/provenance and source-edition review for all 20",
            "Exact Practice family and independent mathematical oracle evidence on tested SHA for all 20",
            "Verify previous 15 effective-body versions, student progress and recorded historic acceptance/drift",
            "Real student authenticated persisted Practice, tenant isolation, correct/wrong/malformed tests on release candidate SHA",
            "Full academic 20/20 P5; then merge + verified Render SHA + owner live UAT"
        ],
        "lessons":rows
    }
def main():
    parser=argparse.ArgumentParser()
    parser.add_argument("--write",action="store_true")
    parser.add_argument("--check",action="store_true")
    args=parser.parse_args()
    doc=build()
    txt=json.dumps(doc,indent=2,ensure_ascii=False)+"\n"
    dest=folder/"US-CCSS-B001-20-P3-ACADEMIC-GAP-LEDGER.json"
    if args.write:dest.write_text(txt,encoding="utf8")
    if args.check:assert dest.read_text(encoding="utf8")==txt
    print(json.dumps({"count":len(doc["lessons"]),"historical":15,"mergedEarlier":3,
        "candidate":2,"academicAccepted":0,"decision":"NO_GO"}))
if __name__=="__main__":main()
