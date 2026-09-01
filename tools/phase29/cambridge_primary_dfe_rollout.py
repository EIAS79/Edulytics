#!/usr/bin/env python3
from __future__ import annotations

import argparse
import hashlib
import json
import re
import sys
import urllib.request
from collections import OrderedDict
from pathlib import Path

try:
    from pypdf import PdfReader
except ImportError as exc:
    raise SystemExit("pypdf is required: pip install pypdf") from exc

ROOT = Path(__file__).resolve().parents[2]
BP_DIR = ROOT / "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs"
CONTENT_DIR = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs"
AUDIT = ROOT / "docs/PHASE_29_CAMBRIDGE_PRIMARY_2_6_ROLLOUT_AUDIT.json"
PACK_CODE = "CAMBRIDGE-INTL-MATH"
VERSION_CODE = "CAMBRIDGE-PATHWAY-2026"
CHECKED_AT = "2026-09-01T00:00:00Z"
OFFICIAL_URL = "https://www.cambridgeinternational.org/programmes-and-qualifications/cambridge-primary/curriculum/mathematics/"
GOV_PAGE = "https://www.gov.uk/government/publications/teaching-mathematics-in-primary-schools"
OGL_URL = "https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/"

PDFS = {
    2: ("DfE-00111-2020", "https://assets.publishing.service.gov.uk/media/6009a9638fa8f5296a72aad6/Maths_guidance_year_2.pdf"),
    3: ("DfE-00112-2020", "https://assets.publishing.service.gov.uk/media/61409475e90e07043fea1c45/Maths_guidance_year_3.pdf"),
    4: ("DfE-00113-2020", "https://assets.publishing.service.gov.uk/media/6009a9888fa8f5296a72aad7/Maths_guidance_year_4.pdf"),
    5: ("DfE-00114-2020", "https://assets.publishing.service.gov.uk/media/6009a99be90e0747975b4ba8/Maths_guidance_year_5.pdf"),
    6: ("DfE-00115-2020", "https://assets.publishing.service.gov.uk/media/614094a6d3bf7f05afde045f/Maths_guidance_year_6.pdf"),
}

UNIT_META = OrderedDict([
    ("NPV", (1, "Number and Place Value")),
    ("NF", (2, "Number Facts and Fluency")),
    ("AS", (3, "Additive Structures and Relationships")),
    ("MD", (4, "Multiplicative Structures and Relationships")),
    ("F", (5, "Fractions")),
    ("G", (6, "Geometry and Measure")),
])


def sha_bytes(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def sha_text(value: str) -> str:
    return hashlib.sha256(value.encode("utf-8")).hexdigest()


def dump(path: Path, obj) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(json.dumps(obj, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")


def fetch_pdf(year: int) -> tuple[bytes, str, str]:
    ref, url = PDFS[year]
    req = urllib.request.Request(url, headers={"User-Agent": "Edulytics-Phase29/1.0 (+https://edulytiks.com)"})
    with urllib.request.urlopen(req, timeout=60) as response:
        data = response.read()
    if len(data) < 100_000 or not data.startswith(b"%PDF"):
        raise SystemExit(f"FAIL: invalid DfE PDF for year {year}")
    return data, ref, url


def pdf_text(data: bytes, year: int) -> str:
    tmp = ROOT / f".phase29-dfe-y{year}.pdf"
    tmp.write_bytes(data)
    try:
        reader = PdfReader(str(tmp))
        text = "\n".join((page.extract_text() or "") for page in reader.pages)
    finally:
        tmp.unlink(missing_ok=True)
    return text.replace("\u2011", "-").replace("\u2013", "-").replace("\u2212", "-")


def clean(value: str) -> str:
    value = value.replace("\u00ad", "").replace("\u0000", " ")
    value = re.sub(r"\s+", " ", value).strip()
    return value


def strand_from_code(code: str) -> str:
    token = re.sub(r"^\d+", "", code).split("-")[0]
    if token == "AS/MD":
        return "AS"
    if token not in UNIT_META:
        raise SystemExit(f"FAIL: unsupported DfE strand {token} ({code})")
    return token


def extract_criteria(text: str, year: int) -> list[dict[str, str]]:
    marker = f"Year {year} guidance"
    pos = text.find(marker)
    if pos < 0:
        raise SystemExit(f"FAIL: cannot find {marker}")
    segment = text[pos:]
    code_pattern = rf"{year}(?:NPV|NF|AS/MD|AS|MD|F|G)-\d+"
    all_codes = sorted(set(re.findall(code_pattern, segment)))
    if not all_codes:
        raise SystemExit(f"FAIL: no DfE criteria found for year {year}")

    detailed: dict[str, dict[str, str]] = {}
    heading_re = re.compile(rf"(?m)^({code_pattern})\s+([^\n]+)$")
    for match in heading_re.finditer(segment):
        code = match.group(1)
        title = clean(match.group(2))
        after = segment[match.end():]
        guide = None
        for guide_code in (code, code.replace("/", "")):
            candidate = re.search(
                rf"(?m)^{re.escape(guide_code)}\s+Teaching guidance\s*$",
                after,
            )
            if candidate is not None and (guide is None or candidate.start() < guide.start()):
                guide = candidate
        if guide is None or guide.start() > 2500:
            continue
        criterion = clean(after[:guide.start()])
        if len(title) < 4 or len(criterion) < 10:
            continue
        detailed.setdefault(code, {"code": code, "title": title, "criterion": criterion})

    missing = sorted(set(all_codes) - set(detailed))
    if missing:
        raise SystemExit(f"FAIL: detailed DfE criteria missing for year {year}: {missing}")
    if len(detailed) < 5:
        raise SystemExit(f"FAIL: suspiciously small DfE criterion set for year {year}: {len(detailed)}")
    return [detailed[code] for code in sorted(detailed, key=lambda x: (strand_from_code(x), x))]


def numeric_context(year: int, strand: str) -> tuple[str, str, str, str]:
    if strand == "NPV":
        examples = {
            2: ("47 = 4 tens + 7 ones", "Move from 47 to the next multiple of ten, 50, then back to 40."),
            3: ("326 = 3 hundreds + 2 tens + 6 ones", "Place 326 between 300 and 400 and explain why it is closer to 300."),
            4: ("4,507 = 4 thousands + 5 hundreds + 0 tens + 7 ones", "Round 4,507 to a useful power of ten and justify the choice."),
            5: ("5.37 = 5 ones + 3 tenths + 7 hundredths", "Compare 5.37 with 5.4 by using place value rather than digit length."),
            6: ("6,203,405 is composed from millions to ones", "Scale a number by powers of ten and track how each digit's place value changes."),
        }
        ex1, ex2 = examples[year]
        rule = "A digit's value depends on its position. Compose and decompose numbers with place-value units, and use a number line or powers of ten to reason about magnitude."
        mistake = "Do not compare numbers by looking at a single digit or by counting digits after the decimal point. Keep the value of each place explicit and check the size of the whole number."
        return rule, ex1, ex2, mistake
    if strand == "NF":
        return (
            "Fluency means recalling or deriving number facts efficiently while preserving the relationship between addition and subtraction or multiplication and division.",
            "Use a known fact such as 7 + 3 = 10 to derive a related fact without recounting every unit.",
            "Use a multiplication fact and its inverse division fact to check each other.",
            "Do not replace reasoning with repeated counting when a known relationship is available. Check inverse facts and notice commutative relationships only where they are valid.",
        )
    if strand == "AS":
        return (
            "Addition and subtraction describe part-whole, change and difference relationships. Later stages also use these relationships to reason about unknowns, ratio and linked calculations.",
            "Represent a problem with an equation, identify the known and unknown quantities, then choose an efficient calculation strategy.",
            "Check an additive result with the inverse operation or by estimating the expected size of the answer.",
            "Do not choose an operation from a keyword alone. Identify the relationship between quantities, preserve the equality, and verify that the answer fits the original context.",
        )
    if strand == "MD":
        return (
            "Multiplication and division connect equal groups, scaling, factors, multiples and inverse relationships. Efficient methods preserve place value and the meaning of the quantities.",
            "Model equal groups and write a multiplication equation; then write the related division equation and interpret the quotient.",
            "Use factors or place-value scaling to simplify a multiplication or division calculation before checking the result.",
            "Do not confuse the number of groups with the size of each group. Interpret remainders in context and check division with multiplication when appropriate.",
        )
    if strand == "F":
        return (
            "Fractions describe equal parts and numbers on a number line. Equivalent fractions preserve value even when numerator and denominator change together.",
            "Represent a fraction with equal parts, locate it on a number line and explain the role of the numerator and denominator.",
            "Use equivalence or a common denominator only when it helps compare or calculate without changing the value.",
            "Do not add denominators when adding fractions, and do not assume a larger denominator means a larger fraction. Compare the value represented by the whole fraction.",
        )
    return (
        "Geometry uses defined properties, measurement and spatial reasoning. A diagram supports a conclusion only when its stated or measured properties justify that conclusion.",
        "Describe a shape using sides, angles, parallel or perpendicular lines, symmetry, coordinates, length or area as appropriate.",
        "Draw or transform a shape from stated properties, then check each required property rather than relying on appearance.",
        "Do not classify a shape by orientation or visual impression alone. Keep length, area, angle and volume distinct, and use the stated scale or unit.",
    )


def make_body(year: int, code: str, title: str, mode: str) -> dict[str, str]:
    strand = strand_from_code(code)
    rule, ex1, ex2, mistake = numeric_context(year, strand)
    mode_text = (
        "Build the idea by representing it in more than one way and explaining why the representations agree."
        if mode == "concept"
        else "Reason and apply by selecting a representation, solving a contextual problem, and checking the result independently."
    )
    display = f"{title}: {'Build the Idea' if mode == 'concept' else 'Reason and Apply'}"
    explanation = (
        f"This Cambridge Primary Stage {year} supporting lesson develops the open DfE Year {year} ready-to-progress focus “{title}”. "
        f"{rule} {mode_text} The lesson is Edulytics-authored from OGL material; Cambridge remains the academic reference authority and no Cambridge objective wording is reproduced here."
    )
    concepts = f"Source focus: {title}. {rule} {mode_text} Always state what each number, unit, operation or geometric property represents before calculating."
    worked = f"Example A: {ex1} Explain each step and name the mathematical relationship being used. Example B: {ex2} Compare two possible approaches and choose the clearer one."
    steps = (
        "Step 1: Read the problem and identify the quantities or properties. "
        "Step 2: Represent the situation with numbers, a diagram, a number line, an equation or a labelled shape. "
        "Step 3: Apply the relevant rule while preserving place value, units and relationships. "
        "Step 4: Calculate carefully and write the result with its meaning. "
        "Step 5: Check using an inverse operation, estimation, an alternative representation or the stated geometric properties."
    )
    mistakes = f"{mistake} A correct-looking number is not enough if the representation or reasoning does not match the original relationship. Re-read the question and check units and magnitude."
    summary = f"{title}: represent the idea clearly, apply the correct relationship, calculate accurately, and verify the result using a second line of reasoning."
    return {
        "CultureCode": "en",
        "Title": display,
        "Explanation": explanation,
        "KeyConceptsAndRules": concepts,
        "WorkedExamples": worked,
        "StepByStepSolutions": steps,
        "CommonMistakes": mistakes,
        "QuickSummary": summary,
    }


def build_stage(year: int) -> tuple[dict, dict, dict]:
    data, ref, pdf_url = fetch_pdf(year)
    source_sha = sha_bytes(data)
    criteria = extract_criteria(pdf_text(data, year), year)
    lessons = []
    unit_lessons: dict[str, list[dict]] = OrderedDict()
    sort_order = 0
    for criterion in criteria:
        strand = strand_from_code(criterion["code"])
        for mode in ("concept", "application"):
            sort_order += 1
            code_token = criterion["code"].replace("/", "-")
            mode_token = "BUILD" if mode == "concept" else "APPLY"
            lesson_code = f"PED:CAMBRIDGE-INTL-MATH:S{year}:{code_token}:{mode_token}"
            source_code = f"DFE-Y{year}:{code_token}:{mode_token}"
            body = make_body(year, criterion["code"], criterion["title"], mode)
            lesson = {
                "SourceLessonCode": source_code,
                "LessonCode": lesson_code,
                "UnitNumber": UNIT_META[strand][0],
                "UnitTitle": UNIT_META[strand][1],
                "LessonNumber": len(unit_lessons.setdefault(strand, [])) + 1,
                "Title": body["Title"],
                "SortOrder": sort_order,
                "SourceUrl": pdf_url,
                "SemanticSha256": sha_text(f"{criterion['code']}|{criterion['title']}|{mode}|{source_sha}"),
                "Alignments": [],
                "OutcomeCodes": [],
                "ApplicableCourses": [f"CAMBRIDGE-PRIMARY-S{year}"],
                "FormalTargets": [],
            }
            unit_lessons[strand].append(lesson)
            lessons.append((lesson, body, criterion))

    units = []
    for strand, rows in unit_lessons.items():
        number, title = UNIT_META[strand]
        units.append({
            "Number": number,
            "UnitCode": f"S{year}-U{number:02d}-{strand}",
            "SortOrder": number,
            "Title": title,
            "LessonCount": len(rows),
            "SourceUrl": pdf_url,
            "SemanticSha256": sha_text("|".join(x["LessonCode"] for x in rows)),
        })

    graph_sha = sha_text(json.dumps({"year": year, "sourceSha": source_sha, "criteria": criteria}, ensure_ascii=False, sort_keys=True))
    blueprint = {
        "SchemaVersion": 1,
        "BlueprintCode": f"CAMBRIDGE-PRIMARY-S{year}:DFE-OGL-V1",
        "PackCode": PACK_CODE,
        "VersionCode": VERSION_CODE,
        "LogicalLevel": year,
        "NativeLevel": f"Cambridge Primary Stage {year}",
        "Pathway": None,
        "OfficialAuthority": "Cambridge International Education",
        "OfficialSourceUrl": OFFICIAL_URL,
        "PedagogicalSourceType": "OpenEducationalResource",
        "SourceTitle": f"Mathematics guidance: year {year}",
        "SourcePublisher": "UK Department for Education",
        "SourceEdition": f"{ref} / June 2020",
        "SourceRootUrl": pdf_url,
        "SourceCheckedAtUtc": CHECKED_AT,
        "SourceLicense": "Open Government Licence v3.0",
        "RequiredDigitalAttribution": "Contains public sector information licensed under the Open Government Licence v3.0.",
        "SourceSelectionReason": "Cambridge official material remains reference-only. DfE primary Mathematics guidance supplies an open, structured pedagogical sequence that Edulytics can lawfully adapt for commercial use.",
        "SourceSelectionEvidence": f"The official DfE Year {year} PDF was retrieved and parsed. Every ready-to-progress criterion found in the Year {year} guidance is represented by an Edulytics concept lesson and an application lesson. These lessons are supporting lessons: no Cambridge objective mapping is claimed without explicit evidence.",
        "SourceEvidenceUrls": [GOV_PAGE, pdf_url, OGL_URL],
        "SourceRightsNote": "Contains public sector information licensed under the Open Government Licence v3.0. No government logos or identified third-party copyright material is reproduced. Cambridge objective prose is not reproduced.",
        "SemanticGraphSha256": graph_sha,
        "AcquisitionDiagnostics": {
            "UnitCount": len(units),
            "LessonCount": len(lessons),
            "OfficialStandardCount": 0,
            "AddressingCoverageCount": 0,
            "FormalMappingCount": 0,
            "LessonsWithoutNumberedGradeReferenceAnyRole": len(lessons),
            "LessonsWithoutNumberedAddressingStandard": len(lessons),
            "LessonsWithoutNumberedAddressingOrBuildingTowardsStandard": len(lessons),
            "MultiStandardLessons": 0,
        },
        "Units": units,
        "Lessons": [x[0] for x in lessons],
    }

    canonical_lessons = []
    for lesson, body, criterion in lessons:
        body_hash = sha_text(json.dumps(body, ensure_ascii=False, sort_keys=True))
        canonical_lessons.append({
            "LessonCode": lesson["LessonCode"],
            "TitleProvenance": "PedagogicalSource",
            "TitleSourceReference": f"DfE Year {year} ready-to-progress criterion {criterion['code']}; Edulytics supporting sequence; no Cambridge outcome mapping claimed.",
            "OutcomeCodes": [],
            "IsSupporting": False,
            "SourceUrl": pdf_url,
            "SourceLocator": criterion["code"],
            "SourceTitle": f"Mathematics guidance: year {year}",
            "SourcePublisher": "UK Department for Education",
            "SourceEdition": f"{ref} / June 2020",
            "SourceRights": "Open Government Licence v3.0. Edulytics learner-facing body is an original adaptation; no government logos or identified third-party material is reused.",
            "SourceSha256": source_sha,
            "CanonicalBodySha256": body_hash,
            "SourceVerifiedAtUtc": CHECKED_AT,
            "RetrievalUrl": pdf_url,
            "RetrievalChannel": "HTTPS",
            "RetrievalTimestamp": CHECKED_AT,
            "AdaptationStatus": "Edulytics-authored curriculum lesson informed by OGL DfE guidance for the corresponding year. Cambridge official identifiers remain in the curriculum reference graph; this lesson does not claim an unverified formal Cambridge mapping.",
            "Translations": [body],
        })

    content = {
        "PackCode": PACK_CODE,
        "VersionCode": VERSION_CODE,
        "ContentVersion": f"phase29-cambridge-primary-stage{year}-dfe-ogl-v1",
        "AcademicLanguage": "en",
        "CurriculumTranslationRequired": False,
        "TargetCurriculumPeriod": "0096 Curriculum Framework v2.1 / June 2025",
        "SourceCurriculumPeriod": "0096 Curriculum Framework v2.1 / June 2025",
        "SourceVersionLabel": "Cambridge Primary Mathematics 0096 Framework v2.1 / June 2025",
        "SourceAuthority": "Cambridge International Education",
        "SourceUrl": OFFICIAL_URL,
        "SourceCheckedAtUtc": CHECKED_AT,
        "SourceResolution": "CurrentOfficial",
        "FallbackReason": "",
        "ReviewMethod": "DfE OGL source extraction, ready-to-progress criterion completeness check, deterministic two-lesson-per-criterion Edulytics authoring, canonical body hashing, and explicit no-unverified-Cambridge-mapping rule.",
        "SourcePolicyVersion": 2,
        "PedagogicalSourceType": "OpenEducationalResource",
        "PedagogicalSourceTitle": f"Mathematics guidance: year {year}",
        "PedagogicalSourcePublisher": "UK Department for Education",
        "PedagogicalSourceEdition": f"{ref} / June 2020",
        "PedagogicalSourceUrl": pdf_url,
        "PedagogicalSourceCheckedAtUtc": CHECKED_AT,
        "PedagogicalSourceSelectionReason": "DfE primary Mathematics guidance supplies lawful, structured pedagogy under OGL v3.0 while Cambridge remains the reference-only academic authority.",
        "PedagogicalSourceSelectionEvidence": f"All {len(criteria)} distinct Year {year} ready-to-progress criteria extracted from the official DfE guidance are represented by two Edulytics lessons, for {len(canonical_lessons)} source-backed supporting lessons.",
        "PedagogicalSourceRightsNote": "Contains public sector information licensed under the Open Government Licence v3.0. No government logos or identified third-party copyright material is reproduced.",
        "Status": "Published",
        "ReviewedBy": "Edulytics Phase 29 deterministic Cambridge Primary source-policy review",
        "ReviewEvidence": f"Stage {year}: {len(criteria)} DfE ready-to-progress criteria, {len(canonical_lessons)} supporting lessons, English academic bodies, zero unverified Cambridge formal mappings, source SHA256 {source_sha}.",
        "Lessons": canonical_lessons,
    }

    stat = {
        "stage": year,
        "criterionCount": len(criteria),
        "lessonCount": len(canonical_lessons),
        "sourceSha256": source_sha,
        "blueprint": f"cambridge-primary-stage{year}-dfe-ogl-v1.lesson-blueprint.json",
        "content": f"cambridge-primary-stage{year}-dfe-ogl-v1.lesson-content-pack.json",
    }
    return blueprint, content, stat


def generate_stage(year: int) -> dict:
    blueprint, content, stat = build_stage(year)
    bp_path = BP_DIR / stat["blueprint"]
    content_path = CONTENT_DIR / stat["content"]
    dump(bp_path, blueprint)
    dump(content_path, content)
    return stat


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--stages", default="2,3,4,5,6")
    parser.add_argument("--write-audit", action="store_true")
    args = parser.parse_args()
    stages = [int(x) for x in args.stages.split(",") if x.strip()]
    if any(x not in PDFS for x in stages):
        raise SystemExit("Only stages 2-6 are supported by this rollout generator.")
    stats = [generate_stage(year) for year in stages]
    audit = {
        "schemaVersion": 1,
        "packCode": PACK_CODE,
        "sourcePolicy": "DfE OGL supporting pedagogy; Cambridge remains reference-only authority; no unverified formal outcome mapping.",
        "stages": stats,
        "totalCriteria": sum(x["criterionCount"] for x in stats),
        "totalLessons": sum(x["lessonCount"] for x in stats),
        "status": "RepositoryCandidateGenerated",
    }
    if args.write_audit:
        dump(AUDIT, audit)
    print(json.dumps(audit, ensure_ascii=False, indent=2))
    return 0


if __name__ == "__main__":
    sys.exit(main())
