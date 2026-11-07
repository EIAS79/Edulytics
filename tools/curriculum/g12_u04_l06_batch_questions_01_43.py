from pathlib import Path
import json,math,datetime,subprocess,re,time
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u04-l06-source-inventory-and-corrected-boundary.json"
a=json.loads(ap.read_text(encoding="utf8"))
assert len(a["SourceExerciseLedger"])==81 and a["OriginalSourceMathReviewed"]==0
Q=[]
def add(question,ans,source):
 n=len(Q)+1
 assert question and ans and source
 Q.append({"OriginalExerciseNumber":n,"IndependentStudentQuestion":question,"TeacherExpectedMath":ans,
 "OriginalSourceSkillMathReviewed":source,"TeacherMarking":"MANUAL_CHECK_EXACT_FORM_AND_DOMAIN",
 "OriginalQuestionCopied":False,"RuntimeVerified":False})
add("Explain when equal-base exponents can be equated.","For b>0,b≠1, b^u=b^v implies u=v.","Original #1 asks methods for solving exponential equations: like bases or logarithms.")
add("Define an extraneous logarithmic equation root.","A candidate that fails a positive log-argument constraint or original equation; reject after substitution.","Original #2 asks how extraneous roots arise and are detected.")
add("When does log_b(A)=log_b(B) imply A=B?","When common valid base b>0,b≠1 and both A,B>0.","Original #3 asks conditions for logarithmic one-to-one property.")
# 4-10 like base equations
same=[
("2^(3x−1)=2^(x+7)","3x−1=x+7 =>x=4","Original #4 equates powers of base4 to solve linear exponents."),
("3^(2x+1)=27^(x−1)","2x+1=3x−3 =>x=4","Original #5 64·4^(3x)=16 rewrites to a common base."),
("5^(x+2)·5^(2x−1)=125","3x+1=3 =>x=2/3","Original #6 products of same-base powers simplify exponents."),
("2^(−2n)·(1/8)=2^(n+3)","−2n−3=n+3 =>n=−2","Original #7 2^(−3n)/4=2^(n+2), same-base exponent rule."),
("25·5^(x+1)=625","2+x+1=4 =>x=1","Original #8 625·5^(3x+3)=125 uses common base 5."),
("36^(2b)/36^b=6^6","36^b=6^6 =>6^(2b)=6^6 =>b=3","Original #9 quotient of powers and base36 converted to base6."),
("(1/16)^(2n)·4=2^10","2^(−8n+2)=2^10 =>n=−1","Original #10 fractional bases converted to powers of2.")
]
for q,ans,src in same:add("Solve "+q+".",ans,src)
assert len(Q)==10
# 11-19 logarithmic isolation with exponential
iso=[
("3^(x−2)=1","x=2","Original #11 9^(x−10)=1 implies x=10."),
("5e^(2x)=25","x=ln5/2","Original #12 2e^(6x)=13 => x=ln(13/2)/6."),
("e^(x+3)−2=5","x=ln7−3","Original #13 e^(r+10)−10=−42 has no real solution because e^(r+10)>0."),
("4·10^(2a)=40","a=1/2","Original #14 2·10^(9a)=29 =>a=log(29/2)/9."),
("−3·10^(p+1)+1=−29","10^(p+1)=10 =>p=0","Original #15 −8·10^(p+7)−7=−24 =>positive exponentiation conditional."),
("2e^(3n−1)+4=−1","No real solution, since 2e^(3n−1)+4>4","Original #16 7e^(3n−5)+5=−89 impossible."),
("e^(−2k)+3=12","k=−(ln9)/2","Original #17 e^(−3k+6)=44 ->k=(6−ln44)/3."),
("−4e^(5x−1)−2=−10","x=(1+ln2)/5","Original #18 −5e^(9x−8)−8=−62 -> exponential isolation."),
("−2e^(3x+2)+1=−7","x=(ln4−2)/3","Original #19 −6e^(9x+8)+2=−74 -> exponential isolation.")
]
for q,ans,src in iso:add("Solve over real numbers: "+q+".",ans,src)
# 20-28 differing bases, quadratic in e^x, invalid negative
advanced=[
("2^(x+2)=5^(x−1)","x=(ln5+2ln2)/(ln5−ln2)","Original #20 2^(x+1)=5^(2x−1) solved via ln."),
("e^(2x)−5e^x+6=0","Let u=e^x>0; (u−2)(u−3)=0 => x=ln2 or ln3","Original #21 quadratic in e^x; reject nonpositive u."),
("6e^(2x)+2=−1","No real solution: 6e^(2x)+2>2","Original #22 7e^(8x+8)−5=−95 impossible."),
("3e^(2x+1)+1=28","e^(2x+1)=9, x=(ln9−1)/2","Original #23 10e^(8x+3)+2=8 isolate positive exponential."),
("2e^(4x−1)−5=9","e^(4x−1)=7, x=(1+ln7)/4","Original #24 4e^(3x+3)−7=53."),
("5e^(−3x−2)−1=−9","No real solution because left side >−1","Original #25 8e^(−5x−2)−4=−90 impossible."),
("4^(x+1)=7^(x−2)","x=(2ln7+ln4)/(ln7−ln4)","Original #26 3^(2x+1)=7^(x−2) requires logarithms."),
("e^(2x)−4e^x−5=0","u=e^x>0, (u−5)(u+1)=0 -> x=ln5; discard u=−1","Original #27 e^(2x)−e^x−6=0 -> x=ln3, discard negative root."),
("2e^(2−x)+1=−8","No real solution since 2e^(2−x)+1>1","Original #28 3e^(3−3x)+6=−31 impossible.")
]
for q,ans,src in advanced:add("Solve with domain and positivity checks: "+q+".",ans,src)
assert len(Q)==28
# 29-35 log definitions
for q,ans,src in [
("Rewrite log(1/1000)=−3 in exponential form.","10^(−3)=1/1000.","Original #29 log(1/100)=−2 equivalent to 10^(−2)=1/100."),
("Rewrite log_81(9)=1/2 in exponential form.","81^(1/2)=9.","Original #30 log_324(18)=1/2 equivalent sqrt324=18."),
("Solve 3log_2(n)=9.","n=2³=8, n>0.","Original #31 5log_7(n)=10 ->n=49."),
("Solve −4log_5(x)=8.","x=5^(−2)=1/25.","Original #32 −8log_9x=16 ->x=1/81."),
("Solve 2+log_3(4k)=0.","log_3(4k)=−2 =>k=1/36.","Original #33 4+log_2(9k)=2 =>k=1/36."),
("Solve 3log(5n+5)=6.","5n+5=100 =>n=19; n>−1.","Original #34 2log(8n+4)+6=10 ->n=12."),
("Solve 5−2ln(4−x)=1.","ln(4−x)=2 =>x=4−e², x<4.","Original #35 10−4ln(9−8x)=6 ->x=(9−e)/8.")
]:add(q,ans,src)
# 36-43 one-to-one with domain checks
for q,ans,src in [
("Solve ln(9−x)=ln(2x).","x=3; both arguments positive.","Original #36 ln(10−3x)=ln(−4x) ->x=−10 (both arguments positive)."),
("Solve log_5(6n+1)=log_5(13−n).","n=12/7; arguments>0.","Original #37 log_13(5n−2)=log_13(8−5n) ->n=1."),
("Solve log(x+5)−log(x)=log(3).","(x+5)/x=3 ->x=5/2>0.","Original #38 log(x+3)−logx=log74 ->x=3/73."),
("Solve ln(−2x)=ln(x²−3x).","x²−x=0 ->x=0 or1, neither x<0: no real solution.","Original #39 ln(−3x)=ln(x²−6x): x=0 or3, neither x<0, no real solution."),
("Solve log_4(8−m)=log_4(2m).","m=8/3; valid 0<m<8.","Original #40 log_4(6−m)=log_4(3m) ->m=3/2."),
("Solve ln(x−1)−ln(x)=ln(1/2).","(x−1)/x=1/2 ->x=2, domain x>1.","Original #41 ln(x−2)−lnx=ln54 => x negative, violates x>2; no real solution."),
("Solve log_3(2n²−8n)=log_3(n²−12).","n²−8n+12=0 -> n=2 or6; only n=6 satisfies positive arguments.","Original #42 log_9(2n²−14n)=log_9(n²−45), n=9 or5, check arguments: n9 valid; n5 invalid."),
("Solve ln(x²−4)+ln4=ln12.","4(x²−4)=12 =>x²=7, x=±sqrt7; arguments>0.","Original #43 ln(x²−10)+ln9=ln10 => x²=100/9+10=100/9? actually 9(x²−10)=10 ->x²=100/9.")
]:add(q,ans,src)
assert len(Q)==43
for row in a["SourceExerciseLedger"][:43]:
 n=row["Number"]
 assert n==Q[n-1]["OriginalExerciseNumber"]
 row.update({"OriginalSourceMathReviewed":True,"IndependentStudentQuestionVerified":True,"TeacherMathVerified":True})
a.update({"OriginalSourceMathReviewed":43,"IndependentSourceMathExercises":43,
"BlockingReasons":["SOURCE_EXERCISES_44_TO_81_MATH_AND_INDEPENDENT_TEACHER_KEYS_PENDING",
"13_WORKED_AND_13_TRYIT_ALTERNATIVES_PENDING","3_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_ASSESSMENT_ROUTES_PENDING"],
"UpdatedUtc":datetime.datetime.now(datetime.timezone.utc).isoformat()})
teacher_rel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u04-l06-firstparty-teacher-led.json"
tp=root/teacher_rel
teacher={"SchemaVersion":1,"Section":"4.6","Course":"US-CCSS-MATH:G12",
"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L06",
"IndependentStudentMathQuestions":Q,"SourceMathReviewed":43,"IndependentMathQuestionCount":43,
"AutoGradingVerified":False,"StageD":"NOT_RUN"}
tp.write_text(json.dumps(teacher,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a["FirstPartyTeacherAssessment"]=teacher_rel
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L06_FIRST43_MATH_REVIEWED_OPEN_38_REMAINING")
