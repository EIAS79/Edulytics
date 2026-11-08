from pathlib import Path
import json,re,datetime,subprocess,pypdfium2
r=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
b=r/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
f=b/"full-grade-progress.json"
p=json.loads(f.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==35 and p["FirstIncompleteLessonOrdinal"]==36
d=pypdfium2.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
pages=[d[i].get_textpage().get_text_range()for i in range(575,585)]
assert "7.1" in pages[0] and "42." in pages[-1]
starts="\n".join(pages[:8])
examples=[(int(n),s.strip())for n,s in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,100})",starts,re.I)]
tries=sorted({int(x)for x in re.findall(r"Try It\s*#?(\d+)",starts,re.I)})
assert [n for n,_ in examples]==list(range(1,11)) and tries==list(range(1,6))
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
asset=b/"u07-l01-source-inventory.json"
record={"SchemaVersion":1,"Unit":7,"Lesson":1,"SourceSection":"7.1",
"SourceSHA256":p["SourceSnapshotSha256"],"PdfPages":[575,584],
"IncorrectBlueprintPages":[576,585],"WorkedExamples":examples,
"TryIt":tries,"SourceExerciseNumbers":list(range(1,43)),
"ExercisesIndividuallyReviewed":0,"Status":"A_C_IN_PROGRESS",
"ReadyForStageD":False,"CI":"NOT_RUN","StageD":"NOT_RUN",
"BlockingReasons":["42 source mathematical exercises and teacher keys pending",
"Independent worked examples, Try It, visual skill alignment pending",
"Stage C scoring route review pending"],"UpdatedUtc":now}
asset.write_text(json.dumps(record,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
row=p["LessonEntries"][35]
row.update({"ACStatus":"A_C_IN_PROGRESS","PdfPageStart":575,"PdfPageEnd":584,
"ReadyForStageD":False,"EvidencePath":asset.relative_to(r).as_posix(),
"BlockingReasons":record["BlockingReasons"],"UpdatedUtc":now})
p["UpdatedUtc"]=now
f.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
subprocess.run(["git","add","--sparse","-f","--",str(asset.relative_to(r)),str(f.relative_to(r)),str(Path(__file__).relative_to(r))],cwd=r,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=r,check=True)
subprocess.run(["git","commit","-m","docs(g12 u07 l01): verify local source and keep academic closure gate open [skip ci]"],cwd=r,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=r,check=True)
print("U07L01 source inventory saved, lesson not closed; last closed ordinal35")
