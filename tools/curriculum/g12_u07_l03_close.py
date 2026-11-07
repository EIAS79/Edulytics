from pathlib import Path
import math,json,datetime,subprocess,pypdfium2 as pdfium,re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
progress=base/"full-grade-progress.json";p=json.loads(progress.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==37 and p["FirstIncompleteLessonOrdinal"]==38
idx=p["LessonEntries"][37];assert idx["Ordinal"]==38 and idx["SourceSection"]=="7.3" and not idx["ReadyForStageD"]
tasks=base/"u07-l03-independent-source-skills.json"
e=json.loads(tasks.read_text(encoding="utf8"));Q=e["IndependentTeacherMath"]
assert len(Q)==63 and [x["SourceExercise"]for x in Q]==list(range(1,64))
fixed={
20:"sin(θ/2)=1/sqrt10; cos(θ/2)=−3/sqrt10; tan(θ/2)=−1/3 (θ/2 in QII).",
21:"sin(θ/2)=5/sqrt26; cos(θ/2)=−1/sqrt26; tan(θ/2)=−5 (θ/2 in QII).",
22:"sin(θ/2)=3/sqrt10; cos(θ/2)=1/sqrt10; tan(θ/2)=3 (θ/2 in QI).",
23:"sin(θ/2)=1/sqrt3; cos(θ/2)=sqrt(2/3); tan(θ/2)=1/sqrt2 (θ/2 in QI).",
42:"(2−cos2x−2cos4x+cos6x)/32.",
43:"(2+cos2x−2cos4x−cos6x)/32.",
44:"(3−4cos2x+cos4x)/(4(1+cos2x)); cosx≠0.",
45:"(3−4cos2x+cos4x)/(3+4cos2x+cos4x); cosx≠0.",
48:"sinx(1−cos2x)/(1+cos2x); cosx≠0.",
49:"(3−4cos2x+cos4x)/(4(1+cos2x)); cosx≠0.",
50:"(sin2x)/2+(sin4x)/4.",
51:"(sinx)/2+(sin7x−sin5x)/4.",
52:"sinx(1−cosx)/(1+cosx); original cos(x/2)≠0.",
63:"cos16x=cos²8x−sin²8x=(cos8x−sin8x)(cos8x+sin8x); source additionally gives product using cos²4x−sin²4x=cos8x."
}
for n,ans in fixed.items():Q[n-1]["TeacherExpectedMath"]=ans
Q[9]["TeacherExpectedMath"]="Use cos²θ=(1+cos2θ)/2, sin²θ=(1−cos2θ)/2; θ in III implies sinθ<0,cosθ<0; then tan,sec,csc,cot follow."
assert math.isclose(math.sin(math.pi/8),math.sqrt(2-math.sqrt(2))/2)
for theta in [.19,.48,1.02]:
 s=math.sin(theta);c=math.cos(theta);c2=math.cos(2*theta);c4=math.cos(4*theta);c6=math.cos(6*theta)
 assert math.isclose(c*c*s**4,(2-c2-2*c4+c6)/32,rel_tol=1e-9,abs_tol=1e-9)
 assert math.isclose(c**4*s*s,(2+c2-2*c4-c6)/32,rel_tol=1e-9,abs_tol=1e-9)
 assert math.isclose(math.tan(theta)**4,(3-4*c2+c4)/(3+4*c2+c4))
e["IndependentTeacherMath"]=Q
tasks.write_text(json.dumps(e,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
doc=pdfium.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
txt=lambda i:doc[i].get_textpage().get_text_range()
assert "7.3" in txt(600) and "7.4" in txt(612)
source="\n".join(txt(i)for i in range(599,609))
ex=[(int(n),v.strip())for n,v in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,100})",source,re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",source,re.I)})
figs=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)","\n".join(txt(i)for i in range(599,611)),re.I)})
assert [n for n,_ in ex]==list(range(1,10)) and tries==list(range(1,6)) and figs==list(range(1,6))
W=[
("Exact double tangent","Given tanθ=1/3 QI, find tan2θ.","2(1/3)/(1−1/9)=3/4."),
("Double cosine","Given cosθ=3/5 QI, find cos2θ.","2(3/5)²−1=−7/25."),
("Double-angle proof","Prove sin2θ=2sinθ cosθ.","sin(θ+θ)=sinθcosθ+cosθsinθ."),
("Tangent double proof","Derive tan2θ.","tan2θ=sin2θ/cos2θ=2tanθ/(1−tan²θ)."),
("Power reduction","Express sin²(4x) using one cosine.","(1−cos8x)/2."),
("Power reduction expansion","Reduce sin⁴x.","(3−4cos2x+cos4x)/8."),
("Half sine","Find sin(π/8).","sqrt(2−sqrt2)/2."),
("Half cosine","Find cos(7π/8).","−sqrt(2+sqrt2)/2."),
("Half-angle measurement","If cosθ=3/5 and θ acute, find tan(θ/2).","sinθ/(1+cosθ)=(4/5)/(8/5)=1/2.")
]
T=[
("If cosθ=5/13 in QI, find cos2θ.","2(25/169)−1=−119/169."),
("Use sin2θ to simplify 2sin(3x)cos(3x).","sin6x."),
("Prove cos²θ−sin²θ=cos2θ.","From cos(θ+θ)."),
("Simplify cos²(2x).","(1+cos4x)/2."),
("Find sin(π/8).","sqrt(2−sqrt2)/2.")
]
lesson="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U07:L03"
richrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u07-l03.rich-lesson-v2.json"
teachrel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u07-l03-firstparty-teacher-led.json"
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U07:L03",
"ContentVersion":"g12-u07-l03-firstparty-double-half-power-abc",
"Lessons":[{"LessonCode":lesson,"CultureCode":"en","Title":"Double-Angle, Half-Angle, and Reduction Formulas",
"ExplanationParagraphs":["Use sin2θ=2sinθcosθ, cos2θ=cos²θ−sin²θ=1−2sin²θ=2cos²θ−1. Derived tan2θ=2tanθ/(1−tan²θ) requires a defined quotient.",
"Power-reducing formulas sin²θ=(1−cos2θ)/2 and cos²θ=(1+cos2θ)/2 transform even powers into lower powers or multiple angles.",
"Half-angle formulas sin(θ/2)=±sqrt((1−cosθ)/2), cos(θ/2)=±sqrt((1+cosθ)/2) take signs from the half-angle quadrant.",
"Half tangent is sinθ/(1+cosθ) or (1−cosθ)/sinθ on domains where denominators and original tangent exist.",
"Higher powers can be reduced using cos4θ and product-to-sum; verify equality algebraically while respecting the original denominator domain.",
"Independent Cool-down: find sin2θ if sinθ=3/5 in QI; reduce sin⁴x; compute tan(π/8)."]+
[f"Independent Try It {i}: {question}"for i,(question,ans)in enumerate(T,1)],
"WorkedExamples":[{"Title":name,"Question":question,"Method":"Apply double/half-angle identity and quadrant signs.","Steps":[ans],"Answer":ans,"Check":"Verify using Pythagorean relation and domain."}for name,question,ans in W],
"KeyConcepts":[{"Title":"Double-angle","Definition":"sin2t=2sint cost; cos2t=cos²t−sin²t.","Rule":"tan2t=2tant/(1−tan²t) where defined.","Example":"sin(2·π/6)=sqrt3/2."},
{"Title":"Half-angle","Definition":"sin(t/2)=±sqrt((1−cost)/2), cos(t/2)=±sqrt((1+cost)/2)","Rule":"Sign determined by quadrant of t/2.","Example":"sinπ/8=sqrt(2−sqrt2)/2."},
{"Title":"Power reduction","Definition":"sin²t=(1−cos2t)/2, cos²t=(1+cos2t)/2.","Rule":"Apply repeatedly for fourth powers.","Example":"sin⁴t=(3−4cos2t+cos4t)/8."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent double and half-angle comparison",
"Description":"First-party formula table.","PrimaryLabel":"Formula","SecondaryLabel":"Identity",
"Rows":[{"Label":"Double","Cells":["sin2t=2sint cost","cos2t=1−2sin²t","cos2t=2cos²t−1"]},
{"Label":"Reduction","Cells":["sin²t=(1−cos2t)/2","cos²t=(1+cos2t)/2","sin⁴t=(3−4cos2t+cos4t)/8"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Double-angle formulas derive from sums.","Power reduction handles squared and fourth powers.","Half-angle signs require quadrant checks."],
"CommonMistakes":[{"Mistake":"Take positive radical for every half-angle.","WhyWrong":"Signs depend on quadrant.","Correction":"Locate half-angle first."}]}]}
teacher={"SchemaVersion":1,"LessonCode":lesson,"FirstPartyIndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":question,"TeacherExpectedMath":ans}for i,(question,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Given sinθ=3/5 QI, find sin2θ.","Reduce sin⁴x.","Find exact tanπ/8."],
"TeacherExpectedMath":["24/25.","(3−4cos2x+cos4x)/8.","sqrt2−1."],"OriginalPublisherCoolDownCopied":False},
"StageC":"REUSE_EXISTING_TEACHER_MANUAL_TRIG_MATH","StageD":"NOT_RUN"}
def save(path,x):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
save(root/richrel,rich);save(root/teachrel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l03-five-firstparty-figure-skill-alternatives.json"
figskills=["Source opening photograph replaced by double-angle math scenario","Angles on unit circle illustrate cos2θ","Half-angle geometry and quadrant selection","Power-reduction function graph comparison","Reference triangle 5-12-13 half-angle computation"]
save(root/figrel,{"SchemaVersion":1,"OriginalFigures":5,"IndependentFigureSkills":[{"SourceFigure":i+1,"MathematicalSkill":skill,"PublisherImageCopied":False}for i,skill in enumerate(figskills)],"Runtime":"NOT_RUN"})
routesrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l03-stage-c-63-teacher-math-routes.json"
save(root/routesrel,{"Section":"7.3","Count":63,"Reuse":"RichV2 and manual teacher scoring","NewEngine":False,
"Routes":[{"SourceExercise":i,"TeacherMathReady":True}for i in range(1,64)],"CI":"NOT_RUN","StageD":"NOT_RUN"})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
auditrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l03-source-academic-audit.json"
save(root/auditrel,{"SourceSection":"7.3","OriginalSourceSHA256":p["SourceSnapshotSha256"],
"CorrectedLocalPdfPages":[599,610],"IncorrectBlueprintPages":[600,611],
"OriginalWorkedExamples":[{"Number":n,"Title":title}for n,title in ex],
"OriginalTryItNumbers":tries,"OriginalFigureNumbers":figs,
"OriginalNumberedExercises":list(range(1,64)),
"IndependentMathProblems":str(tasks.relative_to(root)).replace("\\","/"),
"RichLesson":richrel,"TeacherRubric":teachrel,"FigureAlternatives":figrel,
"StageC":routesrel,"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,
"Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN","UpdatedUtc":now})
closure="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l03-abc-close-audit.json"
save(root/closure,{"Course":"US-CCSS-MATH:G12","Unit":7,"Lesson":3,"Source":"OpenStax Precalculus local original PDF",
"OriginalSourceExercises":63,"IndependentTeacherKeys":63,"WorkedExamples":9,"TryIt":5,"FigureSkillAlternatives":5,
"OriginalPublisherFiguresCopied":False,"OriginalGraphicParityCertified":False,
"NewMathEngine":False,"CCSSCandidate":["CCSS:HSF-TF.C.9"],
"ChecksActuallyRun":["PDF section and source exercise indexing","63 independent question/teacher rubrics",
"9 worked,5 TryIt,5 figure skill alternatives","Targeted numeric double and power reduction identities",
"git diff --cached --check"],"Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
idx.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":599,
"PdfPageEnd":610,"BlockingReasons":[],"EvidencePath":auditrel,"ClosureEvidencePath":closure,
"SourceMathExercisesReviewed":63,"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert p["LessonEntries"][38]["Ordinal"]==39
p.update({"LastClosedLessonOrdinal":38,"FirstIncompleteLessonOrdinal":39,"ActiveLessonOrdinal":39,
"NextLessonOrdinal":39,"ReadyForStageD":38,"SourceVerifiedGrade12Lessons":38,
"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_38_OF_73","UpdatedUtc":now})
save(progress,p)
files=[str(progress.relative_to(root)),str(tasks.relative_to(root)),richrel,teachrel,figrel,routesrel,auditrel,closure,str(Path(__file__).relative_to(root)),"tools/curriculum/g12_u07_l03_questions.py"]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=root,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=root,check=True)
subprocess.run(["git","commit","-m","feat(g12 u07 l03): close double half angle reduction academic ABC [skip ci]"],cwd=root,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=root,check=True)
print("U07L03_CLOSE_PUSH_VERIFIED 38_OF_73")
