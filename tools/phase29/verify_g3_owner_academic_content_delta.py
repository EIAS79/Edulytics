#!/usr/bin/env python3
"""Exact-blob owner-scoped Grade 3 Common Arabic content delta; NOT 2026-27 parity."""
import json
import subprocess
from pathlib import Path

ROOT=Path(__file__).resolve().parents[2]
PACK="src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g3-common-t1-ogl-v1.lesson-content-pack.json"
BLUEPRINT="src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g3-common-t1-ogl-v1.lesson-blueprint.json"
AUTH="artifacts/curriculum-delivery/UAE-MOE-MATH/G3/grade3-owner-academic-publication-authorization-20261110.json"
OLD="27970320c56f328d56741aae78b5ebbb6fc8c6ed"
APPROVED="f1c21155bafa7ef4594f7ba4f136ee0a020461b2"
BLUEPRINT_SHA="1c3abd2cd83746e6a98bd2aaa680b7d8de4cf193"
FIELDS=("Title","Explanation","KeyConceptsAndRules","WorkedExamples","StepByStepSolutions","CommonMistakes","QuickSummary")

def git(*args):
    return subprocess.check_output(["git",*args],cwd=ROOT,text=True,encoding="utf8").strip()
def doc(p):return json.loads((ROOT/p).read_text(encoding="utf-8-sig"))
def validate():
    old=git("rev-parse","origin/main:"+PACK)
    current=git("rev-parse","HEAD:"+PACK)
    worktree=git("hash-object",PACK)
    if old not in (OLD,APPROVED) or current!=APPROVED or worktree!=APPROVED:
        raise ValueError("Grade3 original/approved content pack exact Git blob drift")
    if git("rev-parse","origin/main:"+BLUEPRINT)!=BLUEPRINT_SHA or git("rev-parse","HEAD:"+BLUEPRINT)!=BLUEPRINT_SHA or git("hash-object",BLUEPRINT)!=BLUEPRINT_SHA:
        raise ValueError("Grade3 source blueprint must remain unchanged")
    owner=doc(AUTH)
    expected={"Grade":3,"Framework":"UAE-MOE-MATH","Pathway":"Common",
      "OwnerDecision":"ACADEMICALLY_APPROVED_BY_OWNER",
      "OwnerAcceptsKnownSourceLimitationsForOriginalContentRelease":True,
      "SourceParityIsNotReleaseClaim":True,
      "Independent2026_2027ElementParity":"UNKNOWN",
      "ThirdPartyAssetClearance":"NOT_INDEPENDENTLY_CERTIFIED",
      "NoFalseCIClaim":True}
    if any(owner.get(key)!=value for key,value in expected.items()) or owner.get("IndependentExerciseDenominator") is not None:
        raise ValueError("Grade3 owner academic scope/rights/2026-27 limitations invalid")
    candidate,blue=doc(PACK),doc(BLUEPRINT)
    lessons=candidate.get("Lessons",[])
    codes=[x.get("LessonCode") for x in lessons]
    if not (candidate.get("AcademicLanguage")=="ar" and
            len(lessons)==len(blue.get("Lessons",[]))==len(set(codes))==109 and
            len(blue.get("Units",[]))==14 and
            codes==[x.get("LessonCode") for x in blue["Lessons"]] and
            all(x.startswith("PED:UAE-MOE-MATH:L3:COMMON:") for x in codes)):
        raise ValueError("Grade3 109 Arabic lesson identities or blueprint drift")
    source=owner["SelectedLocalSourceSha256"]
    by_unit={
       **{x:"01_compressed.pdf" for x in range(1,7)},
       **{x:"02_compressed.pdf" for x in range(7,11)},
       **{x:"03_compressed.pdf" for x in range(11,15)}
    }
    for x in lessons:
        code=x["LessonCode"]
        unit=int(code.split(":")[-2])
        if x.get("SourceSha256","").casefold()!=source[by_unit[unit]].casefold():
            raise ValueError("Grade3 source PDF fingerprint drift: "+code)
        translations=[tr for tr in x.get("Translations",[]) if tr.get("CultureCode")=="ar"]
        if len(translations)!=1:
            raise ValueError("Grade3 canonical Arabic missing/duplicated: "+code)
        tr=translations[0]
        for field in FIELDS:
            text=str(tr.get(field,""))
            if len(text.strip())<5:
                raise ValueError("Grade3 canonical original teaching section missing: "+code+":"+field)
            lowered=text.casefold()
            if any(marker in lowered for marker in (
                 "<img","<video","<iframe","<svg","https://","http://",".png",".jpg",".jpeg",".mp4","![")):
                raise ValueError("Grade3 embedded third-party assets require individual rights review: "+code)
    before=json.loads(subprocess.check_output(["git","show","origin/main:"+PACK],cwd=ROOT,text=True,encoding="utf8"))
    first=before.get("Lessons",[])
    if len(first)!=109 or [x.get("LessonCode") for x in first]!=codes:
        raise ValueError("Grade3 main original lesson identity/count changed")
    changed={( "UAE-MOE-MATH",later["LessonCode"])
             for original,later in zip(first,lessons)
             if original.get("Translations")!=later.get("Translations")}
    if old==OLD and len(changed)!=109:
        raise ValueError("Grade3 expected exactly 109 new owner-authored canonical bodies")
    if old==APPROVED and changed:
        raise ValueError("Grade3 already published to main and teaching bodies changed again")
    print("G3_OWNER_EXACT_BLOB_ARABIC",len(lessons),"changedBodies",len(changed),"2026_27_PARITY_UNKNOWN")
    return changed

if __name__=="__main__":
    print("G3_PINNED_OWNER_AUTHORIZED_DELTA_KEYS",len(validate()))
