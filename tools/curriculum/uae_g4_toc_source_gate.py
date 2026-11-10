#!/usr/bin/env python3
"""Fail-closed Grade 4 2026-27 local student TOC consistency audit; not Stage D."""
import argparse
import hashlib
import json
import re
from pathlib import Path

ROOT=Path(__file__).resolve().parents[2]
ART=ROOT/"artifacts/curriculum-delivery/UAE-MOE-MATH/G4"
COUNTS=[6,9,8,11,6,11,9,10,9,11,10,7,3,9,5,8]
PRIMARY_SHA="54760d6eb2483624e94417f63bcf5e3590b4cc46f288b9df13cc7cc0923e5a69"

def validate(local=False):
    index=json.loads((ART/"grade4-2026-2027-toc-132.json").read_text(encoding="utf-8"))
    lock=json.loads((ART/"source-lock.json").read_text(encoding="utf-8"))
    progress=json.loads((ART/"full-grade-progress.json").read_text(encoding="utf-8"))
    migration=json.loads((ART/"grade4-edition-migration-map.json").read_text(encoding="utf-8"))
    units=index["Units"]
    lessons=index["Lessons"]
    assert len(units)==16 and [u["LessonCount"] for u in units]==COUNTS
    assert len(lessons)==132 and sum(COUNTS)==132
    assert len({x["LessonCode"] for x in lessons})==132
    assert all(index[k]==v for k,v in [
        ("Framework","UAE-MOE-MATH"),("Grade",4),("Pathway","Common"),
        ("AcademicLanguage","ar"),("TargetEdition","2026-2027")])
    for pos,row in enumerate(lessons,1):
        assert row["Ordinal"]==pos
        u=row["Unit"]
        l=row["UnitLessonOrdinal"]
        assert 1<=u<=16 and row["UnitTitleAr"]==units[u-1]["TitleAr"]
        assert row["TocPdfPage"]==u+7
        assert row["LessonCode"]==f"PED:UAE-MOE-MATH:L4:COMMON:{u:02}:{l:02}"
        assert row["TitleAr"].strip() and row["TocSourceSha256"]==PRIMARY_SHA
        if u<=6:
            assert row["StudentPrintedStartPage"] is not None
            assert row["StudentPdfStartPage2026_2027"]==row["StudentPrintedStartPage"]+23
            assert row["StudentBodyEditionStatus"]=="2026_2027_STUDENT_PAGES_AVAILABLE"
        else:
            assert row["StudentPdfStartPage2026_2027"] is None
            assert row["StudentBodyEditionStatus"]=="SOURCE_EDITION_GAP"
        if u>=10:
            assert row["StudentPrintedStartPage"] is None
    assert lock["PrimarySourceSha256"]==PRIMARY_SHA
    assert lock["LatestBodySourceConfirmedLessons"]==51
    assert lock["BodySourceEditionGaps"]==81
    assert progress["TargetLessons"]==132 and progress["TargetUnits"]==16
    assert progress["A_B_C_Ready"]==0 and progress["FirstUnclosedLessonOrdinal"]==1
    assert len(migration["OldLessons"])==93 and len(migration["TargetLessons"])==132
    old={x["OldLessonCode"] for x in migration["OldLessons"]}
    assert len(old)==93
    assert all(x["MigrationDecision"]=="UNVERIFIED" for x in migration["OldLessons"])
    if local:
        p=Path(lock["PrimarySource"])
        assert p.is_file(),f"Missing actual selected PDF: {p}"
        h=hashlib.sha256()
        with p.open("rb") as fp:
            for chunk in iter(lambda:fp.read(1<<20),b""):
                h.update(chunk)
        assert h.hexdigest()==PRIMARY_SHA
    print("G4_TOC_SOURCE_LOCK_PASS units=16 lessons=132 current_edition_bodies=51 source_gaps=81 old_legacy=93 A_B_C_ready=0")

if __name__=="__main__":
    ap=argparse.ArgumentParser()
    ap.add_argument("--require-local-pdf",action="store_true")
    opts=ap.parse_args()
    validate(local=opts.require_local_pdf)
