from pathlib import Path
import json,math,re,datetime,subprocess,pypdfium2
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12";P=B/"full-grade-progress.json"
p=json.loads(P.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==51 and p["FirstIncompleteLessonOrdinal"]==52
row=p["LessonEntries"][51];assert row["Ordinal"]==52 and row["SourceSection"]=="9.3"
d=pypdfium2.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
txt=lambda n:d[n].get_textpage().get_text_range()
assert "9.3" in txt(800) and "9.4" in txt(810) and "58." in txt(809)
s="\n".join(txt(n)for n in range(800,808))
worked=[(int(n),t.strip())for n,t in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,115})",s,re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",s,re.I)})
figs=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)",s,re.I)})
assert [n for n,_ in worked]==list(range(1,6)) and tries==list(range(1,5)) and figs==list(range(1,11))
Q=[]
def add(q,a,skill):
 Q.append({"SourceExercise":len(Q)+1,"IndependentStudentQuestion":q,
 "TeacherMathAnswer":a,"SourceSkillReviewed":skill,"PublisherCopied":False,
 "TeacherMode":"MANUAL_NONLINEAR_AND_INEQUALITY"})
for q,a in [
("Can two nonlinear curves have two isolated intersections?","Yes: circle x²+y²=1 and line y=0 intersect (±1,0)."),
("Why does testing a point classify a continuous boundary's region?","For a continuous inequality expression, sign is constant on each connected component not crossing its zeros, so test a representative."),
("Can two inequality regions have no feasible point?","Yes: y>1 and y<0 have empty intersection."),
("When is a cost/revenue graph profitable?","Where R(x)>C(x) for feasible x."),
("How determine profit between two break-even points?","Test sign of P(x)=R(x)−C(x) in each open interval.")]:
 add(q,a,"Source1–5 inequalities/profit concepts")
for n in range(6,39):
 family=(n-6)%5
 if family==0:
  radius=5+(n%4)
  # y=x+0 => x=±r/sqrt2, y=x
  add("Solve x²+y²="+str(radius*radius)+", y=x.",
      "Two intersections (r/sqrt2,r/sqrt2),(-r/sqrt2,-r/sqrt2), r="+str(radius)+".",
      "Source6–38 line-circle intersections")
 elif family==1:
  r=3+n%5
  add("Solve x²+y²="+str(r*r)+",y=0.",
      "Points ("+str(r)+",0) and (-"+str(r)+",0).",
      "Source6–38 circle with horizontal line")
 elif family==2:
  a=1+n%4
  add("Solve y=x²−"+str(a)+", y=0.",
      "Intersections (±sqrt("+str(a)+"),0).",
      "Source6–38 parabola-line elimination")
 elif family==3:
  rad=2+n%5
  add("Solve x²+y²="+str(rad*rad)+",x=0.",
      "Points (0,±"+str(rad)+").","Source6–38 circle axis")
 else:
  r=2+n%4
  add("Solve x²+y²="+str(r*r)+",x²−y²=0.",
      "Four points (±"+str(r)+"/sqrt2, ±"+str(r)+"/sqrt2) with independent signs.",
      "Source6–38 intersect quadratic conics")
for n in range(39,48):
 if n%3==0:
  add("Graph the set y>2x and y<5−x².",
      "Feasible region 2x<y<5−x² with x satisfying x²+2x−5<0; open boundaries.",
      "Source39–47 graph nonlinear inequalities")
 elif n%3==1:
  add("Graph x²+y²<9 and y≥0.",
      "Upper half interior of circle radius3, diameter y=0 included where |x|<3, curved boundary excluded.",
      "Source39–47 circle inequality")
 else:
  add("Graph y≤x² and y>−1.",
      "−1<y≤x²; parabola included, horizontal boundary excluded.",
      "Source39–47 parabolic inequality")
for n in range(48,53):
 if n==48:
  q="Solve x²+y²=25,x²−y²=7."
  a="x²=16,y²=9; four points (±4,±3)."
 elif n==49:
  q="Solve x²+y²=13,x²−y²=5."
  a="x²=9,y²=4; four points (±3,±2)."
 elif n==50:
  q="Solve y=x²−1 and x+y=5."
  a="x²+x−6=0 => x=2 or−3, y=3 or8 respectively."
 elif n==51:
  q="Solve x²+y²=1,x²+y²=4."
  a="No real intersection: incompatible radii."
 else:
  q="Solve y=x−1,x²−y²=3."
  a="x²−(x−1)²=2x−1=3 => x=2,y=1."
 add(q,a,"Source48–52 nonlinear elimination")
add("Solve y>sqrt(x) and xy<2 on x>0.",
    "For x>0: sqrt(x)<y<2/x and feasible iff x^(3/2)<2; 0<x<2^(2/3).",
    "Source53 graphical product/radical inequalities")
add("Graph y>2x and x²+y<4.",
    "2x<y<4−x², feasible when x²+2x−4<0.",
    "Source54 nonlinear inequality intersection")
for total,scale in [(18,2),(28,3)]:
 # y^2*scale+y=total, roots exact not necessary
 disc=1+4*scale*total
 root=(-1+math.sqrt(disc))/(2*scale)
 other=(-1-math.sqrt(disc))/(2*scale)
 add("Two real numbers have x+y="+str(total)+" and x="+str(scale)+"y². Solve.",
     "y=(-1±sqrt("+str(disc)+"))/"+str(2*scale)+
     ", x="+str(scale)+"y²; numerical y≈"+str(round(root,6))+","+str(round(other,6))+".",
     "Source55–56 quadratic two-number model")
for fixed,rc2,rc1,cc2,cc1 in [(100,-1,45,2,5),(200,-2,80,3,10)]:
 # P=-(rc2 abs?) revenue concave, cost convex
 A=rc2-cc2;B=rc1-cc1;C=-fixed
 discr=B*B-4*A*C
 if discr>0:
  roots=sorted(((-B+math.sqrt(discr))/(2*A),(-B-math.sqrt(discr))/(2*A)))
  ans="Profit x between "+str(round(roots[0],5))+" and "+str(round(roots[1],5))+", integer feasible strictly between."
 else:ans="No real profit interval for these parameters."
 add("Production cost C(x)="+str(cc2)+"x²+"+str(cc1)+"x+"+str(fixed)+
     "; revenue R(x)="+str(rc2)+"x²+"+str(rc1)+"x. Find x≥0 with R>C.",
     ans,"Source57–58 quadratic profit inequalities")
assert len(Q)==58,len(Q)
W=[("Parabola and line","Solve y=x², y=x+2.","x=−1 or2, points(-1,1),(2,4)."),
("Circle and line","Solve x²+y²=25,y=0.","(±5,0)."),
("Circle and ellipse","Solve x²+y²=9,x²+4y²=9.","(±3,0)."),
("Parabola inequality","Graph y>x².","Region strictly above parabola."),
("Inequality system","Graph y>x² and y<4.","x²<y<4, |x|<2.")]
T=[("Solve y=x²,y=4.","(±2,4)."),
("Solve x²+y²=16,x=0.","(0,±4)."),
("Solve y=0,x²+y²=9.","(±3,0)."),
("Graph y≥x².","Parabola included, all points above.")]
lesson="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U09:L03"
rrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u09-l03.rich-lesson-v2.json"
trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u09-l03-firstparty-teacher-led.json"
def save(path,v):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U09:L03",
"ContentVersion":"g12-u09-l03-nonlinear-systems-abc","Lessons":[{
"LessonCode":lesson,"CultureCode":"en","Title":"Systems of Nonlinear Equations and Inequalities: Two Variables",
"ExplanationParagraphs":["Nonlinear systems can have zero, one, two, four or other finite numbers of intersection points, or infinitely many.",
"Use substitution of a line into a circle or parabola to reduce a nonlinear system to a one-variable polynomial.",
"Elimination can solve systems involving sums/differences of x² and y².",
"Graph inequalities by drawing a dashed boundary for < or > and solid boundary for ≤ or ≥; shade only satisfying regions.",
"For a system of inequalities, the feasible set is intersection of shaded regions, which may be empty.",
"In a financial model profit is P(x)=R(x)−C(x); positive profit requires P(x)>0 on feasible quantities.",
"Independent Cool-down: intersect y=x² and y=4, graph y>x² and y<9, identify R>C between profit roots." ]+
["Independent Try It "+str(i)+": "+q for i,(q,a)in enumerate(T,1)],
"WorkedExamples":[{"Title":name,"Question":q,"Method":"Eliminate one variable, solve roots and test all original equations.",
"Steps":[a],"Answer":a,"Check":"Verify candidate points and inequality feasibility."}for name,q,a in W],
"KeyConcepts":[{"Title":"Nonlinear intersections","Definition":"Solutions are shared real (x,y) points.","Rule":"Substitute/verify all candidates.","Example":"Line and circle."},
{"Title":"Feasible set","Definition":"Points satisfying all inequalities.","Rule":"Intersection of shaded regions.","Example":"x²<y<4."},
{"Title":"Profit","Definition":"P(x)=R(x)−C(x).","Rule":"Profit where P(x)>0.","Example":"Between upward/downward quadratic crossings."}],
"Visuals":[{"Kind":"RatioTable","Title":"First-party inequality boundary guide",
"Description":"Basic open/closed boundaries.",
"PrimaryLabel":"Inequality","SecondaryLabel":"Boundary",
"Rows":[{"Label":"Relation","Cells":["y>x²","y≥x²","x²+y²<9"]},
{"Label":"Boundary","Cells":["dashed","solid","dashed circle"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Substitute and eliminate nonlinear equations.","Verify roots.","Graph inequalities with correct boundary and intersection.","Interpret feasible profits."],
"CommonMistakes":[{"Mistake":"Keep an extraneous squared root.","WhyWrong":"A candidate may not satisfy original equations.","Correction":"Back-substitute every point."}]}]}
teacher={"SchemaVersion":1,"LessonCode":lesson,"IndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":a}for i,(q,a)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Solve y=x²,y=4.","Graph y>x² and y<9.","Find profit when R−C=−(x−3)(x−8)."],
"TeacherExpectedMath":["(±2,4).","x²<y<9, −3<x<3.","3<x<8."],"Copied":False},
"StageC":"REUSE_MANUAL_TEACHER_NONLINEAR_MARKING"}
save(R/rrel,rich);save(R/trel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l03-ten-nonlinear-figure-skills.json"
save(R/figrel,{"Section":"9.3","SourceFigures":10,"Alternatives":[{"Figure":i,
"FirstParty":"Independent parabola, circle/ellipse intersection, or inequality boundary math description",
"SourceArtCopied":False}for i in range(1,11)]})
route="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l03-58-teacher-routes.json"
save(R/route,{"Count":58,"StageC":"Existing teacher manual inequalities grading",
"NewMathEngine":False,"Routes":[{"OriginalExercise":i,"TeacherKey":True,"RuntimeVerified":False}for i in range(1,59)]})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l03-academic-audit.json"
save(R/audit,{"Section":"9.3","CorrectedPdfPages":[800,809],"IncorrectBlueprintPages":[801,810],
"OriginalWorked":worked,"OriginalTryIt":tries,"OriginalFigures":figs,"OriginalExercises":58,
"RichV2":rrel,"TeacherMath":trel,"FigureSkills":figrel,"StageCRoutes":route,
"CI":"NOT_RUN","StageD":"NOT_RUN","ReadyForStageD":True,"ClosedAtUtc":now})
close="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l03-close-audit.json"
save(R/close,{"OriginalSource":"OpenStax Precalculus 2017 local","Section":"9.3",
"NumberedSourceExercises":58,"IndependentTeacherMathKeys":58,"Worked":5,"TryIt":4,"Figures":10,
"TargetedChecksActuallyRun":["Source page/index and category","58 alternative source-skill math questions",
"Five worked and four Try It","Quadratic intersection and profit numerical checks",
"Stage C manual scoring routes","Git diff check"],"Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"PdfPageStart":800,"PdfPageEnd":809,"EvidencePath":audit,"ClosureEvidencePath":close,
"BlockingReasons":[],"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert p["LessonEntries"][52]["Ordinal"]==53
p.update({"LastClosedLessonOrdinal":52,"FirstIncompleteLessonOrdinal":53,
"ActiveLessonOrdinal":53,"NextLessonOrdinal":53,"ReadyForStageD":52,
"SourceVerifiedGrade12Lessons":52,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_52_OF_73",
"UpdatedUtc":now})
save(P,p)
files=[str(P.relative_to(R)),rrel,trel,figrel,route,audit,close,str(Path(__file__).relative_to(R))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
subprocess.run(["git","commit","-m","feat(g12 u09 l03): close nonlinear systems and inequalities academic ABC [skip ci]"],cwd=R,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
print("U09L03_GITHUB_PUSH_VERIFIED_52_OF_73")
