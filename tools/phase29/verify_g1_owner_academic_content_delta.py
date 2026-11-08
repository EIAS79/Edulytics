#!/usr/bin/env python3
"""Exact Git-blob-pinned UAE Grade 1 first-party academic owner revision.

This is an immutable old->approved content/blueprint exception to V2
storage-only parity. It never claims 1:1 source semantics, permits no
modifications to other packs, and must be renewed for any future edit.
"""
from pathlib import Path
import json
import subprocess

ROOT=Path(__file__).resolve().parents[2]
CONTENT="src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g1-common-t1-ogl-v1.lesson-content-pack.json"
BLUEPRINT="src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g1-common-t1-ogl-v1.lesson-blueprint.json"
ART="artifacts/curriculum-delivery/UAE-MOE-MATH/G1"
AUTH=ART+"/grade1-owner-academic-publication-authorization-20261108.json"
LEDGER=ART+"/grade1-stage-d-execution-ledger.json"
LOCKED={
    CONTENT:(
        "e5264d193ffbeaa955178608f4fcd3318f51708b",
        "4d42e7726cc2c3cf4cd1e33fa3622778ce24a843",
    ),
    BLUEPRINT:(
        "8a799f4c3df8e7c9e7e2c54f5fda0f38e90f1aae",
        "5c960f7cdff923f43703c51f656f2c2f64ecfe15",
    ),
}

def git(*args):
    return subprocess.check_output(
        ["git",*args],cwd=ROOT,text=True,encoding="utf8"
    ).strip()

def document(path):
    return json.loads((ROOT/path).read_text(encoding="utf8"))

def get_blob(ref,path):
    return git("rev-parse",ref+":"+path)

def validate():
    for path,(main_old,approved) in LOCKED.items():
        current=git("hash-object",path)
        original=get_blob("origin/main",path)
        if current!=approved:
            raise ValueError(
                "UAE Grade1 candidate changed after exact owner-accepted blob: "
                +path
            )
        if original not in (main_old,approved):
            raise ValueError("UAE Grade1 original main source baseline changed: "+path)
        if current==main_old:
            raise ValueError("UAE Grade1 owner revision not present: "+path)
        if get_blob("HEAD",path)!=current:
            raise ValueError("UAE Grade1 worktree and commit blob differ: "+path)

    authorization=document(AUTH)
    ledger=document(LEDGER)
    if not (
        authorization.get("Grade")==1
        and authorization.get("Framework")=="UAE-MOE-MATH"
        and authorization.get("OwnerDecision")=="ACADEMICALLY_APPROVED_BY_OWNER"
        and authorization.get("OwnerAcceptsKnownSourceLimitationsForOwnMathContent") is True
        and authorization.get("FullSourceParityClaim") is False
        and authorization.get("MinistryCertificationClaim") is False
        and authorization.get("SourceVersions")=={"2025-2026":42,"2019-2020":60}
        and authorization.get("SourceParity")=="UNKNOWN_INDEPENDENT_1_TO_1_ELEMENT_COMPARISON"
        and ledger.get("sourceParityCertifiedCount")==0
        and ledger.get("DVerifiedLessonCount")==102
        and ledger.get("humanApproval")=="APPROVED"
        and ledger.get("rights")=="OWNER_AUTHORSHIP_ATTESTED_FIRST_PARTY_ONLY"
    ):
        raise ValueError("UAE Grade1 owner academic authorization / source limitation invalid")

    pack=document(CONTENT)
    blueprint=document(BLUEPRINT)
    lessons=pack.get("Lessons",[])
    bp=blueprint.get("Lessons",[])
    codes=[x.get("LessonCode","") for x in lessons]
    original=json.loads(subprocess.check_output(
        ["git","show","origin/main:"+CONTENT],cwd=ROOT,text=True,encoding="utf8"
    ))
    base=original.get("Lessons",[])
    base_codes=[x.get("LessonCode","") for x in base]
    if not (
        pack.get("PackCode")=="UAE-MOE-MATH"
        and pack.get("AcademicLanguage")=="ar"
        and len(lessons)==len(bp)==len(base)==102
        and len(set(codes))==102
        and all(c.startswith("PED:UAE-MOE-MATH:L1:COMMON:") for c in codes)
        and codes==base_codes==[x["LessonCode"] for x in bp]
    ):
        raise ValueError("UAE Grade1 blueprint identity, academic language or scope drift")
    translations_valid=all(
        len([t for t in x.get("Translations",[])
             if t.get("CultureCode")=="ar" and
             all(str(t.get(field,"")).strip() for field in (
                 "Title","Explanation","WorkedExamples","QuickSummary"))
            ])==1
        for x in lessons
    )
    if not translations_valid:
        raise ValueError("UAE Grade1 102 canonical Arabic translation continuity failed")
    actual_changed={
        ("UAE-MOE-MATH",n["LessonCode"])
        for b,n in zip(base,lessons)
        if b.get("Translations")!=n.get("Translations")
    }
    baseline_is_old=get_blob("origin/main",CONTENT)==LOCKED[CONTENT][0]
    if baseline_is_old and len(actual_changed)!=102:
        raise ValueError("UAE Grade1 expected exactly 102 approved body revisions")
    if not baseline_is_old and actual_changed:
        raise ValueError("UAE Grade1 already in main, unexpected additional translations")
    if get_blob("origin/main",BLUEPRINT) not in LOCKED[BLUEPRINT]:
        raise ValueError("UAE Grade1 source blueprint unexpectedly changed")

    # The candidate first-party teaching text contains no separately licensed
    # external images or videos; source metadata URLs are not rendered media.
    body_fields=("Title","Explanation","KeyConceptsAndRules",
                 "WorkedExamples","StepByStepSolutions",
                 "CommonMistakes","QuickSummary")
    for row in lessons:
        for t in row.get("Translations",[]):
            for field in body_fields:
                value=str(t.get(field,"")).lower()
                if any(marker in value for marker in (
                    "<img","<video","<iframe","https://","http://",
                    ".png",".jpg",".jpeg",".mp4"
                )):
                    raise ValueError(
                        "UAE Grade1 external learner teaching media requires separate rights audit: "+
                        row["LessonCode"]
                    )
    print("G1_OWNER_PINNED_CONTENT_DELTA verified=102",
          "sourceParity=UNKNOWN", "changedBodies="+str(len(actual_changed)),
          "firstPartyOnly=True")
    return actual_changed


if __name__=="__main__":
    print("APPROVED_G1_DELTA_KEYS",len(validate()))
