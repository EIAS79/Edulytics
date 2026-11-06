from __future__ import annotations
from pathlib import Path
import argparse
import hashlib
import json
import os
import tempfile
from datetime import datetime,timezone
from pypdf import PdfReader

def prepare(unit: int) -> None:
    repo = Path(__file__).resolve().parents[2]
    progress = json.loads((repo / "artifacts/curriculum-delivery/US-CCSS-MATH/G12/full-grade-progress.json").read_text(encoding="utf-8"))
    lessons = [v for v in progress["LessonEntries"] if v["Unit"] == unit]
    if not lessons:
        raise ValueError(f"No Grade12 OpenStax lessons found for unit {unit}")
    fallback = r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf"
    pdf = Path(os.environ.get("G12_PRECALCULUS_PDF", fallback))
    if not pdf.is_file():
        raise FileNotFoundError(f"OpenStax local PDF unavailable: {pdf}")
    source_sha = hashlib.sha256(pdf.read_bytes()).hexdigest()
    expected_sha="f0a83745e1ea5b4200bac371afb64615742528e4d31e1e4d241623a38e454e7d"
    if source_sha != expected_sha:
        raise RuntimeError("Unexpected source PDF edition/hash; no cache written")
    cache = Path(tempfile.gettempdir()) / "edulytics-g12-precalculus-pdf-cache" / f"unit{unit:02d}"
    manifest = cache / "manifest.json"
    if manifest.is_file():
        previous=json.loads(manifest.read_text(encoding="utf-8"))
        if previous.get("SourceSha256")==source_sha and all(Path(v["CacheFile"]).is_file() for v in previous["Lessons"]):
            print(f"UNIT_{unit:02d}_SOURCE_CACHE_REUSED {len(previous['Lessons'])} lessons")
            return
    cache.mkdir(parents=True,exist_ok=True)
    reader=PdfReader(str(pdf))
    rows=[]
    for lesson in lessons:
        pages=[{"PdfPage":i+1,"Text":reader.pages[i].extract_text() or ""} for i in range(lesson["PdfPageStart"]-1,lesson["PdfPageEnd"])]
        data={"SourceSha256":source_sha,"Section":lesson["SourceSection"],"Ordinal":lesson["Ordinal"],"Title":lesson["Title"],"Pages":pages}
        output=cache/f"lesson{lesson['Ordinal']:03d}.json"
        output.write_text(json.dumps(data,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
        rows.append({"Ordinal":lesson["Ordinal"],"CacheFile":str(output),"PageCount":len(pages)})
    manifest.write_text(json.dumps({"SourceSha256":source_sha,"Unit":unit,"CreatedUtc":datetime.now(timezone.utc).isoformat(),"Lessons":rows},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
    print(f"UNIT_{unit:02d}_SOURCE_CACHE_READY {len(rows)} lessons (local only)")

if __name__=="__main__":
    parser=argparse.ArgumentParser(description="One-pass local PDF section cache; never copies copyrighted source into GitHub")
    parser.add_argument("--unit",type=int,default=1,help="grade12 chapter number (1..12)")
    args=parser.parse_args()
    if not 1<=args.unit<=12:
        parser.error("--unit must be between 1 and 12")
    prepare(args.unit)
