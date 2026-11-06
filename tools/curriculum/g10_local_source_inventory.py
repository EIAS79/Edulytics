#!/usr/bin/env python3
"""Read-only local Grade 10 Geometry source metadata extraction.

Run only against the owner's local source archive, not on GitHub Actions.
Headings/URLs/alt text are inventory, NOT evidence of source fidelity,
rights clearance, or of a corresponding Edulytics runtime element.
"""
import hashlib
from html.parser import HTMLParser
import json
import os
from pathlib import Path
import re
import sys
from datetime import datetime, timezone

COUNTS = (22, 15, 16, 11, 18, 17, 14, 11)
ARCHIVE = Path(os.environ.get("G10_LOCAL_ARCHIVE",
    r"C:\Users\khali\Downloads\New folder (2)\Common Core"))
OUT = Path(os.environ.get("G10_SOURCE_INVENTORY_OUT",
    "artifacts/curriculum-delivery/US-CCSS-MATH/G10/g10-local-source-element-inventory-20261106.json"))

class Page(HTMLParser):
    def __init__(self):
        super().__init__()
        self.headers = []
        self.images = []
        self.links = []
        self._header = None
    def handle_starttag(self, name, attrs):
        attributes = dict(attrs)
        if name in ("h1", "h2", "h3", "h4", "h5", "h6"):
            self._header = [name, ""]
        elif name == "img":
            uri = attributes.get("src", "")
            if uri and not uri.endswith("/assets/expand.png"):
                self.images.append({"src": uri, "alt": attributes.get("alt", "")})
        elif name == "a":
            uri = attributes.get("href", "")
            if re.search(r"\.(pdf|svg)(\?|#|$)", uri, re.I):
                self.links.append(uri)
    def handle_data(self, value):
        if self._header is not None:
            self._header[1] += value
    def handle_endtag(self, name):
        if self._header and self._header[0] == name:
            label = " ".join(self._header[1].split())
            if label:
                self.headers.append({"tag": name, "label": label})
            self._header = None

def snapshot(path):
    if not path.is_file():
        return {"available": False}
    contents = path.read_bytes()
    parsed = Page()
    parsed.feed(contents.decode("utf-8", errors="replace"))
    images = list({entry["src"]: entry for entry in parsed.images}.values())
    return {
        "available": True,
        "sha256": hashlib.sha256(contents).hexdigest(),
        "byteCount": len(contents),
        "headings": [{"elementId": f"h-{i+1:03d}", **h}
                     for i, h in enumerate(parsed.headers)],
        "headingCount": len(parsed.headers),
        "visualAssets": [{"elementId": f"img-{i+1:03d}",
                           "src": row["src"], "alt": row["alt"],
                           "assetBytesInspected": False, "rightsVerified": False}
                         for i, row in enumerate(images)],
        "visualAssetCount": len(images),
        "linkedPdfSvgAssets": list(dict.fromkeys(parsed.links)),
        "linkedAssetBytesInspected": False,
    }

def main():
    result = {
        "SchemaVersion": 1,
        "Status": "SOURCE_INVENTORY_ONLY_UNVERIFIED",
        "CourseCode": "GEO",
        "Grade": 10,
        "SourceScope": "Local teacher/student lesson pages and local student practice HTML",
        "SourceLocation": "Owner-supplied local archive (not transferred to CI)",
        "RawSourceTextRedistributed": False,
        "AllAssetsAccessible": "NOT_VERIFIED",
        "ThirdPartyRights": "NOT_VERIFIED",
        "MatchingToEdulytics": "NOT_VERIFIED",
        "CreatedAtUtc": datetime.now(timezone.utc).isoformat(),
        "Lessons": []
    }
    digests = []
    for unit, total in enumerate(COUNTS, 1):
        for lesson in range(1, total+1):
            documents = {
                "TeacherPreparation": ARCHIVE / "10_HS_Geometry" / str(unit) / str(lesson) / "preparation.html",
                "TeacherLesson": ARCHIVE / "10_HS_Geometry" / str(unit) / str(lesson) / "index.html",
                "StudentLesson": ARCHIVE / "10_HS_Geometry_Student" / str(unit) / str(lesson) / "index.html",
                "StudentPractice": ARCHIVE / "10_HS_Geometry_Student" / str(unit) / str(lesson) / "practice.html",
            }
            record = {
                "lessonCode": f"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U{unit:02d}:L{lesson:02d}",
                "unit": unit,
                "lesson": lesson,
                "ordinal": len(result["Lessons"])+1,
                "documents": {},
                "sourceTaskCount": None,
                "mappedVerifiedElementCount": 0,
                "sourceParityStatus": "UNKNOWN",
            }
            for key, file in documents.items():
                row = snapshot(file)
                record["documents"][key] = row
                if row["available"]:
                    digests.append(key+"|"+record["lessonCode"]+"|"+row["sha256"])
            result["Lessons"].append(record)
    result["SourceSnapshotId"] = "g10-local-html-sha256:" + hashlib.sha256(
        "\n".join(digests).encode()).hexdigest()
    result["availableHtmlFiles"] = sum(row["available"]
        for lesson in result["Lessons"] for row in lesson["documents"].values())
    result["expectedHtmlFiles"] = len(result["Lessons"]) * 4
    result["totalHeadingElements"] = sum(row.get("headingCount",0)
        for lesson in result["Lessons"] for row in lesson["documents"].values())
    result["totalDistinctWithinPageVisualAssets"] = sum(row.get("visualAssetCount",0)
        for lesson in result["Lessons"] for row in lesson["documents"].values())
    result["allSourcesAccountedFor"] = result["availableHtmlFiles"] == result["expectedHtmlFiles"]
    OUT.parent.mkdir(parents=True,exist_ok=True)
    OUT.write_text(json.dumps(result, ensure_ascii=False, indent=2)+"\n", encoding="utf-8")
    print(json.dumps({k:result[k] for k in ("SourceSnapshotId","expectedHtmlFiles",
        "availableHtmlFiles","totalHeadingElements",
        "totalDistinctWithinPageVisualAssets","allSourcesAccountedFor")},indent=2))
    return 0 if result["allSourcesAccountedFor"] else 3

if __name__ == "__main__":
    sys.exit(main())
