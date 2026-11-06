#!/usr/bin/env python3
"""Section 1.1 exercise-type evidence, chapter source resources. No grading claims."""
from pathlib import Path
import json,datetime
ROOT=Path(__file__).resolve().parents[2]
BASE=ROOT/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
load=lambda n:json.loads((BASE/n).read_text(encoding="utf8"))
save=lambda n,obj:(BASE/n).write_text(json.dumps(obj,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
s=load("u01-l01-element-coverage.json")
groups=[
 (1,5,"VERBAL","TEACHER_REVIEW_EXPLANATION_REQUIRED","Conceptual definition, justify relation/function/one-to-one"),
 (6,39,"ALGEBRAIC_TABLE_AND_RELATION","EXACT_ENGINE_CANDIDATE_WITH_DOMAIN_REVIEW","Input-output mappings, algebraic formulas and domain constraints"),
 (40,67,"GRAPH_AND_VERTICAL_LINE","GRAPHICAL_TEACHER_REVIEW_OR_ACCESSIBLE_GRAPH_ENGINE_REQUIRED","Diagram classification, graph interpretation, vertical and horizontal line tests"),
 (68,75,"FUNCTION_EVALUATION","EXISTING_FUNCTION_FAMILY_REUSE_CANDIDATE","Function evaluations, algebraic inputs and substitutions"),
 (76,87,"TECHNOLOGY_VIEWPORT","TEACHER_GRAPHING_TOOL_REVIEW_REQUIRED","Graphing window ranges and interpretation of scale"),
 (88,92,"APPLIED_AND_PROOF","TEACHER_REVIEW_OR_FIT_FOR_PURPOSE_DIGITAL_GRADER","Contextual function modeling and one-to-one proofs")
]
allseen=set()
for lo,hi,category,route,scope in groups:
 for n in range(lo,hi+1):
  assert n not in allseen
  allseen.add(n)
  item=next((x for x in s["Elements"] if x["SourceElementId"]==f"G12-U01-L01-EXERCISE{n:03d}"),None)
  assert item
  item["ExerciseCategory"]=category
  item["ProposedReviewRoute"]=route
  item["SourceMathematicsScope"]=scope
  item["DeliveryStatus"]="NOT_YET_IMPLEMENTED"
  item["SubpartsAndDiagramsFullyReconciled"]=False
assert allseen==set(range(1,93))
s["ExerciseGroupInventory"]=[
 {"From":lo,"To":hi,"Count":hi-lo+1,"Category":category,
  "RecommendedRoute":route,"SourceMathematicsScope":scope,
  "MatchingStatus":"UNMATCHED_SOURCE_IDENTIFIED"}
 for lo,hi,category,route,scope in groups]
s["All92MainExerciseNumbersLocated"]=True
s["All92ExerciseSubpartsSemanticallyMatched"]=False
s["All92GraphMediaVerified"]=False
s["UpdatedUtc"]=now
save("u01-l01-element-coverage.json",s)
resources={
 "Unit":1,"ChapterTitle":"Functions","Status":"CHAPTER_RESOURCE_LOCATORS_ONLY",
 "SourcePdfSha256":s["SourceSnapshotId"],
 "ChapterIntro":{"PdfPage":17,"Status":"SOURCE_LOCATED_NOT_IMPLEMENTED"},
 "ReviewAndKeyTerms":{"PdfPages":[129,133],"Status":"SOURCE_LOCATED_NOT_ATOMICALLY_MAPPED"},
 "ChapterReviewExercises":{
  "PdfPages":[134,138],"CandidateNumberRange":[1,80],
  "CandidateNumberCount":80,
  "Coverage":"SHARED_ACROSS_U01_L01_TO_L07; NOT AUTOMATICALLY_ONE_MORE_LESSON",
  "ElementDenominatorVerified":False,
  "SourceToLessonMappingsVerified":False},
 "ChapterPracticeTest":{
  "PdfPages":[139,140],"CandidateNumberRange":[1,35],
  "CandidateNumberCount":35,
  "Coverage":"CHAPTER_SHARED_ASSESSMENT; NOT_ADDED_TO_73_SECTION_COUNT",
  "ElementDenominatorVerified":False,
  "SourceToLessonMappingsVerified":False},
 "TryItAnswerAppendix":{
  "ChapterOnePdfPage":1097,
  "Section11AnswerStartPdfPage":1097,
  "Coverage":"SOURCE_LOCATOR_ONLY; DO NOT ASSUME COMPLETE EVEN_QUESTION_ANSWER_KEY"},
 "OddAnswerAppendix":{
  "ChapterOneStartPdfPage":1107,
  "ChapterOneEndPdfPage":1111,
  "Section11StartPdfPage":1107,
  "Coverage":"ODD_ANSWERS_NOT_FULL_SOLUTION_KEY"},
 "UnitOneSections":7,"SourceUnitElementsVerified":False,
 "ProblemRouteGrouping":"G12-U01-L01-EXERCISE001..092 classify per source section only; review/test groups are independently tracked.",
 "CreatedAtUtc":now
}
save("u01-shared-review-and-answer-resources.json",resources)
p=load("full-grade-progress.json")
row=p["LessonEntries"][0]
row["MainExerciseGroupBreakdown"]={x["Category"]:x["Count"] for x in s["ExerciseGroupInventory"]}
row["ChapterResourcesIdentifiedPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-shared-review-and-answer-resources.json"
row["UpdatedUtc"]=now
save("full-grade-progress.json",p)
print("G12_U01_L01_92_EXERCISES_CLASSIFIED_NOT_MATCHED",
      [(x["Category"],x["Count"]) for x in s["ExerciseGroupInventory"]],
      "CHAPTER_SHARED_REVIEW_80_PRACTICE_TEST_35_IDENTIFIED_ONLY")
