#!/usr/bin/env python3
"""Grade 12 A-B-C initial state: source indexed, zero unverified lessons closed."""
import datetime,json
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2]
BASE=ROOT/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
inv=json.loads((BASE/"g12-openstax-local-source-inventory-20261106.json").read_text(encoding="utf8"))
NOW=datetime.datetime.now(datetime.timezone.utc).isoformat()
SHA=inv["sourceSha256"]
assert inv["indexedSections"]==73 and len(inv["sectionRows"])==73
assert inv["unitCounts"]==[7,4,9,8,4,3,6,8,8,5,7,4]
def save(name,data):
    (BASE/name).write_text(json.dumps(data,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
save("source-of-truth.json",{
    "Course":"US-CCSS-MATH:G12","Book":"OpenStax Precalculus",
    "OriginalPublicationYear":2014,"LocalCopyrightYear":2017,
    "LocalRevision":"PR-2015-002(03/17)-BW",
    "PdfPageCount":1156,"UnitCount":12,"LessonCount":73,
    "PdfReadOnlyPath":inv["sourcePathReadOnly"],"LocalPdfSha256":SHA,
    "AcceptedSources":[{"path":inv["sourcePathReadOnly"],"role":"SOLE_BASELINE_FOR_GRADE_12"}],
    "OtherSources":"Exact_Sources supplements only with independently documented mapping.",
    "ExcludedFamilies":["AccessIM","v360","UAE Grade 12","Grade 11 Algebra 2","HS Fourth Courses"],
    "SourceLicense":"CC BY 4.0 International; attribution required; separate third-party figure and external link rights.",
    "CCSSStatus":"SEPARATE_MAPPING_REQUIRED; PRECALCULUS MAY_EXTEND_CCSS_PLUS",
    "SemanticSourceStatus":"NOT_100_PERCENT_VERIFIED","CourseStatus":"A_C_IN_PROGRESS",
    "AcademicOwnerPosition":"OWNER_ACCEPTS_FIRST_PARTY_DIRECTION_NOT_TECHNICAL_OR_THIRD_PARTY_RIGHTS_CERTIFICATION",
    "NoTeacherEditionClaim":True,"AttributionSeparateFromLessonCopy":True,
    "SnapshotCreatedUtc":NOW})
rows=[]
for s in inv["sectionRows"]:
    rows.append({
      "Ordinal":s["ordinal"],"LessonCode":s["lessonCode"],"Unit":s["unitNumber"],
      "LessonNumber":s["lessonNumber"],"Title":s["sourceTitle"],
      "SourceSection":s["sourceSection"],"PdfPageStart":s["pdfPageStart"],
      "PdfPageEnd":s["pdfPageEndInclusive"],
      "ACStatus":"A_C_IN_PROGRESS" if s["ordinal"]==1 else "A_C_PENDING",
      "SourceLocatorVerified":True,"SourceAtomicDenominatorVerified":False,
      "SemanticEquivalenceVerified":False,"VisualsVerified":False,
      "PracticeVerified":False,"RightsVerified":False,
      "ReadyForStageD":False,"Tests":"WRITTEN_NOT_RUN" if s["ordinal"]==1 else "NOT_WRITTEN",
      "EvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-element-coverage.json" if s["ordinal"]==1 else None,
      "LastCommitSha":None,
      "BlockingReasons":["NO_INDEPENDENT_ATOMIC_SOURCE_PARITY","NO_PROVEN_VISUAL_PARITY","NO_VERIFIED_G12_LESSON_PRACTICE"] if s["ordinal"]==1 else ["NOT_YET_REVIEWED"]})
save("full-grade-progress.json",{
    "SchemaVersion":1,"Grade":12,"Curriculum":"US-CCSS-MATH",
    "SourceEdition":inv["sourceVersion"],"SourceSnapshotSha256":SHA,
    "UnitCounts":inv["unitCounts"],"UnitTitles":inv["unitTitles"],"TotalLessons":73,
    "ReadyForStageD":0,"FirstIncompleteLessonOrdinal":1,
    "LastClosedLessonOrdinal":0,"ActiveLessonOrdinal":1,"NextLessonOrdinal":None,
    "GradeStatus":"GRADE12_A_C_INCOMPLETE","CIRun":"NOT_RUN",
    "StageD":"NOT_STARTED","Publication":"NOT_AUTHORIZED_BY_A_C",
    "NoFullSyncOrBuild":True,"LessonEntries":rows,"UpdatedUtc":NOW})
first=inv["sectionRows"][0]
locators=[]
def add(kind,no,page,audience="Student",rights="CC_BY_4_SOURCE_MATERIAL"):
    locators.append({
      "SourceElementId":f"G12-U01-L01-{kind}{no:03d}",
      "ParentElementId":"G12-U01-L01","Category":kind,
      "SourceLocator":{"PdfPage":page,"Section":"1.1",
          "SectionTitle":first["sourceTitle"],"SourceSha256":SHA},
      "ExpectedMathematics":"SOURCE_IDENTIFIED_NOT_YET_FULLY_VERIFIED",
      "TeacherOrStudentAudience":audience,"RightsStatus":rights,
      "EdulyticsArtifactPath":None,"ImplementationType":"PENDING",
      "VerificationMethod":"PDF_BOOKMARK_AND_MARKER_CROSSWALK_INITIAL_ONLY",
      "TestNames":["Grade12PrecalculusSection11SourceContractsTests (WRITTEN_NOT_RUN)"],
      "Status":"SOURCE_IDENTIFIED","Reviewer":"NOT_ASSIGNED",
      "ContentVersion":"g12-u01-l01-abc-draft","CandidateSha":"PENDING_COMMIT",
      "UpdatedUtc":NOW})
for i in range(1,6):add("OBJ",i,18)
for kind in ("EXAMPLE","TRY","FIG","TABLE"):
    seen=set()
    for x in first["candidateMarkers"].get(kind,[]):
        key=int(x["marker"])
        if key in seen:continue
        seen.add(key)
        rights=("REQUIRES_SEPARATE_VISUAL_PROVENANCE_CHECK" if kind in ("FIG","TABLE") else "CC_BY_4_SOURCE_MATERIAL")
        add(kind,key,x["pdfPage"],"Student",rights)
for i,x in enumerate(first["candidateMarkers"].get("HOW_TO",[]),1):
    add("HOWTO",i,x["pdfPage"])
for kind in ("ANALYSIS","Q_AND_A"):
    for i,x in enumerate(first["candidateMarkers"].get(kind,[]),1):
        add(kind,i,x["pdfPage"])
# Original section exercises 1–92: page ranges visually located from section PDF.
for i in range(1,93):
    page=34 if i<=39 else 35 if i<=51 else 36 if i<=67 else 37
    add("EXERCISE",i,page)
for i in range(1,6):add("MEDIA",i,33,"Student","EXTERNAL_LINK_TERMS_UNREVIEWED")
assert len({x["SourceElementId"] for x in locators})==len(locators)
save("u01-l01-element-coverage.json",{
    "Grade":12,"Unit":1,"Lesson":1,"Ordinal":1,
    "LessonCode":first["lessonCode"],
    "SourceEdition":inv["sourceVersion"],"SourceSnapshotId":SHA,
    "PdfPages":[18,37],"Count":len(locators),
    "AtomicSourceDenominatorVerified":False,
    "SourceInventory":"PARTIAL_ENUMERATION_NARRATIVE_MATH_GLYPHS_AND_VISUALS_PENDING",
    "MatchedDigitally":0,"MatchedTeacherLed":0,"OpenSourceElements":len(locators),
    "Elements":locators,"Status":"A_C_IN_PROGRESS","StageD":"NOT_RUN",
    "Caveat":"Numbers, examples and figure anchors are source-located. Subparts, full diagram semantics and editorial details remain unverified. No source coverage denominator or 1:1 parity is claimed.",
    "CreatedUtc":NOW})
guide="""# Grade 12 / US-CCSS-MATH — binding A–B–C execution rules

Scope: OpenStax Precalculus local ©2017 PDF revision PR-2015-002(03/17)-BW, 12 chapters, 73 Sections. Local PDF is READ ONLY. Exact_Sources is supplementary; exclude AccessIM/v360, Grade11 Algebra2, HS Fourth Courses and UAE Grade12 from this baseline.

Discovery: the 2026-11-06 main snapshot has no US-CCSS-MATH Grade12 Precalculus Blueprint or RichV2 content. Create only missing independent course parts after inspecting contracts, never relabel other pathways.

Sequence: read first not-ready lesson from full-grade-progress.json, inspect all original source elements and chapter resources, align Stage A then author/repair Stage B then Stage C, write but DO NOT RUN tests, save element evidence/checkpoint, commit lesson closure before moving to the next. Prefetch whole chapter page ranges without marking future lessons complete.

Speed: batch local PDF reading and independent artifact writes. One commit per lesson when truly closed, no repeated sources, full solution builds or CI. Target three minutes only for straightforward repairs; correctness and denominators override elapsed time. Reuse existing math engine, extend narrow family only on verified gap, build new engine only for distinct interaction.

Cool-down: author first-party Edulytics cool-down when original requires unavailable teacher access; never claim it is original. Human evaluation and drawings require teacher-review contract, not automatic-grading claims.

Completion: distinguish source inventory, pedagogic-semantic parity, digitally assessed skills and evidence completeness. Never mark MATCHED without actual equivalent. Missing diagram/rights/skill/atomic item blocks READY_FOR_STAGE_D. Do not add REVIEW complete 2021. No Stage D, automated tests, CI dispatch, merge, Render or production changes during A–B–C.

Academic owner: approves first-party instructional direction and future conditional publication, but CC BY 4.0 attribution and third-party rights remain mandatory; keep copyright credits separate from learner explanation prose.

Current state: 73/73 source section locators indexed; zero source-verified lesson closures, first active U01:L01.
"""
(BASE/"grade12-abc-execution-instructions.md").write_text(guide,encoding="utf8")
print("G12_BOOTSTRAP",len(rows),"LESSONS FIRST",rows[0]["LessonCode"],"SOURCE_ANCHORS",len(locators),"READY",0)
