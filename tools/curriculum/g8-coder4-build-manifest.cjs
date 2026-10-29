'use strict';
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'../..');
const bpath='src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade8-our-2017.lesson-blueprint.json';
const ppath='src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g8-phase29-v1.lesson-content-pack.json';
const cpath='docs/curriculum/us-ccss-g8-coder4-source-crosswalk-20261029.json';
const rpath='src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json';
const rpath2='src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json';
const mpath='src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json';
const json=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const hash=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,p))).digest('hex');
const b=json(bpath),pack=json(ppath),sources=json(cpath),rich=json(rpath),rich2=json(rpath2),maps=json(mpath);
const fail=(x,msg)=>{if(!x)throw Error(msg)};
fail(b.BlueprintCode==='US-CCSS-MATH:G8:OUR-IM-2017','Wrong course');
fail(b.NativeLevel==='Grade 8'&&b.LogicalLevel===9,'Wrong internal level');
fail(b.Units.length===9&&b.Lessons.length===131,'Wrong 9-unit lesson inventory');
fail(pack.lessons.length===131&&sources.rows.length===131,'Source/pack count mismatch');
fail(sources.retrieved===131,'Unretrieved publisher preparation pages');
const content=new Map(pack.lessons.map(x=>[x.lessonCode,x]));
const bySource=new Map(sources.rows.map(x=>[x.lessonCode,x]));
const richMap=new Map([...rich.Lessons,...rich2.Lessons].map(x=>[x.LessonCode,x]));
const skillMap=new Map(maps.mappings.filter(x=>x.lessonCode?.startsWith('PED:US-CCSS-MATH:G8:')).map(x=>[x.lessonCode,x]));
fail(content.size===131&&bySource.size===131&&richMap.size===17,'Duplicate grade lessons');
const rows=[],used=new Set(),sourceIds=new Set();
for(const [idx,l] of b.Lessons.entries()){
 fail(!used.has(l.LessonCode)&&!sourceIds.has(l.SourceLessonCode),'Duplicate blueprint identity');
 used.add(l.LessonCode);sourceIds.add(l.SourceLessonCode);
 const cp=content.get(l.LessonCode),s=bySource.get(l.LessonCode),rr=richMap.get(l.LessonCode);
 fail(cp&&s,'Missing record: '+l.LessonCode);
 fail(l.SortOrder===idx+1,'Wrong sort order: '+l.LessonCode);
 fail(l.SourceUrl===cp.sourceUrl&&l.SourceUrl===s.url,'Publisher source mismatch: '+l.LessonCode);
 fail(JSON.stringify(l.OutcomeCodes)===JSON.stringify(cp.outcomeCodes),'Outcome mismatch: '+l.LessonCode);
 fail(s.sourceLessonCode===l.SourceLessonCode,'Source code mismatch: '+l.LessonCode);
 const t=cp.translations.find(x=>x.cultureCode==='en');
 fail(t,'Missing English content: '+l.LessonCode);
 rows.push({
  ordinal:idx+1,lessonCode:l.LessonCode,sourceLessonCode:l.SourceLessonCode,title:l.Title,unit:l.UnitNumber,
  unitTitle:l.UnitTitle,sourceUrl:l.SourceUrl,sourceEdition:b.SourceEdition,
  publisherStudentGoal:s.sourceStudentGoal,
  blueprintRoles:l.Alignments.map(x=>({role:x.Role,reference:x.ReferenceCode})),
  publisherRoles:s.sourceRoles,blueprintRoleReviewNeeded:s.blueprintExtraRoles.length>0||s.blueprintMissingRoles.length>0,
  blueprintExtraRoles:s.blueprintExtraRoles,blueprintMissingRoles:s.blueprintMissingRoles,
  outcomeCodes:l.OutcomeCodes,blueprintSemanticSha256:l.SemanticSha256,
  canonicalBodySha256:cp.canonicalBodySha256,sourceBodySha256:cp.sourceSha256,
  canonicalPackVersion:pack.contentVersion,
  rawPackTemplatedGoal:/^Learning goal: You will [^.\n]+\./.test(t.explanation),
  rawPackGenericFiveSteps:t.stepByStepSolutions?.startsWith('Step 1: Identify what the task asks you to find or show.')??false,
  authoredRichV2Asset:rr?(idx<8?rpath:rpath2):null,
  authoredRichV2Version:rr?(idx<8?rich.ContentVersion:rich2.ContentVersion):null,
  contentStatus:rr?'FIRST_PARTY_AUTHORING_CANDIDATE_NOT_RUNTIME_VERIFIED':'SOURCE_AND_EFFECTIVE_BODY_REVIEW_PENDING',
  sourcePreparationPageRetrieved:true,
  completePublisherActivitiesReviewed:false,
  explicitLessonSkillFamilies:skillMap.get(l.LessonCode)?.allowedQuestionFamilies??null,
  exactPracticeChecked:false,humanAssessmentObserved:false,authStudentRenderingChecked:false,
  positiveNegativeBoundaryTestChecked:false,academicAccepted:false,stageD:'NOT_RUN',
  unresolved:rr?
   ['Publisher activity-level parity','Actual Rich V2 rendering and accessibility','Exact Practice or teacher assessment interaction','Auth saved attempts/tenant isolation','.NET/CI final SHA','Lesson-specific academic acceptance']:
   ['Full source/student activity review','Effective content repair or accepted reuse','Exact Practice/teacher assessment','Authenticated render/attempts','Tests and academic acceptance']
 });
}
const counts={
 curriculumLessons:rows.length,units:b.Units.length,sourcePagesFetched:rows.filter(x=>x.sourcePreparationPageRetrieved).length,
 standardsRoleReviewRows:rows.filter(x=>x.blueprintRoleReviewNeeded).length,
 rawPackTemplatedGoals:rows.filter(x=>x.rawPackTemplatedGoal).length,
 rawPackGenericFiveSteps:rows.filter(x=>x.rawPackGenericFiveSteps).length,
 richV2AuthoredCandidates:rows.filter(x=>x.authoredRichV2Asset).length,
 exactPracticeVerified:0,humanAssessmentVerified:0,
 academicAccepted:0,fullGradeVerified:0
};
const report={schemaVersion:1,document:'EDULYTIKS_G8_CODER4_WORK_IN_PROGRESS',baseMainSha:'39c757630d2bd67217a1468fce099653ed0f3f53',
 hashes:{blueprint:hash(bpath),canonicalPack:hash(ppath),publisherCrosswalk:hash(cpath),richCandidateU01L01L08:hash(rpath),richCandidateU01L09L17:hash(rpath2),skillRegistry:hash(mpath)},
 counts,license:'Open Up Resources / IM 6-8 Math, 2017-2019, CC BY 4.0. Source links in individual rows.',
 noClaim:'Source-page HTML and authored candidates are not complete academic A-D, authenticated runtime, exact Practice, full-grade CI, publisher artwork equivalence, or teacher evaluation. No APPROVED/RELEASED row is inferred.',rows};
const output='docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json';
fs.writeFileSync(path.join(root,output),JSON.stringify(report,null,2)+'\n');
console.log(JSON.stringify({output,counts,first:rows[0].lessonCode,last:rows.at(-1).lessonCode},null,2));
