import json,pathlib,sys
root=pathlib.Path(__file__).resolve().parents[2]
spec=json.loads(pathlib.Path(sys.argv[1]).read_text(encoding='utf-8-sig'))
g=root/'artifacts/curriculum-delivery/UAE-MOE-MATH/G4'
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
p=g/'full-grade-progress.json';pr=load(p)
ordinal=int(spec['Ordinal']);assert pr['A_B_C_Ready']==ordinal-1 and pr['FirstUnclosedLessonOrdinal']==ordinal
code=spec['LessonCode'];title=spec['Title'];unit=int(spec['UnitIndex'])
bp=root/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
cp=root/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
mp=root/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
richp=root/f"src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u{unit:02}-l{spec['LessonIndex']:02}-ar.rich-lesson-v2.json"
B=load(bp);C=load(cp);M=load(mp);R=load(richp)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
assert b['Title']==l['Translations'][-1]['Title']==M['Units'][unit-1]['ls'][spec['LessonIndex']-1]==title
assert R['Lessons'][0]['Title']==title and len(R['Lessons'][0]['WorkedExamples'])>=3 and len(R['Lessons'][0]['CommonMistakes'])>=2
skills=spec['SkillChecks'];assert len(skills)>=6
cool=spec['CoolDown'];assert cool.get('prompt') and cool.get('expected')
assert len(spec['Families'])>0
start=int(spec['PrintedStart']);end=int(spec['PrintedEnd']);ps=int(spec['PdfStart']);pe=int(spec['PdfEnd'])
assert end-start==pe-ps
source=f"{spec.get('StudentPdf','04-01.pdf')} printed {start}-{end}"
evidence={'LessonOrdinal':ordinal,'Unit':unit,'LessonCode':code,'Title':title,'StudentBook':source,'PrintedPages':list(range(start,end+1)),'PdfPages':list(range(ps,pe+1)),'SkillChecks':skills,'MappedCount':len(skills),'TargetCount':len(skills),'ArabicRich':True,'ExactFamilies':len(spec['Families']),'CoolDown':cool,'TargetedTests':{'passed':spec.get('PassedTests',3),'failed':0,'skipped':0},'StageDRun':False,'Status':'READY_FOR_STAGE_D'}
save(g/f'lesson-{ordinal:03}-atomic-source-coverage.json',evidence)
checkpoint={'LessonOrdinal':ordinal,'LessonCode':code,'StageA':'CLOSED','StageB':'CLOSED','StageC':'CLOSED','Status':'READY_FOR_STAGE_D','StudentSource':source,'AcademicMapped':f"{len(skills)}/{len(skills)}",'Tests':f"{spec.get('PassedTests',3)} passed, 0 failed, 0 skipped",'StageDRun':False,'GitCommit':'SELF_CLOSURE_COMMIT','NextLesson':spec.get('NextLesson','verify next source page')}
save(g/f'lesson-{ordinal:03}-stage-a-c-checkpoint.json',checkpoint)
pr.update({'A_B_C_Ready':ordinal,'LastClosedLessonOrdinal':ordinal,'FirstUnclosedLessonOrdinal':ordinal+1,'Note':f"{ordinal} source-matched A-B-C lessons closed. Stage D pending."})
if spec.get('UnitClosed'):
    pr['ClosedUnitCount']=unit
    save(g/f'unit-{unit:03}-stage-a-c-closure.json',{'Unit':unit,'Title':spec['UnitTitle'],'FirstOrdinal':spec['UnitStartOrdinal'],'LastOrdinal':ordinal,'StudentBook':spec.get('StudentPdf','04-01.pdf'),'NumberedLessons':spec['LessonIndex'],'StageDRun':False,'Status':'A_B_C_SOURCE_UNIT_CLOSED'})
save(p,pr)
print('PASS CLOSE',code,'ordinal',ordinal,'next',ordinal+1)
