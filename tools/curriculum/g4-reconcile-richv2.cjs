'use strict';
// Grade 4 101–149: repairs structural Rich V2 schema gaps without asserting academic acceptance.
const fs=require('node:fs'), path=require('node:path'),assert=require('node:assert/strict');
const root=path.resolve(__dirname,'../..');
const abs=p=>path.join(root,p);
const read=p=>JSON.parse(fs.readFileSync(abs(p),'utf8'));
const write=process.argv.includes('--write');
const manifest=read('artifacts/curriculum-delivery/US-CCSS-MATH/G4/lesson-102-149-original-source-intake.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g4-phase29-v1.lesson-content-pack.json');
const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings;
const result={schemaVersion:1,grade:4,lessonsExamined:49,structuredRichV2Count:0,lessonsWithExactPracticeMappings:0,exactPracticeMappingsUnverified:[],needsOriginalTaskAndDiagramReview:[],perLesson:[]};
function text(s){return String(s||'').trim();}
function unique(list){return [...new Set(list.map(text).filter(Boolean))];}
function lessonMeta(n){
 if(n===101)return {publisherPurpose:"Find unknown rectangle side lengths by division and partial quotients.",activities:["Warm-up: Estimation Exploration: Area of a Soccer Field","Activity 1: Elena's Mural","Activity 2: Tyler's Mural"],studentFacingEvidence:["A soccer field's length is unknown and its width and area are supplied.","Elena makes a mural from 189 square tiles that is seven tiles wide.","Tyler has 197 tiles and a rectangular mural six tiles wide. Interpret any leftovers."]};
 return manifest.records.find(x=>x.ordinal===n);
}
function validateRich(x,lessonCode){
 assert.equal(x.SchemaVersion,1);assert.equal(x.PackCode,'US-CCSS-MATH');assert.equal(x.Lessons.length,1);
 const l=x.Lessons[0];assert.equal(l.LessonCode,lessonCode);
 for(const [k,min] of [['ExplanationParagraphs',2],['KeyConcepts',3],['WorkedExamples',3],['CommonMistakes',2],['SummaryPoints',3]])assert.ok(l[k].length>=min,lessonCode+' '+k);
 for(const v of l.WorkedExamples){for(const k of ['Title','Question','Method','Answer'])assert.ok(text(v[k]),lessonCode+' '+k);assert.ok(v.Steps.length>=3,lessonCode+' example steps');}
}
for(let n=101;n<=149;n++){
 const packLesson=pack.lessons[n-1];assert.ok(packLesson,'missing pack '+n);
 const src=lessonMeta(n);assert.ok(src,'missing source metadata '+n);
 const cp=read('artifacts/curriculum-delivery/US-CCSS-MATH/G4/lesson-'+n+'-v2-stage-a-d-checkpoint.json');
 assert.equal(cp.lessonCode,packLesson.lessonCode);assert.equal(cp.academicApproved,false);
 const file='src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g4-ordinal-'+n+'-2021.rich-lesson-v2.json';
 const r=read(file),l=r.Lessons[0];assert.equal(l.LessonCode,packLesson.lessonCode);
 const t=packLesson.translations[0];
 const rule=text(l.KeyConcepts[0]?.Rule)||text(t.keyConceptsAndRules);
 const goal=text(t.explanation.split('\n')[0]);
 const sourceTask=text(src.studentFacingEvidence?.[1])||text(src.studentFacingEvidence?.[0])||goal;
 const sourceActivity=text(src.activities?.[1])||'original lesson source activity';
 const cooldown=text(cp.firstPartyCooldown?.prompt)||"Explain the lesson\u0027s mathematical relationship.";
 const teacherKey=text(cp.firstPartyCooldown?.teacherAnswer)||'Teacher evaluates the result and explanation.';
 // Extra concepts describe distinct elements of the source lesson, not a generic repetition of a formula.
 const conceptAdd=[
   {Title:'Task representation',Definition:text(src.publisherPurpose)||goal,
    Rule:'Interpret the given source task with its actual quantities, labels, units and required construction; preserve open-response tasks.',
    Example:sourceTask.slice(0,310)},
   {Title:'Check and interpretation',Definition:'The source question must be answered with the mathematical representation or explanation it requests.',
    Rule:'Apply the taught relationship, then check the result against the original conditions; teacher assesses drawings, choices and justifications.',
    Example:cooldown}
 ];
 const titles=new Set(l.KeyConcepts.map(x=>x.Title));
 for(const c of conceptAdd){if(!titles.has(c.Title)&&l.KeyConcepts.length<3){l.KeyConcepts.push(c);titles.add(c.Title);}}
 const solutions=unique(t.stepByStepSolutions.split('\n').map(x=>x.replace(/^Step\s+\d+:\s*/i,'')));
 const teacherSteps=[
  'Identify exactly what is supplied and asked in '+sourceActivity+'.',
  'Select the source-required model or operations. '+rule,
  'Produce the diagram, expression, explanation or answer and verify it against the source constraints.'
 ];
 const cooldownSteps=[
  'Read the Edulytics first-party question and mark the given values or required attributes.',
  'Use the source-aligned lesson relationship: '+rule,
  'Check the result or constructed figure against the stated conditions; teacher key: '+teacherKey
 ];
 const examples=[
  {Title:'Original source-task reasoning plan (teacher-reviewed)',Question:sourceTask.slice(0,340),Method:text(src.publisherPurpose)||rule,
   Steps:teacherSteps,Answer:'Open source task: original visual and student explanation require teacher evaluation; no automatic source answer is claimed.',
   Check:'Do not mark automatically passed. Check original 2021 teacher-page Student Facing activity and required figure or construction.'},
  {Title:'Edulytics first-party Cool-down demonstration',Question:cooldown,Method:rule,
   Steps:cooldownSteps,Answer:teacherKey,Check:'First-party answer only; this is not the gated publisher assessment or a verified Practice generator.'}
 ];
 for(const x of examples) if(l.WorkedExamples.length<3&&!l.WorkedExamples.some(y=>y.Title===x.Title))l.WorkedExamples.push(x);
 if(l.CommonMistakes.length<2)l.CommonMistakes.push({Mistake:'Using a result without checking the requested representation, labels or context.',
  WhyWrong:'The numeric answer alone does not establish the geometric, measurement, source task or explanation requirements.',
  Correction:'Re-read '+sourceActivity+'; apply '+rule+' and explain why the response matches every condition.'});
 if(l.SummaryPoints.length<3)for(const x of unique([goal,'Check with the original lesson: '+sourceActivity,'Source visuals, open student responses, Practice and authenticated scoring remain pending.']))if(l.SummaryPoints.length<3)l.SummaryPoints.push(x);
 validateRich(r,packLesson.lessonCode);
 if(write)fs.writeFileSync(abs(file),JSON.stringify(r,null,2)+'\n');
 const exact=mappings.find(x=>x.lessonCode===packLesson.lessonCode);
 const mapped=!!exact&&exact.practiceReadiness==='READY_VERIFIED'&&Array.isArray(exact.allowedQuestionFamilies)&&exact.allowedQuestionFamilies.length>0;
 if(mapped)result.lessonsWithExactPracticeMappings++;
 else result.exactPracticeMappingsUnverified.push({ordinal:n,lessonCode:packLesson.lessonCode,reason:exact?'Mapping not ready or missing exact families':'No lesson-specific entry in embedded lesson-skill-mappings.v1.json'});
 result.needsOriginalTaskAndDiagramReview.push(n);
 result.structuredRichV2Count++;
 result.perLesson.push({ordinal:n,lessonCode:packLesson.lessonCode,richV2StructuralValid:true,exactPracticeMapped:mapped,fullStageDVerified:false});
}
assert.equal(result.structuredRichV2Count,49);
const outfile='artifacts/curriculum-delivery/US-CCSS-MATH/G4/g4-101-149-richv2-practice-audit.json';
if(write)fs.writeFileSync(abs(outfile),JSON.stringify(result,null,2)+'\n');
else if(fs.existsSync(abs(outfile)))assert.deepEqual(read(outfile),result);
console.log(JSON.stringify({mode:write?'WRITE':'CHECK',structureValid:result.structuredRichV2Count,exactMapped:result.lessonsWithExactPracticeMappings,missingExact:result.exactPracticeMappingsUnverified.length,stageDApproved:0}));
