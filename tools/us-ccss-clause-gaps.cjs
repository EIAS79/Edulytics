#!/usr/bin/env node
'use strict';
// PHASE 05: evidence gaps for all 121 CCSS child clauses; candidates not endorsements.
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const ledger=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/us-ccss-academic-evidence-ledger.json'),'utf8'));
const rows=ledger.clauseEvidence;
if(rows.length!==121)throw Error('Expected 121 subordinate clauses');
const group=c=>String(c.parent).startsWith('HS')?'HS':String(c.parent).split('.')[0];
const groups=new Map();
for(const row of rows){
 const k=group(row);
 if(!groups.has(k))groups.set(k,{total:0,withoutCandidate:0,directPublisher:0,certified:0});
 const g=groups.get(k);g.total++;
 if(row.assessmentEvidenceStatus==='NO_CANDIDATE_READY_ROUTED_FAMILY')g.withoutCandidate++;
 if(row.exactSourceReferenceCount>0)g.directPublisher++;
 if(row.certificationStatus==='ACADEMICALLY_CERTIFIED')g.certified++;
}
const noCandidate=rows.filter(r=>r.assessmentEvidenceStatus==='NO_CANDIDATE_READY_ROUTED_FAMILY');
const parentOnly=rows.filter(r=>r.exactSourceReferenceCount===0);
if(noCandidate.length!==63||parentOnly.length!==15||rows.some(r=>r.certificationStatus==='ACADEMICALLY_CERTIFIED'))
 throw Error('Clause ledger drift');
const lines=[
 '# PHASE 05 — US Common Core subordinate-clause evidence gaps',
 '',
 '**Gate 05: NO-GO. This is a missing evidence report, not a certification report.**',
 '',
 '63 clauses have no mapped READY_VERIFIED candidate in the current ledger, 58 have a candidate. None of the 121 is independently academically certified. A candidate Practice question is not equivalent to the required concept, construction, explanation, graph or proof.',
 '',
 '| Grade/group | Child clauses | No READY_VERIFIED candidate | Direct publisher child reference | Academically signed |',
 '|---|---:|---:|---:|---:|',
 ...[...groups].sort((a,b)=>a[0]==='HS'?1:b[0]==='HS'?-1:Number(a[0])-Number(b[0])).map(([grade,x])=>
  '| '+grade+' | '+x.total+' | '+x.withoutCandidate+' | '+x.directPublisher+' | '+x.certified+' |'),
 '',
 '## No ready routed question family (63; evidence deficit, not proof source lesson is absent)',
 '',
 ...noCandidate.map(r=>'- '+r.clause+' — '+r.parent+
  '; publisher status: '+r.sourceReferenceStatus+
  '; lesson-to-clause review, exact SkillContract and assessment rubric required'),
 '',
 '## Parent-only publisher source references (15; do not invent exact alignment)',
 '',
 ...parentOnly.map(r=>'- '+r.clause+' — '+r.parent+
  '; original source must be checked for the exact child requirement'),
 '',
 '## Academic closure rule',
 '',
 'Every parent standard (363) and child clause (121) requires source-anchored teaching, competency, question/rubric or teacher-reviewed constructed-response evidence, negative mathematical tests, and independent reviewer signature. This report is a triage list only; all 121 signoffs remain outstanding.',
 ''
];
const dst=path.join(root,'artifacts/academic-us-ccss/05-clause-coverage-gaps.md'),text=lines.join('\n');
if(process.argv.includes('--write')){fs.mkdirSync(path.dirname(dst),{recursive:true});fs.writeFileSync(dst,text);}
else if(!fs.existsSync(dst)||fs.readFileSync(dst,'utf8')!==text)
 throw Error('Clause gap artifact drift');
console.log(JSON.stringify({clauses:rows.length,noReadyCandidate:noCandidate.length,
 parentOnly:parentOnly.length,independentlyCertified:0}));
