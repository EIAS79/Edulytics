#!/usr/bin/env python3
from __future__ import annotations

import re
from pathlib import Path
from typing import Any

from supporting_practice_rules import (
    SupportingRule,
    choose_translation,
    clean_list,
    get_case,
    load_rules,
    normalize_space,
    normalize_title,
    read_json,
    validate_rules,
)


def _is_reviewed_exact_pattern(pattern: re.Pattern[str]) -> bool:
    raw = pattern.pattern.strip()
    return raw.startswith("^") and raw.endswith("$")


def _matches_reviewed_exact_title(
    lesson_code: str,
    title: str,
    rule: SupportingRule,
) -> bool:
    exact_patterns = [
        pattern for pattern in rule.title_patterns
        if _is_reviewed_exact_pattern(pattern)
    ]
    if not exact_patterns:
        return False

    normalized = normalize_title(title)
    if not any(pattern.fullmatch(normalized) for pattern in exact_patterns):
        return False

    if rule.code_patterns and not any(
        pattern.search(lesson_code)
        for pattern in rule.code_patterns
    ):
        return False

    return True


def mapping_from_rule(
    lesson_code: str,
    outcome_codes: list[str],
    rule: SupportingRule,
    match_mode: str,
) -> dict[str, Any]:
    source_type = (
        "OfficialReviewedExactTitleRule"
        if match_mode == "EXACT_TITLE"
        else (
            "OfficialReviewedUniqueTitleRule"
            if match_mode == "UNIQUE_REVIEWED_TITLE"
            else "OfficialReviewedCanonicalEvidence"
        )
    )
    confidence = (
        "ReviewedExactTitle"
        if match_mode == "EXACT_TITLE"
        else (
            "ReviewedUniqueTitle"
            if match_mode == "UNIQUE_REVIEWED_TITLE"
            else "ReviewedCanonicalEvidence"
        )
    )
    return {
        "lessonCode": lesson_code,
        "primarySkills": [rule.skill_id],
        "sourceType": source_type,
        "officialOutcomeMapped": True,
        "officialOutcomeCodes": outcome_codes,
        "mappingConfidence": confidence,
        "officialPracticeMatchMode": match_mode,
        "practiceReadiness": "READY_VERIFIED",
        "practiceMechanic": rule.mechanic,
        "allowedQuestionFamilies": list(rule.families),
        "officialPracticeRuleId": rule.rule_id,
        "evidence": [
            "The canonical lesson has official OutcomeCode provenance.",
            (
                "The lesson title full-matches a reviewed anchored Practice target rule."
                if match_mode == "EXACT_TITLE"
                else "Exactly one reviewed Practice target rule matches the canonical lesson title."
            ),
            "Semantic content, family, solver and verifier gates remain independently enforced.",
        ],
    }


def load_reviewed_official_rule_mappings(
    content_dir: Path,
) -> tuple[dict[str, dict[str, Any]], list[str]]:
    rules, errors = load_rules()
    errors.extend(validate_rules(rules))

    mappings: dict[str, dict[str, Any]] = {}
    seen: set[str] = set()

    for path in sorted(content_dir.glob("*.lesson-content-pack.json")):
        try:
            doc = read_json(path)
        except (OSError, ValueError) as ex:
            errors.append(
                f"Unable to read official Practice content pack {path.name}: {ex}"
            )
            continue

        pack_code = str(
            get_case(doc, "PackCode", "packCode", default="") or ""
        ).strip()

        # The current Polish canonical packs use deterministic broad-domain
        # fallback lesson identities. They do not contain the exact official
        # outcome wording needed for safe lesson-level Practice promotion.
        # P13 must classify those nodes explicitly rather than guessing a skill.
        if pack_code == "PL-NATIONAL-MATH":
            continue

        academic_language = str(
            get_case(doc, "AcademicLanguage", "academicLanguage", default="") or ""
        ).strip()
        lessons = get_case(doc, "Lessons", "lessons", default=[])
        if not isinstance(lessons, list):
            continue

        for lesson in lessons:
            if not isinstance(lesson, dict):
                continue

            lesson_code = str(
                get_case(lesson, "LessonCode", "lessonCode", default="") or ""
            ).strip()
            if not lesson_code or lesson_code in seen:
                continue
            seen.add(lesson_code)

            outcomes = clean_list(
                get_case(lesson, "OutcomeCodes", "outcomeCodes", default=[])
            )
            if not outcomes:
                continue

            translation = choose_translation(lesson, academic_language)
            title = normalize_space(
                get_case(translation, "Title", "title", default="")
                or get_case(lesson, "Title", "title", default="")
            )

            exact_candidates = [
                rule
                for rule in rules
                if _matches_reviewed_exact_title(lesson_code, title, rule)
            ]
            if len(exact_candidates) > 1:
                errors.append(
                    "Official exact-title Practice rule collision for "
                    f"{lesson_code}: {title!r} -> "
                    + ", ".join(rule.rule_id for rule in exact_candidates)
                )
                continue

            if len(exact_candidates) == 1:
                candidates = exact_candidates
                match_mode = "EXACT_TITLE"
            else:
                normalized = normalize_title(title)
                candidates = []
                for rule in rules:
                    if rule.title_patterns and not any(
                        pattern.search(normalized)
                        for pattern in rule.title_patterns
                    ):
                        continue
                    if rule.code_patterns and not any(
                        pattern.search(lesson_code)
                        for pattern in rule.code_patterns
                    ):
                        continue
                    candidates.append(rule)

                if len(candidates) == 1:
                    match_mode = "UNIQUE_REVIEWED_TITLE"
                else:
                    evidence = normalize_space(" ".join([
                        normalized,
                        str(get_case(translation, "Explanation", "explanation", default="") or ""),
                        str(get_case(translation, "KeyConceptsAndRules", "keyConceptsAndRules", default="") or ""),
                        str(get_case(translation, "WorkedExamples", "workedExamples", default="") or ""),
                    ]))
                    candidates = []
                    for rule in rules:
                        if rule.title_patterns and not any(
                            pattern.search(evidence)
                            for pattern in rule.title_patterns
                        ):
                            continue
                        if rule.code_patterns and not any(
                            pattern.search(lesson_code)
                            for pattern in rule.code_patterns
                        ):
                            continue
                        candidates.append(rule)

                    if len(candidates) != 1:
                        # Canonical evidence must yield one and only one reviewed
                        # target. Ambiguity remains fail-closed.
                        continue
                    match_mode = "UNIQUE_REVIEWED_CANONICAL_EVIDENCE"

            mappings[lesson_code] = mapping_from_rule(
                lesson_code,
                outcomes,
                candidates[0],
                match_mode,
            )

    return mappings, errors
