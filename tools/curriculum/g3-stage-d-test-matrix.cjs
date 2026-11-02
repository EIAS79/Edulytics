"use strict";
const fs=require("node:fs"),path=require("node:path"),assert=require("node:assert/strict");
const root="artifacts/curriculum-delivery/US-CCSS-MATH/G3";
function collect(){
 const manifest=JSON.parse(fs.readFileSync(path.join(root,"grade-manifest.json"),"utf8"));
 const progress=JSON.parse(fs.readFileSync(path.join(root,"full-grade-progress.json"),"utf8"));
 const stage=JSON.parse(fs.readFileSync(path.join(root,"stage-d-execution-ledger.json"),"utf8"));
 assert.equal(manifest.totalLessons,143);
 assert.equal(manifest.units.length,8);
 assert.equal(progress.lessons.length,143);
 assert.equal(progress.totals.lessonsFullyReadyForStageD,143);
 assert.equal(stage.lessons.length,143);
 assert.equal(stage.academicApprovalsNew,0);
 assert.equal(stage.sourceParityPercentProven,null);
 let table=[];
 for(const unit of manifest.units)for(let n=1;n<=unit.lessonCount;n++){
  const ordinal=table.length+1;
  const code="PED:US-CCSS-MATH:G3:U"+String(unit.unitNumber).padStart(2,"0")+":L"+String(n).padStart(2,"0");
  const group=ordinal<=48?"D1":ordinal<=96?"D2":"D3";
  assert.equal(progress.lessons[ordinal-1].lessonCode,code);
  assert.equal(progress.lessons[ordinal-1].authoringClosure,"READY_FOR_STAGE_D");
  assert.equal(stage.lessons[ordinal-1].lessonCode,code);
  assert.equal(stage.lessons[ordinal-1].batch,group);
  table.push({ordinal,unit:unit.unitNumber,lesson:n,batch:group});
 }
 assert.equal(table.length,143);
 const dir="tests/Edulytics.Tests/MathematicsIntelligence";
 const map=new Map();
 for(const file of fs.readdirSync(dir)){
  const match=/^UsCcssG3Unit([1-8])Lesson(\d+)[A-Za-z0-9]*Tests\.cs$/i.exec(file);
  if(!match)continue;
  const item=table.find(x=>x.unit===Number(match[1])&&x.lesson===Number(match[2]));
  assert.ok(item,"Unexpected Grade 3 test file "+file);
  if(!map.has(item.ordinal))map.set(item.ordinal,[]);
  map.get(item.ordinal).push(path.basename(file,".cs"));
 }
 const missing=table.filter(x=>!map.has(x.ordinal));
 assert.deepEqual(missing.map(x=>x.ordinal),[],"143 lessons each require named test classes");
 const shared=["CommonCoreG3SourceTeachingCorrectionsTests","Grade3SourceCompanionGradingTests","UsCcssG3OriginalCoolDownCandidatesTests","UsCcssG3RemainingSourceIntakeContracts"];
 for(const cl of shared)assert.ok(fs.existsSync(path.join(dir,cl+".cs")),"Missing shared Grade 3 test "+cl);
 return {table,map,shared};
}
function filter(batch,x){
 assert.ok(["D1","D2","D3"].includes(batch));
 const classes=x.table.filter(t=>t.batch===batch).flatMap(t=>x.map.get(t.ordinal));
 if(batch==="D1")classes.push(...x.shared);
 return classes.map(c=>"FullyQualifiedName~Edulytics.Tests.MathematicsIntelligence."+c+".").join("|");
}
if(require.main===module){
 const action=process.argv[2]||"verify",x=collect();
 if(action==="filter")console.log(filter(process.argv[3],x));
 else if(action==="verify"){
  const n=[...x.map.values()].reduce((sum,a)=>sum+a.length,0);
  console.log("143/143 source IDs have "+n+" Grade 3-specific test classes; D1=48 D2=48 D3=47; no academic approval.");
 }else if(action==="inventory"){
  console.log(JSON.stringify({lessons:143,testClasses:[...x.map.values()].reduce((sum,a)=>sum+a.length,0),batches:Object.fromEntries(["D1","D2","D3"].map(b=>[b,x.table.filter(t=>t.batch===b).map(t=>({ordinal:t.ordinal,classes:x.map.get(t.ordinal)}))]))},null,2));
 }else{throw Error("Usage: verify | inventory | filter D1|D2|D3");}
}
