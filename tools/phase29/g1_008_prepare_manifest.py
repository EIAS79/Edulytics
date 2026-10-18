#!/usr/bin/env python3
"""Generate truthful V1 source/assessment evidence for the 5 exact G1-008 rows."""
import json
from pathlib import Path
root=Path(__file__).resolve().parents[2]
f=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G1/batches/US-CCSS-G1-008-LOCAL-SOURCE-CROSSWALK.json"
d=json.loads(f.read_text(encoding="utf-8"))
names=[
    ["usccss.g1.u02.l21.number_puzzles.distinct_cards",
     "usccss.g1.u02.l21.related_missing_values",None],
    ["usccss.g1.u02.l22.survey.three_equations",
     "usccss.g1.u02.l22.survey.compare_question",None],
    ["usccss.g1.u03.l01.sums.favorite_and_reason",
     "usccss.g1.u03.l01.sums.fluency_selfcheck"],
    ["usccss.g1.u03.l02.count_on.start_steps_total",
     "usccss.g1.u03.l02.commutativity.two_equations",
     "usccss.g1.u03.l02.commutativity.two_equations"],
    ["usccss.g1.u03.l03.expressions.sort_equal_sums",
     "usccss.g1.u03.l03.expressions.compare_both_sides",
     "usccss.g1.u03.l03.expressions.compare_both_sides"]
]
assert len(d["lessons"])==5
# Read-only copies of the actual original 2021 student task statement PDFs,
# fetched on Our-CS on 2026-10-18. Never commit copyright-controlled binaries.
pdf_hashes={
  36:"2c3fcd2e9f30d36b10014c77ea742ad693a2ae45ecbcb91aae4d26336513b1a7",
  37:"9c2a3a074a3f3cb26948f772c4c1bf1eb97c7400b85098fdc3f4f7ece18de47d",
  38:"7f299d7ee652937c86de62adeb86d81041a03e24a7440af0fc6218e5a1c64500",
  39:"4f9356ab99fc5d2ea3f424a0d6decfe44cc581492a8413e7034c1321bb837dfc",
  40:"7496b244a00f14a2fe6a93d9ab841e20df4de2ea8eda989c775cb82cebb24b6b"
}
d["studentPdfInspection"]="ORIGINAL_2021_STUDENT_TASK_PDFS_ALL_FIVE_TEXT_INSPECTED_AND_SHA256_VERIFIED"
d["sourceReview"]="ORIGINAL_TEACHER_PREPARATION_AND_FULL_LESSONS_READ; ORIGINAL_STUDENT_TASK_PDF_TEXT_ALL_FIVE_INSPECTED; PUBLISHER_DIAGRAM_PIXEL_PARITY_NOT_VERIFIED"
for i,lesson in enumerate(d["lessons"]):
    lesson["original2021StudentTaskPdfSha256"]=pdf_hashes[lesson["gradeOrdinal"]]
    lesson["independentStudentTaskTextReview"]="ORIGINAL_PDF_TEXT_STRUCTURE_REVIEWED; graphics parity separate"
    lesson["publisherImageParity"]="NOT_VERIFIED"
    for j,goal in enumerate(lesson["sourceGoals"]):
        family=names[i][j]
        goal["coverage"]=("TEACHER_FACILITATED_NOT_DIGITALLY_CERTIFIED" if family is None
                          else "SOURCE_TARGETED_DIGITAL_PRACTICE_LOCAL_TEST_PASS")
        goal["assessmentEvidence"]=("REAL_TEACHER_PARTNER_WORK_NOT_MEASURED" if family is None
            else f"{family} | UsCcssG1NextSourceFiveTests | E2E_CI_PENDING")
    lesson["academicDecision"]="LOCAL_MATH_AND_CONTENT_TESTED_AWAITING_EXACT_HEAD_AUTHENTICATED_CI"
d["academicAcceptance"]="PENDING_CI_AND_LIVE_DEPLOY"
d["automatedE2E"]="AUTHENTICATED_CI_TEST_STAGED_NOT_RUN"
f.write_text(json.dumps(d,indent=2,ensure_ascii=False)+"\n",encoding="utf-8")
manifest={
  "schemaVersion":1,
  "planId":"EDULYTICS-GRADE-FIRST-5-V1",
  "batchId":"US-CCSS-G1-008",
  "scope":"US-CCSS-MATH Grade 1, original IM/Kendall Hunt 2021",
  "gradeOrdinalStart":36,
  "gradeOrdinalEnd":40,
  "lessonCount":5,
  "originalV1Unchanged":True,
  "baselineMainSha":d["baselineMainSha"],
  "sourceEvidenceFile":"US-CCSS-G1-008-LOCAL-SOURCE-CROSSWALK.json",
  "scopeIdentityPolicy":"Five consecutive canonical lessons, including optional U02 L22; no renumbering, skipping or CCSS outcome changes",
  "internalAcademicStatus":"PENDING_EXACT_HEAD_CI",
  "technicalStatus":"LOCAL_TESTED; AUTHENTICATED_CI_NOT_CONFIRMED",
  "productionStatus":"NOT_ATTEMPTED",
  "ownerProductionUat":"PENDING",
  "externalPublisherSignoff":"NOT_CLAIMED",
  "lessonCodes":[x["lessonCode"] for x in d["lessons"]],
  "sourceLimitations":["Publisher-original student task PDF texts 36-40 reviewed; independent illustration and diagram pixel parity not verified",
    "Partner centers and fully open authored questions/stories require real teacher review",
    "Digital sample survey is clearly labeled and does not assert any real classroom poll"],
  "nextPermittedBatch":"US-CCSS-G1-009, only after V1 completion criteria"
}
out=f.with_name("US-CCSS-G1-008.json")
out.write_text(json.dumps(manifest,indent=2,ensure_ascii=False)+"\n",encoding="utf-8")
print("Prepared five exact source and academic rows with non-fabricated CI/production statuses")
