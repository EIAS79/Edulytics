#!/usr/bin/env python3
"""Keep legacy full-catalogue regression baselines exact after adding G12 73.
Do not count the new course as 2026 legacy 1,560, and keep separate Grade12 tests."""
from pathlib import Path
import subprocess
ROOT=Path(__file__).resolve().parents[2]
BASE="tests/Edulytics.Tests/"
def edit(rel,transform):
 p=ROOT/(BASE+rel)
 if p.exists():s=p.read_text(encoding="utf8")
 else:s=subprocess.check_output(["git","show","HEAD:"+BASE+rel],cwd=ROOT).decode("utf8")
 new=transform(s)
 if new==s:raise AssertionError("Nothing changed "+rel)
 p.parent.mkdir(parents=True,exist_ok=True)
 p.write_text(new,encoding="utf8")
 print("UPDATED",rel)
def sub(s,old,new,n=1):
 assert s.count(old)==n,(old,s.count(old))
 return s.replace(old,new)
def counts(s,old,new,n):
 return sub(s,str(old),str(new),n)
for rel,n in [
 ("MathematicsIntelligence/CanonicalLessonContentMaterializerTests.cs",1),
 ("MathematicsIntelligence/CatalogueContentPracticeDivergenceInventoryTests.cs",1),
 ("MathematicsIntelligence/EffectiveLessonContentSnapshotTests.cs",1),
 ("MathematicsIntelligence/RichLessonContentV2PilotTests.cs",1),
 ("MathematicsIntelligence/RichLessonContentV2RolloutTests.cs",2),
 ("Phase29/CleanNeonEmbeddedJsonReadTests.cs",1)
]:
 edit(rel,lambda s,n=n:counts(s,5110,5183,n))
edit("MathematicsIntelligence/PracticeSessionDiversityCertificationTests.cs",lambda s:counts(s,5110,5138,1))
def traditional_practice(s):
 old='''            .Where(lesson => lesson.LessonCode.StartsWith("PED:US-CCSS-MATH:", StringComparison.Ordinal))'''
 new='''            .Where(lesson => lesson.LessonCode.StartsWith("PED:US-CCSS-MATH:", StringComparison.Ordinal)
                && !lesson.LessonCode.StartsWith("PED:US-CCSS-MATH:HS:ADV:PRECALC:", StringComparison.Ordinal))'''
 return sub(s,old,new)
edit("MathematicsIntelligence/UsCommonCoreFullLessonPracticeAuditTests.cs",traditional_practice)
def rollout(s):
 old='.Where(x => x.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode)'
 assert s.count(old)==2
 s=s.replace(old,old[:-1]+' && x.CourseCode != "PRECALC")',1)
 s=s.replace(old,old[:-1]+''' &&
                !x.Lessons.Any(l => l.LessonCode.StartsWith("PED:US-CCSS-MATH:HS:ADV:PRECALC:", StringComparison.Ordinal)))''',1)
 return s
edit("Phase29/Phase29CommonCoreFullContentRolloutTests.cs",rollout)
def final_policy(s):
 old='.Where(x => x.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode).ToArray();'
 new='''.Where(x => x.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode
                && !x.Lessons.Any(l => l.LessonCode.StartsWith(
                    "PED:US-CCSS-MATH:HS:ADV:PRECALC:", StringComparison.Ordinal))).ToArray();'''
 return sub(s,old,new)
edit("Phase29/Phase29FinalCurriculumLanguagePolicyTests.cs",final_policy)
def hs_blueprint(s):
 old='''x.PackCode ==
                        MathematicsCurriculumPackRegistry
                            .CommonCoreCode)'''
 new='''x.PackCode ==
                        MathematicsCurriculumPackRegistry
                            .CommonCoreCode &&
                        x.CourseCode != "PRECALC")'''
 s=sub(s,old,new)
 # Legitimate L13 PRECALC course must not be confused with unintended
 # pathwayless high-school fallback rows.
 old='''x.LogicalLevelFrom <= 13);'''
 new='''x.LogicalLevelFrom <= 13 &&
                        !x.Code.StartsWith(
                            "PED:US-CCSS-MATH:HS:ADV:PRECALC:"));'''
 s=sub(s,old,new)
 return s
edit("Phase29/Phase29CommonCoreHighSchoolBlueprintV2Tests.cs",hs_blueprint)
def supporting(s):
 old='''x.PackCode ==
                        MathematicsCurriculumPackRegistry
                            .CommonCoreCode)'''
 new='''x.PackCode ==
                        MathematicsCurriculumPackRegistry
                            .CommonCoreCode &&
                        x.CourseCode != "PRECALC")'''
 s=sub(s,old,new)
 old='''x.FrameworkVersionId ==
                        versionId)
                .Select(
                    x => x.Id)'''
 new='''x.FrameworkVersionId ==
                        versionId &&
                        !x.Code.StartsWith(
                            "PED:US-CCSS-MATH:HS:ADV:PRECALC:"))
                .Select(
                    x => x.Id)'''
 s=sub(s,old,new)
 return s
edit("Phase29/Phase29CommonCoreSupportingLessonPolicyTests.cs",supporting)
print("LEGACY_CATALOGUE_AND_GRADE12_SCOPES_SEPARATED")
