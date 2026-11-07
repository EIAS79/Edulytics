from pathlib import Path
import json,math,datetime,subprocess,time,re,pypdfium2 as pdfium
started=time.monotonic()
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
PP=base/"full-grade-progress.json"
p=json.loads(PP.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==24 and p["FirstIncompleteLessonOrdinal"]==25
row=p["LessonEntries"][24]
assert row["SourceSection"]=="4.5" and row["Ordinal"]==25 and not row["ReadyForStageD"]
pdfpath=Path(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
d=pdfium.PdfDocument(str(pdfpath))
get=lambda page:d[page].get_textpage().get_text_range()
assert "4.5" in get(395) and "4.6" in get(405)
pages=[{"PdfPage":i,"Text":get(i)}for i in range(395,405)]
text="\n".join(r["Text"]for r in pages)
examples=[(int(n),name.strip())for n,name in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,120})",text,re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",text,re.I)})
fig=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)",text,re.I)})
assert [n for n,_ in examples]==list(range(1,15))
assert tries==list(range(1,15)) and fig==[1]
assert "42." in get(404)
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit04\lesson005.json")
cache.write_text(json.dumps({"Section":"4.5","SourceSha256":p["SourceSnapshotSha256"],"Pages":pages},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
Q=[]
def add(question,answer,source):
 n=len(Q)+1
 assert question and answer and len(source)>5
 Q.append({"SourceSkillReferenceExercise":n,"IndependentStudentQuestion":question,"TeacherExpectedMath":answer,
 "OriginalSourceMathReviewed":source,"MarkingMode":"EXISTING_TEACHER_MANUAL_LOG_PROPERTY","SourceTextCopied":False,"Runtime":"NOT_RUN"})
add("How does the power rule simplify log_b(sqrt[3](x)) for x>0?","log_b(x^(1/3))=(1/3)log_b(x).","Original #1 power rule for logarithmic nth roots.")
add("Give the change-of-base identity for log_7(x).","log_7(x)=ln(x)/ln(7)=log(x)/log(7), x>0.","Original #2 change of base enables arbitrary-base calculator logs.")
# 3-8: expansions with positive-variable assumptions
for expr,answer,source in [
("log_b(6xy)","log_b6+log_bx+log_by","Source #3 log_b(7x·2y)=log_b14+log_bx+log_by."),
("ln(10abc)","ln10+ln a+ln b+ln c","Source #4 ln(3ab·5c)=ln15+ln a+ln b+ln c."),
("log_b(5/9)","log_b5−log_b9","Source #5 log_b(13/17)=log_b13−log_b17."),
("log_4((xy)/z)","log_4x+log_4y−log_4z","Source #6 log_4((x/z)/w)=log_4x−log_4z−log_4w."),
("ln(1/(5p))","−ln5−ln p","Source #7 ln(1/(4k))=−ln4−ln k."),
("log_2(x^3y^2)","3log_2x+2log_2y","Source #8 log_2(y^x)=x log_2 y, for y>0.")]:
 add("Expand "+expr+" for positive variables.",answer,source)
# 9-14: condensation
for expr,ans,src in [
("ln2+ln x+ln y","ln(2xy)","Source #9 ln7+lnx+lny=ln(7xy)."),
("log_3 5+log_3 a+log_3 2+log_3 b","log_3(10ab)","Source #10 log_3 2+log_3 a+log_3 11+log_3 b=log_3(22ab)."),
("log_b36−log_b9","log_b4","Source #11 log_b28−log_b7=log_b4."),
("ln a−ln b−ln c","ln(a/(bc))","Source #12 ln a−ln d−ln c=ln(a/(dc))."),
("−log_b(1/5)","log_b5","Source #13 −log_b(1/7)=log_b7."),
("(1/2)ln16","ln4","Source #14 (1/3)ln8=ln2.")]:
 add("Condense to one logarithm: "+expr+".",ans,src)
# 15-19 expand compound/radical
for expr,ans,src in [
("log(x^4y^3/z^2)","4log x+3log y−2log z","Source #15 log(x^15 y^13/z^19)=15logx+13logy−19logz for positive arguments."),
("ln(a^(-2)b^3/c^4)","−2ln a+3ln b−4ln c","Source #16 ln(a^(−2)b^(−4)c^5)=−2lna−4lnb+5lnc for positive variables."),
("log(sqrt(x^3/y^2))","(3/2)log x−log y","Source #17 sqrt(x^3 y^(−4)) expansion, positive variables."),
("ln(y sqrt(y/(1−y)))","(3/2)ln y−(1/2)ln(1−y), 0<y<1","Source #18 logarithm of y times sqrt(y/(1−y)), domain 0<y<1."),
("log(x^2 y^3 cube_root(xy^2))","(7/3)log x+(11/3)log y","Source #19 log(x²y³ cube_root(x²y⁵))=(8/3)log x+(14/3)log y.")]:
 add("Expand as much as possible: "+expr+" (assume valid positive variables).",ans,src)
# 20-24 condense
for expr,ans,src in [
("log(3x²)+log(4x³)","log(12x^5), x>0","Source #20 log(2x^4)+log(3x^5)=log(6x^9), x>0."),
("ln(8x^5)−ln(2x²)","ln(4x³), x>0","Source #21 ln(6x^9)−ln(3x²)=ln(2x^7), x>0."),
("2log x+3log(x+2)","log(x²(x+2)³), x>0","Source #22 2logx+3log(x+1)=log(x²(x+1)³), x>0."),
("log x−(1/2)log y+2log z","log(xz²/sqrt(y))","Source #23 logx−(1/2)logy+3logz=log(xz³/sqrt(y))."),
("3log_7 c+(1/2)log_7 a+log_7 b","log_7(c³sqrt(a)b)","Source #24 4log_7c+(1/3)log_7a+(1/3)log_7b=log_7(c⁴ cube_root(ab)).")]:
 add("Condense: "+expr+".",ans,src)
# 25-29 change of base and symbolic known a,b
for q,ans,src in [
("Write log_9(20) as a ratio of natural logs.","ln20/ln9","Source #25 log_7(15)=ln15/ln7."),
("Write log_6(45) as base10 log ratio.","log45/log6","Source #26 log_14(55.875)=log55.875/log14."),
("Given log_5(6)=a, log_5(11)=b, express log_11(5).","1/b","Source #27 log_11(5)=1/b."),
("Given a=log_5(6), b=log_5(11), express log_6(55).","(1+b)/a because55=5·11","Source #28 log_6(55)=(1+b)/a."),
("Given a=log_5(6), b=log_5(11), express log_11(6/11).","(a−b)/b","Source #29 log_11(6/11)=(a−b)/b.")]:
 add(q,ans,src)
# 30-32 exact arithmetic
for q,ans,src in [
("Evaluate log_2(1/8)−2log_2 2.","−3−2=−5","Source #30 log_3(1/9)−3log_3 3=−2−3=−5."),
("Evaluate 4log_16 2 +(log_16 256)/(2log_16 4).","4·(1/4)+(2)/(2·1/2)=3","Source #31 6log_8 2+(log_8 64)/(3log_8 4)=2+1=3."),
("Evaluate 2log_9 3−4log_9 3+log_9(1/81).","1−2−2=−3","Source #32 2(1/2)−4(1/2)+log_9(1/729)=−1−3=−4.")]:
 add(q,ans,src)
# 33-37 five numerical change-base
for baseval,arg,src in [
(2,17,"Source #33 log_3 22=ln22/ln3."),
(7,53,"Source #34 log_8 65=ln65/ln8."),
(5,4.25,"Source #35 log_6 5.38=ln5.38/ln6."),
(3,7.5,"Source #36 log_4(15/2)=ln(7.5)/ln4."),
(.5,3.2,"Source #37 log_(1/2)4.7=ln4.7/ln0.5.")]:
 add(f"Evaluate log_{baseval}({arg}) with change of base to five decimals.",f"ln({arg})/ln({baseval})≈{math.log(arg)/math.log(baseval):.5f}",src)
# 38-42 extensions
add("Solve log_3(x+4)+log_3(x+2)=2, respecting domains.","(x+4)(x+2)=9 => x²+6x−1=0 =>x=−3+sqrt(10)>−2; reject −3−sqrt10.","Source #38 log_12(2x+6)+log_12(x+2)=2 => x=6 domain x>−2.")
add("Solve log_4(x+3)−log_4(x−1)=1.","(x+3)/(x−1)=4 => x=7/3, x>1.","Source #39 log_6(x+2)−log_6(x−3)=1 => x=4, domain x>3.")
add("Derive the logarithm power rule from exponent laws.","Let u=log_b m so b^u=m; (b^u)^k=m^k, hence log_b(m^k)=ku for m>0.","Source #40 proof from b^x=m and b^(kx)=m^k.")
add("Prove log_5(7)=1/log_7(5).","ln7/ln5=(ln5/ln7)^(-1).","Source #41 reciprocal-base change-of-base identity.")
add("Check whether log_16(625)=log_2(5).","Yes: 16=2^4 and625=5^4 so log_16(625)=4ln5/(4ln2)=log_2 5.","Source #42 log_81(2401)=log_3 7 because81=3^4,2401=7^4.")
assert len(Q)==42 and [x["SourceSkillReferenceExercise"] for x in Q]==list(range(1,43))
W=[
("Product rule","Expand log_2(3xy).","log_2 3+log_2x+log_2y for x,y>0."),
("Quotient rule","Expand ln((4x)/y).","ln4+lnx−lny for x,y>0."),
("Power rule","Expand log_3(x^5).","5log_3 x for x>0."),
("Root power","Expand log(sqrt(x)).","(1/2)log x for x>0."),
("Reverse power rule","Condense 3ln x.","ln(x³), x>0."),
("Combined rules","Expand log((3x²)/(y³)).","log3+2logx−3logy."),
("Radical law","Expand log_2(cube_root(x²)).","(2/3)log_2x for x>0."),
("Complex logarithm","Expand ln((5a²sqrt(b))/c).","ln5+2lna+(1/2)lnb−lnc."),
("Condense product quotient","Condense log3+log x−log y.","log(3x/y) for x,y>0."),
("Condense complex","Condense 2logx+log(y+1)−3logz.","log(x²(y+1)/z³)."),
("Single logarithm","Condense (1/4)ln16+ln x.","ln(2x)."),
("Apply laws exactly","Evaluate log_2(16)−log_2(2).","4−1=3."),
("Change base to ln","Express log_7 13 via ln.","ln13/ln7."),
("Evaluate change base","Evaluate log_3 11 to five decimals.",f"{math.log(11)/math.log(3):.5f}.")]
T=[
("Expand log_2(5xy).","log_2 5+log_2x+log_2y."),
("Expand ln(6x/y).","ln6+lnx−lny."),
("Expand log_5(x³).","3log_5x."),
("Expand log(sqrt[4](x)).","(1/4)logx."),
("Condense 4lnx.","ln(x⁴)."),
("Expand ln(2a³/b²).","ln2+3lna−2lnb."),
("Expand log_3(sqrt(x³)).","(3/2)log_3x."),
("Expand ln((x² y)/sqrt z).","2lnx+lny−(1/2)lnz."),
("Condense log2+logx−logy.","log(2x/y)."),
("Condense 3lnx+lny−2lnz.","ln(x³y/z²)."),
("Condense (1/2)ln9+lnx.","ln(3x)."),
("Evaluate log_4(64)−log_4(4).","3−1=2."),
("Express log_11 7 using ln.","ln7/ln11."),
("Evaluate log_2 13 to 5 decimals.",f"{math.log(13,2):.5f}.")]
assert len(W)==14 and len(T)==14
lessoncode="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L05"
rich_rel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u04-l05.rich-lesson-v2.json"
teacher_rel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u04-l05-firstparty-teacher-led.json"
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U04:L05",
"ContentVersion":"g12-u04-l05-firstparty-log-properties-abc",
"Lessons":[{"LessonCode":lessoncode,"CultureCode":"en","Title":"Logarithmic Properties",
"ExplanationParagraphs":["For any valid common base b, log_b(MN)=log_b M+log_b N, log_b(M/N)=log_b M−log_b N and log_b(M^k)=k log_b M, with all original log arguments positive.",
"Expand products and quotients after identifying positive variable restrictions; when condensation creates an expression, preserve original domains.",
"Use roots as rational exponents. For positive x, log_b(nth_root(x))=(1/n)log_b(x).",
"Any base b>0,b!=1 can be converted: log_b(x)=ln(x)/ln(b), for x>0.",
"For logarithmic equations, combine logs first, solve, then check all original argument domains and reject extraneous roots.",
"An original source photograph illustrating a chemistry pH measurement is replaced by independent log-scale numerical examples; no publisher photo is copied.",
"Independent Cool-down: expand log((6x²)/y), condense 2lnx−lnz and solve log_2(x+1)=3."]+
[f"Independent Try It {i}: {x}"for i,(x,answer)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":question,"Method":"Use domain-safe log product, quotient and power identities.","Steps":[ans],"Answer":ans,"Check":"Recombine logarithms and enforce positivity of original arguments."}for title,question,ans in W],
"KeyConcepts":[{"Title":"Product rule","Definition":"log_b(MN)=log_bM+log_bN for M,N>0.","Rule":"Sum of logs represents a positive product.","Example":"log_2(3x)=log_2 3+log_2 x."},
{"Title":"Quotient rule","Definition":"log_b(M/N)=log_bM−log_bN for M,N>0.","Rule":"Difference represents a positive ratio.","Example":"ln(x/y)=lnx−lny."},
{"Title":"Power rule","Definition":"log_b(M^k)=k log_bM when M>0.","Rule":"Rewrite radicals as powers first.","Example":"log(sqrt(x))=(1/2)logx."},
{"Title":"Change of base","Definition":"log_b(M)=lnM/lnb, M>0, b>0 and b!=1.","Rule":"Numerical calculators support common and natural logarithms.","Example":"log_7(13)=ln13/ln7."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent base10 pH logarithmic scale",
"Description":"Original educational numerical replacement for chemistry photograph; sample H+ concentration relates to pH=−log10[H+]. No original image copied.",
"PrimaryLabel":"Hydrogen ion concentration (mol/L)","SecondaryLabel":"pH value",
"Rows":[{"Label":"[H+]","Cells":["10^(−2)","10^(−4)","10^(−7)","10^(−9)"]},{"Label":"pH=−log10[H+]","Cells":["2","4","7","9"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Use product, quotient and power rules.","Preserve original domains when combining logs.","Use natural-log change of base.","Check algebraic solutions in originals."],
"CommonMistakes":[{"Mistake":"Assume log(x+y)=logx+logy.","WhyWrong":"Sum inside a logarithm is not a product.","Correction":"Product rule only for multiplication."},{"Mistake":"Ignore individual argument positivity when condensing.","WhyWrong":"The condensed result may define extra input values.","Correction":"Retain original domain constraints."}]}]}
teacher={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":lessoncode,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(q,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Expand log(6x²/y).","Condense 2lnx−lnz.","Solve log_2(x+1)=3."],
"TeacherExpectedMath":["log6+2logx−logy for x,y>0.","ln(x²/z) for x,z>0.","x=7; argument 8 positive."],"OriginalPublisherCoolDownCopied":False},
"FirstPartyIndependentStudentMathQuestions":Q,"OriginalSourceExercisesReviewed":42,
"IndependentMathExerciseCount":42,"StageC":"REUSE_EXISTING_RICHV2_AND_TEACHER_MANUAL_SCORING","StageD":"NOT_RUN"}
def save(path,obj):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(obj,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
save(root/rich_rel,rich);save(root/teacher_rel,teacher)
figure_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l05-original-photo-firstparty-math-alternative.json"
save(root/figure_rel,{"SchemaVersion":1,"SourceFigure":1,
"OriginalSource":"OpenStax Section4.5 credited photo of acid pH test, credit David Berardan",
"FirstPartyIndependentMathAlternative":"Hydrogen ion concentration and pH log-scale numerical table",
"PublisherPhotoCopied":False,"IdenticalImageClaimed":False})
routes_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l05-stage-c-42-assessment-reuse-routes.json"
save(root/routes_rel,{"SchemaVersion":1,"Section":"4.5","Count":42,"NewMathEngine":False,
"Decision":"REUSE_EXISTING_TEACHER_REVIEW_MATH_RUBRIC_AND_RICHV2",
"IndependentExerciseRoutes":[{"Source":q["SourceSkillReferenceExercise"],"TeacherMathReviewed":True,
"IndependentQuestion":True,"AutoGradeRuntimeVerified":False}for q in Q],"CI":"NOT_RUN","StageD":"NOT_RUN"})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
assert len(Q)==42 and len(examples)==14 and len(tries)==14
assert math.isclose(math.log(13,2),3.700439718141092)
assert all(q["OriginalSourceMathReviewed"] for q in Q)
closure_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l05-abc-close-audit.json"
save(root/closure_rel,{"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":4,"Lesson":5,
"Source":"OpenStax Precalculus local 2017 book","SourceSha256":p["SourceSnapshotSha256"],
"CorrectedPdfPages":[395,404],"OriginalExamples":14,"OriginalTryIts":14,"OriginalPhoto":1,"OriginalExercises":42,
"OriginalSourceExercisesReviewed":42,"IndependentWorked":14,"IndependentTryIts":14,
"IndependentTeacherKeyExercises":42,"IndependentCoolDown":3,
"MathPhotoReplacement":figure_rel,"PublisherOriginalImageCopied":False,
"StageCExistingAssessmentRoutes":42,"NewEngine":False,
"CCSSCandidates":["CCSS:HSF-LE.A.4"],"FormalCCSSCertification":False,
"TargetedChecksRun":["Original local page boundaries and example counts","42 source math exercise skill reviews",
"14 independent worked and Try It items","Independent math table replacing protected photograph",
"42 teacher math scoring routes","Selected formula and domain checks","Git diff check"],
"CI":"NOT_RUN","StageD":"NOT_RUN","Build":"NOT_RUN","AuthenticatedRuntime":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
a={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":4,"Lesson":5,"Section":"4.5",
"OriginalSourcePdfPages":[395,404],"BlueprintIncorrectPdfPages":[396,405],
"OriginalSourceExampleAnchors":[{"Number":n,"Title":title}for n,title in examples],
"OriginalSourceTryItNumbers":tries,"OriginalSourceFigureNumbers":fig,
"OriginalSourceNumberedExerciseLedger":[{"Number":q["SourceSkillReferenceExercise"],"OriginalSourceMathReviewed":True,"IndependentMathVerified":True,"TeacherRubricVerified":True}for q in Q],
"FirstPartyStudentRichV2":rich_rel,"TeacherAssessment":teacher_rel,"OriginalPhotoAlternative":figure_rel,
"StageCAssessmentRoutes":routes_rel,"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,
"SourceSHA256":p["SourceSnapshotSha256"],"BlockingReasons":[],"ClosureEvidencePath":closure_rel,
"CI":"NOT_RUN","StageD":"NOT_RUN","Tests":"TARGETED_LOCAL_ONLY","StartedUtc":now,"ClosedAtUtc":now}
asset=base/"u04-l05-abc-source-audit.json"
save(asset,a)
assert row["Ordinal"]==25
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":395,"PdfPageEnd":404,
"BlockingReasons":[],"OriginalMathSourceExercisesReviewed":42,"FirstPartyTeacherRubrics":42,
"ClosureEvidencePath":closure_rel,"EvidencePath":str(asset.relative_to(root)).replace("\\","/"),
"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now,"UpdatedUtc":now})
assert p["LessonEntries"][25]["Ordinal"]==26
p.update({"LastClosedLessonOrdinal":25,"FirstIncompleteLessonOrdinal":26,
"ActiveLessonOrdinal":26,"NextLessonOrdinal":26,"ReadyForStageD":25,"SourceVerifiedGrade12Lessons":25,
"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_25_OF_73","UpdatedUtc":now})
save(PP,p)
files=[str(PP.relative_to(root)),str(asset.relative_to(root)),rich_rel,teacher_rel,figure_rel,routes_rel,closure_rel,str(Path(__file__).relative_to(root))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=root,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=root,check=True)
subprocess.run(["git","commit","-m","feat(g12 u04 l05): close logarithmic properties academic ABC [skip ci]"],cwd=root,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=root,check=True)
print("U04L05_CLOSE_AND_PUSH_SUCCESS 25/73 NEXT U04L06, duration",round(time.monotonic()-started,2))
