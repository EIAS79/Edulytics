#!/usr/bin/env python3
from __future__ import annotations

from dataclasses import dataclass
from typing import Any


@dataclass(frozen=True)
class PracticeEligibility:
    status: str
    reason_code: str
    evidence: tuple[str, ...]


def _text(row: dict[str, Any], *names: str) -> str:
    for name in names:
        if name in row:
            return str(row.get(name) or "").strip()
    return ""


def classify_lesson(
    pack: dict[str, Any],
    lesson: dict[str, Any],
) -> PracticeEligibility:
    pack_code = _text(pack, "PackCode", "packCode")
    pedagogical_source_type = _text(
        pack,
        "PedagogicalSourceType",
        "pedagogicalSourceType",
    )
    title_provenance = _text(
        lesson,
        "TitleProvenance",
        "titleProvenance",
    )
    adaptation_status = _text(
        lesson,
        "AdaptationStatus",
        "adaptationStatus",
    )

    # Polish Phase-29 fallback nodes are deterministic identities attached to
    # official requirements, not independently sourced pedagogical lessons.
    # Their canonical body is deliberately broad and repeated within a domain.
    # Treating the synthetic "ćwiczenie NN" identities as exact standalone
    # Practice targets would invent lesson-level mathematical specificity that
    # the repository does not contain.
    if (
        pack_code == "PL-NATIONAL-MATH"
        and pedagogical_source_type == "OfficialFrameworkOnly"
        and title_provenance == "EdulyticsDerivedFromOfficialOutcome"
        and "deterministic one-outcome-per-lesson fallback identity"
        in adaptation_status.lower()
    ):
        return PracticeEligibility(
            "NON_STANDALONE_WITH_EVIDENCE",
            "OFFICIAL_FRAMEWORK_FALLBACK_IDENTITY",
            (
                "Pack uses OfficialFrameworkOnly rather than a standalone pedagogical source.",
                "Lesson title provenance is EdulyticsDerivedFromOfficialOutcome.",
                "AdaptationStatus identifies the node as a deterministic one-outcome-per-lesson fallback identity.",
                "The node remains curriculum-visible, but exact lesson Practice must not be invented from its broad fallback title/body.",
            ),
        )

    return PracticeEligibility(
        "PRACTICE_ELIGIBLE",
        "STANDALONE_OR_EXPLICIT_PRACTICE_TARGET",
        (),
    )
