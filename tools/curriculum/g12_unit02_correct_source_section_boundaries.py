from pathlib import Path
import json
from pypdf import PdfReader
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pdf=Path(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
reader=PdfReader(str(pdf))
correct=[(141,157),(158,176),(177,189),(190,201)]
state=base/"full-grade-progress.json"
p=json.loads(state.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==7 and p["FirstIncompleteLessonOrdinal"]==8
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02")
for n,(start,end)in enumerate(correct,1):
 row=next(x for x in p["LessonEntries"]if x["Unit"]==2 and x["LessonNumber"]==n)
 assert (row["PdfPageStart"],row["PdfPageEnd"])==(start+1,end+1)
 pages=[{"PdfPage":i,"Text":reader.pages[i].extract_text() or ""}for i in range(start,end+1)]
 assert row["SourceSection"]in pages[0]["Text"],(row["SourceSection"],pages[0]["Text"][:100])
 assert "SECTION" in pages[-1]["Text"] or "CHAPTER" in pages[-1]["Text"]
 location=cache/f"lesson{n:03d}.json"
 doc={"SourceSha256":"f0a83745e1ea5b4200bac371afb64615742528e4d31e1e4d241623a38e454e7d",
 "Section":row["SourceSection"],"Ordinal":row["Ordinal"],"Title":row["Title"],"Pages":pages,
 "SourceBoundaryCorrected":"ORIGINAL_SECTION_FIRST_AND_LAST_PAGE_VERIFIED_BY_HEADINGS"}
 location.write_text(json.dumps(doc,ensure_ascii=False,indent=1)+"\n",encoding="utf-8")
 row["PdfPageStart"]=start;row["PdfPageEnd"]=end
 print("LOCAL_SOURCE_BOUNDARY_CORRECTED",n,start,end)
cp=base/"u02-l01-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
assert c["OriginalPDFPages"]==[142,157]
c["OriginalPDFPages"]=[141,157]
c["OriginalSectionBoundaryVerified"]=True
c["OriginalSourcePreviewPageWasErroneouslyOffsetByPlusOne"]=True
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
p["UpdatedUtc"]=datetime.now(timezone.utc).isoformat()
state.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"unit02-local-pdf-page-boundary-correction.json").write_text(json.dumps({
"OriginalSourceBook":"OpenStax Precalculus (2017)",
"OriginalSourceSHA256":"f0a83745e1ea5b4200bac371afb64615742528e4d31e1e4d241623a38e454e7d",
"Unit2SectionSourceBoundaryVerified":True,
"IncorrectPreviousRecordedUnit2PdfPages":[[142,158],[159,177],[178,190],[191,202]],
"CorrectPdfZeroBasedPages":[[141,157],[158,176],[177,189],[190,201]],
"SourcePagesManuallyCheckedBySectionHeadings":True,
"Unit2ReviewBeginsAtZeroBasedPage":202,
"StageD":"NOT_RUN",
"StageABCNotCompleteUnit2":True
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("UNIT02_LOCAL_PDF_PAGE_BOUNDARIES_REPAIRED_ALL_FOUR_SECTIONS")
