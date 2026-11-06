from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
coverage=base/"u01-l01-element-coverage.json"
ledger=json.loads(coverage.read_text(encoding="utf-8"))
row=next(x for x in ledger["Elements"] if x["SourceElementId"]=="G12-U01-L01-EXERCISE052")
assert row["Status"]=="SOURCE_IDENTIFIED"
row["Status"]="SOURCE_VERIFIED"
row["ExpectedMathematics"]="Source Section 1.1 exercise 52: (a) read f(-1) approximately 1; (b) graphically estimate input at level y=3, stating the printed curve only approaches this height at the right edge, so no exact algebraic x-value can be asserted from the source image."
row["SourceMathReadiness"]="ORIGINAL_GRAPH_INTERPRETATION_VERIFIED_WITH_EXPLICIT_PRECISION_LIMIT"
row["SourceMathPartB"]="Graph drawn near y=3 at the rightmost visible x (~6), but the printed window does not establish an exact crossing. Independent y=sqrt(x+2) teaching graph has x=7 when y=3, not a proven exact original-source solution."
row["OriginalExactRootCertified"]=False
row["OriginalGraphVisualInspected"]=True
row["OriginalGraphApproximateReadValidated"]=True
row["SourceMathAllSubpartsVerified"]=False
row["Reviewer"]="VISUAL_REVIEW_ORIGINAL_LOCAL_PDF_PAGE_36_AND_PRECISION_BOUNDARY"
row["UpdatedUtc"]=datetime.now(timezone.utc).isoformat()
assert sum(x["Status"]=="SOURCE_VERIFIED" for x in ledger["Elements"])==len(ledger["Elements"])==170
ledger["OriginalSourceAnchorsSourceVerified"]=170
ledger["OriginalSourceAnchorsStillIdentified"]=0
ledger["OriginalSourceGraph52PrecisionLimitPreserved"]=True
ledger["SourceAtomicDenominatorVerified"]=False
coverage.write_text(json.dumps(ledger,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

routing=base/"u01-l01-exercise-routing.json"
r=json.loads(routing.read_text(encoding="utf-8"))
ex=r["SourceExerciseContracts"][51]
assert ex["OriginalExerciseNumber"]==52
ex["SourceMathVerificationStatus"]="GRAPH_SOURCE_VISUALLY_REVIEWED_PRECISION_LIMIT_ANNOTATED"
ex["OriginalMathematicsSourceReview"]=row["ExpectedMathematics"]
ex["OriginalSourcePrecisionLimit"]="Exact original x when f(x)=3 cannot be read from bounded source image"
ex["FirstPartyTeachingAnswer"]="y=sqrt(x+2) has f(-1)=1 and f(7)=3; not exact original-source claim"
ex["AllSourceSubpartsVerified"]=False
ex["SemanticParityStatus"]="FIRST_PARTY_GRAPH_ASSESSMENT_AUTHORED_SOURCE_EXACT_PARITY_OPEN"
r["OriginalSourceNamedExerciseAnchorsSourceReviewed"]=92
r["OriginalSourceExercise52SourceInterpretationReviewed"]=True
r["OriginalSourceExercise52ExactRootVerified"]=False
routing.write_text(json.dumps(r,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

summary=base/"u01-l01-a-b-c-source-coverage-audit.json"
a=json.loads(summary.read_text(encoding="utf-8"))
assert a["SourceTopLevelSourceVerified"]==169 and a["SourceTopLevelOnlyIdentified"]==1
a["SourceTopLevelSourceVerified"]=170
a["SourceTopLevelOnlyIdentified"]=0
a["OriginalSource52PartBUnresolved"]=True
a["SourceGraph52InterpretationVerified"]="ORIGINAL_GRAPH_READ_WITH_NO_DEMONSTRABLE_EXACT_INPUT"
a["SourceAtomicDenominatorVerified"]=False
a["FullSourceSemanticAndAtomicDenominatorVerified"]=False
a["ReadyForStageD"]=False
a["SourceCategoryReview"]["EXERCISE"]["SourceVerified"]=92
a["SourceCategoryReview"]["EXERCISE"]["IdentifiedNotComplete"]=0
summary.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

check=base/"u01-l01-abc-checkpoint.json"
c=json.loads(check.read_text(encoding="utf-8"))
c["OriginalSourceAnchorsSourceVerified"]=170
c["SourceTopLevelAnchorsSourceVerified"]=170
c["SourceTopLevelAnchorsOriginalGraph52Partial"]=1
c["SourceGraph52ExactInputStatus"]="ORIGINAL_BOUND_GRAPH_HAS_NO_PROVABLE_EXACT_ROOT; FIRST_PARTY_X7_VALID"
c["SourceGraph52SourceReadingComplete"]=True
c["ReadyForStageD"]=False
c["SourceAtomicDenominatorVerified"]=False
blocks=c["ReasonForNotMovingToLesson2"]
old="Exercise 52 source graph requires exact point verification; all 92 need verified Edulytics implementation, not just teacher rubric proposals"
new="Exercise 52 source precision limit documented and first-party x=7 teacher-scored separately; full 92-question source-semantic/assessment parity remains unverified, beyond teacher rubric proposals"
assert blocks.count(old)==1
c["ReasonForNotMovingToLesson2"]=[new if v==old else v for v in blocks]
check.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
ent=p["LessonEntries"][0]
ent["OriginalSourceAnchorsSourceVerified"]=170
ent["SourceTopLevelAnchorsSourceVerified"]=170
ent["SourceTopLevelAnchorsOriginalGraph52Partial"]=1
ent["SourceOriginalGraph52PrecisionLimitVerified"]=True
ent["ReadyForStageD"]=False
p["LastClosedLessonOrdinal"]=0
p["FirstIncompleteLessonOrdinal"]=1
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

memo=base/"u01-l01-graph52-original-pdf-source-precision-review.md"
memo.write_text("""# Original Section 1.1 Exercise 52: verified graph, unresolved exact root

Source: authorized local OpenStax Precalculus 2017 PDF, page 36.
Exercise 52(a) evaluates f(-1); 52(b) solves f(x)=3 from the graph.

Actual printed source graph: curve begins near (-2,0), rises concavely
down through approximately (-1,1), and approaches y~2.8-3.0 near the
right edge of the x-window (around x=6). There is no stated algebraic
formula or unambiguous marked exact intersection for y=3. An exact
numerical root cannot be proved from the finite image.

SOURCE_VERIFIED means the source was READ and its ambiguity documented,
not that the first-party deliverable was matched. The original source
52(b) exact numerical answer is still expressly unknown.

The separate first-party assessment graph is y=sqrt(x+2), whose
f(-1)=1 and f(x)=3 at x=7 are exact. Teacher guidance now
accepts x=7 for the first-party task only; it never attributes that
exact solution to the original.

This prevents redundant source rereads; it does NOT certify original
figure parity, the complete lesson source-atomic denominator or
Stage A-B-C closure. No tests/CI/Stage D run.
""",encoding="utf-8",newline="\n")
print("SOURCE_170_PARENT_ANCHORS_REVIEWED; GRAPH52_EXACT_ROOT_STILL_UNKNOWN; LESSON_OPEN")
