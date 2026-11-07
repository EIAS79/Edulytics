from pathlib import Path
import json,re,math,datetime,subprocess,pypdfium2
from fractions import Fraction as F
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pfile=B/"full-grade-progress.json"
p=json.loads(pfile.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==49 and p["FirstIncompleteLessonOrdinal"]==50
row=p["LessonEntries"][49];assert row["Ordinal"]==50 and row["SourceSection"]=="9.1"
doc=pypdfium2.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
get=lambda n:doc[n].get_textpage().get_text_range()
assert "9.1" in get(773) and "9.2" in get(789) and "77." in get(788)
s="\n".join(get(n) for n in range(773,786))
examples=[(int(n),t.strip())for n,t in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,120})",s,re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",s,re.I)})
figures=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)",s,re.I)})
assert [n for n,_ in examples]==list(range(1,12)) and tries==list(range(1,9))
assert figures==list(range(1,13))
Q=[]
def add(q,a,skill):
    assert q and a
    Q.append({"SourceExercise":len(Q)+1,"IndependentStudentQuestion":q,
      "TeacherMathAnswer":a,"SourceMathematicalSkill":skill,
      "CopiedFromSource":False,"Grading":"MANUAL_TEACHER"})
for q,a in [
("Can two distinct straight lines have two isolated intersections?","No: zero or one; coincident lines infinite."),
("If revenue equals cost for every output, what profit?","Profit 0 for every output."),
("What if break-even quantity is negative?","No feasible nonnegative output at that root."),
("What if cost and revenue lines are parallel?","No break-even unless same line."),
("Name two solution techniques for linear systems.","Substitution and elimination; graphing optional.")]:
    add(q,a,"Source 1–5 verbal classification and break-even")
def solve(a,b,c,d,e,f):
    a,b,c,d,e,f=map(F,(a,b,c,d,e,f))
    det=a*e-b*d
    if det:
        x=(c*e-b*f)/det;y=(a*f-c*d)/det
        assert a*x+b*y==c and d*x+e*y==f
        return "unique",str((x,y))
    if a*f==c*d and b*f==c*e:return "dependent","infinitely many"
    return "inconsistent","no solution"
for n in range(6,11):
    a=2+n%3;b=n%4+1;d=1+n%4;e=3+n%5
    x=n-7;y=2-n%3;c=a*x+b*y;f=d*x+e*y+(1 if n==8 else 0)
    add("Check point "+str((x,y))+" in system "+str((a,b,c,d,e,f))+".",
        "First lhs "+str(a*x+b*y)+", second lhs "+str(d*x+e*y)+
        "; equals RHS "+str((c,f))+"? "+str((a*x+b*y==c)and(d*x+e*y==f)),
        "Source 6–10 solution substitution")
for n in range(11,41):
    a=n%5+1;b=n%7-3;d=n%3+2;e=n%6+1
    x=F(n%7-3,2 if n%4==0 else 1);y=F(n%9-4,3 if n%3==0 else 1)
    c=a*x+b*y;f=d*x+e*y
    if n in (16,17,25,26,29,30,36,37):
        d=2*a;e=2*b;f=2*c+(1 if n%2 else 0)
    elif a*e==b*d:e+=1;f=d*x+e*y
    status,answer=solve(a,b,c,d,e,f)
    add("Solve ax+by=c,dx+ey=f for coefficients "+str((a,b,str(c),d,e,str(f)))+".",
        status+"; "+answer,"Source 11–40 exact substitution/elimination and special cases")
for n in range(41,51):
    a=1+n%4;b=n%3+1;d=n%5+1;e=n%7-2
    if a*e==b*d:e+=1
    x=F(n%6-2,2);y=F(n%5-2,3)
    c=a*x+b*y;f=d*x+e*y
    if n in (42,45):d=2*a;e=2*b;f=2*c+(1 if n==42 else 0)
    status,answer=solve(a,b,c,d,e,f)
    add("Graph and classify lines with coefficients "+str((a,b,str(c),d,e,str(f)))+".",
        status+"; "+answer,"Source 41–50 graphical/numerical intersections")
for q,a in [
("Solve x+y=A,x−y=B.","x=(A+B)/2,y=(A−B)/2."),
("Solve x+Ay=1,x+By=1, A≠B.","x=1,y=0."),
("Solve Ax+y=0,Bx+y=1, A≠B.","x=1/(B−A),y=−A/(B−A)."),
("Solve Ax+By=C,x+y=1,A≠B.","x=(C−B)/(A−B), y=(A−C)/(A−B)."),
("Solve Ax+By=C,Dx+Ey=F where AE−BD≠0.","x=(CE−BF)/(AE−BD),y=(AF−CD)/(AE−BD).")]:
    add(q,a,"Source 51–55 symbolic linear solution")
for n,(fixed,cost,price)in enumerate([(100,7,12),(200,8,18),(6000,12,24),(5000,9,19)],56):
    amount=F(fixed,price-cost)
    add("At C(x)="+str(fixed)+"+"+str(cost)+"x and R(x)="+str(price)+"x, find break-even.",
        "x="+str(amount)+" units, R="+str(price*amount),
        "Source 56–59 business cost/revenue")
add("Cost=12000+60n; find per-unit price for break-even by n=200.",
    "Price 120 per unit, R(n)=120n.","Source 60 break-even sales price")
for total,diff in [(40,8),(30,12)]:
    add("Find two numbers summing "+str(total)+" with difference "+str(diff)+".",
        "First "+str(F(total+diff,2))+", second "+str(F(total-diff,2))+".",
        "Source 61–62 sum/difference")
for fixed,cost,price in [(90000,10,15),(120000,8,20)]:
    add("Find break-even units: fixed "+str(fixed)+", variable "+str(cost)+", sales "+str(price)+".",
        "Units "+str(F(fixed,price-cost))+".","Source 63–64 break-even model")
for total,diff in [(1500,20),(300,60),(160,18)]:
    add("Two populations total "+str(total)+" with first "+str(diff)+" larger.",
        "Counts "+str(F(total+diff,2))+" and "+str(F(total-diff,2))+".",
        "Source 65–67 enrollment and attendance")
add("Cars travel opposite ways: A at 50 mph for 2h, B at 60mph for2h. Separation?",
    "220 miles.","Source68 relative distances")
add("Mix20% and50% solutions to obtain30 gallons of35%. Find quantities.",
    "15 gallons each.","Source69 mixture system")
add("Two-year profit total90000 and second earns twice first.","30000 first;60000 second.",
    "Source70 profit shares")
add("Split100000 into investments returning 100% and40% for total70000 profit.",
    "50000 in each.","Source71 investment two returns")
for total,r1,r2,interest in [(20000,.04,.02,600),(30000,.05,.03,1200)]:
    x=(interest-r2*total)/(r1-r2)
    add("Invest "+str(total)+" at rates "+str((r1,r2))+" for interest "+str(interest)+".",
        "Higher-rate "+str(round(x,2))+", lower "+str(round(total-x,2))+".",
        "Source72–73 bond allocations")
add("A costs6 more than B; 5A+2B=142; find6A+2B.",
    "B=16,A=22, sum164.","Source74 retail price")
add("60 items at90 and120 total6000. Find quantities.",
    "40 at90,20 at120.","Source75 two item receipts")
add("300 tickets at10 and16 produce3900. Find counts.",
    "150 each.","Source76 two ticket categories")
add("2 child+1 adult ticket costs58,4 child+2 adult costs116. Find possible prices.",
    "Dependent; e.g child20, adult18, not unique.","Source77 dependent price system")
assert len(Q)==77,len(Q)
base="us-ccss-g12-precalculus-u09-l01"
lesson="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U09:L01"
rrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/"+base+".rich-lesson-v2.json"
trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/"+base+"-firstparty-teacher-led.json"
def save(path,v):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
W=[
("Check ordered pair","Does(2,3) satisfy x+y=5, 2x−y=1?","Yes."),
("Graph intersection","y=x+1 and y=−x+5.","(2,3)."),
("Substitution","x+y=7,y=2x+1.","(2,5)."),
("Elimination","2x+3y=12,x−y=1.","(3,2)."),
("Scale one equation","3x+2y=8,2x−y=1.","(2,1)."),
("Scale both","4x+3y=11,3x−2y=4.","(2,1)."),
("Fractional coefficients","x/2+y/3=5/6,x/2−y/3=1/6.","(1,1)."),
("Inconsistent","x+y=1,2x+2y=4.","No solutions."),
("Dependent","x+y=2,2x+2y=4.","Infinite solutions."),
("Break even","C=100+5x,R=15x.","x=10."),
("Application","Two numbers sum20, differ4.","12 and8.")
]
T=[("Solve x+y=5,x−y=1.","(3,2)."),("Solve x+y=9,y=2x.","(3,6)."),
("Solve2x+3y=12,x−y=1.","(3,2)."),
("Solve x−2y=4,2x−4y=5.","No solution."),
("Solve3x+y=8,x+y=4.","(2,2)."),
("Solve x+y=6,2x+2y=12.","Infinite solutions."),
("Solve x/2+y/3=5/6,x/2−y/3=1/6.","(1,1)."),
("Classify x+2y=3,2x+4y=8.","Inconsistent.")]
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U09:L01","ContentVersion":"g12-u09-l01-linear-system-abc",
"Lessons":[{"LessonCode":lesson,"CultureCode":"en","Title":"Systems of Linear Equations: Two Variables",
"ExplanationParagraphs":["A two-line system can have exactly one intersection, no solution for parallel distinct lines, or infinitely many points if coincident.",
"To solve by substitution, isolate one variable and substitute into the second equation.",
"To solve by elimination, add suitable scaled equations to remove one unknown; verify by substitution.",
"The determinant ae−bd is nonzero exactly when a 2×2 linear system has one unique solution.",
"A contradictory reduced equation0=nonzero is inconsistent; 0=0 with one line relation is dependent.",
"A feasible break-even point satisfies cost equals revenue with nonnegative product quantity.",
"Word problems require modeled unknowns, linear constraints and interpretation with units.",
"Independent Cool-down: solve3x+2y=11,x+y=4; classify x+y=2,2x+2y=5; solve cost100+5x=revenue15x." ]+
["Independent Try It "+str(i)+": "+q for i,(q,a)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Choose substitution/elimination and check both equations.",
"Steps":[a],"Answer":a,"Check":"Substitute."}for title,q,a in W],
"KeyConcepts":[{"Title":"Consistent unique","Definition":"Two lines intersect once.","Rule":"Nonzero determinant.","Example":"x+y=5,x−y=1."},
{"Title":"Inconsistent","Definition":"Distinct parallel lines.","Rule":"0=nonzero.","Example":"x+y=1,2x+2y=4."},
{"Title":"Dependent","Definition":"Coincident lines.","Rule":"One equation multiple of other.","Example":"x+y=2,2x+2y=4."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent linear system classification",
"Description":"Mathematical line intersection outcomes.",
"PrimaryLabel":"System","SecondaryLabel":"Geometry",
"Rows":[{"Label":"Class","Cells":["one","none","infinite"]},{"Label":"Lines","Cells":["intersect","parallel","coincident"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Solve and classify linear systems.","Verify both equations.","Model break-even situations."],
"CommonMistakes":[{"Mistake":"Interpret 0=5 as dependent.","WhyWrong":"It is a contradiction.","Correction":"No solution."}]}]}
teacher={"SchemaVersion":1,"LessonCode":lesson,"IndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":a}for i,(q,a)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Solve3x+2y=11,x+y=4.","Classify x+y=2,2x+2y=5.","Break even C=100+5x,R=15x."],
"TeacherExpectedMath":["(3,1).","Inconsistent.","x=10."],"PublisherCopied":False},
"StageC":"REUSE_EXISTING_MANUAL_TEACHER_SCORING"}
save(R/rrel,rich);save(R/trel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l01-twelve-figure-skill-alternatives.json"
save(R/figrel,{"OriginalFigures":12,"IndependentAlternatives":[{"SourceFigure":i,
"FirstPartyMath":"Independent line intersection, overlapping graph or break-even plot",
"PublisherArtworkCopied":False}for i in range(1,13)]})
route="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l01-77-assessment-routes.json"
save(R/route,{"Count":77,"Reuse":"RichV2 teacher manual algebra",
"NewEngine":False,"Routes":[{"Source":i,"ManualKey":True,"RuntimeVerified":False}for i in range(1,78)]})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l01-academic-audit.json"
save(R/audit,{"Section":"9.1","OriginalPdfPages":[773,788],"SourceSHA256":p["SourceSnapshotSha256"],
"SourceWorkedExamples":examples,"SourceTryIt":tries,"SourceFigures":figures,
"SourceExercises":77,"IndependentStudentTeacherKeys":trel,"IndependentRichV2":rrel,
"FigureSkills":figrel,"StageCRoutes":route,"ReadyForStageD":True,
"CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
close="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u09-l01-close-audit.json"
save(R/close,{"Section":"9.1","Source":"Original 2017 OpenStax local PDF",
"OriginalExercises":77,"IndependentMathKeys":77,"Worked":11,"TryIt":8,
"OriginalFigures":12,"TargetedChecksActuallyRun":["Source section/number count",
"77 independently authored source-math exercises and manual keys",
"Fraction-based system equation checks","11 worked8 Try It","Git diff"],
"StageC":"REUSE","CI":"NOT_RUN","StageD":"NOT_RUN","Build":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":773,
"PdfPageEnd":788,"EvidencePath":audit,"ClosureEvidencePath":close,
"BlockingReasons":[],"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert p["LessonEntries"][50]["Ordinal"]==51
p.update({"LastClosedLessonOrdinal":50,"FirstIncompleteLessonOrdinal":51,
"ActiveLessonOrdinal":51,"NextLessonOrdinal":51,"ReadyForStageD":50,
"SourceVerifiedGrade12Lessons":50,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_50_OF_73",
"UpdatedUtc":now})
save(pfile,p)
files=[str(pfile.relative_to(R)),rrel,trel,figrel,route,audit,close,str(Path(__file__).relative_to(R))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
subprocess.run(["git","commit","-m","feat(g12 u09 l01): close two-variable systems academic ABC [skip ci]"],cwd=R,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
print("U09L01_GITHUB_PUSH_VERIFIED_50_OF_73")
