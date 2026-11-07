from pathlib import Path
import json,datetime,re,pypdfium2 as pdf
r=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
b=r/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
p=json.loads((b/"full-grade-progress.json").read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==39 and p["FirstIncompleteLessonOrdinal"]==40
d=pdf.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
t=lambda i:d[i].get_textpage().get_text_range()
assert "7.5" in t(618) and "7.6" in t(632)
start="\n".join(t(i)for i in range(618,629))
a=[(int(n),v.strip())for n,v in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,100})",start,re.I)]
tr=sorted(set(map(int,re.findall(r"Try It\s*#?(\d+)",start,re.I))))
fig=sorted(set(map(int,re.findall(r"Figure\s+(\d+)","\n".join(t(i)for i in range(618,632)),re.I))))
print("WORKED",a);print("TRY",tr,"FIG",fig)
assert [n for n,_ in a]==list(range(1,20)) and tr==list(range(1,7)) and fig==list(range(1,6))
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
inventory={"SourceSection":"7.5","OriginalSource":"OpenStax Precalculus original local PDF",
"SourceSHA256":p["SourceSnapshotSha256"],"CorrectedPdfPages":[618,631],
"IncorrectBlueprintPages":[619,632],"OriginalWorked":a,"OriginalTryItNumbers":tr,
"OriginalFigureNumbers":fig,"NumberedExercises":list(range(1,107)),
"IndependentExercisesWithTeacherKeys":0,"ReadyForStageD":False,"Status":"A_C_IN_PROGRESS",
"Unresolved":["106 source exercise math skills pending independent teacher answer keys",
"19 worked examples, 6 Try It and 5 figure-skills require first-party alternatives",
"Validate complete solution sets on [0,2π) and restricted trig domains"],
"CI":"NOT_RUN","StageD":"NOT_RUN","UpdatedUtc":now}
path=b/"u07-l05-original-source-inventory.json"
path.write_text(json.dumps(inventory,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U07L05_INVENTORY_VERIFIED_NOT_CLOSED")
