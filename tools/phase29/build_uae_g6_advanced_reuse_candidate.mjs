#!/usr/bin/env node
// Review-only curriculum reuse candidate: never writes active Packs/ directories.
// Edulytics-authored Grade 6 General content is evaluated for reuse in the
// separately verified 2025–2026 Grade 6 Advanced Reveal Math source sequence.
import { createHash } from 'node:crypto';
import { readFileSync, mkdirSync, writeFileSync } from 'node:fs';
import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';
const here=dirname(fileURLToPath(import.meta.url));
const repoRoot=resolve(here,'../..');
const bpDir=resolve(repoRoot,'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs');
const contDir=resolve(repoRoot,'src/Edulytics.Core/Curriculum/LessonContent/Packs');
const evidence=JSON.parse(readFileSync(resolve(here,'evidence/uae_advanced_student_sources_2025_2026.json'),'utf8'));
const student=evidence.books.find(x=>x.grade===6&&x.pathway==='Advanced'&&x.volume===1&&x.sourceAcademicYear==='2025-2026');
if(!student)throw Error('Missing verified Grade 6 Advanced 2025-2026 source');
const bp=JSON.parse(readFileSync(resolve(bpDir,'uae-g6-general-t1-ogl-v1.lesson-blueprint.json'),'utf8'));
const content=JSON.parse(readFileSync(resolve(contDir,'uae-g6-general-t1-ogl-v1.lesson-content-pack.json'),'utf8'));
const sha=x=>createHash('sha256').update(x).digest('hex');
function pyJSON(v){
 if(v===null||typeof v!=='object')return JSON.stringify(v);
 if(Array.isArray(v))return '['+v.map(pyJSON).join(', ')+']';
 return '{'+Object.keys(v).sort().map(k=>JSON.stringify(k)+': '+pyJSON(v[k])).join(', ')+'}';
}
const norm=s=>String(s||'').toLowerCase().replace(/[^a-z0-9]/g,'');
const notes=[];
for(const u of student.printedUnits.filter(u=>u.number<=5)){
 const fromPack=bp.Lessons.filter(x=>x.UnitNumber===u.number).sort((a,b)=>a.LessonNumber-b.LessonNumber);
 if(fromPack.length!==u.numberedLessons)throw Error(`Grade 6 unit ${u.number} count mismatch`);
 for(let i=0;i<fromPack.length;i++)if(norm(fromPack[i].Title)!==norm(u.verifiedNumberedLessonTitles[i]))notes.push(`${u.number}-${i+1}`);
}
if(notes.length)throw Error('Printed Advanced source vs General mismatch '+notes.join(', '));
if(bp.Lessons.length!==59||content.Lessons.length!==59)throw Error('Expected Grade 6 59-lesson General corpus');
const originalCodes=new Set(bp.Lessons.map(x=>x.LessonCode));
const contentCodes=new Set(content.Lessons.map(x=>x.LessonCode));
if(originalCodes.size!==59||contentCodes.size!==59||[...originalCodes].some(k=>!contentCodes.has(k)))throw Error('General content/blueprint lesson codes disagree');
// Check actual canonical-body integrity on all original Edulytics-authored translations.
for(const l of content.Lessons){
 const digest=sha(pyJSON(l.Translations[0]));
 if(digest!==l.CanonicalBodySha256)throw Error('General source content digest mismatch '+l.LessonCode);
}
const copy=x=>JSON.parse(JSON.stringify(x));
const newBp=copy(bp),newContent=copy(content);
const adv=s=>s.replaceAll('GENERAL','ADVANCED').replaceAll('General','Advanced');
newBp.Pathway='Advanced';
newBp.BlueprintCode=adv(bp.BlueprintCode);
newBp.SourceSelectionEvidence='Operator-verified Reveal Math Grade 6 Advanced Volume 1 (2025–2026) cover and printed TOC. The first five Advanced modules (33 numbered lessons) match the existing UAE Grade 6 General 2025–2026 chapter-by-chapter. Full 59-lesson mathematics review is still required before publication.';
newBp.SourceRightsNote='Official UAE textbook used for verified structure only. Academic explanations are Edulytics-authored and require manual pathway review.';
newBp.SemanticGraphSha256=sha(newBp.Lessons.map(l=>adv(l.LessonCode)).join('|'));
for(const unit of newBp.Units){
 unit.UnitCode=adv(unit.UnitCode);
 unit.SemanticSha256=sha(bp.Lessons.filter(l=>l.UnitNumber===unit.Number).map(l=>adv(l.LessonCode)).join('|'));
}
for(const l of newBp.Lessons){
 l.SourceLessonCode=adv(l.SourceLessonCode);
 l.LessonCode=adv(l.LessonCode);
 l.OfficialReferenceCode=adv(l.OfficialReferenceCode);
 if(l.Alignments)for(const a of l.Alignments)a.ReferenceCode=adv(a.ReferenceCode);
 l.ApplicableCourses=(l.ApplicableCourses||[]).map(adv);
 l.SemanticSha256=sha(`UAE-MOE-MATH|6|Advanced|${l.UnitTitle}|${l.Title}|${l.SourceUrl}`);
}
newContent.ContentVersion='p29-uae-g6-advanced-t1-ogl-v1-review';
newContent.TargetCurriculumPeriod='2025-2026';
newContent.SourceCurriculumPeriod='2025-2026 verified UAE Edition';
newContent.SourceVersionLabel='Reveal Math Grade 6 Advanced 2025–2026 Volume 1';
newContent.SourceResolution='OperatorVerifiedTextbookReuseCandidate';
newContent.Status='Draft';
newContent.ReviewedBy=null;
newContent.ReviewEvidence='First 33 Advanced textbook lesson titles verified identical to Grade 6 General; remaining 26 require individual comparison. NOT APPROVED FOR PUBLICATION.';
newContent.FallbackReason='Do not claim a ministry-published policy of shared streams; this is a local book-content reuse candidate.';
for(const l of newContent.Lessons){
 l.LessonCode=adv(l.LessonCode);
 l.OfficialReferenceCode=adv(l.OfficialReferenceCode);
 l.TitleSourceReference=adv(l.TitleSourceReference);
 l.SourceLocator=adv(l.SourceLocator||'');
 l.AdaptationStatus='DRAFT — Edulytics-authored mathematics content reused from matching student-book structure; grade-pathway review outstanding.';
 for(const t of l.Translations)for(const k of ['Title','Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary']){
  if(typeof t[k]==='string')t[k]=t[k].replaceAll('Grade 6 General','Grade 6 Advanced').replaceAll('G6 General','G6 Advanced');
 }
 l.CanonicalBodySha256=sha(pyJSON(l.Translations[0]));
}
const finalBp=new Set(newBp.Lessons.map(x=>x.LessonCode)),finalContent=new Set(newContent.Lessons.map(x=>x.LessonCode));
if(finalBp.size!==59||finalContent.size!==59||[...finalBp].some(x=>!finalContent.has(x)))throw Error('Advanced candidate identity mismatch');
const out=resolve(process.argv[2]||resolve(repoRoot,'artifacts/phase29/uae-g6-advanced-reuse'));
mkdirSync(out,{recursive:true});
writeFileSync(resolve(out,'uae-g6-advanced-t1-ogl-v1.lesson-blueprint.review.json'),JSON.stringify(newBp,null,2)+'\n');
writeFileSync(resolve(out,'uae-g6-advanced-t1-ogl-v1.lesson-content-pack.review.json'),JSON.stringify(newContent,null,2)+'\n');
console.log('UAE_G6_ADVANCED_REUSE_DRAFT_PASS: 59 lesson codes, 59 nonpublished bodies; first 33 verified TOC matches; remaining 26 require review.');
