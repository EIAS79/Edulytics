"""One-time Grade 11 U01:L01 first-party source-aligned A–B–C repair. No tests executed."""
from pathlib import Path
import json, hashlib, datetime
ROOT=Path(__file__).resolve().parents[2]
SRC=Path(r"C:\Users\khali\Downloads\New folder (2)\Common Core")
TEACH=SRC/"11_HS_Algebra_2"/"1"/"1"
STUDENT=SRC/"11_HS_Algebra_2_Student"/"1"/"1"
CODE="PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L01"
NOW=datetime.datetime.now(datetime.timezone.utc).isoformat()
def dump(path,data):
 p=ROOT/path;p.parent.mkdir(parents=True,exist_ok=True)
 p.write_text(json.dumps(data,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
 return str(path).replace("\\","/")
paths={f"{role}/{name}":p/name for role,base in (("teacher",TEACH),("student",STUDENT)) for name in (("index.html","practice.html","preparation.html") if role=="teacher" else ("index.html","practice.html")) for p in [base]}
for k,p in paths.items():assert p.is_file(),k
digest={k:hashlib.sha256(p.read_bytes()).hexdigest() for k,p in paths.items()}
PACK=Path("src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json")
pack=json.loads((ROOT/PACK).read_text(encoding="utf8"))
lesson=next(x for x in pack["lessons"] if x["lessonCode"]==CODE)
tr=next(x for x in lesson["translations"] if x["cultureCode"]=="en")
tr.update({
 "explanation":"Learning goal: create sequences from a context and describe a precise rule for obtaining each term from earlier terms. A sequence is an ordered list: a first term and a rule can define later terms. In a three-peg disc-transfer puzzle, moving n discs optimally requires transferring n-1 smaller discs away, moving the largest disc once, and transferring the smaller stack back. Consequently M(1)=1 and M(n)=2M(n-1)+1. The number of moves depends on the number of discs, not on the physical size of the pegs.",
 "keyConceptsAndRules":"Sequence: ordered terms with positions. Recursive description: state the initial term and how the next term is calculated. With start 2 and next=2*previous-1, the first five terms are 2,3,5,9,17; another initial term gives a different valid sequence. Disc-transfer minimum: M(1)=1, M(n)=2M(n-1)+1, so M(n)=2^n-1 for positive integer n. For the optional two-colour checker exchange puzzle with n checkers of each colour, n(n+2) legal moves are needed under the stated forward and jump restrictions; justify a proposed rule rather than guessing from two cases.",
 "workedExamples":"Sequence example: starting with 4 and applying next=2*previous-1 generates 4,7,13,25,49. Recursive disc example: M(2)=3, M(3)=7, M(4)=15, M(5)=31, M(7)=127. Explanation: to move five discs, shift four aside (15), move the bottom disc (1), restore four (15): 31. Fractional recursion: starting at 1/2, repeatedly halve to obtain 1/4,1/8,1/16,1/32,1/64. Backward recursion: if a4=-7 and a(n+1)=-2a(n)+3 then a3=5, a2=-1, a1=2; next terms a5=17,a6=-31,a7=65.",
 "stepByStepSolutions":"1. Identify the term position and initial value. 2. Translate the verbal relationship into a recurrence, using parentheses correctly (2x-1 is not 1-2x). 3. Apply the recurrence term-by-term while retaining the order. 4. When a solution demands an explanation, justify why both sub-puzzles of size n-1 are necessary and sufficient. 5. Check the predicted sequence by hand for 1,2,3,4 discs; do not treat an untested pattern as proof.",
 "commonMistakes":"Interpreting 'one less than twice the previous term' as 1-2x rather than 2x-1; forgetting to include the initial term in the requested number of terms; using 2^n instead of 2^n-1 for the disc minimum; treating a physical strategy or checker drawing as automatically verified by a numeric response.",
 "quickSummary":"An ordered list with a specified first term and recursive rule defines a sequence. The three-peg disc puzzle yields 1,3,7,15,31,... and M(n)=2M(n-1)+1. Explain the move constraints before claiming a minimum. The optional checker task and open explanations are teacher-reviewed."
})
(RootPack:=ROOT/PACK).write_text(json.dumps(pack,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
rich={
 "LessonCode":CODE,"CultureCode":"en","Title":"A Towering Sequence",
 "ExplanationParagraphs":[
 "Goal: describe an ordered numerical sequence and use a recursive rule to generate new terms from an initial condition.",
 "Warm-up, 5 minutes. Write a sequence of five terms. For each term after the first use two times the previous term minus one. Choose a starting value and compare with a partner's different starting value; both may follow the same rule.",
 "Main task, 30 minutes. Use three pegs and different-sized coins or discs; move one top disc at a time and never place a larger one onto a smaller one. Find the fewest legal moves for 2, 3, and 4 discs, and explain how the solution with n-1 discs permits the n-disc solution.",
 "The minimum move count is a sequence in the positive integer input n: 1,3,7,15,31,... . To move n discs, temporarily move n-1, move the largest once, then move n-1 back. The recurrence is M(1)=1, M(n)=2M(n-1)+1; deriving M(n)=2^n-1 requires a mathematical justification, not extrapolation alone.",
 "Extension: predict the number of moves for 5 and 7 discs, and estimate the elapsed years for 64 discs at one second per move. Allow independent reasoning, then compare recurrence and explicit formulas.",
 "Optional alternate activity: swap two colours of checkers on a line with a centre gap. Only forward moves into an empty space or jumps over one opposing checker are legal. Collect the minimum counts for 1,2,3,4 checkers per colour, explain growth and predict the 7-per-colour count. A physical/manipulative task is teacher assessed.",
 "Teacher delivery: pairs need three marked peg positions and 2–5 different-sized discs, a timer and a table for disc count/moves. Do not substitute an auto-scored arithmetic quiz for observing legal physical moves; teacher verifies the move sequence, an explanation and a table.",
 "Practice: generate 10 Fibonacci-type terms from 0,1; continue 1/2,1/4,1/8 using an explicit halving rule; work backward and forward around the fourth term -7 under next=-2*previous+3; propose two distinct legitimate sequences starting 0,5.",
 "Cool-down, authored by Edulytics: a stack of six discs follows the same legal-move rules. Find its minimum move count, describe why the minimum for six discs is twice the minimum for five discs plus one, and identify the first four sequence terms. Teacher checks the justification; not claimed to reproduce a restricted original cool-down."
 ],
 "KeyConcepts":[
  {"Title":"Sequence","Definition":"An ordered list of terms indexed by positions.","Rule":"A starting term and a recurrence determine subsequent terms when the rule is well defined.","Example":"With a1=2 and a(n+1)=2a(n)-1, terms start 2,3,5,9,17."},
  {"Title":"Minimum legal moves","Definition":"Fewest permitted one-disc transfers between three pegs with larger discs never placed on smaller discs.","Rule":"M(1)=1 and M(n)=2M(n-1)+1 for n>1.","Example":"M(4)=2(7)+1=15."},
  {"Title":"Recursive versus explicit forms","Definition":"A recurrence uses earlier terms; an explicit expression uses the term index.","Rule":"For this puzzle, M(n)=2^n-1 for n>=1.","Example":"M(7)=127."}
 ],
 "WorkedExamples":[
  {"Title":"Different initial values","Question":"Start at 4; each next term is one less than twice the previous term. List five terms.","Method":"Apply a(n+1)=2a(n)-1 four times.","Steps":["a1=4.","a2=7; a3=13.","a4=25; a5=49."],"Answer":"4, 7, 13, 25, 49","Check":"All four transitions satisfy next=2*previous-1."},
  {"Title":"Minimum disc transfers","Question":"What is the minimum number of transfers for 5 discs?","Method":"Solve the four-disc subproblem twice and move the largest once.","Steps":["M(1)=1, M(2)=3.","M(3)=7, M(4)=15.","M(5)=2*15+1=31."],"Answer":"31 legal moves","Check":"The largest disc must move once; all smaller discs must move away and return."},
  {"Title":"Practice a recurrence in both directions","Question":"Given a(n+1)=-2a(n)+3 and a4=-7, find a1,a2,a3,a5.","Method":"Reverse by a(n)=(3-a(n+1))/2; then apply the forward rule.","Steps":["a3=(3-(-7))/2=5.","a2=(3-5)/2=-1; a1=(3-(-1))/2=2.","a5=-2*(-7)+3=17."],"Answer":"a1=2, a2=-1, a3=5, a5=17","Check":"2 -> -1 -> 5 -> -7 -> 17 obeys the rule."}
 ],
 "CommonMistakes":[
  {"Mistake":"Using 1-2x instead of 2x-1.","WhyWrong":"It reverses the subtraction order.","Correction":"Compute twice x first, then subtract one."},
  {"Mistake":"Counting 2^n moves rather than 2^n-1.","WhyWrong":"The recurrence starts at one move and adds one to twice the previous count.","Correction":"For n=3 verify the minimum is 7, not 8."},
  {"Mistake":"Assuming a numerical formula verifies legal puzzle play.","WhyWrong":"The actual move constraints and strategy are additional evidence.","Correction":"Teacher checks a move log, demonstration or justified construction."}
 ],
 "SummaryPoints":["A sequence has ordered terms; a recurrence specifies the next term using earlier terms.","Always state the initial term.","Three-peg disc minimum is 2^n-1 for n positive.","Manipulative play, checker jumping and reasoning remain teacher-reviewed."],
 "Visuals":[],"Videos":[]
}
RICH=Path("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u01-l01.rich-lesson-v2.json")
dump(RICH,{"SchemaVersion":2,"PackCode":"US-CCSS-MATH","ContentVersion":"g11-u01-l01-source-aligned-20261106","Lessons":[rich]})
ASSESS=Path("src/Edulytics.Core/Curriculum/LessonContent/Assessment/alg2-u01-l01-source-teacher-led-20261106.json")
assessment={"LessonCode":CODE,"Unit":1,"Lesson":1,"SourceTaskTypes":["recursive sequence generation","disc transfer demonstration","optional checker manipulation","verbal reasoning","cumulative independent practice"],"CompetencyContracts":[{"skill":"recursive-sequence-generation","answerTypes":["ordered-term-list","numeric"],"allowedQuestionFamilies":["recursive-rule-term-evaluation"],"forbiddenQuestionFamilies":["unrelated-arithmetic-fallback"],"grading":"deterministic exact recurrence if existing engine support is proved; else teacher review","capabilityReadiness":"UNVERIFIED_ENGINE_MAPPING"},{"skill":"minimum-disc-transfer-reasoning","answerTypes":["numeric","verbal-justification","legal-move-log"],"allowedQuestionFamilies":["recursive-count-if-engine-proved"],"forbiddenQuestionFamilies":["unrelated-multiple-choice"],"grading":"teacher verifies physical constraints and explanation","capabilityReadiness":"TEACHER_REVIEW_REQUIRED"}],"IndependentCoolDown":{"Prompt":"A six-disc puzzle has three pegs and the usual legal move constraints. Give the fewest moves, calculate it from the five-disc result, and explain why your recurrence must use two five-disc transfers.","Expected":"M(6)=2*M(5)+1=63; five smaller discs must be moved away and back.","Origin":"FIRST_PARTY_AUTHORED_NOT_PUBLISHER_COOLDOWN","Grading":"TEACHER_REVIEW_REQUIRED"},"TeacherReview":{"Required":True,"Evidence":["disc move log or demonstration","sequence table","recursion explanation","checker explanation if optional"],"NoClaimOfAutomatedRuntime":True},"TestsAuthoredNotRun":["tools/curriculum/us-ccss-g11-u01-l01-authoring-contract.test.cjs"],"TestsExecuted":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN","RichV2Path":str(RICH).replace("\\","/")}
dump(ASSESS,assessment)
base=Path("artifacts/curriculum-delivery/US-CCSS-MATH/G11")
spec=[
 ("U01L01-PREP-GOALS","teacher/preparation.html","Learning Goals","teaching goals and sequence vocabulary","TEACHER_LED"),
 ("U01L01-WU-RULE","teacher/index.html","1.1 Student Facing","five terms, twice previous minus one","DIGITAL"),
 ("U01L01-WU-DISCUSSION","teacher/index.html","1.1 Launch","partner comparison; initial value can vary","TEACHER_LED"),
 ("U01L01-A1-RULES","student/index.html","1.2 Tower","three pegs, one disc, never larger on smaller","TEACHER_LED"),
 ("U01L01-A1-3","student/index.html","1.2 Tower","minimum 3 discs = 7","TEACHER_LED"),
 ("U01L01-A1-4","student/index.html","1.2 Tower","minimum 4 discs = 15","TEACHER_LED"),
 ("U01L01-A1-5","student/index.html","1.2 Tower","prediction 5 discs =31","TEACHER_LED"),
 ("U01L01-A1-7","student/index.html","1.2 Tower","prediction 7 discs =127","TEACHER_LED"),
 ("U01L01-A1-RECURSION","teacher/index.html","1.2 Synthesis","M(n)=2*M(n-1)+1","TEACHER_LED"),
 ("U01L01-A1-64","student/index.html","Are you ready for more","2^64-1 seconds extension","TEACHER_LED"),
 ("U01L01-A2-CHECKERS","student/index.html","1.3 Checker Jumping Puzzle","optional coloured-checker swapping with move restrictions","TEACHER_LED"),
 ("U01L01-P01","student/practice.html","Problem 1","ten terms, initial 0 1, sum of two previous","DIGITAL"),
 ("U01L01-P02","student/practice.html","Problem 2","halve fractional sequence; three more terms","DIGITAL"),
 ("U01L01-P03","student/practice.html","Problem 3","forward/backward -2a+3, fourth term -7","DIGITAL"),
 ("U01L01-P04","student/practice.html","Problem 4","two distinct rules from 0 5","TEACHER_LED"),
 ("U01L01-SUMMARY","student/index.html","Summary","ordered terms and recursive relationships","DIGITAL"),
 ("U01L01-MATERIALS","teacher/preparation.html","Required Materials","physical discs, internet device optional","TEACHER_LED")
]
elements=[{"SourceElementId":k,"SourceLocator":loc+"#"+section,"MathematicalRequirement":math,"ImplementationType":kind,"TargetArtifactPath":str(RICH).replace("\\","/"),"RightsStatus":"LOCAL_CC_BY_4_SOURCE_CONTEXT","Status":"MATCHED_TEACHER_LED" if kind=="TEACHER_LED" else "MATCHED_DIGITAL_AUTHORING_ONLY","StageDRuntime":"NOT_RUN"} for k,loc,section,math,kind in spec]
ELEM=base/"u01-l01-element-coverage.json"
dump(ELEM,{"LessonCode":CODE,"SourceEdition":"Kendall Hunt archival Algebra 2; local pages show 2019 copyright, precise release tag not independently proven","LocalSourceHashes":digest,"SourceElementCount":len(elements),"Elements":elements,"PublisherCoolDown":"UNAVAILABLE_AUTHENTICATED_TEACHER; replaced by first-party equivalent","VideoApplet":"EXTERNAL_OPTIONAL_PHYSICAL_MANIPULATIVE_FALLBACK","TestState":"WRITTEN_NOT_RUN","StageD":"NOT_RUN"})
checkpoint=base/"u01-l01-abc-checkpoint.json"
dump(checkpoint,{"LessonCode":CODE,"Ordinal":1,"StartedUtc":NOW,"ClosedUtc":NOW,"SourceHashes":digest,"BlueprintIdPreserved":True,"PhaseA":"COMPARED","PhaseB":"FIRST_PARTY_RICH_V2_WRITTEN_RUNTIME_NOT_RUN","PhaseC":"ASSESSMENT_CONTRACT_AUTHORED_ENGINE_UNVERIFIED","AssessmentPath":str(ASSESS).replace("\\","/"),"ContentPaths":[str(PACK).replace("\\","/"),str(RICH).replace("\\","/")],"ElementEvidencePath":str(ELEM).replace("\\","/"),"StageD":"NOT_RUN","Status":"A_C_EXPLICIT_GAP","Blockers":["Dedicated Algebra 2 recurrence family not verified in existing runtime: cannot claim automatic numeric practice.","Lesson-specific Rich V2 registration in runtime is not proven; Stage D runtime verification remains pending.","Teacher-reviewed puzzle submission pathway only authored; no G11 portal integration proven."],"TestsExecuted":"NOT_RUN"})
print("G11_FIRST_LESSON_SOURCE_ALIGNED_AUTHORING_AND_GAP_RECORDED",len(elements),len(digest))
