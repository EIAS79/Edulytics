'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const dir='src/Edulytics.Core/Curriculum/LessonContent/RichV2';
const docs=fs.readdirSync(path.join(root,dir)).filter(x=>/^us-ccss-g8-.*\.rich-lesson-v2\.json$/.test(x)).map(x=>read(dir+'/'+x));
const lesson=new Map(docs.flatMap(x=>x.Lessons).map(x=>[x.LessonCode,x]));
const report=read('docs/curriculum/us-ccss-g8-coder4-v10-original-math-representations-20261029.json');
const publisher=read('docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json');
const source=new Map(publisher.rows.map(x=>[x.lessonCode,x]));
const evidence=read('docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json');
const sha=x=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,x))).digest('hex');
test('all 131 first-party Grade 8 lesson records and 66 authored exact worked-example visuals retain unique source identity',()=>{
 assert.equal(lesson.size,131);
 assert.equal(report.count,66);
 assert.equal(report.rows.length,66);
 assert.equal(new Set(report.rows.map(x=>x.lessonCode)).size,66);
 assert.deepEqual([...new Set(report.rows.map(x=>x.lessonCode.match(/:U(\d+):/)[1]))].sort(),['01','02','03','04','05','06','07','08','09']);
 let checked=0;
 for(const record of report.rows){
  const l=lesson.get(record.lessonCode),s=source.get(record.lessonCode);
  assert.ok(l&&s,record.lessonCode);
  assert.equal(record.sourceStudentUrl,s.sourceStudentUrl);
  assert.ok(l.WorkedExamples.some(x=>x.Question.includes(record.authoredExampleAnchor)),record.lessonCode);
  assert.ok(l.Visuals.length>=1,'original V10 visual must remain: '+record.lessonCode); // Later course-owned visuals may be appended; visual[0] must remain identical to V10 evidence.
  const v=l.Visuals[0];
  assert.deepEqual(v,record.visuals[0],record.lessonCode);
  assert.equal(v.Kind,'RatioTable');
  assert.ok(v.Title.length>10);
  assert.ok(v.Description.length>30);
  assert.equal(v.Rows.length>=2,true);
  const width=v.Rows[0].Cells.length;
  assert.ok(width>=1&&width<=6,record.lessonCode);
  assert.ok(v.Rows.every(row=>row.Cells.length===width&&row.Cells.every(cell=>String(cell).trim()!=='')&&row.Label.trim()!==''));
  assert.ok(!JSON.stringify(v).includes('cms-assets.illustrativemathematics.org'));
  const by=new Map(v.Rows.map(row=>[row.Label,row.Cells]));
  if(by.size===2&&by.has('x')&&by.has('y')){
   const m=v.Description.match(/Each row uses y = (-?\d+(?:\.\d+)?)x ([+−]) (\d+(?:\.\d+)?)\./);
   assert.ok(m,record.lessonCode+' '+v.Description);
   const slope=Number(m[1]),intercept=(m[2]==='−'?-1:1)*Number(m[3]);
   for(let j=0;j<width;j++){
    const x=Number(by.get('x')[j]),y=Number(by.get('y')[j]);
    assert.ok(Math.abs(y-(slope*x+intercept))<1e-9,record.lessonCode);
   }
   checked++;
  }
  if(by.has('vertex')&&by.has('new x')){
   const dx=Number(by.get('new x')[0])-Number(by.get('x')[0]);
   const dy=Number(by.get('new y')[0])-Number(by.get('y')[0]);
   for(let j=0;j<width;j++){
    assert.equal(Number(by.get('new x')[j])-Number(by.get('x')[j]),dx);
    assert.equal(Number(by.get('new y')[j])-Number(by.get('y')[j]),dy);
   }
   checked++;
  }
  if(by.has('exponent')&&by.has('value')){
   const m=v.Description.match(/nonzero base is (\d+(?:\.\d+)?)/);
   assert.ok(m,record.lessonCode);
   for(let j=0;j<width;j++)assert.ok(Math.abs(Number(by.get('value')[j])-Number(m[1])**Number(by.get('exponent')[j]))<1e-9);
   checked++;
  }
  if(by.has('a')&&by.has('b')&&by.has('a²+b²')&&by.has('c')){
   for(let j=0;j<width;j++){
    const a=Number(by.get('a')[j]),b=Number(by.get('b')[j]),c=Number(by.get('c')[j]);
    assert.equal(a*a+b*b,Number(by.get('a²+b²')[j]));
    assert.equal(c*c,a*a+b*b);
   }
   checked++;
  }
  // Independent arithmetic oracles for source-aligned custom presentations.
  const nums=label=>by.get(label)?.map(Number);
  if(by.has('x')&&by.has('x²')){
   for(let j=0;j<width;j++)assert.equal(nums('x²')[j],nums('x')[j]**2);checked++;
  }
  if(by.has('x')&&by.has('y=x²')){
   for(let j=0;j<width;j++)assert.equal(nums('y=x²')[j],nums('x')[j]**2);checked++;
  }
  if(by.has('x')&&by.has('y')&&by.has('2x+3y')){
   for(let j=0;j<width;j++)assert.equal(nums('2x+3y')[j],2*nums('x')[j]+3*nums('y')[j]);checked++;
  }
  if(by.has('state')&&by.has('length')&&by.has('width')&&by.has('area')){
   for(let j=0;j<width;j++)assert.equal(nums('area')[j],nums('length')[j]*nums('width')[j]);checked++;
  }
  if(by.has('x')&&by.has('left')&&by.has('right')&&by.has('equal?')){
   for(let j=0;j<width;j++){
    assert.equal(nums('left')[j],2*nums('x')[j]+3);
    assert.equal(by.get('equal?')[j],nums('left')[j]===nums('right')[j]?'Yes':'No');
   }checked++;
  }
  if(by.has('5x+2')&&by.has('2x+14')){
   for(let j=0;j<width;j++){
    assert.equal(nums('5x+2')[j],5*nums('x')[j]+2);
    assert.equal(nums('2x+14')[j],2*nums('x')[j]+14);
   }checked++;
  }
  if(by.has('2(x+3)')&&by.has('2x+6')){
   for(let j=0;j<width;j++){
    assert.equal(nums('2(x+3)')[j],2*(nums('x')[j]+3));
    assert.equal(nums('2x+6')[j],2*nums('x')[j]+6);
    assert.equal(nums('2x+1')[j],2*nums('x')[j]+1);
    assert.equal(nums('2x+5')[j],2*nums('x')[j]+5);
   }checked++;
  }
  if(by.has('months')&&by.has('A cost')&&by.has('B cost')){
   for(let j=0;j<width;j++){
    assert.equal(nums('A cost')[j],6+3*nums('months')[j]);
    assert.equal(nums('B cost')[j],18+nums('months')[j]);
   }checked++;
  }
  if(by.has('y=x+1')&&by.has('y=−x+5')){
   for(let j=0;j<width;j++){
    assert.equal(nums('y=x+1')[j],nums('x')[j]+1);
    assert.equal(nums('y=−x+5')[j],5-nums('x')[j]);
   }checked++;
  }
  if(by.has('y=2x+1')&&by.has('y=2x−3')){
   for(let j=0;j<width;j++){
    assert.equal(nums('y=2x+1')[j],2*nums('x')[j]+1);
    assert.equal(nums('y=2x−3')[j],2*nums('x')[j]-3);
   }checked++;
  }
  if(by.has('y=2x+1')&&by.has('x+y')){
   for(let j=0;j<width;j++){
    assert.equal(nums('y=2x+1')[j],2*nums('x')[j]+1);
    assert.equal(nums('x+y')[j],nums('x')[j]+nums('y=2x+1')[j]);
   }checked++;
  }
  if(by.has('x+y')&&by.has('x−y')){
   for(let j=0;j<width;j++){
    assert.equal(nums('x+y')[j],nums('x')[j]+nums('y')[j]);
    assert.equal(nums('x−y')[j],nums('x')[j]-nums('y')[j]);
   }checked++;
  }
  if(by.has('apples')&&by.has('pears')&&by.has('cost')){
   for(let j=0;j<width;j++){
    assert.equal(nums('number')[j],nums('apples')[j]+nums('pears')[j]);
    assert.equal(nums('cost')[j],2*nums('apples')[j]+3*nums('pears')[j]);
   }checked++;
  }
  if(by.has('t')&&by.has('rule')&&by.has('f(t)')){
   for(let j=0;j<width;j++){
    const t=nums('t')[j];
    assert.equal(nums('f(t)')[j],t<=2?2*t:4);
   }checked++;
  }
  if(by.has('volume (cm³)')&&by.has('height (cm)')){
   for(let j=0;j<width;j++)assert.equal(nums('height (cm)')[j]*20,nums('volume (cm³)')[j]);checked++;
  }
  if(by.has('height')&&by.has('V/π')){
   for(let j=0;j<width;j++)assert.equal(nums('V/π')[j],nums('height')[j]*9);checked++;
  }
  if(by.has('height h')&&by.has('V/π')){
   for(let j=0;j<width;j++)assert.equal(nums('V/π')[j],nums('height h')[j]*4);checked++;
  }
  if(by.has('radius')&&by.has('V/π')){
   for(let j=0;j<width;j++){
    const exact=(4*nums('radius')[j]**3)/3;
    const value=by.get('V/π')[j].includes('/')?by.get('V/π')[j].split('/').map(Number).reduce((a,b)=>a/b):Number(by.get('V/π')[j]);
    assert.ok(Math.abs(value-exact)<1e-9);
   }checked++;
  }
  if(by.has('observed')&&by.has('predicted')&&by.has('residual')){
   for(let j=0;j<width;j++)assert.equal(nums('residual')[j],nums('observed')[j]-nums('predicted')[j]);checked++;
  }
  if(by.has('edge')&&by.has('area')){
   for(let j=0;j<width;j++)assert.equal(nums('area')[j],nums('edge')[j]**2);checked++;
  }
  if(by.has('edge')&&by.has('volume')){
   for(let j=0;j<width;j++)assert.equal(nums('volume')[j],nums('edge')[j]**3);checked++;
  }
  if(by.has('cube root')){
   for(let j=0;j<width;j++)assert.equal(nums('cube root')[j]**3,nums('x')[j]);checked++;
  }
  if(by.has('number')&&by.has('degrees total')){
   for(let j=0;j<width;j++)assert.equal(nums('degrees total')[j],120*nums('number')[j]);checked++;
  }
  if(by.has('corner angle')&&by.has('sum')){
   for(let j=0;j<width;j++)assert.equal(nums('sum')[j],nums('corner angle')[j]*nums('number')[j]);checked++;
  }

 }
 assert.ok(checked>=45, 'independent arithmetic checks must cover most representations');
 const other=lesson.get('PED:US-CCSS-MATH:G8:U03:L12');
 assert.equal(other.Visuals.length,2,'preserve existing two-variable lesson instead of overwriting');
 const withVisuals=[...lesson.values()].filter(x=>x.Visuals.length>0);
 assert.equal(withVisuals.length,67);
 for(const name of Object.keys(evidence.hashes.richCandidates))
  assert.equal(evidence.hashes.richCandidates[name],sha(dir+'/'+name),name);
});
test('source figure priorities are preserved without automatic A-D acceptance',()=>{
 const v9=read('docs/curriculum/us-ccss-g8-coder4-v9-acceptance-ledger-20261029.json');
 assert.equal(v9.counts.AAccepted,0);assert.equal(v9.counts.BAccepted,0);
 assert.equal(v9.counts.CVerified,0);assert.equal(v9.counts.DVerified,0);
 const figures=new Set(publisher.rows.filter(x=>x.uniqueMathFiguresInActivities>0).map(x=>x.lessonCode));
 assert.equal(figures.size,113);
 const enriched=report.rows.filter(x=>figures.has(x.lessonCode)).length;
 assert.ok(enriched>30);
 for(const row of report.rows){
  const l=lesson.get(row.lessonCode);
  assert.equal(l.Visuals[0]?.Kind,'RatioTable','historic V10 primary visual preserved; newer supported course-owned kinds may follow');
 }
});
