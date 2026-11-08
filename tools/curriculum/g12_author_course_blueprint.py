#!/usr/bin/env python3
"""Create a V2 course identity from the *existing* local-PDF source manifest.
Only source-matched U01:L01 standards receive verified evidence references.
Other 72 lessons remain explicitly UNMAPPED, not READY or D verified.
Does NOT run tests/CI or touch a production database.
"""
import datetime,hashlib,json
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2]
BASE=ROOT/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
SRC=json.loads((BASE/"g12-precalculus-blueprint-source-candidate.json").read_text(encoding="utf8"))
INV=json.loads((BASE/"g12-openstax-local-source-inventory-20261106.json").read_text(encoding="utf8"))
OFF=json.loads((ROOT/"docs/curriculum/reference/us-ccss-official-codes.snapshot.json").read_text(encoding="utf8"))
NOW=datetime.datetime.now(datetime.timezone.utc).isoformat()
assert SRC["SourcePdfSha256"]==INV["sourceSha256"]
assert SRC["LogicalLevelCandidate"]==13, "American Grade 12 must use logical level 13"
assert len(SRC["Entries"])==73 and len(INV["sectionRows"])==73
for code in ("HSF.IF.A.1","HSF.IF.A.2"):
 assert code in OFF["codes"],code
h=lambda value:hashlib.sha256(json.dumps(value,sort_keys=True,ensure_ascii=False).encode("utf8")).hexdigest()
url="https://openstax.org/details/books/precalculus"
versions="OpenStax Precalculus, original 2014, local ©2017, revision PR-2015-002(03/17)-BW"
units=[]
for number,count in enumerate(INV["unitCounts"],1):
 lines=[s for s in INV["sectionRows"] if s["unitNumber"]==number]
 assert len(lines)==count
 units.append({
 "Number":number,"UnitCode":f"U{number:02d}","SortOrder":number,
 "Title":INV["unitTitles"][number-1],"LessonCount":count,
 "SourceUrl":url,
 "SemanticSha256":h({"sourcePdfSha256":INV["sourceSha256"],"unit":number,
                     "pageTextHashes":[a["pageTextHashes"] for a in lines]}),
 })
lessons=[]
for entry,section in zip(SRC["Entries"],INV["sectionRows"]):
 n=entry["ordinal"]
 assert n==section["ordinal"]
 assert entry["lessonCode"]==section["lessonCode"]
 num=section["unitNumber"];local=section["lessonNumber"]
 page=section["pdfPageStart"]
 term={
   "SourceLessonCode":f"OPENSTAX-PRECALCULUS-2017:{num}.{local}",
   "LessonCode":entry["lessonCode"],
   "UnitNumber":num,"UnitTitle":INV["unitTitles"][num-1],
   "LessonNumber":local,"Title":entry["titlePrinted"],
   "SortOrder":n,"SourceUrl":url,
   "SemanticSha256":h({"sourcePdfSha256":INV["sourceSha256"],
                       "section":section["sourceSection"],
                       "pages":section["pageTextHashes"]}),
   "Alignments":[],"OutcomeCodes":[],
   "ApplicableCourses":["Precalculus"],"FormalTargets":[]
 }
 if n==1:
  for k,(official,description) in enumerate([
   ("HSF-IF.A.1","Source section 1.1 pp18–22: relation, domain, range, repeated-input rule and function distinction."),
   ("HSF-IF.A.2","Source section 1.1 pp20–27: interpret function notation, evaluate f(x) and interpret input/output in context.")
  ],1):
   term["FormalTargets"].append({
    "OutcomeCode":"CCSS:"+official,
    "EvidenceKind":"VerifiedContentCoverage",
    "PublisherSuppliedAlignment":False,"SortOrder":k,
    "EvidenceReferences":[{
     "SourceKey":"OPENSTAX-PRECALC-2017",
     "SourceFamily":"OpenStax Precalculus local ©2017 PDF",
     "SourceUrl":url,
     "SectionOrTask":f"Local PDF SHA256 {INV['sourceSha256']}; {description} Source content supports official CCSS concept, not a publisher-issued standards map or Edulytics final lesson parity.",
     "EvidenceSha256":INV["sourceSha256"],
     "License":"CC BY 4.0"
    }]
   })
 lessons.append(term)
assert sum(len(x["FormalTargets"]) for x in lessons)==2
bp={
 "SchemaVersion":2,
 "BlueprintCode":"US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017",
 "PackCode":"US-CCSS-MATH",
 "VersionCode":"CCSSM-2010","LogicalLevel":0,"CourseCode":"PRECALC",
 "LogicalLevelFrom":13,"LogicalLevelTo":13,
 "SequenceAuthority":"PublisherSourceSequence",
 "SuppressOutcomeFallbackForLogicalRange":True,
 "NativeLevel":"Precalculus","Pathway":"Advanced Precalculus",
 "OfficialAuthority":"Common Core State Standards Initiative",
 "OfficialSourceUrl":"https://corestandards.org/wp-content/uploads/2023/09/ADA-Compliant-Math-Standards.pdf",
 "SourceCheckedAtUtc":NOW,
 "SourceSelectionReason":"User-selected Grade 12 baseline is the local ©2017 OpenStax Precalculus PDF, 12 chapters/73 Sections; do not relabel IM Algebra2, UAE G12 or US Fourth Courses.",
 "SourceSelectionEvidence":"Local source PDF 1156 pages SHA256 "+INV["sourceSha256"]+"; 73 bookmarks and their page hashes captured separately at artifacts/curriculum-delivery/US-CCSS-MATH/G12.",
 "SemanticGraphSha256":h({"bookPdfSha256":INV["sourceSha256"],"lessons":lessons,"units":units}),
 "Sources":[{
  "SourceKey":"OPENSTAX-PRECALC-2017",
  "SourceType":"Open-licensed textbook source",
  "Title":"Precalculus","Publisher":"OpenStax / Rice University",
  "Edition":versions,"RootUrl":url,
  "License":"CC BY 4.0",
  "RequiredDigitalAttribution":"OpenStax, Precalculus (Rice University, local ©2017 edition), CC BY 4.0; book link and license notice.",
  "RightsNote":"General work licensed CC BY 4.0; third-party graphics, external links and trademarks need separate license/credit review before reuse.",
  "EvidenceUrls":[url,"https://creativecommons.org/licenses/by/4.0/"]
 }],
 "AcquisitionDiagnostics":{
  "UnitCount":12,"LessonCount":73,"OfficialStandardCount":2,
  "AddressingCoverageCount":2,"FormalMappingCount":2,
  "LessonsWithoutNumberedGradeReferenceAnyRole":72,
  "LessonsWithoutNumberedAddressingStandard":72,
  "LessonsWithoutNumberedAddressingOrBuildingTowardsStandard":72,
  "MultiStandardLessons":1
 },
 "Units":units,"Lessons":lessons
}
out=ROOT/"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-advanced-precalculus-openstax-2017.lesson-blueprint.json"
assert not out.exists(),f"Prevent overwrite without review: {out}"
out.write_text(json.dumps(bp,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
manifest={
 "Status":"UNVERIFIED_INDEPENDENT_RUNTIME_COURSE_DRAFT",
 "BlueprintCode":bp["BlueprintCode"],
 "CourseCode":bp["CourseCode"],"Pathway":bp["Pathway"],"LogicalLevel":[12,12],
 "LessonCodeCount":len(lessons),"SourcePdfSha256":INV["sourceSha256"],
 "CCSSVerifiedSourceContentOnly":["HSF.IF.A.1","HSF.IF.A.2"],
 "CCSSOtherLessonsUnmapped":72,
 "SemanticParityVerified":False,"TeacherAndStudentRouteVerified":False,
 "Lesson12ChapterStructureVerified":True,"RichV2WrittenLessonCount":1,
 "PracticeDigitalValidatedLessonCount":0,
 "CI":"NOT_RUN","ContentDeployment":"NO_GO",
 "GeneratedUtc":NOW}
(BASE/"g12-runtime-blueprint-draft-state.json").write_text(json.dumps(manifest,indent=2)+"\n",encoding="utf8")
print("G12_BLUEPRINT_CREATED",out,"UNIT",len(units),"LESSONS",len(lessons),"STANDARDS_1ST_LESSON",2,"UNMAPPED_OTHER",72)
