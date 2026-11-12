import argparse,json
from pathlib import Path
r=Path(__file__).resolve().parents[2]
parser=argparse.ArgumentParser()
for f in ("unit","index","ordinal"):parser.add_argument("--"+f,type=int,required=True)
for f in ("title","unit-title"):parser.add_argument("--"+f,required=True)
args=parser.parse_args()
code=f"PED:UAE-MOE-MATH:L4:COMMON:{args.unit:02}:{args.index:02}"
def rd(p):return json.loads((r/p).read_text(encoding="utf-8-sig"))
def wr(p,obj):(r/p).write_text(json.dumps(obj,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
bp="src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json"
mp="docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json"
b,m=rd(bp),rd(mp)
l=next(x for x in b["Lessons"] if x["LessonCode"]==code)
assert l["SortOrder"]==args.ordinal
assert len(m["Units"][args.unit-1]["ls"])>=args.index
if l["Title"]!=args.title:
    print("FIX TITLE",l["Title"],"->",args.title)
l["Title"]=args.title
l["UnitTitle"]=args.unit_title
b["Units"][args.unit-1]["Title"]=args.unit_title
m["Units"][args.unit-1]["ls"][args.index-1]=args.title
m["Units"][args.unit-1]["title"]=args.unit_title
assert b["Units"][args.unit-1]["LessonCount"]==len(m["Units"][args.unit-1]["ls"])
assert len(b["Lessons"])==len(m["Units"][0]["ls"])+sum(len(u["ls"]) for u in m["Units"][1:])
wr(bp,b);wr(mp,m)
print("VERIFIED INDEX",code,"ordinal",args.ordinal,args.title)
