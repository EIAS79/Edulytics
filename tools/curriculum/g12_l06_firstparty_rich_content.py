from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson006.json").read_text(encoding="utf-8"))
assert source["Section"]=="1.6" and len(source["Pages"])==11
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L06"
models=[
("Model a symmetric number-line tolerance","Find all real numbers within or exactly three units of six.","|x−6|≤3, so x∈[3,9]."),
("Manufacturing tolerance with units","A resistor rated at 10 kΩ allows a ±2% tolerance. Express acceptable resistance R in ohms and interval notation.","|R−10000|≤200 ohms; R∈[9800,10200] ohms."),
("Recover the V-shape equation","An upward V-shaped graph has vertex (−1,−3) and crosses the vertical axis at (0,−1). Determine f(x)=a|x−h|+k.","f(x)=2|x+1|−3, with a=2, vertex (−1,−3), range [−3,∞)."),
("Compute zeros of an absolute-value function","Find all zeros of f(x)=|3x+2|−8.","3x+2=±8; x=2 or x=−10/3."),
("Recognize a no-solution equation","Solve 2|x−1|+3=1.","No real solution: a nonnegative absolute term cannot yield −2."),
("Solve a strict absolute inequality","Solve |x−4|<3.","−3<x−4<3, so x∈(1,7)."),
("Solve where a transformed V graph is below zero","For f(x)=−(1/2)|2x−5|+3, find all x with f(x)<0.","|2x−5|>6, so x<−1/2 or x>11/2.")
]
tryits=[
("Distance from center","Find all x within two units of −1.","|x+1|≤2, interval [−3,1]."),
("Test passing score","A score within 15 points of 75 meets a stated requirement. Express inclusive absolute-value inequality.","|x−75|≤15, 60≤x≤90."),
("Graph a V transformation","Reflect f(x)=|x| over x-axis, then shift right 2 and up 4.","g(x)=−|x−2|+4, vertex (2,4), range (−∞,4]."),
("Solve for both roots","Find zeros of f(x)=|2x+3|−5.","x=1 and x=−4."),
("Locate graph intercepts","Find intercepts of f(x)=−|x+1|+4.","x-intercepts x=−5 and x=3; y-intercept f(0)=3."),
("Inclusive absolute inequality","Solve |x−3|≤5.","[−2,8]."),
("Inequality after dividing by negative","Solve −2|k−4|≤−6.","|k−4|≥3; k≤1 or k≥7.")
]
concepts=[
("Absolute distance","|x−a| denotes nonnegative real distance of x from a.","|x−a|≤d for d≥0 is the closed interval [a−d,a+d].","|x−6|≤3 gives [3,9]."),
("Absolute V-shape","f(x)=a|x−h|+k has corner/vertex (h,k).","Sign of a controls opening, |a| controls steepness; domain all real.","−2|x+1|+4 opens down from (−1,4)."),
("Absolute equations","If |A|=c with c>0, then A=c or A=−c.","For c<0 there are no solutions; for c=0 solve A=0.","|2x−3|=5 gives x=4 or −1."),
("Absolute inequalities","Inside ≤ yields intersection; outside ≥ yields two rays for a nonnegative bound.","For negative bounds examine the nonnegative absolute value before case splitting.","|x−3|≤5 gives [−2,8]; |x−3|>5 gives two rays."),
("Intercepts","Set y=0 for x-intercepts and x=0 for y-intercept.","An upward shifted V may have no x-intercepts even though it always has a y-intercept.","2|x−1|+3 has no x-intercepts."),
("Context and units","Engineering tolerances model bounded deviations.","State center, allowable distance, inclusion, units and interpretation.","|R−10000|≤200 Ω."),
("Viewing windows","A bounded plotted x-window generally has a range different from the full real-domain range.","Check vertex inside the window and both included endpoints.","10|x−2| on [0,4] attains [0,20].")
]
visuals=[
 {"Kind":"RatioTable","Title":"Distance from a target","Description":"The equal-distance endpoints are included when inequality uses ≤.","PrimaryLabel":"x","SecondaryLabel":"Distance","Rows":[{"Label":"Input x","Cells":["3","4","6","8","9"]},{"Label":"|x−6|","Cells":["3","2","0","2","3"]}],"Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"V transformation from vertex","Description":"Piecewise absolute-value graph: every input has exactly one output.","PrimaryLabel":"x","SecondaryLabel":"f(x)","Rows":[{"Label":"x","Cells":["−3","−2","−1","0","1"]},{"Label":"2|x+1|−3","Cells":["1","−1","−3","−1","1"]}],"Items":[],"SecondaryItems":[]}
]
cool={"StudentPrompt":"(a) Solve |x+2|≤4 using interval notation. (b) For f(x)=−2|x−3|+4 state vertex, range and x-intercepts. (c) Explain why |3x−1|=−2 has no solution.","TeacherExpectedMathematics":"(a) [−6,2]; (b) vertex (3,4), range (−∞,4], x-intercepts 1 and 5; (c) absolute value cannot be negative.","OriginalTeacherOnlyPublisherCoolDownCopied":False}
lesson={
"LessonCode":code,"CultureCode":"en","Title":"Absolute Value Functions",
"ExplanationParagraphs":[
 "Learning goals: express numerical distances and contextual tolerances with |x−a|, construct transformed absolute-value graphs, solve absolute equations and graph absolute inequalities.",
 "The absolute value |x−a| is a nonnegative distance. For a nonnegative tolerance d, |x−a|≤d includes all x from a−d through a+d, while |x−a|≥d describes two external rays.",
 "The graph f(x)=a|x−h|+k has vertex (h,k). If a>0 it opens upward and attains minimum k; if a<0 it opens downward and attains maximum k.",
 "For |A|=c: if c>0 solve A=±c; if c=0 solve A=0; if c<0 there are no real solutions. Check each candidate in the original formula.",
 "A strict inside inequality |A|<c has two simultaneous bounds; an outside inequality |A|>c describes separate rays. When multiplying or dividing inequalities by a negative factor, reverse the sign.",
 "Graph intercepts require setting the appropriate coordinate to zero. Always state endpoints and included/excluded bounds for real-domain and graph window problems.",
 "Independent Edulytics Cool-down: "+cool["StudentPrompt"]
 ]+[f"Independent Try It {i} — {title}. {q}"for i,(title,q,answer)in enumerate(tryits,1)],
"WorkedExamples":[{"Title":t,"Question":q,"Method":"Use nonnegative absolute-distance definitions, source-skill V-graph properties, exact algebraic intervals and teacher review.","Steps":[ans],"Answer":ans,"Check":"Verify by substitution and endpoint inclusion."}for t,q,ans in models],
"KeyConcepts":[{"Title":t,"Definition":d,"Rule":r,"Example":e}for t,d,r,e in concepts],
"SummaryPoints":[
 "Absolute value represents nonnegative distance from a target.",
 "V-shaped functions have a vertex determined by horizontal and vertical shifts.",
 "An equation |A|=c has no real solution if c<0.",
 "Inside inequalities produce bounded intervals; outside inequalities usually produce two rays.",
 "Always check boundary inclusions, intercepts, graph window and physical units.",
 "Teacher reviews diagrams and source-skill reasoning in Stage C."
],
"Visuals":visuals,"Videos":[],
"CommonMistakes":[
 {"Mistake":"Splitting |A|=−2 into two equations.","WhyWrong":"Absolute values cannot be negative.","Correction":"Conclude no real solutions before splitting."},
 {"Mistake":"Writing an OR statement for |x−a|<d.","WhyWrong":"Inside a radius imposes simultaneous lower and upper bounds.","Correction":"Use a−d<x<a+d for d>0."},
 {"Mistake":"Neglecting to reverse inequality when dividing by a negative multiplier.","WhyWrong":"This reverses which real values satisfy the inequality.","Correction":"Reverse the comparison direction on negative division."},
 {"Mistake":"Confusing the vertex location with graph intercepts.","WhyWrong":"Vertex generally has nonzero x or y coordinate.","Correction":"Set x=0/y=0 explicitly to find intercepts."}]}
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l06.rich-lesson-v2.json"
assert not rp.exists()
rp.write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U01:L06","ContentVersion":"g12-u01-l06-firstparty-source-skill-a-b-c-20261107","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u01-l06-firstparty-tryit-teacher-key-and-cooldown.json").write_text(json.dumps({
"OriginalSourceTryIts":7,"IndependentFirstPartyTryIts":7,"TeacherAnswers":[{"Number":i,"ExpectedMathematics":ans}for i,(_,_,ans)in enumerate(tryits,1)],
"IndependentCoolDown":cool,"SourceOriginalPublisherFiguresCopied":False,"StageD":"NOT_RUN"
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":1,"Lesson":6,
"SourceSection":"1.6","SourceSHA256":source["SourceSha256"],"OriginalSourcePdfPageRange":[105,115],
"SourceWorkedExampleSkillCount":7,"SourceTryItSkillCount":7,"SourceNumberedExercisesCount":64,
"FirstPartyWorkedExamplesAuthored":7,"FirstPartyTryItsAuthored":7,"FirstPartyConceptsAuthored":7,
"FirstPartyTableVisualsAuthored":2,"IndependentCoolDownAuthored":True,
"RichV2Path":rp.relative_to(root).as_posix(),
"Source64NumberedExerciseSkillRoutes":"IN_PROGRESS","SourceFigureDiagramSkillCrosswalk":"IN_PROGRESS",
"OriginalPublisherExactPixelAndVerbatimParityCertified":False,
"ReadyForStageD":False,"StageD":"NOT_RUN","CI":"NOT_RUN",
"UpdatedUtc":datetime.now(timezone.utc).isoformat()
}
(base/"u01-l06-abc-checkpoint.json").write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
pfile=base/"full-grade-progress.json"
p=json.loads(pfile.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==5 and p["FirstIncompleteLessonOrdinal"]==6
entry=p["LessonEntries"][5]
entry["ACStatus"]="A_C_IN_PROGRESS"
entry["RichV2Path"]=cp["RichV2Path"]
entry["OriginalSourceNumberedExercisesLocated"]=64
entry["EvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l06-abc-checkpoint.json"
entry["ReadyForStageD"]=False
entry["BlockingReasons"]=["Individual source 64 exercise skills and teacher expected answers need authoring","Source graph figure and visual anchors need review"]
p["UpdatedUtc"]=cp["UpdatedUtc"]
pfile.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U01_L06_FIRSTPARTY_7_EXAMPLES_7_TRYIT_7_CONCEPTS_2_TABLES_AND_COOLDOWN")
