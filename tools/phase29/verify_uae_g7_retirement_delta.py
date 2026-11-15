#!/usr/bin/env python3
"""Strict Grade7 English-corpus retirement exception; no source parity or release claim.

Only the 62 retired English General lesson IDs, one content pack, one
blueprint and exactly 65 corresponding core-pack reference/catalogue nodes
may disappear. All unrelated curriculum pack nodes remain bitwise identical.
"""
import json
import subprocess
from pathlib import Path

OLD_CONTENT = "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g7-general-t1-ogl-v1.lesson-content-pack.json"
OLD_BLUEPRINT = "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g7-general-t1-ogl-v1.lesson-blueprint.json"
NEW_CONTENT = "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g7-general-ar73-student-local-v1.lesson-content-pack.json"
NEW_BLUEPRINT = "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g7-general-ar73-student-local-v1.lesson-blueprint.json"
CORE_PACK = "src/Edulytics.Core/Curriculum/Packs/uae-moe-math.curriculum-pack.json"

def verify(root: Path):
    root = Path(root)
    def current(p): return json.loads((root/p).read_text(encoding="utf-8"))
    def baseline(p): return json.loads(subprocess.check_output(
        ["git", "show", "origin/main:"+p], cwd=root).decode("utf-8-sig"))
    def exists_main(p):
        return subprocess.run(["git", "cat-file", "-e", "origin/main:"+p],
                              cwd=root, stdout=subprocess.DEVNULL,
                              stderr=subprocess.DEVNULL).returncode == 0
    approval = current("artifacts/curriculum-delivery/UAE-MOE-MATH/G7/grade7-owner-academic-approval.json")
    if not (approval.get("grade") == 7 and approval.get("pathway") == "General"
            and approval.get("ownerAcademicApproval") is True
            and approval.get("ownerPermissionToProceedToReleaseGates") is True
            and approval.get("originalAnnualArabicIndexLessons") == 73
            and approval.get("supplementalArabicEditionCandidateLessons") == 5
            and approval.get("indexedLessons") == 78
            and approval.get("publisherBookReferenceOnly") is True
            and approval.get("publisherArtworkOrVerbatimTextReuseLicensed") is False
            and approval.get("originalPublisherElementParity") == "NOT_INDEPENDENTLY_VERIFIED"):
        raise ValueError("Grade7 scoped owner decision absent or misrepresented")

    # Once grade 7 is actually merged, legacy files no longer live in main.
    # This helper explicitly handles both before-release and after-release baselines.
    old_in_main = exists_main(OLD_CONTENT) or exists_main(OLD_BLUEPRINT)
    if exists_main(OLD_CONTENT) != exists_main(OLD_BLUEPRINT):
        raise ValueError("asymmetric historical Grade7 content/blueprint presence")
    if (root/OLD_CONTENT).exists() or (root/OLD_BLUEPRINT).exists():
        raise ValueError("retired English Grade7 resource reintroduced")

    content = current(NEW_CONTENT)
    blueprint = current(NEW_BLUEPRINT)
    new_codes = [x["LessonCode"] for x in content["Lessons"]]
    b_codes = [x["LessonCode"] for x in blueprint["Lessons"]]
    if not (len(new_codes) == len(set(new_codes)) == len(b_codes) == 78
            and new_codes == b_codes
            and all(x.startswith("PED:UAE-MOE-MATH:L7:GENERAL:") for x in new_codes)
            and all(len(x.get("Translations", [])) >= 1 and any(
                t.get("CultureCode") == "ar" for t in x["Translations"])
                for x in content["Lessons"])
            and content.get("AcademicLanguage") == "ar"
            and content.get("Status") == "Published"
            and approval.get("contentPublicationMetadataApproved") is True
            and approval.get("approvedPublicationContentStatus") == "Published"
            and blueprint.get("Pathway") == "General"
            and blueprint.get("LogicalLevel") == 7
            and len(blueprint["Units"]) == 11):
        raise ValueError("Grade7 owner-approved Arabic 78 scope and content identities invalid")

    old_keys = set()
    if old_in_main:
        old_content = baseline(OLD_CONTENT)
        old_blueprint = baseline(OLD_BLUEPRINT)
        old_codes = [x["LessonCode"] for x in old_content["Lessons"]]
        if not (len(old_codes) == len(set(old_codes)) == 62
                and set(old_codes) == {x["LessonCode"] for x in old_blueprint["Lessons"]}
                and all(x.startswith("PED:UAE-MOE-MATH:L7:GENERAL:") for x in old_codes)
                and set(old_codes).isdisjoint(new_codes)
                and old_content.get("AcademicLanguage") == "en"):
            raise ValueError("unreviewed legacy English General ID or source scope")
        old_keys = {("UAE-MOE-MATH", c) for c in old_codes}

    all_current_content = sorted(x.name for x in
        (root/"src/Edulytics.Core/Curriculum/LessonContent/Packs").glob(
            "uae-g7-general-*.lesson-content-pack.json"))
    all_current_blueprints = sorted(x.name for x in
        (root/"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs").glob(
            "uae-g7-general-*.lesson-blueprint.json"))
    if all_current_content != [Path(NEW_CONTENT).name] or all_current_blueprints != [Path(NEW_BLUEPRINT).name]:
        raise ValueError("extra G7 General content/blueprint remains")

    before = baseline(CORE_PACK)
    after = current(CORE_PACK)
    def obsolete(x):
        return x["Code"].startswith("UAE:REF:TEXTBOOK:G7:GENERAL:") or x["Code"].startswith("UAE:CATALOG:G7:GENERAL:")
    old_nodes = before["Nodes"]
    obsolete_nodes = [x for x in old_nodes if obsolete(x)]
    if old_in_main:
        if not (len(obsolete_nodes) == 65
                and sum(x["Kind"] == "Reference" and x.get("IsOfficial") for x in obsolete_nodes) == 62
                and sum(x["Kind"] == "SourceCatalog" for x in obsolete_nodes) == 3):
            raise ValueError("expected only retired 62+3 English G7 core nodes")
    else:
        if obsolete_nodes:
            raise ValueError("new main restored old English Grade7 nodes")
    if after["Nodes"] != [x for x in old_nodes if not obsolete(x)]:
        raise ValueError("unrelated official nodes, positions or source definitions modified")
    for key in before:
        if key in ("Nodes", "NodeCount", "OfficialNodeCount", "ContentDigest"):
            continue
        if before[key] != after.get(key):
            raise ValueError("unrelated UAE pack metadata altered: "+key)
    if set(after) != set(before):
        raise ValueError("core-pack metadata keys altered")
    actual_official = sum(bool(x.get("IsOfficial")) and x.get("Kind") in
                          ("Standard", "Outcome", "Reference") for x in after["Nodes"])
    if not (len(after["Nodes"]) == after["NodeCount"] == 1467
            and after["OfficialNodeCount"] == actual_official == 1358
            and len(after["Links"]) == after["LinkCount"] == 48):
        raise ValueError("Grade7 English reference retirement pack counts drift")
    return {
        "oldContentPath": OLD_CONTENT if old_in_main else None,
        "oldBlueprintPath": OLD_BLUEPRINT if old_in_main else None,
        "oldIdentityKeys": old_keys,
        "corePackPath": CORE_PACK,
        "verifiedRemovedNodes": 65 if old_in_main else 0,
        "newCandidateCount": len(new_codes),
    }
