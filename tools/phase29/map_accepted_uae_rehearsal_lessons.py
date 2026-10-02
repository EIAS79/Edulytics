"""Apply reviewed mappings to existing UAE codes without reauthoring lesson bodies.

Code identities/applicability come from the accepted UAE pack. Subject meanings
were checked against MoE's Strands and Standards Summary (11 October 2021),
archived at https://www.scribd.com/document/826281797/Strands-and-Standards-Summary-MAT.
This is mapping evidence, not a replacement curriculum or an official grade scope.
No new official code or applicability range is created here.
"""
import json
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
CURRICULUM = ROOT / "src/Edulytics.Core/Curriculum"
EVIDENCE = "https://www.scribd.com/document/826281797/Strands-and-Standards-Summary-MAT"

# Exact lesson titles and accepted standard codes; uncovered topics stay Supporting.
PRIMARY = {
    3: {
        "Place value to thousands": "1.02.04",
        "Addition with regrouping": "1.03.04",
        "Subtraction with regrouping": "1.03.04",
        "Multiplication facts": "1.04.02",
        "Division facts": "1.04.02",
        "Fractions on a number line": "1.05.02",
        "Equivalent fractions": "1.05.02",
        "Compare fractions": "1.05.02",
        "Add and subtract related fractions": "1.05.04",
        "Perimeter": "3.02.10",
        "Area by counting squares": "3.02.10",
        "Time and timetables": "3.02.05",
        "Bar charts": "4.02.02",
        "Tables and scales": "4.02.02",
        "Interpreting data": "4.02.02",
    },
    4: {
        "Place value to large whole numbers": "1.02.05",
        "Rounding to powers of ten": "1.02.05",
        "Mental calculation strategies": "1.03.05",
        "Formal addition": "1.03.05",
        "Formal subtraction": "1.03.05",
        "Factors and multiples": "1.04.03",
        "Equivalent fractions": "1.05.02",
        "Add and subtract fractions": "1.05.04",
        "Tenths and hundredths": "1.06.01",
        "Compare decimals": "1.06.01",
        "Decimal rounding": "1.06.01",
        "Perimeter of rectilinear shapes": "3.02.10",
        "Area of rectangles": "3.02.10",
        "Unit conversion": "3.02.08",
        "Time problems": "3.02.05",
        "Angle classification": "3.07.03",
        "Triangles": "3.08.03",
        "Coordinates in the first quadrant": "3.03.01",
        "Bar charts with scales": "4.02.02",
        "Interpreting tables": "4.02.02",
    },
}
LOWER = {
    "Percentages": "1.07.02", "Ratio and proportion": "1.07.03",
    "Rates and unit rates": "1.07.01", "Algebraic expressions": "2.02.03",
    "Substitution": "2.02.03", "Expanding brackets": "2.02.02",
    "Factorising expressions": "2.02.02", "Linear equations": "2.02.04",
    "Inequalities": "2.02.05", "Coordinates and straight-line graphs": "2.02.08",
    "Congruence and similarity": "3.05.02", "Transformations": "3.05.01",
    "Perimeter and area": "3.02.10", "Scatter graphs": "4.02.05",
}
UPPER = {
    "Sequences": "2.05.02",
    "Quadratic equations": "5.02.01", "Exponential models": "5.03.01",
}


def apply():
    nodes = {n["Code"]: n for n in json.loads((CURRICULUM / "Packs/uae-moe-math.curriculum-pack.json").read_text())["Nodes"]}
    for level in (3, 4, 7, 8, 11, 12):
        pathway = "common" if level < 5 else "advanced"
        stem = f"uae-g{level}-{pathway}-t1-ogl-v1"
        bp_path = CURRICULUM / f"LessonBlueprints/Packs/{stem}.lesson-blueprint.json"
        cp_path = CURRICULUM / f"LessonContent/Packs/{stem}.lesson-content-pack.json"
        bp, cp = json.loads(bp_path.read_text()), json.loads(cp_path.read_text())
        mappings = dict(PRIMARY[level] if level < 5 else LOWER if level < 9 else UPPER)
        if level == 8:
            mappings.update({"Simultaneous equations": "2.02.10", "Linear modelling": "2.02.19", "Pythagoras theorem": "3.08.02"})
        if level == 12:
            mappings.pop("Quadratic equations")
            mappings.update({"Logarithms": "5.03.01", "Sequences and series": "2.05.01", "Introductory differentiation": "6.02.01", "Rates of change": "6.02.01"})
        content_by_code = {lesson["LessonCode"]: lesson for lesson in cp["Lessons"]}
        seen = set()
        for lesson in bp["Lessons"]:
            title = lesson["Title"].removesuffix(" — advanced reasoning")
            standard = mappings.get(title)
            codes = [f"UAE:STD:MAT.{standard}"] if standard else []
            for code in codes:
                node = nodes[code]
                assert node["IsOfficial"] and node["IsActive"] and node["Kind"] == "Standard"
                assert node["LogicalLevelFrom"] <= level <= node["LogicalLevelTo"]
                assert not node.get("Pathway") or node["Pathway"].lower() == pathway
                seen.add(title)
            lesson["OutcomeCodes"] = codes
            lesson["Alignments"] = [dict(Role="Addressing", ReferenceCode=code, ReferenceKind="NumberedStandard", ResolutionKind="ExactAcceptedStandard", OutcomeCode=code, SortOrder=i + 1) for i, code in enumerate(codes)]
            content = content_by_code[lesson["LessonCode"]]
            content["OutcomeCodes"] = codes
            content["IsSupporting"] = not codes
        assert seen == set(mappings), (level, set(mappings) - seen)
        mapped = sum(bool(x["OutcomeCodes"]) for x in bp["Lessons"])
        standards = len({c for x in bp["Lessons"] for c in x["OutcomeCodes"]})
        supporting = len(bp["Lessons"]) - mapped
        bp["SourceSelectionEvidence"] = f"Reviewed direct mappings to {standards} existing accepted UAE MoE numbered standards cover {mapped} lessons. The remaining {supporting} lessons are Supporting. Official code identities and grade/pathway scopes are unchanged; standard meanings were checked against the MoE Strands and Standards Summary."
        if EVIDENCE not in bp["SourceEvidenceUrls"]:
            bp["SourceEvidenceUrls"].append(EVIDENCE)
        d = bp["AcquisitionDiagnostics"]
        d.update(OfficialStandardCount=standards, AddressingCoverageCount=standards, FormalMappingCount=mapped, LessonsWithoutNumberedGradeReferenceAnyRole=supporting, LessonsWithoutNumberedAddressingStandard=supporting, LessonsWithoutNumberedAddressingOrBuildingTowardsStandard=supporting, MultiStandardLessons=0)
        cp["ContentVersion"] = f"p29-{stem}-official-map-v1"
        cp["TargetCurriculumPeriod"] = cp["SourceCurriculumPeriod"] = cp["SourceVersionLabel"] = "2025-2026 Term 1"
        review = "Direct mappings reviewed against existing accepted UAE standard meanings and grade/pathway applicability; lesson bodies preserved."
        if review not in cp["ReviewMethod"]:
            cp["ReviewMethod"] += " " + review
        cp["PedagogicalSourceSelectionEvidence"] = bp["SourceSelectionEvidence"]
        cp["ReviewEvidence"] = f"UAE G{level}/{pathway}: {mapped} officially mapped lessons, {supporting} Supporting; evidence {EVIDENCE}; no synthetic references and no learner-facing body changes."
        for path, document in ((bp_path, bp), (cp_path, cp)):
            path.write_text(json.dumps(document, ensure_ascii=False, indent=2) + "\n")
        print(f"G{level}: {mapped}/{len(bp['Lessons'])} mapped, {standards} standards, {supporting} Supporting")


if __name__ == "__main__":
    apply()
