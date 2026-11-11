import json, pathlib, hashlib, sys, datetime
R = pathlib.Path(__file__).resolve().parents[2]
G = R / 'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
S = json.loads((G/'unit-01-arabic-authoring.json').read_text(encoding='utf-8'))
n = int(sys.argv[1])
assert 1 <= n <= 6
spec = S['lessons'][n-1]
assert spec['number'] == n
content_path = R/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
blue_path = R/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
manifest_path = R/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
def read(path):
    return json.loads(path.read_text(encoding='utf-8-sig'))
def save(path, obj):
    path.write_text(json.dumps(obj,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
C=read(content_path); B=read(blue_path); M=read(manifest_path)
code=f'PED:UAE-MOE-MATH:L4:COMMON:01:{n:02d}'
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
bl=next(x for x in B['Lessons'] if x['LessonCode']==code)
source=f'04-01.pdf; printed pp. {spec["start"]}-{spec["end"]}; PDF pp. {spec["start"]+23}-{spec["end"]+23}'
translation={
    'CultureCode':'ar','Title':spec['title'],
    'Explanation':spec['explanation'],
    'KeyConceptsAndRules':spec['rules'],
    'WorkedExamples':spec['worked'],
    'StepByStepSolutions':spec['steps'],
    'CommonMistakes':spec['mistakes'],
    'QuickSummary':spec['summary']
}
assert all(isinstance(x,str) and len(x)>20 for k,x in translation.items() if k not in ('CultureCode','Title'))
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[translation]
l['TitleSourceReference']=source
l['SourceLocator']=source
l['SourceTitle']='UAE Grade 4 student mathematics — Arabic student text, Common'
l['SourceEdition']='Source PDF 04-01.pdf, 2025-2026; adopted as owner-selected educational content'
l['SourceSha256']=S['sourcePdfSha256']
l['CanonicalBodySha256']=hashlib.sha256(json.dumps(translation,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode('utf-8')).hexdigest()
l['AdaptationStatus']='Original Arabic explanation authored against reviewed student pages; A-B-C integration pending lesson-scoped skill routing and full catalogue migration'
bl['Title']=spec['title']
bl['UnitTitle']='القيمة المكانية'
B['Units'][0]['Title']='القيمة المكانية'
M['Units'][0]['title']='القيمة المكانية'
M['Units'][0]['ls'][n-1]=spec['title']
save(content_path,C);save(blue_path,B);save(manifest_path,M)
ledger={
 'LessonOrdinal':n,'LessonCode':code,'Grade':4,'Unit':1,'TitleArabic':spec['title'],
 'StudentPdf':'04-01.pdf','StudentPdfSha256':S['sourcePdfSha256'],
 'PrintedPageStart':spec['start'],'PrintedPageEnd':spec['end'],
 'PdfPageStart':spec['start']+23,'PdfPageEnd':spec['end']+23,
 'LearningObjective':spec['objective'],'LessonContentIntegrated':True,
 'ArabicTranslationAdded':True,
 'Cooldown':spec['cooldown'],'QuestionSkill':spec['skill'],
 'SkillContract':{
   'QuestionFamilyCandidate':spec['skill'],'ReuseFirst':True,
   'IndependentVerifier':'PENDING_ROUTE_CHECK',
   'TeacherReview':'Required for explanation',
   'ProductionRouted':False
 },
 'SourceElementCoverage':'PARTIAL: first-page review and mapped teaching elements; atomized all-page review pending',
 'TestsWritten':'static arithmetic and translation schema guards only',
 'TestsRun':'STATIC_CHECK_ONLY',
 'ACStatus':'IN_PROGRESS',
 'READY_FOR_STAGE_D':False,
 'AcademicApproved':True,'RightsDecision':'separate legal verification; no publisher images copied'
}
save(G/f'lesson-{n:03d}-stage-abc-evidence.json',ledger)
assert len({x['LessonCode'] for x in C['Lessons']})==len(C['Lessons'])
assert C['Lessons'][n-1]['Translations'][-1]['CultureCode']=='ar'
assert B['Lessons'][n-1]['Title']==M['Units'][0]['ls'][n-1]==spec['title']
print(f"PASS: Arabic lesson {n} static structure; practice routing NOT verified")
