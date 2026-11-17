#!/usr/bin/env python3
"""Fail-closed, exact-Git-blob owner-approved G8 General 77 English -> 76 Arabic.

A classification of FIRST-PARTY content change, NEVER textbook item parity,
publisher copyright clearance or evidence of actual production publication.
"""
import json
import subprocess
from pathlib import Path

PACK = "UAE-MOE-MATH"
PREFIX = "PED:UAE-MOE-MATH:L8:GENERAL:"
EXTRA = PREFIX + "12:05:INFINITE-SETS"
CONTENT = "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g8-general-t1-ogl-v1.lesson-content-pack.json"
BLUEPRINT = "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g8-general-t1-ogl-v1.lesson-blueprint.json"
APPROVAL = "artifacts/curriculum-delivery/UAE-MOE-MATH/G8/grade8-owner-academic-approval.json"
LOCKS = {
    CONTENT: ("c9990841a08e66a6b002d931546bc93214a59bd1",
              "2f0125ab4bff1ee6ae1afb620c8fb6d9f4e3f296"),
    BLUEPRINT: ("547d51c74520bdcb4b11ee5e1b96695280e73056",
                "c8125adc1012bb0cc694cec2520218beb002b1a2"),
}

def git(root, *args):
    return subprocess.check_output(["git", *args], cwd=root,
                                   text=True, encoding="utf-8").strip()

def load(root, path):
    return json.loads((root / path).read_text(encoding="utf-8-sig"))

def verify(root: Path):
    root = Path(root)
    owner = load(root, APPROVAL)
    if not (owner.get("ownerAcademicApproval") is True
            and owner.get("ownerPermissionToProceedToReleaseGates") is True
            and owner.get("ownerFirstPartyAuthorshipRepresentation") is True
            and owner.get("grade") == 8
            and owner.get("pathway") == "General"
            and owner.get("ownerSelectedIndex") == "12 units, 76 lessons, Arabic RTL"
            and owner.get("sourceEditionIdentityClaim") == "NONE"
            and owner.get("originalPublisherElementParity") == "NOT_INDEPENDENTLY_VERIFIED"
            and owner.get("publisherArtworkOrVerbatimTextReuseLicensed") is False):
        raise ValueError("Owner approval or source/rights limitations missing")
    before, after = {}, {}
    for path, (original_sha, approved_sha) in LOCKS.items():
        baseline = git(root, "rev-parse", f"origin/main:{path}")
        candidate = git(root, "hash-object", str(root / path))
        if candidate != approved_sha or baseline not in (original_sha, approved_sha):
            raise ValueError(f"Unexpected G8 source or blueprint Git blob: {path}")
        if (baseline == original_sha) != (git(root, "rev-parse", f"origin/main:{CONTENT}") == LOCKS[CONTENT][0]):
            raise ValueError("Main Grade8 baseline mixes old/new content and blueprint")
        before[path] = json.loads(git(root, "show", f"origin/main:{path}"))
        after[path] = load(root, path)
    curr = after[CONTENT]
    blueprint = after[BLUEPRINT]
    new_codes = [row["LessonCode"] for row in curr["Lessons"]]
    blueprint_codes = [row["LessonCode"] for row in blueprint["Lessons"]]
    if not (len(new_codes) == len(set(new_codes)) == 76
            and new_codes == blueprint_codes
            and all(x.startswith(PREFIX) for x in new_codes)
            and EXTRA not in new_codes
            and len(blueprint["Units"]) == 12
            and curr.get("PackCode") == blueprint.get("PackCode") == PACK
            and curr.get("VersionCode") == blueprint.get("VersionCode") == "MOE-2026-2027-T1"
            and curr.get("AcademicLanguage") == "ar"
            and curr.get("Status") == "Published"
            and blueprint.get("LogicalLevel") == 8
            and blueprint.get("Pathway") == "General"
            and all(len(row.get("Translations", [])) == 1
                    and row["Translations"][0].get("CultureCode") == "ar"
                    for row in curr["Lessons"])):
        raise ValueError("Arabic 76 candidate, instructional culture, counts or statuses drift")
    baseline = before[CONTENT]
    old_codes = [row["LessonCode"] for row in baseline["Lessons"]]
    old_blueprint_codes = [row["LessonCode"] for row in before[BLUEPRINT]["Lessons"]]
    if old_codes != old_blueprint_codes:
        raise ValueError("Original main G8 content/blueprint not aligned")
    if len(old_codes) == 77:
        if not (len(set(old_codes)) == 77
                and baseline.get("AcademicLanguage") == "en"
                and old_codes[:-1] == new_codes and old_codes[-1] == EXTRA):
            raise ValueError("Unexpected old English Grade8 77 index or retired identity")
    elif old_codes != new_codes or baseline.get("AcademicLanguage") != "ar":
        raise ValueError("Already merged G8 main base is not locked Arabic 76")
    current_paths = [
        sorted(x.name for x in (root / folder).glob("uae-g8-general-*.json"))
        for folder in (
            "src/Edulytics.Core/Curriculum/LessonContent/Packs",
            "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs")
    ]
    if current_paths[0] != [Path(CONTENT).name] or current_paths[1] != [Path(BLUEPRINT).name]:
        raise ValueError("Extra or missing G8 General pack/blueprint in candidate")
    return {
        "changedKeys": {(PACK, code) for code in new_codes}
                       if len(old_codes) == 77 else set(),
        "retiredLegacyKeys": {(PACK, EXTRA)} if len(old_codes) == 77 else set(),
        "blueprintPath": BLUEPRINT,
        "existingLessonCount": len(old_codes),
        "newLessonCount": len(new_codes),
    }
