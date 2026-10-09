#!/usr/bin/env python3
"""Offline, non-personal Edulytics curriculum JSON inventory for clean Neon rebuilds.

Run on the checked-out repository. Never contacts Neon or reads user data.
"""
from __future__ import annotations

import argparse
import hashlib
import json
from collections import Counter, defaultdict
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
EXPECTED_PACKS = {
    "CAMBRIDGE-INTL-MATH",
    "PL-NATIONAL-MATH",
    "UAE-MOE-MATH",
    "US-CCSS-MATH",
}


def _get(data: dict[str, Any], name: str) -> Any:
    return data.get(name) if name in data else data.get(name[0].lower() + name[1:])


def _load(path: Path) -> dict[str, Any]:
    data = json.loads(path.read_text(encoding="utf-8"))
    if not isinstance(data, dict):
        raise ValueError("Root must be a JSON object")
    return data


def inventory(root: Path) -> dict[str, Any]:
    curriculum = root / "src/Edulytics.Core/Curriculum"
    content_dir = curriculum / "LessonContent/Packs"
    blueprints_dir = curriculum / "LessonBlueprints/Packs"
    rich_dir = curriculum / "LessonContent/RichV2"
    curriculum_dir = curriculum / "Packs"

    errors: list[str] = []
    warnings: list[str] = []
    by_pack: dict[str, dict[str, Any]] = {}
    identity_sources: dict[tuple[str, str], str] = {}
    all_content_files = sorted(content_dir.glob("*.lesson-content-pack.json"))

    for path in all_content_files:
        rel = str(path.relative_to(root))
        try:
            doc = _load(path)
        except (ValueError, OSError, json.JSONDecodeError) as exc:
            errors.append(f"{rel}: cannot parse JSON: {exc}")
            continue

        pack = str(_get(doc, "PackCode") or "").strip()
        version = str(_get(doc, "VersionCode") or "").strip()
        lessons = _get(doc, "Lessons")
        status = str(_get(doc, "Status") or "").strip()
        if not pack or not version:
            errors.append(f"{rel}: missing PackCode or VersionCode")
        if pack not in EXPECTED_PACKS:
            errors.append(f"{rel}: unrecognized PackCode {pack!r}")
        if not isinstance(lessons, list):
            errors.append(f"{rel}: Lessons must be an array")
            continue

        row = by_pack.setdefault(pack, {
            "contentFiles": 0,
            "lessonEntries": 0,
            "uniqueLessonCodes": 0,
            "publishedFiles": 0,
            "languages": [],
        })
        row["contentFiles"] += 1
        row["lessonEntries"] += len(lessons)
        if status.lower() == "published":
            row["publishedFiles"] += 1

        for index, lesson in enumerate(lessons):
            if not isinstance(lesson, dict):
                errors.append(f"{rel}: Lessons[{index}] must be an object")
                continue
            code = str(_get(lesson, "LessonCode") or "").strip()
            if not code:
                errors.append(f"{rel}: Lessons[{index}] has no LessonCode")
                continue
            identity = (pack, code)
            if identity in identity_sources:
                errors.append(
                    f"duplicate lesson identity {pack}/{code}: "
                    f"{identity_sources[identity]} and {rel}"
                )
            else:
                identity_sources[identity] = rel

            translations = _get(lesson, "Translations")
            if not isinstance(translations, list) or not translations:
                warnings.append(f"{rel}: {code}: no translation array")
                continue
            languages = []
            for translation in translations:
                if not isinstance(translation, dict):
                    errors.append(f"{rel}: {code}: translation is not an object")
                    continue
                culture = str(_get(translation, "CultureCode") or "").strip()
                if not culture:
                    errors.append(f"{rel}: {code}: translation culture missing")
                    continue
                if culture in languages:
                    errors.append(f"{rel}: {code}: duplicate culture {culture}")
                languages.append(culture)
            existing = set(row["languages"])
            row["languages"] = sorted(existing.union(languages))

    for pack in EXPECTED_PACKS:
        if pack not in by_pack:
            errors.append(f"No canonical content files for {pack}")
        else:
            by_pack[pack]["uniqueLessonCodes"] = sum(
                1 for item in identity_sources if item[0] == pack
            )

    core_packs = sorted(curriculum_dir.glob("*.curriculum-pack.json"))
    core_codes: set[str] = set()
    for path in core_packs:
        rel = str(path.relative_to(root))
        try:
            doc = _load(path)
            code = str(_get(doc, "PackCode") or "").strip()
            core_codes.add(code)
        except (ValueError, OSError, json.JSONDecodeError) as exc:
            errors.append(f"{rel}: invalid curriculum JSON: {exc}")
    missing_core = EXPECTED_PACKS - core_codes
    if missing_core:
        errors.append(f"Missing core curriculum pack(s): {', '.join(sorted(missing_core))}")

    blueprint_paths = sorted(blueprints_dir.glob("*.lesson-blueprint.json"))
    for path in blueprint_paths:
        try:
            _load(path)
        except (ValueError, OSError, json.JSONDecodeError) as exc:
            errors.append(f"{path.relative_to(root)}: invalid blueprint JSON: {exc}")

    rich_paths = sorted(rich_dir.glob("*.rich-lesson-v2.json"))
    for path in rich_paths:
        try:
            _load(path)
        except (ValueError, OSError, json.JSONDecodeError) as exc:
            errors.append(f"{path.relative_to(root)}: invalid RichV2 JSON: {exc}")

    manifest_paths = all_content_files + blueprint_paths + core_packs + rich_paths
    digest = hashlib.sha256()
    for path in sorted(manifest_paths):
        digest.update(str(path.relative_to(root)).encode("utf-8"))
        digest.update(b"\0")
        digest.update(hashlib.sha256(path.read_bytes()).digest())

    totals = {
        "curriculumPacks": len(core_packs),
        "lessonContentPacks": len(all_content_files),
        "lessonBlueprintPacks": len(blueprint_paths),
        "richV2Documents": len(rich_paths),
        "lessonEntries": sum(row["lessonEntries"] for row in by_pack.values()),
        "uniqueLessonCodes": len(identity_sources),
        "errorCount": len(errors),
        "warningCount": len(warnings),
    }
    return {
        "schemaVersion": 1,
        "purpose": "Offline JSON corpus inventory, not proof of DB import or pedagogical QA",
        "sourceDigestSha256": digest.hexdigest(),
        "totals": totals,
        "byPack": {name: by_pack[name] for name in sorted(by_pack)},
        "errors": errors,
        "warnings": warnings,
    }


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--root", type=Path, default=ROOT)
    parser.add_argument("--write-report", action="store_true")
    parser.add_argument("--report", type=Path, default=None)
    parser.add_argument("--strict", action="store_true")
    args = parser.parse_args()

    report = inventory(args.root.resolve())
    report_path = args.report or (
        args.root.resolve() / "artifacts/phase29/clean-neon-content-inventory.json"
    )
    if args.write_report:
        report_path.parent.mkdir(parents=True, exist_ok=True)
        report_path.write_text(
            json.dumps(report, ensure_ascii=False, indent=2) + "\n",
            encoding="utf-8",
        )
    print(json.dumps(report["totals"], ensure_ascii=False, indent=2))
    if report["errors"]:
        print("Errors:\n" + "\n".join(report["errors"][:30]))
    return 2 if args.strict and report["errors"] else 0


if __name__ == "__main__":
    raise SystemExit(main())
