"""Write (not execute) unit-2 lesson-scoped authoring and math-oracle contract."""
from pathlib import Path
import sys
n=int(sys.argv[1]);assert 1<=n<=26
ROOT=Path(__file__).resolve().parents[2]
oracles={
1:"assert.equal(2*(20-4)*(12-4),256);",
2:"assert.equal(1000*(1+1.05+1.05**2),3152.5);",
3:"assert.equal(3*2**4-2*2**2+7,47);",
4:"assert.equal((1+3)*4+(2-1)*2+1,19);",
5:"assert.equal((2-2)*(2+3),0);assert.equal((-3-2)*(-3+3)===0,true);",
6:"assert.equal(4*8-64*4+240*2,256);",
7:"assert.equal((3+1)*(3-3),0);assert.equal((-1+1)*(-1-3)===0,true);",
8:"assert.equal(Math.sign(2*(-100)**4-3*(-100)+1),1);",
9:"assert.equal(Math.sign(-2*(-100)**3+7*(-100)),1);",
10:"assert.equal((1-1)**2*(1+2),0);",
11:"assert.equal(2**2,2*2);",
12:"assert.equal(3**2+5*3+6,(3+2)*(3+3));",
13:"assert.equal((2+2)*(2*2**2-2-6),2*2**3+3*2**2-8*2-12);",
14:"assert.equal((0+2)*(0-1)*(0-3),6);",
15:"assert.equal(1**3-4*1+3,0);",
16:"assert.equal(2*25+2*5*4,90);",
17:"assert.equal(10/4,2.5);",
18:"assert.equal(5+1000/100,15);",
19:"assert.equal(2*2**2+3,(2*2-2)*(2+1)+5);",
20:"assert.equal(5+200/50,9);",
21:"assert.equal(8/(6-2)+8/(6+2),3);",
22:"assert.equal((-2)**2-4,0);",
23:"assert.equal((4+3)**2,4**2+2*4*3+3**2);",
24:"assert.equal((5+2)**2+(5-2)**2,2*(5**2+2**2));",
25:"assert.equal(3*(2**4-1)/(2-1),45);",
26:"assert.equal(5*(2**5-1),155);"
}
code=f"PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U02:L{n:02d}"
rich=f"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u02-l{n:02d}.rich-lesson-v2.json"
assessment=f"src/Edulytics.Core/Curriculum/LessonContent/Assessment/alg2-u02-l{n:02d}-local-source-20261106.json"
evidence=f"artifacts/curriculum-delivery/US-CCSS-MATH/G11/u02-l{n:02d}-element-coverage.json"
out=f"""'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code={code!r}, rich=read({rich!r}).Lessons[0];
const assessment=read({assessment!r}),evidence=read({evidence!r});
test('unit 2 lesson identity and source task coverage evidence',()=>{{
 assert.equal(rich.LessonCode,code);
 assert.equal(assessment.LessonCode,code);
 assert.equal(evidence.LessonCode,code);
 assert.ok(evidence.Goals.length>0);
 assert.ok(evidence.ElementCount>=8);
 assert.equal(evidence.SourceElements.length,evidence.ElementCount);
 assert.equal(new Set(evidence.SourceElements.map(x=>x.SourceElementId)).size,evidence.ElementCount);
 assert.ok(rich.ExplanationParagraphs.length>=5);
 assert.equal(assessment.TestsExecuted,'NOT_RUN');
 assert.equal(assessment.StageD,'NOT_RUN');
 assert.equal(assessment.TeacherReview.Required,true);
 assert.equal(assessment.FirstPartyCoolDown.Origin,'EDULYTICS_ORIGINAL');
}});
test('lesson-specific independent arithmetic oracle',()=>{{{oracles[n]}}});
test('teacher and learner source still distinguish authored from tested',()=>{{
 assert.equal(assessment.QuestionRouting,'UNPROMOTED_NO_RUNTIME_CLAIM');
 assert.ok(assessment.CoreMathWorkedExample.Question.length>15);
 assert.ok(assessment.CoreMathWorkedExample.Answer.length>2);
 assert.ok(evidence.SourceElements.some(x=>x.SourceElementId.includes('PRACTICE')));
}});
"""
p=ROOT/f"tools/curriculum/us-ccss-g11-u02-l{n:02d}-authoring-contract.test.cjs";p.write_text(out,encoding='utf8')
print("G11_U02_TEST_WRITTEN_NOT_EXECUTED",n)
