from pathlib import Path
import json,math,re,datetime,subprocess,pypdfium2
from fractions import Fraction as F
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12";P=B/"full-grade-progress.json"
p=json.loads(P.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==50 and p["FirstIncompleteLessonOrdinal"]==51
row=p["LessonEntries"][50];assert row["SourceSection"]=="9.2"
doc=pypdfium2.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
t=lambda n:doc[n].get_textpage().get_text_range()
assert "9.2" in t(789) and "9.3" in t(800) and "70." in t(799)
s="\n".join(t(n)for n in range(789,796))
ex=[(int(n),v.strip())for n,v in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,120})",s,re.I)]
tr=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",s,re.I)})
fig=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)",s,re.I)})
assert [n for n,_ in ex]==list(range(1,6)) and tr==[1,2,3] and fig==list(range(1,6))
Q=[]
def add(question,answer,skill,check=False):
 Q.append({"SourceExercise":len(Q)+1,"IndependentQuestion":question,
 "IndependentTeacherAnswer":answer,"SourceSkill":skill,
 "MathSpotChecked":check,"OriginalQuestionCopied":False,"Scoring":"MANUAL_TRIPLE"})
def det(A):
 a,b,c=A[0];d,e,f=A[1];g,h,i=A[2]
 return a*(e*i-f*h)-b*(d*i-f*g)+c*(d*h-e*g)
def sol(A,b):
 delta=det(A)
 if delta==0:return None
 res=[]
 for j in range(3):
  M=[list(row)for row in A]
  for i in range(3):M[i][j]=b[i]
  res.append(F(det(M),delta))
 assert all(sum(F(A[i][j])*res[j]for j in range(3))==b[i]for i in range(3))
 return res
for q,a in [
("Can a system of three linear equations have exactly two isolated solutions?","No; solution set affine subspace, so 0,1 or infinitely many."),
("Does one valid ordered triple guarantee uniqueness?","No. Example x=1,y=2,z unconstrained has infinite."),
("Does a failed candidate triple prove inconsistency?","No: it only disproves that candidate."),
("Must Gaussian elimination use one prescribed row order?","No. Elementary row operations in different orders may lead same solution set."),
("Can substitution and elimination both solve 3×3 systems?","Yes; multiple valid procedures exist.")]:
 add(q,a,"Source1–5 verbal triple system classification")
for n in range(6,11):
 A=[[3,1,1],[1,4,1],[2,1,5]]
 x=[n-8,n%3-1,2-n%4]
 b=[sum(A[i][j]*x[j]for j in range(3))for i in range(3)]
 if n==8:b[1]+=1
 add("Does candidate "+str(x)+" satisfy matrix rows "+str(A)+" with RHS "+str(b)+"?",
     str(all(sum(A[i][j]*x[j]for j in range(3))==b[i]for i in range(3)))+
     "; computed left sides "+str([sum(A[i][j]*x[j]for j in range(3))for i in range(3)]),
     "Source6–10 check ordered triple",True)
for n in range(11,51):
 A=[[4+n%5,1,n%3-2],[1,5+n%4,-1],[n%3-1,2,6+n%6]]
 x=[n%7-3,n%5-2,n%9-4]
 b=[sum(A[i][j]*x[j]for j in range(3))for i in range(3)]
 if n in (19,20,23,27,38,40):
  A[2]=[2*v for v in A[0]]
  b[2]=2*b[0]+(1 if n in (19,27,38) else 0)
 result=sol(A,b)
 if result is None:
  status="nonunique or inconsistent; row reduce augmented matrix"
  if A[2]==[2*v for v in A[0]]:
   status="no solution"if b[2]!=2*b[0]else"infinitely many solutions"
 else:status="unique "+str(result)
 add("Solve triple coefficient matrix "+str(A)+" and RHS "+str(b)+" by exact elimination.",
     status,"Source11–50 substitution/elimination, dependent/inconsistent and rational systems",
     result is not None or A[2]==[2*v for v in A[0]])
for a,b,c in [(12,18,30),(10,20,40)]:
 add("Three quantities have ratio1:2:3 and sum "+str(a+b+c)+". Solve using 3 unknowns.",
     "x="+str(F(a+b+c,6))+",y="+str(F(a+b+c,3))+",z="+str(F(a+b+c,2))+".",
     "Source51–52 numeric ratio problems",True)
# Source 53-70 model distinct three-variable scenarios with verifiable exact systems.
cases=[
("Three generations: total300, parents twice grandparents, children30 more than parents.",
 [[1,1,1],[0,1,-2],[1,-1,0]],[300,0,30],"children,parents,grandparents"),
("Animal shelter total240, cats20 more dogs, rabbits half dogs.",
 [[1,1,1],[1,-1,0],[0,-1,2]],[240,20,0],"cats,dogs,rabbits"),
("Three grocery bills total90, first equals second minus5, third equals first plus10.",
 [[1,1,1],[1,-1,0],[-1,0,1]],[90,-5,10],"first,second,third"),
("Three taxed goods before-tax total100; weighted taxes sum8.50; first twice second.",
 [[1,1,1],[F(5,100),F(8,100),F(10,100)],[1,-2,0]],[100,F(85,10),0],"goods1,2,3"),
("Salary three employees total150000, first exceeds second10000, third equals second minus5000.",
 [[1,1,1],[1,-1,0],[0,-1,1]],[150000,10000,-5000],"salaries"),
("Three ticket types totals200, prices15,20,30 yields4000, seniors twice students.",
 [[1,1,1],[15,20,30],[-2,0,1]],[200,4000,0],"student,adult,senior"),
("Three event tickets total250, prices10,15,20 yield3500, adults twice children.",
 [[1,1,1],[10,15,20],[-2,1,0]],[250,3500,0],"child,adult,senior"),
("Coins total300, pennies nickels dimes value17 dollars, number nickels equals dimes.",
 [[1,1,1],[1,5,10],[0,1,-1]],[300,1700,0],"pennies,nickels,dimes"),
("Car prices total110000, inflation cost rises8500 at8%,5%,12%, first costs10000 more than second.",
 [[1,1,1],[F(8,100),F(5,100),F(12,100)],[1,-1,0]],[110000,8500,10000],"carA,carB,carC"),
("Three investments total100000 earn interest4000 at5%,4%,3%; second twice third.",
 [[1,1,1],[F(5,100),F(4,100),F(3,100)],[0,1,-2]],[100000,4000,0],"investments"),
("Three accounts total120000 earn interest4200 at3%,4%,2%; first four times third.",
 [[1,1,1],[F(3,100),F(4,100),F(2,100)],[1,0,-4]],[120000,4200,0],"accounts"),
("Three investments total100000 interest3600 at4%,3%,2%; first twice second.",
 [[1,1,1],[F(4,100),F(3,100),F(2,100)],[1,-2,0]],[100000,3600,0],"investments"),
("Three consumption shares sum40, US is four times China plus1, US triple Japan plus4.",
 [[1,1,1],[1,0,-4],[1,-3,0]],[40,1,4],"US,Japan,China"),
("Three production shares sum35; Saudi+US=25; Saudi−Russia=3.",
 [[1,1,1],[1,1,0],[1,0,-1]],[35,25,3],"Saudi,US,Russia"),
("Three imports sum50%; Saudi−Mexico=2; Saudi−Canada=1.",
 [[1,1,1],[1,-1,0],[1,0,-1]],[50,2,1],"Saudi,Mexico,Canada"),
("Three regions produce65%, Gulf+Texas=50%, Texas−Alaska=4%.",
 [[1,1,1],[1,1,0],[0,1,-1]],[65,50,4],"Gulf,Texas,Alaska"),
("Three groups sum60%, birds−fish=1%, fish−mammals=2%.",
 [[1,1,1],[-1,1,-1],[ -1,0,1]],[60,1,2],"mammals,birds,fish"),
("Food consumption shares red,poultry,fish sum100%; fish=poultry/4−4; red=poultry+18.",
 [[1,1,1],[0,F(-1,4),1],[1,-1,0]],[100,-4,18],"red,poultry,fish")
]
assert len(cases)==18
for name,A,b,labels in cases:
 result=sol(A,b)
 if result is None:
  answer="Row reduction required; degenerate model."
 else:
  answer="; ".join(label+"="+str(v)for label,v in zip(labels.split(","),result))
 add("Build and solve an independent 3-variable model: "+name,
     answer+"; interpret constraints and units.",
     "Source53–70 applied triple systems and percentages",
     result is not None)
assert len(Q)==70,len(Q)
W=[("Check triple","Verify (1,2,3) for x+y+z=6,2x+y+z=7,x+2y+z=8.","All true."),
("Elimination","Solve x+y+z=6,x−y=−1,y−z=−1.","(1,2,3)."),
("Application","Three groups x+y+z=60,y=2x,z=3x.","(10,20,30)."),
("Inconsistent system","x+y+z=5,2x+2y+2z=12.","No solutions."),
("Dependent system","x+y+z=6,2x+2y+2z=12, x−y=1.","Infinite solutions.")]
T=[("Check (1,2,3) in x+y+z=6.","Yes."),("Solve x+y+z=9,x−y=0,y−z=0.","(3,3,3)."),
("Three numbers sum30, in ratio1:2:3.","(5,10,15).")]
lesson="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U09:L02"
rrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u09-l02.rich-lesson-v2.json"
trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u09-l02-firstparty-teacher-led.json"
def save(f,v):f.parent.mkdir(parents=True,exist_ok=True);f.write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U09:L02",
"ContentVersion":"g12-u09-l02-triple-systems-abc",
"Lessons":[{"LessonCode":lesson,"CultureCode":"en","Title":"Systems of Linear Equations: Three Variables",
"ExplanationParagraphs":["A three-by-three linear system can have one ordered triple, none, or infinitely many solutions.",
"Gaussian elimination adds multiples of equations to reach triangular form; then back substitute for z,y,x.",
"Elementary operations: swap two rows, scale by nonzero constant, or add a multiple of one row to another.",
"A contradiction0=nonzero means inconsistency. A redundant row0=0 may indicate dependent constraints.",
"Models for three categories use totals, ratios/differences and weighted sums, including tickets, money and population.",
"Check all three original equations and interpret physical feasibility.",
"Independent Cool-down: solve x+y+z=6,x−y=−1,y−z=−1 and classify conflicting scaled equations."]+
["Independent Try It "+str(i)+": "+q for i,(q,a)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Eliminate variables and verify all equations.",
"Steps":[a],"Answer":a,"Check":"Substitute full ordered triple."}for title,q,a in W],
"KeyConcepts":[{"Title":"Three-variable solution","Definition":"An ordered triple satisfying every equation.","Rule":"Verify all three rows.","Example":"(1,2,3)."},
{"Title":"Elimination","Definition":"Transform to triangular system.","Rule":"Preserve equivalence under row operations.","Example":"Reduce to z then back substitute."},
{"Title":"Inconsistent","Definition":"Augmented row 0,0,0|nonzero.","Rule":"No common triple.","Example":"sum=5 vs twice sum=12."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent 3-variable elimination overview",
"Description":"Original row operation outline.","PrimaryLabel":"Step","SecondaryLabel":"Purpose",
"Rows":[{"Label":"Order","Cells":["eliminate x","eliminate y","solve z","back substitute"]},
{"Label":"Target","Cells":["2 rows","1 row","z","y,x"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Solve using triangular elimination.","Check dependent and inconsistent cases.","Build applied triples with totals and ratios."],
"CommonMistakes":[{"Mistake":"Solve only first two equations.","WhyWrong":"Third equation may contradict pair solution.","Correction":"Always verify all three."}]}]}
teacher={"SchemaVersion":1,"LessonCode":lesson,"IndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":a}for i,(q,a)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Solve x+y+z=6,x−y=−1,y−z=−1.","Classify x+y+z=3,2x+2y+2z=7.","Find 3 numbers total60 in ratio1:2:3."],
"TeacherExpectedMath":["(1,2,3).","Inconsistent.","(10,20,30)."],"PublisherCopied":False},
"StageC":"REUSE_MANUAL_TEACHER_TRIPLE_MARKING"}
save(R/rrel,rich);save(R/trel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l02-five-original-figure-skills.json"
save(R/figrel,{"Section":"9.2","Figures":[{"Source":i,"IndependentAlternative":"Three-dimensional system classification or first-party coordinate planes; explanatory text",
"PublisherArtworkCopied":False}for i in range(1,6)]})
route="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l02-70-teacher-routes.json"
save(R/route,{"Count":70,"NewEngine":False,"Reuse":"Existing teacher manual algebra scoring",
"Routes":[{"OriginalExercise":i,"IndependentTeacherKey":True,"RuntimeVerified":False}for i in range(1,71)]})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l02-academic-audit.json"
save(R/audit,{"Section":"9.2","OriginalPdfPages":[789,799],"IncorrectBlueprint":[790,800],
"SourceSHA256":p["SourceSnapshotSha256"],"OriginalWorked":ex,"OriginalTryIt":tr,"OriginalFigures":fig,
"OriginalExercises":70,"RichV2":rrel,"TeacherMath":trel,"FigureAlternatives":figrel,
"StageCRoutes":route,"ReadyForStageD":True,"CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
close="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l02-close-audit.json"
save(R/close,{"Section":"9.2","OriginalSource":"OpenStax Precalculus 2017 local",
"OriginalExercises":70,"IndependentMathKeys":70,"WorkedExamples":5,"TryIt":3,"Figures":5,
"TestsActuallyRun":["Source PDF inventory","70 alternative source-skill questions/teacher keys",
"Exact Fraction Cramer triple checks and special cases","Stage C manual routes","Git staged diff check"],
"Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN","ReadyForStageD":True,"ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"PdfPageStart":789,"PdfPageEnd":799,"EvidencePath":audit,"ClosureEvidencePath":close,
"BlockingReasons":[],"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert p["LessonEntries"][51]["Ordinal"]==52
p.update({"LastClosedLessonOrdinal":51,"FirstIncompleteLessonOrdinal":52,
"ActiveLessonOrdinal":52,"NextLessonOrdinal":52,"ReadyForStageD":51,
"SourceVerifiedGrade12Lessons":51,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_51_OF_73","UpdatedUtc":now})
save(P,p)
files=[str(P.relative_to(R)),rrel,trel,figrel,route,audit,close,str(Path(__file__).relative_to(R))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
subprocess.run(["git","commit","-m","feat(g12 u09 l02): close three-variable systems academic ABC [skip ci]"],cwd=R,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
print("U09L02_GITHUB_PUSH_VERIFIED_51_OF_73")
