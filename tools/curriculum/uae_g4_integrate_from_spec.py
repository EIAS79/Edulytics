import json,hashlib,pathlib,sys
root=pathlib.Path(__file__).resolve().parents[2]
spec=json.loads(pathlib.Path(sys.argv[1]).read_text(encoding='utf-8-sig'))
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def save(p,j):p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
code=spec['LessonCode'];title=spec['Title'];unitIndex=spec['UnitIndex'];lessonIndex=spec['LessonIndex']
codePrefix='PED:UAE-MOE-MATH:L4:COMMON:'
assert code==f'{codePrefix}{unitIndex:02}:{lessonIndex:02}'
content=root/'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g4-common-t1-ogl-v1.lesson-content-pack.json'
blueprint=root/'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g4-common-t1-ogl-v1.lesson-blueprint.json'
manifest=root/'docs/curriculum/UAE_G4_COMMON_OFFICIAL_MANIFEST.json'
C=load(content);B=load(blueprint);M=load(manifest)
l=next(x for x in C['Lessons'] if x['LessonCode']==code)
b=next(x for x in B['Lessons'] if x['LessonCode']==code)
assert b['LessonNumber']==lessonIndex and B['Units'][unitIndex-1]['Number']==unitIndex
t={'CultureCode':'ar','Title':title,**spec['Translation']}
assert all(t.get(k) for k in ('Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'))
l['Translations']=[x for x in l['Translations'] if x['CultureCode']!='ar']+[t]
pdf=spec.get('StudentPdf','04-01.pdf')
loc=f"{pdf} U{unitIndex:02} L{lessonIndex:02} printed pp{spec['PrintedStart']}-{spec['PrintedEnd']} PDF{spec['PdfStart']}-{spec['PdfEnd']}"
l.update({'SourceTitle':'UAE Grade 4 General Arabic student mathematics','SourceLocator':loc,'TitleSourceReference':loc,'SourceEdition':'Selected student book; edition date nonblocking','SourceSha256':spec['SourceSha256'],'CanonicalBodySha256':hashlib.sha256(json.dumps(t,ensure_ascii=False,sort_keys=True,separators=(',',':')).encode()).hexdigest(),'AdaptationStatus':'Original Arabic mathematical equivalent; Stage D pending'})
b['Title']=title;b['UnitTitle']=spec['UnitTitle'];B['Units'][unitIndex-1]['Title']=spec['UnitTitle']
M['Units'][unitIndex-1]['title']=spec['UnitTitle'];M['Units'][unitIndex-1]['ls'][lessonIndex-1]=title
save(content,C);save(blueprint,B);save(manifest,M)
f=spec['Families'];skill=spec['SkillId']
p=root/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=p.read_text(encoding='utf-8-sig')
anchor='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(anchor)==1
assert f'byLesson["{code}"]' not in s
contract=('        byLesson["'+code+'"] = new LessonPracticeContract(\n'
'            "'+code+'", "'+skill+'",\n'
'            "'+spec['StrategyCode']+'",\n'
'            ['+', '.join('"'+x+'"' for x in f)+'],\n'
'            "UAE_G4_STUDENT_'+pdf.replace('.pdf','').replace('-','_')+f"_PP{spec['PrintedStart']}_{spec['PrintedEnd']}_STAGE_D_NOT_RUN"+'",\n'
'            "READY_VERIFIED",Version);\n')
p.write_text(s.replace(anchor,contract+anchor),encoding='utf-8')
p=root/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=load(p)
template=next(x for x in j['families'] if x['id']=='uae.g4.u06.l01.divide_powers_10_100_1000')
for fam in f:
    if any(x['id']==fam for x in j['families']):continue
    z=dict(template);z.update({'id':fam,'skillId':skill,'answerType':'exact_integer','verificationPolicy':f'g4-u{unitIndex:02}-l{lessonIndex:02}-source-stage-d-not-run','representations':spec.get('Representations',['equation','place_value']),'productionRouting':False,'lessonPracticeRouting':True})
    j['families'].append(z)
save(p,j)
if spec.get('EngineClass'):
    name=spec['EngineClass'];anchorName=spec.get('EngineAnchor','UaeGrade4Unit6Lesson01DivisionPowersQuestions')
    p=root/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
    s=p.read_text(encoding='utf-8-sig')
    a=anchorName+'.Supports(family.Trim()) ||';assert s.count(a)==1
    s=s.replace(a,a+'\n          '+name+'.Supports(family.Trim()) ||')
    a='if ('+anchorName+'.Supports(family))\n            return '+anchorName+'.Verify(family, parameters, answer);'
    assert s.count(a)==1;s=s.replace(a,a+'\n        if ('+name+'.Supports(family))\n            return '+name+'.Verify(family, parameters, answer);')
    a='if ('+anchorName+'.Supports(family))\n        {\n            var q = '+anchorName+'.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
    assert s.count(a)==1;s=s.replace(a,a+'\n        if ('+name+'.Supports(family))\n        {\n            var q = '+name+'.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
    a='if ('+anchorName+'.Supports(problem.Family))\n            return '+anchorName+'.Solve(problem.Family, p);'
    assert s.count(a)==1;s=s.replace(a,a+'\n        if ('+name+'.Supports(problem.Family))\n            return '+name+'.Solve(problem.Family, p);')
    p.write_text(s,encoding='utf-8')
rich={'SchemaVersion':1,'ContentVersion':f'uae-g4-u{unitIndex:02}-l{lessonIndex:02}-ar-20261111','PackCode':'UAE-MOE-MATH','Lessons':[{'LessonCode':code,'CultureCode':'ar','Title':title,**spec['Rich']}]}
rr=rich['Lessons'][0]
assert len(rr['KeyConcepts'])>=3 and len(rr['WorkedExamples'])>=3 and len(rr['CommonMistakes'])>=2 and len(rr['Visuals'])>0
save(root/f'src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g4-u{unitIndex:02}-l{lessonIndex:02}-ar.rich-lesson-v2.json',rich)
print('PASS',code,title,'families',len(f))
