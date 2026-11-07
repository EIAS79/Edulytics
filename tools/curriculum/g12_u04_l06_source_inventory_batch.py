from pathlib import Path
import json,datetime,subprocess,time,re,pypdfium2 as pdfium
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pp=base/"full-grade-progress.json"
p=json.loads(pp.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==25 and p["FirstIncompleteLessonOrdinal"]==26
row=p["LessonEntries"][25]
assert row["Ordinal"]==26 and row["SourceSection"]=="4.6" and not row["ReadyForStageD"]
pdf=Path(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
d=pdfium.PdfDocument(str(pdf))
get=lambda i:d[i].get_textpage().get_text_range()
assert "4.6" in get(405) and "SECTION 4.7" in get(416)
pages=[{"PdfPage":i,"Text":get(i)}for i in range(405,416)]
alltext="\n".join(x["Text"]for x in pages)
examples=[(int(n),title.strip())for n,title in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,120})",alltext,re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",alltext,re.I)})
figures=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)",alltext,re.I)})
assert [n for n,title in examples]==list(range(1,14))
assert tries==list(range(1,14)) and figures==list(range(1,4))
assert "81." in get(415)
groups=[("VERBAL",1,3),("LIKE_BASES",4,10),("EXPONENTIAL_EQUATIONS",11,28),
("REWRITE_LOG_EXP",29,30),("DEFINITION_OF_LOG",31,35),
("ONE_TO_ONE_LOG",36,43),("COMBINE_LOG_AND_SOLVE",44,50),
("GRAPHICAL_SOLUTIONS",51,64),("REAL_WORLD_MODELS",65,67),
("TECHNOLOGY_LOG_SOLVE",68,77),("EXTENSIONS",78,81)]
assert sum(b-a+1 for fam,a,b in groups)==81
tasks=[{"Number":n,"SkillFamily":fam,"OriginalSourceMathReviewed":False,
"IndependentStudentQuestionVerified":False,"TeacherMathVerified":False}
for fam,a,b in groups for n in range(a,b+1)]
assert [q["Number"]for q in tasks]==list(range(1,82))
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit04\lesson006.json")
cache.write_text(json.dumps({"Section":"4.6","SourceSha256":p["SourceSnapshotSha256"],"Pages":pages},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":4,"Lesson":6,"Section":"4.6",
"Title":"Exponential and Logarithmic Equations","OriginalSourceLocalPdf":str(pdf),
"CorrectedPdfPages":[405,415],"IncorrectBlueprintPdfPages":[406,416],
"SourceSHA256":p["SourceSnapshotSha256"],
"OriginalWorkedExampleAnchors":[{"Number":n,"Title":title}for n,title in examples],
"OriginalTryItNumbers":tries,"OriginalFigureNumbers":figures,
"SourceExerciseLedger":tasks,"OriginalSourceMathReviewed":0,
"IndependentSourceMathExercises":0,"OriginalFigureSkillsAuthored":0,
"StageC":"PENDING","Status":"A_C_IN_PROGRESS","ReadyForStageD":False,
"StageD":"NOT_RUN","CI":"NOT_RUN","StartedUtc":now,"UpdatedUtc":now,
"BlockingReasons":["81_ORIGINAL_NUMBERED_EXERCISES_REQUIRE_MATH_REVIEW_AND_INDEPENDENT_TEACHER_KEYS",
"13_WORKED_AND_13_TRYIT_ALTERNATIVES_PENDING","3_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_ASSESSMENT_ROUTES_PENDING"]}
asset=base/"u04-l06-source-inventory-and-corrected-boundary.json"
asset.write_text(json.dumps(audit,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
row.update({"PdfPageStart":405,"PdfPageEnd":415,"ACStatus":"A_C_IN_PROGRESS",
"ReadyForStageD":False,"OriginalSourceExercisesLocated":81,
"OriginalWorkedExamplesLocated":13,"OriginalTryItsLocated":13,"OriginalFiguresLocated":3,
"EvidencePath":asset.relative_to(root).as_posix(),"BlockingReasons":audit["BlockingReasons"],"UpdatedUtc":now})
p["UpdatedUtc"]=now
pp.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
files=[str(pp.relative_to(root)),str(asset.relative_to(root)),str(Path(__file__).relative_to(root))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=root,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=root,check=True)
subprocess.run(["git","commit","-m","docs(g12 u04 l06): inventory exact 81 equation exercises and keep academic gate open [skip ci]"],cwd=root,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=root,check=True)
print("U04L06_SOURCE_INVENTORY_PUSH_SUCCESS; 81 exercises, 13 worked, 13 tryits, three figures; open at ordinal 26")
