#!/usr/bin/env python3
"""B01: inspect the REAL teacher Lesson activities, not preparation goals alone.

Exactly 500 original lesson URLs are joined to effective Rich V2 and Practice
evidence. Signals are review leads, NEVER L1-L6 PASS or automatic defects.
Only topic labels, short activity headings, counts and digests are retained.
"""
from __future__ import annotations

import argparse
from concurrent.futures import ThreadPoolExecutor, as_completed
from hashlib import sha256
from html.parser import HTMLParser
import json
import os
from pathlib import Path
import re
import time
from urllib.parse import urlparse
from urllib.request import Request, urlopen

ROOT = Path(__file__).resolve().parents[1]
MANIFEST = ROOT / "artifacts/academic-us-ccss/batches/B01-lesson-manifest.json"
PREP_RE = re.compile(r"/k5/teachers/grade-[1-4]/unit-\d+/lesson-\d+/preparation\.html")
FULL_RE = re.compile(r"/k5/teachers/grade-[1-4]/unit-\d+/lesson-\d+/lesson\.html")

# These are *candidate* instructional motifs in the original full teacher
# lesson; teacher-side narrative can mention an optional representation.
CUES = (
    ("DOT_IMAGES", r"\bdot (?:images?|cards?|cubes?|pictures?)\b", ("PlaceValueChart", "TenFrame")),
    ("NUMBER_LINE", r"\bnumber line\b|\bline plots?\b", ("NumberLine", "Scale", "LinePlot")),
    ("FRACTION_PARTS", r"\bfraction (?:bars?|strips?|models?)\b|\bshaded parts?\b",
     ("FractionBars", "ShapeDecomposition")),
    ("CLOCK_FACE", r"\bclock (?:face|hands?)\b|\banalog clock\b", ("ClockFace",)),
    ("PLACE_VALUE", r"\bplace.value chart\b|\bbase.ten (?:blocks?|drawings?)\b",
     ("PlaceValueChart",)),
    ("RECTANGULAR_ARRAY", r"\barrays?\b|\bunit squares?\b",
     ("ShapeDecomposition", "RectangleDiagram", "PlaceValueChart")),
    ("MEASUREMENT_SCALE", r"\brulers?\b|\bmeasurement scale\b",
     ("Scale", "NumberLine")),
    ("DATA_GRAPH", r"\bbar graphs?\b|\bpicture graphs?\b",
     ("BarGraph", "Scale", "RatioTable", "PictureGraph")),
)

class LessonText(HTMLParser):
    def __init__(self):
        super().__init__(convert_charrefs=True)
        self.main_depth = 0
        self.skip_depth = 0
        self.heading_level = None
        self.heading_text = []
        self.headings = []
        self.fragments = []
    def handle_starttag(self, tag, attrs):
        if tag == "main":
            self.main_depth += 1
        if self.main_depth:
            if tag in ("script", "style", "noscript"):
                self.skip_depth += 1
            elif tag == "h2" and not self.skip_depth:
                self.heading_level = tag
                self.heading_text = []
            if tag in ("p", "li", "h1", "h2", "h3", "br"):
                self.fragments.append(" ")
    def handle_endtag(self, tag):
        if self.heading_level == tag:
            value = re.sub(r"\s+", " ", "".join(self.heading_text)).strip()
            if value:
                self.headings.append(value)
            self.heading_level = None
        if tag in ("script", "style", "noscript") and self.skip_depth:
            self.skip_depth -= 1
        if tag == "main":
            self.main_depth = max(0, self.main_depth - 1)
    def handle_data(self, data):
        if self.main_depth and not self.skip_depth:
            self.fragments.append(data)
            if self.heading_level:
                self.heading_text.append(data)

def source_lesson_url(preparation_url):
    parsed = urlparse(preparation_url)
    if (parsed.scheme != "https" or parsed.netloc != "im.kendallhunt.com"
            or not PREP_RE.fullmatch(parsed.path) or parsed.query or parsed.fragment):
        raise ValueError("Unexpected official preparation URL")
    return preparation_url.replace("/preparation.html", "/lesson.html")

def parse_full_activity(html):
    parser = LessonText()
    parser.feed(html)
    text = re.sub(r"\s+", " ", "".join(parser.fragments))
    # Only the real HTML main body is inspected, not navigation and scripts.
    headings = [h for h in parser.headings
                if re.match(r"^(Warm-up|Activity\s+\d+\b|Lesson Synthesis|Cool-down)", h, re.I)]
    activities = [h for h in headings if re.match(r"^Activity\s+\d+\b", h, re.I)]
    signals = []
    for label, pattern, compatible_kinds in CUES:
        if re.search(pattern, text, re.I):
            signals.append({"cue": label, "expectedVisualKinds": list(compatible_kinds)})
    return {
        "teacherLessonHtmlSha256": sha256(html.encode("utf-8")).hexdigest(),
        "normalizedMainTextSha256": sha256(text.encode("utf-8")).hexdigest(),
        "activityCount": len(activities),
        "hasWarmUp": any(h.lower().startswith("warm-up") for h in headings),
        "hasCoolDown": any(h.lower().startswith("cool-down") for h in headings),
        "hasLessonSynthesis": any(h.lower().startswith("lesson synthesis") for h in headings),
        "activityHeadingDigests": [sha256(h.encode("utf-8")).hexdigest() for h in activities],
        "instructionalRepresentationCues": signals,
        "hasReadableLessonBody": bool(text.strip()) and bool(headings),
    }

def fetch_full(url):
    parsed = urlparse(url)
    if parsed.scheme != "https" or parsed.netloc != "im.kendallhunt.com" or not FULL_RE.fullmatch(parsed.path):
        raise ValueError("Rejected teacher Lesson URL")
    cache_dir = os.getenv("USCCSS_B01_TEACHER_LESSON_CACHE")
    cache_file = Path(cache_dir) / (sha256(url.encode()).hexdigest() + ".json") if cache_dir else None
    if cache_file is not None and cache_file.is_file():
        try:
            record = json.loads(cache_file.read_text(encoding="utf-8"))
            if (record["url"] == url and
                time.time() - record["fetchedAt"] < 86400 and
                record["parsed"]["hasReadableLessonBody"]):
                return record["parsed"]
        except (KeyError, OSError, ValueError, TypeError):
            pass
    last_error = None
    for attempt in range(2):
        try:
            request = Request(url, headers={"User-Agent":"Edulytics-CCSS-Research/1.0",
                                            "Accept":"text/html"})
            with urlopen(request, timeout=20) as response:
                if response.url != url:
                    raise ValueError("Unexpected publisher URL redirect")
                html = response.read(1200000).decode("utf-8", "replace")
            parsed = parse_full_activity(html)
            if not parsed["hasReadableLessonBody"]:
                raise ValueError("Missing full official teacher activity text")
            if cache_file is not None:
                cache_file.parent.mkdir(parents=True, exist_ok=True)
                cache_file.write_text(json.dumps({"url":url,"fetchedAt":time.time(),
                                                   "parsed":parsed}),encoding="utf-8")
            return parsed
        except (OSError, ValueError, TimeoutError) as exc:
            last_error = str(exc)[:240]
            if attempt == 0:
                time.sleep(0.5)
    raise RuntimeError(last_error)

def load_preindexed_source(lessons, metadata):
    """Reconcile independently hashed full original teacher HTML metadata.

    Provenance is the locally archived teacher Lesson bytes. This makes
    repeat GitHub checks network-free while preserving all 500 exact source
    identities. Metadata is REVIEW EVIDENCE, never a gate PASS.
    """
    required_columns = [
        "ordinal", "lessonCode", "teacherLessonHtmlSha256",
        "normalizedMainTextSha256", "activityCount",
        "activityHeadingsAggregateSha256", "sectionFlags",
        "representationCueBits",
    ]
    if (metadata.get("batchId") != "B01" or
            metadata.get("kind") != "B01_500_ORIGINAL_TEACHER_LESSON_LOCAL_SOURCE_METADATA" or
            metadata.get("columns") != required_columns or
            metadata.get("representationCueKeys") != [cue[0] for cue in CUES] or
            len(metadata.get("rows", [])) != 500 or len(lessons) != 500):
        raise ValueError("Exact 500-source original teacher index schema mismatch")
    sha = re.compile(r"^[a-f0-9]{64}$")
    fetched = {}
    for ordinal, (item, values) in enumerate(zip(lessons, metadata["rows"]), 1):
        if not isinstance(values, list) or len(values) != len(required_columns):
            raise ValueError("Invalid teacher source index row shape: " + str(ordinal))
        row_ord, code, raw_sha, main_sha, count, heading_sha, flags, cue_bits = values
        if row_ord != ordinal or code != item["lessonCode"]:
            raise ValueError("Stale/reordered local original teacher source: " + str(ordinal))
        if not all(isinstance(v, str) and sha.fullmatch(v)
                   for v in (raw_sha, main_sha, heading_sha)):
            raise ValueError("Missing teacher source SHA256: " + code)
        if (not isinstance(count, int) or isinstance(count, bool) or count < 1
                or not isinstance(flags, int) or isinstance(flags, bool)
                or not 0 <= flags <= 7
                or not isinstance(cue_bits, int) or isinstance(cue_bits, bool)
                or not 0 <= cue_bits < (1 << len(CUES))):
            raise ValueError("Invalid teacher Lesson activity flags: " + code)
        url = source_lesson_url(item["sourceUrl"])
        if url in fetched:
            raise ValueError("Duplicate original teacher Lesson source: " + url)
        fetched[url] = {
            "teacherLessonHtmlSha256": raw_sha,
            "normalizedMainTextSha256": main_sha,
            "activityCount": count,
            "activityHeadingDigests": [],
            "activityHeadingsAggregateSha256": heading_sha,
            "hasWarmUp": bool(flags & 1),
            "hasCoolDown": bool(flags & 2),
            "hasLessonSynthesis": bool(flags & 4),
            "instructionalRepresentationCues": [
                {"cue": cue[0], "expectedVisualKinds": list(cue[2])}
                for i, cue in enumerate(CUES) if cue_bits & (1 << i)
            ],
            "hasReadableLessonBody": True,
        }
    if len(fetched) != 500:
        raise ValueError("500 source activity index has missing original teacher pages")
    return fetched


def analyze(manifest, runtime, rich, fetcher, source_index=None):
    lessons = manifest.get("lessons") or []
    runtime_rows = runtime.get("lessonRows") or []
    rich_rows = rich.get("rows") or []
    if manifest.get("batchId") != "B01" or len(lessons) != 500 or len(runtime_rows) != 500 or len(rich_rows) != 500:
        raise ValueError("B01 requires exactly 500 manifest, effective runtime and Rich V2 rows")
    def map_unique(items, label):
        result = {r["lessonCode"]:r for r in items}
        if len(result) != 500:
            raise ValueError("Missing or duplicate "+label+" identity")
        return result
    r_by = map_unique(runtime_rows, "runtime")
    v_by = map_unique(rich_rows, "Rich V2")
    expected = {x["lessonCode"] for x in lessons}
    if len(expected) != 500 or set(r_by) != expected or set(v_by) != expected:
        raise ValueError("Missing or unexpected B01 LessonCode")
    urls = [source_lesson_url(l["sourceUrl"]) for l in lessons]
    if len(set(urls)) != 500:
        raise ValueError("Duplicate official source URL")
    fetched, fetch_errors = {}, []
    if source_index is not None:
        fetched = load_preindexed_source(lessons, source_index)
    else:
        with ThreadPoolExecutor(max_workers=8) as pool:
            futures = {pool.submit(fetcher, url):url for url in urls}
            for future in as_completed(futures):
                url=futures[future]
                try:
                    fetched[url] = future.result()
                except Exception as exc:
                    fetch_errors.append({"url":url,"reason":str(exc)[:200]})
    rows=[]
    for index, item in enumerate(lessons, 1):
        code=item["lessonCode"]
        actual=r_by[code]
        rv=v_by[code]
        if item.get("ordinal") != index or actual.get("ordinal") != index or rv.get("ordinal") != index:
            raise ValueError("B01 source-runtime-Rich ordinal mismatch: "+code)
        full_url = source_lesson_url(item["sourceUrl"])
        parsed=fetched.get(full_url)
        body=actual.get("effectiveContentSha256")
        rich_body=rv.get("effectiveBodyFingerprint")
        if not body or body != rich_body:
            raise ValueError("Effective content body fingerprint mismatch: "+code)
        actual_kinds=set(rv.get("visualKinds") or [])
        review=[]
        if parsed:
            for entry in parsed["instructionalRepresentationCues"]:
                if not actual_kinds.intersection(entry["expectedVisualKinds"]):
                    review.append(entry["cue"])
        else:
            review.append("SOURCE_TEACHER_LESSON_UNAVAILABLE")
        rows.append({
            "ordinal":index,"lessonCode":code,"officialTeacherLessonUrl":full_url,
            "sourceSemanticSha256":item["sourceSemanticSha256"],
            "effectiveContentSha256":body,
            "teacherLessonHtmlSha256":parsed["teacherLessonHtmlSha256"] if parsed else None,
            "activityCount":parsed["activityCount"] if parsed else None,
            "activityHeadingDigests":parsed["activityHeadingDigests"] if parsed else [],
            "activityHeadingsAggregateSha256":parsed.get("activityHeadingsAggregateSha256") if parsed else None,
            "instructionalRepresentationCues": [x["cue"] for x in
                parsed["instructionalRepresentationCues"]] if parsed else [],
            "actualRichV2VisualKinds":sorted(actual_kinds),
            "candidateMissingRepresentations":review,
            "actualQuestionFamilies":actual.get("actualQuestionFamilies") or [],
            "status":"SOURCE_ACTIVITY_SCREENED_NOT_CERTIFIED" if parsed
                else "BLOCKED_SOURCE_ACTIVITY_NOT_FETCHED"
        })
    from collections import Counter
    counts=Counter(k for r in rows for k in r["candidateMissingRepresentations"])
    return {
        "schemaVersion":1,"kind":"B01_500_OFFICIAL_TEACHER_ACTIVITIES_VS_EFFECTIVE_LEARNER",
        "expected":500,"inspected":len(rows),"teacherPagesFetched":len(fetched),
        "sourceFetchFailures":fetch_errors,
        "sourceAcquisitionMode":"ARCHIVED_SHA256_METADATA" if source_index is not None else "PUBLISHER_HTTP",
        "withReviewCues":sum(bool(r["candidateMissingRepresentations"]) for r in rows),
        "reviewCueTypes":dict(sorted(counts.items())),
        "rows":rows,"academicDecision":"NOT_CERTIFIED",
        "warning":"Full teacher activities supply stronger review leads than preparation keywords; "
                  "a teacher narrative cue alone is not a confirmed defect or L1-L6 PASS. "
                  "Source-to-learner semantic, visual pixels, assessment and L6 remain separate evidence."
    }

def main():
    ap=argparse.ArgumentParser()
    ap.add_argument("--runtime", required=True)
    ap.add_argument("--rich", required=True)
    ap.add_argument("--source-index", required=True,
                    help="B01 original archived teacher Lesson SHA256 metadata; no HTTP")
    ap.add_argument("--output",default="b01-original-teacher-activities-500.json")
    args=ap.parse_args()
    report=analyze(json.loads(MANIFEST.read_text()),
                   json.loads(Path(args.runtime).read_text()),
                   json.loads(Path(args.rich).read_text()),fetch_full,
                   source_index=json.loads(Path(args.source_index).read_text()))
    Path(args.output).write_text(json.dumps(report,indent=2)+"\n")
    print(json.dumps({k:v for k,v in report.items() if k not in ("rows","sourceFetchFailures")},indent=2))
    if report["teacherPagesFetched"] != 500:
        raise SystemExit("B01 original teacher lesson activity coverage incomplete")
if __name__ == "__main__":
    main()
