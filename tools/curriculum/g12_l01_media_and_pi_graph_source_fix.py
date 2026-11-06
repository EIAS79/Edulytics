from pathlib import Path
from datetime import datetime,timezone
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
coveragepath=base/"u01-l01-element-coverage.json"
coverage=json.loads(coveragepath.read_text(encoding="utf-8"))
source_media=[e for e in coverage["Elements"] if e["Category"]=="MEDIA"]
assert len(source_media)==5 and coverage["Count"]==169
links=[
("Determine if a Relation is a Function","http://openstaxcollege.org/l/relationfunction","relation versus function"),
("Vertical Line Test","http://openstaxcollege.org/l/vertlinetest","single-valued relations from graphs"),
("Introduction to Functions","http://openstaxcollege.org/l/introtofunction","function definition and notation"),
("Vertical Line Test of Graph","http://openstaxcollege.org/l/vertlinegraph","vertical-line classification on plotted relations"),
("One-to-one Functions","http://openstaxcollege.org/l/onetoone","injective function rule"),
("Graphs as One-to-one Functions","http://openstaxcollege.org/l/graphonetoone","horizontal-line testing on function graphs")
]
now=datetime.now(timezone.utc).isoformat()
for i,(label,url,objective) in enumerate(links):
    row=source_media[i] if i<5 else dict(source_media[0])
    if i==5:
        row["SourceElementId"]="G12-U01-L01-MEDIA006"
        row["SourceLocator"]=dict(source_media[0]["SourceLocator"])
        coverage["Elements"].append(row)
    assert row["SourceElementId"]==f"G12-U01-L01-MEDIA{i+1:03d}"
    row["SourceLabel"]=label
    row["OriginalPdfPrintedUrl"]=url
    row["OriginalLearningSkill"]=objective
    row["SourceLocator"]["PdfPage"]=33
    row["RightsStatus"]="EXTERNAL_DESTINATION_RIGHTS_AND_CURRENT_ACCESS_NOT_CHECKED"
    row["Status"]="SOURCE_VERIFIED"
    row["EdulyticsArtifactPath"]=None
    row["ImplementationType"]="SUPPORT_SOURCE_LINK_NOT_VERIFIED_AS_DIGITAL_DELIVERY"
    row["ExpectedMathematics"]=objective
    row["VerificationMethod"]="READ_ONLY_LOCAL_PDF_PAGE_33_LINK_TITLES_AND_PRINTED_URLS"
    row["Reviewer"]="LOCAL_PDF_SOURCE_REVIEW"
    row["UpdatedUtc"]=now
coverage["Count"]=len(coverage["Elements"])
assert coverage["Count"]==170
coverage["OriginalSourceMediaResourceCount"]=6
coverage["OriginalSourceMediaPdfPage"]=33
coverage["SourceMediaResourceCoverageVerified"]=False
coverage["SourceInventory"]="PARTIAL_ENUMERATION_NARRATIVE_MATH_GLYPHS_AND_VISUALS_PENDING; SIX_MEDIA_LINKS_IDENTIFIED_FROM_PDF_PAGE_33"
coverage["AuthoringEvidence"]["SourceMediaLinksIdentified"]=6
coverage["AuthoringEvidence"]["SourceSemanticMatchesCertified"]=0
coverage["SourceAtomicDenominatorVerified"]=False
coveragepath.write_text(json.dumps(coverage,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

routingpath=base/"u01-l01-exercise-routing.json"
routing=json.loads(routingpath.read_text(encoding="utf-8"))
ex59=next(x for x in routing["SourceExerciseContracts"] if x["OriginalExerciseNumber"]==59)
assert ex59["SourceGraphDescription"] is not None
if "π" not in ex59["SourceGraphDescription"]:
    ex59["SourceGraphDescription"]+=" The source drawing labels horizontal positions in multiples of π, not as ordinary integer distances; preserve this variable/axis interpretation."
ex59["SourceHorizontalAxisLabels"]=["−π","−π/2","0","π/2","π"]
ex59["FirstPartyGraphAxisStatus"]="SKILL_ONLY_AXES_NOT_SEMANTICALLY_IDENTICAL_TO_SOURCE_PI_RADIAN_SCALE"
ex59["SemanticParityStatus"]="UNMATCHED"
routing["OriginalExercise59PiAxisIdentified"]=True
routing["OriginalExercise59SourcePdfPage"]=36
routingpath.write_text(json.dumps(routing,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

bankpath=base/"u01-l01-first-party-exercise-bank.json"
bank=json.loads(bankpath.read_text(encoding="utf-8"))
item=bank["Items"][58]
assert item["Number"]==59
item["Prompt"]+=" The printed source example uses horizontal-angle labels in multiples of π. When inspecting the original, preserve those tick labels rather than treating the horizontal axis as ordinary integer units."
item["SourceXAxisPiTicksRequired"]=True
bankpath.write_text(json.dumps(bank,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

csfile=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection11Exercises.cs"
src=csfile.read_text(encoding="utf-8")
old="new(59, \"Function and one-to-one tests\","
assert src.count(old)==1
lines=src.splitlines()
for i,line in enumerate(lines):
    if old in line:
        assert line.count("TEACHER_DIAGRAM_REQUIRED")==1 and line.endswith('),')
        original=bank["Items"][58]["Prompt"]
        # Only the question's string literal, not answer key, changes.
        replacement=json.dumps(original,ensure_ascii=False)
        start=line.index(old)+len(old)
        qidx=line.index('\"',start)
        marker='\", \"TEACHER_DIAGRAM_REQUIRED\"'
        end=line.index(marker,qidx)
        line=line[:qidx]+replacement+line[end+1:]
        lines[i]=line
        break
csfile.write_text("\n".join(lines)+"\n",encoding="utf-8",newline="\n")

graph=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection11ExerciseVisuals.cs"
s=graph.read_text(encoding="utf-8")
old='"Repeated outputs on the flat part or either side of the peak defeat the horizontal-line test."'
new='"This shape-only model uses numeric x-ticks; the original exercise uses π-radian x-labels, which must be shown in the teacher-inspected source graph. Repeated outputs defeat the horizontal-line test."'
assert s.count(old)==1
graph.write_text(s.replace(old,new),encoding="utf-8",newline="\n")

guidepath=base/"u01-l01-first-party-exercise-bank-teacher-review.json"
guide=json.loads(guidepath.read_text(encoding="utf-8"))
key="40-59"
assert key in guide["Rules"]
guide["Rules"][key]+=" For original exercise 59, use a π-labelled horizontal axis; the authored generic numeric-axis sketch is not source-figure equivalent."
guidepath.write_text(json.dumps(guide,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

cp_path=base/"u01-l01-abc-checkpoint.json"
cp=json.loads(cp_path.read_text(encoding="utf-8"))
cp["OriginalSourceMediaResourceCountIdentified"]=6
cp["Exercise59PiXAxisSourceReview"]="SOURCE_VERIFIED_FIRST_PARTY_NUMERIC_AXIS_NOT_EQUIVALENT"
cp["SourceAtomicDenominatorVerified"]=False
cp["ReadyForStageD"]=False
cp_path.write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
p_path=base/"full-grade-progress.json"
pg=json.loads(p_path.read_text(encoding="utf-8"))
pg["LessonEntries"][0]["OriginalSourceMediaResourceCountIdentified"]=6
pg["LessonEntries"][0]["Exercise59PiXAxisStatus"]="SOURCE_NOT_FULLY_REPRODUCED"
pg["LessonEntries"][0]["SourceAtomicDenominatorVerified"]=False
pg["LessonEntries"][0]["ReadyForStageD"]=False
p_path.write_text(json.dumps(pg,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("MEDIA_SOURCE_COUNT_FIXED_5_TO_6; EX59_PI_AXIS_DOCUMENTED_NO_FALSE_PARITY")
