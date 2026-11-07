from pathlib import Path
import json,re
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson005.json").read_text(encoding="utf-8"))
assert source["Section"]=="1.5"
tasks={}
def add(n,category,question,expected,parts=None):
 assert n not in tasks,n
 tasks[n]={"Number":n,"SourcePDFPage":101 if n<=30 else 102 if n<=38 else 103 if n<=46 else 104,
 "SourceSkillGroup":category,"FirstPartyStudentPrompt":question,
 "TeacherExpectedMathematics":expected,
 "OriginalSourcePedagogicActions":parts or ["compute and justify complete transformation"],
 "FirstPartyIndependentMathSourceSkillAlternative":True,
 "OriginalPublisherPixelOrExactVerbatimParityClaim":False,
 "TeacherManualScoring":"AUTHORING_COMPLETE_RUNTIME_NOT_TESTED"}
verbal=[
("Explain the difference between an input shift and an output shift in g(x)=f(x−3)+2.","An input shift moves right 3; an output shift moves up 2."),
("Compare a horizontal scaling g(x)=f(2x) with vertical scaling h(x)=2f(x).","g compresses horizontally by 1/2; h stretches vertically by 2."),
("Contrast 0.5f(x) with f(0.5x).","0.5f(x) halves outputs; f(0.5x) doubles x-scale."),
("Describe reflections g(x)=−f(x) and h(x)=f(−x) and what each axis means.","Negating output reflects across x-axis; negating input reflects across y-axis."),
("Explain an algebraic test of even and odd parity and the domain prerequisite.","Even: f(−x)=f(x); odd: f(−x)=−f(x); symmetric domain needed.")
]
for i,(q,a)in enumerate(verbal,1):add(i,"VERBAL",q,a)
for n,parent,inside,outside,answer in [
(6,"sqrt","x+2","+1","√(x+2)+1, domain [−2,∞)"),
(7,"absolute","x−1","−3","|x−1|−3, vertex (1,−3)"),
(8,"reciprocal","x−3","−4","1/(x−3)−4, x≠3"),
(9,"reciprocal-square","x+4","+2","1/(x+4)²+2, x≠−4")
]:
 add(n,"SHIFT_TOOLKIT_FORMULA",
     f"Apply the original Section 1.5 source-skill shifts to the {parent} toolkit parent: substitute input ({inside}) and apply output shift ({outside}). Give formula and relevant domain.",
     answer,["translate input and output","derive final formula and domain"])
transforms={
10:("f(x−49)","right 49"),
11:("f(x+43)","left 43"),
12:("f(x+3)","left 3"),
13:("f(x−4)","right 4"),
14:("f(x)+5","up 5"),
15:("f(x)+8","up 8"),
16:("f(x)−2","down 2"),
17:("f(x)−7","down 7"),
18:("f(x−2)+3","right 2 then up 3"),
19:("f(x+4)−1","left 4 then down 1")
}
for n,(expr,a)in transforms.items():
 add(n,"TRANSLATION_DESCRIPTION",
    f"Classify the horizontal and vertical transformations in g(x)={expr}; explain input versus output units.",a)
monotonic={
20:("4(x+1)²−5","decreasing (−∞,−1), increasing (−1,∞); vertex (−1,−5)"),
21:("5(x+3)²−2","decreasing (−∞,−3), increasing (−3,∞); vertex (−3,−2)"),
22:("√(4−x)","decreasing (−∞,4); domain (−∞,4]"),
23:("−3√x−1","decreasing (0,∞); domain [0,∞)")
}
for n,(formula,answer)in monotonic.items():
 add(n,"INCREASING_DECREASING_OF_TRANSFORMED_TOOLKIT",
    f"Analyze the real function f(x)={formula}. State the permitted x-domain and intervals of increase/decrease, with any turning point or endpoint.",
    answer,["identify real domain","classify increasing and decreasing intervals"])
for n,form,description in [
(24,"2^x+1","Shift parent 2^x up by 1."),
(25,"2^x−3","Shift parent 2^x down by 3."),
(26,"2^x−1","Shift parent 2^x down by 1.")]:
 add(n,"EXPONENTIAL_SHIFT_GRAPH",
    f"Sketch y={form} from independent labelled points of y=2^x at x=−1,0,1,2. Identify the transformed horizontal asymptote.",
    description+f" Graph y={form} with all real inputs.",
    ["plot four transformed coordinates","write horizontal asymptote"])
toolkit={
27:("(t+1)²−3","quadratic shifted left 1 and down 3, vertex (−1,−3)"),
28:("|x−1|+4","absolute value shifted right 1 and up 4, vertex (1,4)"),
29:("(x−2)³−1","cubic shifted right 2 and down 1, inflection at (2,−1)"),
30:("3+√(t+2)","square-root shifted left 2 and up 3, startpoint (−2,3)")
}
for n,(form,ans)in toolkit.items():
 add(n,"DRAW_TOOLKIT_TRANSLATED_GRAPH",
     f"Independently sketch the function f(x)={form} with axes, key points, endpoints, and permitted domain. Name the transformed toolkit parent.",
     ans,["identify parent","label shifts","draw correct shape and endpoints"])
for n,original_f,transformed_g,transformed_h,answer in [
(31,"f(-2)=−2,f(-1)=−1,f(0)=−3,f(1)=1,f(2)=2",
 "g(-1)=−2,g(0)=−1,g(1)=−3,g(2)=1,g(3)=2",
 "h(-2)=−1,h(-1)=0,h(0)=−2,h(1)=2,h(2)=3",
 "g(x)=f(x−1), h(x)=f(x)+1"),
(32,"f(-2)=−1,f(-1)=−3,f(0)=4,f(1)=2,f(2)=1",
 "g(-3)=−1,g(-2)=−3,g(-1)=4,g(0)=2,g(1)=1",
 "h(-2)=−2,h(-1)=−4,h(0)=3,h(1)=1,h(2)=0",
 "g(x)=f(x+1), h(x)=f(x)−1")
]:
 add(n,"TABULAR_HORIZONTAL_AND_VERTICAL_IDENTIFICATION",
     f"Given tabulated {original_f}; {transformed_g}; {transformed_h}. Express g,h as input/output translations of f using the same finite source-skill representation.",
     answer,["identify g input shift","identify h output shift"])
# The original 33–46 contain graph-only figures. Independent, mathematically
# specific graph contracts are teacher-graded, NOT assertions that the
# publisher's original plotted coordinates have been reconstructed.
fig_models=[
("y=(x−2)²+1","parent x² shifted right 2 up 1"),
("y=−(x+1)²","quadratic reflected across x-axis and left 1"),
("y=|x+2|−3","absolute value left 2 down 3"),
("y=√(x+1)−2","square root left 1 down 2"),
("y=−√(4−x)","horizontal reflection and left endpoint transform; domain x≤4"),
("y=(x+1)³−2","cubic left 1 and down 2"),
("y=2|x−1|+1","V shape right1, vertical stretch2, up1"),
("y=−|x+3|+2","absolute value reflected over x-axis, left3 up2"),
("y=√(x−2)+3","root right2 up3"),
("y=−√(x+4)","root left4 reflected across x-axis"),
("y=3(x−1)²−4","quadratic right1, vertical stretch3, down4"),
("y=−2(x+1)³","cubic left1, vertical stretch2 and x-axis reflection"),
("y=(1/2)|x−2|","absolute value right2 vertical compression1/2"),
("y=1/(x+2)+1","reciprocal left2 up1, excluded x=−2")
]
assert len(fig_models)==14
for n,(expr,transform)in enumerate(fig_models,33):
 add(n,"GRAPH_TO_TOOLKIT_EXPRESSION",
     f"An independently drawn source-skill coordinate graph has rule {expr}. Sketch the labelled transformed parent, then derive its formula from the shape, shift, reflection and/or scale; verify key endpoints or asymptotes.",
     transform+"; verify formula "+expr+".",
     ["identify source parent graphic skill","state transformation","give matching formula and domain"])
parity={
47:("3x⁴","even"),
48:("√x","neither; real domain x≥0 not symmetric"),
49:("1/x+3x","odd, on symmetric domain x≠0"),
50:("(x−2)²","neither"),
51:("2x⁴","even"),
52:("2x−x³","odd")
}
for n,(formula,answer)in parity.items():
 add(n,"EVEN_ODD_NEITHER",
    f"Classify f(x)={formula} as even, odd or neither. Compute f(−x), compare with f(x) and −f(x), and check symmetry of the domain.",
    answer,["domain symmetry check","f(-x) identity","classification"])
scales={
53:("−f(x)","reflect over horizontal x-axis"),
54:("f(−x)","reflect over vertical y-axis"),
55:("4f(x)","vertical stretch 4"),
56:("6f(x)","vertical stretch 6"),
57:("f(5x)","horizontal compression to 1/5"),
58:("f(2x)","horizontal compression to 1/2"),
59:("f(x/3)","horizontal stretch 3"),
60:("f(x/5)","horizontal stretch 5"),
61:("3f(−x)","reflect across y-axis then vertical stretch 3"),
62:("−f(3x)","horizontal compression 1/3 then reflect across x-axis")
}
for n,(expr,ans)in scales.items():
 add(n,"SCALE_AND_REFLECT_GENERAL_FUNCTION",
     f"Given g(x)={expr}, describe the transformation of a generic graph f, including whether the factor applies to input or output.",
     ans)
complex_forms={
63:("|−4x|","4|x|; y-axis reflection then horizontal compression factor 1/4 of |x|"),
64:("−√(x/2)","negative square root, horizontal stretch 2 then x-axis reflection; domain x≥0"),
65:("(1/3)/(x+2)²−3","vertical compression 1/3, left 2, down 3; x≠−2"),
66:("8/(x−4)+2","vertical stretch 8, right 4, up 2; x≠4"),
67:("(1/2)(x−5)²+1","quadratic vertical compression half, right 5, up 1"),
68:("((x+4)/3)²−3","quadratic horizontal stretch 3, left 4, down 3")
}
for n,(formula,ans)in complex_forms.items():
 add(n,"COMBINED_TOOLKIT_TRANSFORM_FORMULA",
     f"Using the transformation order given for original source skill {n}, sketch and analyze the independent resulting function g(x)={formula}. Identify each shift/scale/reflection and relevant domain.",
     ans,["sketch resulting graph","decompose transformations","state domain"])
graph_functions={
69:("4(x+1)²−5","vertex (−1,−5), up-opening quadratic, width compressed vertically via factor4"),
70:("5(x+3)²−2","vertex (−3,−2), up-opening quadratic, vertical stretch5"),
71:("−2|x−4|+3","vertex (4,3), reflected V opening down, stretch2"),
72:("−3√x−1","startpoint (0,−1), reflected root with vertical stretch3, x≥0"),
73:("(1/2)x³","cubic vertically compressed by 1/2"),
74:("(1/3)|x−2|","V vertex (2,0), vertically compressed by 1/3"),
75:("(x/3)³−3","cubic horizontal stretch3, down3"),
76:("(x/4)³+1","cubic horizontal stretch4, up1"),
77:("√(4−x)","root reflected horizontally and right shifted4, x≤4")
}
for n,(form,ans)in graph_functions.items():
 add(n,"COMBINED_FUNCTION_GRAPH",
     f"Draw an independent graph of f(x)={form} from its transformed toolkit parent, naming shifts/reflections/scales and stating its real domain and key graph feature.",
     ans,["identify parent rule","graph and label transformation","give domain"])
for n,trans,ans in [
(78,"f(x)−2","subtract 2 from every plotted y, x unchanged"),
(79,"−f(x)","reflect every plotted point (x,y) to (x,−y)"),
(80,"f(x+1)","shift every original point left 1"),
(81,"f(x−2)","shift every original point right 2")
]:
 add(n,"TRANSFORM_ARBITRARY_SOURCE_GRAPH",
     f"An independent source-skill graph of f contains points (−2,1),(0,3),(2,0). Apply g(x)={trans}, then sketch the transformed points and join with the original corresponding graph shape.",
     ans,["transform three labelled coordinates","sketch points with correct horizontal/vertical orientation"])
assert sorted(tasks)==list(range(1,82))
native=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u01-l05-firstparty-teacher-led.json"
native.parent.mkdir(parents=True,exist_ok=True)
assert not native.exists()
assessment={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L05",
"SourceLocalBookSection":"OpenStax Precalculus 2017, Section 1.5",
"OriginalSourceNumberedExerciseSkills":81,
"FirstPartyStudentSkillEquivalentQuestionContracts":81,
"FirstPartyStudentQuestions":[{"Number":n,"SourceSkill":t["SourceSkillGroup"],"StudentPrompt":t["FirstPartyStudentPrompt"],"Grading":"TEACHER_MANUAL_REVIEW"}for n,t in sorted(tasks.items())],
"TeacherOnlyMathAnswers":[{"Number":n,"TeacherExpectedMath":t["TeacherExpectedMathematics"],"SourceActions":t["OriginalSourcePedagogicActions"]}for n,t in sorted(tasks.items())],
"OriginalPublisherGraphAssetsCopied":False,
"ExactOriginalPublisherGraphCoordinateParityClaim":False,
"OriginalSourceExactVerbatimParityClaim":False,
"LiveStudentDeliveryAndGradingNotTested":True,
"StageD":"NOT_RUN"}
native.write_text(json.dumps(assessment,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u01-l05-source-81-firstparty-question-contracts.json").write_text(json.dumps({
"SchemaVersion":1,"SourceSha256":source["SourceSha256"],
"OriginalNumberedSourceExercises":81,
"FirstPartyQuestionBank":81,"OriginalPublisherMediaCopied":0,
"OriginalSourceGraphExercisesAdaptedToIndependentMathModels":True,
"SourcePublisherExactVisualPixelFidelityCertified":False,
"StageD":"NOT_RUN","Items":[tasks[n]for n in sorted(tasks)]},
ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
checkpoint=base/"u01-l05-abc-checkpoint.json"
cp=json.loads(checkpoint.read_text(encoding="utf-8"))
cp["FirstPartyNumberedSourceSkillPracticeQuestionsAuthored"]=81
cp["TeacherOnlyMathAnswerRubricsAuthored"]=81
cp["FirstPartyAssessmentResourcePath"]=native.relative_to(root).as_posix()
cp["SourceNumberedExerciseRoutingManifest"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l05-source-81-firstparty-question-contracts.json"
cp["SourceFiguresSourceVisualReview"]="STILL_PENDING"
cp["ReadyForStageD"]=False
checkpoint.write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_L05_81_SOURCE_SKILL_FIRSTPARTY_TEACHER_REVIEW_QUESTION_BANK_AUTHORED")
