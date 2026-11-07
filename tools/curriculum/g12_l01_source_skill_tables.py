from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
richpath=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l01.rich-lesson-v2.json"
testpath=root/"tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
j=json.loads(richpath.read_text(encoding="utf-8"))
lesson=j["Lessons"][0]
assert len(lesson["Visuals"])==7,len(lesson["Visuals"])
def record(n,title,description,rows):
    return {
        "Kind":"RatioTable",
        "Title":f"Source-skill table {n}: {title}",
        "Description":description,
        "PrimaryLabel":"Input / category",
        "SecondaryLabel":"Output / mapping",
        "Rows":[{"Label":k,"Cells":v} for k,v in rows],
        "Items":[],"SecondaryItems":[]
    }
data=[
record(1,"Grade bands",
    "A fixed grading scale maps each percentage interval to exactly one GPA; many percentages share one GPA.",
    [("Percentage",["0–56","57–61","62–66","67–71","72–77","78–86","87–91","92–100"]),
     ("GPA",["0.0","1.0","1.5","2.0","2.5","3.0","3.5","4.0"])]),
record(2,"Names mapped to ranks",
    "Five distinct historical example names, each paired to exactly one rank. The rank/name direction is functionally invertible within this sample.",
    [("Name",["Babe Ruth","Willie Mays","Ty Cobb","Walter Johnson","Hank Aaron"]),
     ("Rank",["1","2","3","4","5"])]),
record(3,"Month lengths in a non-leap year",
    "Months 1–12 map to the number of calendar days in a non-leap year. Different months can share the same length.",
    [("Month",list(map(str,range(1,13)))),
     ("Days",["31","28","31","30","31","30","31","31","30","31","30","31"])]),
record(4,"Repeated numeric outputs",
    "Inputs 1 through 5 have exactly one output each even though outputs 8 and 6 repeat at other inputs.",
    [("n",["1","2","3","4","5"]),
     ("Q",["8","6","7","6","8"])]),
record(5,"Age–height nonfunction relation",
    "The age 5 appears twice with distinct output heights 40 and 42 inches, so the tabulated relation is not a function.",
    [("Age (years)",["5","5","6","7","8","9","10"]),
     ("Height (inches)",["40","42","44","47","50","52","54"])]),
record(6,"Function table A",
    "Each listed input has one output; all inputs and outputs are distinct.",
    [("Input",["2","5","8"]),("Output",["1","3","6"])]),
record(7,"Function table B",
    "Input/output relation is a function but is not one-to-one, since two different inputs yield 5.",
    [("Input",["−3","0","4"]),("Output",["5","1","5"])]),
record(8,"Nonfunction table",
    "Input 5 repeats with conflicting outputs 2 and 4; this table is not a function.",
    [("Input",["1","5","5"]),("Output",["0","2","4"])]),
record(9,"Exponential-looking lookup table",
    "Each input 1, 2, 3 has exactly one output, 10, 100, 1000.",
    [("Input",["1","2","3"]),("Output",["10","100","1000"])]),
record(10,"Animal memory-hour lookup",
    "Real-world example categories map to an hours figure in the source's illustrative data; each category has one value.",
    [("Type",["Puppy","Adult dog","Cat","Goldfish","Betta fish"]),
     ("Hours",["0.008","0.083","16","2160","3600"])]),
record(11,"Evaluate and invert a finite table",
    "For g(n) use the column for n=3 to evaluate, and find both input columns with output 6.",
    [("n",["1","2","3","4","5"]),("g(n)",["8","6","7","6","8"])]),
record(12,"Letter grade as one-to-one function",
    "Four distinct letter inputs have four different decimal point outputs.",
    [("Letter",["A","B","C","D"]),("Points",["4.0","3.0","2.0","1.0"])]),
record(13,"Basic toolkit functions",
    "Reference for nine common function families; separate authentic graph and sample table representations are still required for full visual parity.",
    [("Family",["Constant","Identity","Absolute value","Quadratic","Cubic","Reciprocal","Reciprocal squared","Square root","Cube root"]),
     ("Formula",["f(x)=c","f(x)=x","f(x)=|x|","f(x)=x²","f(x)=x³","f(x)=1/x","f(x)=1/x²","f(x)=√x","f(x)=∛x"]),
     ("Real domain",["all real","all real","all real","all real","all real","x≠0","x≠0","x≥0","all real"])]),
record(14,"Exercise input–output table",
    "Graph and arithmetic lookup: f(3)=53, while f(x)=1 in the displayed table occurs only at x=2.",
    [("x",list(map(str,range(10)))),
     ("f(x)",["74","28","1","53","56","3","36","45","14","47"])])
]
assert len(data)==14
assert len({v["Title"] for v in data})==14
lesson["Visuals"].extend(data)
assert len(lesson["Visuals"])==21
j["ContentVersion"]="g12-section11-21-first-party-visuals-including-14-source-skill-tables-20261106"
richpath.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

canonpath=root/"src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g12-precalculus-u01-l01-abc-draft.lesson-content-pack.json"
canon=json.loads(canonpath.read_text(encoding="utf-8"))
trans=canon["Lessons"][0]["Translations"][0]
trans["Explanation"]+="\n\nTables for Section 1.1: each input can have exactly one output while several distinct inputs may share a result. The 14 first-party source-skill table representations include menu/grade interpretation, nonnumeric inputs, tabulated function tests, month/day values, lookup and inverted lookup, basic toolkit families, and the Section exercise table. Inspect all original axes, figure captions and source table numerals separately when grading original exercises."
canon["ContentVersion"]="g12-precalculus-u01-l01-source-skill-tables-20261106"
canonpath.write_text(json.dumps(canon,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

test=testpath.read_text(encoding="utf-8")
assert test.count("Assert.Equal(7, lesson.Visuals.Count);")==1
test=test.replace("Assert.Equal(7, lesson.Visuals.Count);","Assert.Equal(21, lesson.Visuals.Count);")
anchor='''        Assert.All(lesson.WorkedExamples, example =>'''
assert test.count(anchor)==1
addition='''        Assert.Equal(14, lesson.Visuals.Count(visual =>
            visual.Title.StartsWith("Source-skill table ", StringComparison.Ordinal)));
        Assert.Contains(lesson.Visuals, visual =>
            visual.Title == "Source-skill table 14: Exercise input–output table" &&
            visual.Rows.Count == 2 && visual.Rows[1].Cells.Count == 10);
'''
test=test.replace(anchor,addition+anchor)
testpath.write_text(test,encoding="utf-8",newline="\n")

coveragepath=base/"u01-l01-element-coverage.json"
coverage=json.loads(coveragepath.read_text(encoding="utf-8"))
for el in coverage["Elements"]:
    if el["Category"]=="TABLE":
        number=int(el["SourceElementId"].split("TABLE")[1])
        assert 1<=number<=14
        el["FirstPartyStructuredVisualTitle"]=data[number-1]["Title"]
        el["FirstPartyStructuredVisualFile"]="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l01.rich-lesson-v2.json"
        el["VisualSemanticStatus"]="FIRST_PARTY_TABLE_AUTHORED_NOT_FULL_SOURCE_TABLE_FIGURE_PARITY"
coverage["AuthoringEvidence"]["RichV2FirstPartyStructuredVisuals"]=21
coverage["AuthoringEvidence"]["FirstPartySourceSkillTablesAuthored"]=14
coverage["AuthoringEvidence"]["SourceSemanticMatchesCertified"]=0
coveragepath.write_text(json.dumps(coverage,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

assessmentpath=base/"u01-l01-assessment.json"
ass=json.loads(assessmentpath.read_text(encoding="utf-8"))
ass["VisualRepresentationDrafts"]["FirstPartySourceSkillTableVisualCount"]=14
ass["VisualRepresentationDrafts"]["OriginalToolkitGraphsReproduced"]=False
assessmentpath.write_text(json.dumps(ass,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

cp_path=base/"u01-l01-abc-checkpoint.json"
cp=json.loads(cp_path.read_text(encoding="utf-8"))
cp["RichV2StructuredVisualsAuthored"]=21
cp["FirstPartySourceSkillTableVisualCount"]=14
cp["StageB"]="16_EXAMPLES_12_TRYITS_21_FIRST_PARTY_STRUCTURED_VISUALS_PLUS_19_EXERCISE_GRAPHS; NOT_FULL_SOURCE_MEDIA_PARITY"
cp["ReadyForStageD"]=False
cp_path.write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

progresspath=base/"full-grade-progress.json"
pg=json.loads(progresspath.read_text(encoding="utf-8"))
pg["LessonEntries"][0]["EdulyticsStructuredVisualsAuthored"]=21
pg["LessonEntries"][0]["FirstPartySourceSkillTableVisualCount"]=14
pg["LessonEntries"][0]["BContentStatus"]="16_EXAMPLES_12_TRYITS_21_SOURCE_SKILL_TABLE_EQUATION_VISUALS_AND_19_FIRST_PARTY_EXERCISE_GRAPHS"
pg["LessonEntries"][0]["ReadyForStageD"]=False
progresspath.write_text(json.dumps(pg,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("14_SOURCE_SKILL_TABLES_AUTHORED_TO_RICH_V2_21_VISUALS_TOTAL")
