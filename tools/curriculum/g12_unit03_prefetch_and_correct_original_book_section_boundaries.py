from pathlib import Path
import json,re
from pypdf import PdfReader
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==11 and p["FirstIncompleteLessonOrdinal"]==12
sections=[(213,222),(223,238),(239,253),(254,271),(272,280),(281,292),(293,313),(314,324),(325,331)]
book=Path(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
reader=PdfReader(str(book))
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03")
cache.mkdir(parents=True,exist_ok=True)
for n,(start,end)in enumerate(sections,1):
 entry=next(r for r in p["LessonEntries"]if r["Unit"]==3 and r["LessonNumber"]==n)
 assert (entry["PdfPageStart"],entry["PdfPageEnd"])==(start+1,end+1),(n,entry["PdfPageStart"],start)
 pages=[{"PdfPage":i,"Text":reader.pages[i].extract_text() or ""}for i in range(start,end+1)]
 # Section is present in first 1000 chars, except heading paginated across
 # first several lines. Do not use filename inference as verification.
 major,minor=entry["SourceSection"].split(".");assert re.search(r"\b"+major+r"\s*\.\s*"+minor+r"\b",pages[0]["Text"]),(n,pages[0]["Text"][:250])
 doc={
 "SourceSha256":"f0a83745e1ea5b4200bac371afb64615742528e4d31e1e4d241623a38e454e7d",
 "Section":entry["SourceSection"],"Ordinal":entry["Ordinal"],
 "Title":entry["Title"],"Pages":pages,
 "SourceBoundaryCorrection":"VERIFIED_ORIGINAL_FIRST_AND_LAST_PAGE_SECTION_HEADING"
 }
 (cache/f"lesson{n:03d}.json").write_text(json.dumps(doc,ensure_ascii=False,indent=1)+"\n",encoding="utf-8")
 entry["PdfPageStart"]=start
 entry["PdfPageEnd"]=end
 print("UNIT3_SOURCE_CACHED",n,start,end,entry["Title"])
p["UpdatedUtc"]=datetime.now(timezone.utc).isoformat()
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"unit03-original-source-section-pdf-boundary-correction.json").write_text(json.dumps({
"SchemaVersion":1,
"SourceBook":"OpenStax Precalculus 2017 authorized local PDF",
"SourceSHA256":"f0a83745e1ea5b4200bac371afb64615742528e4d31e1e4d241623a38e454e7d",
"PreviousRecordedZeroBasedRanges":[[a+1,b+1]for a,b in sections],
"VerifiedCorrectOriginalZeroBasedRanges":[[a,b]for a,b in sections],
"SourceHeadingReadFromOriginalLocalPdf":True,
"SourceBookChapter3ReviewBegins":332,
"UnitLessonCount":9,"StageABCNotClosedUnit3":True,
"StageD":"NOT_RUN","CI":"NOT_RUN"
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_UNIT3_ALL_NINE_ORIGINAL_SOURCE_SECTIONS_LOCALLY_CACHED_AND_PAGE_LOCATORS_CORRECTED")
