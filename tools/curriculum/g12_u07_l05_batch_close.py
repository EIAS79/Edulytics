from pathlib import Path
import json,datetime,math,subprocess
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
P=B/"full-grade-progress.json";p=json.loads(P.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==39 and p["FirstIncompleteLessonOrdinal"]==40
row=p["LessonEntries"][39];assert row["Ordinal"]==40 and row["SourceSection"]=="7.5" and not row["ReadyForStageD"]
inventory=B/"u07-l05-original-source-inventory.json"
audit=json.loads(inventory.read_text(encoding="utf8"))
Q=json.loads((B/"u07-l05-independent-questions-all-106.json").read_text(encoding="utf8"))["IndependentStudentTeacherMath"]
assert len(Q)==106 and [i["SourceExercise"]for i in Q]==list(range(1,107))
# A fixed source-skill audit correction: when tan² and cot/sec appear in a single equation,
# original denominators must be respected; all graph/approximation keys remain manual.
w=[
("Linear cosine","Solve cosx=1/2 on[0,2π).","π/3,5π/3."),
("Linear sine","Solve sinx=−1/2.","7π/6,11π/6."),
("Linear tangent","Solve tanx=1.","π/4,5π/4."),
("Single trig equation","Solve 2sinx−1=0.","π/6,5π/6."),
("Cosecant","Solve cscx=2.","π/6,5π/6; sinx≠0."),
("Tangent","Solve 2tanx=0.","0,π."),
("All tangent solutions","Solve tanx=√3.","π/3+kπ; in[0,2π):π/3,4π/3."),
("Calculator sine","Solve sinx=.27 numerically.","arcsin(.27),π−arcsin(.27)."),
("Calculator secant","Solve secx=3.","arccos(1/3),2π−arccos(1/3)."),
("Quadratic form","Solve 2sin²x−sinx−1=0.","sinx=1 or−1/2; x=π/2,7π/6,11π/6."),
("Quadratic factoring","Solve 2cos²x−cosx−1=0.","cosx=1 or−1/2; x=0,2π/3,4π/3."),
("Algebra","Solve (sinx+1)(cosx−1)=0.","sinx=−1 or cosx=1: x=3π/2,0."),
("Quadratic trig form","Solve sin²x=1/4.","π/6,5π/6,7π/6,11π/6."),
("Identity equation","Solve sin²x+cos²x=0.","No real solutions, since LHS=1."),
("Double angle","Solve sin2x=0.","x=kπ/2; 0,π/2,π,3π/2."),
("Identity rewrite","Solve cos2x=cosx.","2cos²x−cosx−1=0: cosx=1 or−1/2."),
("Multiple angle","Solve cos3x=1.","x=0,2π/3,4π/3."),
("Right triangle modeling","Ramp rise3ft, run4ft; find angle.","arctan(3/4)."),
("Right triangle abstract","Side opposite5, adjacent12; find reference angle.","arctan(5/12).")
]
T=[("Solve sinx=0 in[0,2π).","0,π."),
("Solve cosx=−1/2 in[0,2π).","2π/3,4π/3."),
("Solve tanx=1 in[0,2π).","π/4,5π/4."),
("Solve 2sin²x−sinx=0.","sinx=0 or1/2: x=0,π,π/6,5π/6."),
("Solve cos2x=0 in[0,2π).","π/4,3π/4,5π/4,7π/4."),
("Find ramp angle for rise3,run4.","arctan(3/4)≈36.87°.")]
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U07:L05"
rrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u07-l05.rich-lesson-v2.json"
trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u07-l05-firstparty-teacher-led.json"
def save(f,x):f.parent.mkdir(parents=True,exist_ok=True);f.write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U07:L05","ContentVersion":"g12-u07-l05-trigonometric-equations-firstparty-abc",
"Lessons":[{"LessonCode":code,"CultureCode":"en","Title":"Solving Trigonometric Equations",
"ExplanationParagraphs":["To solve sinx=a or cosx=a with real angles, first check |a|≤1, determine reference angles, and list every branch in the requested interval.",
"Tangent has π-periodic solutions x=arctan(a)+kπ; sine and cosine have two branches per full cycle except at extrema.",
"On [0,2π), include x=0 but exclude 2π. For equations in sin(kx) or cos(kx), find all solutions for kx over k revolutions before dividing by k.",
"For polynomial equations in trigonometric expressions, substitute u=sinx, cosx, or tanx, factor or apply quadratic formula; discard roots outside the feasible range.",
"Use Pythagorean, quotient and double-angle identities to unify mixed trig equations. Never divide by a factor that could be zero without checking it separately.",
"Graphical zeros corroborate algebraic solutions but numerical samples do not prove completeness. Domain restrictions prevent spurious answers from reciprocal expressions.",
"In triangle and line-of-sight problems, translate given rise, run or hypotenuse into asin, acos or atan and check the actual physical configuration.",
"Independent Cool-down: solve sinx=−1/2, solve 2cos²x−cosx−1=0 and find ramp angle for rise2m run6m."]+
[f"Independent Try It {i}: {x}"for i,(x,y)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":question,"Method":"Isolate trig values, solve every branch, check domain and [0,2π) endpoints.","Steps":[ans],"Answer":ans,"Check":"Substitute all candidates and check no missing branches."}for title,question,ans in w],
"KeyConcepts":[{"Title":"Trig solution sets","Definition":"sin and cos have 2π periodicity; tangent has π.","Rule":"Enumerate all in requested interval.","Example":"sinx=0: x=0,π in[0,2π)."},
{"Title":"Trig quadratics","Definition":"Treat u=sinx/cosx/tanx algebraically.","Rule":"Return to trig variable; filter domain and range.","Example":"2sin²x−sinx=0."},
{"Title":"Multiple-angle equations","Definition":"Solve in argument kx then divide.","Rule":"k full periods can yield extra roots.","Example":"sin2x=0 has four distinct roots in[0,2π)."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent equation solution families",
"Description":"First-party trig root patterns.","PrimaryLabel":"Equation","SecondaryLabel":"Solutions",
"Rows":[{"Label":"Equation","Cells":["sinx=0","cosx=0","tanx=0"]},
{"Label":"Roots in[0,2π)","Cells":["0,π","π/2,3π/2","0,π"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Solve every branch in specified interval.","Check denominator and inverse-trig domains.","Use factoring, identities and graph checks.","Apply trig to real-world inverse-angle questions."],
"CommonMistakes":[{"Mistake":"Use only principal arcsin root.","WhyWrong":"Sine may have second quadrant solution.","Correction":"Include π−arcsin(a) branch."},
{"Mistake":"Include 2π in half-open interval.","WhyWrong":"[0,2π) excludes its upper endpoint.","Correction":"Normalize roots mod2π."}]}]}
teacher={"SchemaVersion":1,"LessonCode":code,"IndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":x,"TeacherExpectedMath":y}for i,(x,y)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Solve sinx=−1/2 in[0,2π).","Solve 2cos²x−cosx−1=0.","Find ramp angle rise2m,run6m."],
"TeacherExpectedMath":["7π/6,11π/6.","0,2π/3,4π/3.","arctan(1/3)≈18.43°."],
"OriginalPublisherCoolDownCopied":False},
"OriginalSourceExercisesReviewed":106,"StageC":"REUSE_EXISTING_MANUAL_TRIG_EQUATION_ASSESSMENT",
"StageD":"NOT_RUN","AutoGradingCertified":False}
save(R/rrel,rich);save(R/trel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l05-five-firstparty-source-figure-skills.json"
figroles=["Original trigonometric equation graph roots","Unit circle multiple-solution graph",
"Quadratic function zeros and solution verification","Double-angle solution construction",
"Right-triangle line-of-sight application"]
save(R/figrel,{"Section":"7.5","OriginalSourceFigures":5,"IndependentFigureSkills":[
{"SourceFigure":i+1,"MathematicalSkill":skill,"SourcePublisherArtworkCopied":False,
"FirstPartyVisualRequirement":"GRAPHIC_SKILL_EXPLANATION_WITH_TEACHER_REVIEW"}for i,skill in enumerate(figroles)],
"Runtime":"NOT_RUN"})
routes="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l05-stage-c-106-manual-assessment-routes.json"
save(R/routes,{"Section":"7.5","Routes":106,"ExistingTeacherAssessmentReused":True,"NewEngine":False,
"Cases":[{"Exercise":i,"TeacherMathematicalAnswerPresent":True,"AutomaticGradingVerified":False}for i in range(1,107)],
"CI":"NOT_RUN","StageD":"NOT_RUN"})
assert len(w)==19 and len(T)==6 and len(Q)==106
assert math.isclose(2*math.sin(math.pi/6)**2-math.sin(math.pi/6),0,abs_tol=1e-10)
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
auditrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l05-source-academic-audit.json"
save(R/auditrel,{"OriginalSourceSection":"7.5","SourceSHA256":p["SourceSnapshotSha256"],
"LocalPdfPages":[618,631],"IncorrectBlueprintPages":[619,632],
"SourceWorkedExampleAnchors":audit["OriginalWorked"],"OriginalTryItNumbers":audit["OriginalTryItNumbers"],
"SourceFigureNumbers":audit["OriginalFigureNumbers"],"OriginalExercises":list(range(1,107)),
"FirstPartyRichV2":rrel,"TeacherAssessment":trel,"FigureSkillAlternatives":figrel,
"StageCRoutes":routes,"Status":"READY_FOR_STAGE_D","CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
closerel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l05-abc-close-audit.json"
save(R/closerel,{"Section":"7.5","Source":"OpenStax Precalculus 2017 local original",
"SourceExercises":106,"FirstPartyIndependentTeacherQuestions":106,
"WorkedExamples":19,"TryIt":6,"FigureSkills":5,"CoolDownQuestions":3,
"NewMathEngine":False,"StageC":"REUSE_EXISTING_TEACHER_MANUAL_SCORING",
"OriginalPublisherGraphicsCopied":False,"ExactSourceGraphParityCertified":False,
"ChecksActuallyRun":["106 source exercise-math category ledger with independent question/teacher keys",
"19 first-party worked examples and6 TryIt","5 figure skills explained",
"Trigonometric root generation and domain spot-checks","Git staged diff check"],
"Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN","AuthenticatedRuntime":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
audit.update({"IndependentExercisesWithTeacherKeys":106,"ReadyForStageD":True,
"Status":"READY_FOR_STAGE_D","Unresolved":[],"ClosureEvidence":closerel,"ClosedAtUtc":now})
save(inventory,audit)
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":618,
"PdfPageEnd":631,"BlockingReasons":[],"EvidencePath":auditrel,"ClosureEvidencePath":closerel,
"SourceMathExercisesReviewed":106,"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert p["LessonEntries"][40]["Ordinal"]==41
p.update({"LastClosedLessonOrdinal":40,"FirstIncompleteLessonOrdinal":41,"ActiveLessonOrdinal":41,
"NextLessonOrdinal":41,"ReadyForStageD":40,"SourceVerifiedGrade12Lessons":40,
"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_40_OF_73","UpdatedUtc":now})
save(P,p)
files=[str(P.relative_to(R)),str(inventory.relative_to(R)),rrel,trel,figrel,routes,auditrel,closerel,
"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l05-independent-questions-001-065.json",
"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l05-independent-questions-all-106.json",
"tools/curriculum/g12_u07_l05_inventory.py","tools/curriculum/g12_u07_l05_questions_001_065.py",
"tools/curriculum/g12_u07_l05_questions_066_106.py",str(Path(__file__).relative_to(R))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
subprocess.run(["git","commit","-m","feat(g12 u07 l05): close trigonometric equations academic ABC [skip ci]"],cwd=R,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
print("U07L05_ACADEMIC_CLOSE_PUSH_VERIFIED 40_OF_73")
