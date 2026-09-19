#!/usr/bin/env python3
from __future__ import annotations

import argparse
import hashlib
import html
import json
import re
import sys
import urllib.request
from collections import defaultdict
from pathlib import Path
from typing import Any

from bs4 import BeautifulSoup
from pypdf import PdfReader

ROOT = Path(__file__).resolve().parents[2]
PACK = ROOT / "src/Edulytics.Core/Curriculum/Packs/pl-national-math.curriculum-pack.json"
REPORT = ROOT / "artifacts/math-intelligence/pl-outcome-source-evidence.json"

PRIMARY_URL = "https://eli.gov.pl/api/acts/DU/2024/996/text.html"
UPPER_PDF_URL = "https://eli.gov.pl/api/acts/DU/2024/1019/text/I/D20241019.pdf"

USER_AGENT = "Edulytics-Polish-Practice-Remediation/1.0 (+https://edulytiks.com)"


def read_json(path: Path) -> Any:
    return json.loads(path.read_text(encoding="utf-8-sig"))


def fetch_bytes(url: str) -> bytes:
    req = urllib.request.Request(url, headers={"User-Agent": USER_AGENT})
    with urllib.request.urlopen(req, timeout=60) as response:
        return response.read()


def clean(value: str) -> str:
    value = html.unescape(value or "")
    value = value.replace("\u00a0", " ")
    value = value.replace("\xad", "")
    value = re.sub(r"\s+", " ", value)
    return value.strip()


def lines_from_html(body: bytes) -> list[str]:
    soup = BeautifulSoup(body.decode("utf-8", errors="replace"), "html.parser")
    return [clean(x) for x in soup.get_text("\n").splitlines() if clean(x)]


def lines_from_pdf(body: bytes, tmp_path: Path) -> list[str]:
    tmp_path.parent.mkdir(parents=True, exist_ok=True)
    tmp_path.write_bytes(body)
    reader = PdfReader(str(tmp_path))
    pages: list[str] = []
    in_math = False
    for page in reader.pages:
        text = page.extract_text() or ""
        probe = clean(text).upper()
        if not in_math and "MATEMATYKA" in probe and "ZAKRES PODSTAWOWY I ROZSZERZONY" in probe:
            in_math = True
        if not in_math:
            continue
        if pages and re.search(r"(^|\n)\s*INFORMATYKA\s*($|\n)", text, re.I):
            break
        pages.append(text)
    joined = "\n".join(pages)
    return [clean(x) for x in joined.splitlines() if clean(x)]


def outcome_parts(code: str) -> tuple[str, str]:
    pieces = code.split(":")
    if len(pieces) < 4:
        raise ValueError(f"Unexpected OutcomeCode: {code}")
    mode = pieces[-3]
    ordinal = pieces[-2]
    return mode, ordinal


def _search_key(value: str) -> str:
    return re.sub(r"[^0-9a-ząćęłńóśźż]+", "", clean(value).casefold())


def find_line(lines: list[str], needle: str, start: int = 0) -> int:
    wanted = _search_key(needle)
    # Prefer an exact physical line first. This is important for section
    # boundaries: returning the preceding line would truncate the final
    # numbered requirement in the previous section.
    for i in range(start, len(lines)):
        if wanted and wanted in _search_key(lines[i]):
            return i
    # Only then allow a heading that was genuinely split across two lines.
    for i in range(start, max(start, len(lines) - 1)):
        left = _search_key(lines[i])
        right = _search_key(lines[i + 1])
        if wanted and wanted in (left + right):
            # If the complete heading begins on the next line, preserve that
            # line as the boundary; otherwise the split begins on the current.
            if wanted in right:
                return i + 1
            return i
    return -1


def numbered_items(
    lines: list[str],
    start: int,
    end: int,
    expected: int,
) -> list[str]:
    result: list[str] = []
    current: list[str] = []
    expected_no = 1
    active = False

    for raw in lines[start:end]:
        line = clean(raw)
        match = re.match(r"^(?:#+\s*)?(\d+)\)\s*(.*)$", line)
        if match:
            number = int(match.group(1))
            if number == expected_no:
                if active:
                    result.append(clean(" ".join(current)))
                current = []
                active = True
                tail = clean(match.group(2))
                if tail:
                    current.append(tail)
                expected_no += 1
                continue
            # After the expected sequence is complete, a restarted 1) marks
            # the next scope (for example the extended section).
            if active and expected_no == expected + 1 and number == 1:
                break

        if active:
            # Ignore standalone markdown-style structural headings that may
            # appear between a number marker and its textual payload.
            if re.fullmatch(r"#+\s*", line):
                continue
            current.append(line)

    if active and len(result) < expected:
        result.append(clean(" ".join(current)))

    return result[:expected]


def domain_ranges(
    lines: list[str],
    domains: list[dict[str, Any]],
    start: int,
) -> dict[str, tuple[int, int]]:
    located: list[tuple[int, dict[str, Any]]] = []
    cursor = start
    for domain in domains:
        index = find_line(lines, str(domain["Title"]), cursor)
        if index < 0:
            # Fall back to searching from the band start; some source renderers
            # insert page headers that disturb strict sequential lookup.
            index = find_line(lines, str(domain["Title"]), start)
        if index < 0:
            raise RuntimeError(f"Unable to locate official Polish domain: {domain['Title']}")
        located.append((index, domain))
        cursor = index + 1

    result: dict[str, tuple[int, int]] = {}
    for pos, (index, domain) in enumerate(located):
        end = located[pos + 1][0] if pos + 1 < len(located) else len(lines)
        result[str(domain["Code"])] = (index, end)
    return result


def extract_primary(
    lines: list[str],
    nodes: list[dict[str, Any]],
) -> dict[str, str]:
    by_code = {str(x["Code"]): x for x in nodes}
    outcomes = [
        x for x in nodes
        if x.get("Kind") == "Outcome"
        and x.get("IsOfficial")
        and x.get("IsActive")
        and int(x.get("LogicalLevelTo") or 0) <= 8
    ]
    domains = [
        x for x in nodes
        if x.get("Kind") == "Domain"
        and x.get("IsOfficial")
        and x.get("IsActive")
        and int(x.get("LogicalLevelTo") or 0) <= 8
    ]

    bands = [
        (1, 3, "Edukacja matematyczna"),
        (4, 6, "KLASY IV-VI"),
        (7, 8, "KLASY VII"),
    ]
    result: dict[str, str] = {}

    for level_from, level_to, marker in bands:
        band_domains = [
            d for d in domains
            if int(d.get("LogicalLevelFrom") or 0) == level_from
            and int(d.get("LogicalLevelTo") or 0) == level_to
        ]
        band_domains.sort(key=lambda x: int(x.get("SortOrder") or 0))
        band_start = find_line(lines, marker)
        if band_start < 0:
            raise RuntimeError(f"Unable to locate primary Mathematics band marker: {marker}")
        ranges = domain_ranges(lines, band_domains, band_start)

        grouped: dict[str, list[dict[str, Any]]] = defaultdict(list)
        for outcome in outcomes:
            parent = str(outcome.get("ParentCode") or "")
            domain = by_code.get(parent)
            if domain is None:
                continue
            if int(domain.get("LogicalLevelFrom") or 0) != level_from:
                continue
            grouped[parent].append(outcome)

        for domain in band_domains:
            code = str(domain["Code"])
            rows = grouped.get(code, [])
            rows.sort(key=lambda x: int(x.get("SortOrder") or 0))
            start, end = ranges[code]
            items = numbered_items(lines, start + 1, end, len(rows))
            if len(items) != len(rows):
                raise RuntimeError(
                    f"Primary extraction count mismatch for {domain['Title']}: "
                    f"expected {len(rows)}, got {len(items)}"
                )
            for outcome, text in zip(rows, items):
                result[str(outcome["Code"])] = text

    return result


def extract_upper(
    lines: list[str],
    nodes: list[dict[str, Any]],
) -> dict[str, str]:
    by_code = {str(x["Code"]): x for x in nodes}
    outcomes = [
        x for x in nodes
        if x.get("Kind") == "Outcome"
        and x.get("IsOfficial")
        and x.get("IsActive")
        and int(x.get("LogicalLevelFrom") or 0) >= 9
    ]
    domains = [
        x for x in nodes
        if x.get("Kind") == "Domain"
        and x.get("IsOfficial")
        and x.get("IsActive")
        and int(x.get("LogicalLevelFrom") or 0) >= 9
    ]
    domains.sort(key=lambda x: int(x.get("SortOrder") or 0))

    detail_start = find_line(lines, "Treści nauczania")
    if detail_start < 0:
        raise RuntimeError("Unable to locate upper-secondary Mathematics detailed requirements.")

    ranges = domain_ranges(lines, domains, detail_start)
    result: dict[str, str] = {}

    grouped: dict[str, list[dict[str, Any]]] = defaultdict(list)
    for outcome in outcomes:
        grouped[str(outcome.get("ParentCode") or "")].append(outcome)

    for domain in domains:
        domain_code = str(domain["Code"])
        rows = grouped.get(domain_code, [])
        rows.sort(key=lambda x: int(x.get("SortOrder") or 0))
        start, end = ranges[domain_code]
        segment = lines[start + 1:end]

        basic_rows = [x for x in rows if outcome_parts(str(x["Code"]))[0] == "basic"]
        extended_rows = [x for x in rows if outcome_parts(str(x["Code"]))[0] == "extended"]

        basic_pos = find_line(segment, "Zakres podstawowy")
        extended_pos = find_line(segment, "Zakres rozszerzony")
        if basic_rows and basic_pos < 0:
            raise RuntimeError(f"Missing basic section for {domain['Title']}")

        basic_end = extended_pos if extended_pos >= 0 else len(segment)
        basic_inline = [
            x for x in basic_rows
            if outcome_parts(str(x["Code"]))[1] == "inline"
        ]
        basic_numbered = [
            x for x in basic_rows
            if outcome_parts(str(x["Code"]))[1] != "inline"
        ]
        if basic_inline:
            if len(basic_inline) != 1 or basic_numbered:
                raise RuntimeError(
                    f"Unsupported mixed inline/numbered basic section: {domain['Title']}"
                )
            inline_text = clean(" ".join(segment[basic_pos:basic_end]))
            inline_text = re.sub(
                r"^Zakres podstawowy\.?\s*Uczeń\s*",
                "",
                inline_text,
                flags=re.I,
            )
            inline_text = re.sub(
                r"^Zakres podstawowy\.?\s*",
                "",
                inline_text,
                flags=re.I,
            )
            if not inline_text:
                raise RuntimeError(f"Blank inline basic requirement for {domain['Title']}")
            result[str(basic_inline[0]["Code"])] = inline_text
        else:
            basic_items = numbered_items(
                segment,
                basic_pos + 1,
                basic_end,
                len(basic_numbered),
            )
            if len(basic_items) != len(basic_numbered):
                raise RuntimeError(
                    f"Upper basic extraction mismatch for {domain['Title']}: "
                    f"expected {len(basic_numbered)}, got {len(basic_items)}"
                )
            for outcome, text in zip(basic_numbered, basic_items):
                result[str(outcome["Code"])] = text

        if not extended_rows:
            continue
        if extended_pos < 0:
            raise RuntimeError(f"Missing extended section for {domain['Title']}")

        inline_rows = [
            x for x in extended_rows
            if outcome_parts(str(x["Code"]))[1] == "inline"
        ]
        numbered_rows = [
            x for x in extended_rows
            if outcome_parts(str(x["Code"]))[1] != "inline"
        ]

        if inline_rows:
            if len(inline_rows) != 1 or numbered_rows:
                raise RuntimeError(
                    f"Unsupported mixed inline/numbered extended section: {domain['Title']}"
                )
            text = clean(" ".join(segment[extended_pos + 1:]))
            # Remove boilerplate lead-in while retaining the exact mathematical
            # requirement after "ponadto" when present.
            text = re.sub(
                r"^Uczeń spełnia wymagania określone dla zakresu podstawowego,?\s*a ponadto\s*",
                "",
                text,
                flags=re.I,
            )
            text = re.sub(
                r"^Uczeń spełnia wymagania określone dla zakresu podstawowego,?\s*",
                "",
                text,
                flags=re.I,
            )
            if not text:
                raise RuntimeError(f"Blank inline extended requirement for {domain['Title']}")
            result[str(inline_rows[0]["Code"])] = text
            continue

        extended_items = numbered_items(
            segment,
            extended_pos + 1,
            len(segment),
            len(numbered_rows),
        )
        if len(extended_items) != len(numbered_rows):
            raise RuntimeError(
                f"Upper extended extraction mismatch for {domain['Title']}: "
                f"expected {len(numbered_rows)}, got {len(extended_items)}"
            )
        for outcome, text in zip(numbered_rows, extended_items):
            result[str(outcome["Code"])] = text

    return result


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--write-report", action="store_true")
    parser.add_argument("--strict", action="store_true")
    args = parser.parse_args()

    pack = read_json(PACK)
    nodes = pack.get("Nodes") or []
    official_outcomes = [
        x for x in nodes
        if x.get("Kind") == "Outcome"
        and x.get("IsOfficial")
        and x.get("IsActive")
    ]

    errors: list[str] = []
    evidence: dict[str, str] = {}
    source_meta: dict[str, Any] = {}

    try:
        primary = fetch_bytes(PRIMARY_URL)
        source_meta["primary"] = {
            "url": PRIMARY_URL,
            "sha256": hashlib.sha256(primary).hexdigest(),
            "bytes": len(primary),
        }
        evidence.update(extract_primary(lines_from_html(primary), nodes))
    except Exception as ex:
        errors.append(f"Primary source extraction failed: {ex}")

    try:
        upper = fetch_bytes(UPPER_PDF_URL)
        source_meta["upper"] = {
            "url": UPPER_PDF_URL,
            "sha256": hashlib.sha256(upper).hexdigest(),
            "bytes": len(upper),
        }
        evidence.update(
            extract_upper(
                lines_from_pdf(upper, Path("/tmp/edulytics-polish-upper.pdf")),
                nodes,
            )
        )
    except Exception as ex:
        errors.append(f"Upper source extraction failed: {ex}")

    rows: list[dict[str, Any]] = []
    missing: list[str] = []
    for outcome in sorted(official_outcomes, key=lambda x: int(x.get("SortOrder") or 0)):
        code = str(outcome["Code"])
        text = clean(evidence.get(code, ""))
        if not text:
            missing.append(code)
        parent = next(
            (x for x in nodes if str(x.get("Code") or "") == str(outcome.get("ParentCode") or "")),
            None,
        )
        mode, ordinal = outcome_parts(code)
        rows.append({
            "outcomeCode": code,
            "domain": None if parent is None else parent.get("Title"),
            "logicalLevelFrom": outcome.get("LogicalLevelFrom"),
            "logicalLevelTo": outcome.get("LogicalLevelTo"),
            "pathway": outcome.get("Pathway"),
            "mode": mode,
            "ordinal": ordinal,
            "sourceUrl": outcome.get("SourceUrl"),
            "sourceLocator": outcome.get("SourceLocator"),
            "sourceContentHash": outcome.get("ContentHash"),
            "officialText": text,
            "officialTextSha256": hashlib.sha256(text.encode("utf-8")).hexdigest() if text else None,
        })

    report = {
        "schemaVersion": 1,
        "audit": "Polish National Mathematics official Outcome evidence reconstruction",
        "packCode": pack.get("PackCode"),
        "versionCode": pack.get("VersionCode"),
        "officialOutcomeCount": len(official_outcomes),
        "resolvedOutcomeCount": len(evidence),
        "missingOutcomeCount": len(missing),
        "sourceEvidence": source_meta,
        "errors": errors,
        "missingOutcomeCodes": missing,
        "outcomes": rows,
    }

    if args.write_report:
        REPORT.parent.mkdir(parents=True, exist_ok=True)
        REPORT.write_text(
            json.dumps(report, ensure_ascii=False, indent=2) + "\n",
            encoding="utf-8",
        )

    print(json.dumps({
        "officialOutcomeCount": report["officialOutcomeCount"],
        "resolvedOutcomeCount": report["resolvedOutcomeCount"],
        "missingOutcomeCount": report["missingOutcomeCount"],
        "errorCount": len(errors),
    }, ensure_ascii=False, indent=2))

    if args.strict and (errors or missing or len(evidence) != len(official_outcomes)):
        return 1
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
