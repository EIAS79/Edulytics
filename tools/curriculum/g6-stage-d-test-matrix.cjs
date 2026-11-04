'use strict';
// Grade 6 Stage D diagnostic. Never equate a static pass with academic approval.
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const blueprint=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade6-our-2017.lesson-blueprint.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g6-phase29-v1.lesson-content-pack.json');
const abc=read('artifacts/curriculum-delivery/US-CCSS-MATH/G6/grade6-147-academic-authoring-ledger.json');
const d=read('artifacts/curriculum-delivery/US-CCSS-MATH/G6/grade6-stage-d-execution-ledger.json');
const counts=[19,17,17,17,15,19,19,18,6];
const assert=(c,m)=>{if(!c)throw new Error('G6_STAGE_D_FAIL_CLOSED: '+m);};
const dir=path.join(root,'tests/Edulytics.Tests/MathematicsIntelligence');
const suites=new Map();
for(const name of fs.readdirSync(dir)){
 const m=/^UsCcssG6Unit(\d+)Lesson(\d+).*Tests\.cs$/.exec(name);
 if(!m)continue;
 const text=fs.readFileSync(path.join(dir,name),'utf8');
 const c=/\b(?:public|internal)\s+(?:sealed\s+)?class\s+([A-Za-z0-9_]+)/.exec(text);
 if(!c)continue;
 const k=(+m[1])+':'+(+m[2]);
 if(!suites.has(k))suites.set(k,[]);
 suites.get(k).push({file:name,className:c[1]});
}
function build(){
 assert(blueprint.BlueprintCode==='US-CCSS-MATH:G6:OUR-IM-2017','unexpected source');
 assert(blueprint.Lessons.length===147&&blueprint.Units.length===9,'blueprint denominator');
 assert(pack.lessons.length===147&&abc.lessons.length===147,'content denominator');
 assert(d.lessonCount===147&&d.lessons.length===147&&d.units.join(',')===counts.join(','),'D ledger denominator');
 assert(abc.summary.confirmedABCAuthoredClosures===147&&abc.summary.abcAuthoredReadyForStageD===147,'ABC handoff');
 assert(abc.summary.stageDVerified===0&&d.stageDVerified===0,'historic D verification incorrectly claimed');
 const byCode=new Map(pack.lessons.map(x=>[x.lessonCode,x]));
 assert(byCode.size===147,'duplicate pack code');
 const order=[...blueprint.Lessons].sort((a,b)=>a.UnitNumber-b.UnitNumber||a.LessonNumber-b.LessonNumber);
 const rows=[];
 order.forEach((source,i)=>{
  const ordinal=i+1,unit=source.UnitNumber,lesson=source.LessonNumber;
  const code='PED:US-CCSS-MATH:G6:U'+String(unit).padStart(2,'0')+':L'+String(lesson).padStart(2,'0');
  assert(source.LessonCode===code,'blueprint code '+ordinal);
  assert(abc.lessons[i].sortOrder===ordinal&&abc.lessons[i].lessonCode===code,'ABC sequence '+ordinal);
  assert(d.lessons[i].ordinal===ordinal&&d.lessons[i].lessonCode===code,'D sequence '+ordinal);
  assert(abc.lessons[i].abcAuthoringClosure?.readyForStageD===true,'ABC not ready '+ordinal);
  const p=byCode.get(code);
  assert(p&&p.translations?.some(t=>t.cultureCode==='en'),'missing English content '+code);
  assert(p.sourceUrl===source.SourceUrl,'source URL mismatch '+code);
  const file=abc.lessons[i].abcAuthoringClosure.evidenceFile;
  assert(typeof file==='string'&&!path.isAbsolute(file)&&!file.includes('..'),'bad evidence path '+code);
  assert(fs.existsSync(path.join(root,file)),'missing evidence '+code);
  const e=read(file);
  assert(e.lessonCode===code,'evidence code mismatch '+code);
  rows.push({ordinal,code,unit,lesson,evidenceFile:file,testClasses:suites.get(unit+':'+lesson)||[],sourceParity:'UNKNOWN'});
 });
 counts.forEach((n,i)=>assert(rows.filter(x=>x.unit===i+1).length===n,'unit denominator '+(i+1)));
 return rows;
}
const rows=build(),action=process.argv[2],shard=process.argv[3];
if(action==='verify'){
 const missing=rows.filter(x=>!x.testClasses.length);
 assert(missing.length===0,'missing C# lesson tests '+missing.map(x=>x.code).join(','));
 const names=rows.flatMap(x=>x.testClasses.map(y=>y.className));
 assert(new Set(names).size===names.length,'duplicate test classes');
 console.log('G6_STAGE_D_INVENTORY_VALID 147 lessons 9 units '+names.length+' lesson C# suites; source parity UNKNOWN, no academic approval');
}else if(action==='inventory'){
 console.log(JSON.stringify({scope:'G6 OUR-IM-2017',lessonCount:147,counts,sourceParity:'UNKNOWN',rows},null,2));
}else if(action==='filter'){
 const ranges={D1:[1,50],D2:[51,100],D3:[101,147]};
 assert(ranges[shard],'unknown shard '+shard);
 const [first,last]=ranges[shard];
 const selected=rows.filter(x=>x.ordinal>=first&&x.ordinal<=last);
 assert(selected.length===last-first+1,'incomplete shard '+shard);
 const missing=selected.filter(x=>!x.testClasses.length);
 assert(missing.length===0,'missing C# suites in '+shard+': '+missing.map(x=>x.code).join(','));
 const filter=selected.flatMap(x=>x.testClasses.map(t=>'FullyQualifiedName~'+t.className+'.')).join('|');
 assert(filter.length>100&&!/[^A-Za-z0-9._|~]/.test(filter),'invalid test filter');
 process.stdout.write(filter);
}else{throw new Error('Usage: node g6-stage-d-test-matrix.cjs verify|inventory|filter D1|D2|D3');}
