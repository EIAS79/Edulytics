import argparse,hashlib,json
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding="utf-8-sig"))
def dump(p,obj):p.write_text(json.dumps(obj,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
def main():
    parser=argparse.ArgumentParser()
    parser.add_argument("spec",type=Path)
    parser.add_argument("--new-engine",required=False)
    parser.add_argument("--anchor-engine",required=False)
    args=parser.parse_args()
    spec=load(args.spec)
    code=spec["LessonCode"]
    unit,index,ordinal=(spec[k] for k in ("UnitIndex","LessonIndex","Ordinal"))
    assert code==f"PED:UAE-MOE-MATH:L4:COMMON:{unit:02}:{index:02}"
    progress=load(ROOT/"artifacts/curriculum-delivery/UAE-MOE-MATH/G4/full-grade-progress.json")
    assert progress["FirstUnclosedLessonOrdinal"]==ordinal,progress
    src=Path(r"C:\Users\khali\Downloads\New folder (2)\emirates\Grade_04\01_Student_Books")/spec["StudentPdf"]
    assert src.is_file()
    sha=hashlib.sha256(src.read_bytes()).hexdigest()
    assert sha==spec["SourceSha256"]
    assert spec["PrintedEnd"]-spec["PrintedStart"]==spec["PdfEnd"]-spec["PdfStart"]
    rich_path=ROOT/f"src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u{unit:02}-l{index:02}-ar.rich-lesson-v2.json"
    lesson=load(rich_path)["Lessons"][0]
    assert lesson["LessonCode"]==code and lesson["Title"]==spec["Title"]
    assert len(lesson["KeyConcepts"])>=3 and len(lesson["WorkedExamples"])>=4
    assert all(len(x["Steps"])>=3 for x in lesson["WorkedExamples"])
    assert all("???" not in x for x in lesson["ExplanationParagraphs"])
    blueprint=load(ROOT/"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json")
    b=next(x for x in blueprint["Lessons"] if x["LessonCode"]==code)
    assert b["SortOrder"]==ordinal and b["Title"]==spec["Title"]
    cp=ROOT/"src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json"
    pack=load(cp)
    x=next(z for z in pack["Lessons"] if z["LessonCode"]==code)
    x["SourceSha256"]=sha
    x["TitleSourceReference"]=f"{spec['StudentPdf']} PDF pp{spec['PdfStart']}-{spec['PdfEnd']} printed pp{spec['PrintedStart']}-{spec['PrintedEnd']}"
    x["SourceEdition"]="2025-2026 local Arabic student source; 2026-2027 full annual scope not independently verified"
    x["AdaptationStatus"]="Independently authored Arabic Edulytics lesson, source skill matched; A-B-C tests required; Stage D not run"
    x["Translations"]=[z for z in x["Translations"] if z["CultureCode"]!="ar"]
    x["Translations"].append({"CultureCode":"ar","Title":lesson["Title"],
    "Explanation":" ".join(lesson["ExplanationParagraphs"]),
    "KeyConceptsAndRules":" ".join(z["Rule"] for z in lesson["KeyConcepts"]),
    "WorkedExamples":" ".join(z["Question"]+" "+z["Answer"] for z in lesson["WorkedExamples"]),
    "StepByStepSolutions":" ".join(" ".join(z["Steps"]) for z in lesson["WorkedExamples"]),
    "CommonMistakes":" ".join(z["Correction"] for z in lesson["CommonMistakes"]),
    "QuickSummary":" ".join(lesson["SummaryPoints"])})
    dump(cp,pack)
    contracts=ROOT/"src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs"
    cs=contracts.read_text(encoding="utf-8-sig")
    previous=next(x["LessonCode"] for x in blueprint["Lessons"] if x["SortOrder"]==ordinal-1)
    anchor=f'        byLesson["{previous}"] = new LessonPracticeContract('
    assert cs.count(anchor)==1 and f'byLesson["{code}"]' not in cs
    families=", ".join('"'+f+'"' for f in spec["Families"])
    insertion=f'''        byLesson["{code}"] = new LessonPracticeContract(
            "{code}", "{spec['SkillId']}",
            "{spec['StrategyCode']}",
            [{families}],
            "UAE_G4_STUDENT_{spec['StudentPdf'].replace('.pdf','').replace('-','_')}_PP{spec['PrintedStart']}_{spec['PrintedEnd']}_STAGE_D_NOT_RUN",
            "READY_VERIFIED", Version);
'''
    contracts.write_text(cs.replace(anchor,insertion+anchor,1),encoding="utf-8")
    reg=ROOT/"src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json"
    registry=load(reg)
    known={f["id"] for f in registry["families"]}
    new_families=spec.get("NewFamilies",[])
    for family in new_families:
        assert family not in known
        registry["families"].append({"id":family,"version":1,"skillId":spec["SkillId"],
        "requiredCapabilities":["lesson.practice.exact.generate","lesson.practice.exact.verify"],
        "answerType":"exact_integer",
        "verificationPolicy":f"g4-u{unit:02}-l{index:02}-abc-stage-d-not-run",
        "representations":spec["Representations"],"status":"ShadowVerified",
        "productionRouting":False,"lessonPracticeRouting":True})
    assert set(spec["Families"])<=known|set(new_families)
    dump(reg,registry)
    if args.new_engine:
        assert args.anchor_engine
        engine=ROOT/"src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs"
        s=engine.read_text(encoding="utf-8-sig")
        old,new=args.anchor_engine,args.new_engine
        rules=[
            (f"{old}.Supports(family.Trim()) ||",f"{new}.Supports(family.Trim()) ||\n          {old}.Supports(family.Trim()) ||"),
            (f"if ({old}.Supports(family))\n            return {old}.Verify(family, parameters, answer);",
             f"if ({new}.Supports(family))\n            return {new}.Verify(family, parameters, answer);\n        if ({old}.Supports(family))\n            return {old}.Verify(family, parameters, answer);"),
            (f"if ({old}.Supports(family))\n        {{",
             f"if ({new}.Supports(family))\n        {{\n            var q = {new}.Build(family,random);\n            return new ExactProblem(q.Family,q.Prompt,q.Solution,q.ItemType,q.Parameters);\n        }}\n        if ({old}.Supports(family))\n        {{"),
            (f"if ({old}.Supports(problem.Family))\n            return {old}.Solve(problem.Family, p);",
             f"if ({new}.Supports(problem.Family))\n            return {new}.Solve(problem.Family, p);\n        if ({old}.Supports(problem.Family))\n            return {old}.Solve(problem.Family, p);")]
        for a,b in rules:
            assert s.count(a)==1,(a,s.count(a))
            s=s.replace(a,b,1)
        engine.write_text(s,encoding="utf-8")
    print("ATTACHED",code,"to source",spec["StudentPdf"],"new families",len(new_families))
if __name__=="__main__":main()
