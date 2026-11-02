#!/usr/bin/env python3
import json
import sys
from pathlib import Path


def fail(message: str) -> None:
    print(f"FAIL: {message}")
    raise SystemExit(1)


def finding_key(result: dict) -> str:
    rule_id = result.get("ruleId", "")
    fingerprints = result.get("partialFingerprints", {}) or {}
    line_hash = fingerprints.get("primaryLocationLineHash", "")
    column_fp = fingerprints.get("primaryLocationStartColumnFingerprint", "")
    if line_hash:
        return f"{rule_id}|{line_hash}|{column_fp}"

    locations = result.get("locations", []) or []
    if locations:
        physical = locations[0].get("physicalLocation", {}) or {}
        artifact = physical.get("artifactLocation", {}) or {}
        region = physical.get("region", {}) or {}
        return (
            f"{rule_id}|{artifact.get('uri', '')}|"
            f"{region.get('startLine', '')}|{region.get('startColumn', '')}"
        )

    return f"{rule_id}|unlocated"


def main() -> None:
    if len(sys.argv) != 3:
        fail("usage: ci-codeql-severity-gate.py <results.sarif> <baseline.json>")

    sarif_path = Path(sys.argv[1])
    baseline_path = Path(sys.argv[2])
    if not sarif_path.is_file():
        fail(f"SARIF file not found: {sarif_path}")
    if not baseline_path.is_file():
        fail(f"baseline file not found: {baseline_path}")

    document = json.loads(sarif_path.read_text(encoding="utf-8"))
    baseline = json.loads(baseline_path.read_text(encoding="utf-8"))
    known = {item["key"] for item in baseline.get("findings", []) if item.get("key")}

    high_or_critical = []
    all_results = 0
    for run in document.get("runs", []):
        rules = {
            rule.get("id"): rule
            for rule in (run.get("tool", {}).get("driver", {}).get("rules", []) or [])
        }
        for result in run.get("results", []) or []:
            all_results += 1
            rule = rules.get(result.get("ruleId"), {})
            raw = (rule.get("properties", {}) or {}).get("security-severity")
            try:
                severity = float(raw)
            except (TypeError, ValueError):
                severity = 0.0
            if severity < 7.0:
                continue
            high_or_critical.append(
                {
                    "key": finding_key(result),
                    "ruleId": result.get("ruleId", "unknown"),
                    "securitySeverity": severity,
                }
            )

    new_findings = [item for item in high_or_critical if item["key"] not in known]
    print(
        "CodeQL severity gate: "
        f"results={all_results} high_or_critical={len(high_or_critical)} "
        f"baseline={len(known)} new_high_or_critical={len(new_findings)}"
    )

    if new_findings:
        for item in new_findings:
            level = "CRITICAL" if item["securitySeverity"] >= 9.0 else "HIGH"
            print(
                f"NEW {level}: {item['ruleId']} "
                f"security-severity={item['securitySeverity']} key={item['key']}"
            )
        fail("new CodeQL HIGH/CRITICAL findings are not allowed")

    print("PASS: no new CodeQL HIGH/CRITICAL findings relative to the committed main baseline.")


if __name__ == "__main__":
    main()
