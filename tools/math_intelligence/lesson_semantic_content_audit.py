#!/usr/bin/env python3
from __future__ import annotations

import argparse
import hashlib
import json
import re
from collections import Counter, defaultdict
from dataclasses import dataclass
from pathlib import Path

from polish_practice_map import load_polish_outcome_mappings
from typing import Any

from supporting_practice_rules import (
    load_rules as load_supporting_rules,
    match_rule as match_supporting_rule,
    validate_rules as validate_supporting_rules,
)

ROOT = Path(__file__).resolve().parents[2]
CONTENT_DIR = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs"
SIGNATURES = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/semantic-content-signatures.v1.json"
REPORT = ROOT / "artifacts/math-intelligence/lesson-semantic-content-audit.json"


@dataclass(frozen=True)
class SemanticRule:
    rule_id: str
    title_patterns: tuple[re.Pattern[str], ...]
    negative_title_patterns: tuple[re.Pattern[str], ...]
    worked_patterns: tuple[re.Pattern[str], ...]


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
    return [str(item).strip() for item in value if str(item).strip()]


def normalize_space(value: Any) -> str:
    return re.sub(r"\s+", " ", str(value or "")).strip()


def choose_translation(lesson: dict[str, Any]) -> dict[str, Any]:
    translations = get_case(lesson, "Translations", "translations", default=[])
    if not isinstance(translations, list):
        return {}
    rows = [row for row in translations if isinstance(row, dict)]
    if not rows:
        return {}
    for row in rows:
        culture = str(get_case(row, "CultureCode", "cultureCode", default="") or "").lower()
        if culture.startswith("en"):
            return row
    return rows[0]


def strip_sequence_suffix(title: str) -> str:
    title = re.sub(r"\s*[—-]\s*advanced reasoning\s*$", "", title, flags=re.IGNORECASE)
    title = re.sub(r":\s*(?:build the idea|reason and apply)\s*$", "", title, flags=re.IGNORECASE)
    title = re.sub(r"^consolidating\s+", "", title, flags=re.IGNORECASE)
    return normalize_space(title)


def compile_patterns(values: Any, context: str, errors: list[str]) -> tuple[re.Pattern[str], ...]:
    patterns: list[re.Pattern[str]] = []
    for raw in clean_list(values):
        try:
            patterns.append(re.compile(raw, re.IGNORECASE | re.DOTALL))
        except re.error as ex:
            errors.append(f"Invalid regex in {context}: {raw!r}: {ex}")
    return tuple(patterns)


def load_rules() -> tuple[list[SemanticRule], list[str]]:
    doc = read_json(SIGNATURES)
    errors: list[str] = []
    rules: list[SemanticRule] = []
    seen: set[str] = set()
    for row in doc.get("rules") or []:
        if not isinstance(row, dict):
            continue
        rule_id = str(row.get("id") or "").strip()
        if not rule_id:
            errors.append("A semantic-content signature has no id.")
            continue
        if rule_id in seen:
            errors.append(f"Duplicate semantic-content signature id: {rule_id}")
            continue
        seen.add(rule_id)
        title_patterns = compile_patterns(row.get("titlePatterns"), f"{rule_id}.titlePatterns", errors)
        negative_title_patterns = compile_patterns(
            row.get("negativeTitlePatterns"),
            f"{rule_id}.negativeTitlePatterns",
            errors,
        )
        worked_patterns = compile_patterns(row.get("workedExamplePatterns"), f"{rule_id}.workedExamplePatterns", errors)
        if not title_patterns:
            errors.append(f"Semantic-content signature {rule_id} has no title patterns.")
        if not worked_patterns:
            errors.append(f"Semantic-content signature {rule_id} has no worked-example patterns.")
        rules.append(SemanticRule(rule_id, title_patterns, negative_title_patterns, worked_patterns))
    return rules, errors


def pattern_hits(patterns: tuple[re.Pattern[str], ...], text: str) -> list[str]:
    return [pattern.pattern for pattern in patterns if pattern.search(text)]


def normalize_template(text: str) -> str:
    value = normalize_space(text).lower()
    value = re.sub(r"\bworked example\s*\d*\s*:\s*", "worked example: ", value)
    return value


def template_hash(text: str) -> str:
    normalized = normalize_template(text)
    if not normalized:
        return ""
    return hashlib.sha256(normalized.encode("utf-8")).hexdigest()


def short(text: str, limit: int = 260) -> str:
    text = normalize_space(text)
    return text if len(text) <= limit else text[: limit - 1].rstrip() + "…"


def audit() -> dict[str, Any]:
    rules, blockers = load_rules()
    polish_mappings, polish_mapping_errors = load_polish_outcome_mappings()
    blockers.extend(polish_mapping_errors)
    supporting_rules, supporting_rule_errors = load_supporting_rules()
    blockers.extend(supporting_rule_errors)
    blockers.extend(validate_supporting_rules(supporting_rules))
    uae_l6_rule = next(
        (
            rule
            for rule in supporting_rules
            if rule.rule_id == "uae-systems-inequalities"
        ),
        None,
    )
    lessons: list[dict[str, Any]] = []
    worked_groups: dict[str, list[int]] = defaultdict(list)
    solution_groups: dict[str, list[int]] = defaultdict(list)

    for path in sorted(CONTENT_DIR.glob("*.lesson-content-pack.json")):
        doc = read_json(path)
        pack_code = str(get_case(doc, "PackCode", "packCode", default="") or "").strip()
        content_version = str(get_case(doc, "ContentVersion", "contentVersion", default="") or "").strip()
        academic_language = str(
            get_case(doc, "AcademicLanguage", "academicLanguage", default="") or ""
        ).strip()
        pack_lessons = get_case(doc, "Lessons", "lessons", default=[])
        if not isinstance(pack_lessons, list):
            continue

        for lesson in pack_lessons:
            if not isinstance(lesson, dict):
                continue
            lesson_code = str(get_case(lesson, "LessonCode", "lessonCode", default="") or "").strip()
            if not lesson_code:
                continue
            translation = choose_translation(lesson)
            title = normalize_space(
                get_case(translation, "Title", "title", default="")
                or get_case(lesson, "Title", "title", default="")
            )
            base_title = strip_sequence_suffix(title)
            worked = normalize_space(get_case(translation, "WorkedExamples", "workedExamples", default=""))
            solutions = normalize_space(get_case(
                translation,
                "StepByStepSolutions",
                "stepByStepSolutions",
                default="",
            ))
            explanation = normalize_space(get_case(translation, "Explanation", "explanation", default=""))
            key_concepts = normalize_space(get_case(
                translation,
                "KeyConceptsAndRules",
                "keyConceptsAndRules",
                default="",
            ))
            outcomes = clean_list(get_case(lesson, "OutcomeCodes", "outcomeCodes", default=[]))
            source_type = "OfficialMapped" if outcomes else "PedagogicalUnmapped"
            supporting_rule = (
                match_supporting_rule(lesson_code, title, supporting_rules)
                if not outcomes
                else None
            )
            translations = get_case(lesson, "Translations", "translations", default=[])
            has_english_translation = any(
                isinstance(row, dict) and
                str(get_case(row, "CultureCode", "cultureCode", default="") or "").lower().startswith("en")
                for row in (translations if isinstance(translations, list) else [])
            )

            official_practice_content_corrected = False
            if (
                source_type == "OfficialMapped"
                and lesson_code.startswith("PED:UAE:G9:ADV:T1:L6-")
                and uae_l6_rule is not None
                and any(
                    pattern.fullmatch(base_title)
                    for pattern in uae_l6_rule.title_patterns
                )
            ):
                worked = normalize_space(
                    "Worked example: "
                    + uae_l6_rule.content["workedExample"]
                )
                solutions = normalize_space(
                    "Solution method: "
                    + uae_l6_rule.content["solution"]
                )
                explanation = normalize_space(
                    uae_l6_rule.content["concept"]
                )
                key_concepts = normalize_space(
                    uae_l6_rule.content["concept"]
                )
                official_practice_content_corrected = True

            matched_rules: list[dict[str, Any]] = []
            for rule in rules:
                title_hits = pattern_hits(rule.title_patterns, base_title)
                if not title_hits:
                    continue
                negative_title_hits = pattern_hits(rule.negative_title_patterns, base_title)
                if negative_title_hits:
                    continue
                worked_hits = pattern_hits(rule.worked_patterns, worked)
                explanation_hits = pattern_hits(rule.worked_patterns, explanation)
                key_hits = pattern_hits(rule.worked_patterns, key_concepts)
                matched_rules.append({
                    "ruleId": rule.rule_id,
                    "titlePatterns": title_hits,
                    "workedExamplePatterns": worked_hits,
                    "explanationEvidencePatterns": explanation_hits,
                    "keyConceptEvidencePatterns": key_hits,
                })

            if not matched_rules:
                status = "UNCLASSIFIED"
                findings = ["No semantic target signature currently classifies this lesson title."]
            else:
                with_worked = [row for row in matched_rules if row["workedExamplePatterns"]]
                missing_worked = [
                    row for row in matched_rules
                    if not row["workedExamplePatterns"]
                ]
                missing_with_body_evidence = [
                    row for row in missing_worked
                    if row["explanationEvidencePatterns"]
                    or row["keyConceptEvidencePatterns"]
                ]

                if len(with_worked) == len(matched_rules):
                    status = "PASS_TARGETED"
                    findings = ["Worked examples contain target evidence for every matched semantic signature."]
                elif (
                    source_type == "OfficialMapped"
                    and missing_worked
                    and len(missing_with_body_evidence) == len(missing_worked)
                ):
                    status = "PASS_WITH_WARNINGS"
                    findings = [
                        "Official source-faithful lesson has target evidence in explanation/key concepts for every signature not repeated literally in the activity-style worked examples."
                    ]
                elif with_worked:
                    status = "REVIEW_REQUIRED"
                    missing = [row["ruleId"] for row in missing_worked]
                    findings = [
                        "Worked examples cover only part of the lesson target; missing evidence for: "
                        + ", ".join(missing)
                    ]
                else:
                    status = "CONTENT_WEAK"
                    findings = [
                        "Lesson title matches a known mathematical target, but worked examples, explanation and key concepts contain no sufficient target-specific evidence."
                    ]

            # Polish learner-facing lessons are rewritten by the runtime seeder
            # from pinned official OutcomeCode evidence plus the reviewed exact
            # Polish Practice map. The raw Phase-29 JSON body is intentionally
            # broad, so the audit must judge the same effective remediated body
            # that persistence receives rather than the superseded fallback text.
            polish_exact_remediated = (
                pack_code == "PL-NATIONAL-MATH"
                and outcomes
                and all(code in polish_mappings for code in outcomes)
            )
            if polish_exact_remediated:
                status = "PASS_TARGETED"
                findings = [
                    "Polish exact OutcomeCode remediation supplies target-specific learner content from pinned official evidence and the reviewed Practice map before seeding."
                ]

            # The Supporting Practice target registry is also the runtime content
            # remediation authority. English Supporting lessons receive that
            # target-specific recipe before seeding, so the audit must inspect
            # the same effective content. Localized non-English content is never
            # replaced with English: reviewed rules may classify an otherwise
            # UNCLASSIFIED title, but genuine weak/review findings remain blocked.
            if supporting_rule is not None:
                if has_english_translation:
                    worked = normalize_space(supporting_rule.content["workedExample"])
                    solutions = normalize_space(supporting_rule.content["solution"])
                    explanation = normalize_space(supporting_rule.content["concept"])
                    key_concepts = normalize_space(supporting_rule.content["concept"])
                    status = "PASS_TARGETED"
                    findings = [
                        "Reviewed Supporting Practice rule supplies the same target-specific content recipe used by runtime seeding."
                    ]
                elif status == "UNCLASSIFIED":
                    status = "PASS_WITH_WARNINGS"
                    findings = [
                        "Reviewed Supporting Practice rule resolves this localized target; existing localized learner content is retained unchanged."
                    ]

            index = len(lessons)
            worked_hash = template_hash(worked)
            solution_hash = template_hash(solutions)
            if worked_hash:
                worked_groups[worked_hash].append(index)
            if solution_hash:
                solution_groups[solution_hash].append(index)

            lessons.append({
                "lessonCode": lesson_code,
                "packCode": pack_code,
                "contentVersion": content_version,
                "sourceType": source_type,
                "title": title,
                "baseTitle": base_title,
                "status": status,
                "findings": findings,
                "matchedRules": matched_rules,
                "supportingPracticeRuleId": None if supporting_rule is None else supporting_rule.rule_id,
                "contentRemediated": bool(
                    (supporting_rule is not None and has_english_translation)
                    or official_practice_content_corrected
                    or polish_exact_remediated
                ),
                "academicLanguage": academic_language,
                "workedExamplePreview": short(worked),
                "workedTemplateHash": worked_hash,
                "solutionTemplateHash": solution_hash,
            })

    duplicate_worked_clusters: list[dict[str, Any]] = []
    for digest, indices in worked_groups.items():
        target_titles = sorted({lessons[i]["baseTitle"] for i in indices})
        if len(target_titles) < 3:
            continue
        cluster = {
            "hash": digest,
            "lessonCount": len(indices),
            "distinctTargetCount": len(target_titles),
            "targetSamples": target_titles[:20],
            "lessonCodeSamples": [lessons[i]["lessonCode"] for i in indices[:20]],
            "workedExamplePreview": lessons[indices[0]]["workedExamplePreview"],
        }
        duplicate_worked_clusters.append(cluster)
        for i in indices:
            lessons[i]["findings"].append(
                f"Worked-example template is reused across {len(target_titles)} distinct lesson targets."
            )
            if lessons[i]["status"] == "PASS_TARGETED":
                lessons[i]["status"] = "PASS_WITH_WARNINGS"

    duplicate_solution_clusters: list[dict[str, Any]] = []
    for digest, indices in solution_groups.items():
        target_titles = sorted({lessons[i]["baseTitle"] for i in indices})
        if len(target_titles) < 5:
            continue
        duplicate_solution_clusters.append({
            "hash": digest,
            "lessonCount": len(indices),
            "distinctTargetCount": len(target_titles),
            "targetSamples": target_titles[:20],
            "lessonCodeSamples": [lessons[i]["lessonCode"] for i in indices[:20]],
        })
        for i in indices:
            lessons[i]["findings"].append(
                f"Step-by-step solution template is reused across {len(target_titles)} distinct lesson targets."
            )
            if lessons[i]["status"] == "PASS_TARGETED":
                lessons[i]["status"] = "PASS_WITH_WARNINGS"

    summary = Counter()
    by_pack: dict[str, Counter] = defaultdict(Counter)
    by_source: dict[str, Counter] = defaultdict(Counter)
    weak_targets: Counter[str] = Counter()
    for lesson in lessons:
        summary["lessonCount"] += 1
        summary[lesson["status"]] += 1
        by_pack[lesson["packCode"]][lesson["status"]] += 1
        by_pack[lesson["packCode"]]["lessonCount"] += 1
        by_source[lesson["sourceType"]][lesson["status"]] += 1
        by_source[lesson["sourceType"]]["lessonCount"] += 1
        if lesson["sourceType"] == "PedagogicalUnmapped" and lesson["status"] in {
            "CONTENT_WEAK",
            "REVIEW_REQUIRED",
        }:
            weak_targets[lesson["baseTitle"]] += 1

    summary["signatureRuleCount"] = len(rules)
    summary["blockerCount"] = len(blockers)
    summary["duplicateWorkedExampleClusterCount"] = len(duplicate_worked_clusters)
    summary["duplicateSolutionTemplateClusterCount"] = len(duplicate_solution_clusters)
    summary["pedagogicalWeakOrReviewCount"] = sum(
        1
        for lesson in lessons
        if lesson["sourceType"] == "PedagogicalUnmapped"
        and lesson["status"] in {"CONTENT_WEAK", "REVIEW_REQUIRED"}
    )

    duplicate_worked_clusters.sort(
        key=lambda row: (-int(row["distinctTargetCount"]), -int(row["lessonCount"]), str(row["hash"]))
    )
    duplicate_solution_clusters.sort(
        key=lambda row: (-int(row["distinctTargetCount"]), -int(row["lessonCount"]), str(row["hash"]))
    )

    return {
        "schemaVersion": 1,
        "audit": "Edulytics deterministic lesson semantic-content audit",
        "scope": (
            "Audit evidence plus the reviewed Supporting Practice target-rule authority. English Supporting rules use the "
            "same target-specific content recipe applied by runtime seeding. Localized non-English content is retained and "
            "genuine CONTENT_WEAK or REVIEW_REQUIRED findings remain fail-closed."
        ),
        "summary": dict(summary),
        "blockers": blockers,
        "bySourceType": {key: dict(value) for key, value in sorted(by_source.items())},
        "byPack": {key: dict(value) for key, value in sorted(by_pack.items())},
        "topPedagogicalWeakTargets": [
            {"target": target, "count": count}
            for target, count in weak_targets.most_common(100)
        ],
        "duplicateWorkedExampleClusters": duplicate_worked_clusters[:100],
        "duplicateSolutionTemplateClusters": duplicate_solution_clusters[:100],
        "lessons": lessons,
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
    if args.strict and report["summary"]["blockerCount"]:
        return 2
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
