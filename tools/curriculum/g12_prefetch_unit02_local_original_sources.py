from pathlib import Path
import json
from pypdf import PdfReader
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
p=json.loads((root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12/full-grade-progress.json").read_text(encoding="utf-8"))
pdf=Path(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
assert pdf.is_file()
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02")
cache.mkdir(parents=True,exist_ok=True)
reader=PdfReader(str(pdf))
for row in p["LessonEntries"]:
 if row["Unit"]!=2:continue
 name=f"lesson{row['LessonNumber']:03d}.json"
 target=cache/name
 if target.exists():
  j=json.loads(target.read_text(encoding="utf-8"))
  assert len(j["Pages"])==row["PdfPageEnd"]-row["PdfPageStart"]+1
  print("REUSED",name,len(j["Pages"]))
  continue
 pages=[{"PdfPage":n,"Text":reader.pages[n].extract_text() or ""}for n in range(row["PdfPageStart"],row["PdfPageEnd"]+1)]
 assert all(x["Text"].strip()for x in pages)
 doc={"SourceSha256":"f0a83745e1ea5b4200bac371afb64615742528e4d31e1e4d241623a38e454e7d",
      "Section":row["SourceSection"],"Ordinal":row["Ordinal"],"Title":row["Title"],"Pages":pages}
 target.write_text(json.dumps(doc,ensure_ascii=False,indent=1)+"\n",encoding="utf-8")
 print("CACHED",name,row["Title"],len(pages),sum(len(x["Text"])for x in pages))
print("UNIT02_FOUR_LOCAL_SECTIONS_PREFETCHED")
