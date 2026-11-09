#!/usr/bin/env python3
"""Fail-closed, exact-Git-blob UAE Grade 2 first-party owner approval.

The owner authorizes Edulytics-authored Arabic learning bodies based on
selected older local books. This gate never certifies latest-edition exercise
parity, rights to third-party artwork, or student UAT.
"""
import json
import subprocess
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
PACK = "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g2-common-t1-ogl-v1.lesson-content-pack.json"
BLUEPRINT = "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g2-common-t1-ogl-v1.lesson-blueprint.json"
AUTH = "artifacts/curriculum-delivery/UAE-MOE-MATH/G2/grade2-owner-academic-publication-authorization-20261109.json"
LOCKED = {
    PACK: (
        "e54891902e63c4d8ee6ccad699cfa955355d0174",
        "3352f644b004177f55ef56f677be8b6a763d2c14",
    ),
    BLUEPRINT: (
        "a5ecf31a7f3b918f98f3b745a002f3ec2946becd",
        "44f72802ec57e1919c6ea646352ba8353df263ea",
    ),
}
SOURCE_BY_UNIT = {
    **{n: "867b35b6cfa6f2d01b71af47ccda7c8e8a916e964d54face15d4e17ac600335" for n in range(1, 5)},
    **{n: "1b04e49007d899041189d2750f70893b1649bf5933f3afeed2e078e897fb5ecb" for n in range(5, 9)},
    **{n: "0b888c3918421a411118cfe2b5436b155d4695b6582742c87babd8fe94173784" for n in range(9, 13)},
}
FIELDS = ("Title", "Explanation", "KeyConceptsAndRules", "WorkedExamples",
          "StepByStepSolutions", "CommonMistakes", "QuickSummary")


def git(*args):
    return subprocess.check_output(["git", *args], cwd=ROOT, text=True).strip()


def load(path):
    return json.loads((ROOT / path).read_text(encoding="utf-8-sig"))


def validate():
    for path, (old, new) in LOCKED.items():
        actual = git("hash-object", path)
        baseline = git("rev-parse", "origin/main:" + path)
        head = git("rev-parse", "HEAD:" + path)
        if actual != new or head != new or baseline not in (old, new):
            raise ValueError("Grade2 pinned original/owner-authorized Git blob drift: " + path)

    auth = load(AUTH)
    if not (
        auth.get("Grade") == 2 and
        auth.get("Framework") == "UAE-MOE-MATH" and
        auth.get("Pathway") == "Common" and
        auth.get("OwnerDecision") == "ACADEMICALLY_APPROVED_BY_OWNER" and
        auth.get("OwnerAcceptsKnownSourceLimitationsForOriginalContentRelease") is True and
        auth.get("SourceParityIsNotReleaseClaim") is True and
        auth.get("Independent2026_2027ElementParity") == "UNKNOWN" and
        auth.get("IndependentExerciseDenominator") is None and
        auth.get("ThirdPartyAssetClearance") == "NOT_INDEPENDENTLY_CERTIFIED" and
        auth.get("NoFalseCIClaim") is True and
        auth.get("MinistryCertificationClaim") is not True
    ):
        raise ValueError("Grade2 owner scope/rights/source limitations are unverified")

    content, blueprint = load(PACK), load(BLUEPRINT)
    lessons, b_lessons = content.get("Lessons", []), blueprint.get("Lessons", [])
    codes = [x.get("LessonCode") for x in lessons]
    if not (
        content.get("AcademicLanguage") == "ar"
        and content.get("SourceResolution") == "PreviousOfficialFallback"
        and content.get("FallbackReason")
        and len(lessons) == len(b_lessons) == len(set(codes)) == 99
        and all(isinstance(x, str) and x.startswith("PED:UAE-MOE-MATH:L2:COMMON:") for x in codes)
        and codes == [x.get("LessonCode") for x in b_lessons]
        and len(blueprint.get("Units", [])) == 12
    ):
        raise ValueError("Grade2 99 Arabic lesson/blueprint/source-scope identity drift")

    for lesson in lessons:
        number = int(lesson["LessonCode"].split(":")[-2])
        expected = SOURCE_BY_UNIT[number]
        if str(lesson.get("SourceSha256", "")).casefold() != expected:
            raise ValueError("Grade2 local student PDF SHA mismatch: " + lesson["LessonCode"])
        translations = [x for x in lesson.get("Translations", []) if x.get("CultureCode") == "ar"]
        if len(translations) != 1:
            raise ValueError("Grade2 Arabic body missing/duplicate: " + lesson["LessonCode"])
        tr = translations[0]
        for field in FIELDS:
            value = str(tr.get(field, ""))
            if len(value.strip()) < 5:
                raise ValueError("Grade2 original teaching section absent: " + lesson["LessonCode"] + ":" + field)
            # Owner-authorized first-party content is not a blanket rights
            # clearance for external embedded media.
            lowered = value.lower()
            if any(marker in lowered for marker in (
                "<img", "<video", "<iframe", "https://", "http://", ".png",
                ".jpg", ".jpeg", ".mp4", "!["
            )):
                raise ValueError("Grade2 third-party media requires rights review: " + lesson["LessonCode"])

    baseline = json.loads(subprocess.check_output(
        ["git", "show", "origin/main:" + PACK], cwd=ROOT, text=True))
    baseline_lessons = baseline.get("Lessons", [])
    if len(baseline_lessons) != 99:
        raise ValueError("Grade2 original main count changed")
    if [x["LessonCode"] for x in baseline_lessons] != codes:
        raise ValueError("Grade2 original main lesson identity/order changed")
    changes = {
        ("UAE-MOE-MATH", after["LessonCode"])
        for before, after in zip(baseline_lessons, lessons)
        if before.get("Translations") != after.get("Translations")
    }
    old_baseline = git("rev-parse", "origin/main:" + PACK) == LOCKED[PACK][0]
    if old_baseline and len(changes) != 99:
        raise ValueError("Grade2 expected exactly 99 newly owner-authorized Arabic bodies")
    if not old_baseline and changes:
        raise ValueError("Grade2 already merged to main but teaching bodies changed again")

    print("G2_OWNER_PINNED_ARABIC_CONTENT_DELTA",
          f"verified={len(lessons)}", f"changedBodies={len(changes)}",
          "latest2026_27Parity=UNKNOWN", "firstPartyOnly=True")
    return changes


if __name__ == "__main__":
    print("G2_OWNER_PINNED_APPROVED_DELTA_KEYS", len(validate()))
