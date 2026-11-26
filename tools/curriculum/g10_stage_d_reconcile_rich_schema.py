"""Materialize authored G10 data in the existing strict RichV2 runtime shape.
This does not certify source parity, mathematical correctness or authenticated rendering.
No authored steps, explanations, checks, answers, table cells or mistake text are discarded.
"""
import json,pathlib,collections,copy
root=pathlib.Path(__file__).resolve().parents[2]
g=root/'artifacts/curriculum-delivery/UAE-MOE-MATH/G10'
rich=root/'src/Edulytics.Core/Curriculum/LessonContent/RichV2'
read=lambda p: json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,x): p.write_text(json.dumps(x,ensure_ascii=False,indent=2)+'\n',encoding='utf8')
def replace(p,old,new):
 s=p.read_text(encoding='utf-8-sig'); assert s.count(old)==1,(str(p),old,s.count(old))
 p.write_text(s.replace(old,new),encoding='utf8')
counts=collections.Counter(); changes=[]
files=sorted(rich.glob('uae-g10-general-*json'))
assert len(files)==148,len(files)
for p in files:
 d=read(p); original=copy.deepcopy(d); affected=collections.Counter()
 for l in d['Lessons']:
  for e in l['WorkedExamples']:
   steps=e['Steps']
   if len(steps)>=3: continue
   # Reuse the already-authored verification action and/or method. Never manufacture a generic third step.
   if len(steps)==1:
    method=e['Method'].strip()
    assert method and method not in steps
    steps.insert(0,method)
   check=e['Check'].strip()
   if not check or check in [s.strip() for s in steps]+[e['Answer'].strip()]:
    assert p.name=='uae-g10-general-u02-l02-ar.rich-lesson-v2.json' and e['Title']=='أس سالب ومقلوب موجب'
    e['Check']='٢⁴=١٦؛ وضرب ١/١٦ في ١٦ يعيد ١.'
    check=e['Check']
    affected['specificNegativeExponentCheck']+=1
   steps.append(check)
   assert len(steps)>=3
   affected['existingAuthoredStepsReconciled']+=1
  for i,m in enumerate(l['CommonMistakes']):
   if isinstance(m,str):
    assert m.strip()
    # Existing notice has no distinct correction/reason. Preserve its literal text, never invent a reason.
    l['CommonMistakes'][i]={'Mistake':m,'WhyWrong':'','Correction':''}
    affected['legacyMistakeTextStructured']+=1
  for v in l['Visuals']:
   if v['Kind']=='ComparisonTable':
    assert v.get('Rows'),(p.name,v['Title'])
    affected['comparisonTablesPreserved']+=1
  assert all(isinstance(m,dict) and m['Mistake'] for m in l['CommonMistakes'])
  assert all(len(e['Steps'])>=3 for e in l['WorkedExamples'])
 if d!=original: save(p,d)
 if affected: changes.append({'file':p.name,'lessonCode':d['Lessons'][0]['LessonCode'],**affected})
 counts.update(affected)
assert counts['existingAuthoredStepsReconciled']==1555,counts
assert counts['legacyMistakeTextStructured']==611,counts
assert counts['comparisonTablesPreserved']==340,counts
assert counts['specificNegativeExponentCheck']==1
replace(root/'src/Edulytics.Core/Curriculum/RichLessonContentV2.cs','    CoordinatePlane = 18\n','    CoordinatePlane = 18,\n    // Independently authored comparison rows; stable existing enum IDs are unchanged.\n    ComparisonTable = 19\n')
replace(root/'src/Edulytics.Web/Views/Shared/_RichLessonVisual.cshtml','Model.Kind is RichLessonVisualKind.RatioTable or RichLessonVisualKind.PlaceValueChart)','Model.Kind is RichLessonVisualKind.RatioTable or RichLessonVisualKind.PlaceValueChart or RichLessonVisualKind.ComparisonTable)')
replace(root/'src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml','                <p><strong>@whyWrongLabel:</strong> @mistake.WhyWrong</p>\n                <p><strong>@correctionLabel:</strong> @mistake.Correction</p>','                @if (!string.IsNullOrWhiteSpace(mistake.WhyWrong))\n                {\n                    <p><strong>@whyWrongLabel:</strong> @mistake.WhyWrong</p>\n                }\n                @if (!string.IsNullOrWhiteSpace(mistake.Correction))\n                {\n                    <p><strong>@correctionLabel:</strong> @mistake.Correction</p>\n                }')
# Reconcile the topology test with the owner-approved G10 retirement. Retain all other grade assertions.
p=root/'tests/Edulytics.Tests/MathematicsIntelligence/UaeG9GeneralFirstPartyScopeReleaseTests.cs'
replace(p,'Assert.Equal(18, uae.Levels.Count);','Assert.Equal(17, uae.Levels.Count);')
replace(p,'foreach (var grade in new[] { 5, 6, 7, 10, 11, 12 })','Assert.Contains(uae.Levels, x => x.LogicalLevel == 10 && x.Pathway == "General");\n        Assert.DoesNotContain(uae.Levels, x => x.LogicalLevel == 10 && x.Pathway == "Advanced");\n        foreach (var grade in new[] { 5, 6, 7, 11, 12 })')
report={'schemaVersion':1,'grade':10,'pathway':'General','baseCandidateSha':'05bb8d0e8dff6e08724e6325c8e8949f98c25b21','scope':'RichV2 data-shape compatibility repair; original mathematical content retained, existing checks/methods surfaced as explicit instructional actions. Not an academic or runtime certification.','counts':dict(counts),'affectedLessons':changes,'openDefects':['611 preserved mistake notices still require individual reason/correction academic review when absent','Existing example checks and method content still require per-example mathematical/source verification','All authenticated rendering, grading, source parity and migration gates remain independent.'],'productionRouting':False}
save(g/'grade10-stage-d-rich-schema-reconciliation.json',report)
print('G10_RICH_SCHEMA_RECONCILIATION',dict(counts), 'affected',len(changes))
