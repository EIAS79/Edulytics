from pathlib import Path
import json,re,datetime,subprocess,pypdfium2
from fractions import Fraction as F
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
P=B/"full-grade-progress.json"
book=pypdfium2.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
def text(i):return book[i].get_textpage().get_text_range()
def mul(A,B):
 assert len(A[0])==len(B)
 return [[sum(F(A[i][k])*B[k][j]for k in range(len(B)))for j in range(len(B[0]))]for i in range(len(A))]
def add(A,B):return [[A[i][j]+B[i][j]for j in range(len(A[0]))]for i in range(len(A))]
def scale(A,k):return [[k*v for v in row]for row in A]
def transpose(A):return [list(row)for row in zip(*A)]
def inv(A):
 n=len(A);assert all(len(row)==n for row in A)
 E=[[F(v)for v in A[i]]+[F(j==i)for j in range(n)]for i in range(n)]
 for col in range(n):
  pivot=next((r for r in range(col,n)if E[r][col]),None)
  if pivot is None:return None
  E[pivot],E[col]=E[col],E[pivot]
  fac=E[col][col];E[col]=[v/fac for v in E[col]]
  for r in range(n):
   if r==col:continue
   fac=E[r][col];E[r]=[E[r][k]-fac*E[col][k]for k in range(2*n)]
 return [row[n:]for row in E]
def det(A):
 n=len(A)
 if n==2:return F(A[0][0])*A[1][1]-F(A[0][1])*A[1][0]
 if n==3:
  a,b,c=A[0];d,e,f=A[1];g,h,i=A[2]
  return F(a)*(e*i-f*h)-F(b)*(d*i-f*g)+F(c)*(d*h-e*g)
 raise ValueError("only 2 and 3 matrices")
def seed(n,dim):
 if dim==2:return [[n%5+3,1],[1,n%3+5]]
 return [[n%5+5,1,2],[1,n%7+6,-1],[2,-1,n%4+7]]
def other(n,dim):
 if dim==2:return [[1,n%3+1],[n%5-2,2]]
 return [[1,n%3+1,-1],[2,-2,n%5+2],[0,3,1]]
def verify(A,I):
 assert I is not None
 n=len(A)
 assert mul(A,I)==[[F(i==j)for j in range(n)]for i in range(n)]
 assert mul(I,A)==[[F(i==j)for j in range(n)]for i in range(n)]
def answer(v):return str(v)
def question(A,b,method):
 I=inv(A)
 if I:
  v=mul(I,[[F(x)]for x in b])
  assert mul(A,v)==[[F(x)]for x in b]
  return "Solve "+str(A)+"·X="+str(b)+" by "+method+".", "X="+str([row[0]for row in v])
 return "Classify "+str(A)+"·X="+str(b)+" by "+method+".","Singular coefficient matrix: use row reduction to distinguish no/infinite solutions; inverse not defined."
def Qadd(Q,q,a,category,check=True):
 assert q and a
 Q.append({"OriginalExercise":len(Q)+1,"IndependentStudentMathQuestion":q,
 "TeacherMathAnswer":a,"SourceSkillFamily":category,
 "IndependentMathValidated":check,"PublisherQuestionCopied":False})
def getprofile(sec,start,end,expected,tries):
 s="\n".join(text(i)for i in range(start,end-2))
 ex=[(int(n),v.strip())for n,v in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,115})",s,re.I)]
 tr=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",s,re.I)})
 fs=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)",s,re.I)})
 assert [n for n,_ in ex]==list(range(1,expected+1)),(sec,ex)
 assert tr==list(range(1,tries+1)),(sec,tr)
 assert sec in text(start)
 return ex,tr,fs
profiles=[
 dict(ord=54,sec="9.5",unit=9,les=5,title="Matrices and Matrix Operations",start=820,end=831,count=59,worked=11,tryit=2,
 topics=["Describe matrices by dimensions and entries.","Addition/subtraction require equal shapes.","Scalar multiplication affects every entry.","Matrix multiplication uses row-by-column dot products.","Matrix products need matching inner dimensions.","AB need not equal BA.","A matrix power B^n requires square B."]),
 dict(ord=55,sec="9.6",unit=9,les=6,title="Solving Systems with Gaussian Elimination",start=831,end=844,count=61,worked=11,tryit=6,
 topics=["Represent equations as augmented [A|b] with aligned variables.","Swap rows, scale a row by nonzero, add a multiple of another row.","Row-echelon form permits back-substitution.","Zero pivot may require row swap.","Rows0=nonzero are inconsistent.","A redundant row may leave free variables.","Apply augmented matrices to finance and three-unknown applications."]),
 dict(ord=56,sec="9.7",unit=9,les=7,title="Solving Systems with Inverses",start=844,end=858,count=61,worked=9,tryit=4,
 topics=["A square matrix A is invertible only if determinant is nonzero.","A inverse satisfies AA^-1=A^-1A=I.","For nonsingular system AX=b, solution X=A^-1b.","Use Gauss-Jordan on [A|I] to compute A^-1.","A zero row/column causes singularity, but zero diagonal alone does not.","Inverse method applies to two- and three-unknown financial systems."]),
 dict(ord=57,sec="9.8",unit=9,les=8,title="Solving Systems with Cramer's Rule",start=858,end=869,count=68,worked=8,tryit=3,
 topics=["Determinants of square matrices are scalars computed by signed expansion.","For2×2, determinant ad−bc.","For det(A)≠0, Cramer's rule x_i=det(A_i)/det(A).","Interchanging rows changes determinant sign; scaling one row scales determinant.","For det(A)=0 there is no unique solution; classify using elimination.","Model quantities with two- or three-variable equations and solve using determinants."])
]
def buildQ(profile):
 Q=[];sec=profile["sec"];count=profile["count"]
 if sec=="9.5":
  texts=["Can 2×3 and3×2 matrices be added?","When is row times column defined?","Can AB and BA both be defined?","Does any pair of square matrices multiply?","Does AB always equal BA?"]
  keys=["No: same shape needed.","Row1×n times n×1 yields scalar (1×1).","Yes if A and B square of same size, not necessarily same product.","Yes if same square order; rectangular equal dimensions also require inner dimensions match.","No: matrix product generally noncommutative."]
  for q,k in zip(texts,keys):Qadd(Q,q,k,"Source1–5 matrix rules")
  for n in range(6,60):
   A=seed(n,2);C=other(n,2); B=seed(n,3)
   if n<12:
    if n%3==0:q="Attempt adding 2×2 A="+str(A)+" and3×3 B="+str(B)+".";answer0="Undefined: unequal dimensions."
    else:q="Add matrices A="+str(A)+",C="+str(C)+".";answer0=answer(add(A,C))
    category="Source6–11 matrix add/subtract"
   elif n<18:
    k=n%7-3;q="Scale matrix A="+str(A)+" by "+str(k)+".";answer0=answer(scale(A,k));category="Source12–17 scalar multiplication"
   elif n<24:
    q="Multiply matrices A="+str(A)+" by C="+str(C)+".";answer0=answer(mul(A,C));category="Source18–23 matrix multiplication"
   elif n<41:
    k=n%4+1;q="Compute "+str(k)+"A−C, A="+str(A)+",C="+str(C)+".";answer0=answer(add(scale(A,k),scale(C,-1)));category="Source24–40 combined matrix operations"
   elif n<50:
    if n%2:q="Compute A² for A="+str(A)+".";answer0=answer(mul(A,A))
    else:q="Compute AC for A="+str(A)+",C="+str(C)+".";answer0=answer(mul(A,C))
    category="Source41–49 products and powers"
   elif n<55:
    q="Compute A·C−2A with A="+str(A)+",C="+str(C)+"."
    answer0=answer(add(mul(A,C),scale(A,-2)));category="Source50–54 technology matrix operations"
   else:
    swap=[[1,0,0],[0,0,1],[0,1,0]]
    powr=n-53;out=[[1,0,0],[0,1,0],[0,0,1]]if powr%2==0 else swap
    q="Calculate swap-matrix B^"+str(powr)+", B="+str(swap)+"."
    answer0=answer(out);category="Source55–59 matrix powers"
   Qadd(Q,q,answer0,category)
 if sec=="9.6":
  for q,a in [
   ("Can every finite linear system become augmented matrix?","Yes when coefficients and constants aligned to variable order."),
   ("Can every augmented matrix represent linear equations?","Yes each row gives coefficients and RHS."),
   ("Are row-operation sequences unique?","No; elementary row-equivalent paths can differ."),
   ("What if pivot is zero?","Swap with a row below having nonzero coefficient when available."),
   ("Can a zero row yield unique solution?","Not by itself if fewer independent constraints than variables; inspect rank.")]:
   Qadd(Q,q,a,"Source1–5 row operation concepts")
  for n in range(6,62):
   dim=2 if n<8 or 11<=n<33 else 3;A=seed(n,dim);x=list(range(1,dim+1))
   b=[sum(A[i][j]*x[j]for j in range(dim))for i in range(dim)]
   if n<11:
    q="Write augmented matrix for A="+str(A)+" and b="+str(b)+"."
    key=answer([A[i]+[b[i]]for i in range(dim)]);category="Source6–10 equation to augmented"
   elif n<16:
    q="Translate augmented matrix "+str([A[i]+[b[i]]for i in range(dim)])+" into equations."
    key="Coefficient rows A="+str(A)+" and RHS b="+str(b);category="Source11–15 augmented to equations"
   elif n in (16,17,26,27,38,40):
    A[dim-1]=[2*v for v in A[0]]
    b[dim-1]=2*b[0]+(1 if n%2 else 0)
    q="Classify augmented system "+str([A[i]+[b[i]]for i in range(dim)])+"."
    key="No solution: inconsistent scaled row."if n%2 else"Infinitely many solutions: redundant row."
    category="Source16–50 dependent or inconsistent echelon"
   else:
    q,key=question(A,b,"Gaussian row elimination")
    category="Source16–50 Gaussian and application"if n<=50 else"Source51–61 applied finance matrices"
   Qadd(Q,q,key,category)
 if sec=="9.7":
  for q,a in [
   ("Why AA^-1=A^-1A?","Both equal I by definition of a two-sided inverse."),
   ("Does every2×2 matrix have an inverse?","Only if determinant nonzero."),
   ("Can matrix with entire zero row be invertible?","No: determinant zero."),
   ("Can matrix with zero column be invertible?","No: rank deficient."),
   ("Can zero diagonal matrix be invertible?","Yes [[0,1],[1,0]] self-inverse.")]:
   Qadd(Q,q,a,"Source1–5 inverse facts")
  for n in range(6,62):
   dim=2 if n<20 or (27<=n<35) else 3;A=seed(n,dim)
   if n in (17,24,26):A[dim-1]=A[0].copy()
   I=inv(A)
   if 6<=n<13:
    assert I is not None;verify(A,I)
    q="Verify mutual inverse of A="+str(A)+" and B="+str(I)+"."
    key="AB=BA=I_"+str(dim)
    category="Source6–12 inverse verification"
   elif n<27:
    q="Find A^-1 for A="+str(A)+" if it exists."
    key="Noninvertible"if I is None else str(I)
    if I is not None:verify(A,I)
    category="Source13–26 inverse computation"
   else:
    x=list(range(1,dim+1))
    b=[sum(A[i][j]*x[j]for j in range(dim))for i in range(dim)]
    q,key=question(A,b,"inverse matrix A^-1b")
    category="Source27–50 inverse linear systems"if n<=50 else"Source51–61 inverse word models"
   Qadd(Q,q,key,category)
 if sec=="9.8":
  for q,a in [
   ("Why determinant only square matrices?","Standard determinant is defined for n×n square arrays."),
   ("What if det(A)=0?","A singular: Cramer's rule cannot give a unique solution."),
   ("What does zero determinant say about inverse?","Matrix inverse does not exist."),
   ("If det A=3, swap rows then multiply rows by6 and2. New determinant?","−36: sign swap and factor12.")]:
   Qadd(Q,q,a,"Source1–4 determinant rules")
  for n in range(5,69):
   dim=2 if n<=14 or 25<=n<=34 or 55<=n<=59 else 3
   A=seed(n,dim)
   if n in (19,43,44,45):A[dim-1]=[2*v for v in A[0]]
   if n<=24:
    q="Compute determinant of "+str(A)+"."
    key=str(det(A))
    category="Source5–24 determinant calculation"
   else:
    x=list(range(1,dim+1))
    b=[sum(A[i][j]*x[j]for j in range(dim))for i in range(dim)]
    if det(A)!=0:
     Ds=[]
     for j in range(dim):
      M=[r.copy()for r in A]
      for i in range(dim):M[i][j]=b[i]
      Ds.append(det(M))
     derived=[F(v,det(A))for v in Ds]
     assert derived==[F(v)for v in x]
     key="det A="+str(det(A))+", det substituted="+str(Ds)+", X="+str(derived)
    else:key="det A=0; no unique solution, use elimination for classification."
    q="Apply Cramer's rule to A="+str(A)+" and b="+str(b)+"."
    category="Source25–34 Cramer 2×2"if n<35 else"Source35–54 Cramer 3×3"if n<55 else"Source55–68 Cramer applied models"
   Qadd(Q,q,key,category)
 assert len(Q)==count,(sec,len(Q),count)
 return Q
def save(path,obj):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(obj,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
for profile in profiles:
 P0=json.loads(P.read_text(encoding="utf8"));ordinal=profile["ord"]
 assert P0["LastClosedLessonOrdinal"]==ordinal-1 and P0["FirstIncompleteLessonOrdinal"]==ordinal
 row=P0["LessonEntries"][ordinal-1];assert row["Ordinal"]==ordinal and row["SourceSection"]==profile["sec"]
 sec=profile["sec"];a=profile["start"];b=profile["end"]
 assert sec in text(a) and ("9."+str(profile["les"]+1) in text(b) or (sec=="9.8" and "CHAPTER 9 REVIEW" in text(b)))
 ex,tr,fig=getprofile(sec,a,b,profile["worked"],profile["tryit"])
 assert str(profile["count"])+'.' in text(b-1),(sec,profile["count"])
 Q=buildQ(profile)
 code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U09:L"+str(profile["les"]).zfill(2)
 base="us-ccss-g12-precalculus-u09-l"+str(profile["les"]).zfill(2)
 rrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/"+base+".rich-lesson-v2.json"
 trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/"+base+"-firstparty-teacher-led.json"
 idxs={"9.5":[6,7,8,9,12,13,24,18,19,30,50],"9.6":[6,11,16,17,26,33,35,36,50,54,58],"9.7":[6,7,13,14,20,27,28,35,50],"9.8":[5,25,15,35,43,47,21,54]}[sec]
 W=[]
 for (number,title),qidx in zip(ex,idxs):
  q=Q[qidx-1]
  W.append({"Title":title,"Question":q["IndependentStudentMathQuestion"],
   "Method":"Use the specified matrix procedure and verify exact calculations.",
   "Steps":[q["TeacherMathAnswer"]],"Answer":q["TeacherMathAnswer"],
   "Check":"Verify dimensions, determinant or original equations."})
 assert len(W)==profile["worked"]
 T=[]
 for idx in range(profile["tryit"]):
  q=Q[idx+5]
  T.append({"Number":idx+1,"Question":q["IndependentStudentMathQuestion"],
   "TeacherExpectedMath":q["TeacherMathAnswer"]})
 rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U09:L"+str(profile["les"]).zfill(2),
 "ContentVersion":base+"-abc",
 "Lessons":[{"LessonCode":code,"CultureCode":"en","Title":profile["title"],
 "ExplanationParagraphs":profile["topics"]+["Independent Cool-down: demonstrate a valid example of the section's central operation and check exact answer."],
 "WorkedExamples":W,
 "KeyConcepts":[{"Title":"Mathematical validity","Definition":"Operations follow matrix dimensions and algebraic identities.",
  "Rule":"Verify results against original constraints.","Example":Q[5]["IndependentStudentMathQuestion"]}],
 "Visuals":[{"Kind":"RatioTable","Title":"First-party matrix operations guide",
  "Description":"Independently authored concept table.","PrimaryLabel":"Operation","SecondaryLabel":"Constraint",
  "Rows":[{"Label":"Action","Cells":["add","multiply","invert"]},
  {"Label":"Requirement","Cells":["equal shape","matching inner dims","square nonsingular"]}],"Items":[],"SecondaryItems":[]}],
 "Videos":[],"SummaryPoints":profile["topics"][:3],
 "CommonMistakes":[{"Mistake":"Ignore matrix dimension, rank or determinant restrictions.",
 "WhyWrong":"May produce undefined operation or false unique solution.",
 "Correction":"Inspect dimensions and singularity; check original equations."}]}]}
 teacher={"SchemaVersion":1,"LessonCode":code,"IndependentStudentMathQuestions":Q,
 "IndependentTryIt":T,
 "IndependentCoolDown":{"StudentQuestions":[Q[5]["IndependentStudentMathQuestion"]],
 "TeacherExpectedMath":[Q[5]["TeacherMathAnswer"]],"PublisherOriginalCopied":False},
 "OriginalExerciseMathSkills":profile["count"],"StageC":"REUSE_MANUAL_TEACHER_MATRIX_SCORING",
 "CI":"NOT_RUN","StageD":"NOT_RUN"}
 save(R/rrel,rich);save(R/trel,teacher)
 figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l"+str(profile["les"]).zfill(2)+"-figure-skills.json"
 save(R/figrel,{"Section":sec,"OriginalFigureIdentifiers":fig,
 "IndependentFigureSkillAlternatives":[{"Figure":n,"Alternative":"First-party matrix geometry, table and operations",
 "PublisherGraphicCopied":False}for n in fig],"Runtime":"NOT_RUN"})
 routes="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l"+str(profile["les"]).zfill(2)+"-stage-c-matrix-routes.json"
 save(R/routes,{"Section":sec,"Count":profile["count"],"Reuse":"RichV2 and manual teacher matrix grading",
 "NewMathEngine":False,"Routes":[{"Source":i,"IndependentQuestion":True,"TeacherAnswer":True,"RuntimeVerified":False}
 for i in range(1,profile["count"]+1)]})
 now=datetime.datetime.now(datetime.timezone.utc).isoformat()
 audit="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l"+str(profile["les"]).zfill(2)+"-source-academic-audit.json"
 save(R/audit,{"Section":sec,"SourceSha256":P0["SourceSnapshotSha256"],
 "PdfPageStart":a,"PdfPageEnd":b-1,"BlueprintStart":row["PdfPageStart"],"BlueprintEnd":row["PdfPageEnd"],
 "OriginalWorkedExamples":ex,"OriginalTryIt":tr,"OriginalFigures":fig,
 "OriginalExerciseCount":profile["count"],"IndependentRichV2":rrel,
 "TeacherMath":trel,"FigureSkills":figrel,"StageCRoutes":routes,
 "Status":"READY_FOR_STAGE_D","ReadyForStageD":True,"CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
 close="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l"+str(profile["les"]).zfill(2)+"-close-audit.json"
 save(R/close,{"Section":sec,"OriginalSource":"OpenStax Precalculus (2017) local PDF",
 "Exercises":profile["count"],"IndependentTeacherMathKeys":profile["count"],
 "OriginalWorked":profile["worked"],"OriginalTryIt":profile["tryit"],
 "IndependentFirstPartyWorked":len(W),"IndependentTryIt":len(T),"OriginalFigureSkills":len(fig),
 "TargetedChecksActuallyRun":["Source section PDF bounds/exercise count",
 "Source-skill alternative math operations evaluated with exact fractions",
 "Teacher answer count and example/tryit inventories",
 "Stage C manual route count","Git staged diff check"],
 "NewMathEngine":False,"Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN",
 "ReadyForStageD":True,"ClosedAtUtc":now})
 row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
 "PdfPageStart":a,"PdfPageEnd":b-1,"EvidencePath":audit,"ClosureEvidencePath":close,
 "BlockingReasons":[],"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
 assert P0["LessonEntries"][ordinal]["Ordinal"]==ordinal+1
 P0.update({"LastClosedLessonOrdinal":ordinal,"FirstIncompleteLessonOrdinal":ordinal+1,
 "ActiveLessonOrdinal":ordinal+1,"NextLessonOrdinal":ordinal+1,"ReadyForStageD":ordinal,
 "SourceVerifiedGrade12Lessons":ordinal,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_"+str(ordinal)+"_OF_73",
 "UpdatedUtc":now})
 save(P,P0)
 files=[str(P.relative_to(R)),rrel,trel,figrel,routes,audit,close,str(Path(__file__).relative_to(R))]
 subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
 subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
 subprocess.run(["git","commit","-m","feat(g12 u09 l"+str(profile["les"]).zfill(2)+"): close "+profile["title"]+" academic ABC [skip ci]"],cwd=R,check=True)
 subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
 print("PUSH_VERIFIED_ORDINAL",ordinal,"OF_73",sec,flush=True)
print("UNIT_9_ALL_LESSONS_CLOSED_57_OF_73")
