#!/usr/bin/env python3
"""Local-only OpenStax Precalculus source locator inventory for Grade 12 A-B-C.
PDF is read-only. This enumerates bookmarked section boundaries and *candidate*
items. OCR/text extraction never certifies an exhaustive source denominator.
Not a Stage D test and not a source/asset parity certificate.
"""
import argparse,collections,datetime,hashlib,json,re
from pathlib import Path
from pypdf import PdfReader

COURSE="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017"
UNIT_COUNTS=[7,4,9,8,4,3,6,8,8,5,7,4]
TITLES=[
 "Functions","Linear Functions","Polynomial and Rational Functions",
 "Exponential and Logarithmic Functions","Trigonometric Functions",
 "Periodic Functions","Trigonometric Identities and Equations",
 "Further Applications of Trigonometry","Systems of Equations and Inequalities",
 "Analytic Geometry","Sequences, Probability and Counting Theory",
 "Introduction to Calculus"
]
def outline_flat(reader):
    rows=[]
    def walk(nodes,depth=0):
        for node in nodes:
            if isinstance(node,list):walk(node,depth+1)
            else:
                try:page=reader.get_destination_page_number(node)+1
                except Exception:continue
                rows.append({"title":str(node.title),"page":page,"depth":depth})
    walk(reader.outline)
    return rows
def sha_file(p):
    with p.open("rb") as f:return hashlib.file_digest(f,"sha256").hexdigest()
def main():
    parser=argparse.ArgumentParser()
    parser.add_argument("--pdf",required=True)
    parser.add_argument("--output",required=True)
    parser.add_argument("--max-sections",type=int,default=73)
    a=parser.parse_args()
    src=Path(a.pdf).resolve(strict=True)
    pdf=PdfReader(str(src))
    outlines=outline_flat(pdf)
    section_re=re.compile(r"^\s*(\d{1,2})\s*\.\s*(\d{1,2})\.?\s+(.+?)\s*$")
    sections=[]; chapters=[]; resources=[]
    for i,node in enumerate(outlines):
        m=section_re.match(node["title"])
        if m:
            sections.append({**node,"chapter":int(m.group(1)),"section":int(m.group(2)),"sourceTitle":m.group(3)})
        elif node["depth"]==0 and node["title"].startswith("Chapter "):
            chapters.append(node)
    assert len(pdf.pages)==1156,len(pdf.pages)
    assert len(sections)==73,len(sections)
    assert len(chapters)==12,len(chapters)
    for chapter,expected in enumerate(UNIT_COUNTS,1):
        nodes=[s for s in sections if s["chapter"]==chapter]
        assert len(nodes)==expected,(chapter,len(nodes),expected)
        assert [s["section"] for s in nodes]==list(range(1,expected+1)),chapter
    snapshot=sha_file(src)
    lookup={(s["chapter"],s["section"]):s for s in sections}
    entries=[];ordinal=0
    # Text marker extraction is approximate, not an acceptance-grade question audit.
    for chapter,expected in enumerate(UNIT_COUNTS,1):
      for section in range(1,expected+1):
        ordinal+=1
        row=lookup[(chapter,section)]
        nextSection=(lookup.get((chapter,section+1)) or next(
          (x for x in outlines if x["page"]>row["page"] and
           ((x["title"].strip().lower()=="glossary" and x["depth"]==1) or
            x["depth"]==0 and x["title"].startswith("Chapter "))),None))
        if not nextSection:raise ValueError("Missing next bookmark after "+str(row))
        last=nextSection["page"]-1
        assert row["page"]<=last and last<=1156
        markers=collections.defaultdict(list);page_hashes=[];raw_len=0
        for pdf_page in range(row["page"],last+1):
            page=pdf.pages[pdf_page-1]
            body=page.extract_text() or ""
            raw_len+=len(body)
            page_hashes.append({"pdfPage":pdf_page,"extractedTextSha256":hashlib.sha256(body.encode("utf8")).hexdigest(),"textCharacters":len(body)})
            for kind,pattern in [
              ("EXAMPLE",r"(?i)\bExample\s+(\d{1,2})\b"),
              ("TRY",r"(?i)\bTry\s+It\s*#?\s*(\d{1,2})\b"),
              ("FIG",r"(?i)\bFigure\s+(\d{1,3})\b"),
              ("TABLE",r"(?i)\bTable\s+(\d{1,3})\b"),
              ("HOW_TO",r"(?i)\bHow\s+To[\.\s…]*"),
              ("ANALYSIS",r"(?i)\bAnalysis\b"),
              ("Q_AND_A",r"(?i)\bQ\s*&\s*A\b"),
            ]:
              for m in re.finditer(pattern,body):
                key=m.group(1) if m.lastindex else "un-numbered"
                if {"pdfPage":pdf_page,"marker":str(key)} not in markers[kind]:
                    markers[kind].append({"pdfPage":pdf_page,"marker":str(key)})
            if re.search(r"(?i)section\s+exercises",body):
                markers["EXERCISE_SECTION_PAGES"].append(pdf_page)
                # Exercise lines may contain several problem numbers per line. This is a
                # candidate list; a human must resolve superscript/figure ambiguities.
                for m in re.finditer(r"(?<![a-zA-Z0-9])(\d{1,3})\.(?=\s|$)",body):
                    num=int(m.group(1))
                    if num>0:
                        ref={"pdfPage":pdf_page,"number":num}
                        if ref not in markers["EXERCISE_CANDIDATES"]:
                            markers["EXERCISE_CANDIDATES"].append(ref)
        code=f"{COURSE}:U{chapter:02d}:L{section:02d}"
        entries.append({
          "ordinal":ordinal,"lessonCode":code,"unitNumber":chapter,
          "lessonNumber":section,"sourceSection":f"{chapter}.{section}",
          "sourceTitle":row["sourceTitle"],
          "pdfPageStart":row["page"],"pdfPageEndInclusive":last,
          "pageCount":last-row["page"]+1,
          "pageTextHashes":page_hashes,
          "sourceSnapshotSha256":snapshot,"textCharacterCount":raw_len,
          "candidateMarkers":dict(markers),
          "extractionCaveat":"Text extraction loses graphical geometry, math fonts and some multi-column problem boundaries; candidate marker counts are NOT complete source element counts.",
          "sourceDenominatorVerified":False,"semanticParityVerified":False,
          "mediaRightsVerified":False,"digitalPracticeVerified":False,
          "aCStatus":"A_C_PENDING"
        })
        print(f"SOURCE_INDEXED {ordinal:02d}/73 SECTION {chapter}.{section} PDF {row['page']}-{last}",flush=True)
        if ordinal>=a.max_sections:break
      if ordinal>=a.max_sections:break
    for i,ch in enumerate(chapters):
        nxt=chapters[i+1]["page"] if i<len(chapters)-1 else 1093
        lastSection=lookup[(i+1,UNIT_COUNTS[i])]
        ends=[s["page"] for s in outlines if s["page"]>lastSection["page"] and s["page"]<nxt and s["depth"]==1 and s["title"].lower()=="glossary"]
        sharedStart=ends[0] if ends else None
        resources.append({"chapter":i+1,"chapterIntroPdfPage":ch["page"],
                          "sharedChapterMaterialsStartPdfPage":sharedStart,
                          "sharedChapterMaterialsEndPdfPage":nxt-1,
                          "resourceKindsToCheck":["Glossary","Key Equations","Key Concepts","Chapter Review","Review Exercises","Practice Test"]})
    out={
      "schemaVersion":1,"course":"US-CCSS-MATH:G12","status":"SOURCE_LOCATOR_INVENTORY_NOT_ACADEMIC_ACCEPTANCE",
      "sourceFileName":src.name,"sourcePathReadOnly":str(src),"sourceSha256":snapshot,
      "sourceVersion":"OpenStax Precalculus, Rice University ©2017, Revision PR-2015-002(03/17)-BW, original 2014",
      "rights":"CC BY 4.0 International; check all embedded third-party figures and external links independently",
      "originalPublicationYear":2014,"fileCopyrightYear":2017,
      "pdfPages":len(pdf.pages),"expectedSections":73,"indexedSections":len(entries),
      "unitCounts":UNIT_COUNTS,"unitTitles":TITLES,"sectionRows":entries,
      "chapterResourceLocators":resources,
      "answerAppendix":"Try It Answers and Odd Answers start at PDF bookmarks after page 1093; verify per section independently",
      "caveat":"No exhaustive atomic 1:1 mapping or rights clearance performed; never mark READY_FOR_STAGE_D merely from this index.",
      "createdAtUtc":datetime.datetime.now(datetime.timezone.utc).isoformat()
    }
    dest=Path(a.output);dest.parent.mkdir(parents=True,exist_ok=True)
    dest.write_text(json.dumps(out,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
    print("COMPLETE_SOURCE_INDEX",len(entries),"SHA256",snapshot,"WRITTEN",str(dest),flush=True)
if __name__=="__main__":main()
