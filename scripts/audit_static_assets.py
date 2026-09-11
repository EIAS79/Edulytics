#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from collections import deque
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
    return (
        rel,
        "/" + rel,
        "~/" + rel,
        asset.name,
    )


def main() -> int:
    parser = argparse.ArgumentParser(description="Conservative reachability audit for Edulytics wwwroot assets")
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

    assets = sorted(assets)
    asset_set = set(assets)
    tokens = {asset: asset_tokens(asset) for asset in assets}
    incoming: dict[Path, set[Path]] = {asset: set() for asset in assets}
    edges: dict[Path, set[Path]] = {asset: set() for asset in assets}
    roots: set[Path] = set()
    root_sources: dict[Path, set[str]] = {asset: set() for asset in assets}

    for source, text in text_index:
        source_is_asset = source in asset_set
        for target in assets:
            if source == target:
                continue
            if not any(token in text for token in tokens[target]):
                continue
            if source_is_asset:
                edges[source].add(target)
                incoming[target].add(source)
            else:
                roots.add(target)
                root_sources[target].add(source.relative_to(ROOT).as_posix())

    reachable = set(roots)
    queue = deque(roots)
    while queue:
        current = queue.popleft()
        for target in edges[current]:
            if target in reachable:
                continue
            reachable.add(target)
            queue.append(target)

    unreachable = [asset for asset in assets if asset not in reachable]
    payload = {
        "scanned_assets": len(assets),
        "rooted_assets": len(roots),
        "reachable_assets": len(reachable),
        "unreachable_candidates": len(unreachable),
        "unreachable_bytes": sum(asset.stat().st_size for asset in unreachable),
        "unused": [
            {
                "asset": asset.relative_to(ROOT).as_posix(),
                "size": asset.stat().st_size,
                "incoming_from_unreachable_assets": sorted(
                    ref.relative_to(ROOT).as_posix() for ref in incoming[asset] if ref not in reachable
                ),
            }
            for asset in unreachable
        ],
    }

    if args.json:
        output = json.dumps(payload, ensure_ascii=False, indent=2)
    else:
        lines = [
            f"Scanned assets: {payload['scanned_assets']}",
            f"Rooted by non-wwwroot source: {payload['rooted_assets']}",
            f"Reachable production assets: {payload['reachable_assets']}",
            f"Unreachable candidates: {payload['unreachable_candidates']}",
            f"Unreachable bytes: {payload['unreachable_bytes']}",
            "",
            "UNREACHABLE CANDIDATES (manual review required):",
        ]
        for item in payload["unused"]:
            lines.append(f"{item['size']:>10}  {item['asset']}")
            for ref in item["incoming_from_unreachable_assets"]:
                lines.append(f"             <- {ref}")
        output = "\n".join(lines)

    print(output)
    if args.report:
        report_path = ROOT / args.report
        report_path.parent.mkdir(parents=True, exist_ok=True)
        report_path.write_text(output + "\n", encoding="utf-8")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
