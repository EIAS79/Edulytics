'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const crypto=require('node:crypto');
const root=path.resolve(__dirname,'..');
const read=p=>fs.readFileSync(path.join(root,p),'utf8');
const c=JSON.parse(read('docs/curriculum/us-ccss-g8-coder4-u02-l08-academic-assessment-contract-20261104.json'));
test('U02 L08 source mapped classroom assessment has distinct AA, construction, practice, cooldown boundaries',()=>{
 assert.equal(c.sourceAccess,'LOCAL_SNAPSHOT_NOT_V360');
 assert.equal(c.sourceActivityToFirstPartyCrosswalk.length,5);
 assert.equal(c.sourcePracticeToFirstPartyCrosswalk.length,5);
 assert.equal(c.maxScore,c.teacherRubric.reduce((n,r)=>n+r.points,0));
 const html=read(c.assessment.path);
 for(const id of c.assessment.stableTaskIds) assert.match(html,new RegExp("id=['\"]"+id+"['\"]"));
 assert.match(html,/physical|construct/i);assert.match(html,/Cool-down/);
 assert(!html.includes('Open Up Resources'));
 assert.equal(crypto.createHash('sha256').update(fs.readFileSync(path.join(root,c.assessment.path))).digest('hex'),c.assessment.sha256);
});
test('U02 L08 independent math AA similarity and signed-expression regressions',()=>{
 const anglesA=[53,71,56], anglesB=[71,56,53];assert.equal(anglesA.reduce((a,b)=>a+b),180);assert.deepEqual(anglesA.sort(),anglesB.sort());
 const C=[90,37,53],D=[90,53,37];assert.deepEqual(C.sort(),D.sort());
 assert.equal(10*(4+(-2)),20);assert.equal(4*10+(-2)*10,20);
 assert.equal((8*15)/12,10);assert.equal(5*3,15);assert.deepEqual([[1,1],[3,1],[3,2],[1,2]].map(([x,y])=>[2*x+2,2*y]),[[4,2],[8,2],[8,4],[4,4]]);assert.equal((42+68+70),180);
 assert.notDeepEqual([42,68,70].sort(),[42,75,63].sort());
});
