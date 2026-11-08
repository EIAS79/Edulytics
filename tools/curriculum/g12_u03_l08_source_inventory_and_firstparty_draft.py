from pathlib import Path
import json,re,datetime
from fractions import Fraction
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson008.json")
src=json.loads(cache.read_text(encoding="utf-8"))
p=json.loads((base/"full-grade-progress.json").read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==18 and p["FirstIncompleteLessonOrdinal"]==19
assert src["Section"]=="3.8" and [src["Pages"][0]["PdfPage"],src["Pages"][-1]["PdfPage"]]==[314,324]
assert src["SourceSha256"]==p["SourceSnapshotSha256"]
text="\n".join(page["Text"] for page in src["Pages"])
examples=[(int(n),title.strip()) for n,title in re.findall(r"Example\s+(\d+)\s+([^\n]{1,140})",text,re.I)]
tries=sorted({int(n) for n in re.findall(r"Try It\s*#?(\d+)",text,re.I)})
fig=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)\b",text,re.I)})
assert [n for n,_ in examples]==list(range(1,9)) and tries==list(range(1,6)) and fig==list(range(1,10))
categories=[("VERBAL",1,4),("RESTRICTED_QUADRATIC_INVERSE",5,12),("CUBIC_INVERSE",13,16),("RADICAL_INVERSE",17,22),("RATIONAL_INVERSE",23,28),("COMPLETING_SQUARE_INVERSE",29,31),("GRAPH_INVERSE_REFLECTION",32,41),("RADICAL_RATIONAL_DOMAIN",42,46),("NUMERIC_INVERSE_POINTS",47,51),("PARAMETRIC_INVERSE",52,56),("APPLIED_INVERSE_MODELS",57,66)]
assert sum(hi-lo+1 for _,lo,hi in categories)==66
tasks=[{"Number":n,"TaskFamily":family,"OriginalSourceSection":"3.8","SourceLocallyLocated":True,"OriginalMathematicalAnswerIndividuallyVerified":False,"FirstPartyIndependentMathQuestionVerified":False,"TeacherRubricVerified":False,"RequiresGraph":family in ("GRAPH_INVERSE_REFLECTION","RADICAL_RATIONAL_DOMAIN","NUMERIC_INVERSE_POINTS")}for family,lo,hi in categories for n in range(lo,hi+1)]
assert [x["Number"]for x in tasks]==list(range(1,67))
worked=[
("Verify inverse functions","Show f(x)=2x−7 and g(x)=(x+7)/2 are inverses.","f(g(x))=x and g(f(x))=x after simplifying both compositions."),
("Inverse cubic polynomial","Invert f(x)=3x³+1 over real numbers.","Solve y=3x³+1 to get f⁻¹(y)=∛((y−1)/3)."),
("Domain-restricted quadratic","Find inverse of f(x)=(x−4)² for x≥4.","Its inverse is f⁻¹(y)=4+√y for y≥0; the positive square-root branch follows original restriction."),
("Choose a quadratic inverse branch","Invert f(x)=6−(x+2)² for x≥−2.","f⁻¹(y)=−2+√(6−y) for y≤6; inverse outputs are ≥−2."),
("Inverse radical with domain","Invert f(x)=2+√(x−1).","f⁻¹(y)=(y−2)²+1, inverse input y≥2; original input x≥1."),
("Cone application","A cone has height twice its radius and volume V=2πr³/3. Find r for V=18π.","r=∛(3V/(2π)); V=18π gives r=3 and height6 length units."),
("Radical of rational domain","Find domain of h(x)=√((x−2)/(x+3)).","Require (x−2)/(x+3)≥0 and x≠−3: (−∞,−3) ∪ [2,∞)."),
("Inverse rational function","Invert f(x)=(3x+1)/(x−2).","Solve y(x−2)=3x+1: inverse f⁻¹(y)=(2y+1)/(y−3), y≠3.")
]
tryits=[
("Composition","For f(x)=x³−5, write f⁻¹ and verify by composition.","f⁻¹(x)=∛(x+5); both compositions give x."),
("Quadratic increasing branch","Find inverse of f(x)=(x+1)² on x≥−1.","f⁻¹(x)=−1+√x, x≥0."),
("Quadratic decreasing branch","Find inverse of f(x)=(x−2)² on x≤2.","f⁻¹(x)=2−√x, x≥0."),
("Radical inverse","Invert f(x)=1+√(3x−6).","f⁻¹(x)=((x−1)²+6)/3, inverse domain x≥1."),
("Rational inverse","Invert f(x)=(2x−3)/(x+4).","f⁻¹(x)=(4x+3)/(2−x), inverse domain x≠2.")
]
assert len(worked)==8 and len(tryits)==5
assert Fraction(3*18,2)==27 and 3**3==27
assert Fraction(2*(0)+1,(0)-3)==Fraction(-1,3)
assert Fraction(4*0+3,2-0)==Fraction(3,2)
concepts=[
("Inverse functions","Interchange inputs and outputs; an inverse function exists if the original is one-to-one on its stated domain.","Verify both compositions and exchange domain/range.","f(x)=2x−7 has inverse (x+7)/2."),
("Restricted quadratic inverse","A full parabola is not one-to-one; restrict to one monotone branch.","The original branch determines ±√ after swapping x and y.","f(x)=(x−4)², x≥4 has inverse 4+√x."),
("Radical inverse","Undo the radical with squaring/cubing, but preserve range and domain restrictions.","Squaring may introduce extraneous roots; always check against original range.","f(x)=√(x−1)+2 inverse (x−2)²+1, x≥2."),
("Inverse rational functions","Swap x,y then solve linearly for x; exclude denominator zeros in original and inverse.","Check impossible horizontal-asymptote outputs are not in inverse domain.","(3x+1)/(x−2) has inverse (2x+1)/(x−3)."),
("Radical rational domains","A square root requires nonnegative radicand and nonzero original denominator.","Use a sign chart of numerator zeros and denominator poles.","√((x−2)/(x+3)) permits x<−3 or x≥2.")
]
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L08"
explanations=["Original section skills: invert polynomial and rational functions, restrict non-one-to-one polynomial domains, invert radicals, analyze composed radical-rational domains and apply inverse models.",
"An inverse reflects ordered pairs across y=x. Always compare the original domain with the inverse range.",
"For quadratic inverses, impose the original increasing or decreasing branch before choosing the sign of a square root.",
"For radical or rational expressions, keep the original exclusions; clearing denominators and squaring may introduce invalid candidates.",
"Independent Cool-down draft: (a) invert f(x)=(x+2)² on x≥−2; (b) invert f(x)=√(x−4)+3; (c) find domain of √((x−1)/(x+2))."]+[f"Independent Try It {i}: {q}"for i,(_,q,ans) in enumerate(tryits,1)]
lesson={"LessonCode":code,"CultureCode":"en","Title":"Inverses and Radical Functions",
"ExplanationParagraphs":explanations,
"WorkedExamples":[{"Title":name,"Question":q,"Method":"Reverse input/output, solve and respect one-to-one domain restrictions.","Steps":[ans],"Answer":ans,"Check":"Recompose and verify original domain/range restrictions."}for name,q,ans in worked],
"KeyConcepts":[{"Title":name,"Definition":defn,"Rule":rule,"Example":example}for name,defn,rule,example in concepts],
"Visuals":[{"Kind":"RatioTable","Title":"Inverse function ordered pairs","Description":"Independent reflection table; swap inputs and outputs.","PrimaryLabel":"Original f","SecondaryLabel":"Inverse f⁻¹","Rows":[{"Label":"Input","Cells":["0","1","2"]},{"Label":"Output f(x)=2x−7","Cells":["−7","−5","−3"]},{"Label":"Inverse input","Cells":["−7","−5","−3"]},{"Label":"Inverse output","Cells":["0","1","2"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Verify inverse compositions.","Restrict quadratics to monotone branch.","Exchange domain and range.","Keep radical and denominator restrictions.","Interpret inverse models with physical units."],
"CommonMistakes":[{"Mistake":"Use both square-root branches for a single inverse function.","WhyWrong":"Without an original domain restriction a parabola is not one-to-one.","Correction":"Choose branch based on the restricted original domain."},{"Mistake":"Ignore radical output constraints when inverting.","WhyWrong":"Squaring can introduce invalid extraneous cases.","Correction":"Record inverse domain from original range and check."}]}
rich_rel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l08.rich-lesson-v2.json"
rich_path=root/rich_rel
rich_path.write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U03:L08","ContentVersion":"g12-u03-l08-firstparty-inverse-radical-draft-20261107","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
teacher_rel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l08-firstparty-teacher-led.json"
teacher_path=root/teacher_rel
teacher_path.write_text(json.dumps({"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":code,"OriginalSourceExerciseCount":66,
"FirstPartyIndependentTryItTeacherGuidance":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(_,q,ans)in enumerate(tryits,1)],
"IndependentCoolDownTeacherExpectedMath":["f⁻¹(x)=−2+√x, x≥0","f⁻¹(x)=(x−3)²+4, x≥3","domain (−∞,−2) ∪ [1,∞)"],
"SourceExerciseTeacherRubricsAuthored":0,"SourceImagesCopied":False,"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
asset_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l08-source-inventory-and-gap-audit.json"
a={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":8,"SourceSection":"3.8",
"SourceSHA256":src["SourceSha256"],"OriginalPdfPages":[314,324],"StartedAtUtc":now,
"OriginalSourceLearningObjectives":["Find inverse of polynomial function","Restrict polynomial domain to find inverse"],
"SourceWorkedExampleAnchors":[{"Number":n,"Title":title}for n,title in examples],
"SourceTryItNumbers":tries,"SourceFigureNumbers":fig,"SourceNumberedExerciseLedger":tasks,
"FirstPartyDraftRichV2Path":rich_rel,"FirstPartyDraftTeacherGuidePath":teacher_rel,
"FirstPartyIndependentWorkedExamplesAuthored":8,"FirstPartyIndependentTryItAuthored":5,
"OriginalSourceMathExercisesIndividuallyVerified":0,"FirstPartyIndependentSourceExerciseMathAuthored":0,
"OriginalFiguresIndependentMathAlternatives":0,"StageC":"PENDING",
"BlockingReasons":["66_SOURCE_NUMBERED_EXERCISES_REQUIRE_MATH_REVIEW_AND_INDEPENDENT_TASKS","9_SOURCE_FIGURE_MATH_ALTERNATIVES_PENDING","STAGE_C_PRACTICE_AND_TEACHER_REVIEW_ROUTES_PENDING"],
"StageD":"NOT_RUN","CI":"NOT_RUN","Tests":"TARGETED_LOCAL_JSON_SOURCE_INVENTORY_ONLY","ReadyForStageD":False,"Status":"A_C_IN_PROGRESS"}
(base/"u03-l08-source-inventory-and-gap-audit.json").write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
row=p["LessonEntries"][18]
assert row["SourceSection"]=="3.8" and row["Ordinal"]==19 and not row["ReadyForStageD"]
row.update({"ACStatus":"A_C_IN_PROGRESS","ReadyForStageD":False,"FirstPartyRichV2Path":rich_rel,"EvidencePath":asset_rel,
"OriginalSourceWorkedExamplesLocated":8,"OriginalSourceTryItsLocated":5,
"OriginalSourceFigureAnchorsLocated":9,"OriginalSourceNumberedExercisesLocated":66,
"BlockingReasons":a["BlockingReasons"],"UpdatedUtc":now})
p["UpdatedUtc"]=now
(base/"full-grade-progress.json").write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U03_L08_SOURCE_66_INVENTORY_8_EXAMPLES_5_TRYITS_DRAFT_STILL_OPEN")
