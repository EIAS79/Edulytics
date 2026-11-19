#!/usr/bin/env python3
"""Fail-closed owner-authorized removal of old UAE Grade 9 assets ONLY.

No original lesson content is copied or backed up. Validate the exact scope of
removed General and Advanced Grade 9 lesson resources and framework nodes;
all unrelated UAE and non-UAE academic content must remain identical.
"""
import json
import subprocess
from pathlib import Path

CORE_PACK="src/Edulytics.Core/Curriculum/Packs/uae-moe-math.curriculum-pack.json"
OLD_CONTENT_PATHS=(
 "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-advanced-t1-ogl-v1.lesson-content-pack.json",
 "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-t1-ogl-v1.lesson-content-pack.json",
 "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-moe-g9-advanced-official-full-v2.lesson-content-pack.json",
 "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-moe-math-g9-advanced-t1-pilot.lesson-content-pack.json",
)
OLD_BLUEPRINT_PATHS=(
 "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g9-advanced-t1-ogl-v1.lesson-blueprint.json",
 "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g9-general-t1-ogl-v1.lesson-blueprint.json",
)
OLD_OTHER_PATHS=(
 "docs/curriculum/UAE_G9_GENERAL_OFFICIAL_MANIFEST.json",
 "docs/curriculum/UAE_G9_ADVANCED_OFFICIAL_MANIFEST.json",
 "src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-advanced-official.rich-lesson-v2.json",
)
ALL_OLD=OLD_CONTENT_PATHS+OLD_BLUEPRINT_PATHS+OLD_OTHER_PATHS
SOURCE_DIGEST="fc15730084f266a4384265c5382697edfaa52b5333bee70a69a52b757aa311d6"
CONTENT_DIGEST="51eddf58a55de58b6a1861b618ec7d6dc1ceb8a08f3f04a2ccb5d78d8f94768b"
RETIRED_KINDS=frozenset({"Reference","SourceCatalog","Lesson","Unit","TermScope"})

def target(node):
    return (node["LogicalLevelFrom"]==9 and node["LogicalLevelTo"]==9
            and node["Kind"] in RETIRED_KINDS)

def _main(root,path):
    p=subprocess.run(["git","show",f"origin/main:{path}"],cwd=root,
        capture_output=True)
    if p.returncode!=0:
        return None
    return json.loads(p.stdout.decode("utf-8-sig"))

def verify(root):
    root=Path(root)
    for path in ALL_OLD:
        if (root/path).exists():
            raise ValueError("Owner-retired UAE Grade9 asset reintroduced: "+path)
    pack=json.loads((root/CORE_PACK).read_text(encoding="utf-8-sig"))
    remaining=pack["Nodes"]
    if (pack["PackCode"]!="UAE-MOE-MATH" or
        pack["NodeCount"]!=len(remaining) or len(remaining)!=1234 or
        pack["OfficialNodeCount"]!=1180 or
        pack["UnitCount"]!=0 or pack["LessonCount"]!=0 or
        pack["LinkCount"]!=0 or pack["Links"] or
        pack["SourceDigest"]!=SOURCE_DIGEST or
        pack["ContentDigest"]!=CONTENT_DIGEST):
        raise ValueError("Owner-retired UAE Grade9 pack count or digest drift")
    if any(target(node) for node in remaining):
        raise ValueError("Old UAE Grade9 catalog, reference, unit or Lesson remains")
    if sum(x["IsOfficial"] and x["Kind"] in ("Standard","Outcome","Reference")
           for x in remaining)!=1180:
        raise ValueError("Unrelated official UAE standard/reference content drift")
    if sum(x["Kind"]=="Reference" for x in remaining)!=1133:
        raise ValueError("Unrelated UAE textbook reference count drift")
    codes={x["Code"] for x in remaining}
    if len(codes)!=1234 or any(x.get("ParentCode") not in (None,*codes) for x in remaining):
        raise ValueError("Duplicate UAE nodes or broken non-G9 parent")
    before=_main(root,CORE_PACK)
    if before is not None:
        removed=[x for x in before["Nodes"] if target(x)]
        if removed:
            if (before["NodeCount"]!=1467 or len(removed)!=233 or
                len(before["Links"])!=48 or
                sum(x["Kind"]=="Reference" for x in removed)!=178 or
                sum(x["Kind"]=="Lesson" for x in removed)!=42 or
                sum(x["Kind"]=="Unit" for x in removed)!=6 or
                sum(x["Kind"]=="SourceCatalog" for x in removed)!=6 or
                sum(x["Kind"]=="TermScope" for x in removed)!=1 or
                remaining!=[x for x in before["Nodes"] if not target(x)]):
                raise ValueError("UAE Grade9 removal changed unrelated node data/order")
            removed_codes={x["Code"] for x in removed}
            if not all(x["FromCode"] in removed_codes or x["ToCode"] in removed_codes
                       for x in before["Links"]):
                raise ValueError("Grade9 removal would discard unrelated standard links")
            allowed={"Nodes","Links","NodeCount","OfficialNodeCount",
                     "UnitCount","LessonCount","LinkCount",
                     "SourceDigest","ContentDigest"}
            if set(before)!=set(pack) or any(before[k]!=pack[k]
                    for k in before if k not in allowed):
                raise ValueError("UAE Grade9 deletion altered unrelated core-pack metadata")
        elif before!=pack:
            raise ValueError("Retired UAE core pack unexpectedly changed after merge")
    for path in ALL_OLD:
        # A baseline may already be retired once the PR is merged.
        baseline=_main(root,path)
        if baseline is not None and path in OLD_CONTENT_PATHS:
            if baseline.get("PackCode",baseline.get("packCode"))!="UAE-MOE-MATH":
                raise ValueError("Deletion manifest refers to non-UAE content: "+path)
    return {
      "oldContentPaths":OLD_CONTENT_PATHS,
      "oldBlueprintPaths":OLD_BLUEPRINT_PATHS,
      "corePackPath":CORE_PACK,
      "removedGrade9Nodes":233,
      "removedGrade9Links":48,
      "remainingNonGrade9UaeNodes":1234
    }

if __name__=="__main__":
    result=verify(Path(__file__).resolve().parents[2])
    print("UAE_G9_OWNER_APPROVED_NO_BACKUP_RETIREMENT_PASS",result)
