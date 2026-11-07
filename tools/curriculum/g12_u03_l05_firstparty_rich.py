from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
src=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson005.json").read_text(encoding="utf-8"))
assert src["Section"]=="3.5" and (src["Pages"][0]["PdfPage"],src["Pages"][-1]["PdfPage"])==(272,280)
worked=[
("Polynomial long division by a monic binomial","Divide 2x²+7x+3 by x+3, including remainder.","2x+1, remainder0, because (x+3)(2x+1)=2x²+7x+3."),
("Polynomial long division by a general linear divisor","Divide 6x³+7x²−16x+3 by 3x−1.","Quotient 2x²+3x−13/3, remainder −4/3; verify dividend=divisor·quotient+remainder."),
("Set up synthetic division correctly","Divide x³−4x²+x+6 by x−2 using coefficient row [1,−4,1,6].","Quotient x²−2x−3, remainder0; synthetic parameter k=2."),
("Synthetic division with negative k","Divide x³+2x²−5x−6 by x+1.","Quotient x²+x−6, remainder0; k=−1."),
("Synthetic division with missing powers","Divide x⁴+3x²−4 by x−1, including zero coefficients for x³ and x.","Quotient x³+x²+4x+4, remainder0."),
("Use volume-to-height division","A rectangular prism has volume V=(x+2)(x−1)(2x+3) cubic units; length (x+2), width (x−1). Find height.","Height H(x)=2x+3 units with x>1 for positive dimensions.")
]
tries=[
("General long division","Divide 3x³−5x²+2x+10 by x+2.","Quotient 3x²−11x+24, remainder −38."),
("Synthetic division with missing term","Divide 2x⁴−3x²+7 by x−1.","Quotient 2x³+2x²−x−1, remainder6."),
("Find geometric missing side","A rectangle area A=(x+4)(3x−2) and width w=x+4; find its length.","3x−2; check width nonzero and physical dimensions.")
]
concepts=[
("Polynomial division identity","For nonzero divisor d(x), p(x)=d(x)q(x)+r(x), deg(r)<deg(d).","Always verify by multiplying back.","x²−1=(x−1)(x+1)."),
("Long division","Divide leading term, multiply, subtract, carry down, repeat.","Continue while remainder degree ≥ divisor degree.","(2x²+7x+3)/(x+3)=2x+1."),
("Synthetic division","Shortcut for divisor x−k with coefficient row including zero placeholders.","Use k with sign opposite binomial constant.","x+2 uses k=−2."),
("Missing coefficients","A missing x^j term has coefficient zero in synthetic division.","Do not shift powers accidentally.","x⁴+3x²−4 has [1,0,3,0,−4]."),
("Remainder theorem preview","Division by x−k leaves constant remainder p(k).","Zero remainder means x−k divides p.","p(1)=0 ⇒ x−1 factor."),
("Nonmonic divisor","For ax+b factor, synthetic divide by x−(−b/a) and scale quotient by 1/a.","Direct long division also works.","Divide by 3x−1, root k=1/3."),
("Polynomial factor test","A zero remainder certifies exact divisibility.","Nonzero remainder cannot be silently ignored.","x³−1 divisible by x−1."),
("Complex synthetic root","Synthetic division with complex k can produce complex quotient coefficients.","Keep real and imaginary parts exact.","x²+1 divisible by x−i."),
("Geometric applications","Unknown height or width comes from volume/area divided by known dimensions.","State physical restrictions such as nonzero positive lengths.","H=V/(LW).")
]
cool={"StudentPrompt":"(a) Divide x³−3x²−4x+12 by x−3. (b) Divide 2x²+5x+2 by 2x+1. (c) Explain why zero remainder confirms a factor.","TeacherExpectedMath":"(a) quotient x²−4, remainder0. (b) quotient x+2, remainder0. (c) p=dq+r, and r=0 makes p divisible by d.","OriginalPublisherRestrictedCoolDownCopied":False}
lesson={"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L05",
"CultureCode":"en","Title":"Dividing Polynomials",
"ExplanationParagraphs":[
 "Learning goals from authorized source: use long division and synthetic division to divide polynomials.",
 "The polynomial division identity p=dq+r requires either zero remainder or a remainder of degree lower than the divisor.",
 "Long division: divide leading terms, multiply the divisor, subtract, and bring down the next coefficient; keep each polynomial degree aligned.",
 "Synthetic division requires a divisor x−k, uses the coefficient sequence including zeros for omitted powers, and outputs quotient coefficients followed by the remainder.",
 "For ax+b divisors use long division directly or transform to x−k and adjust the quotient by the leading scale a.",
 "Use polynomial quotient in geometric volume/area applications only where all divisor dimensions are nonzero and admissible.",
 "Independent Edulytics cool-down: "+cool["StudentPrompt"]
 ]+[f"Independent Try It {i} — {title}. {prompt}"for i,(title,prompt,a)in enumerate(tries,1)],
"WorkedExamples":[{"Title":title,"Question":prompt,"Method":"Use polynomial long/synthetic division and check dividend=divisor×quotient+remainder.","Steps":[ans],"Answer":ans,"Check":"Reconstruct exact dividend from quotient, divisor and remainder."}for title,prompt,ans in worked],
"KeyConcepts":[{"Title":title,"Definition":de,"Rule":ru,"Example":ex}for title,de,ru,ex in concepts],
"Visuals":[
 {"Kind":"RatioTable","Title":"First-party synthetic division trace","Description":"Synthetic division of x³−4x²+x+6 by x−2.","PrimaryLabel":"Coefficients","SecondaryLabel":"Result","Rows":[{"Label":"Original","Cells":["1","−4","1","6"]},{"Label":"Synthetic final","Cells":["1","−2","−3","0"]}],"Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"Zero coefficient placeholders","Description":"Degree four polynomial missing x³ and x terms.","PrimaryLabel":"Power","SecondaryLabel":"Coefficient","Rows":[{"Label":"Power","Cells":["x⁴","x³","x²","x¹","x⁰"]},{"Label":"x⁴+3x²−4","Cells":["1","0","3","0","−4"]}],"Items":[],"SecondaryItems":[]}
 ],"Videos":[],
"SummaryPoints":["Division identity p=dq+r must be preserved.","Long division works for every nonzero polynomial divisor.","Synthetic division shortcut uses x−k and zero coefficient placeholders.","Remainder zero implies exact factor.","Geometric quotients require physically valid dimensions."],
"CommonMistakes":[
 {"Mistake":"Use k=+2 for dividing by x+2.","WhyWrong":"Synthetic k is the root x=−2.","Correction":"Write divisor as x−(−2)."},
 {"Mistake":"Omit zero coefficients in synthetic division.","WhyWrong":"Misaligns polynomial powers and corrupts quotient.","Correction":"Include a zero for each missing degree."},
 {"Mistake":"Ignore a nonzero remainder.","WhyWrong":"Division identity is then false.","Correction":"State quotient and remainder and verify by multiplication."}
]}
rich=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l05.rich-lesson-v2.json"
assert not rich.exists()
rich.write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U03:L05",
"ContentVersion":"g12-u03-l05-firstparty-division-source-math-20261107","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u03-l05-three-tryit-teacher-key-and-independent-cooldown.json").write_text(json.dumps({
"OriginalSourceTryItSkills":3,"IndependentFirstPartyTryItSkills":3,
"TeacherExpectedKeys":[{"Number":i,"Expected":a}for i,(_,_,a)in enumerate(tries,1)],
"IndependentCoolDown":cool,"PublisherOriginalFigureArtCopied":False,
"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":5,
"OriginalSourceSection":"3.5","OriginalSourceSHA256":src["SourceSha256"],
"OriginalPdfPages":[272,280],
"OriginalSourceWorkedExamples":6,"OriginalSourceTryIts":3,"OriginalSourceNumberedExercises":73,
"FirstPartyWorkedExamples":6,"FirstPartyTryItAlternatives":3,
"FirstPartyKeyConcepts":9,"FirstPartyVisualTables":2,"IndependentCoolDown":True,
"RichV2Path":rich.relative_to(root).as_posix(),
"Original73SourceExerciseAcademicSkillRubrics":"PENDING",
"OriginalSourceFigureCrosswalk":"PENDING",
"ReadyForStageD":False,"StageD":"NOT_RUN","CI":"NOT_RUN",
"StartedAtUtc":datetime.now(timezone.utc).isoformat()}
(base/"u03-l05-abc-checkpoint.json").write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==15 and p["FirstIncompleteLessonOrdinal"]==16
row=p["LessonEntries"][15]
row["ACStatus"]="A_C_IN_PROGRESS";row["FirstPartyRichV2Path"]=cp["RichV2Path"]
row["EvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l05-abc-checkpoint.json"
row["ReadyForStageD"]=False
row["BlockingReasons"]=["Original73 numbered polynomial division math questions not rubricked","Original source figure skill crosswalk pending"]
p["UpdatedUtc"]=cp["StartedAtUtc"]
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U03_L05_6_WORKED_3_TRYIT_9_CONCEPTS_2_VISUALS_COOLDOWN_AUTHORED")
