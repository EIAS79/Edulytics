#!/usr/bin/env python3
"""American Grade 12 is logical 13, not 12 (which is Grade 11)."""
from pathlib import Path
import json
root=Path(__file__).resolve().parents[2]
core=root/"src/Edulytics.Core"
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def fix_one(path,old,new):
 s=path.read_text(encoding="utf8")
 assert s.count(old)==1,(path,old,s.count(old))
 path.write_text(s.replace(old,new),encoding="utf8")
 return str(path.relative_to(root))
changed=[]
file=core/"Curriculum/CurriculumLevelIdentity.cs"
changed.append(fix_one(file,'course.Code == "PRECALC" && level.LogicalLevel != 12','course.Code == "PRECALC" && level.LogicalLevel != 13'))
file=core/"Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-advanced-precalculus-openstax-2017.lesson-blueprint.json"
bp=json.loads(file.read_text(encoding="utf8"))
assert (bp["LogicalLevelFrom"],bp["LogicalLevelTo"])==(12,12)
bp["LogicalLevelFrom"]=13;bp["LogicalLevelTo"]=13
file.write_text(json.dumps(bp,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
changed.append(str(file.relative_to(root)))
file=root/"tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
changed.append(fix_one(file,'Assert.Equal(12, blueprint.LogicalLevelFrom);','Assert.Equal(13, blueprint.LogicalLevelFrom);'))
changed.append(fix_one(file,'Assert.Equal(12, blueprint.LogicalLevelTo);','Assert.Equal(13, blueprint.LogicalLevelTo);'))
file=root/"tests/Edulytics.Tests/Phase29/Phase29ExplicitCurriculumLevelIdentityTests.cs"
changed.append(fix_one(file,'var expectedCount = logical == 12 ? 10 : 9;','var expectedCount = logical == 13 ? 10 : 9;'))
changed.append(fix_one(file,'.Where(course => course.Code != "PRECALC" || logical == 12);','.Where(course => course.Code != "PRECALC" || logical == 13);'))
changed.append(fix_one(file,'Assert.Equal(12, level.LogicalLevel);','Assert.Equal(13, level.LogicalLevel);'))
changed.append(fix_one(file,'BuildKey(code, 12, "Advanced Precalculus", "PRECALC")','BuildKey(code, 13, "Advanced Precalculus", "PRECALC")'))
changed.append(fix_one(file,'item => item.CourseCode == "PRECALC" && item.LogicalLevel != 12','item => item.CourseCode == "PRECALC" && item.LogicalLevel != 13'))
file=root/"tools/curriculum/g12_author_course_blueprint.py"
changed.append(fix_one(file,'"LogicalLevelFrom":12,"LogicalLevelTo":12,','"LogicalLevelFrom":13,"LogicalLevelTo":13,'))
file=root/"tools/curriculum/g12_course_identity_richv2_patch.py"
changed.append(fix_one(file,'course.Code == "PRECALC" && level.LogicalLevel != 12','course.Code == "PRECALC" && level.LogicalLevel != 13'))
file=root/"tools/curriculum/g12_adoption_contract_test_patch.py"
s=file.read_text(encoding="utf8")
for old,new in [
 ("var expectedCount = logical == 12 ? 10 : 9;","var expectedCount = logical == 13 ? 10 : 9;"),
 ('.Where(course => course.Code != "PRECALC" || logical == 12);','.Where(course => course.Code != "PRECALC" || logical == 13);'),
 ('Assert.Equal(12, level.LogicalLevel);','Assert.Equal(13, level.LogicalLevel);'),
 ('BuildKey(code, 12, "Advanced Precalculus", "PRECALC")','BuildKey(code, 13, "Advanced Precalculus", "PRECALC")'),
 ('item => item.CourseCode == "PRECALC" && item.LogicalLevel != 12','item => item.CourseCode == "PRECALC" && item.LogicalLevel != 13'),
 ('Assert.Equal(12, blueprint.LogicalLevelFrom);','Assert.Equal(13, blueprint.LogicalLevelFrom);'),
 ('Assert.Equal(12, blueprint.LogicalLevelTo);','Assert.Equal(13, blueprint.LogicalLevelTo);')
 ]:
 assert s.count(old)==1,(old,s.count(old))
 s=s.replace(old,new)
file.write_text(s,encoding="utf8")
changed.append(str(file.relative_to(root)))
file=root/"tools/curriculum/g12_source_catalogue_checkpoint.py"
changed.append(fix_one(file,'"LogicalLevelCandidate":12,','"LogicalLevelCandidate":13,'))
file=base/"g12-precalculus-blueprint-source-candidate.json"
j=json.loads(file.read_text(encoding="utf8"))
assert j["LogicalLevelCandidate"]==12
j["LogicalLevelCandidate"]=13
j["CourseLogicalGradeEvidence"]="American US-CCSS-MATH pack: Grade 12 NativeLabel uses logical level 13"
file.write_text(json.dumps(j,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
changed.append(str(file.relative_to(root)))
file=base/"g12-runtime-blueprint-draft-state.json"
j=json.loads(file.read_text(encoding="utf8"))
assert j["LogicalLevel"]==[12,12]
j["LogicalLevel"]=[13,13]
j["SourceGradeLabel"]="Grade 12"
j["ScopeProof"]="MathematicsCurriculumPackRegistry.US-CCSS-MATH: Grade 12 is LogicalLevel 13"
file.write_text(json.dumps(j,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
changed.append(str(file.relative_to(root)))
file=base/"source-of-truth.json"
j=json.loads(file.read_text(encoding="utf8"))
assert j["SelectorScopedGrade"]==12
j["SelectorLogicalLevel"]=13
j["SelectorAmericanGradeLabel"]="Grade 12"
j["GradePlacementProof"]="US-CCSS-MATH Grade12 is LogicalLevel 13 (Grade11 is 12); MathematicsCurriculumPackRegistry.cs"
file.write_text(json.dumps(j,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
changed.append(str(file.relative_to(root)))
file=base/"full-grade-progress.json"
j=json.loads(file.read_text(encoding="utf8"))
assert j["G12BluePrintCourseIdentityAuthored"]==True
j["CourseLogicalLevel"]=13
j["CourseLabelGrade"]=12
j["LessonEntries"][0]["CourseAdoptionLogicalLevel"]=13
file.write_text(json.dumps(j,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
changed.append(str(file.relative_to(root)))
file=base/"grade12-a-c-handoff.md"
s=file.read_text(encoding="utf8")
marker="- New **independent 73-lesson V2 Blueprint**"
assert marker in s
s=s.replace(marker,"- **Corrected grade placement:** American Grade 12 uses logical level 13, not logical 12 (Grade 11). Blueprint, selector and authored tests fixed before commit.\n- New **independent 73-lesson V2 Blueprint**",1)
file.write_text(s,encoding="utf8")
changed.append(str(file.relative_to(root)))
file=base/"grade12-abc-execution-instructions.md"
s=file.read_text(encoding="utf8")+"\n**Grade placement invariant:** US-CCSS-MATH Grade12 = logical level 13; never reuse UAE Grade12 logical12. PRECALC is offered only at logical13. Grade11 uses logical12.\n"
file.write_text(s,encoding="utf8")
changed.append(str(file.relative_to(root)))
print("CORRECTED_AMERICAN_GRADE12_LOGICAL_LEVEL_13",len(set(changed)),"FILES")
