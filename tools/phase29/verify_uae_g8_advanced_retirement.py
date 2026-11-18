"""Exact, owner-directed retirement guard for UAE Grade 8 Advanced (77 English lessons).

This is NOT a general exclusion from curriculum parity. Only three original
Git-blob-pinned files can disappear, and all 76 Grade 8 General Arabic lesson
identities must remain. The official curriculum source graph is unchanged.
"""
import json
import subprocess
from pathlib import Path

ADV = "PED:UAE-MOE-MATH:L8:ADVANCED:"
GENERAL = "PED:UAE-MOE-MATH:L8:GENERAL:"
OLD = {
    "manifest": (
        "docs/curriculum/UAE_G8_ADVANCED_OFFICIAL_MANIFEST.json",
        "a63db08c347a2b400c5d16eb5a499e04eb2e1de8"),
    "blueprint": (
        "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g8-advanced-t1-ogl-v1.lesson-blueprint.json",
        "984fd08cc54f303624b7aea4ecc7410f6abe1943"),
    "content": (
        "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g8-advanced-t1-ogl-v1.lesson-content-pack.json",
        "fc2d507408d8c7ee0581541b2f1709a7f1eb20f1"),
}
GENERAL_PATH = "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g8-general-t1-ogl-v1.lesson-content-pack.json"
REGISTRY_PATH = "src/Edulytics.Core/Curriculum/MathematicsCurriculumPackRegistry.cs"


def git(root: Path, *args: str) -> bytes:
    return subprocess.check_output(["git", *args], cwd=root)


def verify(root: Path):
    root = Path(root)
    original_present = []
    for path, expected_blob in OLD.values():
        result = subprocess.run(
            ["git", "cat-file", "-e", "origin/main:" + path],
            cwd=root, capture_output=True, check=False)
        original_present.append(result.returncode == 0)

    # Once a successful retirement has been merged, it is part of main;
    # the baseline has no Advanced files and no exclusion is necessary.
    if not any(original_present):
        if any((root / path).exists() for path, _ in OLD.values()):
            raise ValueError("Grade 8 Advanced teaching pack was reintroduced")
        return None
    if not all(original_present):
        raise ValueError("Partial or unexpected original G8 Advanced baseline")

    originals = {}
    for kind, (path, expected_blob) in OLD.items():
        observed = git(root, "rev-parse", "origin/main:" + path).decode().strip()
        if observed != expected_blob:
            raise ValueError(f"Unapproved original G8 Advanced {kind} blob: {observed}")
        if (root / path).exists():
            raise ValueError(f"Owner-retired G8 Advanced {kind} still in candidate")
        originals[kind] = json.loads(git(root, "show", "origin/main:" + path))

    manifest, blueprint, content = (
        originals["manifest"], originals["blueprint"], originals["content"])
    manifest_count = sum(len(unit["Lessons"]) for unit in manifest["Units"])
    bp_codes = [lesson["LessonCode"] for lesson in blueprint["Lessons"]]
    cp_codes = [lesson["LessonCode"] for lesson in content["Lessons"]]
    if not (
        manifest["PackCode"] == "UAE-MOE-MATH"
        and manifest["Grade"] == 8
        and manifest["Pathway"] == "Advanced"
        and manifest["LessonCount"] == manifest_count == 77
        and blueprint["PackCode"] == "UAE-MOE-MATH"
        and blueprint["LogicalLevel"] == 8
        and blueprint["Pathway"] == "Advanced"
        and content["PackCode"] == "UAE-MOE-MATH"
        and content["AcademicLanguage"] == "en"
        and len(bp_codes) == len(cp_codes) == 77
        and len(set(bp_codes)) == 77
        and set(bp_codes) == set(cp_codes)
        and all(code.startswith(ADV) for code in bp_codes)
    ):
        raise ValueError("Source-pinned Advanced 77 identities diverged")

    general = json.loads((root / GENERAL_PATH).read_text(encoding="utf-8"))
    general_codes = [lesson["LessonCode"] for lesson in general["Lessons"]]
    if (general["AcademicLanguage"] != "ar"
        or len(general_codes) != 76
        or len(set(general_codes)) != 76
        or not all(code.startswith(GENERAL) for code in general_codes)):
        raise ValueError("76 General Arabic lessons were changed or dropped")

    registry = (root / REGISTRY_PATH).read_text(encoding="utf-8")
    if ('new(8,"Grade 8","Cycle 2","Advanced",true)' in registry
        or 'new(8,"Grade 8","Cycle 2","General",true)' not in registry):
        raise ValueError("G8 Advanced remains offered or General was removed")

    print("UAE_G8_ADVANCED_OWNER_RETIREMENT verified=77 "
          "GeneralArabic=76 originalBlobs=3 officialGraph=unchanged")
    return {
        "oldContentPath": OLD["content"][0],
        "oldBlueprintPath": OLD["blueprint"][0],
        "retiredLegacyKeys": {("UAE-MOE-MATH", code) for code in cp_codes},
    }
