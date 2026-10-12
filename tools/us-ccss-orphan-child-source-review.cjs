#!/usr/bin/env node
'use strict';
// Phase05/07: 15 unresolved CCSS subclauses, strict publisher provenance.
// Source parent Addressing is NOT child Addressing, and a registered exact
// question-family is NOT independent academic competency certification.
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const roster=load('artifacts/academic-us-ccss/05-orphan-child-source-gap-review.json');
const norm=x=>String(x||'').replace(/^CCSS:/i,'').trim().toLowerCase();
const must=(v,m)=>{if(!v)throw Error(m);};
const expectedNoParent=[
 'HSN-VM.B.4.a','HSN-VM.B.4.b','HSN-VM.B.4.c',
 'HSN-VM.B.5.a','HSN-VM.B.5.b','HSS-MD.B.5.a','HSS-MD.B.5.b'];
function audit(candidateRoster=roster){
 const official=load('artifacts/academic-us-ccss/01-authority-registry.json');
 const reconciled=load('artifacts/academic-us-ccss/05-clause-publisher-reference-reconciliation.json');
 const legacy=load('docs/curriculum/us-ccss-121-clause-publisher-evidence.json');
 const familyRegistry=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
 const g3shadow=load('artifacts/academic-us-ccss/05-three-orphan-child-math-shadow-candidates.json');
 const dir='src/Edulytics.Core/Curriculum/LessonBlueprints/Packs';
 const names=fs.readdirSync(path.join(root,dir))
   .filter(x=>/^us-ccss-.*\.lesson-blueprint\.json$/.test(x)).sort();
 must(names.length===17,'Expected all 17 publisher blueprint editions');
 const lessons=names.flatMap(name=>load(dir+'/'+name).Lessons);
 must(lessons.length===1560,'1560 canonical publisher lesson count drift');
 must(reconciled.summary.clauses===121 &&
  reconciled.summary.publisherExactReferenced===106 &&
  reconciled.summary.publisherUnresolved===15 &&
  official.subordinateClauses.length===121 &&
  legacy.clauses.length===121,'Publisher/official clause ledger drift');
 must(candidateRoster.publicationDecision==='NO-GO' &&
  candidateRoster.independentMathSignoffs===0 &&
  candidateRoster.publisherExactChildReferencesAdded===0,
  'Unreviewed child clauses cannot be signed off by this shadow roster');
 must(candidateRoster.candidates.length===15 &&
  new Set(candidateRoster.candidates.map(x=>norm(x.clause))).size===15,
  'Expected exactly 15 unique source-gap review candidates');
 const unresolved=reconciled.clauses.filter(x=>x.publisherExactReferences===0);
 must(unresolved.length===15,'Publisher unresolved count changed');
 must(unresolved.every(x=>candidateRoster.candidates.some(y=>
  norm(y.clause)===norm(x.clause))), 'Official unresolved code absent from review roster');
 const output=[];
 for(const item of candidateRoster.candidates){
  const c=reconciled.clauses.find(x=>norm(x.clause)===norm(item.clause));
  const auth=official.subordinateClauses.find(x=>norm(x.clauseCode)===norm(item.clause));
  const old=legacy.clauses.find(x=>norm(x.clause)===norm(item.clause));
  must(c && auth && old && c.publisherExactReferences===0 &&
    auth.exactPublisherSourceRefs===0 &&
    old.publisherExactAddressingCount===0,
    'Publisher child source changed; refuse stale candidate '+item.clause);
  must(norm(c.parent)===norm(auth.parentCode) &&
    norm(c.parent)===norm(old.parent) &&
    old.officialUrl===auth.officialPageUrl &&
    old.officialUrl===c.officialUrl &&
    old.officialText.length>25,
    'Official text/page/parent identity drift '+item.clause);
  must(item.sourceChildAddressing===false &&
    item.academicApproved===false &&
    item.reviewerId===null,
    'A candidate cannot self-authorize source or academic review '+item.clause);
  const actualChild=lessons.flatMap(l=>(l.Alignments||[])
   .filter(a=>a.Role==='Addressing' &&
    norm(a.ReferenceCode)===norm(item.clause)).map(a=>l.LessonCode));
  must(actualChild.length===0,'New exact child publisher reference needs evidence review '+item.clause);
  const parentHits=lessons.flatMap(l=>(l.Alignments||[])
   .filter(a=>a.Role==='Addressing' &&
    norm(a.ReferenceCode)===norm(c.parent))
   .map(a=>({lessonCode:l.LessonCode,title:l.Title,sourceUrl:l.SourceUrl,
    reference:a.ReferenceCode})));
  const family=familyRegistry.families.find(f=>f.id===item.candidateFamily);
  must(family && family.status==='ShadowVerified' &&
    family.productionRouting===false &&
    typeof family.lessonPracticeRouting==='boolean',
    'Not a strictly shadow candidate family '+item.candidateFamily);
  output.push({clause:item.clause,parent:c.parent,
   officialText:old.officialText,authorityUrl:old.officialUrl,
   publisherExactChildAddressing:0,
   exactParentAddressingCount:parentHits.length,
   exampleParentOnlyLesson:parentHits[0]||null,
   historicalParentLinkedLessonCount:old.parentLinkedLessonCount,
   candidateFamily:item.candidateFamily,
   lessonPracticeRoutingMetadata:family.lessonPracticeRouting,
   productionRoutingMetadata:family.productionRouting,
   sourceStatus:parentHits.length?
    'PUBLISHER_PARENT_ONLY_NOT_CHILD':'NO_PUBLISHER_PARENT_ADDRESSING',
   reviewStatus:'PENDING_INDEPENDENT_MATH_AND_SOURCE_REVIEW',
   humanReviewerId:null,certified:false});
 }
 const noParent=output.filter(x=>x.exactParentAddressingCount===0)
  .map(x=>x.clause);
 must(JSON.stringify(noParent)===JSON.stringify(expectedNoParent),
  'Seven ungrounded HS vector/probability source gaps changed');
 must(output.filter(x=>x.exactParentAddressingCount>0).length===8,
  'Eight parent-only source candidate count changed');
 must(g3shadow.clauses.length===3 &&
  g3shadow.studentPracticeRouting===false,
  'Preexisting three primitive numeric-only shadow probes changed');
 return {scope:candidateRoster.scope,publicationDecision:'NO-GO',
  officialTotal:121,publisherChildExact:106,publisherChildUnresolved:15,
  sourceParentAddressingFound:8,sourceParentAddressingAbsent:7,
  candidateFamilies:15,independentReviewerSignoffs:0,
  candidates:output};
}
if(require.main===module){
 if(!process.argv.includes('--check'))throw Error('Use explicit --check');
 console.log(JSON.stringify(audit(),null,2));
}
module.exports={audit,roster};
