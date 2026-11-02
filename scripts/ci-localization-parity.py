#!/usr/bin/env python3

from __future__ import annotations

import sys
import xml.etree.ElementTree as ET
from pathlib import Path


ROOT = Path(
    "src/Edulytics.Web/Resources"
)

LOCALIZED_CULTURES = ("pl", "ar")
LOCALIZED_SUFFIXES = tuple(
    f".{culture}.resx"
    for culture in LOCALIZED_CULTURES
)


def fail(message: str) -> None:
    raise SystemExit(f"FAIL: {message}")


def keys(path: Path) -> set[str]:
    root = ET.parse(path).getroot()
    result: set[str] = set()

    for data in root.findall("data"):
        name = data.attrib.get("name")

        if not name:
            fail(
                f"resource entry without name: {path}"
            )

        if name in result:
            fail(
                f"duplicate resource key {name}: {path}"
            )

        result.add(name)

        value = data.find("value")

        if (
            value is None
            or value.text is None
            or not value.text.strip()
        ):
            fail(
                f"empty localized value "
                f"{name}: {path}"
            )

    return result


if not ROOT.is_dir():
    fail(f"resource directory missing: {ROOT}")

defaults = sorted(
    path
    for path in ROOT.rglob("*.resx")
    if not path.name.endswith(
        LOCALIZED_SUFFIXES
    )
)

if not defaults:
    fail("no default resource files found")

localized_pair_count = 0
key_count = 0

for default in defaults:
    default_keys = keys(default)
    key_count += len(default_keys)

    for culture in LOCALIZED_CULTURES:
        localized = default.with_name(
            f"{default.stem}.{culture}.resx"
        )

        if not localized.is_file():
            fail(
                f"{culture} resource counterpart missing: "
                f"{localized}"
            )

        localized_keys = keys(localized)

        missing = sorted(
            default_keys - localized_keys
        )

        orphan = sorted(
            localized_keys - default_keys
        )

        if missing or orphan:
            fail(
                f"resource parity mismatch: {default}; "
                f"culture={culture}; "
                f"missing={missing}; "
                f"orphan={orphan}"
            )

        localized_pair_count += 1

for culture in LOCALIZED_CULTURES:
    suffix = f".{culture}.resx"

    for localized in ROOT.rglob(
        f"*{suffix}"
    ):
        base = localized.with_name(
            localized.name.removesuffix(
                suffix
            )
            + ".resx"
        )

        if not base.is_file():
            fail(
                f"orphan {culture} resource: "
                f"{localized}"
            )

print(
    "PASS: EN/default ↔ PL ↔ AR resource parity "
    f"sets={len(defaults)}, "
    f"localized_pairs={localized_pair_count}, "
    f"keys={key_count}"
)
