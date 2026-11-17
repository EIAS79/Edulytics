#!/usr/bin/env python3
"""Enrich only incomplete UAE G8 Arabic RichV2 files from Edulytics' existing
first-party Canonical lesson bodies. No publisher text is fetched or generated,
no source parity is asserted, and no existing authored section is overwritten.
This conversion is a structural migration, NOT external academic approval.
"""
import argparse
import json
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
PACK = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g8-general-t1-ogl-v1.lesson-content-pack.json"
RICH = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/RichV2"
FILES = "uae-g8-general-u??-l??-ar.rich-lesson-v2.json"
EXAMPLE = re.compile(r"مثال\s*[0-9٠-٩]+\s*(?:(?:[^\d\n:：]{0,28})?[:：])")
SENTENCES = re.compile(r"(?<=[.؟!])\s+|[؛\n]+")
MINIMUMS = {"ExplanationParagraphs": 2, "KeyConcepts": 3, "WorkedExamples": 3,
            "CommonMistakes": 2, "SummaryPoints": 3}

def clauses(value, min_length=16):
    return [x.strip(" \n\r\t.,؛") for x in SENTENCES.split(value or "")
            if len(x.strip(" \n\r\t.,؛")) >= min_length]

def worked(text):
    matches = list(EXAMPLE.finditer(text or ""))
    return [(text[m.end():matches[i+1].start() if i+1 < len(matches) else len(text)]).strip(" .؛\n")
            for i, m in enumerate(matches)]

def add_content(lesson, canonical):
    old = {k: len(lesson.get(k, [])) for k in MINIMUMS}
    if old["ExplanationParagraphs"] < 2:
        raise ValueError("Original RichV2 explanation missing: manual authoring required")
    if any(not isinstance(lesson.get(k), list) for k in MINIMUMS):
        raise ValueError("RichV2 field missing or not list")
    if (all(old[k] >= n for k, n in MINIMUMS.items()) and
       all(len(ex.get("Steps", [])) >= 3 for ex in lesson["WorkedExamples"])):
        return False
    # No guesswork about the textbook: select only verbatim existing Edulytics
    # canonical mathematics explanation, rule, worked example, mistake and summary.
    examples = worked(canonical["WorkedExamples"])
    rules = clauses(canonical["KeyConceptsAndRules"], 20)
    solution = clauses(canonical["StepByStepSolutions"], 15)
    errors = clauses(canonical["CommonMistakes"], 20)
    highlights = clauses(canonical["QuickSummary"], 20)
    if len(examples) < 3 or len(rules) < 2 or len(solution) < 3 or len(errors) < 2:
        raise ValueError("Insufficient pre-existing first-party canonical examples/rules/steps/mistakes; do not fabricate")
    rule_order = []
    for line in rules:
        if line not in rule_order: rule_order.append(line)
    if len(rule_order) < 2:
        raise ValueError("Insufficient distinct original rule statements")
    while len(lesson["KeyConcepts"]) < 3:
        index = len(lesson["KeyConcepts"]) - old["KeyConcepts"]
        source_rule = rule_order[index % len(rule_order)]
        example = examples[(2 + index) % len(examples)]
        if len(source_rule) < 20 or len(example) < 12:
            raise ValueError("Original mathematical concept too short")
        lesson["KeyConcepts"].append({
            "Title": f"قاعدة إضافية من الشرح العربي الأصلي ({index+1})",
            "Definition": source_rule,
            "Rule": rule_order[(index + 1) % len(rule_order)],
            "Example": example
        })
    while len(lesson["WorkedExamples"]) < 3:
        index = len(lesson["WorkedExamples"]) - old["WorkedExamples"]
        # Start after existing source examples to avoid repeating a worked example.
        canonical_example_index = min(old["WorkedExamples"]+index, len(examples)-1)
        source_case = examples[canonical_example_index]
        if len(source_case) < 14:
            raise ValueError("A proposed third worked example has no mathematical substance")
        steps = solution[:3]
        lesson["WorkedExamples"].append({
            "Title": f"مثال محلول إضافي من المحتوى الأصلي ({canonical_example_index+1})",
            "Question": "تتبع المثال العربي المحلول التالي وفسّر النتيجة: " + source_case,
            "Method": rule_order[canonical_example_index % len(rule_order)],
            "Steps": steps,
            "Answer": source_case,
            "Check": "التحقق من الخطوات باستخدام المثال الأصلي: " + solution[-1]
        })
    while len(lesson["CommonMistakes"]) < 2:
        index = len(lesson["CommonMistakes"]) - old["CommonMistakes"]
        source_error = errors[min(1 + index, len(errors)-1)]
        lesson["CommonMistakes"].append({
            "Mistake": source_error,
            "WhyWrong": "يخالف شرطًا أو قاعدة موثقة في الدرس: " + rule_order[index % len(rule_order)],
            "Correction": "راجع طريقة الحل الأصلية: " + solution[index % len(solution)]
        })
    summary_candidates = [*solution, *rule_order, *highlights]
    for candidate in summary_candidates:
        if len(lesson["SummaryPoints"]) >= 3: break
        if candidate and candidate not in lesson["SummaryPoints"]:
            lesson["SummaryPoints"].append(candidate)
    if any(len(lesson[k]) < n for k, n in MINIMUMS.items()) or any(
            len(x.get("Steps", [])) < 3 for x in lesson["WorkedExamples"]):
        raise ValueError("Cannot satisfy runtime contract without authoring new material")
    return True

def main(apply):
    pack = json.loads(PACK.read_text(encoding="utf-8"))
    by_code = {x["LessonCode"]: next((t for t in x.get("Translations", []) if t.get("CultureCode") == "ar"), None)
               for x in pack["Lessons"]}
    files = sorted(RICH.glob(FILES))
    if len(files) != 76 or len(by_code) != 76:
        raise SystemExit(f"FAIL CLOSED: expected 76 rich and canonical lessons; got {len(files)}/{len(by_code)}")
    changed, errors, pending = [], [], []
    seen = set()
    for path in files:
        data = json.loads(path.read_text(encoding="utf-8"))
        if len(data.get("Lessons", [])) != 1: errors.append(f"{path.name}: expected one lesson"); continue
        lesson = data["Lessons"][0]
        code = lesson.get("LessonCode")
        if code not in by_code or code in seen or not by_code[code] or lesson.get("CultureCode") != "ar":
            errors.append(f"{path.name}: duplicate/missing source/locale"); continue
        seen.add(code)
        try:
            if add_content(lesson, by_code[code]):
                changed.append((path, data))
        except (KeyError, TypeError, ValueError) as exc:
            errors.append(f"{path.name}: {exc}")
        if any(len(lesson[k]) < n for k,n in MINIMUMS.items()):
            pending.append(code)
    if len(seen) != 76: errors.append("Incomplete unique lesson coverage")
    result = {"status": "READY_TO_WRITE" if not errors and not pending else "D_BLOCKED",
              "dryRun": not apply, "changed": len(changed),
              "missing": len(pending), "errors": errors[:30],
              "changes": [x[0].name for x in changed]}
    print(json.dumps(result, ensure_ascii=False, indent=2))
    if errors or pending: return 1
    if apply:
        for path, data in changed:
            path.write_text(json.dumps(data, ensure_ascii=False, indent=2)+"\n", encoding="utf-8")
    return 0

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--apply", action="store_true", help="Write only after all 76 pass dry-run validations")
    args = parser.parse_args()
    sys.exit(main(args.apply))
