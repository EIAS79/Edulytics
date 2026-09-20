#!/usr/bin/env python3
from __future__ import annotations

from pathlib import Path
from typing import Any

from supporting_practice_rules import (
    SupportingRule,
    clean_list,
    load_rules as load_target_rules,
    read_json,
    validate_rules as validate_target_rules,
)

ROOT = Path(__file__).resolve().parents[2]
MAP_FILE = (
    ROOT
    / "src/Edulytics.Core/Mathematics/Curriculum"
    / "polish-outcome-practice-map.v1.json"
)


def load_polish_outcome_mappings() -> tuple[dict[str, dict[str, Any]], list[str]]:
    doc = read_json(MAP_FILE)
    rules, errors = load_target_rules()
    errors.extend(validate_target_rules(rules))
    rule_by_id = {rule.rule_id: rule for rule in rules}

    entries = doc.get("entries") or []
    declared = int(doc.get("outcomeCount") or 0)
    result: dict[str, dict[str, Any]] = {}
    serials: set[int] = set()

    for row in entries:
        if not isinstance(row, dict):
            errors.append("Polish Practice map contains a non-object entry.")
            continue
        code = str(row.get("outcomeCode") or "").strip()
        try:
            serial = int(row.get("serial"))
        except (TypeError, ValueError):
            errors.append(f"Polish Practice map has invalid serial for {code!r}.")
            continue
        target_ids = clean_list(row.get("targetRuleIds"))
        if not code:
            errors.append(f"Polish Practice map serial {serial} has no OutcomeCode.")
            continue
        if code in result:
            errors.append(f"Duplicate Polish Practice OutcomeCode: {code}.")
            continue
        if serial in serials:
            errors.append(f"Duplicate Polish Practice serial: {serial}.")
            continue
        serials.add(serial)

        target_rules: list[SupportingRule] = []
        for target_id in target_ids:
            rule = rule_by_id.get(target_id)
            if rule is None:
                errors.append(
                    f"Polish Practice outcome {code} references unknown target rule {target_id}."
                )
                continue
            target_rules.append(rule)
        if not target_rules:
            errors.append(f"Polish Practice outcome {code} has no valid target rules.")

        result[code] = {
            **row,
            "targetRules": target_rules,
        }

    if declared != 306 or len(result) != 306 or len(serials) != 306:
        errors.append(
            "Polish Practice map must contain exactly 306 unique outcomes: "
            f"declared={declared}, outcomes={len(result)}, serials={len(serials)}."
        )
    missing_serials = [str(i) for i in range(1, 307) if i not in serials]
    if missing_serials:
        errors.append(
            "Polish Practice map missing serials: " + ", ".join(missing_serials)
        )

    return result, errors


def lesson_mapping(
    lesson_code: str,
    outcome_codes: list[str],
    mappings: dict[str, dict[str, Any]],
) -> dict[str, Any] | None:
    resolved = [mappings.get(code) for code in outcome_codes]
    if not outcome_codes or not all(row is not None for row in resolved):
        return None

    rules: dict[str, SupportingRule] = {}
    for row in resolved:
        assert row is not None
        for rule in row["targetRules"]:
            rules[rule.rule_id] = rule

    ordered = [rules[key] for key in sorted(rules)]
    if not ordered:
        return None

    skills = sorted({rule.skill_id for rule in ordered})
    families = sorted({
        family
        for rule in ordered
        for family in rule.families
    })
    mechanics = sorted({rule.mechanic for rule in ordered})

    return {
        "lessonCode": lesson_code,
        "primarySkills": skills,
        "sourceType": "PolishOfficialOutcomeMap",
        "officialOutcomeMapped": True,
        "officialOutcomeCodes": outcome_codes,
        "mappingConfidence": "ReviewedExactOutcomeCode",
        "officialPracticeMatchMode": "POLISH_OFFICIAL_OUTCOME_MAP",
        "practiceReadiness": "READY_VERIFIED",
        "practiceMechanic": (
            mechanics[0]
            if len(mechanics) == 1
            else "POLISH_OFFICIAL_MULTI_TARGET"
        ),
        "allowedQuestionFamilies": families,
        "officialPracticeRuleIds": sorted(rules),
        "evidence": [
            "Every Polish lesson OutcomeCode resolves through the reviewed exact Polish outcome Practice map.",
            "The mapping key is the accepted official OutcomeCode, never the generated Phase-29 lesson title.",
            "Each mapped target delegates to an existing solver/verifier-backed Practice target rule.",
        ],
    }
