'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const plot=require(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u01-l01-source-plot-builder.js'));
const tasks=JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u01l01-original-61-parallel-skill-practice.json'),'utf8')).records;
const rawTimes=['14:20','14:55','15:10','15:45','16:05','16:40','17:00','17:30','18:15','18:45','19:10','20:25','20:50','21:30'];
const seconds=raw=>{const [m,s]=raw.split(':').map(Number);return 60*m+s;};
const arabic=raw=>raw.replace(/\d/g,ch=>String.fromCharCode(0x0660+Number(ch)));
const persian=raw=>raw.replace(/\d/g,ch=>String.fromCharCode(0x06f0+Number(ch)));
const normalize=text=>String(text)
  .replace(/[\u0660-\u0669]/g,ch=>String(ch.charCodeAt(0)-0x0660))
  .replace(/[\u06f0-\u06f9]/g,ch=>String(ch.charCodeAt(0)-0x06f0));

test('Q48 source representation is minute:second data, not invented official time values or decimal-only graph',()=>{
  assert.equal(tasks.length,61);
  const item=tasks[47];
  assert.equal(item.sourceItem,'U01L01-Q48');
  assert.equal(item.printedPage,9);
  assert.equal(item.timeNotation,'mm:ss');
  assert.equal(item.originalTimeNotationSourceMatched,true);
  assert.equal(item.sourceNumericalValuesReused,false);
  assert.equal(item.gradingRoute,'TEACHER_REVIEW_REQUIRED');
  assert.equal((normalize(item.studentPrompt).match(/\b\d{2}:\d{2}\b/g)||[]).length,14);
  for(const raw of rawTimes) assert.ok(normalize(item.studentPrompt).includes(raw));
  for(const out of ['14:20','15:45','17:15','19:10','21:30','3:25','7:10'])
    assert.ok(normalize(item.teacherExpectedEvidence).includes(out),out);
  const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u01-l01-original-61-practice-ar.html'),'utf8');
  assert.ok(html.includes(item.studentPrompt));
});

test('Independent 14-time raw data yield exact integer-second quartiles and correct whiskers',()=>{
  assert.equal(rawTimes.length,14);
  const sorted=rawTimes.map(seconds);
  assert.ok(sorted.every((v,i)=>!i||sorted[i-1]<=v));
  const median=(a,b)=>(a+b)/2;
  const expected=[sorted[0],sorted[3],median(sorted[6],sorted[7]),sorted[10],sorted[13]];
  assert.deepEqual(expected,[seconds('14:20'),seconds('15:45'),seconds('17:15'),seconds('19:10'),seconds('21:30')]);
  assert.equal(expected[3]-expected[1],seconds('3:25'));
  assert.equal(expected[4]-expected[0],seconds('7:10'));
});

test('Q48 minute-second SVG validator accepts Eastern Arabic/Persian digits, preserves raw entries and rejects invalid seconds',()=>{
  const five=['14:20','15:45','17:15','19:10','21:30'];
  for(const values of [five,five.map(arabic),five.map(persian)]) {
    const res=plot.validate('U01L01-Q48',values,'Times');
    assert.equal(res.ok,true);
    assert.equal(res.graded,false);
    assert.equal(res.review,'TEACHER_REVIEW_REQUIRED');
    assert.equal(res.timeFormat,'mm:ss');
    assert.equal(res.rawEntries.length,5);
    assert.deepEqual(res.values.map(x=>Math.round(x*60)),five.map(seconds));
  }
  for(const input of ['14:60','-2:10','14:6','14:aa','14:99','1000:00','14:12<script>','14:12.5']){
    const bad=plot.validate('U01L01-Q48',[input,'15:45','17:15','19:10','21:30'],'');
    assert.equal(bad.ok,false,input);
  }
  assert.equal(plot.validate('U01L01-Q48',['14:20',15.75,'17:15','19:10','21:30'],'').ok,false);
  assert.equal(plot.validate('U01L01-Q48',['14:20','15:45','21:30','19:10','21:30'],'').ok,false);
  // Q47 must never accept mm:ss values as integer vote counts.
  assert.equal(plot.validate('U01L01-Q47',['14:20',2,3,4,5,6,7],'Original votes').ok,false);
});

test('Q48 SVG axis uses minute-second labels without leaking expected answers or enabling autograding',()=>{
  const doc={
    createElementNS(_namespace,tag){
      return {tagName:tag,attrs:{},children:[],textContent:'',setAttribute(key,value){this.attrs[key]=String(value)},append(node){this.children.push(node)}};
    }
  };
  const values=['14:20','15:45','17:15','19:10','21:30'];
  const res=plot.validate('U01L01-Q48',values,'My minute:second box');
  const svg=plot.draw(doc,res);
  assert.equal(svg.tagName,'svg');
  assert.equal(svg.attrs.role,'img');
  assert.ok(svg.children.some(n=>n.tagName==='text' && n.textContent==='16:00'));
  assert.ok(svg.children.some(n=>n.tagName==='text' && /^\d+:\d\d$/.test(n.textContent)));
  assert.equal(res.graded,false);
  assert.ok(!JSON.stringify(svg).includes('grade'));
});
