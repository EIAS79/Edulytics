#!/usr/bin/env python3
from __future__ import annotations

import re
from dataclasses import dataclass
from pathlib import Path
from typing import Any

from supporting_practice_rules import (
    SupportingRule,
    clean_list,
    get_case,
    load_rules as load_target_rules,
    normalize_space,
    read_json,
    validate_rules as validate_target_rules,
)

ROOT = Path(__file__).resolve().parents[2]
RULES_FILE = (
    ROOT
    / "src/Edulytics.Core/Mathematics/Curriculum"
    / "official-outcome-practice-rules.v1.json"
)
CURRICULUM_PACK_DIR = ROOT / "src/Edulytics.Core/Curriculum/Packs"


@dataclass(frozen=True)
class OfficialOutcomeRule:
    rule_id: str
    priority: int
    code_patterns: tuple[re.Pattern[str], ...]
    text_patterns: tuple[re.Pattern[str], ...]
    target_rule: SupportingRule


@dataclass(frozen=True)
class OfficialOutcomeResolution:
    outcome_code: str
    rule_id: str
    target_rule: SupportingRule


def _compile(
    values: Any,
    context: str,
    errors: list[str],
) -> tuple[re.Pattern[str], ...]:
    result: list[re.Pattern[str]] = []
    for raw in clean_list(values):
        try:
            result.append(re.compile(raw, re.I | re.S))
        except re.error as ex:
            errors.append(f"Invalid official Practice regex {context}: {raw!r}: {ex}")
    return tuple(result)


def load_rules() -> tuple[list[OfficialOutcomeRule], list[str]]:
    doc = read_json(RULES_FILE)
    targets, errors = load_target_rules()
    errors.extend(validate_target_rules(targets))
    target_by_id = {rule.rule_id: rule for rule in targets}

    result: list[OfficialOutcomeRule] = []
    seen: set[str] = set()
    for row in doc.get("rules") or []:
        if not isinstance(row, dict):
            continue
        rule_id = str(row.get("id") or "").strip()
        if not rule_id:
            errors.append("Official outcome Practice rule has no id.")
            continue
        if rule_id in seen:
            errors.append(f"Duplicate official outcome Practice rule id: {rule_id}")
            continue
        seen.add(rule_id)

        try:
            priority = int(row.get("priority"))
        except (TypeError, ValueError):
            errors.append(f"Official outcome Practice rule {rule_id} has invalid priority.")
            continue

        target_id = str(row.get("targetRuleId") or "").strip()
        target = target_by_id.get(target_id)
        if target is None:
            errors.append(
                f"Official outcome Practice rule {rule_id} references unknown target rule {target_id!r}."
            )
            continue

        code_patterns = _compile(
            row.get("codePatterns"),
            f"{rule_id}.codePatterns",
            errors,
        )
        text_patterns = _compile(
            row.get("textPatterns"),
            f"{rule_id}.textPatterns",
            errors,
        )
        if not code_patterns:
            errors.append(
                f"Official outcome Practice rule {rule_id} has no OutcomeCode scope."
            )
            continue

        result.append(
            OfficialOutcomeRule(
                rule_id,
                priority,
                code_patterns,
                text_patterns,
                target,
            )
        )

    return result, errors


def load_outcome_evidence() -> tuple[dict[str, str], list[str]]:
    evidence: dict[str, str] = {}
    errors: list[str] = []
    for path in sorted(CURRICULUM_PACK_DIR.glob("*.curriculum-pack.json")):
        try:
            doc = read_json(path)
        except (OSError, ValueError) as ex:
            errors.append(f"Unable to read curriculum pack {path.name}: {ex}")
            continue

        nodes = get_case(doc, "Nodes", "nodes", default=[])
        if not isinstance(nodes, list):
            continue
        for node in nodes:
            if not isinstance(node, dict):
                continue
            kind = str(get_case(node, "Kind", "kind", default="") or "").strip()
            if kind.lower() not in {"standard", "outcome", "reference"}:
                continue
            code = str(get_case(node, "Code", "code", default="") or "").strip()
            if not code:
                continue
            text = normalize_space(
                " ".join(
                    [
                        str(get_case(node, "Title", "title", default="") or ""),
                        str(
                            get_case(
                                node,
                                "OfficialText",
                                "officialText",
                                default="",
                            )
                            or ""
                        ),
                        str(
                            get_case(
                                node,
                                "AuthorDescription",
                                "authorDescription",
                                default="",
                            )
                            or ""
                        ),
                    ]
                )
            )
            evidence.setdefault(code, text)

    return evidence, errors


def load_resolutions() -> tuple[dict[str, OfficialOutcomeResolution], list[str]]:
    rules, errors = load_rules()
    evidence, evidence_errors = load_outcome_evidence()
    errors.extend(evidence_errors)

    result: dict[str, OfficialOutcomeResolution] = {}
    for outcome_code, text in sorted(evidence.items()):
        matches = [
            rule
            for rule in rules
            if any(pattern.search(outcome_code) for pattern in rule.code_patterns)
            and (
                not rule.text_patterns
                or any(pattern.search(text) for pattern in rule.text_patterns)
            )
        ]
        if not matches:
            continue

        highest = max(rule.priority for rule in matches)
        winners = [rule for rule in matches if rule.priority == highest]
        if len(winners) != 1:
            errors.append(
                f"Official Practice outcome mapping ambiguous for {outcome_code}: "
                + ", ".join(rule.rule_id for rule in winners)
            )
            continue

        winner = winners[0]
        result[outcome_code] = OfficialOutcomeResolution(
            outcome_code,
            winner.rule_id,
            winner.target_rule,
        )

    return result, errors
