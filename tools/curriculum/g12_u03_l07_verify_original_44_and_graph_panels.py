from pathlib import Path
import json,datetime
from fractions import Fraction
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
p=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l07-source-inventory-and-gap-audit.json"
a=json.loads(p.read_text(encoding="utf-8"))
assert a["OriginalExerciseSourceMathReviewCount"]==79
d={item["Number"]:item for item in a["SourceNumberedExerciseLedger"]}
entry=d[44]
entry["SourceVisualPdfPage"]=311
entry["SourceTranscriptionRiskResolved"]=True
entry.pop("SourceTranscriptionRisk",None)
entry["SourceMathReview"]="REVIEWED_WITH_TEACHER_EXPECTED_MATH_FROM_VISUAL_ORIGINAL"
entry["SourceTeacherExpectedMath"]="Original PDF visually confirms denominator 3x²−14x+15=(3x−5)(x−3). Numerator 2x²+7x−15=(2x−3)(x+5). Thus x-intercepts (3/2,0), (−5,0); y-intercept (0,−1); vertical asymptotes x=5/3 and x=3; horizontal asymptote y=2/3; no common factors."
entry["TeacherAnswerVerified"]=True
entry["FirstPartyIndependentMathQuestionVerified"]=False
assert Fraction(-15,15)==-1
def pval(c,x):return sum(Fraction(v)*Fraction(x)**i for i,v in enumerate(c))
for x in [Fraction(3,2),Fraction(-5,1)]:assert pval([-15,7,2],x)==0
for x in [Fraction(5,3),Fraction(3,1)]:assert pval([15,-14,3],x)==0
# The original graph panels were inspected visually in the authorized local PDF.
# These observations capture branch behavior, not exact graph-to-formula matching.
graphs=[
(57,311,"Graph displays multiple rational branches and red dashed vertical asymptotes; graph-to-formula requires checking marked integer coordinates and intercept values."),
(58,311,"Two vertical asymptotes separate three branches. Graph crosses the horizontal axis on the middle branch. Exact scale factor remains to establish."),
(59,311,"Two red dashed vertical asymptotes. Outer branches rise and central branch crosses the horizontal axis. Teacher must infer factors from visible features."),
(60,312,"Two vertical asymptotes frame a decreasing central branch; graph has an x-axis crossing and negative outer branches. Exact factor multiplicities require source coordinate verification."),
(61,312,"One vertical asymptote, increasing left and right branches with opposite divergence; graph crosses x-axis in the left branch and in the right branch."),
(62,312,"One vertical asymptote; left branch falls toward minus infinity and right branch descends from positive infinity, crossing x-axis at positive input."),
(63,312,"Two vertical asymptotes; middle branch is below x-axis except at a tangent contact, indicating an even-multiplicity zero; outer branches separated."),
(64,312,"Two vertical asymptotes; middle branch touches x-axis at a local minimum and rises on both sides; compare denominator sign and repeated zero.")
]
for n,page,meaning in graphs:
 assert n in range(57,65)
 d[n]["OriginalPdfPageVisuallyInspected"]=page
 d[n]["SourceGraphSemanticObservation"]=meaning
 d[n]["SourceMathReview"]="SOURCE_GRAPH_VISUALLY_INSPECTED_EXACT_RATIONAL_MODEL_NOT_YET_VERIFIED"
 d[n]["TeacherAnswerVerified"]=False
 d[n]["FirstPartyIndependentMathQuestionVerified"]=False
a["SourceOriginalGraphPanelsVisuallyInspected"]=[n for n,_,_ in graphs]
a["OriginalSourcePdfPagesVisuallyReviewedForExercises"]=[311,312]
a["OriginalExerciseSourceMathReviewNumbers"]=list(range(1,57))+list(range(65,89))
a["OriginalExerciseSourceMathReviewCount"]=80
a["SourceExercise44TranscriptionBlocker"]="RESOLVED_FROM_AUTHORIZED_LOCAL_PDF_VISUAL"
a["SourceGraphDependentExerciseNumbersStillUnreviewed"]=list(range(57,65))
a["SourceExerciseGraphSemanticsInspectedWithoutExactTeacherKeys"]=8
a["BlockingReasons"]=["EXERCISE_GRAPHS_57_TO_64_NEED_COMPLETE_EXACT_MATH_MODEL_TEACHER_KEYS","ALL_88_FIRST_PARTY_EXERCISE_ALTERNATIVES_AND_TEACHER_RUBRICS_NOT_AUTHORED","23_SOURCE_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_STUDENT_ASSESSMENT_ROUTE_AND_REUSE_CONTRACT_PENDING"]
a["ReadyForStageD"]=False
a["UpdatedAtUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
p.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U03_L07_80_SOURCE_MATH_REVIEWED_EIGHT_GRAPHS_VISUALLY_INSPECTED_NOT_CLOSED")
