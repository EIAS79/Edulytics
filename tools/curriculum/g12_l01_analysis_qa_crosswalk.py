from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ledger_path=base/"u01-l01-element-coverage.json"
atomic_path=base/"u01-l01-256-named-source-atomic-actions.json"
ledger=json.loads(ledger_path.read_text(encoding="utf-8"))
atomic=json.loads(atomic_path.read_text(encoding="utf-8"))
rich_rel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l01.rich-lesson-v2.json"
pack_rel="src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g12-precalculus-u01-l01-abc-draft.lesson-content-pack.json"
rich=json.loads((root/rich_rel).read_text(encoding="utf-8"))["Lessons"][0]
pack=json.loads((root/pack_rel).read_text(encoding="utf-8"))["Lessons"][0]["Translations"][0]
# Restore the source's originally nonnumeric, month-name function example.
rich_file=root/rich_rel
rich_json=json.loads(rich_file.read_text(encoding="utf-8"))
rich_lesson=rich_json["Lessons"][0]
month_note=("The input to a function need not be numerical. For a fixed non-leap year, "
            "let D(m) return the number of days in the named month m; "
            "D(March)=31. The year is fixed so that February is unambiguous.")
if month_note not in rich_lesson["ExplanationParagraphs"]:
    rich_lesson["ExplanationParagraphs"].insert(3, month_note)
rich_json["ContentVersion"]="g12-u01-l01-source-nonnumeric-month-analysis-20261106"
rich_file.write_text(json.dumps(rich_json, ensure_ascii=False, indent=2)+"\n", encoding="utf-8")
rich=rich_lesson
proof={
"G12-U01-L01-ANALYSIS001":("Nonnumeric inputs such as names and month labels are permitted.","Nonnumeric",3),
"G12-U01-L01-ANALYSIS002":("Some relations defined by equations fail the single-output test, including a circle.","Implicit function subtlety",10),
"G12-U01-L01-Q_AND_A001":("The function symbol and output symbol differ in educational function notation.","Notation choice",4),
"G12-U01-L01-Q_AND_A002":("An implicit relation may define a function without an elementary explicit formula.","Implicit function subtlety",10),
}
for key,(meaning,phrase,worked_number) in proof.items():
    x=next(x for x in ledger["Elements"] if x["SourceElementId"]==key)
    item=next(x for x in atomic["Atoms"] if x["AtomId"]==key)
    corpus=" ".join(rich["ExplanationParagraphs"] + [w["Question"]+" "+w["Answer"] for w in rich["WorkedExamples"]])
    if phrase=="Nonnumeric":
        assert "month" in corpus.lower() and "month" in pack["Explanation"].lower()
    else:
        assert phrase in corpus and phrase in pack["Explanation"]
    x["EdulyticsArtifactPath"]=rich_rel
    x["FirstPartyNarrativePackPath"]=pack_rel
    x["FirstPartySkillEvidence"]=meaning
    x["ImplementationType"]="FIRST_PARTY_EDUCATIONAL_EXPLANATION_WITH_ORIGINAL_SOURCE_CONCEPT"
    x["SourceSkillRepresentativeVerifiedInBothContentPaths"]=True
    item["FirstPartyDelivery"]=rich_rel
    item["FirstPartyPackAlternative"]=pack_rel
    item["SourceSkillRepresentativeVerifiedInBothContentPaths"]=True
    item["SourceElementSemanticParityCertified"]=False
atomic["SourceAtomicLeavesWithFirstPartyArtifactLocator"]=sum(bool(x.get("FirstPartyDelivery")) for x in atomic["Atoms"])
assert atomic["SourceAtomicLeavesWithFirstPartyArtifactLocator"]==len(atomic["Atoms"])==256
atomic["SourceAtomicLeavesWithExactSourcePedagogicSemanticMatchCertified"]=0
atomic["SourceCompleteFineGrainedMediaCellAndParagraphDenominatorVerified"]=False
atomic["ReadyForStageD"]=False
ledger["SourceFineGrainedSourceAtomicDenominatorVerified"]=False
ledger["SourceAtomicDenominatorVerified"]=False
ledger["AllNamedSourceLeafActionFirstPartyArtifactLocatorsRecorded"]=True
ledger_path.write_text(json.dumps(ledger,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
atomic_path.write_text(json.dumps(atomic,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
checkpoint=base/"u01-l01-abc-checkpoint.json"
c=json.loads(checkpoint.read_text(encoding="utf-8"))
c["NamedSourceLeafActionArtifactLocatorsRecorded"]=256
c["OriginalSourceAnnotationAndQALinksReconciled"]=4
c["SourceElementSemanticParityCertified"]=False
c["ReadyForStageD"]=False
checkpoint.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
e=p["LessonEntries"][0]
e["NamedSourceLeafActionArtifactLocatorsRecorded"]=256
e["ReadyForStageD"]=False
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
path=base/"u01-l01-analysis-qa-source-content-crosswalk.md"
path.write_text("""# Section 1.1: original Analysis and Q&A first-party crosswalk

The four original source markers are 2 Analysis paragraphs and 2 Q&A
paragraphs. They were checked in the read-only authorized local PDF
(2017 edition, original Section 1.1).

- Analysis 1, PDF 21: nonscalar function inputs may be words such as
  months and names. First-party explanation and worked example 3 discuss
  this explicitly.
- Analysis 2, PDF 25: an implicit equation can have multiple outputs,
  so it does not automatically define a function. First-party worked
  example 10 uses the circle and checks two branches.
- Q&A 1, PDF 21: y=y(x) notation is valid, but distinct symbol f in
  y=f(x) distinguishes a function rule from its value. Both RichV2
  explanation and canonical content pack now teach the distinction.
- Q&A 2, PDF 25: not every implicit function yields a simple explicit
  formula. Both RichV2 and canonical pack teach uniqueness even without
  symbolic isolation.

The crosswalk resolves four originally null first-party artifact
locators in the 256 named action ledger. **256/256 now have an
identified first-party delivery artifact**. This is evidence of
traceability, **NOT** 256 academically approved exact source-semantic
matches; visual scales, exercise implementations, atomic figure
details and original authored response contracts still require
validation. No tests, full build, CI or Stage D run.
""",encoding="utf-8",newline="\n")
print("G12_U01_L01_256_OF_256_NAMED_SOURCE_LEAVES_NOW_HAVE_FIRST_PARTY_ARTIFACT_POINTERS")
