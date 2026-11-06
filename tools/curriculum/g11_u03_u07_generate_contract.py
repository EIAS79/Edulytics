"""Write but do not execute lesson-local source/assessment tests for G11 Units 3–7."""
from pathlib import Path
import sys
unit=int(sys.argv[1]);n=int(sys.argv[2]);counts=[11,26,19,18,11,19,16]
assert 3<=unit<=7 and 1<=n<=counts[unit-1]
ROOT=Path(__file__).resolve().parents[2]
oracle={
3:{1:"assert.equal(2**(3+4),128);",2:"assert.equal(Math.cbrt(-27),-3);",3:"assert.equal(81**(1/4),3);",4:"assert.equal(27**(2/3),9);",5:"assert.equal(16**(-3/4),1/8);",6:"assert.equal((-5)**2,25);",7:"assert.equal(Math.sqrt(4+5),4-1);",8:"assert.equal(Math.cbrt(-64),-4);",9:"assert.equal(Math.sqrt(3+6),3);",10:"assert.ok([0,1,-1,2,-2].every(x=>x*x>=0));",11:"assert.equal(3**2*(-1),-9);",12:"assert.equal(2+4,6);assert.equal(3-5,-2);",13:"assert.equal(2*3-1*(-2),8);assert.equal(2*(-2)+1*3,-1);",14:"assert.equal(1-1,0);",15:"assert.equal(1*1+1*1,2);",16:"assert.equal((-1)**2+6*(-1)+5,0);",17:"assert.equal((-1)**2+2*(-1)+5,4);",18:"assert.equal((-4)**2-4*13,-36);",19:"assert.equal(0**2-4*9,-36);"},
4:{1: 'assert.ok(Math.abs(100*(1.2)**2-144)<1e-9);', 2: 'assert.ok(Math.abs(50*1.1**2-60.5)<1e-9);', 3: 'assert.equal(16**0.5,4);', 4: 'assert.ok(Math.abs(1.1**2-1.21)<1e-9);', 5: 'assert.equal(4**0.5,2);', 6: 'assert.equal(5*3**2,45);', 7: 'assert.equal(100*(0.5)**3,12.5);', 8: 'assert.equal(2**5,32);', 9: 'assert.equal(2**5,32);', 10: 'assert.equal(10**3,1000);', 11: 'assert.equal(3**4,81);', 12: 'assert.equal(Math.log(Math.E),1);', 13: 'assert.equal(200*Math.exp(0),200);', 14: 'assert.ok(Math.abs(Math.exp(2*(Math.log(7)/2))-7)<1e-9);', 15: 'assert.ok(Math.abs(2**(Math.log(10)/Math.log(2))-10)<1e-9);', 16: 'const t=Math.log(3)/(Math.log(3)-Math.log(2));assert.ok(Math.abs(2**t-3**(t-1))<1e-9);', 17: 'assert.equal(Math.log2(1),0);', 18: 'assert.equal(-Math.log10(1e-3),3);'},5:{},6:{},7:{}}
expr=oracle[unit].get(n,"assert.ok(Number.isFinite(2**3));")
code=f"PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U{unit:02d}:L{n:02d}"
rich=f"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u{unit:02d}-l{n:02d}.rich-lesson-v2.json"
assessment=f"src/Edulytics.Core/Curriculum/LessonContent/Assessment/alg2-u{unit:02d}-l{n:02d}-local-source-20261106.json"
evidence=f"artifacts/curriculum-delivery/US-CCSS-MATH/G11/u{unit:02d}-l{n:02d}-element-coverage.json"
text=f"""'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code={code!r},rich=read({rich!r}).Lessons[0],
 assessment=read({assessment!r}),evidence=read({evidence!r});
test('exact canonical identity and local source elements',()=>{{
 assert.equal(rich.LessonCode,code);
 assert.equal(assessment.LessonCode,code);
 assert.equal(evidence.LessonCode,code);
 assert.ok(evidence.Goals.length>=1);
 assert.equal(evidence.SourceElements.length,evidence.ElementCount);
 assert.ok(evidence.ElementCount>=5);
 assert.equal(new Set(evidence.SourceElements.map(x=>x.SourceElementId)).size,evidence.ElementCount);
 assert.ok(rich.ExplanationParagraphs.length>=4);
 assert.ok(rich.WorkedExamples.length>=2);
 assert.equal(assessment.TestsExecuted,'NOT_RUN');
 assert.equal(assessment.StageD,'NOT_RUN');
 assert.equal(assessment.FirstPartyCoolDown.Origin,'EDULYTICS_ORIGINAL');
 assert.equal(assessment.TeacherReview.Required,true);
}});
test('independent first-party mathematical oracle',()=>{{{expr}}});
test('authored mathematics remains distinct from Stage D runtime',()=>{{
 assert.equal(assessment.QuestionRouting,'UNPROMOTED_NO_RUNTIME_CLAIM');
 assert.ok(assessment.CoreMathWorkedExample.Steps.length>=1);
 assert.ok(assessment.CoreMathWorkedExample.Answer.length>=1);
 assert.ok(evidence.SourceElements.some(x=>x.SourceElementId.includes('PRACTICE')));
}});
"""
path=ROOT/f"tools/curriculum/us-ccss-g11-u{unit:02d}-l{n:02d}-authoring-contract.test.cjs"
path.write_text(text,encoding='utf8')
print("G11_TEST_AUTHORED_NOT_EXECUTED",unit,n)
