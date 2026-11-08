from __future__ import annotations
import json
from pathlib import Path
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
route_path=base/"u01-l01-exercise-routing.json"
candidate_path=base/"u01-l01-exercise-subparts-candidate-ledger.json"
bank_path=base/"u01-l01-first-party-exercise-bank.json"
checkpoint_path=base/"u01-l01-abc-checkpoint.json"
coverage_path=base/"u01-l01-element-coverage.json"
progress_path=base/"full-grade-progress.json"
route=json.loads(route_path.read_text(encoding="utf-8"))
candidate=json.loads(candidate_path.read_text(encoding="utf-8"))
bank=json.loads(bank_path.read_text(encoding="utf-8"))
checkpoint=json.loads(checkpoint_path.read_text(encoding="utf-8"))
coverage=json.loads(coverage_path.read_text(encoding="utf-8"))
progress=json.loads(progress_path.read_text(encoding="utf-8"))
src=route["SourceExerciseContracts"]
assert len(src)==92 and [x["OriginalExerciseNumber"] for x in src]==list(range(1,93))
cand=candidate["Items"]
assert len(cand)==106
by_num={x["OriginalExerciseNumber"]:[] for x in src}
for item in cand:
    n=item["OriginalExerciseNumber"]
    assert n in by_num
    assert item["ParentElementId"]==src[n-1]["SourceElementId"]
    by_num[n].append(item)
assert len([x for x in by_num.values() if x])==36
assert sorted(n for n,v in by_num.items() if v)==list(range(27,32))+list(range(34,40))+list(range(52,55))+list(range(68,74))+list(range(76,88))+list(range(88,92))
assert len([x for x in by_num.values() if not x])==56
assert len(bank["Items"])==92
assert all(item["Number"]==i+1 for i,item in enumerate(bank["Items"]))
atoms=[]
for source in src:
    n=source["OriginalExerciseNumber"]
    parts=by_num[n]
    base_atom={
        "OriginalExerciseNumber":n,
        "ParentExerciseId":source["SourceElementId"],
        "SourceSection":"1.1",
        "PdfPage":source["SourcePdfPage"],
        "SourceSnapshotSha256":checkpoint["SourceSnapshotSha256"],
        "SkillKind":source["SkillKind"],
        "EdulyticsPracticeTaskOrdinal":n,
        "PracticeTaskEvidenceFile":"src/Edulytics.Web/Presentation/Grade12PrecalculusSection11Exercises.cs",
        "PracticeTaskStatus":"FIRST_PARTY_CANDIDATE_PENDING_1_TO_1_SEMANTIC_REVIEW",
        "SemanticParity":"UNMATCHED",
        "AssessmentTests":"WRITTEN_NOT_RUN",
    }
    if not parts:
        atoms.append({
            **base_atom,
            "AtomicElementId":source["SourceElementId"],
            "AtomicScope":"COMPLETE_SINGLE_PROMPT",
            "OriginalPartLabel":None,
            "SourceIdentificationStatus":"SOURCE_IDENTIFIED_AT_NUMBERED_SECTION_EXERCISE",
        })
    else:
        for part in parts:
            assert "PartLabel" in part
            atoms.append({
                **base_atom,
                "AtomicElementId":part["SourceElementId"],
                "AtomicScope":"EXPLICIT_EXERCISE_PART",
                "OriginalPartLabel":part["PartLabel"],
                "SourcePartScope":part["PromptScope"],
                "SourceIdentificationStatus":part["OriginalSourcePartStatus"],
                "RequiresSourceFigure":part["RequiresSourceFigure"],
            })
assert len(atoms)==162
assert len({a["AtomicElementId"] for a in atoms})==162
for row in atoms:
    if row["OriginalExerciseNumber"]==52 and row["OriginalPartLabel"]=="b":
        assert row["SourceIdentificationStatus"]=="SOURCE_GRAPH_EXACT_ROOT_UNRESOLVED"
        row["SemanticParity"]="SOURCE_UNRESOLVED_DO_NOT_INVENT_EXACT_ROOT"
manifest={
    "SchemaVersion":1,
    "Course":"US-CCSS-MATH:G12",
    "Unit":1,"Lesson":1,"SourceSection":"1.1",
    "SourceSnapshotSha256":checkpoint["SourceSnapshotSha256"],
    "OriginalSourceNumberedExercises":92,
    "MultiPartExerciseNumbers":36,
    "SingletonExercises":56,
    "OriginalExplicitPartElements":106,
    "AtomicSourceExerciseTasksWithoutDoubleCounting":162,
    "DenominatorStatus":"EXERCISE_SUBTASK_NUMBERING_IDENTIFIED_162; NOT_FULL_LESSON_ATOMIC_DENOMINATOR",
    "SourceFiguresIndividuallyMatched":False,
    "SourceSkillSemanticsIndividuallyMatched":False,
    "TeacherOnlyGraph52BPrecision":"SOURCE_UNRESOLVED",
    "FullLessonSourceAtomicDenominatorVerified":False,
    "ReadyForStageD":False,
    "Tests":"WRITTEN_NOT_RUN",
    "Proof":"92 original Section 1.1 exercises on local PDF pages 34-37; reviewed 106 explicitly requested subpart actions within 36 multiparts; 56 unpartitioned exercise actions remain as singleton atoms. No parent and child task counted together.",
    "NotCountedHere":"Worked example/Analysis/Try It substeps, source tables, images, Q&A, media, learning objectives, chapter-wide resources",
    "Atoms":atoms
}
dest=base/"u01-l01-exercise-atomic-source-inventory.json"
dest.write_text(json.dumps(manifest,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
route["SourceExerciseAtomicInventoryPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-exercise-atomic-source-inventory.json"
route["SourceExerciseAtomicElementsIdentified"]=162
route["SourceExerciseAtomicDenominatorIdentifiedByOrdinal"]=True
route["SourceExerciseAtomicElementsSemanticallyMatched"]=0
route["All92ExerciseSubpartsSemanticallyMatched"]=False
route_path.write_text(json.dumps(route,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
coverage["SourceExerciseAtomicInventoryPath"]=route["SourceExerciseAtomicInventoryPath"]
coverage["SourceExerciseAtomicElementsIdentified"]=162
coverage["SourceExerciseAtomDenominatorIdentified"]=True
coverage["SourceSemanticMatchesCertified"]=0
coverage["SourceAtomicDenominatorVerified"]=False
coverage["AtomicSourceDenominatorVerified"]=False
coverage["AuthoringEvidence"]["SourceExerciseAtomicElementsIdentified"]=162
coverage_path.write_text(json.dumps(coverage,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
checkpoint["SourceExerciseAtomicInventoryPath"]=route["SourceExerciseAtomicInventoryPath"]
checkpoint["SourceExerciseAtomicElementsIdentified"]=162
checkpoint["SourceExerciseAtomicDenominatorIdentified"]=True
checkpoint["SourceAtomicDenominatorVerified"]=False
checkpoint["ReadyForStageD"]=False
checkpoint_path.write_text(json.dumps(checkpoint,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
p=progress["LessonEntries"][0]
p["SourceExerciseAtomicInventoryPath"]=route["SourceExerciseAtomicInventoryPath"]
p["SourceExerciseAtomicElementsIdentified"]=162
p["SourceExerciseAtomicDenominatorIdentified"]=True
p["SourceAtomicDenominatorVerified"]=False
p["ReadyForStageD"]=False
p["UpdatedUtc"]=datetime.now(timezone.utc).isoformat()
progress_path.write_text(json.dumps(progress,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("EXERCISE_ATOMS_162 (56 singleton + 106 explicit parts; 36 multiparts)")
print("OTHER_SOURCE_ATOMS_NOT_YET_COMPLETE; G12_L01_NOT_CLOSED")
