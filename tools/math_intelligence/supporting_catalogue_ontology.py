#!/usr/bin/env python3
from __future__ import annotations

import argparse
import hashlib
import json
import re
from collections import Counter, defaultdict
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
CONTENT_DIR = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs"
MAPPINGS = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json"
RULES = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/supporting-target-domain-rules.v1.json"
REPORT = ROOT / "artifacts/math-intelligence/supporting-catalogue-ontology.json"


def read_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def get_case(row: dict[str, Any], *names: str, default: Any = None) -> Any:
    for name in names:
        if name in row:
            return row[name]
    return default


def clean_list(value: Any) -> list[str]:
    if not isinstance(value, list):
        return []
    return [str(x).strip() for x in value if str(x).strip()]


def choose_translation(lesson: dict[str, Any]) -> dict[str, Any]:
    rows = get_case(lesson, "Translations", "translations", default=[])
    if not isinstance(rows, list):
        return {}
    rows = [x for x in rows if isinstance(x, dict)]
    for row in rows:
        culture = str(get_case(row, "CultureCode", "cultureCode", default="") or "").lower()
        if culture.startswith("en"):
            return row
    return rows[0] if rows else {}


def canonical_title(lesson: dict[str, Any]) -> str:
    tr = choose_translation(lesson)
    raw = (
        get_case(tr, "Title", "title", default="")
        or get_case(lesson, "Title", "title", default="")
        or ""
    )
    return re.sub(r"\s+", " ", str(raw)).strip()


def normalize_target(title: str, rules_doc: dict[str, Any]) -> str:
    value = re.sub(r"\s+", " ", title).strip().lower()
    for pattern in rules_doc.get("stripPatterns") or []:
        value = re.sub(str(pattern), "", value, flags=re.IGNORECASE).strip()
    value = re.sub(r"\s+", " ", value).strip(" .:-—")
    return value


def infer_domain(target: str, rules_doc: dict[str, Any]) -> str:
    for row in rules_doc.get("domainRules") or []:
        if not isinstance(row, dict):
            continue
        domain = str(row.get("domain") or "").strip()
        for raw in row.get("patterns") or []:
            if re.search(str(raw), target, flags=re.IGNORECASE):
                return domain
    return str(rules_doc.get("fallbackDomain") or "general_mathematics")


def slugify(target: str, limit: int = 72) -> str:
    slug = re.sub(r"[^a-z0-9]+", "_", target.lower()).strip("_")
    slug = re.sub(r"_+", "_", slug)
    if not slug:
        slug = "target"
    return slug[:limit].rstrip("_")


def target_id(domain: str, target: str) -> str:
    digest = hashlib.sha256(target.encode("utf-8")).hexdigest()[:10]
    return f"supporting.{domain}.{slugify(target)}.{digest}"


def load_explicit_mappings() -> dict[str, dict[str, Any]]:
    doc = read_json(MAPPINGS)
    result: dict[str, dict[str, Any]] = {}
    for row in doc.get("mappings") or []:
        if not isinstance(row, dict):
            continue
        code = str(row.get("lessonCode") or "").strip()
        if code:
            result[code] = row
    return result


def resolve_catalogue_target(
    lesson_code: str,
    title: str,
    rules_doc: dict[str, Any],
) -> dict[str, Any]:
    normalized = normalize_target(title, rules_doc)
    if not normalized:
        return {
            "lessonCode": lesson_code,
            "status": "UNRESOLVED_EMPTY_TARGET",
            "targetId": None,
            "domain": None,
            "normalizedTarget": "",
            "title": title,
        }

    domain = infer_domain(normalized, rules_doc)
    return {
        "lessonCode": lesson_code,
        "status": "CATALOGUE_TARGET_CLASSIFIED",
        "targetId": target_id(domain, normalized),
        "domain": domain,
        "normalizedTarget": normalized,
        "title": title,
    }


def audit() -> dict[str, Any]:
    rules_doc = read_json(RULES)
    mappings = load_explicit_mappings()

    rows: list[dict[str, Any]] = []
    clusters: dict[str, list[str]] = defaultdict(list)
    target_meta: dict[str, dict[str, Any]] = {}
    blockers: list[str] = []
    summary = Counter()

    for path in sorted(CONTENT_DIR.glob("*.lesson-content-pack.json")):
        doc = read_json(path)
        pack_code = str(get_case(doc, "PackCode", "packCode", default="") or "").strip()
        lessons = get_case(doc, "Lessons", "lessons", default=[])
        if not isinstance(lessons, list):
            continue

        for lesson in lessons:
            if not isinstance(lesson, dict):
                continue

            code = str(get_case(lesson, "LessonCode", "lessonCode", default="") or "").strip()
            if not code:
                continue

            outcomes = clean_list(get_case(lesson, "OutcomeCodes", "outcomeCodes", default=[]))
            if outcomes:
                continue

            summary["supportingLessonCount"] += 1
            title = canonical_title(lesson)

            explicit = mappings.get(code)
            if explicit is not None:
                row = {
                    "lessonCode": code,
                    "packCode": pack_code,
                    "title": title,
                    "status": "EXPLICIT_SKILL_MAPPING",
                    "targetId": None,
                    "domain": None,
                    "normalizedTarget": normalize_target(title, rules_doc),
                    "primarySkills": clean_list(explicit.get("primarySkills")),
                }
                summary["explicitSkillMappingCount"] += 1
                rows.append(row)
                continue

            row = resolve_catalogue_target(code, title, rules_doc)
            row["packCode"] = pack_code
            rows.append(row)

            if row["status"] != "CATALOGUE_TARGET_CLASSIFIED":
                blockers.append(f"Supporting lesson has no canonical target text: {code}")
                summary["unresolvedCount"] += 1
                continue

            summary["catalogueTargetClassifiedCount"] += 1
            tid = str(row["targetId"])
            clusters[tid].append(code)
            target_meta.setdefault(tid, {
                "targetId": tid,
                "domain": row["domain"],
                "normalizedTarget": row["normalizedTarget"],
            })

    collision_check: dict[tuple[str, str], str] = {}
    for target in target_meta.values():
        key = (str(target["domain"]), str(target["normalizedTarget"]))
        existing = collision_check.get(key)
        if existing is not None and existing != target["targetId"]:
            blockers.append(
                f"Target identity collision for {key}: {existing} vs {target['targetId']}"
            )
        collision_check[key] = str(target["targetId"])

    cluster_rows = []
    for tid, codes in sorted(clusters.items()):
        meta = target_meta[tid]
        cluster_rows.append({
            **meta,
            "lessonCount": len(codes),
            "lessonCodes": sorted(codes),
        })

    summary["canonicalTargetCount"] = len(cluster_rows)
    summary["reusedTargetCount"] = sum(1 for x in cluster_rows if x["lessonCount"] > 1)
    summary["singleLessonTargetCount"] = sum(1 for x in cluster_rows if x["lessonCount"] == 1)
    summary["blockerCount"] = len(blockers)

    return {
        "schemaVersion": 1,
        "audit": "Supporting catalogue canonical target ontology",
        "authority": (
            "Ontology/classification only. A CATALOGUE_TARGET_CLASSIFIED result is not "
            "learner-facing Practice authorization. Exact question family, solver, verifier, "
            "content sufficiency and alignment gates remain mandatory."
        ),
        "normalizationVersion": rules_doc.get("normalizationVersion"),
        "summary": dict(summary),
        "blockers": blockers,
        "targets": cluster_rows,
        "lessons": rows,
    }


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--write-report", action="store_true")
    parser.add_argument("--strict", action="store_true")
    args = parser.parse_args()

    report = audit()
    if args.write_report:
        REPORT.parent.mkdir(parents=True, exist_ok=True)
        REPORT.write_text(
            json.dumps(report, ensure_ascii=False, indent=2) + "\n",
            encoding="utf-8",
        )

    print(json.dumps(report["summary"], ensure_ascii=False, indent=2))

    if args.strict:
        summary = report["summary"]
        if summary.get("blockerCount", 0):
            for blocker in report["blockers"]:
                print(f"BLOCKER: {blocker}")
            return 2

        classified = int(summary.get("catalogueTargetClassifiedCount", 0))
        targets = int(summary.get("canonicalTargetCount", 0))
        if classified > 0 and targets >= classified:
            print(
                "BLOCKER: Supporting ontology did not demonstrate target reuse; "
                "this would imply one target per lesson."
            )
            return 2

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
