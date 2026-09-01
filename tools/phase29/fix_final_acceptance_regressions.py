#!/usr/bin/env python3
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]


def patch(path: str, old: str, new: str, label: str) -> None:
    target = ROOT / path
    text = target.read_text(encoding="utf-8")
    if new in text:
        print(f"PASS already fixed: {label}")
        return
    if old not in text:
        raise SystemExit(f"FAIL missing regression anchor: {label} ({path})")
    target.write_text(text.replace(old, new, 1), encoding="utf-8")
    print(f"PASS fixed: {label}")


# Acceptance decision #5 is explicit: the library keeps exactly
# Total lessons / Production ready / Supporting lessons.
# The old test expectation for officiallyAlignedCount is therefore stale.
patch(
    "tests/Edulytics.Tests/Phase29/Phase29LessonContentVisualContractTests.cs",
    '''        Assert.Contains(\n            "officiallyAlignedCount",\n            index);\n''',
    '''        Assert.DoesNotContain(\n            "officiallyAlignedCount",\n            index);\n''',
    "remove stale Officially Aligned KPI test expectation",
)


# The final-acceptance presentation contract intentionally creates a safe,
# local deterministic fallback visual when source content has no reconstructable
# explicit diagram. Stage-1 visual acceptance inspects WorkedExamples, so the
# same fallback policy must apply to examples as well as explanations.
patch(
    "src/Edulytics.Web/Presentation/LessonPresentationParser.cs",
    '''        if (\n            sectionKind == "explanation" &&\n            result.All(x => !x.IsVisual))\n''',
    '''        if (\n            sectionKind is "explanation" or "examples" &&\n            result.All(x => !x.IsVisual))\n''',
    "apply safe fallback visuals to worked examples",
)
