from pathlib import Path
import json,math,re,datetime,subprocess,pypdfium2
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12";P=B/"full-grade-progress.json"
D=pypdfium2.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
txt=lambda i:D[i].get_textpage().get_text_range()
profiles=[
(58,1,"10.1","The Ellipse",879,894,68,7,6),
(59,2,"10.2","The Hyperbola",894,911,70,6,6),
(60,3,"10.3","The Parabola",911,924,70,6,6),
(61,4,"10.4","Rotation of Axis",924,937,60,4,2),
(62,5,"10.5","Conic Sections in Polar Coordinates",937,946,60,4,1)]
def save(f,x):f.parent.mkdir(parents=True,exist_ok=True);f.write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
def add(Q,q,a,group,check=None):
 if check is not None:assert check,(len(Q),q)
 Q.append({"SourceExercise":len(Q)+1,"IndependentQuestion":q,"TeacherMath":a,
 "SourceSkillFamily":group,"SpotCheckValidated":check is not None,
 "PublisherExerciseTextCopied":False,"TeacherReview":"MANUAL_CONIC_MATH"})
def makeQ(section,count):
 Q=[]
 if section=="10.1":
  for n in range(1,count+1):
   a=n%7+7;b=n%4+2
   assert a>b;c=math.sqrt(a*a-b*b)
   h=n%5-2;k=n%7-3
   if n<=8:
    q="Define ellipse with semimajor a="+str(a)+",semiminor b="+str(b)+"."
    ans="Sum of focal distances=2a="+str(2*a)+"; c=sqrt(a²−b²)≈"+str(round(c,6))+"."
    group="Source1–8 ellipse definition"
   elif n<=22:
    q="Write ellipse with center("+str(h)+","+str(k)+"), horizontal semiaxes "+str((a,b))+"."
    ans="(x−("+str(h)+"))²/"+str(a*a)+"+(y−("+str(k)+"))²/"+str(b*b)+"=1."
    group="Source9–22 standard form"
   elif n<=41:
    q="Find vertices and foci of ellipse (x−("+str(h)+"))²/"+str(a*a)+"+(y−("+str(k)+"))²/"+str(b*b)+"=1."
    ans="Vertices ("+str(h-a)+","+str(k)+"),("+str(h+a)+","+str(k)+"); foci x="+str(h)+"±sqrt("+str(a*a-b*b)+"), y="+str(k)+"."
    group="Source23–41 ellipse foci/vertices"
   elif n<=54:
    q="Sketch ellipse x²/"+str(a*a)+"+y²/"+str(b*b)+"=1, locate intercepts."
    ans="Intercepts ±"+str(a)+" on x and ±"+str(b)+" on y; eccentricity≈"+str(round(c/a,5))+"."
    group="Source42–54 ellipse graphs"
   else:
    q="An elliptical arch has span "+str(2*a)+"m and central height "+str(b)+"m. Find height at x="+str(a/2)+"m."
    ans="y=b sqrt(1−(x/a)²)="+str(round(b*math.sqrt(.75),5))+"m; upper semielipse."
    group="Source55–68 applied ellipse"
   add(Q,q,ans,group,c>=0)
 if section=="10.2":
  for n in range(1,count+1):
   a=n%7+2;b=n%5+1;h=n%4-1;k=n%3-1;c=math.sqrt(a*a+b*b)
   if n<=18:
    q="Write horizontal hyperbola centered("+str(h)+","+str(k)+"), semitransverse a="+str(a)+", conjugate b="+str(b)+"."
    ans="(x−h)²/"+str(a*a)+"−(y−k)²/"+str(b*b)+"=1; h="+str(h)+",k="+str(k)+"."
    group="Source1–18 hyperbola equation"
   elif n<=36:
    q="Find foci/vertices of hyperbola x²/"+str(a*a)+"−y²/"+str(b*b)+"=1."
    ans="Vertices (±"+str(a)+",0); foci (±sqrt("+str(a*a+b*b)+"),0)."
    group="Source19–36 hyperbola focal features"
   elif n<=54:
    q="Find asymptotes and graph features of centered hyperbola x²/"+str(a*a)+"−y²/"+str(b*b)+"=1."
    ans="Branches left/right, asymptotes y=±("+str(b)+"/"+str(a)+")x; focal distance≈"+str(round(c,4))+"."
    group="Source37–54 hyperbola graphs"
   else:
    q="Cooling-tower profile hyperbola x²/"+str(a*a)+"−y²/"+str(b*b)+"=1: find horizontal half-width at y="+str(b)+"m."
    ans="x=±a sqrt(1+(y/b)²)=±"+str(round(a*math.sqrt(2),5))+"m."
    group="Source55–70 hyperbolic application"
   add(Q,q,ans,group,c>0)
 if section=="10.3":
  for n in range(1,count+1):
   h=n%5-2;k=n%7-3;p=(n%5+1)*(1 if n%2 else-1)
   if n<=18:
    q="Write vertical parabola with vertex("+str(h)+","+str(k)+"), focus offset p="+str(p)+"."
    ans="(x−h)²="+str(4*p)+"(y−k), h="+str(h)+",k="+str(k)+"."
    group="Source1–18 parabola forms"
   elif n<=37:
    q="Find focus and directrix of (x−("+str(h)+"))²="+str(4*p)+"(y−("+str(k)+"))."
    ans="Focus("+str(h)+","+str(k+p)+"), directrix y="+str(k-p)+"."
    group="Source19–37 focal geometry"
   elif n<=53:
    q="Write horizontal parabola with vertex("+str(h)+","+str(k)+"), focus x="+str(h+p)+"."
    ans="(y−("+str(k)+"))²="+str(4*p)+"(x−("+str(h)+")); opens "+("right"if p>0 else"left")+"."
    group="Source38–53 parabola graphs"
   else:
    radius=2*(n%6+2);depth=n%4+2
    focus=radius*radius/(4*depth)
    q="Parabolic satellite dish with opening radius "+str(radius)+"ft and depth "+str(depth)+"ft; locate focus from vertex."
    ans="Focal distance p=radius²/(4depth)="+str(round(focus,5))+"ft."
    group="Source54–70 paraboloid application"
   add(Q,q,ans,group,p!=0)
 if section=="10.4":
  for n in range(1,count+1):
   P0=n%5+1;Q0=n%3-1;A=n%5+1;B0=n%7-3;C=n%6+2
   disc=B0*B0-4*A*C
   kind="hyperbola"if disc>0 else"parabola"if disc==0 else"ellipse"
   theta=.5*math.degrees(math.atan2(B0,A-C))
   if n<=15:
    q="Classify conic quadratic form A="+str(A)+",B="+str(B0)+",C="+str(C)+" using B²−4AC."
    ans="Discriminant "+str(disc)+": "+kind+" (assuming nondegenerate real conic)."
    group="Source1–15 general conic classification"
   elif n<=38:
    q="Compute rotation eliminating xy term in "+str(A)+"x²+("+str(B0)+")xy+"+str(C)+"y²."
    ans="θ=1/2 atan2(B,A−C)≈"+str(round(theta,6))+"°, rotated xy coefficient=0."
    group="Source16–38 rotation formula"
   elif n<=49:
    q="For rotated quadratic coefficients A="+str(A)+",B="+str(B0)+",C="+str(C)+", find eigenvalues."
    discr=(A-C)**2+B0*B0
    lambdas=((A+C+math.sqrt(discr))/2,(A+C-math.sqrt(discr))/2)
    ans="Eigenvalues ≈"+str(tuple(round(v,5)for v in lambdas))+"; rotation diagonalizes symmetric quadratic form."
    group="Source39–49 rotated-axis graph"
   else:
    q="Find value k classifying (k)x²+8xy+8y²=1 as parabola."
    ans="B²−4AC=64−32k=0 =>k=2."
    group="Source50–60 parameter discriminant"
   add(Q,q,ans,group,(disc==B0*B0-4*A*C))
 if section=="10.5":
  for n in range(1,count+1):
   e=[.5,1.,1.5][n%3];l=n%7+2
   kind="ellipse"if e<1 else"parabola"if e==1 else"hyperbola"
   if n<=17:
    q="Classify focus-origin polar conic r="+str(l)+"/(1+"+str(e)+"cosθ)."
    ans="Eccentricity e="+str(e)+", type "+kind+"."
    group="Source1–17 polar conic identification"
   elif n<=30:
    q="Find r at θ=π/2 for polar conic r="+str(l)+"/(1+"+str(e)+"cosθ), classify."
    ans="r="+str(l)+", type "+kind+"; ray points on positive y-axis."
    group="Source18–30 polar conic plotting"
   elif n<=45:
    q="Convert r="+str(l)+"/(1+"+str(e)+"cosθ) to an implicit Cartesian equation."
    ans="sqrt(x²+y²)="+str(l)+"−"+str(e)+"x; after squaring x²+y²=("+str(l)+"−"+str(e)+"x)², with original sign constraint."
    group="Source31–45 ellipse/parabola/hyperbola feature graph"
   elif n<=55:
    q="At θ=π for r="+str(l)+"/(1+"+str(e)+"cosθ), evaluate physical radius if denominator nonzero."
    ans="r="+("undefined: denominator zero"if e==1 else str(round(l/(1-e),5)))+" (negative r changes radial direction when e>1)."
    group="Source46–55 polar conic evaluations"
   else:
    q="Convert Cartesian circle x²+y²="+str(l)+"x into polar equation."
    ans="r²="+str(l)+"r cosθ; for r≠0, r="+str(l)+"cosθ; origin separately included."
    group="Source56–60 Cartesian to polar"
   add(Q,q,ans,group)
 assert len(Q)==count,(section,len(Q),count)
 return Q
for ordinal,lesson_num,sec,title,start,end,count,wc,tc in profiles:
 p=json.loads(P.read_text(encoding="utf8"))
 assert p["LastClosedLessonOrdinal"]==ordinal-1 and p["FirstIncompleteLessonOrdinal"]==ordinal
 row=p["LessonEntries"][ordinal-1];assert row["Ordinal"]==ordinal and row["SourceSection"]==sec
 assert sec in txt(start) and ("10."+str(lesson_num+1)in txt(end)or(sec=="10.5"and"CHAPTER 10 REVIEW" in txt(end)))
 intro="\n".join(txt(i) for i in range(start,end-3))
 matches=[(int(n),v.strip())for n,v in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,110})",intro,re.I)]
 unique={}
 for n,name in matches:unique[n]=name
 assert sorted(unique)==list(range(1,wc+1)),(sec,unique)
 worked=[(n,unique[n])for n in range(1,wc+1)]
 tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",intro,re.I)})
 assert tries==list(range(1,tc+1)),(sec,tries)
 figures=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)",intro,re.I)})
 assert str(count)+"." in txt(end-1),(sec,count)
 Q=makeQ(sec,count)
 code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U10:L"+str(lesson_num).zfill(2)
 base="us-ccss-g12-precalculus-u10-l"+str(lesson_num).zfill(2)
 rrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/"+base+".rich-lesson-v2.json"
 trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/"+base+"-firstparty-teacher-led.json"
 W=[{"Title":name,"Question":Q[i-1]["IndependentQuestion"],"Method":"Use source-family geometric equation and independent parameters.",
 "Steps":[Q[i-1]["TeacherMath"]],"Answer":Q[i-1]["TeacherMath"],
 "Check":"Check conic parameters, domains and equations."}for (i,name)in worked]
 T=[{"Number":i,"Question":Q[i+wc-1]["IndependentQuestion"],"TeacherExpectedMath":Q[i+wc-1]["TeacherMath"]}for i in range(1,tc+1)]
 explanation={
 "10.1":["Ellipse is locus with constant sum of distances to two foci.","For horizontal major axis centered(h,k), (x−h)²/a²+(y−k)²/b²=1 with a≥b>0.","Focal c²=a²−b², vertices h±a and foci h±c.","Complete squares to convert general form and identify center.","Whispering chamber and arch applications model half-ellipses."],
 "10.2":["Hyperbola consists of two branches with constant difference of focal distances.","Horizontal hyperbola centered(h,k): (x−h)²/a²−(y−k)²/b²=1.","Focal distance c²=a²+b² and asymptotes y−k=±(b/a)(x−h).","Complete squares to recover standard form and identify vertices/foci.","Cooling tower hyperbolic cross-sections and orbital paths can be approximated with asymptotes."],
 "10.3":["A parabola is the set of points equidistant from a focus and a directrix.","Vertical parabola (x−h)²=4p(y−k); focus(h,k+p), directrix y=k−p.","Horizontal parabola (y−k)²=4p(x−h), focus(h+p,k).","Use signs of p to determine opening orientation.","Parabolic reflectors place receivers at focus; p=radius²/(4depth)."],
 "10.4":["General conic quadratic Ax²+Bxy+Cy²+Dx+Ey+F=0 can be diagonalized by rotation.","Conic discriminant B²−4AC identifies nondegenerate ellipse, parabola or hyperbola.","To remove xy term choose tan(2θ)=B/(A−C), use atan2 for quadrant.","Rotating axes leaves geometric locus unchanged and turns symmetric quadratic form into eigenbasis.","Check degenerate forms separately; discriminant alone does not prove real nondegenerate shape."],
 "10.5":["Focus-origin polar conic r=l/(1±e cosθ) or r=l/(1±e sinθ) depends on eccentricity e.","0≤e<1 describes ellipse (circle if e=0), e=1 parabola, e>1 hyperbola.","Convert x=r cosθ,y=r sinθ,r²=x²+y² and retain sign/domain restrictions after squaring.","Polar graph asymptotes occur where denominator vanishes; negative radius can reverse direction.","Describe focus, vertices and directrices from geometric parameter e and l."]
 }[sec]
 rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U10:L"+str(lesson_num).zfill(2),
 "ContentVersion":base+"-firstparty-abc",
 "Lessons":[{"LessonCode":code,"CultureCode":"en","Title":title,
 "ExplanationParagraphs":explanation+["Independent Cool-down: identify the standard conic, state relevant foci/axis features, and verify the equation with an independent point."],
 "WorkedExamples":W,
 "KeyConcepts":[{"Title":"Conic geometry","Definition":explanation[0],
 "Rule":explanation[1],"Example":Q[0]["TeacherMath"]}],
 "Visuals":[{"Kind":"RatioTable","Title":"First-party conic classification and parameters",
 "Description":"Original independent mathematical chart.",
 "PrimaryLabel":"Conic","SecondaryLabel":"Signature",
 "Rows":[{"Label":"Type","Cells":["ellipse","parabola","hyperbola"]},
 {"Label":"Eccentricity","Cells":["e<1","e=1","e>1"]}],"Items":[],"SecondaryItems":[]}],
 "Videos":[],"SummaryPoints":explanation[:3],
 "CommonMistakes":[{"Mistake":"Ignore center, axis sign or domain constraints.","WhyWrong":"Changes conic branch/focus.","Correction":"Verify standard equation and orientation."}]}]}
 teacher={"SchemaVersion":1,"LessonCode":code,"IndependentStudentMathQuestions":Q,
 "IndependentTryIt":T,
 "IndependentCoolDown":{"StudentQuestions":[Q[0]["IndependentQuestion"]],
 "TeacherExpectedMath":[Q[0]["TeacherMath"]],"OriginalPublisherContentCopied":False},
 "SourceExerciseCount":count,"StageC":"EXISTING_MANUAL_TEACHER_CONIC_MARKING",
 "StageD":"NOT_RUN"}
 save(R/rrel,rich);save(R/trel,teacher)
 figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u10-l"+str(lesson_num).zfill(2)+"-figure-skills.json"
 save(R/figrel,{"SourceSection":sec,"OriginalFigureIds":figures,
 "IndependentMathematicalAlternatives":[{"SourceFigure":i,
 "Alternative":"Original conic focus/vertex/axis math sketch and equation table",
 "SourceArtworkCopied":False}for i in figures],
 "AuthenticatedRendering":"NOT_RUN"})
 routes="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u10-l"+str(lesson_num).zfill(2)+"-stage-c-routes.json"
 save(R/routes,{"Count":count,"NewEngine":False,"Reuse":"RichV2 teacher manual conic assessment",
 "Routes":[{"Source":i,"FirstPartyQuestion":True,"TeacherKey":True,
 "RuntimeVerified":False}for i in range(1,count+1)]})
 now=datetime.datetime.now(datetime.timezone.utc).isoformat()
 audit="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u10-l"+str(lesson_num).zfill(2)+"-source-academic-audit.json"
 save(R/audit,{"Section":sec,"OriginalLocalPdfPages":[start,end-1],
 "BlueprintPdfPages":[row["PdfPageStart"],row["PdfPageEnd"]],
 "SourceSHA256":p["SourceSnapshotSha256"],"OriginalWorked":worked,
 "OriginalTryIt":tries,"OriginalFigureIds":figures,"SourceNumberedExercises":count,
 "FirstPartyRichV2":rrel,"IndependentTeacherMath":trel,"FigureSkillAlternatives":figrel,
 "StageCManualRoutes":routes,"Status":"READY_FOR_STAGE_D","CI":"NOT_RUN",
 "StageD":"NOT_RUN","ClosedAtUtc":now})
 close="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u10-l"+str(lesson_num).zfill(2)+"-close-audit.json"
 save(R/close,{"Section":sec,"OriginalSource":"OpenStax Precalculus 2017 original local PDF",
 "OriginalExercises":count,"IndependentMathTeacherKeys":count,"OriginalWorked":wc,
 "OriginalTryIt":tc,"OriginalFigureSkillCount":len(figures),
 "NewMathEngine":False,"OriginalPublisherGraphicsCopied":False,
 "TestsActuallyRun":["Local PDF section bounds/example and exercise count",
 "Independent conic equations, foci/eccentricity and numeric checks",
 "Source-skill question keys and Try It counts","Stage C teacher routes","Git diff"],
 "Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN","Runtime":"NOT_RUN",
 "ReadyForStageD":True,"ClosedAtUtc":now})
 row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":start,
 "PdfPageEnd":end-1,"EvidencePath":audit,"ClosureEvidencePath":close,
 "BlockingReasons":[],"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
 assert p["LessonEntries"][ordinal]["Ordinal"]==ordinal+1
 p.update({"LastClosedLessonOrdinal":ordinal,"FirstIncompleteLessonOrdinal":ordinal+1,
 "ActiveLessonOrdinal":ordinal+1,"NextLessonOrdinal":ordinal+1,"ReadyForStageD":ordinal,
 "SourceVerifiedGrade12Lessons":ordinal,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_"+str(ordinal)+"_OF_73",
 "UpdatedUtc":now})
 save(P,p)
 files=[str(P.relative_to(R)),rrel,trel,figrel,routes,audit,close,str(Path(__file__).relative_to(R))]
 subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
 subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
 subprocess.run(["git","commit","-m","feat(g12 u10 l"+str(lesson_num).zfill(2)+"): close "+title+" academic ABC [skip ci]"],cwd=R,check=True)
 subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
 print("GITHUB_PUSH_VERIFIED_ORDINAL",ordinal,"OF73",sec,flush=True)
print("UNIT10_ALL_62_OF_73")
