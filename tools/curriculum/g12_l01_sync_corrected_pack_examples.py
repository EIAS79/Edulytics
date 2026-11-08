from pathlib import Path
import json,re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
rich_path=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l01.rich-lesson-v2.json"
pack_path=root/"src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g12-precalculus-u01-l01-abc-draft.lesson-content-pack.json"
rich=json.loads(rich_path.read_text(encoding="utf-8"))
pack=json.loads(pack_path.read_text(encoding="utf-8"))
examples=rich["Lessons"][0]["WorkedExamples"]
t=pack["Lessons"][0]["Translations"][0]
assert len(examples)==16
for number in (5,6,14):
    item=examples[number-1]
    worked=(
        f"{number}. {item['Title']}\nTask: {item['Question']}\n"
        f"Method: {item['Method']}\n" +
        "\n".join(f"Step {i}: {v}" for i,v in enumerate(item["Steps"],1)) +
        f"\nAnswer: {item['Answer']}\nCheck: {item['Check']}\n\n"
    )
    solution=(
        f"{number}. {item['Title']}: " +
        " ".join(item["Steps"]) +
        f" Final: {item['Answer']}\n\n"
    )
    pattern=rf"(?ms)^{number}\. .*?(?=^\d+\. |\Z)"
    for field,replacement in (("WorkedExamples",worked),("StepByStepSolutions",solution)):
        s=t[field]
        found=re.search(pattern,s)
        assert found is not None,(number,field)
        t[field]=s[:found.start()]+replacement+s[found.end():]
        assert item["Answer"] in t[field],(number,field)
pack["ContentVersion"]="g12-precalculus-u01-l01-20261106-multipart-examples-richv2-pack-reconciled"
pack_path.write_text(json.dumps(pack,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("CANONICAL_PACK_SYNCED_3_CORRECTED_EXAMPLES_FROM_RICH_V2")
