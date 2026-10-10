#!/usr/bin/env node
// Compares a read-only Neon outcome export with the accepted source blueprints.
'use strict';
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const bpdir=path.join(root,'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs');
const dbPath=process.argv[2]||path.join(root,'artifacts/curriculum/us-ccss-neon-outcome-baseline.json');
const read=p=>JSON.parse(fs.readFileSync(p,'utf8'));
const normalize=x=>[...new Set(x||[])].sort();
const bps=fs.readdirSync(bpdir).filter(x=>/^us-ccss-math-.*\.lesson-blueprint\.json$/.test(x));
const exp=new Map(),gradeStats={};
for(const file of bps){
 const d=read(path.join(bpdir,file));
 for(const l of d.Lessons){
  const codes=normalize([...(l.OutcomeCodes||[]),...(l.FormalTargets||[]).map(x=>x.OutcomeCode)].filter(Boolean));
  if(exp.has(l.LessonCode))throw Error('Duplicate Blueprint LessonCode: '+l.LessonCode);
  exp.set(l.LessonCode,codes);
  const k=d.NativeLevel||file;
  gradeStats[k]=(gradeStats[k]||0)+1;
 }
}
const db=read(dbPath),matrix=db.mappings||{},delta=[];
let actualLinks=0,expectedLinks=0;
for(const [lessonCode,expected] of exp){
 const actual=normalize(matrix[lessonCode]);
 expectedLinks+=expected.length;actualLinks+=actual.length;
 const added=actual.filter(x=>!expected.includes(x));
 const missing=expected.filter(x=>!actual.includes(x));
 if(added.length||missing.length)delta.push({lessonCode,expected,actual,extraInDb:added,missingFromDb:missing});
}
const noDb=[...exp.keys()].filter(k=>!Object.hasOwn(matrix,k));
const extraDb=Object.keys(matrix).filter(k=>!exp.has(k));
const report={schemaVersion:1,kind:'US Common Core production ↔ source blueprint outcome identity diff',
 sourceCommit:'1620cd3ad5de33c9f745606b3ea792043089bca1',databaseReadOnly:true,
 blueprintLessons:exp.size,neonLessons:Object.keys(matrix).length,expectedLinks,actualLinks,
 noDb,extraDb,linkDeltas:delta,gradeCounts:gradeStats,
 status:delta.length===0&&noDb.length===0&&extraDb.length===0?'PASS':'FAIL',
 limitations:['Valid identity mapping does not prove that every standard is pedagogically correct for that lesson.',
  'The database export includes only curricular lesson codes and outcome codes; no student data or credentials.']};
const output=path.join(root,'docs/curriculum/us-ccss-neon-blueprint-diff.json');
fs.writeFileSync(output,JSON.stringify(report,null,2)+'\n');
console.log(JSON.stringify({status:report.status,blueprintLessons:exp.size,neonLessons:report.neonLessons,
 expectedLinks,actualLinks,lessonsWithDifferences:delta.length,missingDbLessons:noDb.length,unexpectedDbLessons:extraDb.length,
 firstDeltas:delta.slice(0,8)},null,2));
if(report.status!=='PASS')process.exitCode=1;