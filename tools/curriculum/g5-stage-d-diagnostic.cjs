#!/usr/bin/env node
'use strict';
// Grade 5 Stage D diagnostic. A successful preflight or test job never implies academic certification.
const fs=require('node:fs');
const path=require('node:path');
const cp=require('node:child_process');
const ROOT=path.resolve(__dirname,'../..');
const rel='artifacts/curriculum-delivery/US-CCSS-MATH/G5/';
const read=p=>JSON.parse(fs.readFileSync(path.join(ROOT,p),'utf8'));
const exists=p=>typeof p==='string' && !path.isAbsolute(p) && !p.includes('..') && fs.existsSync(path.join(ROOT,p));
const units=[12,17,20,21,26,21,13,18];
function inventory(){
  const ledger=read(rel+'g5-abc-lesson-ledger-v1.json');
  const blueprint=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade5-im-k5-2021.lesson-blueprint.json');
  const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g5-phase29-v1.lesson-content-pack.json');
  const errors=[];
  const a=ledger.lessons,b=blueprint.Lessons,c=pack.lessons;
  if(!Array.isArray(a)||a.length!==148||!Array.isArray(b)||b.length!==148||!Array.isArray(c)||c.length!==148)
    errors.push('EXACT_148_LESSON_DENOMINATOR_MISMATCH');
  const seen=new Set(); const records=[];
  for(let i=0;i<148;i++){
    const row=a[i], bp=b[i], pc=c[i];
    if(!row||!bp||!pc){errors.push('MISSING_ROW_'+(i+1));continue;}
    const code=row.LessonCode, expectedCode='PED:US-CCSS-MATH:G5:U'+String(units.findIndex((_,u)=>i<units.slice(0,u+1).reduce((p,n)=>p+n,0))+1).padStart(2,'0')+':L'+String(i-units.slice(0,units.findIndex((_,u)=>i<units.slice(0,u+1).reduce((p,n)=>p+n,0))).reduce((p,n)=>p+n,0)+1).padStart(2,'0');
    if(code!==expectedCode)errors.push('LESSON_ORDER_'+(i+1)+':'+code);
    if(bp.LessonCode!==code||pc.lessonCode!==code)errors.push('BLUEPRINT_CONTENT_LEDGER_CODE_MISMATCH_'+(i+1));
    if(seen.has(code))errors.push('DUPLICATE_'+code);seen.add(code);
    if(!row.READY_FOR_STAGE_D)errors.push('NO_A_C_HANDOFF_'+code);
    const test=row.testPath||row.testFile,ev=row.closureEvidencePath;
    if(!exists(test))errors.push('TEST_SOURCE_MISSING_'+code);
    if(!exists(ev))errors.push('SOURCE_CLOSURE_MISSING_'+code);
    if(!pc.translations?.length)errors.push('PACK_TRANSLATION_MISSING_'+code);
    records.push({ordinal:i+1,code,test,evidence:ev});
  }
  return {errors,records,blueprintCount:b?.length,packCount:c?.length,ledgerCount:a?.length};
}
function preflight(){
  const q=inventory();
  console.log('G5_STAGE_D_MANIFEST '+JSON.stringify({expected:148,blueprint:q.blueprintCount,pack:q.packCount,ledger:q.ledgerCount,linkage:q.records.length,errors:q.errors.slice(0,30),errorCount:q.errors.length}));
  if(q.errors.length)process.exitCode=1;
}
function shard(name){
  const ranges={D1:[1,50],D2:[51,100],D3:[101,148]};
  if(!ranges[name])throw Error('Bad shard name');
  const q=inventory();
  if(q.errors.length)throw Error('Preflight failed: '+q.errors.slice(0,6));
  const [lo,hi]=ranges[name], rows=q.records.filter(x=>x.ordinal>=lo&&x.ordinal<=hi);
  if(rows.length!==hi-lo+1)throw Error('Shard denominator mismatch');
  const node=[...new Set(rows.filter(x=>x.test.endsWith('.cjs')).map(x=>x.test))].sort();
  const cs=rows.filter(x=>x.test.endsWith('.cs')).length;
  console.log('G5_STAGE_D_SHARD '+JSON.stringify({name,range:[lo,hi],lessons:rows.length,nodeFileCount:node.length,dotnetLinkedLessons:cs,tests:node}));
  if(node.length){
    // Node's standard exit code is zero even for unimplemented TODO assertions.
    // A nonempty TODO count must not be reported as a Stage D success.
    const p=cp.spawnSync(process.execPath,['--test',...node],{cwd:ROOT,encoding:'utf8',timeout:25*60*1000,maxBuffer:32*1024*1024});
    if(p.stdout)process.stdout.write(p.stdout);
    if(p.stderr)process.stderr.write(p.stderr);
    if(p.error)throw p.error;
    if(p.status!==0)process.exitCode=p.status||1;
    const summary=(p.stdout||'').match(/(?:^|\n)(?:[^\n]*?)(?:ℹ todo|# todo)\s+(\d+)\s*(?:\n|$)/);
    const todos=summary?Number(summary[1]):null;
    if(todos===null){
      console.error('G5_STAGE_D_NODE_SUMMARY_MISSING: cannot prove there are zero TODO tests');
      process.exitCode=2;
    }else if(todos!==0){
      console.error('G5_STAGE_D_NODE_TODO_BLOCKED '+JSON.stringify({batch:name,todo:todos,reason:'Outstanding runtime/source cases are not passing tests'}));
      process.exitCode=2;
    }
  }else console.log('G5_STAGE_D_DOTNET_ONLY: Node count 0; C# unit tests are executed in the separate shared .NET job');
  console.log('G5_STAGE_D_SHARD_END '+name+' node='+node.length+' cs='+cs+' nodeResult='+process.exitCode);
}
function academic(){
  const ownerConsent=ownerAcademicConsent();
  const q=inventory(); const blockers=[...q.errors];
  if(!ownerConsent.ownerContentConsent)blockers.push('GRADE5_OWNER_ACADEMIC_CONSENT_NOT_RECORDED');
  for(const row of q.records){
    const source=read(row.evidence);
    if(source.status==='A_C_EXPLICIT_GAP'||source.academicAccepted===false||source.sourceCoverage100Percent===false)
      blockers.push(row.code+':A_C_SOURCE_ACCEPTANCE_NOT_PROVEN');
    // The source lacks verified per-element artwork/rights and authorized human signoffs.
    if(source.sourceRights?.status==='RIGHTS_RESTRICTED')blockers.push(row.code+':RIGHTS_RESTRICTED');
  }
  // No Stage D test evidence or authorized human reviewer in the handoff; never promote on static files alone.
  blockers.push('G5_TECHNICAL_AND_AUTHENTICATED_E2E_CANDIDATE_EVIDENCE_NOT_COLLECTED');
  blockers.push('G5_ORIGINAL_IMAGE_PDF_RIGHTS_AND_TEACHER_APPROVAL_NOT_CERTIFIED');
  console.error('G5_STAGE_D_ACADEMIC_NO_GO '+JSON.stringify({decision:'NO_GO_TECHNICAL',
    ownerAcademicConsent:ownerConsent.ownerDecision,
    blockerCount:blockers.length,sample:blockers.slice(0,35)}));
  process.exitCode=2;
}

// The accepted A-C handoff has historical element-array schemas. Missing a
// recognized array is a diagnostic bug; an itemized list is still NOT academic approval.
function analyzeElementInventory(evidence, lessonCode){
  const sourceElements=Array.isArray(evidence.lessonElements)?evidence.lessonElements:
    Array.isArray(evidence.elements)?evidence.elements:
    Array.isArray(evidence.sourceElements)?evidence.sourceElements:null;
  const declaredCounts=[evidence.sourceElementsKnown,evidence.sourceElementsCount]
    .filter(Number.isInteger);
  // Some later A-C closures use sourceElements as short *strings* describing
  // activities. Those are source outlines, NOT 1:1 source-element records.
  const structured=sourceElements!==null && sourceElements.length>0 &&
    sourceElements.every(e=>e!==null && typeof e==='object' && !Array.isArray(e) &&
      typeof e.SourceElementId==='string' && e.SourceElementId.trim() &&
      typeof e.LessonCode==='string' && e.LessonCode.trim());
  const sourceElementRows=structured?sourceElements:null;
  const descriptiveOutlineCount=sourceElements!==null && !structured?sourceElements.length:null;
  const identified=sourceElementRows?.length ?? declaredCounts[0] ?? null;
  const matched=sourceElementRows?.filter(x=>x.Status==='MATCHED_DIGITAL'||x.Status==='MATCHED_TEACHER_LED').length ?? null;
  const statusCounts={};
  const blockers=[];
  if(sourceElements===null)blockers.push('SOURCE_ITEMIZED_DENOMINATOR_UNKNOWN');
  else if(!structured)blockers.push('SOURCE_ELEMENT_ARRAY_IS_NOT_STRUCTURED_1_TO_1_EVIDENCE');
  if(sourceElements){
    const ids=new Set();
    for(const item of sourceElementRows||[]){
      const status=typeof item?.Status==='string'?item.Status:'UNKNOWN';
      statusCounts[status]=(statusCounts[status]||0)+1;
      if(typeof item?.SourceElementId!=='string'||!item.SourceElementId.trim())
        blockers.push('SOURCE_ELEMENT_ID_MISSING');
      else if(ids.has(item.SourceElementId))
        blockers.push('DUPLICATE_SOURCE_ELEMENT_ID');
      else ids.add(item.SourceElementId);
      if(item?.LessonCode!==lessonCode)blockers.push('SOURCE_ELEMENT_LESSON_CODE_MISMATCH');
      if(typeof item?.SourceSnapshotId!=='string'||!item.SourceSnapshotId.trim())
        blockers.push('MISSING_ELEMENT_SOURCE_SNAPSHOT');
    }
    if(declaredCounts.some(n=>n!==sourceElements.length))blockers.push('SOURCE_ITEM_COUNT_MISMATCH');
    if(structured && Number.isInteger(evidence.sourceElementsAccepted)&&evidence.sourceElementsAccepted!==matched)
      blockers.push('ACCEPTED_ELEMENT_COUNT_MISMATCH');
    if(structured && matched!==sourceElementRows.length)blockers.push('UNMATCHED_IDENTIFIED_SOURCE_ELEMENTS');
  }
  if(evidence.lessonCode && evidence.lessonCode!==lessonCode)
    blockers.push('SOURCE_EVIDENCE_LESSON_CODE_MISMATCH');
  return {sourceElements:sourceElementRows,hasSourceElementArray:sourceElements!==null,
    descriptiveOutlineCount,identified,matched,statusCounts,blockers:[...new Set(blockers)]};
}


/**
 * A 2024 v.360 student HTML is not a 2021 Kendall Hunt student source, even
 * when its unit and lesson code happen to coincide. A file hash proves identity
 * of the captured bytes only, never semantic equivalence or redistribution.
 */
function analyzeEditionPair(lessonCode, entries){
  const blockers=[];
  const teacher=entries.filter(x=>x.role==='Teacher');
  const student=entries.filter(x=>x.role==='Student');
  const exactHash=x=>typeof x?.sha256==='string'&&/^[a-f0-9]{64}$/.test(x.sha256);
  if(teacher.length!==1||student.length!==1)
    blockers.push('SOURCE_EDITION_MISSING_OR_DUPLICATE_ROLE');
  const t=teacher[0],s=student[0];
  if(!exactHash(t)||!exactHash(s))
    blockers.push('SOURCE_EDITION_HASH_MISSING_OR_MALFORMED');
  if(t?.htmlDocumentTitle!=='Illustrative Mathematics | Kendall Hunt' ||
      t?.editionMatch!=='IM2021_TEACHER_CANDIDATE_SOURCE')
    blockers.push('TEACHER_IM2021_EDITION_NOT_VERIFIED');
  if(s?.htmlDocumentTitle!=='Illustrative Mathematics | Kendall Hunt' ||
      s?.editionMatch!=='IM2021_STUDENT_CANDIDATE_SOURCE')
    blockers.push('STUDENT_IM2021_EDITION_NOT_VERIFIED');
  if(s?.editionMatch==='WRONG_EDITION_V360_STUDENT_MIRROR' ||
      s?.htmlDocumentTitle==='Illustrative Mathematics | v.360 Curriculum')
    blockers.push('SOURCE_EDITION_MISMATCH_V360_STUDENT');
  // Authentic title/path alone does not prove student/teacher semantic match.
  if(t?.educationalParityCertified!==true || s?.educationalParityCertified!==true)
    blockers.push('TEACHER_STUDENT_SEMANTIC_PARITY_NOT_CERTIFIED');
  return {lessonCode,teacherSha256:t?.sha256||null,studentSha256:s?.sha256||null,
    teacherTitle:t?.htmlDocumentTitle||null,studentTitle:s?.htmlDocumentTitle||null,
    blockers:[...new Set(blockers)]};
}
function grade5EditionManifests(){
  const lookup=new Map();
  for(let u=1;u<=units.length;u++){
    const p=rel+'local-teacher-student-html-source-hashes-u'+String(u).padStart(2,'0')+'.json';
    if(!exists(p))throw Error('MISSING_G5_UNIT_EDITION_MANIFEST '+p);
    const manifest=read(p);
    if(!Array.isArray(manifest.lessons))throw Error('MALFORMED_G5_UNIT_EDITION_MANIFEST '+p);
    for(const entry of manifest.lessons){
      const key='PED:US-CCSS-MATH:G5:U'+String(u).padStart(2,'0')+
        ':L'+String(entry.lesson).padStart(2,'0');
      if(!lookup.has(key))lookup.set(key,[]);
      lookup.get(key).push(entry);
    }
  }
  return lookup;
}


function original2021StudentFacingLocator(){
  const lookup=new Map();
  for(let u=1;u<=units.length;u++){
    const key=String(u).padStart(2,'0');
    const file=rel+'im2021-student-facing-local-source-u'+key+'.json';
    if(!exists(file))throw Error('ORIGINAL_2021_STUDENT_FACING_MANIFEST_MISSING '+file);
    const manifest=read(file);
    if(!Array.isArray(manifest.lessons)||manifest.lessons.length!==units[u-1]||
        manifest.fullItemizedParity!=='UNVERIFIED')
      throw Error('ORIGINAL_2021_STUDENT_FACING_MANIFEST_INVALID '+file);
    for(const x of manifest.lessons){
      const code='PED:US-CCSS-MATH:G5:U'+key+':L'+String(x.lesson).padStart(2,'0');
      if(lookup.has(code))throw Error('DUPLICATE_2021_STUDENT_FACING_SOURCE '+code);
      lookup.set(code,{path:file,sections:x.studentFacingSections,
        cmsAssetReferences:x.cmsAssetReferences});
    }
  }
  return lookup;
}



/** Hash/offset source locators are original IM2021 TEACHER Student Facing blocks.
 * Neither their presence nor their source hashes prove complete tasks, pedagogy,
 * publisher media licenses, student PDFs or human academic approval.
 */
function validateOriginal2021StudentFacingUnit(m,unit,editionLookup,legacyLookup){
  const expected=units[unit-1];
  const validSha=x=>typeof x==='string'&&/^[0-9a-f]{64}$/.test(x);
  const code=(n)=>'PED:US-CCSS-MATH:G5:U'+String(unit).padStart(2,'0')+
    ':L'+String(n).padStart(2,'0');
  if(m?.schemaVersion!==1||m?.unit!==unit||m?.lessonCount!==expected||
      m?.publisherImageBytesIncluded!==false||
      m?.fullSourceItemParityVerified!==false||
      m?.academicCertification!=='PENDING_SOURCE_ELEMENT_REVIEW'||
      !Array.isArray(m.lessons)||m.lessons.length!==expected)
    throw Error('G5_2021_PROVENANCE_MANIFEST_INVALID_U'+unit);
  const map=new Map();let count=0;
  for(let i=0;i<expected;i++){
    const row=m.lessons[i],id=code(i+1);
    const teacher=(editionLookup.get(id)||[]).filter(x=>x.role==='Teacher');
    const legacy=legacyLookup.get(id);
    if(row?.lessonCode!==id||row?.unit!==unit||row?.lesson!==i+1||
       row?.sourceRelativePath!=='unit-'+unit+'/lesson-'+(i+1)+'/lesson.html'||
       !validSha(row.sourceHtmlSha256)||teacher.length!==1||
       teacher[0].sha256!==row.sourceHtmlSha256||
       !legacy||row.studentFacingSectionCount!==legacy.sections||
       !Array.isArray(row.studentFacingSections)||
       row.studentFacingSections.length!==row.studentFacingSectionCount||
       row.studentFacingSectionCount<1)
      throw Error('G5_2021_PROVENANCE_TEACHER_HASH_OR_LESSON_DRIFT_'+id);
    let prevOffset=-1;
    for(let j=0;j<row.studentFacingSections.length;j++){
      const s=row.studentFacingSections[j];
      if(s?.ordinal!==j+1||!validSha(s.sourceSectionSha256)||
         !Number.isSafeInteger(s.sourceHtmlStartOffset)||
         s.sourceHtmlStartOffset<=prevOffset||
         !Number.isSafeInteger(s.sourceHtmlCharacterLength)||
         s.sourceHtmlCharacterLength<0||
         !Array.isArray(s.embeddedFigureRecordIds)||
         !s.embeddedFigureRecordIds.every(x=>/^\d+$/.test(x)))
        throw Error('G5_2021_PROVENANCE_SECTION_INVALID_'+id+'_'+j);
      prevOffset=s.sourceHtmlStartOffset;
    }
    count+=row.studentFacingSectionCount;
    map.set(id,{teacherSourceHtmlSha256:row.sourceHtmlSha256,
      studentFacingSections:row.studentFacingSections,
      sourceRelativePath:row.sourceRelativePath});
  }
  if(count!==m.studentFacingSectionTotal)
    throw Error('G5_2021_PROVENANCE_UNIT_SECTION_COUNT_DRIFT_'+unit);
  return map;
}
function original2021StudentFacingSectionProvenance(editionLookup,legacyLookup){
  const map=new Map();
  for(let unit=1;unit<=units.length;unit++){
    const source=rel+'im2021-student-facing-section-provenance-u'+String(unit).padStart(2,'0')+'.json';
    if(!exists(source))throw Error('G5_2021_PROVENANCE_UNIT_MANIFEST_MISSING_'+source);
    const rows=validateOriginal2021StudentFacingUnit(read(source),unit,editionLookup,legacyLookup);
    for(const [code,row] of rows){
      if(map.has(code))throw Error('G5_2021_PROVENANCE_DUPLICATE_'+code);
      map.set(code,row);
    }
  }
  if(map.size!==148)throw Error('G5_2021_PROVENANCE_148_DENOMINATOR_MISMATCH');
  return map;
}

function ownerAcademicConsent(){
  const file=rel+'grade5-owner-academic-consent-20261101.json';
  if(!exists(file))throw Error('GRADE5_OWNER_CONSENT_RECORD_MISSING');
  const c=read(file);
  if(c.ownerDecision!=='OWNER_ACADEMIC_CONSENT_GRANTED' ||
      c.scope?.repository!=='EIAS79/Edulytics' ||
      c.scope?.prNumber!==596 ||
      c.scope?.grade!=='G5' || c.scope?.stage!=='D' ||
      c.scope?.lessonCount!==148 || c.scope?.requiredEntryState!=='READY_FOR_STAGE_D' ||
      c.authorizationClaims?.ownerConsentsToReviewedGrade5Content!==true ||
      c.authorizationClaims?.ownerConsentsTo107IdentifiedTextChanges!==true ||
      c.ownerSignedCryptographicDocument!==false ||
      !/^https:\/\/github\.com\/EIAS79\/Edulytics\/issues\/619#issuecomment-\d+$/.test(c.publicDecisionEvidence||'') ||
      Object.values(c.certificationExclusions||{}).some(v=>v!==false) ||
      Object.keys(c.certificationExclusions||{}).length!==6)
    throw Error('GRADE5_OWNER_CONSENT_SCOPE_OR_EVIDENCE_INVALID');
  return {ownerDecision:c.ownerDecision,publicDecisionEvidence:c.publicDecisionEvidence,
    ownerContentConsent:true,individualSourceParityCertified:false,
    thirdPartyRightsCertified:false,studentTeacherE2ECertified:false,releaseCertified:false};
}

function sourceEvidenceAudit(){
  const ownerConsent=ownerAcademicConsent();
  const q=inventory();
  const editionLookup=grade5EditionManifests();
  const firstEditionStudentFacing=original2021StudentFacingLocator();
  const originalSectionProvenance=original2021StudentFacingSectionProvenance(
    editionLookup,firstEditionStudentFacing);
  const rows=[];
  for(const item of q.records){
    const evidence=read(item.evidence);
    const {sourceElements,hasSourceElementArray,descriptiveOutlineCount,
      identified,matched,statusCounts,blockers}=analyzeElementInventory(evidence,item.code);
    const editionEvidence=analyzeEditionPair(item.code,editionLookup.get(item.code)||[]);
    blockers.push(...editionEvidence.blockers);
    const sourceStudentFacing=firstEditionStudentFacing.get(item.code)||null;
    const sectionSource=originalSectionProvenance.get(item.code)||null;
    if(!sectionSource)blockers.push('ORIGINAL_2021_STUDENT_FACING_SOURCE_SECTION_HASH_MISSING');
    if(!sourceStudentFacing||!Number.isInteger(sourceStudentFacing.sections)||
        sourceStudentFacing.sections<1)
      blockers.push('ORIGINAL_2021_STUDENT_FACING_SECTION_NOT_LOCATED');
    if(evidence.status==='A_C_EXPLICIT_GAP')blockers.push('A_C_EXPLICIT_GAP');
    if(evidence.sourceCoverage100Percent!==true)blockers.push('SOURCE_PARITY_NOT_INDEPENDENTLY_PROVEN');
    // Project owner approved this Grade 5 content pack, including the 107 edits.
    // This is not independent source-element verification; retain that evidence gate.
    if(!ownerConsent.ownerContentConsent && evidence.academicAccepted!==true)
      blockers.push('GRADE5_CONTENT_OWNER_APPROVAL_NOT_PROVEN');
    const acceptedReviewer=typeof evidence.academicReviewerSignature==='string'&&evidence.academicReviewerSignature.trim().length>0;
    if(!acceptedReviewer)blockers.push('INDEPENDENT_SOURCE_ELEMENT_REVIEW_NOT_EVIDENCED');
    rows.push({ordinal:item.ordinal,lessonCode:item.code,sourceEvidencePath:item.evidence,
      sourceSnapshotId:evidence.sourceSnapshotId||null,identifiedElementCount:identified,
      hasSourceElementArray,descriptiveOutlineElementCount:descriptiveOutlineCount,
      itemizedElementCount:sourceElements?.length??null,matchedElementCount:matched,
      sourceElementStatusCounts:statusCounts,editionEvidence,
      original2021StudentFacing:sourceStudentFacing,
      original2021StudentFacingSectionProvenance:sectionSource,
      sourceParity:evidence.sourceCoverage100Percent===true?'CLAIMED_IN_AC_REQUIRES_INDEPENDENT_VALIDATION':'UNKNOWN',
      academicApproval:ownerConsent.ownerContentConsent
        ? 'OWNER_APPROVED_GRADE5_CONTENT_SOURCE_VERIFICATION_PENDING'
        : evidence.academicAccepted===true?'CLAIMED_IN_AC_REQUIRES_SIGNED_REVIEW':'NOT_APPROVED',
      ownerContentApproved:ownerConsent.ownerContentConsent,independentElementReviewPassed:acceptedReviewer,
      blockers});
  }
  const result={schemaVersion:1,reportType:'SOURCE_ELEMENT_EVIDENCE_DIAGNOSTIC_NOT_ACADEMIC_CERTIFICATION',
    ownerAcademicConsent:ownerConsent,
    gitCandidateSha:cp.execFileSync('git',['rev-parse','HEAD'],{cwd:ROOT,encoding:'utf8'}).trim(),
    scopeLessons:148,lessonCount:rows.length,
    rawElementArrayLessons:rows.filter(x=>x.hasSourceElementArray).length,
    descriptiveOutlineLessons:rows.filter(x=>x.descriptiveOutlineElementCount!==null).length,
    itemizedLessonCount:rows.filter(x=>x.itemizedElementCount!==null).length,
    completeElementSnapshotLessons:rows.filter(x=>x.itemizedElementCount!==null &&
      !x.blockers.includes('MISSING_ELEMENT_SOURCE_SNAPSHOT')).length,
    noElementArrayLessons:rows.filter(x=>!x.hasSourceElementArray).length,
    unknownSourceDenominatorLessons:rows.filter(x=>x.itemizedElementCount===null).length,
    wrongEditionStudentLessons:rows.filter(x=>x.blockers.includes('SOURCE_EDITION_MISMATCH_V360_STUDENT')).length,
    original2021TeacherStudentFacingLessons:rows.filter(x=>x.original2021StudentFacing?.sections>0).length,
    original2021TeacherStudentFacingSections:rows.reduce((n,x)=>n+(x.original2021StudentFacing?.sections||0),0),
    original2021TeacherSectionHashLinkedLessons:rows.filter(x=>
      x.original2021StudentFacingSectionProvenance?.studentFacingSections.length>0).length,
    original2021TeacherSectionHashLinkedCount:rows.reduce((n,x)=>
      n+(x.original2021StudentFacingSectionProvenance?.studentFacingSections.length||0),0),
    sourceParityVerifiedLessons:0,
    ownerContentApprovedLessons:ownerConsent.ownerContentConsent?rows.length:0,
    independentlySourceApprovedLessons:0,
    lessonsWithExplicitACGap:rows.filter(x=>x.blockers.includes('A_C_EXPLICIT_GAP')).map(x=>x.lessonCode),
    blockedLessons:rows.filter(x=>x.blockers.length).length,rows};
  fs.mkdirSync(path.join(ROOT,'stage-d-results'),{recursive:true});
  fs.writeFileSync(path.join(ROOT,'stage-d-results/grade5-source-evidence.json'),JSON.stringify(result,null,2)+'\n');
  console.log('G5_SOURCE_EVIDENCE_AUDIT '+JSON.stringify({candidateSha:result.gitCandidateSha,scope:result.scopeLessons,
    rawArrays:result.rawElementArrayLessons,descriptiveOutlines:result.descriptiveOutlineLessons,
    itemized:result.itemizedLessonCount,completeSnapshots:result.completeElementSnapshotLessons,
    unknownDenominators:result.unknownSourceDenominatorLessons,
    wrongEditionStudentLessons:result.wrongEditionStudentLessons,
    original2021StudentFacingLessons:result.original2021TeacherStudentFacingLessons,
    original2021StudentFacingSections:result.original2021TeacherStudentFacingSections,
    sourceHashLinkedLessons:result.original2021TeacherSectionHashLinkedLessons,
    sourceHashLinkedSections:result.original2021TeacherSectionHashLinkedCount,
    ownerContentApprovedLessons:result.ownerContentApprovedLessons,
    independentlySourceApprovedLessons:result.independentlySourceApprovedLessons,
    explicitGaps:result.lessonsWithExplicitACGap,blocked:result.blockedLessons,
    artifact:'stage-d-results/grade5-source-evidence.json'}));
  if(q.errors.length||result.blockedLessons)process.exitCode=2;
}

if(require.main===module){
  const mode=process.argv[2]||'preflight';
  if(mode==='preflight')preflight();
  else if(mode==='shard')shard(process.argv[3]);
  else if(mode==='academic')academic();
  else if(mode==='source-evidence')sourceEvidenceAudit();
  else throw Error('Unknown mode '+mode);
}
module.exports={analyzeElementInventory,analyzeEditionPair,grade5EditionManifests,original2021StudentFacingLocator,ownerAcademicConsent,
  validateOriginal2021StudentFacingUnit,original2021StudentFacingSectionProvenance};
