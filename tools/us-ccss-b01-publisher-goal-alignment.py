#!/usr/bin/env python3
"""B01 official publisher purpose vs *effective* learner text, 500 at once.

This is a content-review queue, not a certification authority.
Only reads allowlisted original IM K-5 teacher preparation URLs.
Does not store/republish the publisher's full lesson pages.
"""
from __future__ import annotations

import argparse
from concurrent.futures import ThreadPoolExecutor, as_completed
import hashlib
from html.parser import HTMLParser
import json
import os
import pathlib
import re
import time
from urllib.parse import urlparse
from urllib.request import Request, urlopen
from urllib.error import URLError, HTTPError

ROOT = pathlib.Path(__file__).resolve().parents[1]
MANIFEST = ROOT / "artifacts/academic-us-ccss/batches/B01-lesson-manifest.json"
STOP = set(("the a an and or in on from with to of for this that they their into is are was "
            "be as by at it we our students student teacher teachers lesson lessons "
            "learn learning use using make making will can have has two one based "
            "way ways number numbers understand understanding purpose goal goals "
            "work working show showing write writing mathematics mathematical math "
            "find finding solve solving solve").split())
TOKENS = re.compile(r"[a-z]{4,}")
STANDARDS = re.compile(r"\b(?:K|[1-8])\.(?:OA|NBT|MD|G|NF)\.[A-Z]\.\d+\b", re.I)

class MainText(HTMLParser):
    def __init__(self):
        super().__init__(convert_charrefs=True)
        self.depth = 0
        self.skip = 0
        self.fragments = []
    def handle_starttag(self, tag, attrs):
        if tag == "main": self.depth += 1
        if tag in ("script","style","noscript"): self.skip += 1
        if self.depth and tag in ("p","h1","h2","h3","h4","li","br"):
            self.fragments.append("\n")
    def handle_endtag(self, tag):
        if tag == "main": self.depth = max(0,self.depth - 1)
        if tag in ("script","style","noscript"): self.skip = max(0,self.skip - 1)
        if self.depth and tag in ("p","h1","h2","h3","h4","li"):
            self.fragments.append("\n")
    def handle_data(self, data):
        if self.depth > 0 and self.skip == 0:
            self.fragments.append(data)

def parse_publisher(html: str) -> dict:
    p = MainText(); p.feed(html)
    lines = [re.sub(r"\s+"," ",x).strip() for x in
             "".join(p.fragments).splitlines()]
    lines = [x for x in lines if x]
    def section(start, ends):
        i = next((i for i,s in enumerate(lines) if s.lower() == start.lower()),-1)
        if i == -1:return ""
        result=[]
        for s in lines[i+1:]:
            if s.lower() in [e.lower() for e in ends]: break
            result.append(s)
        return " ".join(result)
    purpose = section("Lesson Purpose",["Lesson Narrative","Learning Goals","Required Materials"])
    narrative = section("Lesson Narrative",["Learning Goals","Required Materials","CCSS Standards"])
    goals = section("Learning Goals",["Required Materials","Required Preparation","CCSS Standards",
                                     "Lesson Timeline","Suggested Centers"])
    std = section("CCSS Standards",["Lesson Timeline","Teacher Reflection Questions",
                                     "Suggested Centers","Print Formatted Materials"])
    title = ""
    for i,s in enumerate(lines[:8]):
        if re.fullmatch(r"Lesson\s+\d+",s) and i+1<len(lines):
            title = lines[i+1]
            break
    return {"title":title,"purpose":purpose,"narrative":narrative,
            "goals":goals,"standards":sorted(set(STANDARDS.findall(std)))}

def words(s):
    return {w for w in TOKENS.findall(s.lower()) if w not in STOP}

def align_row(item, actual, original):
    problems=[]
    title = re.sub(r"\s+PLC Activity$","",item["title"],flags=re.I).strip()
    source_title = re.sub(r"\s+PLC Activity$","",original["title"],flags=re.I).strip()
    if source_title and title.lower().replace("’","'") != source_title.lower().replace("’","'"):
        problems.append("SOURCE_TITLE_REVIEW")
    expected = {x.replace("CCSS:","").upper() for x in item["officialOutcomeCodes"]}
    official = {x.upper() for x in original["standards"]}
    # Some lesson preparation pages specify "Building On/Towards" rather than
    # "Addressing". A code being mentioned is not proof of direct targeting.
    if official and not expected.issubset(official):
        problems.append("SOURCE_OFFICIAL_CODES_MISMATCH_CANDIDATE")
    if not original["purpose"] or not original["goals"]:
        problems.append("MISSING_PUBLISHER_PURPOSE_OR_GOALS")
    actualwords=words(actual.get("learnerTeachingText") or "")
    goalwords=words(original["purpose"]+" "+original["goals"])
    matched=goalwords.intersection(actualwords)
    overlap=round(len(matched)/max(1,len(goalwords)),3)
    if len(goalwords)>=5 and overlap<0.18:
        problems.append("SOURCE_GOAL_INSTRUCTIONAL_ALIGNMENT_REVIEW")
    if actual.get("sourcePreparationUrl") != item["sourceUrl"]:
        problems.append("EFFECTIVE_SOURCE_URL_MISMATCH")
    return {
      "ordinal":item["ordinal"],"lessonCode":item["lessonCode"],
      "sourceUrl":item["sourceUrl"],
      "effectiveContentSha256":actual.get("effectiveContentSha256"),
      "publisherTitle":original["title"],
      "publisherPurposeDigestSha256":hashlib.sha256(original["purpose"].encode()).hexdigest(),
      "publisherGoalDigestSha256":hashlib.sha256(original["goals"].encode()).hexdigest(),
      "sourcePurposePreview":original["purpose"][:180],
      "officialSourceCodes":original["standards"],
      "sourceGoalKeywordCount":len(goalwords),
      "goalKeywordOverlap":overlap,
      "missingGoalKeywords":sorted(goalwords-actualwords)[:30],
      "issues":problems,
      "verdict":"REVIEW_REQUIRED" if problems else "SOURCE_SCREENED_NOT_CERTIFIED"
    }

def allowed_source_url(url):
    p=urlparse(url)
    return (p.scheme=="https" and p.netloc=="im.kendallhunt.com"
            and bool(re.fullmatch(r"/k5/teachers/grade-[1-4]/unit-\d+/lesson-\d+/preparation\.html",p.path))
            and not p.query and not p.fragment)

def fetch_source(url):
    if not allowed_source_url(url): raise ValueError("Source URL is not allowlisted")
    cache_folder = os.getenv('USCCSS_B01_SOURCE_CACHE')
    cache_file = (pathlib.Path(cache_folder) / (hashlib.sha256(url.encode()).hexdigest()+'.json')
                  if cache_folder else None)
    if cache_file is not None and cache_file.exists():
        try:
            cached=json.loads(cache_file.read_text())
            if (cached.get('originalUrl')==url and
                0 <= time.time()-cached.get('fetchedAtUnix',0)<86400 and
                cached.get('data',{}).get('purpose') and
                cached.get('data',{}).get('title')):
                return cached['data']
        except (OSError, ValueError, TypeError):
            pass
    last=None
    for retry in range(3):
        try:
            req=Request(url,headers={"User-Agent":"Edulytics-CCSS-Research/1.0 (+https://github.com/EIAS79/Edulytics)",
                                     "Accept":"text/html"})
            with urlopen(req,timeout=25) as res:
                if not allowed_source_url(res.url): raise ValueError("Publisher redirected outside allowlist")
                html=res.read(750000).decode("utf-8","replace")
                result=parse_publisher(html)
                if not result["purpose"]:
                    raise ValueError("Publisher preparation lacks visible Lesson Purpose")
                if cache_file is not None:
                    cache_file.parent.mkdir(parents=True,exist_ok=True)
                    cache_file.write_text(json.dumps({
                        'originalUrl':url,
                        'fetchedAtUnix':time.time(),
                        'data':result
                    },ensure_ascii=False))
                return result
        except (HTTPError,URLError,TimeoutError,ValueError) as exc:
            last=str(exc)[:240]
            if retry<2:time.sleep(1.5*(retry+1))
    raise RuntimeError(last)

# IM 2021 optional Grade 2 U02 L16 explicitly does not publish separate
# Learning Goals, but has a detailed publisher Teacher Lesson activity.
# Only this exact publisher-owned page can fill the alternative activity slot.
OPTIONAL_ACTIVITY_LESSON_CODE = "PED:US-CCSS-MATH:G2:U02:L16"
OPTIONAL_ACTIVITY_URL = (
    "https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-16/lesson.html"
)

def check_optional_market_activity(html: str):
    parser = MainText()
    parser.feed(html)
    narrative = re.sub(r"\s+", " ", "".join(parser.fragments)).lower()
    required = ["3 kinds of items", "100 items", "inventory"]
    if not all(phrase in narrative for phrase in required):
        raise ValueError("Official optional market activity missing required stock/transaction constraints")
    return {
        "sourceUrl": OPTIONAL_ACTIVITY_URL,
        "activityHtmlSha256": hashlib.sha256(html.encode("utf-8")).hexdigest(),
        "publisherActivityConstraints": [
            "THREE_ITEM_KINDS", "TOTAL_100_STOCK", "SALES_INVENTORY"
        ],
        "goalSectionSource": "OPTIONAL_LESSON_HAS_PURPOSE_AND_TEACHER_ACTIVITY_NO_SEPARATE_GOALS",
        "status": "OFFICIAL_ACTIVITY_VERIFIED_NOT_CERTIFIED"
    }

def fetch_optional_market_activity():
    request = Request(OPTIONAL_ACTIVITY_URL, headers={
        "User-Agent": "Edulytics-CCSS-Research/1.0 (+https://github.com/EIAS79/Edulytics)",
        "Accept": "text/html"
    })
    with urlopen(request, timeout=25) as response:
        if response.url != OPTIONAL_ACTIVITY_URL:
            raise ValueError("Unexpected publisher activity redirect")
        body = response.read(850000).decode("utf-8", "replace")
    return check_optional_market_activity(body)

def run(manifest, effective, fetcher, activity_fetcher=None):
    items=manifest["lessons"]
    rows=effective["lessonRows"]
    if len(items)!=500 or len(rows)!=500 or manifest["batchId"]!="B01":
        raise ValueError("500-lesson B01 input scope mismatch")
    by_code={r["lessonCode"]:r for r in rows}
    if len(by_code)!=500 or set(by_code)!={x["lessonCode"] for x in items}:
        raise ValueError("Effective runtime sample does not cover 500 manifest codes")
    sources={}; errors=[]
    urls=sorted(set(x["sourceUrl"] for x in items))
    # Bounded concurrency; no broad website crawl and no arbitrary URLs.
    with ThreadPoolExecutor(max_workers=4) as executor:
        tasks={executor.submit(fetcher,url):url for url in urls}
        for idx,future in enumerate(as_completed(tasks),1):
            url=tasks[future]
            try:sources[url]=future.result()
            except Exception as e:errors.append({"url":url,"reason":str(e)[:240]})
            if idx%50==0:print(f"Fetched official preparations: {idx}/{len(urls)}",flush=True)
    # G2 U02 L16 is the publisher's optional lesson with no Learning Goals
    # heading. An independently checked official Teacher Lesson, NOT an
    # invented teacher-facing goal, is used for alternative provenance.
    activity_evidence = None
    optional_errors = []
    optional_item = next((item for item in items if
        item["lessonCode"] == OPTIONAL_ACTIVITY_LESSON_CODE), None)
    if optional_item is not None:
        original_optional = sources.get(optional_item["sourceUrl"])
        if original_optional and original_optional["purpose"] and not original_optional["goals"]:
            try:
                activity_evidence = (activity_fetcher or fetch_optional_market_activity)()
            except (HTTPError, URLError, TimeoutError, ValueError) as exc:
                optional_errors.append(str(exc)[:240])

    matrix=[]
    for item in items:
        original=sources.get(item["sourceUrl"])
        if original is None:
            matrix.append({"ordinal":item["ordinal"],"lessonCode":item["lessonCode"],
                           "issues":["PUBLISHER_SOURCE_NOT_FETCHED"],"verdict":"BLOCKED"})
            continue
        row = align_row(item, by_code[item["lessonCode"]], original)
        if item["lessonCode"] == OPTIONAL_ACTIVITY_LESSON_CODE and not original["goals"]:
            row["publisherGoalSection"] = "NOT_PUBLISHED_FOR_OPTIONAL_LESSON"
            if activity_evidence:
                row["officialOptionalActivitySha256"] = activity_evidence["activityHtmlSha256"]
                row["publisherActivityBasis"] = activity_evidence["goalSectionSource"]
                row["issues"] = [issue for issue in row["issues"] if
                    issue != "MISSING_PUBLISHER_PURPOSE_OR_GOALS"]
                row["verdict"] = ("SOURCE_SCREENED_NOT_CERTIFIED" if not row["issues"] else
                    "REVIEW_REQUIRED")
            else:
                row["issues"].append("OPTIONAL_LESSON_ACTIVITY_EVIDENCE_NOT_FETCHED")
        matrix.append(row)
    problems={}
    for row in matrix:
        for issue in row["issues"]:
            problems[issue]=problems.get(issue,0)+1
    return {
      "kind":"B01_500_OFFICIAL_PUBLISHER_VS_EFFECTIVE_LEARNER_ALIGNMENT",
      "expected":500,"observed":len(matrix),
      "distinctPublisherUrls":len(urls),"fetchedPublisherUrls":len(sources),
      "sourceFetchFailures":errors,"optionalSourceErrors":optional_errors,
      "optionalPublisherActivityEvidence":activity_evidence,
      "issueCounts":dict(sorted(problems.items())),
      "rows":matrix,"academicDecision":"NOT_CERTIFIED",
      "note":"This reads all 500 official lesson goals but keyword overlap is a triage heuristic, not L1-L6 PASS. Full publisher lesson activities, semantic math, visual rendering, practice alignment and eight dimensions require independent confirmation."
    }

def main():
    a=argparse.ArgumentParser()
    a.add_argument("--effective-json",required=True)
    a.add_argument("--out",default="b01-publisher-alignment-500.json")
    args=a.parse_args()
    manifest=json.loads(MANIFEST.read_text())
    effective=json.loads(pathlib.Path(args.effective_json).read_text())
    report=run(manifest,effective,fetch_source)
    pathlib.Path(args.out).write_text(json.dumps(report,indent=2,ensure_ascii=False)+"\n")
    print(json.dumps({k:v for k,v in report.items() if k not in ("rows","sourceFetchFailures")},indent=2))
    if report["fetchedPublisherUrls"]!=report["distinctPublisherUrls"]:
        raise SystemExit("B01 publisher source fetch incomplete; academic release blocked.")
if __name__=="__main__":main()
