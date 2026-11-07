from pathlib import Path
from collections import Counter
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
path=base/"u01-l01-exercise-routing.json"
data=json.loads(path.read_text(encoding="utf-8"))
contracts=data["SourceExerciseContracts"]
assert len(contracts)==92
def route(n):
    if 1<=n<=5:
        return (None,"TEACHER_LED_PROOF","Conceptual explanation and counterexamples require human assessment.")
    if 6<=n<=7:
        return ("functions.relations.unique_output.classify","EXACT_FINITE_RELATION_SKILL_ANALOGUE","Existing relation classification family covers unique-input/output semantics; student original-numbered task remains teacher-reviewed.")
    if 8<=n<=26:
        return ("functions.relations.implicit.classify","PARTIAL_FIXED_VARIANTS_PLUS_TEACHER","Existing 5 implicit-equation variants cover function/nonfunction branching; original 19 arbitrary source equations still require teacher checking.")
    if 27<=n<=31:
        return ("functions.notation.evaluate_context","PARTIAL_NUMERIC_PLUS_TEACHER_SYMBOLIC","Existing notation evaluation covers numeric substitutions, not every original five-part symbolic expression.")
    if 32<=n<=33:
        return (None,"TEACHER_LED_SYMBOLIC_DIFFERENCE_QUOTIENT","Exact symbolic simplification and nonzero denominator are checked by teacher; no unverified symbolic auto-scorer claimed.")
    if 34<=n<=39:
        return ("functions.quadratic.output_roots.solve" if n in (36,37) else "functions.notation.evaluate_context","PARTIAL_EXACT_SKILL_PLUS_TEACHER_MULTIPART","Existing numeric or quadratic-root families address selected skill only; all original evaluation/solving subparts are reviewed separately.")
    if 40<=n<=54:
        return (None,"TEACHER_LED_SOURCE_GRAPH","First-party accessible graph is rendered; source-specific graph reading and vertical-line classification have teacher review, not an automatic image grader.")
    if 55<=n<=59:
        return ("functions.relations.one_to_one.classify","PARTIAL_FINITE_RELATION_PLUS_TEACHER_GRAPH","Reusable injectivity family tests finite mappings; graph horizontal-line classification remains teacher-scored.")
    if 60<=n<=65:
        return ("functions.relations.unique_output.classify","EXACT_RELATION_CLASSIFICATION_SKILL_ANALOGUE","Reusable finite relation classification covers repeated-input functionhood; student original structure remains teacher-reviewed.")
    if 66<=n<=67:
        return ("functions.table.lookup.invert","EXACT_TABLE_LOOKUP_SKILL_ANALOGUE","Existing exact table evaluator/inverter covers output and inverse-input lookup.")
    if 68<=n<=73:
        return ("functions.notation.evaluate_context","PARTIAL_NUMERIC_PLUS_TEACHER_FIVE_INPUTS","Existing function evaluation covers a numeric substitution skill, but does not independently grade all five original table inputs and domain/radical answers.")
    if 74<=n<=75:
        return ("functions.notation.evaluate_context","PARTIAL_VALUE_ARITHMETIC_PLUS_TEACHER","A numeric evaluation family is reusable, but complete multi-function arithmetic and substitution are teacher-reviewed.")
    if 76<=n<=87:
        return (None,"TEACHER_LED_GRAPH_WINDOW_REQUIRED","Original graph and range require physical window endpoints; first-party bounded graph is rendered, and coordinate evidence is mandatory for teacher assessment.")
    if 88<=n<=91:
        return (None,"TEACHER_LED_CONTEXT_AND_UNITS","Unit-sensitive real context, notation and interpretation are teacher-scored; no fabricated numeric auto-grader.")
    if n==92:
        return ("functions.relations.one_to_one.classify","PARTIAL_INJECTIVITY_PLUS_TEACHER_PROOF","Finite relation injector covers an analogous one-to-one skill; original quadratic counterexample proof remains teacher reviewed.")
    raise ValueError(n)

for i,entry in enumerate(contracts,1):
    assert entry["OriginalExerciseNumber"]==i
    family, mode, reason=route(i)
    entry["ProposedExistingQuestionFamily"]=family
    entry["StageCQuestionFamilyCapability"]=family
    entry["StageCOriginalNumberedExerciseDelivery"]="FIRST_PARTY_STUDENT_PROMPT_AND_TENANT_SCOPED_TEACHER_REVIEW_AUTHORED"
    entry["StageCAssessmentMode"]=mode
    entry["StageCQuestionFamilyReuseRationale"]=reason
    entry["StageCFullOriginalQuestionAutomaticallyGraded"]=False
    entry["StageCOriginalSourceDiagramCopied"]=False
    entry["StageCAllSourceSubpartsTeacherReviewChecklistAuthored"]=True
    entry["StageCCapabilityReadiness"]="WRITTEN_NOT_RUN"
    entry["CapabilityStatus"]="FAMILY_ANALOGUE_OR_TEACHER_ASSESSMENT_CONTRACT_AUTHORED_NOT_RUNTIME_VERIFIED"
    if i==52:
        entry["StageCOriginalSourceGraph52ExactRoot"]="UNKNOWN_FROM_PRINTED_SOURCE"
        entry["StageCFirstPartyGraph52Grading"]="ACCEPT_FIRST_PARTY_X7_WITH_EXPLICIT_SOURCE_BOUNDARY"
    if 76<=i<=87:
        entry["StageCGraphEvidenceMandatory"]=True
data["StageCOriginalExerciseContractCount"]=92
data["StageCSourceExerciseAssessmentContractCount"]=92
data["StageCOriginalExercisesAutomatedExact1To1Count"]=0
data["StageCTeacherReviewOriginalExerciseCount"]=92
data["StageCNumberedSourceExerciseSubpartsTeacherChecklistCount"]=162
data["StageCQuestionFamilyDesignStatus"]="EXISTING_EIGHT_FAMILIES_USED_WHERE_SEMANTICALLY_RELEVANT; NO_SOURCE_92_AUTO_GRADING_CLAIM"
data["StageCQuestionFamilyPlan"]=dict(Counter(c["StageCAssessmentMode"] for c in contracts))
data["StageCDeliverableStatus"]="AUTHORING_AND_TEACHER_REVIEW_CONTRACTS_WRITTEN_NOT_TESTED"
path.write_text(json.dumps(data,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
assessment=base/"u01-l01-assessment.json"
a=json.loads(assessment.read_text(encoding="utf-8"))
a["Practice"]["ExerciseRoutingStatus"]="ALL_92_SOURCE_SKILLS_MAPPED_TO_EIGHT_REUSED_OR_EXTENDED_ANALOGUE_FAMILIES_AND_TEACHER_REVIEW; RUNTIME_NOT_VERIFIED"
a["Practice"]["SourceNumberedExerciseAssessmentContracts"]=92
a["Practice"]["SourceExerciseTeacherChecklistLeaves"]=162
a["Practice"]["SourceExercisesFullyAutoGraded1To1"]=0
a["Practice"]["StageCQuestionFamilyPlan"]=data["StageCQuestionFamilyPlan"]
a["Practice"]["MissingFamiliesOrCapabilities"]=[
    "Original graphic constructs and drawings require authenticated Stage D review of the authored graph-coordinate teacher route.",
    "Full automatic scoring is not claimed for all 92 original-numbered source tasks; partial existing exact families and teacher-scoped rubrics are explicitly distinguished.",
    "Exact symbolically equivalent arbitrary function-solving and difference quotient automation is not implemented; the source skill is delivered in a human-scored task.",
    "Original source exercise 52(b) exact numerical root is not supplied by the printed image; teacher must not confuse the first-party x=7 answer with original source exactness."
]
a["Practice"]["PracticeReady"]="STAGE_C_AUTHORING_EIGHT_FAMILY_REUSE_AND_92_TEACHER_REVIEW_CONTRACTS_SOURCE_SEMANTIC_PARITY_OPEN"
assessment.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
for filename in ["u01-l01-abc-checkpoint.json","full-grade-progress.json"]:
    fp=base/filename
    j=json.loads(fp.read_text(encoding="utf-8"))
    row=j if filename.startswith("u01") else j["LessonEntries"][0]
    row["OriginalSourceExerciseStageCAssessmentContractsAuthored"]=92
    row["OriginalSourceExerciseStageCTeacherChecklistActions"]=162
    row["OriginalSourceExerciseExactAutoGrader1To1Count"]=0
    row["StageCEightFamilyReuseMappingStatus"]="EXPLICIT_CAPABILITY_CANDIDATE_NOT_RUNTIME_OR_PARITY_CERTIFIED"
    row["ReadyForStageD"]=False
    fp.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("STAGE_C_ROUTED_92_SOURCE_EXERCISES_TO_8_EXISTING_FAMILY_CANDIDATES_OR_TEACHER; NO_FALSE_AUTOGRADING")
