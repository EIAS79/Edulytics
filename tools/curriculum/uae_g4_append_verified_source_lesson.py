import argparse,copy,hashlib,json
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2]
def load(p):return json.loads(p.read_text(encoding="utf-8-sig"))
def save(p,obj):p.write_text(json.dumps(obj,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
def main():
    a=argparse.ArgumentParser()
    for field in ("unit","index","ordinal","printed-start","printed-end","pdf-start","pdf-end"):
        a.add_argument("--"+field,type=int,required=True)
    for field in ("title","unit-title","source"):
        a.add_argument("--"+field,required=True)
    args=a.parse_args()
    u,i,o=args.unit,args.index,args.ordinal
    code=f"PED:UAE-MOE-MATH:L4:COMMON:{u:02}:{i:02}"
    previous=f"PED:UAE-MOE-MATH:L4:COMMON:{u:02}:{i-1:02}"
    bp=ROOT/"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json"
    cp=ROOT/"src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json"
    mp=ROOT/"docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json"
    progress=load(ROOT/"artifacts/curriculum-delivery/UAE-MOE-MATH/G4/full-grade-progress.json")
    assert progress["FirstUnclosedLessonOrdinal"]==o and progress["LastClosedLessonOrdinal"]==o-1
    b,c,m=load(bp),load(cp),load(mp)
    assert 1<=u<=len(b["Units"])==len(m["Units"])
    assert b["Units"][u-1]["LessonCount"]==len(m["Units"][u-1]["ls"])==i-1
    assert len(b["Lessons"])==len(c["Lessons"])==m["LessonCount"]
    assert not any(x["LessonCode"]==code for x in b["Lessons"]+c["Lessons"])
    assert sorted(x["SortOrder"] for x in b["Lessons"])==list(range(1,len(b["Lessons"])+1))
    base=next(x for x in b["Lessons"] if x["LessonCode"]==previous)
    source=Path(args.source)
    assert source.is_file()
    assert args.printed_end-args.printed_start==args.pdf_end-args.pdf_start
    ref=f"UAE:REF:TEXTBOOK:G4:COMMON:T2:U{u:02}:L{i:02}"
    bnew=copy.deepcopy(base)
    bnew.update({"SourceLessonCode":f"EDU:UAE-MOE-MATH:L4:COMMON:U{u:02}:L{i:02}",
    "LessonCode":code,"UnitNumber":u,"UnitTitle":args.unit_title,"LessonNumber":i,
    "Title":args.title,"SortOrder":o,"OfficialReferenceCode":ref,
    "SemanticSha256":hashlib.sha256((code+"|"+args.title).encode()).hexdigest()})
    for alignment in bnew["Alignments"]:alignment["ReferenceCode"]=ref
    for lesson in b["Lessons"]:
        if lesson["SortOrder"]>=o:lesson["SortOrder"]+=1
    b["Lessons"].append(bnew);b["Lessons"].sort(key=lambda x:x["SortOrder"])
    b["Units"][u-1]["LessonCount"]+=1
    cnew=copy.deepcopy(next(x for x in c["Lessons"] if x["LessonCode"]==previous))
    cnew.update({"LessonCode":code,"OfficialReferenceCode":ref,
    "SourceLocator":f"{source.name} printed {args.printed_start}-{args.printed_end}",
    "TitleSourceReference":f"{source.name} PDF {args.pdf_start}-{args.pdf_end}",
    "SourceSha256":hashlib.sha256(source.read_bytes()).hexdigest(),
    "AdaptationStatus":"Source identity restored; Arabic authoring and academic closure pending"})
    cnew["Translations"]=[{"CultureCode":"en","Title":args.title,
    "Explanation":"Source identity placeholder pending separately authored Arabic Edulytics content.",
    "KeyConceptsAndRules":"Original Arabic skill descriptions to follow source audit.",
    "WorkedExamples":"Not yet authored.","StepByStepSolutions":"Not yet authored.",
    "CommonMistakes":"Not yet authored.","QuickSummary":"Not yet authored."}]
    c["Lessons"].append(cnew)
    m["Units"][u-1]["ls"].append(args.title)
    m["LessonCount"]+=1
    assert len(b["Lessons"])==len(c["Lessons"])==m["LessonCount"]
    assert sorted(x["SortOrder"] for x in b["Lessons"])==list(range(1,m["LessonCount"]+1))
    assert sum(x["LessonCount"] for x in b["Units"])==m["LessonCount"]
    assert sum(len(x["ls"]) for x in m["Units"])==m["LessonCount"]
    save(bp,b);save(cp,c);save(mp,m)
    print("RESTORED",code,"ordinal",o,"indexed_count",m["LessonCount"])
if __name__=="__main__":main()
