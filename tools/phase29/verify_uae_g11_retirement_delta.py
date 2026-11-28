#!/usr/bin/env python3
"""Owner-directed UAE Grade 11 retirement only; preserve every unrelated node."""
import hashlib
import json
import subprocess
from pathlib import Path

BASE = "5f249a9b3b27e8f2991ede798b56e7245d195a44"
PREFIX = "PED:UAE-MOE-MATH:L11:"
CORE_PACK = "src/Edulytics.Core/Curriculum/Packs/uae-moe-math.curriculum-pack.json"
CONTENT_PATHS = tuple(f"src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g11-{track}-t1-ogl-v1.lesson-content-pack.json" for track in ("general", "advanced"))
BLUEPRINT_PATHS = tuple(f"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g11-{track}-t1-ogl-v1.lesson-blueprint.json" for track in ("general", "advanced"))
MANIFEST_PATHS = tuple(f"docs/curriculum/UAE_G11_{track.upper()}_OFFICIAL_MANIFEST.json" for track in ("general", "advanced"))

def target(node):
    return (node["LogicalLevelFrom"] == node["LogicalLevelTo"] == 11 and node["Kind"] in ("Reference", "SourceCatalog", "Lesson", "Unit", "TermScope"))

def digest(codes):
    return hashlib.sha256("\n".join(sorted(codes)).encode()).hexdigest()

def verify(root):
    root = Path(root)
    def baseline(path):
        return json.loads(subprocess.check_output(["git", "show", f"{BASE}:{path}"], cwd=root).decode("utf-8-sig"))
    def current(path):
        return json.loads((root / path).read_text(encoding="utf-8-sig"))
    if any((root / p).exists() for p in CONTENT_PATHS + BLUEPRINT_PATHS + MANIFEST_PATHS):
        raise ValueError("retired UAE Grade11 active resource reintroduced")
    identities = set()
    for track, cp, bp, count in zip(("GENERAL", "ADVANCED"), CONTENT_PATHS, BLUEPRINT_PATHS, (49, 74)):
        content, blueprint = baseline(cp), baseline(bp)
        codes = {x["LessonCode"] for x in content["Lessons"]}
        if not (content["PackCode"] == blueprint["PackCode"] == "UAE-MOE-MATH" and len(codes) == len(content["Lessons"]) == len(blueprint["Lessons"]) == count and codes == {x["LessonCode"] for x in blueprint["Lessons"]} and all(c.startswith(PREFIX + track + ":") for c in codes)):
            raise ValueError("unexpected historical G11 identities")
        identities.update(codes)
    if len(identities) != 123 or digest(identities) != "d058eb29f085df8de09ebb46ba6404696fc44388c0bd96afd6e8f1282b5305e3":
        raise ValueError("exact G11 identity digest mismatch")
    before, after = baseline(CORE_PACK), current(CORE_PACK)
    removed = [n for n in before["Nodes"] if target(n)]
    if not (len(removed) == 129 and sum(n["Kind"] == "Reference" and n["IsOfficial"] for n in removed) == 123 and sum(n["Kind"] == "SourceCatalog" for n in removed) == 6 and digest(n["Code"] for n in removed) == "3931c8676aedc61e28b209bb594bd1b1c0a6b021abd6635c5ed05417c808ba66"):
        raise ValueError("exact G11 source-node scope mismatch")
    expected = dict(before)
    expected["Nodes"] = [n for n in before["Nodes"] if not target(n)]
    expected["NodeCount"] = 1105
    expected["OfficialNodeCount"] = 1057
    expected["ContentDigest"] = hashlib.sha256(("EDULYTICS-UAE-G11-RETIREMENT-V1\n" + before["ContentDigest"] + "\n" + "\n".join(sorted(n["Code"] for n in removed))).encode()).hexdigest()
    if after != expected or any(target(n) for n in after["Nodes"]):
        raise ValueError("G11 retirement modified unrelated source nodes/metadata")
    for directory, suffix in (("LessonContent/Packs", ".lesson-content-pack.json"), ("LessonBlueprints/Packs", ".lesson-blueprint.json")):
        for path in (root / "src/Edulytics.Core/Curriculum" / directory).glob("*" + suffix):
            doc = json.loads(path.read_text(encoding="utf-8-sig"))
            if any(str(x.get("LessonCode", "")).startswith(PREFIX) for x in doc.get("Lessons", [])):
                raise ValueError(f"retired G11 identity in {path}")
    return {"oldContentPaths": CONTENT_PATHS, "oldBlueprintPaths": BLUEPRINT_PATHS, "corePackPath": CORE_PACK, "removedLessons": 123, "removedNodes": 129}

if __name__ == "__main__":
    print("UAE_G11_EXACT_RETIREMENT_PASS", verify(Path(__file__).resolve().parents[2]))
