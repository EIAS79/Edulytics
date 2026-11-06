#!/usr/bin/env python3
"""Strict source catalogue & checkpoint synchronization for Grade12 A-C.
This is source-location work. No stage D or source semantic equality asserted.
"""
from pathlib import Path
import datetime,hashlib,json
root=Path(__file__).resolve().parents[2]
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def load(name):return json.loads((base/name).read_text(encoding="utf8"))
def save(name,o):(base/name).write_text(json.dumps(o,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
inv=load("g12-openstax-local-source-inventory-20261106.json")
progress=load("full-grade-progress.json")
first=load("u01-l01-element-coverage.json")
assessment=load("u01-l01-assessment.json")
resource=load("u01-shared-review-and-answer-resources.json")
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
assert len(inv["sectionRows"])==73 and inv["indexedSections"]==73
assert inv["sourceSha256"]==progress["SourceSnapshotSha256"]
assert sum(inv["unitCounts"])==73
assert len(first["Elements"])==169
sources=[
 {"ordinal":row["ordinal"],
  "lessonCode":row["lessonCode"],"unit":row["unitNumber"],
  "unitTitlePrinted":inv["unitTitles"][row["unitNumber"]-1],
  "sectionNumber":row["sourceSection"],
  "titlePrinted":row["sourceTitle"],
  "pdfPages":[row["pdfPageStart"],row["pdfPageEndInclusive"]],
  "sourceTextHashes":[x["extractedTextSha256"] for x in row["pageTextHashes"]],
  "academicStatus":("A_C_EXPLICIT_GAP" if row["ordinal"]==1 else "A_C_PENDING"),
  "CCSSStandards":"UNMAPPED; DO_NOT_INFER_BY_TOPIC_ONLY",
  "lessonBlueprintRuntime":"NOT_YET_CREATED",
  "lessonPracticeRuntime":"NOT_YET_VERIFIED",
  "sourceDenominator":"INCOMPLETE",
 } for row in inv["sectionRows"]]
save("g12-precalculus-blueprint-source-candidate.json",{
 "Status":"SOURCE_IDENTITY_MANIFEST_ONLY_NOT_VALIDATED_RUNTIME_BLUEPRINT",
 "CourseCode":"PRECALC","PackCode":"US-CCSS-MATH",
 "LessonCodePrefix":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:",
 "PathwayCandidate":"Advanced Precalculus",
 "LogicalLevelCandidate":13,
 "ExpectedSections":73,"ExpectedUnits":12,
 "UnitSectionCounts":inv["unitCounts"],"SourcePdfSha256":inv["sourceSha256"],
 "DoNotInheritPathways":["Traditional Algebra2","Fourth Courses","UAE Grade12"],
 "OfficialCCSSMapping":"CANDIDATE_ONLY_PENDING_SOURCE_SPECIFIC_APPROVAL",
 "SourceTitleConflict":{
  "Location":"Chapter 4",
  "BookmarkString":"Exponential and Rational Functions",
  "CorrectPrintedContentsString":"Exponential and Logarithmic Functions",
  "EvidencePdfPages":[7,10,344],
  "Resolution":"Use printed book contents and page header; ignore inconsistent PDF bookmark title",
  "Status":"RESOLVED_PRINTED_PRIMARY_SOURCE"
 },
 "Entries":sources,"CreatedUtc":now})
source=load("source-of-truth.json")
source["Chapter4PdfBookmarkConflict"]="Incorrect PDF bookmark says Exponential and Rational Functions; printed book PDF pp.7,10,344 says Exponential and Logarithmic Functions. Use printed heading."
source["CourseRuntimeStatus"]="NOT_YET_AVAILABLE; INDEPENDENT PRECALCULUS COURSE BLUEPRINT REQUIRED"
source["SourceLocatorManifest"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/g12-precalculus-blueprint-source-candidate.json"
save("source-of-truth.json",source)
checked=[int(x["SourceElementId"].rsplit("EXERCISE",1)[1]) for x in first["Elements"] if x["Category"]=="EXERCISE" and x["Status"]=="SOURCE_VERIFIED"]
assert len(checked)==len(set(checked))==91 and set(range(1,93))-set(checked)=={52}
graph=[x["SourceElementId"] for x in first["Elements"] if x["Category"]=="FIG" and x.get("SourceVisualInspected")]
assert len(graph)==4
checkpoint=load("u01-l01-abc-checkpoint.json")
checkpoint.update({
 "SourceMathReviewedOriginalExerciseCount":91,
 "OriginalSourceExerciseTotalNumbers":92,
 "OriginalExerciseNumbersStillPartiallyReviewed":[52],
 "SourceFiguresVisuallyInspectedIds":graph,
 "SourceFiguresStillNeedAccessibleRenderAndRightsReview":True,
 "SharedUnitChapterResourcesIdentified":{
  "ReviewExercises":{"PdfPages":[134,138],"CandidateNumberCount":80},
  "ChapterPracticeTest":{"PdfPages":[139,140],"CandidateNumberCount":35},
  "OriginalTryItAnswerStartPdfPage":1097,
  "OriginalOddAnswerStartPdfPage":1107
 },
 "UnitSharedResourcesFile":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-shared-review-and-answer-resources.json",
 "SourceExercisesMatchedInEdulytics":0,
 "SourceMatchedPercent":"NOT_CALCULABLE_DENOMINATOR_INCOMPLETE",
 "Tests":"WRITTEN_NOT_RUN","CI":"NOT_RUN",
 "ReadyForStageD":False,"ClosedAtUtc":None,"UpdatedUtc":now})
save("u01-l01-abc-checkpoint.json",checkpoint)
progress["ActiveLessonOrdinal"]=1
progress["ReadyForStageD"]=0
progress["LastClosedLessonOrdinal"]=0
progress["FirstIncompleteLessonOrdinal"]=1
progress["LessonEntries"][0]["ACStatus"]="A_C_EXPLICIT_GAP"
progress["LessonEntries"][0]["SourceExercisesIndividuallyInterpreted"]=91
progress["LessonEntries"][0]["ChapterResourceStatus"]="LOCATED_80_REVIEW_EXERCISES_35_PRACTICE_TEST_NOT_IMPLEMENTED"
progress["LessonEntries"][0]["SourceMathReviewIsNotMatching"]=True
progress["LessonEntries"][0]["ReadyForStageD"]=False
progress["UpdatedUtc"]=now
save("full-grade-progress.json",progress)
handoff=base/"grade12-a-c-handoff.md"
s=handoff.read_text(encoding="utf8")
s=s.replace("92 numbered Exercises and other source anchors.",
  "92 numbered Exercises and other source anchors; 91/92 numbered source exercises now mathematically inspected (52 still partly ambiguous as a plotted x-reading).")
s=s.replace("- Grade12 test file written", "- Source Chapter 1 shared resources found: 80 Chapter Review Exercises on PDF pp.134–138; 35 Chapter Practice Test items on PDF pp.139–140; source answer appendices begin at PDF pp.1097 and 1107. These are **not** digitally delivered or source-matched.\\n- Grade12 source candidate identifies all 73 lesson codes; Chapter 4 printed title overrides an erroneous PDF bookmark. This manifest is **not** a runtime Blueprint.\\n- Grade12 test file written")
s=s.replace(" 92 exercises and subparts,"," exercise 52 graph uncertainty and other exercise subparts,")
handoff.write_text(s,encoding="utf8")
print("G12_SOURCE_CATALOGUE_73_ENTRIES_CHECKPOINT_91_EXERCISES_INSPECTED_NO_GRADE12_READY")
