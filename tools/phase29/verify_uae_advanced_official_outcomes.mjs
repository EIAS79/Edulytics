#!/usr/bin/env node
// Publication gate for UAE Grade 5/6 Advanced. An official textbook
// reference is NOT an official learning outcome or its lesson mapping.
import { readFileSync, existsSync } from 'node:fs';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';
const root=resolve(dirname(fileURLToPath(import.meta.url)),'../..');
const source=JSON.parse(readFileSync(resolve(root,'tools/phase29/evidence/uae_advanced_student_sources_2025_2026.json'),'utf8'));
const dirs=['src/Edulytics.Core/Curriculum/LessonBlueprints/Packs','src/Edulytics.Core/Curriculum/LessonContent/Packs'];
let blocked=0;
for (const grade of [5,6]) {
  const key=`uae-g${grade}-advanced-t1-ogl-v1`;
  const blueprint=resolve(root,dirs[0],key+'.lesson-blueprint.json');
  const content=resolve(root,dirs[1],key+'.lesson-content-pack.json');
  const sourceCount=source.books.filter(b=>b.grade===grade&&b.pathway==='Advanced').reduce((sum,b)=>sum+b.printedUnits.reduce((n,u)=>n+u.numberedLessons,0),0);
  if(!existsSync(blueprint)||!existsSync(content)) {
    console.error(`BLOCKED G${grade}: missing active blueprint/content JSON; source-backed TOC lessons=${sourceCount}`);
    blocked++;continue;
  }
  const bp=JSON.parse(readFileSync(blueprint,'utf8')), cp=JSON.parse(readFileSync(content,'utf8'));
  const lessonMap=new Map((cp.Lessons||[]).map(x=>[x.LessonCode,x]));
  if(bp.Pathway!=='Advanced'||bp.LogicalLevel!==grade||cp.Status!=='Published'||bp.Lessons?.length!==sourceCount||lessonMap.size!==sourceCount) {console.error(`BLOCKED G${grade}: wrong pathway, lesson count, or publication state`);blocked++;continue;}
  for(const lesson of bp.Lessons) {
    const body=lessonMap.get(lesson.LessonCode);
    const outcomes=lesson.OutcomeCodes||[];
    const alignments=lesson.Alignments||[];
    // Source-backed actual codes, not synthetic UAE:REF:TEXTBOOK identifiers.
    if(!body||outcomes.length===0||!outcomes.every(code=>alignments.some(a=>a.OutcomeCode===code&&a.Role==='Addressing'&&a.ReferenceKind!=='OfficialReference'&&a.SourceUrl))) {
      console.error(`BLOCKED G${grade}: missing verified official outcome/mapping for ${lesson.LessonCode}`);
      blocked++;continue;
    }
    const translations=body.Translations||[];
    if(!translations.some(t=>t.Explanation&&t.WorkedExamples?.length&&t.StepByStepSolutions?.length&&t.CommonMistakes?.length&&t.QuickSummary)) {
      console.error(`BLOCKED G${grade}: incomplete canonical teaching body for ${lesson.LessonCode}`);
      blocked++;
    }
  }
}
if(blocked){console.error(`UAE_ADVANCED_OFFICIAL_OUTCOMES_BLOCKED count=${blocked}; do not publish or cut over`);process.exitCode=1;}
else console.log('UAE_ADVANCED_OFFICIAL_OUTCOMES_PASS');
