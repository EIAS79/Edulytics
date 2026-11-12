import json, pathlib
r=pathlib.Path(__file__).resolve().parents[2]
path=r/'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'
s=path.read_text(encoding='utf-8-sig')
a='UaeGrade3Unit1Questions.Supports(family.Trim()) ||'
assert s.count(a)==1
s=s.replace(a, a+'\n          UaeGrade4Unit1Lesson01Questions.Supports(family.Trim()) ||')
a='if (UaeGrade3Unit1Questions.Supports(family))\n            return UaeGrade3Unit1Questions.Verify(family, parameters, answer);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit1Lesson01Questions.Supports(family))\n            return UaeGrade4Unit1Lesson01Questions.Verify(family, parameters, answer);')
a='if (UaeGrade3Unit1Questions.Supports(family))\n        {\n            var q = UaeGrade3Unit1Questions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit1Lesson01Questions.Supports(family))\n        {\n            var q = UaeGrade4Unit1Lesson01Questions.Build(family, random);\n            return new ExactProblem(q.Family, q.Prompt, q.Solution, q.ItemType, q.Parameters);\n        }')
a='if (UaeGrade3Unit1Questions.Supports(problem.Family))\n            return UaeGrade3Unit1Questions.Solve(problem.Family, p);'
assert s.count(a)==1
s=s.replace(a,a+'\n        if (UaeGrade4Unit1Lesson01Questions.Supports(problem.Family))\n            return UaeGrade4Unit1Lesson01Questions.Solve(problem.Family, p);')
path.write_text(s,encoding='utf-8')
contract=r/'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'
s=contract.read_text(encoding='utf-8-sig')
marker='        byLesson["PED:UAE-MOE-MATH:L3:COMMON:01:01"] = new LessonPracticeContract('
assert s.count(marker)==1
insert='''        byLesson["PED:UAE-MOE-MATH:L4:COMMON:01:01"] = new LessonPracticeContract(
            "PED:UAE-MOE-MATH:L4:COMMON:01:01",
            "number.place_value.to_millions",
            "SEVEN_DIGIT_PLACE_VALUE_CHART_WITH_MILLIONS_GROUP",
            ["uae.g4.u01.l01.digit_value_to_millions",
             "uae.g4.u01.l01.compose_seven_digits",
             "uae.g4.u01.l01.expanded_to_standard"],
            "UAE_G4_ARABIC_STUDENT_04_01_PRINTED_PP11_16_STAGE_D_NOT_RUN",
            "READY_VERIFIED",
            Version);
'''
s=s.replace(marker,insert+marker)
contract.write_text(s,encoding='utf-8')
reg=r/'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'
j=json.loads(reg.read_text(encoding='utf-8-sig'))
print('registry container type',type(j).__name__)
if isinstance(j,dict):
    print('registry keys',list(j))
    collection=next((v for v in j.values() if isinstance(v,list) and v and isinstance(v[0],dict) and 'id' in v[0]),None)
else:
    collection=j
assert collection is not None
g3=next(x for x in collection if x.get('id')=='uae.g3.u01.l01.digit_value')
for f in ('digit_value_to_millions','compose_seven_digits','expanded_to_standard'):
    fid=f'uae.g4.u01.l01.{f}'
    assert not any(x.get('id')==fid for x in collection)
    e=dict(g3)
    e.update({'id':fid,'skillId':'number.place_value.to_millions','verificationPolicy':'g4-u01-l01-arabic-student-skill-contract-d-tests-not-run','representations':['place_value_chart','symbolic'],'status':'ShadowVerified','productionRouting':False,'lessonPracticeRouting':True})
    collection.append(e)
reg.write_text(json.dumps(j,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
print('PASS: wired exact engine supports/build/verify/solve and practice/registry 3 families')
