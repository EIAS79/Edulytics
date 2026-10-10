#!/usr/bin/env node
// Read-only US Common Core Mathematics audit. Never modifies curriculum data.
'use strict';
const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const root = path.resolve(__dirname, '..');
const blueprintDir = path.join(root, 'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs');
const registryDir = path.join(root, 'src/Edulytics.Core/Mathematics/Curriculum');
const officialPath = path.join(root, 'docs/curriculum/reference/us-ccss-official-codes.snapshot.json');
const reportPath = path.join(root, 'docs/curriculum/us-ccss-g1-g12-structural-audit.json');
const online = process.argv.includes('--refresh-official');
const strict = process.argv.includes('--strict');
const read = p => JSON.parse(fs.readFileSync(p, 'utf8'));
const uniq = xs => [...new Set(xs)].sort();
const sha = s => crypto.createHash('sha256').update(s).digest('hex');
const normalizeCode = s => s.replace(/^(HS[NAFGS])-([A-Z]{1,3})\./,'$1.$2.');
const prefix = 'https://www.thecorestandards.org';
const gradeDomains = {
  1:['OA','NBT','MD','G'], 2:['OA','NBT','MD','G'],
  3:['OA','NBT','NF','MD','G'], 4:['OA','NBT','NF','MD','G'],
  5:['OA','NBT','NF','MD','G'], 6:['RP','NS','EE','G','SP'],
  7:['RP','NS','EE','G','SP'], 8:['NS','EE','F','G','SP']
};
const highDomains = ['HSN','HSA','HSF','HSG','HSS'];
function officialCodes(html) {
  const matches = [...html.matchAll(/CCSS\.Math\.Content\.((?:[1-8]\.[A-Z]{1,3}\.[A-Z]\.\d+[a-z]?|HS[NAFGS]\.[A-Z]{1,3}\.[A-Z]\.\d+[a-z]?))/g)];
  return uniq(matches.map(m=>m[1]));
}
async function fetchText(url) {
  const r = await fetch(url, {signal:AbortSignal.timeout(25000), headers:{'User-Agent':'Edulytiks-Curriculum-Audit/1.0'}});
  if (!r.ok) throw Error(url+' HTTP '+r.status);
  return r.text();
}
async function refresh() {
  const targets=[];
  for(const [grade, domains] of Object.entries(gradeDomains))
    for(const domain of domains)
      targets.push({url:prefix+'/Math/Content/'+grade+'/'+domain+'/', kind:'grade', partition:grade+'.'+domain});
  const highRoots = await Promise.all(highDomains.map(async d=>({
    domain:d,url:prefix+'/Math/Content/'+d+'/',html:await fetchText(prefix+'/Math/Content/'+d+'/')
  })));
  for(const item of highRoots) {
    const re = /href\s*=\s*["']([^"']+)["']/g;
    const links=[];
    for(const m of item.html.matchAll(re)){
      try{
        const url=new URL(m[1],item.url);
        if(url.hostname!=='www.thecorestandards.org' && url.hostname!=='corestandards.org')continue;
        const pattern=new RegExp('/Math/Content/'+item.domain+'/[A-Z]{1,3}/?$');
        if(pattern.test(url.pathname))links.push(url.origin+url.pathname.replace(/\/?$/,'/'));
      }catch{}
    }
    const unique=uniq(links);
    if(unique.length===0) throw Error('No official high-school clusters discovered for '+item.domain);
    for(const url of unique) targets.push({url, kind:'high',partition:item.domain});
  }
  const partitions={}, errors=[], pageData=[];
  let next=0;
  async function worker(){
    while(next<targets.length){
      const t=targets[next++];
      try{
        const h=await fetchText(t.url);
        const all=officialCodes(h);
        const codes=all.filter(s=>t.kind==='grade' ? s.startsWith(t.partition+'.') : s.startsWith(t.partition+'.'));
        pageData.push({url:t.url,partition:t.partition,count:codes.length,sha256:sha(h),codes});
        partitions[t.partition]=uniq([...(partitions[t.partition]||[]),...codes]);
      }catch(e){errors.push(String(e.message));}
    }
  }
  await Promise.all(Array.from({length:6},()=>worker()));
  const codes=uniq(Object.values(partitions).flat());
  const data={schemaVersion:1,authority:'Common Core State Standards Initiative',
    sourceRoot:prefix+'/Math/Content/', version:'CCSSM-2010',
    retrievedUtc:new Date().toISOString(), pages:pageData.sort((a,b)=>a.url.localeCompare(b.url)),
    errors, codes, counts:{codes:codes.length, pages:pageData.length}};
  fs.mkdirSync(path.dirname(officialPath),{recursive:true});
  fs.writeFileSync(officialPath,JSON.stringify(data,null,2)+'\n');
  return data;
}
async function main() {
  const official=online ? await refresh() : (fs.existsSync(officialPath)?read(officialPath):null);
  const map=read(path.join(registryDir,'official-outcome-practice-map.v1.json'));
  const rules=read(path.join(registryDir,'supporting-practice-target-rules.v1.json'));
  const skills=read(path.join(registryDir,'lesson-skill-mappings.v1.json'));
  const rulesById=new Set(rules.rules.map(r=>r.id));
  const mappedCodes=uniq(map.entries.filter(e=>e.outcomeCode.startsWith('CCSS:')).map(e=>normalizeCode(e.outcomeCode.slice(5))));
  const allowedGrades = s => /^[1-8]\./.test(s)||/^HS[NAFGS]\./.test(s);
  const liveCodes=mappedCodes.filter(allowedGrades);
  const officialCodesInScope=official ? official.codes.filter(allowedGrades) : [];
  const missing=official ? officialCodesInScope.filter(c=>!liveCodes.includes(c)) : null;
  const extra=official ? liveCodes.filter(c=>!officialCodesInScope.includes(c)) : null;
  const blueprints=fs.readdirSync(blueprintDir).filter(n=>/^us-ccss-math-.*\.lesson-blueprint\.json$/.test(n));
  const all=[], sourceProblems=[], contentCodes=new Set(), gradeCounts={};
  for(const file of blueprints){
    const b=read(path.join(blueprintDir,file)), lessons=b.Lessons || [];
    for(const l of lessons){
      const codes=uniq([...(l.OutcomeCodes||[]),...(l.FormalTargets||[]).map(t=>t.OutcomeCode)].filter(c=>c&&c.startsWith('CCSS:')).map(c=>normalizeCode(c.slice(5))));
      const address=uniq((l.Alignments||[]).filter(a=>a.Role==='Addressing'&&a.OutcomeCode).map(a=>normalizeCode(a.OutcomeCode.slice(5))));
      for(const c of codes) contentCodes.add(c);
      if(address.some(c=>!codes.includes(c)))sourceProblems.push({lessonCode:l.LessonCode,problem:'addressing_missing_outcome',codes:address.filter(c=>!codes.includes(c))});
      if(codes.some(c=>!mappedCodes.includes(c) && !/^MP\./.test(c)))sourceProblems.push({lessonCode:l.LessonCode,problem:'outcome_missing_practice_map',codes:codes.filter(c=>!mappedCodes.includes(c)&&!/^MP\./.test(c))});
      all.push({lessonCode:l.LessonCode,title:l.Title,blueprint:file,
        sourceUrl:l.SourceUrl,gradeOrCourse:b.NativeLevel,
        officialCodes:codes,alignmentRoles:uniq((l.Alignments||[]).map(a=>a.Role)),
        practicesOnly:codes.length>0 && codes.every(c=>c.startsWith('MP.')),
        noOutcome:codes.length===0});
    }
    gradeCounts[b.NativeLevel||file]=(gradeCounts[b.NativeLevel||file]||0)+lessons.length;
  }
  const noOutcome=all.filter(x=>x.noOutcome);
  const onlyMP=all.filter(x=>x.practicesOnly);
  const badRules=map.entries.filter(x=>!rulesById.has(x.targetRuleId));
  const missingRegistryOutcome=[...contentCodes].filter(c=>!c.startsWith('MP.')&&!mappedCodes.includes(c));
  const explicitUS=skills.mappings.filter(x=>String(x.lessonCode).startsWith('PED:US-CCSS-MATH:'));
  const report={schemaVersion:1,sourceCommit:process.env.GITHUB_SHA||require('node:child_process').execFileSync('git',['rev-parse','HEAD'],{cwd:root,encoding:'utf8'}).trim(),generatedUtc:new Date().toISOString(),
    scope:'US Common Core grade 1–8 and traditional high-school courses, plus supplements; Kindergarten excluded',
    official:{snapshot:officialPath.replace(root+path.sep,'').replaceAll('\\','/'),available:!!official,
      pageCount:official?.pages.length??0,downloadErrors:official?.errors||[],
      codesTotal:officialCodesInScope.length,codesInEngine:liveCodes.length,
      missingInEngine:missing,engineCodesNotOnOfficialPages:extra},
    lessonInventory:{blueprints:blueprints.length,lessons:all.length,grades:gradeCounts,
      noOutcomeCount:noOutcome.length,practicesOnlyCount:onlyMP.length,
      noOutcomeLessons:noOutcome.map(x=>({lessonCode:x.lessonCode,title:x.title,gradeOrCourse:x.gradeOrCourse})),
      practicesOnlyLessons:onlyMP.map(x=>({lessonCode:x.lessonCode,title:x.title,gradeOrCourse:x.gradeOrCourse}))},
    skillMap:{contentOutcomesMapped:liveCodes.length,unknownTargetRuleCount:badRules.length,
      unknownTargetRules:badRules.map(x=>x.outcomeCode),
      explicitUSLessonMappings:explicitUS.length,missingRegistryOutcome},
    blueprintAlignmentProblems:sourceProblems,
    gates:{
      officialInventory:official&&official.errors.length===0&&missing.length===0&&extra.length===0?'PASS':'BLOCKED',
      lessonSourceStructure:sourceProblems.length===0?'PASS':'BLOCKED',
      semantics:'NOT_VERIFIED',practiceRuntime:'NOT_VERIFIED',
      highSchoolPathway:'TRADITIONAL_ONLY',
      productionReady:false
    },
    limitations:[
      'Official website code-presence diff is not an independent word-for-word comparison of OfficialText in Neon.',
      'Blueprint mappings are historical provenance assertions; semantic correctness needs independently reviewed source evidence.',
      'Explicit lesson-skill map count is not the count of practice-ready lessons; projection requires runtime exercise.',
      'No production deployment, DB mutation or student mastery alteration is performed by this script.'
    ]};
  fs.mkdirSync(path.dirname(reportPath),{recursive:true});
  fs.writeFileSync(reportPath,JSON.stringify(report,null,2)+'\n');
  console.log(JSON.stringify({official:report.official,lessons:all.length,noOutcome:noOutcome.length,
    mpOnly:onlyMP.length,blueprintProblems:sourceProblems.length,skillMap:report.skillMap,gates:report.gates},null,2));
  if(strict&&(report.gates.officialInventory!=='PASS'||report.gates.lessonSourceStructure!=='PASS')) process.exitCode=1;
}
main().catch(e=>{console.error(e);process.exitCode=1;});