from pathlib import Path
import re,json,datetime,time,subprocess
import pypdfium2 as pdfium
start=time.monotonic()
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
progresspath=base/"full-grade-progress.json"
p=json.loads(progresspath.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==23 and p["FirstIncompleteLessonOrdinal"]==24
row=p["LessonEntries"][23]
assert row["Ordinal"]==24 and row["SourceSection"]=="4.4" and not row["ReadyForStageD"]
pdfpath=Path(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
d=pdfium.PdfDocument(str(pdfpath))
get=lambda i:d[i].get_textpage().get_text_range()
assert "SECTION 4.4" in get(378) and "4.5" in get(395)
pages=[{"PdfPage":i,"Text":get(i)}for i in range(378,395)]
text="\n".join(x["Text"]for x in pages)
worked=[{"Number":int(n),"Title":name.strip()}for n,name in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,130})",text,re.I)]
tries=sorted(set(map(int,re.findall(r"Try It\s*#?(\d+)",text,re.I))))
fig=sorted(set(map(int,re.findall(r"Figure\s+(\d+)",text,re.I))))
assert [x["Number"]for x in worked]==list(range(1,12))
assert tries==list(range(1,12)) and fig==list(range(1,20))
assert "60." in get(394)
groups=[("VERBAL",1,2),("DOMAINS_AND_GRAPH_PROPERTIES",3,10),("TRANSFORM_LOG_FUNCTION",11,25),
("SOURCE_FIGURE_17_INTERPRET",26,30),("SOURCE_FIGURE_18_INTERPRET",31,35),
("LOG_GRAPH_RENDERING",36,50),("GRAPHICAL_SOLVE_EQUATIONS",51,55),("EXTENSIONS",56,60)]
# Coverage of independently indexed source exercises; no false proof of individual mathematical review.
assert sum(b-a+1 for _,a,b in groups)==60
ledger=[{"Number":n,"SourceSkillFamily":fam,"SourceMathematicsIndividuallyReviewed":False,
"IndependentMathExerciseAuthored":False,"TeacherExpectedMathVerified":False}for fam,a,b in groups for n in range(a,b+1)]
assert [x["Number"]for x in ledger]==list(range(1,61))
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit04\lesson004.json")
cache.write_text(json.dumps({"Section":"4.4","SourceSha256":p["SourceSnapshotSha256"],"Pages":pages},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Section":"4.4","Unit":4,"Lesson":4,
"Title":"Graphs of Logarithmic Functions","OriginalSourceLocalPDF":str(pdfpath),
"SourceSHA256":p["SourceSnapshotSha256"],"CorrectedPdfPages":[378,394],
"IncorrectBlueprintPdfPages":[379,395],"OriginalWorkedExamples":worked,
"OriginalTryItNumbers":tries,"OriginalFigureNumbers":fig,
"OriginalNumberedExerciseLedger":ledger,"OriginalExerciseMathVerified":0,
"IndependentSourceSkillExercises":0,"IndependentFigureSkillAlternatives":0,
"StageC":"PENDING","ReadyForStageD":False,"CI":"NOT_RUN","StageD":"NOT_RUN",
"Status":"A_C_IN_PROGRESS","BlockingReasons":["60_NUMBERED_EXERCISES_REQUIRE_INDIVIDUAL_MATH_REVIEW_AND_FIRSTPARTY_TASKS",
"11_WORKED_AND_11_TRYIT_TEACHER_CONTENT_PENDING",
"19_SOURCE_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_TEACHER_MARKING_ROUTES_PENDING"],
"StartedUtc":now,"UpdatedUtc":now}
asset=base/"u04-l04-source-inventory-corrected-boundary.json"
asset.write_text(json.dumps(audit,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
row.update({"ACStatus":"A_C_IN_PROGRESS","PdfPageStart":378,"PdfPageEnd":394,
"ReadyForStageD":False,"OriginalWorkedExamplesLocated":11,
"OriginalTryItsLocated":11,"OriginalFigureSkillsLocated":19,"OriginalExercisesLocated":60,
"EvidencePath":asset.relative_to(root).as_posix(),"BlockingReasons":audit["BlockingReasons"],"UpdatedUtc":now})
p["UpdatedUtc"]=now
progresspath.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
files=[str(progresspath.relative_to(root)),str(asset.relative_to(root)),str(Path(__file__).relative_to(root))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=root,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=root,check=True)
subprocess.run(["git","commit","-m","docs(g12 u04 l04): index exact local logarithmic graph source and preserve open gate [skip ci]"],cwd=root,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=root,check=True)
print("U04_L04_SOURCE_INVENTORY_PUSH_SUCCESS", "11 worked, 11 tryits, 19 figures, 60 tasks, still open", "elapsed",round(time.monotonic()-start,2))
