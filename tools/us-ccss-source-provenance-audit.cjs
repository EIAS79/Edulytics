#!/usr/bin/env node
'use strict';
// PHASE 00/01: source metadata audit, not copyright clearance or academic signoff.
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto'),cp=require('node:child_process');
const root=path.resolve(__dirname,'..'), writing=process.argv.includes('--write');
const rel={
 manifest:'artifacts/academic-us-ccss/00-source-manifest.json',
 authority:'artifacts/academic-us-ccss/01-authority-registry.json',
 publisher:'artifacts/academic-us-ccss/01-publisher-provenance.json',
 licensing:'artifacts/academic-us-ccss/01-licensing-review.md'
};
const norm=s=>String(s||'').replace(/^CCSS:/,'').replace(/^(HS[NAFGS])-([A-Z]{1,3})\./,'$1.$2.');
const sha=cp.execFileSync('git',['rev-parse','HEAD'],{cwd:root,encoding:'utf8'}).trim();
const sources={};
function read(p){const b=fs.readFileSync(path.join(root,p));sources[p]={sha256:crypto.createHash('sha256').update(b).digest('hex'),bytes:b.length};return JSON.parse(b.toString('utf8'));}
const reference='docs/curriculum/reference/us-ccss-official-codes.snapshot.json';
const childrenPath='docs/curriculum/us-ccss-121-clause-evidence-corrected.json';
const ref=read(reference),children=read(childrenPath);
if(ref.codes.length!==363||children.clauses.length!==121||ref.version!=='CCSSM-2010')throw Error('Source count/version drift');
const pageByCode=new Map();
for(const p of ref.pages){
 if(!/^https:\/\//.test(p.url)||! /^[a-f0-9]{64}$/.test(p.sha256))throw Error('Incomplete official page '+p.partition);
 for(const c of p.codes){const k=norm(c);if(pageByCode.has(k))throw Error('Duplicate parent '+k);pageByCode.set(k,p);}
}
if(pageByCode.size!==363)throw Error('Source pages do not cover all 363 parents');
const authorityRows=ref.codes.map(c=>{const p=pageByCode.get(norm(c));if(!p)throw Error('No page '+c);
 return {standardCode:'CCSS:'+c,officialPageUrl:p.url,capturedPageSha256:p.sha256,capturedAtUtc:ref.retrievedUtc,
 reviewState:'SOURCE_SNAPSHOT_PRESENT_ACADEMIC_REVIEW_REQUIRED',reviewerId:null};});
const clauseRows=children.clauses.map(c=>{const p=pageByCode.get(norm(c.parent));if(!p)throw Error('No clause parent '+c.clause);
 return {clauseCode:c.clause,parentCode:c.parent,officialPageUrl:p.url,capturedPageSha256:p.sha256,
 publisherAlignmentDeclared:c.sourceReferenceStatus,exactPublisherSourceRefs:c.exactSourceReferenceCount,
 publisherSample:c.examplePublisherReference||null,reviewState:'PENDING_CLAUSE_SPECIFIC_ACADEMIC_REVIEW',reviewerId:null};});
const dirname='src/Edulytics.Core/Curriculum/LessonBlueprints/Packs';
const filenames=fs.readdirSync(path.join(root,dirname)).filter(n=>/^us-ccss-math-.*\.lesson-blueprint\.json$/.test(n)).sort();
if(filenames.length!==17)throw Error('Expected 17 publisher blueprint files');
const blueprints=[],lessons=[],seen=new Set();
for(const name of filenames){
 const file=dirname+'/'+name,b=read(file);
 if(b.PackCode!=='US-CCSS-MATH'||b.VersionCode!=='CCSSM-2010'||!Array.isArray(b.Lessons))
  throw Error('Unexpected blueprint '+file);
 // V2 high-school blueprints place legal/source metadata in Sources[].
 // Do not infer a lesson's official publisher attribution merely from a shared site.
 const declaredSources=Array.isArray(b.Sources)&&b.Sources.length
  ? b.Sources.map(s=>({sourceKey:s.SourceKey||null,title:s.Title||null,
    publisher:s.Publisher||null,edition:s.Edition||null,rootUrl:s.RootUrl||null,
    licenseClaim:s.License||null,rightsNote:s.RightsNote||null,
    attribution:s.RequiredDigitalAttribution||null,evidenceUrls:s.EvidenceUrls||[]}))
  : [{sourceKey:null,title:b.SourceTitle||null,publisher:b.SourcePublisher||null,
    edition:b.SourceEdition||null,rootUrl:b.SourceRootUrl||null,
    licenseClaim:b.SourceLicense||null,rightsNote:b.SourceRightsNote||null,
    attribution:b.RequiredDigitalAttribution||null,evidenceUrls:b.SourceEvidenceUrls||[]}];
 blueprints.push({code:b.BlueprintCode,sourceFile:file,sha256:sources[file].sha256,
  sourceCheckedAtUtc:b.SourceCheckedAtUtc||null,declaredSources,
  count:b.Lessons.length,rightsDecision:'PENDING_INDEPENDENT_RIGHTS_REVIEW'});
 for(const l of b.Lessons){
  if(!l.LessonCode||seen.has(l.LessonCode))throw Error('Missing/duplicate lesson '+l.LessonCode);
  seen.add(l.LessonCode);
  const linked=declaredSources.filter(s=>
    (l.SourceKey && s.sourceKey===l.SourceKey) ||
    (s.rootUrl && l.SourceUrl && l.SourceUrl.startsWith(s.rootUrl)));
  const uniqueSource=linked.length===1?linked[0]:declaredSources.length===1?declaredSources[0]:null;
  const validUrl=/^https:\/\//.test(l.SourceUrl||'');
  lessons.push({lessonCode:l.LessonCode,sourceBlueprintCode:b.BlueprintCode,sourceFile:file,
   sourceFileSha256:sources[file].sha256,title:l.Title||null,
   publisherLessonUrl:l.SourceUrl||null,edition:uniqueSource?.edition||null,
   publisher:uniqueSource?.publisher||null,licenseClaim:uniqueSource?.licenseClaim||null,
   declaredSourceKey:l.SourceKey||null,
   declaredSourceCandidates:uniqueSource?[uniqueSource.sourceKey||uniqueSource.rootUrl]:declaredSources.map(s=>s.sourceKey||s.rootUrl),
   declaredSemanticSha256:l.SemanticSha256||null,officialOutcomeCodes:l.OutcomeCodes||[],
   sourceStatus:!validUrl?'BLOCKED_SOURCE_URL_MISSING'
    :uniqueSource?'SOURCE_URL_AND_DECLARED_PUBLISHER_PRESENT_NOT_INDEPENDENTLY_CHECKED'
    :'MULTIPLE_PUBLISHER_SOURCES_REQUIRE_REVIEW',
   lessonReview:'PENDING_INDEPENDENT_ACADEMIC_REVIEW',
   rightsReview:'PENDING_INDEPENDENT_RIGHTS_REVIEW'});
 }
}
lessons.sort((a,b)=>a.lessonCode.localeCompare(b.lessonCode));
if(lessons.length!==1560)throw Error('Expected 1560 US lesson source records');
const scope='US-CCSS-MATH/CCSSM-2010/G1-G12';
const output={
 manifest:{schemaVersion:1,scope,excludedGrades:['Kindergarten'],sourceCommitSha:sha,
  sourceSnapshotCaptureUtc:ref.retrievedUtc,sourceFiles:sources,
  counts:{blueprints:17,lessons:1560,parentStandards:363,childClauses:121},
  evidenceType:'COMMITTED_SOURCE_INVENTORY_NOT_LIVE_VERIFIED_OR_CERTIFIED'},
 authority:{schemaVersion:1,scope,sourceCommitSha:sha,
  independentAcademicSignoffs:0,sourcePages:ref.pages.length,
  parentStandards:authorityRows,subordinateClauses:clauseRows},
 publisher:{schemaVersion:1,scope,sourceCommitSha:sha,
  summary:{blueprints:17,lessons:1560,missingLessonUrls:lessons.filter(x=>x.sourceStatus.startsWith('BLOCKED_')).length,
   missingDeclaredSourceRootUrls:blueprints.flatMap(x=>x.declaredSources).filter(x=>!/^https:\/\//.test(x.rootUrl||'')).length,
   ambiguousPublisherAttribution:lessons.filter(x=>x.sourceStatus==='MULTIPLE_PUBLISHER_SOURCES_REQUIRE_REVIEW').length,
   humanApprovedLicenses:0,academicallyCertifiedLessons:0},
  blueprints,lessons}
};
output.licensing=[
 '# US CCSS source edition and license clearance review',
 '',
 'STATUS: AWAITING INDEPENDENT RIGHTS REVIEW. A license claimed in publisher source metadata is NOT independently verified rights clearance.',
 '',
 'Git source SHA: '+sha+'; scope '+scope+'; 17 blueprints; 1560 lesson sources. The source pack must not be treated as permission to redistribute copied third-party tests, publisher artwork, screenshots or protected prose.',
 '',
 '| Blueprint | Publisher | Source license claim | Reviewer approval |',
 '|---|---|---|---|',
 ...blueprints.flatMap(b=>b.declaredSources.map(s=>
  '| '+b.code+' | '+String(s.publisher||'UNKNOWN').replace(/\|/g,'/')+
  ' | '+String(s.licenseClaim||'UNKNOWN').replace(/\|/g,'/')+
  ' | PENDING (source '+String(s.sourceKey||s.rootUrl||'UNRESOLVED')+') |')),
 '',
 'Source reviewer must verify edition-specific license URL, appropriate attribution, derivative permissions, lesson URL authenticity, use of original authored content and any third-party exclusions. Retain 35 MP-only lesson alignments until independently supported. Technical metadata verification does not satisfy Gate 01 licensing signoff.',
 ''
].join('\n');
for(const [key,p] of Object.entries(rel)){
 const text=typeof output[key]==='string'?output[key]:JSON.stringify(output[key],null,2)+'\n';
 const dst=path.join(root,p);
 if(writing){fs.mkdirSync(path.dirname(dst),{recursive:true});fs.writeFileSync(dst,text);}
 else if(!fs.existsSync(dst)||fs.readFileSync(dst,'utf8')!==text)throw Error('Artifact drift '+p);
}
console.log(JSON.stringify({sourceCommitSha:sha,blueprints:blueprints.length,lessons:lessons.length,
 parentStandards:authorityRows.length,childClauses:clauseRows.length,
 missingSourceUrls:output.publisher.summary.missingLessonUrls,
 missingRootUrls:output.publisher.summary.missingDeclaredSourceRootUrls,
 ambiguousLessonSources:output.publisher.summary.ambiguousPublisherAttribution,
 academicReview:'PENDING',licenseClearance:'PENDING'},null,2));
