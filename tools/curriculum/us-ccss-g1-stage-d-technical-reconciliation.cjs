'use strict';
/**
 * Internal Stage D *technical* evidence reconciliation.
 * This does not license publisher images, prove pupil-authored classroom work,
 * replace the existing original V1 lesson acceptance, or authorize deployment.
 * Consumes the authenticated artifact from the SAME GitHub Actions run.
 */
const fs=require('node:fs');
const path=require('node:path');
const crypto=require('node:crypto');
const cp=require('node:child_process');
const ROOT=path.resolve(__dirname,'../..');
const BASE='artifacts/curriculum-delivery/US-CCSS-MATH/G1/';
const read=p=>JSON.parse(fs.readFileSync(path.resolve(ROOT,p),'utf8'));
const sha64=s=>typeof s==='string'&&/^[0-9a-f]{64}$/.test(s);
function evaluateTechnical({artifact,manifest,map,source,checkpoints,progress,checkoutSha,runId,artifactDigest}){
  const errors=[],add=(s)=>errors.push(s),rows=artifact?.rows,expected=manifest?.lessons?.slice(50);
  if(!Array.isArray(rows)||rows.length!==96)throw Error('Authenticated artifact must contain exactly 96 lesson rows');
  if(!Array.isArray(expected)||manifest.lessons.length!==146||
     !Array.isArray(map?.records)||map.records.length!==96||
     !Array.isArray(source?.rows)||source.rows.length!==96||
     !Array.isArray(progress?.newLessons)||progress.newLessons.length!==96)
    throw Error('Required 146/96 canonical source/progress inventory missing');
  if(artifact.testedCheckoutSha!==checkoutSha||
     !/^[0-9a-f]{40}$/.test(checkoutSha))add('ARTIFACT_CHECKOUT_SHA_MISMATCH');
  if(artifact.scope!=='G1_51_TO_146_ONLY')add('ARTIFACT_SCOPE_MISMATCH');
  if(progress.release!=='NO_GO')add('UNAUTHORIZED_RELEASE_LABEL');
  const evidence=[],seenHashes=new Set();
  let sourceBodyCount=0,sourceFragmentCount=0,digitalCount=0,authoredCount=0;
  for(let i=0;i<96;i++){
    const ordinal=i+51,m=expected[i],s=source.rows[i],candidate=map.records[i],
      actual=rows[i],p=progress.newLessons[i],c=checkpoints[ordinal],local=[];
    const bad=reason=>local.push(reason);
    if(m?.gradeOrdinal!==ordinal||m?.lessonCode!==s?.lessonCode||
       s?.lessonCode!==candidate?.lessonCode||candidate?.lessonCode!==actual?.lessonCode||
       p?.lessonCode!==m?.lessonCode||p?.ordinal!==ordinal||actual?.ordinal!==ordinal)
      bad('MISMATCHED_CANONICAL_LESSON');
    if(candidate.sourceTeacherUrl!==m.sourceTeacherUrl||
       !sha64(m.teacherPreparationSha256)||
       m.teacherPreparationSha256!==s.original2021TeacherPreparationSha256||
       !sha64(s.original2021FullTeacherLessonHtmlSha256)||
       !sha64(s.original2021StudentFacingHtmlSha256)||
       !Array.isArray(s.teacherActivityHeadings)||s.teacherActivityHeadings.length===0||
       !Array.isArray(s.studentFacingTaskExcerpts)||s.studentFacingTaskExcerpts.length===0)
      bad('SOURCE_PROVENANCE_OR_TASK_INDEX_MISSING');
    if(!sha64(actual.unescapedAuthenticatedLearnerHtmlSha256)||
       seenHashes.has(actual.unescapedAuthenticatedLearnerHtmlSha256))
      bad('MISSING_OR_DUPLICATE_ACTUAL_AUTHENTICATED_HTML_HASH');
    seenHashes.add(actual.unescapedAuthenticatedLearnerHtmlSha256);
    if(actual.authenticStudentAuthorshipNotClaimed!==true)
      bad('FALSE_CLAIM_OF_REAL_STUDENT_AUTHORSHIP');
    if(ordinal>=57 && (c?.ordinal!==ordinal||c.lessonCode!==m.lessonCode||
        c.aToCClosure?.outcome!=='READY_FOR_STAGE_D'||
        !Array.isArray(c.aToCClosure?.openACDefects)||c.aToCClosure.openACDefects.length))
      bad('STAGE_A_C_SOURCE_CORRECTION_NOT_CLOSED');
    if(ordinal>=57){
      if(!Number.isInteger(actual.originalSourceBodyFragmentsVerified)||
         actual.originalSourceBodyFragmentsVerified<1)
        bad('AUTHENTICATED_SOURCE_TASK_TEXT_NOT_RECORDED');
      else {
        sourceBodyCount++;
        sourceFragmentCount+=actual.originalSourceBodyFragmentsVerified;
      }
    }else if(p.status!=='INTERNALLY_ACCEPTED')bad('HISTORIC_V1_INTERNAL_ACCEPTANCE_CHANGED');
    const mode=candidate.assessmentMode;
    if(!['DIGITAL_PRACTICE','DIGITAL_AND_SOURCE_AUTHORED_TASK','SOURCE_AUTHORED_TASK'].includes(mode)||
       actual.assessmentMode!==mode)
      bad('WRONG_ASSESSMENT_MODE');
    if(mode==='SOURCE_AUTHORED_TASK'){
      authoredCount++;
      if(ordinal!==65||candidate.candidateFamily!==null||
         actual.learnerTeachingRendered!==true||
         Object.hasOwn(actual,'correctAnswerSelfConsistentAndPersisted'))
        bad('OPEN_AUTHORING_FALSE_AUTO_GRADING');
    }else {
      digitalCount++;
      const family=candidate.preferredSourceGoalFamily||candidate.candidateFamily;
      if(typeof family!=='string'||!family||actual.actualFamily!==family||
         (candidate.candidateFamilies&&!candidate.candidateFamilies.includes(family)))
        bad('UNEXPECTED_PRACTICE_FAMILY');
      for(const key of ['correctAnswerSelfConsistentAndPersisted',
          'malformedAnswerRejectedAndPersisted','wellFormedIncorrectAnswerRejectedAndPersisted']){
        if(actual[key]!==true)bad('PERSISTED_GRADE_PROOF_MISSING_'+key);
      }
      if(ordinal===51 && actual.l51AuthenticatedTwoCardSourceBoardVerified!==true)
        bad('LESSON_51_SOURCE_BOARD_NOT_RENDERED');
    }
    const record={ordinal,lessonCode:m.lessonCode,assessmentMode:mode,
      technicalResult:local.length?'BLOCKED':'TECHNICAL_CANDIDATE_VERIFIED',
      testedAuthenticatedHtmlSha256:actual.unescapedAuthenticatedLearnerHtmlSha256,
      actualPracticeFamily:mode==='SOURCE_AUTHORED_TASK'?null:actual.actualFamily,
      sourceBodyFragmentCount:actual.originalSourceBodyFragmentsVerified??null,
      observedStudentClassroomWork:false,
      teacherSignoffRequiredForPublication:false,
      publisherArtworkRightsCertifiedByThisGate:false,
      sourceTasksFullyReproducedByThisGate:false,
      problems:local};
    if(local.length)errors.push(...local.map(e=>ordinal+':'+e));
    evidence.push(record);
  }
  if(digitalCount!==95||authoredCount!==1||sourceBodyCount!==90||
     sourceFragmentCount!==190)
    add('WHOLE_GRADE_EVIDENCE_COUNT_INVALID');
  if(errors.length)throw Error('Technical evidence fail-closed: '+errors.slice(0,18).join('; ')+
     (errors.length>18?' ... total '+errors.length:''));
  return {schemaVersion:1,decision:'TECHNICAL_STAGE_D_EVIDENCE_RECONCILED',
    publicationApproval:'NOT_EVALUATED',
    gradeRelease:'NO_GO',requiresHumanTeacherOrAcademicSignoff:false,
    grade:'G1',newLessonFirst:51,newLessonLast:146,
    authenticatedHtmlVerified:96,sourceOriginalSelectedTextVerified:sourceBodyCount,
    selectedSourceTextFragmentsVerified:sourceFragmentCount,digitalGradingPersisted:digitalCount,
    sourceOnlyAuthoredTeachingRendered:authoredCount,
    sourceBodyScope:'selected original learning tasks only; no complete publisher art or physical task parity',
    actualStudentWorkOrClassroomParticipationProven:false,
    thirdPartyPublisherArtworkLicenceVerified:false,
    remainingChecks:['Original V1 full source/learner-task and publisher-art rights scope where applicable',
      'Per-lesson internal acceptance evidence and 96/96 existing grade release gate',
      'Separate owner-controlled merge, staging/production checks and deployment'],
    artifact:{runId,checkoutSha,sha256:artifactDigest},
    rows:evidence};
}
function main(args){
  const input=args.find(x=>x.startsWith('--artifact='))?.slice('--artifact='.length);
  const output=args.find(x=>x.startsWith('--output='))?.slice('--output='.length);
  const id=args.find(x=>x.startsWith('--run-id='))?.slice('--run-id='.length);
  if(!input||!output||!id||!/^\d+$/.test(id))
    throw Error('Usage: --artifact=/path/to/evidence.json --output=/path/to/report.json --run-id=<GITHUB_RUN_ID>');
  const bytes=fs.readFileSync(input),artifact=JSON.parse(bytes.toString('utf8'));
  const checkoutSha=cp.execFileSync('git',['rev-parse','HEAD'],{cwd:ROOT,encoding:'utf8'}).trim();
  const checkpoints={};
  for(let n=57;n<=146;n++)checkpoints[n]=read(BASE+'lesson-'+n+'-v2-stage-a-d-checkpoint.json');
  const result=evaluateTechnical({artifact,manifest:read(BASE+'grade-manifest.json'),
    map:read(BASE+'full-grade-practice-candidate-map.json'),
    source:read(BASE+'full-grade-original-student-facing-task-inventory.json'),
    checkpoints,progress:read(BASE+'full-grade-progress.json'),
    checkoutSha,runId:Number(id),artifactDigest:crypto.createHash('sha256').update(bytes).digest('hex')});
  fs.writeFileSync(output,JSON.stringify(result,null,2)+'\n');
  process.stdout.write('G1_TECHNICAL_EVIDENCE_96_PASS 96 95 90 190; NO HUMAN SIGNOFF; RELEASE NO_GO\n');
}
if(require.main===module){try{main(process.argv.slice(2));}catch(e){console.error(e.stack);process.exitCode=1;}}
module.exports={evaluateTechnical};
