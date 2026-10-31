'use strict';
// Source/identity/effective candidate inventory only; no manufactured academic acceptance.
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'../..');
const bpath='src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade8-our-2017.lesson-blueprint.json';
const ppath='src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g8-phase29-v1.lesson-content-pack.json';
const cpath='docs/curriculum/us-ccss-g8-coder4-source-crosswalk-20261029.json';
const mpath='src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json';
const richDir='src/Edulytics.Core/Curriculum/LessonContent/RichV2';
const json=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const hash=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,p))).digest('hex');
const assert=(b,m)=>{if(!b)throw Error(m)};
const b=json(bpath),pack=json(ppath),sources=json(cpath),maps=json(mpath);
assert(b.BlueprintCode==='US-CCSS-MATH:G8:OUR-IM-2017','Wrong course');
assert(b.NativeLevel==='Grade 8'&&b.LogicalLevel===9,'Wrong internal level');
assert(b.Units.length===9&&b.Lessons.length===131,'Wrong 9-unit Grade 8 inventory');
assert(pack.lessons.length===131&&sources.rows.length===131,'Pack/source count mismatch');
assert(sources.retrieved===131,'Incomplete publisher source retrieval');
const richNames=fs.readdirSync(path.join(root,richDir)).filter(f=>/^us-ccss-g8-.*\.rich-lesson-v2\.json$/i.test(f));
const richMap=new Map(),richSha={};
for(const file of richNames){
 const rp=richDir+'/'+file,d=json(rp);assert(d.SchemaVersion===1&&d.PackCode==='US-CCSS-MATH','Bad rich document '+file);
 richSha[file]=hash(rp);
 for(const x of d.Lessons){assert(!richMap.has(x.LessonCode),'Duplicate Rich V2 code '+x.LessonCode);
  richMap.set(x.LessonCode,{data:x,path:rp,version:d.ContentVersion});}
}
const cont=new Map(pack.lessons.map(x=>[x.lessonCode,x]));
const source=new Map(sources.rows.map(x=>[x.lessonCode,x]));
const skill=new Map(maps.mappings.filter(x=>x.lessonCode?.startsWith('PED:US-CCSS-MATH:G8:')).map(x=>[x.lessonCode,x]));
assert(cont.size===131&&source.size===131,'Duplicate pack/source identity');
const codes=new Set(),sourceIds=new Set(),rows=[];
for(const [index,l] of b.Lessons.entries()){
 assert(!codes.has(l.LessonCode)&&!sourceIds.has(l.SourceLessonCode),'Duplicate canonical/source ID');
 codes.add(l.LessonCode);sourceIds.add(l.SourceLessonCode);
 const c=cont.get(l.LessonCode),s=source.get(l.LessonCode),rr=richMap.get(l.LessonCode);
 assert(c&&s,'Missing lesson '+l.LessonCode);
 assert(l.SortOrder===index+1,'Sort-order drift '+l.LessonCode);
 assert(c.sourceUrl===l.SourceUrl&&s.url===l.SourceUrl,'Source URL conflict '+l.LessonCode);
 assert(s.sourceLessonCode===l.SourceLessonCode,'Source lesson identity mismatch '+l.LessonCode);
 assert(JSON.stringify(c.outcomeCodes)===JSON.stringify(l.OutcomeCodes),'Outcome mismatch '+l.LessonCode);
 const t=c.translations?.find(x=>x.cultureCode==='en');assert(t,'Missing English content '+l.LessonCode);
 rows.push({
  ordinal:index+1,lessonCode:l.LessonCode,sourceLessonCode:l.SourceLessonCode,title:l.Title,unit:l.UnitNumber,unitTitle:l.UnitTitle,
  sourceUrl:l.SourceUrl,sourceEdition:b.SourceEdition,publisherStudentGoal:s.sourceStudentGoal,
  blueprintRoles:l.Alignments.map(a=>({role:a.Role,reference:a.ReferenceCode})),
  publisherRoles:s.sourceRoles,blueprintRoleReviewNeeded:s.blueprintExtraRoles.length>0||s.blueprintMissingRoles.length>0,
  blueprintExtraRoles:s.blueprintExtraRoles,blueprintMissingRoles:s.blueprintMissingRoles,
  outcomeCodes:l.OutcomeCodes,blueprintSemanticSha256:l.SemanticSha256,canonicalBodySha256:c.canonicalBodySha256,
  sourceBodySha256:c.sourceSha256,canonicalPackVersion:pack.contentVersion,
  rawPackTemplatedGoal:/^Learning goal: You will [^.\n]+\./.test(t.explanation),
  rawPackGenericFiveSteps:t.stepByStepSolutions?.startsWith('Step 1: Identify what the task asks you to find or show.')??false,
  authoredRichV2Asset:rr?.path??null,authoredRichV2Version:rr?.version??null,
  sourcePreparationPageRetrieved:true,completePublisherActivitiesReviewed:false,
  contentStatus:rr?'FIRST_PARTY_AUTHORING_CANDIDATE_NOT_RUNTIME_VERIFIED':'SOURCE_AND_EFFECTIVE_BODY_REVIEW_PENDING',
  explicitLessonSkillFamilies:skill.get(l.LessonCode)?.allowedQuestionFamilies??null,
  exactPracticeChecked:false,humanAssessmentObserved:false,authStudentRenderingChecked:false,
  positiveNegativeBoundaryTestChecked:false,academicAccepted:false,stageD:'NOT_RUN',
  unresolved:rr?['Original activity parity','Authenticated rendered lesson/diagrams','Exact Practice or teacher-assessed task persistence','Two-school isolation','Frozen-SHA CI','Independent academic acceptance']:
    ['Activity-by-activity source comparison','Teaching reuse/repair','Exact Practice or teacher-assessed task','Auth rendering and attempts','Mathematical CI and academic acceptance']
 });
}
assert([...richMap.keys()].every(x=>codes.has(x)),'Rich V2 references a non-Grade-8 canonical code');
const counts={curriculumLessons:rows.length,units:b.Units.length,sourcePagesFetched:131,
 standardsRoleReviewRows:rows.filter(x=>x.blueprintRoleReviewNeeded).length,
 rawPackTemplatedGoals:rows.filter(x=>x.rawPackTemplatedGoal).length,
 rawPackGenericFiveSteps:rows.filter(x=>x.rawPackGenericFiveSteps).length,
 richV2AuthoredCandidates:rows.filter(x=>x.authoredRichV2Asset).length,
 exactPracticeVerified:0,humanAssessmentVerified:0,academicAccepted:0,fullGradeVerified:0};
const report={schemaVersion:1,document:'EDULYTIKS_G8_CODER4_WORK_IN_PROGRESS',baseMainSha:'39c757630d2bd67217a1468fce099653ed0f3f53',
 hashes:{blueprint:hash(bpath),canonicalPack:hash(ppath),publisherCrosswalk:hash(cpath),skillRegistry:hash(mpath),richCandidates:richSha},
 counts,license:'Open Up Resources / IM 6-8 Math, 2017-2019, CC BY 4.0; individual source links on each row.',
 noClaim:'Preparation pages, authored content and lesson identities do not certify activity parity, academic A-D, authenticated runtime, exact Practice, user UAT or publisher-figure equivalence.',rows};
const output='docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json';
fs.writeFileSync(path.join(root,output),JSON.stringify(report,null,2)+'\n');
console.log('MANIFEST '+JSON.stringify({output,counts,richFiles:richNames,first:rows[0].lessonCode,last:rows.at(-1).lessonCode}));
