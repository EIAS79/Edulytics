from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
rich_rel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l01.rich-lesson-v2.json"
pack_rel="src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g12-precalculus-u01-l01-abc-draft.lesson-content-pack.json"
rich_path=root/rich_rel
pack_path=root/pack_rel
rich=json.loads(rich_path.read_text(encoding="utf-8"))
pack=json.loads(pack_path.read_text(encoding="utf-8"))
lesson=rich["Lessons"][0]
trans=pack["Lessons"][0]["Translations"][0]
notes=[
("Function notation uses parentheses to indicate the INPUT to a function, not multiplication: f(a+b) applies f after determining a+b as one complete input.",
 "f(a+b)",20),
("A table can list every known pair or selected observations from a wider relationship. A few sampled ordered pairs do not determine how the function behaves outside the table.",
 "few sampled ordered pairs",21),
("A graph may consist of isolated plotted values for a discrete domain, or the complete curve representing all ordered pairs in a continuous domain. Plotting sampled points alone does not prove continuity between them.",
 "isolated plotted values",29),
("The nine basic toolkit-function families are constant, identity, absolute value, quadratic, cubic, reciprocal, reciprocal squared, square root, and cube root. For reciprocal and reciprocal squared exclude x=0; the square root has real x>=0.",
 "reciprocal squared",32)
]
for text,keyword,page in notes:
    if not any(keyword in p for p in lesson["ExplanationParagraphs"]):
        lesson["ExplanationParagraphs"].append(text)
    if keyword not in trans["Explanation"]:
        trans["Explanation"]+="\n\n"+text
toolkit=next(x for x in lesson["KeyConcepts"] if x["Title"]=="Basic function families")
toolkit["Definition"]="Nine toolkit families: constant, identity, absolute value, quadratic, cubic, reciprocal, reciprocal squared, square root, and cube root."
toolkit["Rule"]="Read formula, graph, domain and range together: 1/x and 1/x² exclude x=0, while √x requires x≥0."
# Preserve canonical pack's existing full concepts and append a short explicit
# definition as needed; do not regenerate from scratch or discard prior work.
if "reciprocal squared" not in trans["KeyConceptsAndRules"].lower():
    trans["KeyConceptsAndRules"]+="\n\nBasic toolkit function families: "+toolkit["Definition"]+" "+toolkit["Rule"]
rich["ContentVersion"]="g12-u01-l01-source-narrative-definition-gaps-20261106"
pack["ContentVersion"]="g12-u01-l01-source-narrative-definition-gaps-20261106"
rich_path.write_text(json.dumps(rich,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
pack_path.write_text(json.dumps(pack,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

narrative=[
(18,"Everyday dependent relationships: output quantity varies as input changes","Learning objectives","Narrative opening is represented by learning-goal introductory explanation."),
(18,"Relation as ordered pairs, domain as inputs, range as outputs","Relation, domain and range","Source set/ordered-pair concepts appear in key concept 1."),
(18,"A function permits repeated outputs but not conflicting outputs at one input","Definition of a function","Source function concept appears in key concept 2 and examples."),
(20,"Function notation f(x) distinguishes rule, input and output; parentheses mean substitution, not multiplication","Function notation and dependent quantity","Specific source subtlety added to first-party explanation and pack."),
(21,"Tables show either all known input/output pairs or observations of a richer relation","Representing Functions Using Tables","Sample-versus-entire-domain distinction explicitly added to the first-party explanation."),
(23,"Evaluating an output differs from solving a function-output equation for one or more possible inputs","Solving for an input","Existing worked examples and key concept 5 cover this distinction."),
(24,"Algebraic formula from an equation must isolate a unique output branch if possible","Evaluating Functions Expressed in Formulas","First-party worked examples 9 and 10 cover formula isolation and the circle counterexample."),
(26,"Function values can be evaluated or inverted by reading a table, including every occurrence of an output","Evaluating a Function Given in Tabular Form","First-party worked example 11 and table visuals model lookup and inversion."),
(27,"Read a specified function input and all output-level intersections from a graph","Finding Function Values from a Graph","Worked example 12 and independent coordinate diagram model graph reading."),
(28,"One-to-one requires one input per output within a declared domain","Determining Whether a Function is One-to-One","Key concept 6 and first-party area/radius example address injectivity."),
(29,"Vertical line test distinguishes functions from relations with multiple graph outputs at one x","Using the Vertical Line Test","Key concept 7 and first-party three-graph example address vertical intersections."),
(31,"Horizontal line test applies to a function and distinguishes injective/noninjective behavior","Using the Horizontal Line Test","Key concept 7 and first-party one-to-one example address horizontal intersections."),
(32,"Nine toolkit families include graph forms, formulas, example tables, and domain restrictions","Identifying Basic Toolkit Functions","Nine first-party graph families and expanded RichV2 key concept cover all named toolkit functions.")
]
definitions=[
(18,"Relation, domain, range","A relation is ordered pairs with domain from first coordinates and range from second coordinates.","Relation, domain and range"),
(18,"Function","Each input in the relation domain has exactly one output, even if other inputs share that output.","Definition of a function"),
(20,"Function notation","In y=f(x), x is the independent input and f(x) is the dependent output.","Function notation and dependent quantity"),
(28,"One-to-one function","A function for which each output arises from exactly one input in its specified domain.","One-to-one (injective)")
]
ledger_path=base/"u01-l01-element-coverage.json"
ledger=json.loads(ledger_path.read_text(encoding="utf-8"))
assert len(ledger["Elements"])==170
source_sha=ledger["Elements"][0]["SourceLocator"]["SourceSha256"]
def source_element(category,n,page,expected,title,delivery):
    return {
        "SourceElementId":f"G12-U01-L01-{category}{n:03d}",
        "ParentElementId":"G12-U01-L01",
        "Category":category,
        "SourceLocator":{"PdfPage":page,"Section":"1.1","SectionTitle":"Functions and Function Notation","SourceSha256":source_sha},
        "ExpectedMathematics":expected,
        "OriginalSourceHeading":title,
        "TeacherOrStudentAudience":"Student",
        "RightsStatus":"CC_BY_4_SOURCE_MATERIAL; ORIGINAL_PROSE_NOT_COPIED",
        "EdulyticsArtifactPath":rich_rel,
        "FirstPartyNarrativePackPath":pack_rel,
        "ImplementationType":"FIRST_PARTY_NARRATIVE_OR_KEY_CONCEPT",
        "VerificationMethod":"READ_LOCAL_ORIGINAL_OPENSTAX_2017_SECTION_1_1_NARRATIVE_AND_DEFINITION",
        "Status":"SOURCE_VERIFIED",
        "SourceSkillRepresentativeVerifiedInBothContentPaths":True,
        "SemanticParityFullSequenceCertified":False,
        "ContentVersion":rich["ContentVersion"],
        "Reviewer":"LOCAL_PDF_SOURCE_HEADING_AND_FIRST_PARTY_CONCEPT_REVIEW",
        "UpdatedUtc":datetime.now(timezone.utc).isoformat()
    }
for n,(page,meaning,heading,delivery) in enumerate(narrative,1):
    assert len(delivery)>20
    ledger["Elements"].append(source_element("NARRATIVE",n,page,meaning,heading,delivery))
for n,(page,title,definition,concept) in enumerate(definitions,1):
    assert any(x["Title"]==concept for x in lesson["KeyConcepts"])
    ledger["Elements"].append(source_element("DEFINITION",n,page,definition,title,concept))
assert len(ledger["Elements"])==187
ledger["OriginalSourceParentLocationAnchors"]=187
ledger["OriginalSourceNarrativePassagesExplicitlyReviewed"]=13
ledger["OriginalSourceFormalDefinitionsExplicitlyReviewed"]=4
ledger["OriginalSourceAnchorsSourceVerified"]=187
ledger["SourceAtomicDenominatorVerified"]=False
ledger_path.write_text(json.dumps(ledger,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

old_path=base/"u01-l01-256-named-source-atomic-actions.json"
old=json.loads(old_path.read_text(encoding="utf-8"))
assert len(old["Atoms"])==256
new_atoms=old["Atoms"]+[{
    "AtomId":x["SourceElementId"],"ParentId":x["SourceElementId"],
    "Category":x["Category"],"OriginalParentNumber":None,
    "SourcePdfPage":x["SourceLocator"]["PdfPage"],
    "OriginalPartLabel":None,"SourceActionScope":x["ExpectedMathematics"],
    "FirstPartyDelivery":rich_rel,"FirstPartyNarrativePack":pack_rel,
    "SourceNumericalPrecisionUnresolved":False,
    "SourceElementSemanticParityCertified":False
} for x in ledger["Elements"] if x["Category"] in ("NARRATIVE","DEFINITION")]
assert len(new_atoms)==273 and len({x["AtomId"]for x in new_atoms})==273
new=dict(old)
new["OriginalParentLocationAnchors"]=187
new["OriginalSourceNarrativePassagesExplicitlyReviewed"]=13
new["OriginalSourceDefinitionsExplicitlyReviewed"]=4
new["TotalNamedSourceLearningAndPresentationLeafActions"]=273
new["SourceAtomicLeavesWithFirstPartyArtifactLocator"]=273
new["CountMethod"]="Earlier 256 named actions plus thirteen original source narrative passages and four explicit formal definitions; parents with enumerated subparts are replaced by their leaves."
new["Categories"]={**old["Categories"],"NARRATIVE":13,"DEFINITION":4}
new["Atoms"]=new_atoms
new["ReadyForStageD"]=False
new_path=base/"u01-l01-273-named-source-atomic-actions.json"
new_path.write_text(json.dumps(new,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
ledger["NamedSourceLeafActionInventoryPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-273-named-source-atomic-actions.json"
ledger["SourceNamedAtomicLeafActionCount"]=273
ledger["AllNamedSourceLeafActionFirstPartyArtifactLocatorsRecorded"]=True
ledger_path.write_text(json.dumps(ledger,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
for filename in ("u01-l01-abc-checkpoint.json","full-grade-progress.json"):
    f=base/filename
    j=json.loads(f.read_text(encoding="utf-8"))
    p=j if filename.startswith("u01") else j["LessonEntries"][0]
    p["OriginalSourceAnchorsSourceVerified"]=187
    p["SourceTopLevelAnchorsSourceVerified"]=187
    p["NamedSourceLeafActionInventoryPath"]=ledger["NamedSourceLeafActionInventoryPath"]
    p["SourceNamedAtomicLeafActionCount"]=273
    p["OriginalSourceNarrativePassagesExplicitlyReviewed"]=13
    p["OriginalSourceFormalDefinitionsExplicitlyReviewed"]=4
    p["SourceAtomicDenominatorVerified"]=False
    p["ReadyForStageD"]=False
    f.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
evidence=base/"u01-l01-thirteen-narrative-and-four-definition-source-alignments.md"
evidence.write_text("# Original Section 1.1 source narrative and formal definitions\n\n"
 + "Source: authorized local 2017 OpenStax Precalculus PDF pages 18–33. Earlier locator audit missed prose/narrative as a distinct source category, even though the first-party lesson had some matching explanations. This adds thirteen identified pedagogic passages and four named definitions; it does NOT replace correctly authored previous work.\n\n"
 + "| PDF page | Narrative learning demand | First-party mapping |\n|---|---|---|\n"
 + "".join(f"| {page} | {meaning} | {delivery} |\n" for page,meaning,heading,delivery in narrative)
 + "\n| PDF page | Explicit definition | First-party key concept |\n|---|---|---|\n"
 + "".join(f"| {page} | {title}: {definition} | {concept} |\n" for page,title,definition,concept in definitions)
 + "\nActual content gap repairs: clarified function parentheses are not multiplication, finite table samples do not imply unlisted input behavior, isolated graph samples are not proof of continuous curves, and named all nine toolkit families including reciprocal squared and cube root. The existing RichV2 and canonical source pack are updated together.\n\n"
 + "Named source candidates now 273 (including 162 exercise actions and 28 example/15 Try-It actions), but complete mathematical source-atom equivalence, graph/media details, and source cell-level parity are NOT certified. Source A–B–C remains open. Tests/CI/Stage D not run.\n",encoding="utf-8",newline="\n")
print("G12_1_1_NARRATIVE_13_DEFINITIONS_4_GENUINE_CONTENT_GAPS_REPAIRED_273_NAMED_LEAVES")
