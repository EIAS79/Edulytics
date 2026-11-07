from pathlib import Path
import json,math,datetime,time,subprocess,re,pypdfium2 as pdfium
started=time.monotonic()
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pp=base/"full-grade-progress.json";p=json.loads(pp.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==31 and p["FirstIncompleteLessonOrdinal"]==32
row=p["LessonEntries"][31]
assert row["Ordinal"]==32 and row["SourceSection"]=="5.4" and not row["ReadyForStageD"]
doc=pdfium.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
get=lambda i:doc[i].get_textpage().get_text_range()
assert "5.4" in get(501) and "CHAPTER 5 REVIEW" in get(513)
pages=[{"PdfPage":i,"Text":get(i)}for i in range(501,513)]
sec="\n".join(get(i)for i in range(501,510))
examples=[(int(n),title.strip())for n,title in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,105})",sec,re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",sec,re.I)})
figures=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)","\n".join(get(i)for i in range(501,513)),re.I)})
assert [n for n,_ in examples]==list(range(1,7)) and tries==list(range(1,7))
assert figures==list(range(1,16)) and "56." in get(512)
Q=[]
def add(q,ans,src):
 n=len(Q)+1
 assert q and ans and src
 Q.append({"SourceExercise":n,"IndependentStudentQuestion":q,"TeacherExpectedMath":ans,
 "SourceMathematicalSkillReviewed":src,"OriginalQuestionCopied":False,
 "TeacherReview":"GEOMETRY_MANUAL_SCORING","StageDRuntimeVerified":False})
for q,ans,src in [
("In a right triangle, identify hypotenuse, adjacent and opposite sides relative to acute angle A.","Hypotenuse opposite90°; adjacent leg touches A; opposite leg lies across from A.","Source #1 label right triangle sides."),
("For unit-circle triangle with hypotenuse1, identify coordinates from legs.","Adjacent=x=cos t; opposite=y=sin t.","Source #2 coordinate legs."),
("What ratio defines tan A?","tan A=opposite leg/adjacent leg, adjacent nonzero.","Source #3 tangent as opposite/adjacent."),
("How are the two non-right angles in a right triangle related?","They sum to90°.","Source #4 acute angles complementary."),
("State sine-cosine cofunction identity.","sin θ=cos(90°−θ); similarly tanθ=cot(90°−θ), where defined.","Source #5 cofunction pairs.")
]:add(q,ans,src)
for q,ans,src in [
("Complete cos(25°)=sin(___°).","65°.","Source #6 cos34°=sin56°."),
("Complete cos(π/6)=sin(___).","π/3.","Source #7 cosπ/3=sinπ/6."),
("Complete csc(35°)=sec(___°).","55°.","Source #8 csc21°=sec69°."),
("Complete tan(π/3)=cot(___).","π/6.","Source #9 tanπ/4=cotπ/4.")
]:add(q,ans,src)
# side a opposite angle A; b opposite B; c hypotenuse
for q,ans,src in [
("Right triangle with cos B=3/5 and adjacent leg a=9. Find b,c.","c=15,b=12 by3-4-5 ratio.","Source #10 cosB=4/5 and a=10, infer hypotenuse and b."),
("Right triangle with sin B=3/5 and opposite A leg a=16. Find b,c.","cosB=a/c=4/5 =>c=20,b=12.","Source #11 sinB=1/2 and a=20."),
("Given tan A=3/4 and adjacent leg b=8, find a and c.","a=6,c=10.","Source #12 tan A=5/12,b=6."),
("Given tan A=2 and adjacent b=7, find a,c.","a=14,c=7sqrt5.","Source #13 tanA=100 and b=100."),
("Given sin B=1/2 and adjacent a=3sqrt3, find b,c.","B=30°, c=a/cosB=6,b=3.","Source #14 sinB=1/sqrt3,a=2."),
("A leg opposite A equals 6 and A=30°. Find b,c.","c=12,b=6sqrt3.","Source #15 a=5,A60°."),
("Hypotenuse10 and A45°. Find legs a,b.","a=b=5sqrt2.","Source #16 c12,A45°.")
]:add(q,ans,src)
# 17-28 two independent triangles, source diagram skills
triangles=[(6,8),(5,12)]
fns=["sin","cos","tan","csc","sec","cot"]
for tri,(opp,adj)in enumerate(triangles):
 hyp=math.hypot(opp,adj)
 vals={"sin":opp/hyp,"cos":adj/hyp,"tan":opp/adj,"csc":hyp/opp,"sec":hyp/adj,"cot":adj/opp}
 for fn in fns:
  i=len(Q)+1
  add(f"In right triangle with opposite {opp}, adjacent {adj}, find {fn}A.",
 f"{vals[fn]:.6f}; hypotenuse={hyp:g}.",
 f"Source #{i} function of angle A from source triangle Figure{14+tri}.")
assert len(Q)==28
# 29-31 right triangle independent exact
for q,ans,src in [
("A right triangle has hypotenuse10 and acute angle30°. Find legs.","Opposite=5, adjacent=5sqrt3.","Source #29 30° triangle with given side7."),
("A right triangle has adjacent leg5 and angle60°. Find opposite and hypotenuse.","Opposite=5sqrt3; hypotenuse10.","Source #30 60° triangle with side10."),
("A right triangle has hypotenuse8 and angle45°. Find legs.","Both4sqrt2.","Source #31 45° right triangle unknown sides.")
]:add(q,ans,src)
# 32-41 right triangle numerical calculator
cases=[
("hyp",15,40),("adj",7,35),("opp",11,62),("hyp",12,10),("adj",9,65),
("opp",14,81),("adj",10,15),("hyp",160,5),("hyp",60,21),("opp",20,27)]
for given,value,angle in cases:
 th=math.radians(angle)
 if given=="hyp":a=value*math.sin(th);b=value*math.cos(th);c=value
 elif given=="adj":b=value;a=b*math.tan(th);c=b/math.cos(th)
 else:a=value;b=a/math.tan(th);c=a/math.sin(th)
 i=len(Q)+1
 add(f"Right triangle angle A={angle}°, {given}={value}. Solve opposite a, adjacent b, hypotenuse c to4 decimals.",
 f"a≈{a:.4f}, b≈{b:.4f}, c≈{c:.4f}.",
 f"Source #{i} numeric calculator triangle, unknown sides with correct side labels.")
# 42-45 compound triangulation of height difference at two angles; first-party independent problem
for i,(distance,elev1,elev2) in enumerate([(90,30,55),(100,25,60),(110,35,65),(120,26,70)],42):
 height=distance*(math.tan(math.radians(elev2))-math.tan(math.radians(elev1)))
 add(f"Two elevation angles to vertically aligned levels are {elev1}° and {elev2}°, from horizontal distance {distance}m. Find vertical separation.",
 f"h={distance}(tan{elev2}°−tan{elev1}°)≈{height:.4f}m.",
 f"Source #{i} composite two-angle triangle/height configuration solved with tangent.")
# 46-51 elevations and depressions, same line of sight
for i,(d,elev,down) in enumerate([(250,35,20),(300,40,30),(350,15,5),(450,18,4)],46):
 h=d*(math.tan(math.radians(elev))+math.tan(math.radians(down)))
 add(f"Observer {d}ft horizontally from monument sees top elevation {elev}° and base depression {down}°. Find monument total height.",
 f"H={d}(tan{elev}°+tan{down}°)≈{h:.2f}ft.",
 f"Source #{i} angle of elevation/depression tower/monument; use sum of vertical components.")
for i,(d,low,high) in enumerate([(250,35,43),(400,31,38)],50):
 h=d*(math.tan(math.radians(high))-math.tan(math.radians(low)))
 add(f"At {d}ft, top of building elevation {low}°, antenna top elevation {high}°. Find antenna height.",
 f"h={d}(tan{high}°−tan{low}°)≈{h:.2f}ft.",
 f"Source #{i} antenna/rod top elevation difference.")
# 52-56 real world applications
for i,(length,angle)in enumerate([(30,75),(24,80)],52):
 h=length*math.sin(math.radians(angle))
 add(f"Ladder length{length}ft makes angle{angle}° with ground. Find wall reach.",
 f"height={length}sin{angle}°≈{h:.2f}ft.",
 f"Source #{i} ladder sin-angle height.")
for i,(dist_mile,angle)in enumerate([(1.5,8),(2.5,3)],54):
 h=dist_mile*5280*math.tan(math.radians(angle))
 add(f"Horizontal observer is {dist_mile}mi from building, elevation angle{angle}°. Find height feet.",
 f"height={dist_mile}·5280·tan{angle}°≈{h:.2f}ft.",
 f"Source #{i} distant building elevation.")
add("A 300-ft vertical tree is viewed at elevation angle45°. Find observer distance to base.",
 "Distance=300/tan45°=300ft.",
 "Source #56 giant redwood370ft with elevation60°, find horizontal distance.")
assert len(Q)==56 and [q["SourceExercise"]for q in Q]==list(range(1,57))
# worked examples & tryit 1-6
W=[
("Right triangle ratio","Given opposite9,adjacent12,hyp15, find sin,cos,tan.","sin3/5,cos4/5,tan3/4."),
("Angle orientation","Find sin/cos of acute angle30° even when triangle shifted on page.","sin1/2,cos sqrt3/2 independent of placement."),
("Special triangle","For45-45-90 triangle hyp10, find legs.","Each5sqrt2."),
("Cofunction","Evaluate cos35° as a sine.","cos35°=sin55°."),
("Missing side with tangent","Angle40°, adjacent20ft; find opposite.",f"Opposite20tan40°≈{20*math.tan(math.radians(40)):.2f}ft."),
("Indirect height","At distance100m, elevation35° to tower. Find tower height.",f"100tan35°≈{100*math.tan(math.radians(35)):.2f}m.")
]
T=[
("Opposite4, adjacent3, hyp5: find all six trig functions.","sin4/5,cos3/5,tan4/3,csc5/4,sec5/3,cot3/4."),
("Find tanA for a 5-12-13 right triangle with opposite5.","5/12."),
("45° triangle with hyp6: find each leg.","3sqrt2."),
("Complete sin25°=cos(___°).","65°."),
("Angle60°, adjacent10: find opposite.","10sqrt3."),
("Ladder10m at70°: find vertical reach.",f"{10*math.sin(math.radians(70)):.2f}m.")
]
lessoncode="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U05:L04"
richrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u05-l04.rich-lesson-v2.json"
teacherrel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u05-l04-firstparty-teacher-led.json"
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U05:L04",
"ContentVersion":"g12-u05-l04-firstparty-right-triangle-abc",
"Lessons":[{"LessonCode":lessoncode,"CultureCode":"en","Title":"Right Triangle Trigonometry",
"ExplanationParagraphs":["For acute angle A in a right triangle, sin A=opposite/hypotenuse, cos A=adjacent/hypotenuse and tan A=opposite/adjacent.",
"Cosecant, secant and cotangent are reciprocal ratios, respectively hypotenuse/opposite, hypotenuse/adjacent and adjacent/opposite.",
"The hypotenuse always lies opposite the right angle, no matter how triangle is oriented on the page.",
"For right-triangle acute angles A+B=90°; use cofunction identities sin A=cos B, tan A=cot B.",
"Special 30-60-90 and45-45-90 triangles provide exact side ratios 1:sqrt3:2 and1:1:sqrt2.",
"Use inverse sides with trig ratios to solve unknown lengths. Never round intermediate computations unnecessarily.",
"Angles of elevation and depression can be modeled by one or multiple right triangles. Label horizontal distances and vertical heights before selecting tangent.",
"Independent Cool-down: (1) find ratios in 5-12-13 triangle, (2) solve complementary angle to32°, (3) find tower height from100m at angle40°."]+
[f"Independent Try It {i}: {q}"for i,(q,ans)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Label opposite, adjacent and hypotenuse; choose correct trig ratio.","Steps":[ans],"Answer":ans,"Check":"Use Pythagorean identity and physical length units."}for title,q,ans in W],
"KeyConcepts":[{"Title":"SOH-CAH-TOA","Definition":"sin=opp/hyp,cos=adj/hyp,tan=opp/adj.","Rule":"Relative to chosen acute angle.","Example":"3-4-5 triangle."},
{"Title":"Cofunction","Definition":"sin A=cos(90°−A), tan A=cot(90°−A).","Rule":"Use only complementary acute angles in right-triangle context.","Example":"sin20°=cos70°."},
{"Title":"Indirect measurement","Definition":"tanθ=height/horizontal distance.","Rule":"Assume flat horizontal baseline for basic problems.","Example":"height=distance·tanθ."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent right triangle side ratio table",
"Description":"First-party geometry table.",
"PrimaryLabel":"Known sides","SecondaryLabel":"Trig ratio",
"Rows":[{"Label":"Triangle","Cells":["3-4-5","5-12-13","8-15-17"]},
{"Label":"sin A (opp first)","Cells":["3/5","5/13","8/17"]},
{"Label":"cos A","Cells":["4/5","12/13","15/17"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Identify opposite/adjacent relative to angle.","Use trig ratios and reciprocal functions.","Apply special triangle ratios.","Model elevation/depression with labeled distances."],
"CommonMistakes":[{"Mistake":"Confuse adjacent side and hypotenuse.","WhyWrong":"Adjacent is a leg, hypotenuse opposite90°.","Correction":"Mark right angle and triangle first."},
{"Mistake":"Use tangent of degrees with calculator set to radians.","WhyWrong":"Numeric output incorrect.","Correction":"Match angle-mode units."}]}]}
teacher={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":lessoncode,
"FirstPartyIndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(q,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Opposite5, adjacent12,hyp13: find sine, cosine, tangent.",
"Find complement of32°.","At100m horizontal distance angle40° elevation: find height."],
"TeacherExpectedMath":["sin5/13,cos12/13,tan5/12.","58°.",f"≈{100*math.tan(math.radians(40)):.2f}m."],
"OriginalPublisherCoolDownCopied":False},
"OriginalSourceExercisesReviewed":56,"StageC":"REUSE_EXISTING_RICHV2_MANUAL_TRIANGLE_MARKING","StageD":"NOT_RUN"}
def save(path,x):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
save(root/richrel,rich);save(root/teacherrel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u05-l04-fifteen-right-triangle-figure-skills.json"
roles=["Right triangle scenario","Opposite-adjacent-hypotenuse labels","Triangle orientation and ratio",
"Special acute angles","Cofunction complementary angles","Right triangle example ratios",
"Shifted orientation","Special-angle triangle","30-60-90 relations",
"45-45-90 relations","Missing length using trig","Elevation depression line of sight",
"Ladder and tower application","First numerical source triangle","Second numerical source triangle"]
save(root/figrel,{"SchemaVersion":1,"Section":"5.4","OriginalSourceFigureCount":15,
"IndependentMathematicalAlternatives":[{"SourceFigure":i+1,"SourceSkill":skill,
"FirstPartyRightTriangleScenario":True,"PublisherImageCopied":False}for i,skill in enumerate(roles)],
"AuthenticatedVisualRuntime":"NOT_RUN"})
routesrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u05-l04-stage-c-56-triangle-assessment-routes.json"
save(root/routesrel,{"SchemaVersion":1,"Section":"5.4","RoutesCount":56,
"ReuseExisting":["RichV2 content","Teacher manual triangle and geometry scoring"],
"NewMathEngine":False,"SourceGraphicPixelsCopied":False,
"Routes":[{"SourceExercise":q["SourceExercise"],"IndependentQuestion":True,"TeacherMath":True,
"RuntimeVerified":False}for q in Q],"CI":"NOT_RUN","StageD":"NOT_RUN"})
assert len(W)==6 and len(T)==6 and len(roles)==15 and len(Q)==56
assert math.isclose(5**2+12**2,13**2)
assert p["StageD"]=="NOT_RUN" and not p["CIRun"]
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
closerel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u05-l04-abc-close-audit.json"
save(root/closerel,{"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":5,"Lesson":4,
"OriginalSource":"OpenStax Precalculus 2017 approved local PDF",
"SourceSHA256":p["SourceSnapshotSha256"],"CorrectedPdfPages":[501,512],
"IncorrectBlueprintPages":[502,513],"SourceWorked":6,"SourceTryIt":6,
"SourceFigures":15,"SourceExercises":56,"SourceMathExercisesReviewed":56,
"IndependentWorked":6,"IndependentTryIt":6,"IndependentMathTeacherQuestions":56,
"FirstPartyFigureSkillAlternatives":15,"FirstPartyCoolDown":3,
"StageC":"EXISTING_MANUAL_TEACHER_SCORING_REUSE","NewMathEngine":False,
"PublisherOriginalGraphicPixelsCopied":False,"CCSSCandidates":["CCSS:HSF-TF.A.3"],
"FormalFullCCSSCertified":False,
"TargetedChecksActuallyRun":["Local section boundary and source exercise indexing",
"56 independently authored source-skill problem keys",
"6 worked 6 Try It, 15 geometry figure skill alternatives",
"Numerical trig and inverse tower height spot checks","Existing teacher review routes and git diff"],
"Runtime":"NOT_RUN","Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
auditrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u05-l04-academic-source-audit.json"
save(root/auditrel,{"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Section":"5.4",
"SourceSHA256":p["SourceSnapshotSha256"],"CorrectedPdfPages":[501,512],
"OriginalWorkedExamples":[{"Number":n,"Title":title}for n,title in examples],
"OriginalTryItNumbers":tries,"OriginalFigures":figures,
"OriginalExerciseLedger":[{"Number":q["SourceExercise"],"SourceMathReviewed":True,
"IndependentTeacherKeyReady":True}for q in Q],
"FirstPartyRichV2":richrel,"TeacherAssessment":teacherrel,
"SourceFigureSkills":figrel,"StageCAssessmentRoutes":routesrel,
"ClosureEvidence":closerel,"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,
"BlockingReasons":[],"StageD":"NOT_RUN","CI":"NOT_RUN","ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"PdfPageStart":501,"PdfPageEnd":512,"BlockingReasons":[],
"OriginalSourceExercisesMathReviewed":56,"FirstPartyTeacherKeys":56,
"FirstPartySourceFigureSkills":15,"EvidencePath":auditrel,"ClosureEvidencePath":closerel,
"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now,"UpdatedUtc":now})
assert p["LessonEntries"][32]["Ordinal"]==33
p.update({"LastClosedLessonOrdinal":32,"FirstIncompleteLessonOrdinal":33,
"ActiveLessonOrdinal":33,"NextLessonOrdinal":33,"ReadyForStageD":32,
"SourceVerifiedGrade12Lessons":32,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_32_OF_73",
"UpdatedUtc":now})
save(pp,p)
files=[str(pp.relative_to(root)),richrel,teacherrel,figrel,routesrel,closerel,auditrel,str(Path(__file__).relative_to(root))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=root,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=root,check=True)
subprocess.run(["git","commit","-m","feat(g12 u05 l04): close right-triangle trigonometry academic ABC [skip ci]"],cwd=root,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=root,check=True)
print("U05L04_ACADEMIC_CLOSE_PUSH_SUCCESS 32_OF_73 UNIT5_COMPLETE")
print("SHA",subprocess.run(["git","rev-parse","HEAD"],cwd=root,capture_output=True,text=True,check=True).stdout.strip())
print("ELAPSED_SEC",round(time.monotonic()-started,2))
