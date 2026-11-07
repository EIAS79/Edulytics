#!/usr/bin/env python3
"""Surgical G12 course-selector + first-party structured visual support.
Pending A/B/C; no test or CI invocation here.
"""
import json
from pathlib import Path
repo=Path(__file__).resolve().parents[2]
level=repo/"src/Edulytics.Core/Curriculum/CurriculumLevelIdentity.cs"
s=level.read_text(encoding="utf8")
anchor='''    // Exact CourseCode, NativeLevel and Pathway from the nine accepted
    // src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-*.json files.'''
replacement='''    // CourseCode, NativeLevel, Pathway identity from reviewed source-backed
    // high-school Blueprints. PRECALC is an additional Grade 12-only course;
    // it must not become an implicit choice for earlier HS grade levels.'''
assert s.count(anchor)==1
s=s.replace(anchor,replacement)
anchor='''        new("VECTOR-MATRIX", "Vectors & Matrices", "Fourth Courses")
    ];'''
replacement='''        new("VECTOR-MATRIX", "Vectors & Matrices", "Fourth Courses"),
        new("PRECALC", "Precalculus", "Advanced Precalculus")
    ];'''
assert s.count(anchor)==1
s=s.replace(anchor,replacement)
anchor='''        foreach (var course in SourceBackedAmericanHighSchoolCourses)
        {
            yield return new CurriculumLevelIdentity('''
replacement='''        foreach (var course in SourceBackedAmericanHighSchoolCourses)
        {
            // Only Grade 12 has the reviewed OpenStax Precalculus source
            // baseline. Never expose an unrouted lower/higher grade adoption.
            if (course.Code == "PRECALC" && level.LogicalLevel != 13)
                continue;

            yield return new CurriculumLevelIdentity('''
assert s.count(anchor)==1
s=s.replace(anchor,replacement)
level.write_text(s,encoding="utf8")

rich=repo/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l01.rich-lesson-v2.json"
j=json.loads(rich.read_text(encoding="utf8"))
row=j["Lessons"][0]
assert row["Visuals"]==[],len(row["Visuals"])
row["Visuals"]=[
 {"Kind":"RatioTable","Title":"Different inputs may share one output",
  "Description":"This relation is a function because every displayed input has one output. It is not one-to-one because x=2 and x=3 both produce 5.",
  "PrimaryLabel":"Input","SecondaryLabel":"Output",
  "Rows":[
   {"Label":"x (input)","Cells":["1","2","3"],"Numerator":0,"Denominator":0},
   {"Label":"f(x) (output)","Cells":["4","5","5"],"Numerator":0,"Denominator":0}],
  "Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"One input with two different outputs",
  "Description":"The repeated input x=1 produces 2 and 4, so this relation is not a function. Treat these as two different ordered pairs, not a duplicate row.",
  "PrimaryLabel":"Input","SecondaryLabel":"Output",
  "Rows":[
   {"Label":"x (input)","Cells":["1","1","2"],"Numerator":0,"Denominator":0},
   {"Label":"y (output)","Cells":["2","4","4"],"Numerator":0,"Denominator":0}],
  "Items":[],"SecondaryItems":[]},
 {"Kind":"EquationSet","Title":"Vertical-line test: parabola and sideways parabola",
  "Description":"For the same x=4, y=x² gives only y=16, but x=y² gives y=2 and y=−2. Ask learners to sketch both coordinate graphs and test the vertical line x=4.",
  "PrimaryLabel":"","SecondaryLabel":"","Items":["y = x²: (4, 16)","x = y²: (4, 2) and (4, −2)","Same x with two distinct y-values is NOT a y-function"],
  "SecondaryItems":[],"Rows":[]},
 {"Kind":"EquationSet","Title":"Horizontal-line test under an explicit domain",
  "Description":"At y=4, the unrestricted quadratic f(x)=x² gives two different inputs, but on the domain x≥0 only x=2 remains. Compare the graphs with a teacher.",
  "PrimaryLabel":"","SecondaryLabel":"","Items":["f(−2) = 4 and f(2) = 4","On ℝ: not one-to-one","On x≥0: one-to-one"],
  "SecondaryItems":[],"Rows":[]},
 {"Kind":"RatioTable","Title":"Basic toolkit functions on three input values",
  "Description":"Compare a function's formula with its values; infer constant, identity, quadratic, cubic, and absolute-value graph shapes. Sketch these first-party table-based examples on a coordinate grid.",
  "PrimaryLabel":"Function","SecondaryLabel":"Values at -1, 0, 1",
  "Rows":[
   {"Label":"x","Cells":["−1","0","1"],"Numerator":0,"Denominator":0},
   {"Label":"f(x)=2","Cells":["2","2","2"],"Numerator":0,"Denominator":0},
   {"Label":"g(x)=x","Cells":["−1","0","1"],"Numerator":0,"Denominator":0},
   {"Label":"h(x)=x²","Cells":["1","0","1"],"Numerator":0,"Denominator":0},
   {"Label":"k(x)=x³","Cells":["−1","0","1"],"Numerator":0,"Denominator":0},
   {"Label":"a(x)=|x|","Cells":["1","0","1"],"Numerator":0,"Denominator":0}
  ],"Items":[],"SecondaryItems":[]}
]
j["ContentVersion"]="g12-openstax-u01-l01-abc-authoring-visual-representations-20261106"
rich.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
assessment=repo/"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-assessment.json"
a=json.loads(assessment.read_text(encoding="utf8"))
a["VisualRepresentationDrafts"]={
 "Authoring":"Edulytics first-party table and equation visuals, not original PDF images",
 "StructuredRichV2VisualsWritten":5,
 "SourceFigureOneToOneParityVerified":False,
 "GraphRenderer":"NOT_IN_RICHV2_CORE; teacher-led coordinate sketch required",
 "Accessibility":"Full equation/table descriptions provided; actual accessible graphical interaction still pending",
 "StageD":"NOT_RUN"}
assessment.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("G12_COURSE_IDENTITY_GRADE12_ONLY_UPDATED 5_FIRST_PARTY_RICH_V2_VISUALS")
