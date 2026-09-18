#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
import re
from collections import Counter, defaultdict
from dataclasses import dataclass
from pathlib import Path
from typing import Any, Iterable

from supporting_catalogue_ontology import read_json as read_catalogue_json
from supporting_catalogue_ontology import resolve_catalogue_target

ROOT = Path(__file__).resolve().parents[2]
CONTENT_DIR = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs"
SKILL_REGISTRY = ROOT / "src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json"
LESSON_MAPPINGS = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json"
RULES_FILE = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/skill-resolution-rules.v1.json"
REPORT = ROOT / "artifacts/math-intelligence/lesson-skill-resolution-audit.json"
SUPPORTING_TARGET_RULES = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/supporting-target-domain-rules.v1.json"
R3_REVIEW_DECISIONS = ROOT / "src/Edulytics.Core/Mathematics/Curriculum/supporting-review-decisions.r3.v1.json"

STOPWORDS = {
    "a", "an", "and", "apply", "build", "by", "for", "from", "idea", "in",
    "of", "on", "reason", "the", "to", "using", "with", "within", "work",
}

FIELD_WEIGHTS = {
    "title": "titlePattern",
    "keyConcepts": "keyConceptPattern",
    "explanation": "explanationPattern",
    "workedExamples": "workedExamplePattern",
    "solutions": "solutionPattern",
    "summary": "summaryPattern",
}


@dataclass(frozen=True)
class CompiledRule:
    rule_id: str
    skill_id: str
    requires_title: bool
    title_patterns: tuple[re.Pattern[str], ...]
    content_patterns: tuple[re.Pattern[str], ...]
    negative_patterns: tuple[re.Pattern[str], ...]


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


def load_skill_ids() -> set[str]:
    doc = read_json(SKILL_REGISTRY)
    return {
        str(row.get("id") or "").strip()
        for row in doc.get("skills") or []
        if isinstance(row, dict) and str(row.get("id") or "").strip()
    }


def load_existing_mappings() -> dict[str, dict[str, Any]]:
    doc = read_json(LESSON_MAPPINGS)
    result: dict[str, dict[str, Any]] = {}
    for row in doc.get("mappings") or []:
        if not isinstance(row, dict):
            continue
        code = str(row.get("lessonCode") or "").strip()
        if code:
            result[code] = row
    return result

def load_r3_deferred_codes() -> set[str]:
    if not R3_REVIEW_DECISIONS.exists():
        return set()
    doc = read_json(R3_REVIEW_DECISIONS)
    return {
        str(row.get("lessonCode") or "").strip()
        for row in doc.get("decisions") or []
        if isinstance(row, dict)
        and str(row.get("decision") or "") == "DEFER_TO_R5_ONTOLOGY"
        and str(row.get("lessonCode") or "").strip()
    }



def compile_patterns(values: Any, context: str, errors: list[str]) -> tuple[re.Pattern[str], ...]:
    result: list[re.Pattern[str]] = []
    for raw in clean_list(values):
        try:
            result.append(re.compile(raw, re.IGNORECASE | re.DOTALL))
        except re.error as ex:
            errors.append(f"Invalid regex in {context}: {raw!r}: {ex}")
    return tuple(result)


def load_rules(skill_ids: set[str]) -> tuple[list[CompiledRule], dict[str, int], list[str]]:
    doc = read_json(RULES_FILE)
    errors: list[str] = []
    scoring = doc.get("scoring") or {}
    required_scores = {
        "titlePattern": 8,
        "keyConceptPattern": 4,
        "explanationPattern": 3,
        "workedExamplePattern": 3,
        "solutionPattern": 2,
        "summaryPattern": 2,
        "negativePattern": -8,
        "highConfidenceMinimum": 10,
        "reviewMinimum": 6,
        "minimumWinnerGap": 3,
    }
    scores: dict[str, int] = {}
    for key, fallback in required_scores.items():
        try:
            scores[key] = int(scoring.get(key, fallback))
        except (TypeError, ValueError):
            errors.append(f"Invalid scoring value for {key}.")
            scores[key] = fallback

    seen_rule_ids: set[str] = set()
    compiled: list[CompiledRule] = []
    for row in doc.get("rules") or []:
        if not isinstance(row, dict):
            continue
        rule_id = str(row.get("id") or "").strip()
        skill_id = str(row.get("skillId") or "").strip()
        if not rule_id:
            errors.append("A resolution rule has no id.")
            continue
        if rule_id in seen_rule_ids:
            errors.append(f"Duplicate resolution rule id: {rule_id}")
            continue
        seen_rule_ids.add(rule_id)
        if skill_id not in skill_ids:
            errors.append(f"Resolution rule {rule_id} references unknown SkillId {skill_id!r}.")

        compiled.append(
            CompiledRule(
                rule_id=rule_id,
                skill_id=skill_id,
                requires_title=bool(row.get("requiresTitleMatchForHighConfidence", True)),
                title_patterns=compile_patterns(row.get("titlePatterns"), f"{rule_id}.titlePatterns", errors),
                content_patterns=compile_patterns(row.get("contentPatterns"), f"{rule_id}.contentPatterns", errors),
                negative_patterns=compile_patterns(row.get("negativePatterns"), f"{rule_id}.negativePatterns", errors),
            )
        )
    return compiled, scores, errors


def any_match(patterns: Iterable[re.Pattern[str]], text: str) -> list[str]:
    hits: list[str] = []
    for pattern in patterns:
        if pattern.search(text):
            hits.append(pattern.pattern)
    return hits


def evidence_text(lesson: dict[str, Any]) -> dict[str, str]:
    translation = choose_translation(lesson)
    lesson_title = normalize_space(
        get_case(translation, "Title", "title", default="")
        or get_case(lesson, "Title", "title", default="")
    )
    return {
        "title": lesson_title,
        "keyConcepts": normalize_space(get_case(
            translation,
            "KeyConceptsAndRules",
            "keyConceptsAndRules",
            default="",
        )),
        "explanation": normalize_space(get_case(translation, "Explanation", "explanation", default="")),
        "workedExamples": normalize_space(get_case(translation, "WorkedExamples", "workedExamples", default="")),
        "solutions": normalize_space(get_case(
            translation,
            "StepByStepSolutions",
            "stepByStepSolutions",
            default="",
        )),
        "summary": normalize_space(get_case(translation, "QuickSummary", "quickSummary", default="")),
    }


def short_signal(text: str, limit: int = 220) -> str:
    text = normalize_space(text)
    return text if len(text) <= limit else text[: limit - 1].rstrip() + "…"


def score_rule(
    rule: CompiledRule,
    fields: dict[str, str],
    scores: dict[str, int],
) -> dict[str, Any] | None:
    evidence: list[dict[str, Any]] = []
    score = 0

    title_hits = any_match(rule.title_patterns, fields["title"])
    if title_hits:
        score += scores["titlePattern"]
        evidence.append({
            "type": "LessonTitle",
            "weight": scores["titlePattern"],
            "ruleId": rule.rule_id,
            "patterns": title_hits,
            "signal": short_signal(fields["title"]),
        })

    for field_name in ("keyConcepts", "explanation", "workedExamples", "solutions", "summary"):
        hits = any_match(rule.content_patterns, fields[field_name])
        if not hits:
            continue
        weight_key = FIELD_WEIGHTS[field_name]
        weight = scores[weight_key]
        score += weight
        evidence.append({
            "type": field_name,
            "weight": weight,
            "ruleId": rule.rule_id,
            "patterns": hits,
            "signal": short_signal(fields[field_name]),
        })

    combined = "\n".join(fields.values())
    negative_hits = any_match(rule.negative_patterns, combined)
    if negative_hits:
        score += scores["negativePattern"]
        evidence.append({
            "type": "NegativeEvidence",
            "weight": scores["negativePattern"],
            "ruleId": rule.rule_id,
            "patterns": negative_hits,
            "signal": "Negative discriminator matched.",
        })

    if score <= 0 and not title_hits:
        return None

    return {
        "skillId": rule.skill_id,
        "ruleId": rule.rule_id,
        "score": score,
        "titleMatched": bool(title_hits),
        "requiresTitleMatchForHighConfidence": rule.requires_title,
        "negativeHitCount": len(negative_hits),
        "evidence": evidence,
    }


def classify_candidates(candidates: list[dict[str, Any]], scores: dict[str, int]) -> tuple[str, list[str]]:
    if not candidates:
        return "ONTOLOGY_GAP", ["No current exact-skill rule produced a positive candidate."]

    ordered = sorted(candidates, key=lambda row: (-int(row["score"]), str(row["skillId"])))
    top = ordered[0]
    runner_score = int(ordered[1]["score"]) if len(ordered) > 1 else -10_000
    winner_gap = int(top["score"]) - runner_score

    title_requirement_met = (not top["requiresTitleMatchForHighConfidence"]) or bool(top["titleMatched"])
    if (
        int(top["score"]) >= scores["highConfidenceMinimum"]
        and title_requirement_met
        and int(top["negativeHitCount"]) == 0
        and winner_gap >= scores["minimumWinnerGap"]
    ):
        return "HIGH_CONFIDENCE_CANDIDATE", [
            f"Top candidate exceeds the high-confidence threshold with winner gap {winner_gap}."
        ]

    if len(ordered) > 1 and winner_gap < scores["minimumWinnerGap"]:
        return "AMBIGUOUS", [
            f"Top two candidates are separated by only {winner_gap} point(s)."
        ]

    if int(top["negativeHitCount"]) > 0:
        return "REVIEW_REQUIRED", ["Top candidate also matched negative discriminator evidence."]

    if int(top["score"]) >= scores["reviewMinimum"]:
        return "REVIEW_REQUIRED", ["Candidate evidence exists but is insufficient for automatic promotion."]

    return "ONTOLOGY_GAP", ["Signals are below the minimum review threshold."]


def unresolved_terms(title: str) -> list[str]:
    words = [
        word
        for word in re.findall(r"[a-z][a-z0-9'-]+", title.lower())
        if len(word) >= 3 and word not in STOPWORDS
    ]
    terms = list(words)
    terms.extend(" ".join(words[i : i + 2]) for i in range(len(words) - 1))
    return terms


def audit() -> dict[str, Any]:
    skill_ids = load_skill_ids()
    mappings = load_existing_mappings()
    rules, scores, rule_errors = load_rules(skill_ids)
    supporting_target_rules = read_catalogue_json(SUPPORTING_TARGET_RULES)
    r3_deferred_codes = load_r3_deferred_codes()

    summary = Counter()
    by_pack: dict[str, Counter] = defaultdict(Counter)
    unresolved_term_counter: Counter[str] = Counter()
    rows: list[dict[str, Any]] = []
    seen_lessons: set[str] = set()
    blockers: list[str] = list(rule_errors)

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
            if code in seen_lessons:
                continue
            seen_lessons.add(code)

            outcomes = clean_list(get_case(lesson, "OutcomeCodes", "outcomeCodes", default=[]))
            source_type = "OfficialMapped" if outcomes else "PedagogicalUnmapped"
            fields = evidence_text(lesson)
            existing = mappings.get(code)

            summary["lessonCount"] += 1
            summary[source_type] += 1
            by_pack[pack_code]["lessonCount"] += 1
            by_pack[pack_code][source_type] += 1

            if existing:
                status = "EXISTING_VERIFIED_MAPPING"
                candidates = [{
                    "skillId": skill,
                    "score": None,
                    "titleMatched": None,
                    "evidence": existing.get("evidence") or [],
                } for skill in clean_list(existing.get("primarySkills"))]
                diagnostics = ["Existing explicit lesson-skill mapping takes precedence over candidate resolution."]
            else:
                candidates = [
                    candidate
                    for rule in rules
                    if (candidate := score_rule(rule, fields, scores)) is not None
                ]
                candidates.sort(key=lambda row: (-int(row["score"]), str(row["skillId"])))
                status, diagnostics = classify_candidates(candidates, scores)

                # R5 closes ontology gaps without authorizing Practice. For
                # Supporting lessons that have no explicit mapping, an exact
                # canonical target identity is derived from the source-backed
                # lesson title using deterministic normalization. This is not a
                # fuzzy title match and it never creates an official outcome.
                if (
                    source_type == "PedagogicalUnmapped"
                    and (
                        status == "ONTOLOGY_GAP"
                        or (status == "REVIEW_REQUIRED" and code in r3_deferred_codes)
                    )
                ):
                    catalogue = resolve_catalogue_target(
                        code,
                        fields["title"],
                        supporting_target_rules,
                    )
                    if catalogue["status"] == "CATALOGUE_TARGET_CLASSIFIED":
                        status = "CATALOGUE_TARGET_CLASSIFIED"
                        diagnostics = [
                            "Canonical Supporting target classified by exact source-backed title normalization.",
                            "This ontology identity is not learner-facing Practice authorization."
                        ]
                        candidates = [{
                            "catalogueTargetId": catalogue["targetId"],
                            "catalogueDomain": catalogue["domain"],
                            "normalizedTarget": catalogue["normalizedTarget"],
                            "score": None,
                            "titleMatched": True,
                            "evidence": [{
                                "type": "CanonicalLessonTitle",
                                "signal": fields["title"],
                            }],
                        }]
                    else:
                        diagnostics = [
                            "Supporting lesson target could not be classified because canonical title evidence is empty."
                        ]

            summary[status] += 1
            by_pack[pack_code][status] += 1

            if status in {"ONTOLOGY_GAP", "AMBIGUOUS", "REVIEW_REQUIRED"}:
                unresolved_term_counter.update(unresolved_terms(fields["title"]))

            rows.append({
                "lessonCode": code,
                "packCode": pack_code,
                "sourceType": source_type,
                "outcomeCodes": outcomes,
                "title": fields["title"],
                "status": status,
                "diagnostics": diagnostics,
                "candidates": candidates[:5],
            })

    mapped_targets_missing = sorted(set(mappings) - seen_lessons)
    blockers.extend(
        f"Existing mapping target is absent from canonical lesson content: {code}"
        for code in mapped_targets_missing
    )

    high_confidence_pedagogical = sum(
        1
        for row in rows
        if row["sourceType"] == "PedagogicalUnmapped"
        and row["status"] == "HIGH_CONFIDENCE_CANDIDATE"
    )
    unresolved_pedagogical = sum(
        1
        for row in rows
        if row["sourceType"] == "PedagogicalUnmapped"
        and row["status"] in {"ONTOLOGY_GAP", "AMBIGUOUS", "REVIEW_REQUIRED"}
    )

    summary["ruleCount"] = len(rules)
    summary["skillRegistryCount"] = len(skill_ids)
    summary["blockerCount"] = len(blockers)
    summary["highConfidencePedagogicalCandidateCount"] = high_confidence_pedagogical
    summary["unresolvedPedagogicalCount"] = unresolved_pedagogical

    return {
        "schemaVersion": 1,
        "audit": "Edulytics deterministic lesson-skill candidate resolution audit",
        "authority": (
            "Candidate and ontology classification only. HIGH_CONFIDENCE_CANDIDATE and "
            "CATALOGUE_TARGET_CLASSIFIED never authorize production generation without an explicit "
            "Practice contract, allowed family, solver, verifier and alignment gate."
        ),
        "scoring": scores,
        "summary": dict(summary),
        "blockers": blockers,
        "topUnresolvedTerms": [
            {"term": term, "count": count}
            for term, count in unresolved_term_counter.most_common(100)
        ],
        "byPack": {
            pack: dict(counter)
            for pack, counter in sorted(by_pack.items())
        },
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
    if args.strict and report["summary"]["blockerCount"]:
        return 2
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
