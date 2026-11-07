from pathlib import Path
import json,re,datetime
import pypdfium2 as pdfium
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pdf=Path(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
assert pdf.is_file()
d=pdfium.PdfDocument(str(pdf))
get=lambda i:d[i].get_textpage().get_text_range()
assert "4.1" in get(343) and "Exponential Functions" in get(343)
assert "SECTION 4.2" in get(358)
assert "Technology" in get(357) and "68." in get(357)
source_pages=[{"PdfPage":i,"Text":get(i)}for i in range(343,358)]
s={"SourceSha256":None,"Section":"4.1","Ordinal":21,"Title":"Exponential Functions","Pages":source_pages,"SourceBoundaryCorrection":"PDF zero-based 343–357. Blueprint 344–358 included start of 4.2 and omitted start of 4.1."}
p_path=base/"full-grade-progress.json"
p=json.loads(p_path.read_text(encoding="utf8"))
s["SourceSha256"]=p["SourceSnapshotSha256"]
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit04\lesson001.json")
cache.parent.mkdir(parents=True,exist_ok=True)
cache.write_text(json.dumps(s,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
text="\n".join(x["Text"] for x in source_pages)
examples=[(int(n),title.strip()) for n,title in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,110})",text,re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",text,re.I)})
fig=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)",text,re.I)})
assert [x[0] for x in examples]==list(range(1,13))
assert tries==list(range(1,13)) and fig==list(range(1,7))
assert "68." in text
families=[("VERBAL",1,4),("IDENTIFY_FUNCTION_AND_GROWTH",5,17),("FIT_EXPONENTIAL_POINTS",18,22),("CLASSIFY_TABLES",23,27),("COMPOUND_INTEREST",28,38),("CONTINUOUS_EXPONENTIAL",39,43),("NUMERIC_EVALUATION",44,50),("FIT_TECHNOLOGY_EXPONENTIAL",51,55),("APY_AND_E_EXPONENT_IDENTITY",56,60),("REAL_WORLD_APPLICATIONS",61,68)]
assert sum(hi-lo+1 for _,lo,hi in families)==68
exercise=[{"Number":n,"Family":family,"SourceMathematicsIndividuallyReviewed":False,"FirstPartyIndependentExerciseAuthored":False,"TeacherAnswerVerified":False,"OriginalTextCopied":False}for family,lo,hi in families for n in range(lo,hi+1)]
assert [x["Number"]for x in exercise]==list(range(1,69))
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":4,"Lesson":1,"SourceSection":"4.1","Title":"Exponential Functions","SourceSHA256":s["SourceSha256"],"PdfPages":[343,357],"SourceBoundaryCorrectedFromBlueprint":[344,358],"SourceLocalPdfPath":str(pdf),
"OriginalSourceWorkedExamples":[{"Number":n,"Title":title} for n,title in examples],
"OriginalTryItNumbers":tries,"OriginalFigureNumbers":fig,
"OriginalNumberedExerciseLedger":exercise,"OriginalExerciseMathVerified":0,
"IndependentExerciseTasksAuthored":0,"OriginalFigureSkillAlternatives":0,
"FirstPartyWorkedExamplesAuthored":0,"IndependentTryItsAuthored":0,
"StageC":"PENDING","Status":"A_C_IN_PROGRESS","ReadyForStageD":False,"CI":"NOT_RUN","StageD":"NOT_RUN",
"BlockingReasons":["12_WORKED_EXAMPLE_AND_12_TRYIT_ALTERNATIVES_NOT_YET_AUTHORED","68_NUMBERED_EXERCISES_REQUIRE_INDIVIDUAL_MATH_REVIEW","6_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_TEACHER_ASSESSMENT_REUSE_ROUTING_PENDING"],"StartedUtc":now,"UpdatedUtc":now}
asset=base/"u04-l01-source-inventory-and-corrected-boundary.json"
asset.write_text(json.dumps(audit,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
assert p["LastClosedLessonOrdinal"]==20 and p["FirstIncompleteLessonOrdinal"]==21
row=p["LessonEntries"][20]
assert row["Ordinal"]==21 and row["SourceSection"]=="4.1"
row.update({"PdfPageStart":343,"PdfPageEnd":357,"ACStatus":"A_C_IN_PROGRESS","ReadyForStageD":False,"BlockingReasons":audit["BlockingReasons"],"EvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l01-source-inventory-and-corrected-boundary.json","SourceBoundaryCorrectionVerified":True,"UpdatedUtc":now})
p["UpdatedUtc"]=now
p_path.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L01_LOCAL_SOURCE_PAGES_343_TO_357_12_EXAMPLES_12_TRYIT_6_FIGURES_68_EXERCISES_BOUNDARY_CORRECTED_STATUS_OPEN")
