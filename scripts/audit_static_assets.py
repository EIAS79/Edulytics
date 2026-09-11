#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
WWWROOT = ROOT / "src" / "Edulytics.Web" / "wwwroot"
ARCHIVE = ROOT / "archive"

ASSET_EXTENSIONS = {
    ".css", ".js", ".png", ".jpg", ".jpeg", ".webp", ".svg", ".gif", ".ico", ".txt"
}
TEXT_EXTENSIONS = {
    ".cs", ".cshtml", ".razor", ".css", ".js", ".json", ".md", ".txt", ".xml", ".props",
    ".targets", ".yml", ".yaml", ".ps1", ".sh", ".py", ".csproj", ".sln", ".config"
}
SKIP_PARTS = {".git", "bin", "obj", "node_modules", ".idea", ".vs"}
KEEP_BY_CONVENTION = {
    "favicon.ico",
    "CONTENT_LICENSES.md",
}


def should_skip(path: Path) -> bool:
    try:
        rel = path.relative_to(ROOT)
    except ValueError:
        return True
    if any(part in SKIP_PARTS for part in rel.parts):
        return True
    if ARCHIVE in path.parents:
        return True
    return False


def read_text(path: Path) -> str | None:
    try:
        return path.read_text(encoding="utf-8")
    except (UnicodeDecodeError, OSError):
        return None


def build_text_index() -> list[tuple[Path, str]]:
    result: list[tuple[Path, str]] = []
    for path in ROOT.rglob("*"):
        if not path.is_file() or should_skip(path):
            continue
        if path.suffix.lower() not in TEXT_EXTENSIONS and path.name not in {"Dockerfile"}:
            continue
        text = read_text(path)
        if text is not None:
            result.append((path, text))
    return result


def asset_tokens(asset: Path) -> tuple[str, ...]:
    rel = asset.relative_to(WWWROOT).as_posix()
    basename = asset.name
    return (
        rel,
        "/" + rel,
        "~/" + rel,
        basename,
    )


def main() -> int:
    parser = argparse.ArgumentParser(description="Conservative reference audit for Edulytics wwwroot assets")
    parser.add_argument("--json", action="store_true", help="Emit JSON only")
    parser.add_argument("--report", default="", help="Optional report file path")
    args = parser.parse_args()

    text_index = build_text_index()
    assets: list[Path] = []
    for path in WWWROOT.rglob("*"):
        if not path.is_file():
            continue
        rel = path.relative_to(WWWROOT)
        if rel.parts and rel.parts[0] == "lib":
            continue
        if path.suffix.lower() not in ASSET_EXTENSIONS:
            continue
        if path.name in KEEP_BY_CONVENTION:
            continue
        assets.append(path)

    report = []
    for asset in sorted(assets):
        refs: list[str] = []
        tokens = asset_tokens(asset)
        for source, text in text_index:
            if source == asset:
                continue
            if any(token in text for token in tokens):
                refs.append(source.relative_to(ROOT).as_posix())
        report.append({
            "asset": asset.relative_to(ROOT).as_posix(),
            "size": asset.stat().st_size,
            "references": sorted(set(refs)),
            "reference_count": len(set(refs)),
        })

    unused = [item for item in report if item["reference_count"] == 0]
    used = [item for item in report if item["reference_count"] > 0]
    payload = {
        "scanned_assets": len(report),
        "referenced_assets": len(used),
        "unreferenced_candidates": len(unused),
        "unreferenced_bytes": sum(item["size"] for item in unused),
        "unused": unused,
    }

    if args.json:
        output = json.dumps(payload, ensure_ascii=False, indent=2)
    else:
        lines = [
            f"Scanned assets: {payload['scanned_assets']}",
            f"Referenced assets: {payload['referenced_assets']}",
            f"Unreferenced candidates: {payload['unreferenced_candidates']}",
            f"Unreferenced bytes: {payload['unreferenced_bytes']}",
            "",
            "UNREFERENCED CANDIDATES (manual review required):",
        ]
        for item in unused:
            lines.append(f"{item['size']:>10}  {item['asset']}")
        output = "\n".join(lines)

    print(output)
    if args.report:
        report_path = ROOT / args.report
        report_path.parent.mkdir(parents=True, exist_ok=True)
        report_path.write_text(output + "\n", encoding="utf-8")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
