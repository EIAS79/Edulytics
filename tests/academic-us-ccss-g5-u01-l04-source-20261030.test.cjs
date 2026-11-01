'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('G5 U01 L04 2021 source mathematical oracle and teacher rubric',()=>{
 assert.equal(5*24,120);assert.equal(6*20,120);assert.equal(4*30,120);assert.notEqual(2*7,42);
 const p=JSON.parse(fs.readFileSync(path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json'),'utf8')).Lessons.find(l=>l.LessonCode==='PED:US-CCSS-MATH:G5:U01:L04');
 assert.match(p.ExplanationParagraphs.join(' '),/Teacher 8-point source-aligned review/);
});

test('G5 U01 L04 Stage D source Activity 1 A–F dimensional oracle and gated cooldown evidence, preserving A–B–C handoff',()=>{
 const root=path.resolve(__dirname,'..');
 const pack=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json'),'utf8'));
 const lesson=pack.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L04');
 const evidence=JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l04-stage-d-original-cooldown-source-findings-20261102.json'),'utf8'));
 const expected=[['A',148018,2,3,4,24],['B',148019,3,5,2,30],['C',148020,3,2,5,30],
 ['D',148021,4,2,3,24],['E',148022,4,1,2,8],['F',148023,2,7,6,84]];
 assert.deepEqual(evidence.sourceFigureCorrections.map(x=>[x.label,Number(x.sourceFigureRecordId),...x.dimensions,x.volumeCubicUnits]),expected);
 const table=lesson.Visuals.find(x=>x.Title.includes('Activity 1 A–F'));
 assert.ok(table);assert.match(table.Description,/not image files/i);
 assert.deepEqual(table.Rows.find(x=>x.Label==='Volume (cubic units)').Cells,['24','30','30','24','8','84']);
 for(const [label,id,a,b,c,volume] of expected){
   assert.equal(a*b*c,volume);
   assert.equal(evidence.sourceFigureCorrections.find(x=>x.sourceFigureRecordId===String(id)).label,label);
 }
 assert.equal(evidence.stageDDiscovery.originalCooldownElementId,'G5-U01-L04-CD-Q01');
 assert.equal(evidence.stageDDiscovery.originalCooldownContentAvailable,false);
 assert.equal(evidence.stageDAcceptanceVerified,false);
 assert.equal(evidence.stageDDiscovery.sourceElementsMatchedBeforeBlockedCooldown,20);
 assert.equal(evidence.stageDDiscovery.sourceElementsIdentified,21);
 const grade=JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/G5/g5-abc-lesson-ledger-v1.json'),'utf8'));
 assert.equal(grade.currentAcceptanceCursor,'PED:US-CCSS-MATH:G5:ABC_COMPLETE');
 assert.equal(grade.lessons[3].READY_FOR_STAGE_D,true);
 assert.equal(grade.gradeTotals.READY_FOR_D,148);
 assert.equal(grade.gradeTotals.D_VERIFIED,0);
});
