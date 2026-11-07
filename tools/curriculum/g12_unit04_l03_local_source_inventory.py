from pathlib import Path
import json,re,datetime
import pypdfium2 as pdfium
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pdf=Path(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
d=pdfium.PdfDocument(str(pdf))
get=lambda i:d[i].get_textpage().get_text_range()
assert "SECTION 4.3" in get(370) and "SECTION 4.4" in get(378)
assert "66." in get(377)
p_path=base/"full-grade-progress.json"
p=json.loads(p_path.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==22 and p["FirstIncompleteLessonOrdinal"]==23
pages=[{"PdfPage":i,"Text":get(i)}for i in range(370,378)]
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit04\lesson003.json")
cache.write_text(json.dumps({"SourceSha256":p["SourceSnapshotSha256"],"Section":"4.3","Ordinal":23,
"Title":"Logarithmic Functions","Pages":pages,"SourceBoundaryCorrection":"PDF zero-based370-377, blueprint371-378 off by one."},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
txt="\n".join(x["Text"]for x in pages)
examples=[(int(n),title.strip())for n,title in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,140})",txt,re.I)]
fig=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)",txt,re.I)})
tryraw=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",txt,re.I)})
assert [n for n,_ in examples]==list(range(1,9))
assert fig==[1,2] and tryraw==[1,3,4,5,6,7,8,20]
# Local PDF text extraction glues a trailing zero onto Try It #2: OCR output is "#20" near Example 2.
tries=list(range(1,9))
categories=[("VERBAL",1,5),("CONVERT_LOG_TO_EXPONENTIAL",6,15),("CONVERT_EXP_TO_LOG",16,25),
("SOLVE_USING_LOG_DEFINITION",26,35),("SIMPLIFY_LOG_EXP_INVERSES",36,41),("EVALUATE_LOG_BY_BASE",42,45),
("COMMON_LOG",46,49),("NATURAL_LOG",50,53),("TECHNOLOGY_LOG_EVALUATION",54,58),
("DOMAIN_AND_LOG_EXTENSIONS",59,63),("REAL_WORLD_LOG_MODELS",64,66)]
assert sum(hi-lo+1 for _,lo,hi in categories)==66
tasks=[{"Number":n,"SkillFamily":family,"OriginalSourceMathReviewed":False,"IndependentStudentQuestionAuthored":False,"TeacherRubricVerified":False,"PublisherGraphicCopied":False}for family,lo,hi in categories for n in range(lo,hi+1)]
assert [r["Number"]for r in tasks]==list(range(1,67))
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":4,"Lesson":3,"Section":"4.3",
"Title":"Logarithmic Functions","SourceSha256":p["SourceSnapshotSha256"],
"CorrectedLocalPdfPages":[370,377],"IncorrectBlueprintPages":[371,378],
"OriginalLearningObjectives":["Convert logarithmic to exponential form","Convert exponential to logarithmic form",
"Evaluate logarithms","Use common logarithms","Use natural logarithms"],
"OriginalWorkedExamples":[{"Number":n,"Title":title}for n,title in examples],
"OriginalTryItNumbers":tries,"OriginalTryItTextExtractionException":"Original local PDF extracted #20 at Example 2 position, normalized to #2 from sequence.",
"OriginalFigureNumbers":fig,"OriginalFigureRightsNote":"Original Figure 1 is a credited disaster photograph, must not be copied as a generic mathematical illustration.",
"OriginalNumberedExerciseLedger":tasks,"OriginalSourceMathReviewed":0,"FirstPartyIndependentExercisesAuthored":0,
"FirstPartyFigureSkillAlternatives":0,"StageC":"PENDING","ReadyForStageD":False,
"Status":"A_C_IN_PROGRESS","CI":"NOT_RUN","StageD":"NOT_RUN",
"BlockingReasons":["8_WORKED_AND_8_TRYIT_SOURCE_SKILL_ALTERNATIVES_PENDING",
"66_NUMBERED_LOG_EXERCISES_REQUIRE_INDIVIDUAL_MATH_AND_FIRSTPARTY_TEACHER_KEYS",
"2_SOURCE_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_TEACHER_MARKING_ROUTES_PENDING"],
"StartedUtc":now,"UpdatedUtc":now}
asset=base/"u04-l03-source-inventory-and-corrected-boundary.json"
asset.write_text(json.dumps(audit,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
row=p["LessonEntries"][22]
assert row["Ordinal"]==23 and row["SourceSection"]=="4.3" and not row["ReadyForStageD"]
row.update({"PdfPageStart":370,"PdfPageEnd":377,"ACStatus":"A_C_IN_PROGRESS","ReadyForStageD":False,
"SourceBoundaryCorrectionVerified":True,"EvidencePath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l03-source-inventory-and-corrected-boundary.json",
"OriginalWorkedExamplesLocated":8,"OriginalTryItsLocated":8,"OriginalFiguresLocated":2,
"OriginalExercisesLocated":66,"BlockingReasons":audit["BlockingReasons"],"UpdatedUtc":now})
p["UpdatedUtc"]=now
p_path.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L03_SOURCE_8_WORKED_8_TRYITS_2_FIGURES_66_EXERCISES_CORRECTED_BOUNDARY_PAGES_370_377_OPEN")
