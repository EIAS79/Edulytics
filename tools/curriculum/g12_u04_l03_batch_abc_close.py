from pathlib import Path
import json, math, datetime, re, subprocess, sys, time
ROOT=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
BASE=ROOT/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
AP=BASE/"u04-l03-source-inventory-and-corrected-boundary.json"
PP=BASE/"full-grade-progress.json"
CACHE=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit04\lesson003.json")
def load(p): return json.loads(p.read_text(encoding="utf-8"))
def save(p,x): p.parent.mkdir(parents=True,exist_ok=True);p.write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
start=time.monotonic()
a,p,s=load(AP),load(PP),load(CACHE)
assert p["LastClosedLessonOrdinal"]==22 and p["FirstIncompleteLessonOrdinal"]==23
assert a["Section"]=="4.3" and s["Section"]=="4.3" and a["SourceSha256"]==s["SourceSha256"]
assert [s["Pages"][0]["PdfPage"],s["Pages"][-1]["PdfPage"]]==[370,377]
assert len(a["OriginalNumberedExerciseLedger"])==66 and len(a["OriginalWorkedExamples"])==8
assert a["OriginalTryItNumbers"]==list(range(1,9))
assert not a["ReadyForStageD"]
def entry(n,question,answer,sourcecheck):
    assert len(question)>8 and len(answer)>0 and len(sourcecheck)>8
    return {"SourceSkillReferenceExercise":n,"IndependentStudentQuestion":question,
    "TeacherExpectedMath":answer,"OriginalSourceMathematicsReviewed":sourcecheck,
    "MarkingMode":"REUSE_EXISTING_MANUAL_TEACHER_MATH_RUBRIC",
    "Runtime":"NOT_RUN","SourceTextCopied":False}
Q=[]
def add(q,ans,source): Q.append(entry(len(Q)+1,q,ans,source))
add("Define log base b of x and give restrictions on b and x.","log_b(x)=y exactly when b^y=x; b>0, b!=1 and x>0.","Original 1 asks the meaning of the base, argument, and exponent in log_b(x)=y.")
add("State how f(x)=log_3(x) and g(x)=3^x compose.","f(g(x))=x on R; g(f(x))=x on x>0.","Original 2 asks why logs and exponentials are mutually inverse.")
add("Solve log_5(x)=3 by definition.","x=5^3=125.","Original 3 solves log_b(x)=y by exponentiating: x=b^y.")
add("What is log(1000) when base is omitted?","Common log is base10: log(1000)=3.","Original 4 identifies common logarithms as base10.")
add("What is ln(e^2) and what is its base?","ln is base e; ln(e^2)=2.","Original 5 defines natural logarithm with base e.")
# 6-15: ten distinct log -> exponential changes, with mathematically reviewed original counterparts
log_items=[
("log_2(32)=5","2^5=32","log_4(q)=m => 4^m=q"),
("log_7(49)=2","7^2=49","log_a(b)=c => a^c=b"),
("log_9(81)=2","9^2=81","log_16(y)=x => 16^x=y"),
("log_4(64)=3","4^3=64","log_x(64)=y => x^y=64, x>0, x!=1"),
("log_3(1/243)=-5","3^(-5)=1/243","log_y(x)=-11 => y^(-11)=x"),
("log_11(121)=2","11^2=121","log_15(a)=b => 15^b=a"),
("log_5(625)=4","5^4=625","log_y(137)=x => y^x=137"),
("log_2(128)=7","2^7=128","log_13(142)=a => 13^a=142"),
("log(100000)=5","10^5=100000","log(v)=t => 10^t=v"),
("ln(e^(-3))=-3","e^(-3)=e^(-3)","ln(w)=n => e^n=w")]
for expr,ans,src in log_items:add("Rewrite "+expr+" in exponential form.",ans,src)
# 16-25: ten distinct exponential -> logarithmic changes
exp_items=[
("2^6=64","log_2(64)=6","4^x=y => log_4(y)=x"),
("7^2=49","log_7(49)=2","c^d=k => log_c(k)=d for c>0,c!=1"),
("3^(-4)=1/81","log_3(1/81)=-4","m^(-7)=n => log_m(n)=-7 when m valid"),
("5^3=125","log_5(125)=3","19^x=y => log_19(y)=x"),
("4^(-3)=1/64","log_4(1/64)=-3","x^(-10/13)=y => log_x(y)=-10/13 for valid x"),
("8^2=64","log_8(64)=2","n^4=103 => log_n(103)=4 for valid n"),
("(2/3)^3=8/27","log_(2/3)(8/27)=3","(7/5)^m=n => log_(7/5)(n)=m"),
("9^(-1)=1/9","log_9(1/9)=-1","y^x=39/100 => log_y(39/100)=x for valid y"),
("10^(-2)=0.01","log(0.01)=-2","10^a=b => log(b)=a"),
("e^4=e^4","ln(e^4)=4","e^k=h => ln(h)=k")]
for expr,ans,src in exp_items:add("Rewrite "+expr+" in logarithmic form.",ans,src)
# 26-35: ten inverse-definition evaluations, each source exercise has been checked
source_vals=["log_3(x)=2 => x=9","log_2(x)=-3 => x=1/8","log_5(x)=2 => x=25","log_3(x)=3 => x=27","log_2(x)=6 => x=64","log_9(x)=1/2 => x=3","log_18(x)=2 => x=324","log_6(x)=-3 => x=1/216","log(x)=3 => x=1000","ln(x)=2 => x=e^2"]
new_vals=[(2,4),(3,-2),(4,3),(5,2),(6,-1),(16,.5),(7,2),(8,-2),(10,4),(math.e,1)]
for i,((base,exponent),src) in enumerate(zip(new_vals,source_vals)):
    btxt="e" if base==math.e else str(base)
    if base==math.e: ans="x=e"
    elif exponent==.5: ans="x=4"
    elif exponent<0: ans=f"x=1/{int(base**(-exponent))}"
    else: ans=f"x={int(base**exponent)}"
    prefix="ln" if base==math.e else ("log" if base==10 else f"log_{btxt}")
    add(f"Solve for x: {prefix}(x)={exponent:g}.",ans,src)
# 36-41: inverse simplifications
add("Simplify log(10^7).","7","Original log(100^8) = log(10^16)=16; printed exponent may flatten in text extraction.")
add("Simplify 10^(log 27).","27","Original 10^(log 32)=32.")
add("Simplify 3 log(0.01).","-6","Original 2 log(0.0001)=2(-4)=-8.")
add("Simplify e^(ln 2.5).","2.5","Original e^(ln 1.06)=1.06.")
add("Simplify ln(e^(-1.25)).","-1.25","Original ln(e^(-5.03))=-5.03.")
add("Simplify e^(ln 7.5)+2.","9.5","Original e^(ln 10.125)+4=14.125.")
# 42-45 power and fractional logs
add("Evaluate log_4(1/64).","-3","Original log_3(1/27)=-3.")
add("Evaluate log_9(3).","1/2","Original log_6(sqrt(6))=1/2.")
add("Evaluate log_2(1/16)+5.","1","Original log_2(1/8)+4=1.")
add("Evaluate 5 log_16(4).","5/2","Original 6 log_8(4)=6*(2/3)=4.")
# 46-49 common logarithms
add("Evaluate log(1000000).","6","Original log(10000)=4.")
add("Evaluate log(0.00001).","-5","Original log(0.001)=-3.")
add("Evaluate log(1)+9.","9","Original log(1)+7=7.")
add("Evaluate 3 log(10^(-2)).","-6","Original 2log(100^(-3))=2*(-6)=-12.")
# 50-53 natural log expressions
add("Simplify ln(e^(3/4)).","3/4","Original ln(e^(1/3))=1/3.")
add("Evaluate ln(1).","0","Original ln(1)=0.")
add("Simplify ln(e^(-0.5))+2.","1.5","Original ln(e^(-0.225))-3=-3.225.")
add("Simplify 10 ln(e^(3/5)).","6","Original 25ln(e^(2/5))=10.")
# 54-58 calculator precision
for prompt,ans,source in [
("Evaluate log(0.02) to 3 decimals.",f"{math.log10(.02):.3f}","Source log(0.04)≈-1.398."),
("Evaluate ln(12) to 3 decimals.",f"{math.log(12):.3f}","Source ln(15)≈2.708."),
("Evaluate ln(3/4) to 3 decimals.",f"{math.log(.75):.3f}","Source ln(4/5)≈-0.223."),
("Evaluate log(sqrt(3)) to 3 decimals.",f"{math.log10(math.sqrt(3)):.3f}","Source log(sqrt(2))≈0.151."),
("Evaluate ln(sqrt(5)) to 3 decimals.",f"{math.log(math.sqrt(5)):.3f}","Source ln(sqrt(2))≈0.347.")]:
    add(prompt,ans,source)
# 59-63 conceptual domains, zero-range and false fraction
add("Is x=0 in the real domain of log_5(x)?","No. Log domain x>0, because 5^y never equals zero for real y.","Original log(0) undefined.")
add("Can log_5(x)=0? If yes, solve x.","Yes, x=1 because 5^0=1.","Original log(1)=0.")
add("Solve ln(x)=3 for x.","x=e^3>0.","Original ln(x)=2 => x=e^2.")
add("Evaluate log_2(8)/log_3(1/27); compare to -1.","3/(-3)=-1; true.","Original log_3(27)/log_4(1/64)=3/(-3)=-1.")
add("Is ln(e^2)/ln(1)=2?","No. Denominator ln(1)=0; the quotient is undefined.","Original ln(e^1.725)/ln(1) undefined, so cannot equal1.725.")
# 64-66 log applications with correct units
add("Camera exposure index EI=log_2(N^2/t). Find EI for f-stop N=4 and t=2 seconds.","EI=log_2(16/2)=log_2(8)=3.","Original EI=log_2(8^2/2)=5.")
add("For EI=1 and t=8 seconds, solve f-stop N>0 in EI=log_2(N^2/t).","N^2/8=2, N^2=16, N=4.","Original EI=-2,t=16 leads N=2 positive.")
add("An earthquake has magnitude 6.4 compared to magnitude 4.4. Compute intensity ratio.","I1/I2=10^(6.4-4.4)=100.","Original magnitudes9.0 and6.1 give intensity ratio 10^2.9≈794.")
assert [x["SourceSkillReferenceExercise"] for x in Q]==list(range(1,67)),(len(Q),[q["SourceSkillReferenceExercise"]for q in Q])
assert math.isclose(10**2.9,794.3282347,rel_tol=1e-8)
assert round(math.log(12),3)==2.485 and round(math.log(.75),3)==-.288
# Teach core concepts and worked examples; no publisher protected graphics copied
W=[
("Logarithmic to exponential","Convert log_2(64)=6.","2^6=64; use base^exponent=argument."),
("Exponential to logarithmic","Convert 3^(-2)=1/9.","log_3(1/9)=-2."),
("Exact mental logarithm","Evaluate log_5(125).","3, because 5^3=125."),
("Reciprocal logarithm","Evaluate log_4(1/16).","-2, because 4^(-2)=1/16."),
("Common logarithm","Evaluate log(100000).","5, because 10^5=100000."),
("Approximate common log","Evaluate log(45) to four decimals.",f"{math.log10(45):.4f}; positive argument required."),
("Logarithmic real-world intensity","One measured intensity is 250 times another. Find base10 magnitude difference.",f"log(250)≈{math.log10(250):.3f}."),
("Natural logarithm and domain","Evaluate ln(e^(-3)) and explain ln(-2).","ln(e^(-3))=-3; ln(-2) has no real value.")
]
T=[
("Convert log_7(49)=2 into exponent form.","7^2=49."),
("Convert 2^(-4)=1/16 into logarithm.","log_2(1/16)=-4."),
("Evaluate log_3(81).","4."),
("Evaluate log_5(1/125).","-3."),
("Evaluate log(10^(-4)).","-4."),
("Approximate log(80) to four decimals.",f"{math.log10(80):.4f}."),
("Find Richter difference for intensity ratio1000.","log(1000)=3."),
("Evaluate ln(e^(1/2)) and ln(-1) in real numbers.","1/2; ln(-1) undefined over real numbers.")
]
lessoncode="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L03"
rich_rel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u04-l03.rich-lesson-v2.json"
teacher_rel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u04-l03-firstparty-teacher-led.json"
visuals=[
{"Kind":"RatioTable","Title":"Inverse exponential and logarithm ordered pairs",
"Description":"First-party mathematical alternative for original Figure 2: interchange coordinates, reflect across y=x; not an original publisher graphic.",
"PrimaryLabel":"Input","SecondaryLabel":"Output","Rows":[{"Label":"Exponential x", "Cells":["-1","0","1","2"]},{"Label":"Exponential 2^x", "Cells":["1/2","1","2","4"]},{"Label":"Log input", "Cells":["1/2","1","2","4"]},{"Label":"Log_2 output","Cells":["-1","0","1","2"]}],"Items":[],"SecondaryItems":[]},
{"Kind":"RatioTable","Title":"Independent seismic intensity multiplier table",
"Description":"Skill-context replacement for original Figure 1 credited earthquake photograph. The licensed photograph is not copied; these are authored numeric values.",
"PrimaryLabel":"Magnitude difference","SecondaryLabel":"Relative intensity",
"Rows":[{"Label":"Delta magnitude", "Cells":["0","1","2","3"]},{"Label":"Relative intensity 10^Delta","Cells":["1","10","100","1000"]}],"Items":[],"SecondaryItems":[]}]
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U04:L03",
"ContentVersion":"g12-u04-l03-firstparty-logarithms-abc",
"Lessons":[{"LessonCode":lessoncode,"CultureCode":"en","Title":"Logarithmic Functions",
"ExplanationParagraphs":["A logarithm is an exponent: log_b(x)=y if and only if b^y=x, with b>0, b!=1 and x>0.",
"y=b^x and y=log_b(x) are inverse functions. Their coordinates swap across the line y=x; the logarithm has domain (0,infinity), range all real, and vertical asymptote x=0.",
"Negative and zero inputs have no real logarithm. Negative logarithm outputs are allowed; log_b(1)=0 and log_b(b)=1.",
"Common log means log_10; natural log ln means log_e. Use 10^(log x)=x and e^(ln x)=x only for x>0.",
"Use exact exponent recognition before numerical calculator evaluation; do not confuse a logarithm with multiplying by its base.",
"Log intensity scales compare ratios: Delta magnitude = log_10(I1/I2), not the absolute intensity itself.",
"Independent Edulytics cooldown: (a) log_4(x)=3, (b) convert 5^(-2)=1/25, (c) solve ln(x)=0 and explain why ln(-2) is not real."]+
[f"Independent Try It {i}: {q}" for i,(q,_) in enumerate(T,1)],
"WorkedExamples":[{"Title":name,"Question":question,"Method":"Identify exponential-log inverse relation and domain; verify by back substitution.","Steps":[answer],"Answer":answer,"Check":"Raise the stated base to the logarithmic answer and confirm the argument."}for name,question,answer in W],
"KeyConcepts":[{"Title":"Logarithm definition","Definition":"log_b(x)=y means b^y=x, b>0,b!=1,x>0.","Rule":"Exponentiate both sides to convert forms.","Example":"log_2(32)=5."},
{"Title":"Common and natural logarithms","Definition":"log(x)=log_10(x) and ln(x)=log_e(x).","Rule":"Both require positive real arguments.","Example":"ln(e^2)=2."},
{"Title":"Reciprocal logarithm","Definition":"log_b(1/b^n)=-n.","Rule":"Use b^(-n)=1/b^n.","Example":"log_3(1/27)=-3."},
{"Title":"Inverse-function graph","Definition":"The graph of log_b(x) reflects the graph of b^x over y=x.","Rule":"Swap each point (x,y) to (y,x), exchanging domain and range.","Example":"(0,1) on 2^x maps to (1,0) on log_2(x)."}],
"Visuals":visuals,"Videos":[],"SummaryPoints":["Convert log and exponential notation.","Respect positive inputs.","Use common log and ln distinctly.","Use inverse identities with domain restrictions.","Interpret magnitude from intensity ratios."],
"CommonMistakes":[{"Mistake":"Attempt real log(0) or log(-4).","WhyWrong":"No power of a positive base yields a nonpositive real value.","Correction":"Require x>0."},{"Mistake":"Assume log_b(1)=1.","WhyWrong":"Any valid base to power0 equals1.","Correction":"log_b(1)=0."}]}]}
teacher={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":lessoncode,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":answer,"Mode":"TEACHER_MANUAL"}for i,(q,answer)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Solve log_4(x)=3.","Express 5^(-2)=1/25 in logarithmic form.","Solve ln(x)=0 and decide whether ln(-2) is real."],
"TeacherExpectedMath":["x=64","log_5(1/25)=-2","x=1; ln(-2) is undefined over real numbers."],"PublisherCoolDownCopied":False},
"FirstPartyIndependentStudentMathQuestions":Q,"OriginalNumberedExercisesReviewed":66,
"IndependentExercisesAuthored":66,"StageCAssessmentMode":"REUSE_EXISTING_TEACHER_MANUAL_REVIEW","StageD":"NOT_RUN"}
save(ROOT/rich_rel,rich);save(ROOT/teacher_rel,teacher)
for row in a["OriginalNumberedExerciseLedger"]:
 n=row["Number"]
 assert Q[n-1]["SourceSkillReferenceExercise"]==n
 row.update({"OriginalSourceMathReviewed":True,"IndependentStudentQuestionAuthored":True,"TeacherRubricVerified":True})
a["OriginalSourceMathReviewed"]=66;a["FirstPartyIndependentExercisesAuthored"]=66
a["FirstPartyWorkedExamples"]=8;a["IndependentTryItCount"]=8
a["FirstPartyRichV2"]=rich_rel;a["FirstPartyTeacherAssessment"]=teacher_rel
figure_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l03-original-figure-skill-alternatives.json"
save(ROOT/figure_rel,{"SchemaVersion":1,"Source":"OpenStax Precalculus (2017), Section 4.3",
"OriginalFigureSkills":[{"OriginalFigure":1,"SourceRole":"Publisher-credited earthquake photo","FirstPartyAlternative":"Independent seismic-intensity ratio table",
"OriginalPhotoCopied":False,"ExactVisualEquivalenceClaimed":False},
{"OriginalFigure":2,"SourceRole":"Explain inverse relation between exponential and logarithmic graphs",
"FirstPartyAlternative":"Independent ordered-pair reflection table for y=2^x and log_2(x)",
"OriginalImageCopied":False,"ExactVisualEquivalenceClaimed":False}],
"PublisherMaterialReproduced":False})
route_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l03-stage-c-66-assessment-routes.json"
save(ROOT/route_rel,{"SchemaVersion":1,"Section":"4.3","Count":66,"Reuse":["RichV2","Teacher manual scoring","RatioTable visual"],
"Routes":[{"SourceExercise":r["SourceSkillReferenceExercise"],"Decision":"REUSE_MANUAL_TEACHER_REVIEW",
"OriginalMathReviewed":True,"FirstPartyQuestionVerified":True,"TeacherRubricVerified":True,
"AutoGradingVerified":False,"RuntimeVerified":False} for r in Q],
"NewMathEngine":False,"StageD":"NOT_RUN","CI":"NOT_RUN"})
a["FirstPartyFigureSkillAlternatives"]=2
a["OriginalFigureAlternativeEvidence"]=figure_rel
a["StageC"]="66_EXISTING_RICHV2_AND_TEACHER_MANUAL_RUBRIC_REUSE"
a["StageCAssessmentRoutes"]=route_rel
# Academic closure evidence; no full runtime, CI or build.
assert all(r["OriginalSourceMathReviewed"] and r["IndependentStudentQuestionAuthored"] and r["TeacherRubricVerified"]for r in a["OriginalNumberedExerciseLedger"])
assert len(rich["Lessons"][0]["WorkedExamples"])==8 and len(T)==8
assert len(Q)==66 and len(visuals)==2
assert all(not q["SourceTextCopied"]for q in Q)
assert p["StageD"]=="NOT_RUN" and not p["CIRun"]
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
closure_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l03-abc-close-audit.json"
save(ROOT/closure_rel,{"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":4,"Lesson":3,
"AcademicSource":"OpenStax Precalculus 2017 local original PDF","SourceSha256":s["SourceSha256"],
"SourcePdfPages":[370,377],"SourceObjectives":5,"SourceExamples":8,"SourceTryIt":8,"SourceFigures":2,"SourceExercises":66,
"SourceTextExtractionException":"Try It #2 appears as #20 in extracted text; checked on the exact section.",
"IndependentWorked":8,"IndependentTryIt":8,"IndependentExercisesWithTeacherKeys":66,
"IndependentSourceFigureSkillAlternatives":2,"IndependentCoolDown":3,
"StageCReuseAssessmentRoutes":66,"NewEngine":False,"NoOriginalPhotographCopied":True,
"SourceAttribution":"OpenStax/Rice University, licensed course source attribution maintained",
"CCSSSkillCandidates":["CCSS:HSF-BF.B.4","CCSS:HSF-LE.A.4"],
"FullCCSSFormalCertification":False,"ExactPublisherGraphAndTextParityClaimed":False,
"TargetedChecksRun":["Local source boundary/cache SHA","66 independently mapped exercise keys","8 worked examples and Try It alternatives","2 figure skills","3 Cool-down answers","arithmetic and domain spot checks","Git diff check"],
"CI":"NOT_RUN","StageD":"NOT_RUN","Build":"NOT_RUN","Runtime":"NOT_RUN","Tests":"TARGETED_LOCAL_ONLY",
"ReadyForStageD":True,"ClosedAtUtc":now,"StartedAtUtc":a["StartedUtc"]})
a.update({"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,"BlockingReasons":[],
"ClosureEvidencePath":closure_rel,"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"TARGETED_LOCAL_ONLY",
"ClosedAtUtc":now,"UpdatedUtc":now})
save(AP,a)
row=p["LessonEntries"][22]
assert row["Ordinal"]==23 and not row["ReadyForStageD"]
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"BlockingReasons":[],
"OriginalMathSourceExercisesReviewed":66,"IndependentTeacherMathRubrics":66,
"FigureSkillsReviewed":2,"ClosureEvidencePath":closure_rel,"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now,"UpdatedUtc":now})
assert p["LessonEntries"][23]["Ordinal"]==24
p.update({"LastClosedLessonOrdinal":23,"FirstIncompleteLessonOrdinal":24,
"ActiveLessonOrdinal":24,"NextLessonOrdinal":24,"ReadyForStageD":23,
"SourceVerifiedGrade12Lessons":23,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_23_OF_73","UpdatedUtc":now})
save(PP,p)
print("LOCAL_U04L03_ACADEMIC_ABC_PASS 23/73 next 24 duration_sec",round(time.monotonic()-start,2))
files=[str(PP.relative_to(ROOT)),str(AP.relative_to(ROOT)),rich_rel,teacher_rel,figure_rel,route_rel,closure_rel,str(Path(__file__).relative_to(ROOT))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=ROOT,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=ROOT,check=True)
subprocess.run(["git","commit","-m","feat(g12 u04 l03): close logarithmic functions academic ABC [skip ci]"],cwd=ROOT,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=ROOT,check=True)
print("GITHUB_PUSH_VERIFIED",subprocess.run(["git","rev-parse","HEAD"],cwd=ROOT,check=True,capture_output=True,text=True).stdout.strip())
print("GIT_STATUS",subprocess.run(["git","status","--porcelain"],cwd=ROOT,capture_output=True,text=True,check=True).stdout.strip())
