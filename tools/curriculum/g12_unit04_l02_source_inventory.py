from pathlib import Path
import pypdfium2 as pdfium
import json,re,datetime
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pdf=Path(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
d=pdfium.PdfDocument(str(pdf))
get=lambda i:d[i].get_textpage().get_text_range()
assert "SECTION 4.2" in get(358) and "SECTION 4.3" in get(370)
assert "54." in get(369)
pages=[{"PdfPage":i,"Text":get(i)}for i in range(358,370)]
p_path=base/"full-grade-progress.json"
p=json.loads(p_path.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==21 and p["FirstIncompleteLessonOrdinal"]==22
s={"SourceSha256":p["SourceSnapshotSha256"],"Section":"4.2","Ordinal":22,"Title":"Graphs of Exponential Functions","Pages":pages,"SourceBoundaryCorrection":"PDF zero-based pages 358-369, not blueprint 359-370."}
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit04\lesson002.json")
cache.write_text(json.dumps(s,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
alltext="\n".join(p["Text"]for p in pages)
examples=[(int(n),v.strip())for n,v in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,120})",alltext,re.I)]
tries=sorted({int(x)for x in re.findall(r"Try It\s*#?(\d+)",alltext,re.I)})
figures=sorted({int(x)for x in re.findall(r"Figure\s+(\d+)",alltext,re.I)})
assert [n for n,_ in examples]==list(range(1,7))
assert tries==list(range(1,7)) and figures==list(range(1,14))
families=[("VERBAL",1,2),("TRANSFORMATION_ALGEBRA",3,7),("REFLECT_Y_AXIS",8,10),("COMPARE_EXPONENTIAL",11,12),("MATCH_SOURCE_FIGURE12",13,18),("INTERPRET_SOURCE_FIGURE13",19,22),("REFLECT_X_AXIS",23,25),("GRAPH_TRANSFORMS_DOMAIN_RANGE",26,28),("END_BEHAVIOR",29,31),("PARENT_GRAPH_SHIFTS",32,37),("READ_SOURCE_GRAPH",38,42),("NUMERIC_EVALUATION",43,45),("SOLVE_GRAPHIC_EQUATIONS",46,50),("EXTENSIONS_REFLECTION_TRANSLATION",51,54)]
assert sum(b-a+1 for _,a,b in families)==54
tasks=[{"Number":n,"Family":f,"OriginalSourceMathematicsVerified":False,"IndependentStudentExerciseAuthored":False,"TeacherExpectedMathVerified":False,"SourceGraphicCopied":False}for f,a,b in families for n in range(a,b+1)]
assert [r["Number"]for r in tasks]==list(range(1,55))
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":4,"Lesson":2,"Section":"4.2",
"Title":"Graphs of Exponential Functions","SourceSHA256":s["SourceSha256"],
"OriginalSourcePdfPages":[358,369],"IncorrectBlueprintPdfPages":[359,370],
"OriginalObjectives":["Graph exponential functions","Graph exponential functions using transformations"],
"OriginalWorkedExamples":[{"Number":n,"Title":v}for n,v in examples],
"OriginalTryItNumbers":tries,"OriginalFigureNumbers":figures,
"OriginalNumberedExerciseLedger":tasks,"OriginalExerciseMathVerified":0,
"FirstPartyIndependentExercisesAuthored":0,"OriginalFigureSkillAlternatives":0,
"StageC":"PENDING","ReadyForStageD":False,"Status":"A_C_IN_PROGRESS","StageD":"NOT_RUN","CI":"NOT_RUN",
"BlockingReasons":["SIX_WORKED_AND_SIX_TRYIT_SOURCE_SKILL_ALTERNATIVES_PENDING","54_SOURCE_EXERCISES_REQUIRE_ACADEMIC_AND_INDEPENDENT_TEACHER_REVIEW","13_ORIGINAL_FIGURES_REQUIRE_SKILL_ALTERNATIVES","STAGE_C_TEACHER_MARKING_ROUTING_PENDING"],"StartedUtc":now,"UpdatedUtc":now}
rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l02-source-inventory-corrected-boundary.json"
(base/"u04-l02-source-inventory-corrected-boundary.json").write_text(json.dumps(audit,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
row=p["LessonEntries"][21]
assert row["Ordinal"]==22 and row["SourceSection"]=="4.2"
row.update({"PdfPageStart":358,"PdfPageEnd":369,"ACStatus":"A_C_IN_PROGRESS","ReadyForStageD":False,
"OriginalSourceFigureCount":13,"OriginalSourceExerciseCount":54,"OriginalWorkedExamples":6,"OriginalTryIts":6,
"SourceBoundaryCorrectionVerified":True,"EvidencePath":rel,"BlockingReasons":audit["BlockingReasons"],"UpdatedUtc":now})
p["UpdatedUtc"]=now
p_path.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L02_SOURCE_PAGES_358_369_SIX_EXAMPLES_SIX_TRYITS_13_FIGURES_54_EXERCISES_CORRECTED")
