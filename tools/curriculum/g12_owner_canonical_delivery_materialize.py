#!/usr/bin/env python3
"""Build exact G12 owner-authorized canonical delivery from already authored RichV2.
Does not certify 1:1 textbook parity, independently grade answers or deploy."""
import copy,hashlib,json,re,subprocess
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2]
CANON="src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g12-precalculus-u01-l01-abc-draft.lesson-content-pack.json"
PROGRESS="artifacts/curriculum-delivery/US-CCSS-MATH/G12/full-grade-progress.json"
AUTH="artifacts/curriculum-delivery/US-CCSS-MATH/G12/grade12-owner-academic-release-authorization-20261107.md"
CONTENT_VERSION="g12-precalculus-73-owner-approved-canonical-20261107"
def load(rel):
    p=ROOT/rel
    if p.exists():
        return json.loads(p.read_text(encoding="utf-8"))
    return json.loads(subprocess.check_output(["git","show","HEAD:"+rel],cwd=ROOT).decode("utf-8"))
def render_translation(lesson):
    def text(v):
        return str(v or "").strip()
    def join(items):
        return "\n\n".join(text(x) for x in items if text(x))
    def concepts():
        return join(["\n".join(text(k.get(p)) for p in ("Title","Definition","Rule","Example") if text(k.get(p))) for k in lesson.get("KeyConcepts",[])])
    def worked():
        return join(["\n".join(x for x in [
            f"Example {i}: {text(w.get('Title'))}",text(w.get("Question")),
            f"Method: {text(w.get('Method'))}",f"Answer: {text(w.get('Answer'))}"] if x)
            for i,w in enumerate(lesson.get("WorkedExamples",[]),1)])
    def solutions():
        return join([f"{i}. {text(w.get('Title'))}\n"+
                     "\n".join(f"Step {j}: {text(s)}" for j,s in enumerate(w.get("Steps",[]),1))
                     for i,w in enumerate(lesson.get("WorkedExamples",[]),1)])
    def mistakes():
        return join([f"{text(m.get('Mistake'))}\nWhy: {text(m.get('WhyWrong'))}\nCorrection: {text(m.get('Correction'))}"
                     for m in lesson.get("CommonMistakes",[])])
    def summary():
        return join([f"{i}. {text(s)}" for i,s in enumerate(lesson.get("SummaryPoints",[]),1)])
    translation={"CultureCode":"en","Title":text(lesson["Title"]),
        "Explanation":join(lesson["ExplanationParagraphs"]),
        "KeyConceptsAndRules":concepts(),
        "WorkedExamples":worked(),
        "StepByStepSolutions":solutions(),
        "CommonMistakes":mistakes(),
        "QuickSummary":summary()}
    for k,v in translation.items():
        if not v:raise ValueError(f"Canonical translation cannot omit {k} for {lesson['LessonCode']}")
    return translation
def main():
    progress=load(PROGRESS)
    entries=progress["LessonEntries"]
    assert len(entries)==73
    assert all(e["ReadyForStageD"] for e in entries)
    assert (ROOT/AUTH).exists()
    original=load(CANON)
    assert original["Lessons"][0]["LessonCode"]==entries[0]["LessonCode"]
    assert len(original["Lessons"]) in (1,73)
    # The full course is the user's owner-approved content. This does not
    # upgrade the source-derived comparison ledger or waive rights notices.
    first=copy.deepcopy(original["Lessons"][0])
    first["AdaptationStatus"]="OWNER_AUTHORIZED_FIRST_PARTY_CANONICAL_CANDIDATE"
    all_lessons=[first]
    for entry in entries[1:]:
        rich_path=entry.get("RichV2Path") or (
            "src/Edulytics.Core/Curriculum/LessonContent/RichV2/"+
            f"us-ccss-g12-precalculus-u{entry['Unit']:02d}-l{entry['LessonNumber']:02d}.rich-lesson-v2.json")
        raw=load(rich_path)
        ls=raw.get("Lessons") or []
        assert len(ls)==1,rich_path
        rich=ls[0]
        code=entry["LessonCode"]
        assert rich["LessonCode"]==code
        t=render_translation(rich)
        sha=hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True).encode("utf-8")).hexdigest()
        source_ref=f"Precalculus 2017 Section {entry['SourceSection']}; local PDF SHA256 {progress['SourceSnapshotSha256']}"
        all_lessons.append({
            "LessonCode":code,
            "TitleProvenance":"PedagogicalSource",
            "TitleSourceReference":source_ref,
            "OutcomeCodes":[],
            "OfficialReferenceCode":None,
            "IsSupporting":False,
            "SourceUrl":"https://openstax.org/details/books/precalculus",
            "SourceLocator":f"Section {entry['SourceSection']}, PDF pp. {entry.get('PdfPageStart','?')}–{entry.get('PdfPageEnd','?')}",
            "SourceTitle":"Precalculus",
            "SourcePublisher":"OpenStax / Rice University",
            "SourceEdition":"Local 2017 revision PR-2015-002(03/17)-BW",
            "SourceRights":"User-authored mathematical explanation and task alternatives; CC BY 4.0 credit for any adapted source content appears in website credits.",
            "SourceSha256":progress["SourceSnapshotSha256"],
            "CanonicalBodySha256":sha,
            "SourceVerifiedAtUtc":"2026-11-06T10:52:00Z",
            "RetrievalUrl":"https://openstax.org/details/books/precalculus",
            "RetrievalChannel":"USER_LOCAL_READ_ONLY",
            "RetrievalTimestamp":"2026-11-06T10:52:00Z",
            "AdaptationStatus":"OWNER_AUTHORIZED_FIRST_PARTY_CANONICAL_CANDIDATE",
            "Translations":[t]})
    assert len(all_lessons)==73
    assert len({l["LessonCode"] for l in all_lessons})==73
    assert sum(1 for l in all_lessons if l["OutcomeCodes"])==1
    original["Status"]="Published"
    original["ReviewedBy"]="Edulytics programme owner — approval 2026-11-07"
    original["ReviewEvidence"]=AUTH+"; authorization of academic release; technical Stage D must pass before merge"
    original["ReviewMethod"]="Owner-authorized release candidate, 73 already authored RichV2 lessons, original source snapshots and per-lesson A-B-C close records; no new AI-generated mathematics"
    original["ContentVersion"]=CONTENT_VERSION
    original["Lessons"]=all_lessons
    original["PedagogicalSourceRightsNote"]="Independent mathematics authoring asserted by owner; original textbook provenance/CC BY 4.0 credits maintained separately in content-sources-licenses.html; external third-party assets not automatically licensed."
    p=ROOT/CANON
    p.parent.mkdir(parents=True,exist_ok=True)
    p.write_text(json.dumps(original,indent=2,ensure_ascii=False)+"\n",encoding="utf-8")
    print(json.dumps({"status":"CANDIDATE_ONLY_NOT_DEPLOYED","canonicalCount":len(all_lessons),
        "ownerAuthorization":AUTH,"publisherNameInLearnerProseCount":sum(any("openstax" in str(t[f]).lower() for f in ("Explanation","KeyConceptsAndRules","WorkedExamples","StepByStepSolutions","CommonMistakes","QuickSummary")) for l in all_lessons for t in l["Translations"]),
        "contentVersion":CONTENT_VERSION}))
if __name__=="__main__":
    main()
