#!/usr/bin/env python3
"""Attach genuine authored G12 U01 L01 work; never promote unverified source atoms."""
from pathlib import Path
import datetime,json
ROOT=Path(__file__).resolve().parents[2]
BASE=ROOT/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
SOURCE="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-element-coverage.json"
RICH="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l01.rich-lesson-v2.json"
ASSESS="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-assessment.json"
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
load=lambda p:json.loads(p.read_text(encoding="utf8"))
save=lambda p,o:p.write_text(json.dumps(o,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
m=load(BASE/"u01-l01-element-coverage.json")
rich=load(ROOT/RICH)
assess=load(ROOT/ASSESS)
lesson=rich["Lessons"][0]
assert lesson["LessonCode"]==m["LessonCode"]
assert len(lesson["WorkedExamples"])==15
assert len(lesson["KeyConcepts"])>=5
assert all(len(e["Steps"])>=3 for e in lesson["WorkedExamples"])
assert assess["LessonCode"]==lesson["LessonCode"]
mapped=[]
for item in m["Elements"]:
    kind=item["Category"]
    if kind=="EXAMPLE":
        number=int(item["SourceElementId"].rsplit("EXAMPLE",1)[1])
        assert 1<=number<=len(lesson["WorkedExamples"])
        item["EdulyticsArtifactPath"]=RICH
        item["ImplementationType"]="DIGITAL_LESSON_EXAMPLE_AUTHORED_NOT_YET_VERIFIED"
        item["VerificationMethod"]="FIRST_PARTY_MATHEMATICS_AUTHORED_SEMANTIC_PARITY_TO_SOURCE_NOT_YET_ATOMICALLY_APPROVED"
        item["ExpectedMathematics"]=lesson["WorkedExamples"][number-1]["Title"]
        mapped.append(item["SourceElementId"])
    if kind=="OBJ":
        item["EdulyticsArtifactPath"]=RICH
        item["ImplementationType"]="RICHV2_OBJECTIVE_STATED_NOT_1_TO_1_VERIFIED"
    if kind=="HOWTO":
        item["EdulyticsArtifactPath"]=RICH
        item["ImplementationType"]="TEACHING_PROCEDURE_DRAFT_ONLY"
    item["Status"]="SOURCE_IDENTIFIED"
m["AuthoringEvidence"]={
  "RichV2":RICH,"Assessment":ASSESS,
  "WrittenDistinctWorkedExamples":15,
  "FirstPartyCoolDown":True,
  "ReusableExistingFamilyCandidates":4,
  "RuntimeLessonInBlueprint":False,
  "SourceSemanticMatchesCertified":0,
  "OriginalFiguresReproducedOrSemanticallyRendered":0,
  "Original92ExercisesIndividuallyVerified":0,
  "StageDTests":"WRITTEN_NOT_RUN"}
m["CandidateExampleLinks"]=mapped
m["MatchedDigitally"]=0
m["MatchedTeacherLed"]=0
m["Status"]="A_C_EXPLICIT_GAP"
m["AtomicSourceDenominatorVerified"]=False
m["StillOpen"]=[
 "All figure descriptions, axis scales, alternative media and rights require visual review",
 "All 92 exercises including subparts and graph tasks require item-level matching",
 "Chapter Review, Review Exercises, Practice Test and answer-appendix links are not yet crosswalked",
 "G12 independent course Blueprint/SchoolAdoption/lesson Practice selection is not yet implemented",
 "Teacher review endpoint and real Grade12 question-family readiness not yet implemented",
 "Source-specific CCSS mapping is still pending"
]
m["UpdatedUtc"]=now
save(BASE/"u01-l01-element-coverage.json",m)
p=load(BASE/"full-grade-progress.json")
row=p["LessonEntries"][0]
assert row["Ordinal"]==1 and row["ACStatus"]=="A_C_IN_PROGRESS"
row.update({
 "ACStatus":"A_C_EXPLICIT_GAP",
 "BContentStatus":"15_SOURCE_SCOPED_MATH_EXAMPLES_AUTHORED_NOT_RUNTIME_VERIFIED",
 "CStatus":"FIRST_PARTY_COOLDOWN_AND_REUSE_CANDIDATES_WRITTEN_NOT_ROUTED",
 "SourceElementCountRecorded":m["Count"],
 "SourceExamplesAuthored":15,
 "SourceElementsIndependentlyMatched":0,
 "EvidencePath":SOURCE,
 "RichV2Path":RICH,
 "AssessmentPath":ASSESS,
 "BlockingReasons":m["StillOpen"],
 "ReadyForStageD":False,
 "Tests":"WRITTEN_NOT_RUN",
 "ClosedAtUtc":None,
 "UpdatedUtc":now})
p.update({"FirstIncompleteLessonOrdinal":1,"ActiveLessonOrdinal":1,
          "LastClosedLessonOrdinal":0,"ReadyForStageD":0,
          "GradeStatus":"GRADE12_A_C_INCOMPLETE","UpdatedUtc":now})
save(BASE/"full-grade-progress.json",p)
save(BASE/"u01-l01-abc-checkpoint.json",{
 "LessonCode":lesson["LessonCode"],"Ordinal":1,"Status":"A_C_EXPLICIT_GAP",
 "StageA":"SOURCE_LOCATED_169_ATOMIC_CANDIDATE_MARKERS_DENOMINATOR_INCOMPLETE",
 "StageB":"15_ORIGINAL_SKILL_SCOPED_EDULYTICS_WORKED_EXAMPLES_DRAFT_RICHV2",
 "StageC":"INDEPENDENT_COOLDOWN_TEACHER_RUBRIC_AND_FOUR_REUSE_FAMILIES_PROPOSED",
 "SourceSnapshotSha256":p["SourceSnapshotSha256"],"BranchHeadAtStart":"ea009d7054fa91027ac9ba78a1df2986e8f8a2c2",
 "SourceEvidencePath":SOURCE,"RichV2Path":RICH,"AssessmentPath":ASSESS,
 "OriginalSourcePdfPages":[18,37],
 "SourceCountCandidateElements":m["Count"],
 "SourceVerifiedEquivalentElements":0,
 "DTests":"WRITTEN_NOT_RUN","CI":"NOT_RUN","StageD":"NOT_STARTED",
 "ReadyForStageD":False,"ClosedAtUtc":None,
 "ReasonForNotMovingToLesson2":m["StillOpen"],"UpdatedUtc":now})
# Remove editorial provenance status from learner-facing mathematical text.
r=load(ROOT/RICH)
old=("The original source's 92 numbered exercises and figure-dependent prompts remain "
     "in the source crosswalk and require separate per-item verification; they are not "
     "declared digitally delivered by this explanatory page.")
new=("Independent exercises, including table, algebra, graph, and applied tasks, "
     "must be discussed using their own models, units and stated input domains. "
     "A teacher checks graph drawings and written explanations.")
n=0
for i,para in enumerate(r["Lessons"][0]["ExplanationParagraphs"]):
    if old in para:
        r["Lessons"][0]["ExplanationParagraphs"][i]=para.replace(old,new)
        n+=1
assert n==1
save(ROOT/RICH,r)
print("G12_U01_L01_STAGE_A_B_C_WRITTEN_NOT_CLOSED",m["Count"],"SOURCE_ANCHORS",len(mapped),"WORKED_EXAMPLES","READY",p["ReadyForStageD"])
