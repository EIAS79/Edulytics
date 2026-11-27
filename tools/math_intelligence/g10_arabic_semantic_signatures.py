"""Scoped Arabic target-language checks for the exact UAE G10 candidate."""
import json
import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
MANIFEST = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/uae-g10-arabic-semantic-signatures.v1.json"
BLUEPRINT = ROOT / "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json"
PACK = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json"
PREFIX = "PED:UAE-MOE-MATH:L10:GENERAL:"


def read(path):
    return json.loads(path.read_text(encoding="utf-8-sig"))


def load_g10_rules():
    """Reject missing/duplicated identities or mismatched titles; every rule has multiple target groups."""
    rows = read(MANIFEST)["rules"]
    blueprint = read(BLUEPRINT)
    expected = sorted(blueprint["Lessons"], key=lambda x: x["SortOrder"])
    if len(rows) != len(expected) or len(rows) != 148 or len(blueprint["Units"]) != 18:
        raise ValueError("G10 semantic target scope must be precisely 148/18")
    result = {}
    for ordinal, (row, lesson) in enumerate(zip(rows, expected), start=1):
        code = row["lessonCode"]
        groups = row["workedPatterns"]
        if (row["ordinal"] != ordinal or lesson["SortOrder"] != ordinal
                or code != lesson["LessonCode"] or not code.startswith(PREFIX)
                or row["title"] != lesson["Title"] or code in result
                or len(groups) < 2 or len(set(groups)) != len(groups)
                or any(not isinstance(p, str) or not p.strip() for p in groups)):
            raise ValueError(f"G10 semantic signature scope mismatch at ordinal {ordinal}")
        compiled = [re.compile(p, re.IGNORECASE | re.DOTALL) for p in groups]
        if any(p.search("") for p in compiled):
            raise ValueError(f"G10 semantic pattern accepts empty text: {code}")
        result[code] = (row["title"], compiled)
    return result


def arabic_translation(lesson):
    translations = lesson.get("Translations", lesson.get("translations", []))
    ar = [t for t in translations
          if str(t.get("CultureCode", t.get("cultureCode", ""))).lower() == "ar"]
    return ar[0] if len(ar) == 1 else {}


def match_g10_rule(rule, title, worked):
    expected_title, groups = rule
    missing = [p.pattern for p in groups if not p.search(worked)]
    return title == expected_title and not missing, missing
