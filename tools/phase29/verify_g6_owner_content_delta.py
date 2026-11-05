#!/usr/bin/env python3
"""Bounded Grade 6 first-party English authored delta. NOT publisher-source parity or release signoff.

This only reconciles the pinned 147-lesson Edulytics-authored candidate with the
published canonical baseline. It never approves a third-party source element,
teacher judgement, copyright/asset rights, or deployment.
"""
import json
import subprocess
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
RELATIVE = ("src/Edulytics.Core/Curriculum/LessonContent/Packs/"
            "us-ccss-math-g6-phase29-v1.lesson-content-pack.json")
MAIN_BLOB = "30e44c9440c4970b12c8e9e8dc872254ccbb0b7d"
CANDIDATE_BLOB = "d6837ce3a413c49a9135f367b967ad180c49667d"
BEFORE_VERSION = "phase30-common-core-rich-content-v1"
AFTER_VERSION = "us-ccss-g6-full-grade-authoring-candidate-20261028-v1"
TEACHING_FIELDS = frozenset((
    "title", "explanation", "keyConceptsAndRules", "workedExamples",
    "stepByStepSolutions", "commonMistakes", "quickSummary"
))


def git(*args):
    return subprocess.check_output(["git", *args], cwd=ROOT, text=True, encoding="utf-8").strip()


def validate_pair(original, candidate):
    original_top, candidate_top = dict(original), dict(candidate)
    old = original_top.pop("lessons", None)
    new = candidate_top.pop("lessons", None)
    old_version = original_top.pop("contentVersion", None)
    new_version = candidate_top.pop("contentVersion", None)
    if (old_version, new_version) != (BEFORE_VERSION, AFTER_VERSION):
        raise ValueError("G6 unexpected first-party contentVersion transition")
    if (original_top != candidate_top or not isinstance(old, list) or
            not isinstance(new, list) or len(old) != 147 or len(new) != 147):
        raise ValueError("G6 official top metadata/147-lesson count changed")
    changed = set()
    seen = set()
    for old_lesson, new_lesson in zip(old, new):
        code = old_lesson.get("lessonCode")
        if (not isinstance(code, str) or
                not code.startswith("PED:US-CCSS-MATH:G6:") or
                code in seen or code != new_lesson.get("lessonCode")):
            raise ValueError("G6 changed/duplicated lesson code or ordering")
        seen.add(code)
        before, after = dict(old_lesson), dict(new_lesson)
        old_translations = before.pop("translations", None)
        new_translations = after.pop("translations", None)
        if before != after:
            raise ValueError("G6 publisher/source or official lesson metadata changed: "+code)
        if (not isinstance(old_translations, list) or not isinstance(new_translations, list) or
                len(old_translations) != len(new_translations)):
            raise ValueError("G6 translation count changed: "+code)
        originals = {t["cultureCode"]: t for t in old_translations}
        if len(originals) != len(old_translations):
            raise ValueError("G6 duplicate translation culture: "+code)
        if set(originals) != {t["cultureCode"] for t in new_translations}:
            raise ValueError("G6 translation culture changed: "+code)
        for translation in new_translations:
            old_translation = originals[translation["cultureCode"]]
            if translation["cultureCode"] != "en":
                if translation != old_translation:
                    raise ValueError("G6 non-English translation changed: "+code)
                continue
            delta = {key for key in set(old_translation)|set(translation)
                     if old_translation.get(key) != translation.get(key)}
            if not delta <= TEACHING_FIELDS or set(old_translation) != set(translation):
                raise ValueError("G6 non-teaching English field changed: "+code)
            if delta:
                changed.add(("US-CCSS-MATH", code))
    if len(changed) != 147:
        raise ValueError(f"G6 changed English teaching count must be 147, got {len(changed)}")
    return changed


def validate():
    actual_main = git("rev-parse", "origin/main:" + RELATIVE)
    actual_candidate = git("hash-object", RELATIVE)
    if actual_candidate == actual_main:
        return set()
    if actual_main != MAIN_BLOB or actual_candidate != CANDIDATE_BLOB:
        raise ValueError("G6 exact original/first-party authored Git blobs not pinned")
    original = json.loads(git("show", "origin/main:" + RELATIVE))
    candidate = json.loads((ROOT / RELATIVE).read_text(encoding="utf-8"))
    return validate_pair(original, candidate)


if __name__ == "__main__":
    accepted = validate()
    print(f"G6_FIRST_PARTY_TECHNICAL_DELTA 147 lessons; {len(accepted)} "
          "English teaching revisions; publisher source parity/rights/release NOT certified")
