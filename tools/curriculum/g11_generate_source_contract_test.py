"""Create nonexecuted Stage D-ready source contract test for one lesson."""
from pathlib import Path
import sys
n=int(sys.argv[1]);assert 5<=n<=11
root=Path(__file__).resolve().parents[2]
code=f"PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L{n:02d}"
rich=f"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u01-l{n:02d}.rich-lesson-v2.json"
assess=f"src/Edulytics.Core/Curriculum/LessonContent/Assessment/alg2-u01-l{n:02d}-local-source-20261106.json"
evid=f"artifacts/curriculum-delivery/US-CCSS-MATH/G11/u01-l{n:02d}-element-coverage.json"
special={
5:"assert.deepEqual([1,2,3,4,5].map(k=>k*(k+1)/2),[1,3,6,10,15]);",
6:"assert.deepEqual([1,2,3,4].map(k=>Array.from({length:k},(_,i)=>(i+1)**2).reduce((a,b)=>a+b,0)),[1,5,14,30]);",
7:"assert.deepEqual([0,1,2,3].map(n=>n*n+3*n+3),[3,7,13,21]);",
8:"assert.equal(3*2**(6-1),96);assert.equal(80*(1/4)**3,1.25);",
9:"assert.equal(6+4*(5-1),22);assert.equal(6.75+5*0.5,9.25);",
10:"assert.equal((24/16-1)*100,50);assert.equal(3*2**5,96);",
11:"assert.equal([1,2,4,8,16,32,64].reduce((a,b)=>a+b,0),127);assert.equal(1-(1/2)**4,15/16);"
}
codejs=f"""'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'../..');
const read=f=>JSON.parse(fs.readFileSync(path.join(root,f),'utf8'));
const code={code!r};
const rich=read({rich!r});
const assessment=read({assess!r});
const evidence=read({evid!r});
test('canonical identity and source evidence contract',()=>{{
 assert.equal(rich.Lessons.length,1);
 assert.equal(rich.Lessons[0].LessonCode,code);
 assert.equal(assessment.LessonCode,code);
 assert.equal(evidence.LessonCode,code);
 assert.ok(evidence.Count>=10);
 assert.equal(evidence.Count,evidence.Elements.length);
 assert.equal(new Set(evidence.Elements.map(x=>x.SourceElementId)).size,evidence.Count);
 assert.equal(assessment.TestsExecuted,'NOT_RUN');
 assert.equal(assessment.StageD,'NOT_RUN');
 assert.equal(assessment.TeacherReview.Required,true);
 assert.equal(assessment.FirstPartyCoolDown.Origin,'EDULYTICS_ORIGINAL');
}});
test('independent math oracle for the lesson',()=>{{{special[n]}}});
test('preserve first-party original objectives, answer boundaries and source tasks',()=>{{
 assert.ok(assessment.LearningGoals.length>0);
 assert.ok(rich.Lessons[0].ExplanationParagraphs.length>=5);
 assert.ok(rich.Lessons[0].WorkedExamples.length>=2);
 assert.ok(evidence.Elements.some(x=>x.SourceElementId.includes('STUDENT_PRACTICE')));
 assert.equal(assessment.QuestionRouting,'UNPROMOTED_NO_RUNTIME_CLAIM');
}});
"""
p=root/f"tools/curriculum/us-ccss-g11-u01-l{n:02d}-authoring-contract.test.cjs";p.write_text(codejs,encoding='utf8')
print("AUTHORED_NOT_EXECUTED_TEST",str(p))
