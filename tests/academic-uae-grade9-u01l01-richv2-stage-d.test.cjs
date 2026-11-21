'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='src/Edulytics.Core/Curriculum/LessonContent/';
const code='PED:UAE-MOE-MATH:L9:GENERAL:01:01:VARIABLES-AND-EXPRESSIONS';
test('Grade9 U01L01 RichV2 is an additive Arabic rendering of the existing authored pack, not a new lesson identity',()=>{
 const pack=read(base+'Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const src=pack.Lessons.find(x=>x.LessonCode===code);
 const rich=read(base+'RichV2/uae-g9-general-u01-l01-ar.rich-lesson-v2.json');
 const lesson=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(src&&lesson);
 assert.equal(rich.SchemaVersion,1);
 assert.equal(rich.PackCode,'UAE-MOE-MATH');
 assert.equal(lesson.CultureCode,'ar');
 assert.equal(lesson.Title,src.Translations.find(x=>x.CultureCode==='ar').Title);
 assert.ok(lesson.ExplanationParagraphs.length>=3);
 assert.ok(lesson.KeyConcepts.length>=4);
 assert.ok(lesson.WorkedExamples.length>=4);
 assert.ok(lesson.Visuals.length>=3);
 for(const e of lesson.WorkedExamples){assert.ok(e.Question);assert.ok(e.Method);assert.ok(e.Steps?.length);assert.ok(e.Answer);assert.ok(e.Check);}
 assert.ok(lesson.KeyConcepts.some(x=>x.Title==='ترجمة العبارات إلى جبر'));
 assert.ok(lesson.WorkedExamples.some(x=>x.Title==='نموذج تكلفة برسوم ثابتة'));
});
test('Algebra expression coefficient is not improperly asserted inequivalent for every value including zero',()=>{
 const lesson=read(base+'RichV2/uae-g9-general-u01-l01-ar.rich-lesson-v2.json').Lessons[0];
 const rule=lesson.KeyConcepts.find(x=>x.Title==='القوى والأقواس').Rule;
 assert.match(rule,/غير متكافئين لكل س/);
 assert.match(rule,/س=٠/);
 assert.equal(3*(0**2),(3*0)**2);
 assert.notEqual(3*(2**2),(3*2)**2);
});
test('U01L01 scanned source application coverage includes same-base powers and cylinder volume',()=>{
 const lesson=read(base+'RichV2/uae-g9-general-u01-l01-ar.rich-lesson-v2.json').Lessons[0];
 const powers=lesson.KeyConcepts.find(x=>x.Title==="\u0636\u0631\u0628 \u0627\u0644\u0642\u0648\u0649 \u0630\u0627\u062a \u0627\u0644\u0623\u0633\u0627\u0633 \u0646\u0641\u0633\u0647");
 const cylinder=lesson.KeyConcepts.find(x=>x.Title==="\u062a\u0645\u062b\u064a\u0644 \u062d\u062c\u0645 \u0627\u0644\u0623\u0633\u0637\u0648\u0627\u0646\u0629 \u0628\u062a\u0639\u0628\u064a\u0631 \u062c\u0628\u0631\u064a");
 assert.ok(powers&&cylinder);
 assert.ok(powers.Rule.includes("\u0633\u00b2\u00d7\u0633\u00b3=\u0633\u2075"));
 assert.ok(cylinder.Rule.includes("\u03c0\u0646\u0642\u00b2"));
 const powerExample=lesson.WorkedExamples.find(x=>x.Title==="\u062a\u0641\u0633\u064a\u0631 \u0636\u0631\u0628 \u0627\u0644\u0642\u0648\u0649");
 const cylinderExample=lesson.WorkedExamples.find(x=>x.Title==="\u0646\u0645\u0648\u0630\u062c \u062d\u062c\u0645 \u0623\u0633\u0637\u0648\u0627\u0646\u0629");
 assert.equal(powerExample?.Answer,"\u0661\u0660\u2077");
 assert.ok(cylinderExample?.Answer.includes("\u0664\u0665\u03c0"));
 assert.equal((10**3)*(10**4),10**7);
 assert.equal(Math.PI*3**2*5,45*Math.PI);
});
test('U01L01 teacher cooldown covers explicit verbal order and selective power geometry and fee contexts',()=>{
 const html=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general/u01-l01-source-cooldown-ar.html'),'utf8');
 assert.ok(html.includes("\u0627\u0643\u062a\u0628 \u062a\u0639\u0628\u064a\u0631\u064b\u0627 \u062c\u0628\u0631\u064a\u064b\u0627 \u0644\u062b\u0644\u0627\u062b\u0629 \u0623\u0645\u062b\u0627\u0644 \u0627\u0644\u0639\u062f\u062f \u0633\u060c \u062b\u0645 \u0623\u0636\u0641 \u062e\u0645\u0633\u0629."));
 for(let i=1;i<=7;i++) assert.ok(html.includes('id="q'+i+'"'),'missing teacher task '+i);
 for(const topic of ["\u0661\u0660\u00b2 \u00d7 \u0661\u0660\u2074","\u062d\u062c\u0645\u0647\u0627","\u0627\u0634\u062a\u0631\u0627\u0643 \u0634\u0647\u0631\u064a","\u0627\u0644\u062a\u0648\u0632\u064a\u0639"]) assert.ok(html.includes(topic),topic);
 assert.ok(html.includes('Array.from({length:7}'));
 assert.ok(html.includes("graded:false,submitted:false"));
 assert.ok(html.includes('TEACHER_REVIEW_REQUIRED'));
});
test('U01L01 source exercise 1 through 61 is visually categorized without a false parity claim',()=>{
 const source=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u01l01-all-61-source-exercises-preparation.json');
 const audit=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u01l01-stage-d-selective-source-visual-audit.json');
 assert.equal(source.items.length,61);
 assert.deepEqual(source.items.map(x=>Number(x.sourceItem.split('-Q')[1])),Array.from({length:61},(_,i)=>i+1));
 assert.ok(source.items.every(x=>x.visualSourceSkillCategory && x.visualSkillClassificationStatus==='LOCAL_PDF_VISUALLY_CLASSIFIED_NOT_PARITY_VERIFIED'));
 assert.ok(source.items.every(x=>x.stageDAudit==='NOT_RUN'));
 assert.equal(source.items[28].visualSourceSkillCategory,'CYLINDER_VOLUME_MODEL');
 assert.equal(source.items[36].visualSourceSkillCategory,'SAME_BASE_POWER_PRODUCT');
 assert.equal(source.items[46].proposedDigitalTreatment,'PRIOR_SKILL_REVIEW_SEPARATE_FROM_U01L01_PRIMARY_GRADER');
 assert.equal(audit.fullSourceAtomicAuditPassed,false);
 assert.equal(audit.fullLessonParityGranted,false);
 assert.equal(audit.fullyComparedDigitalExerciseItems,0);
});
test('U01L01 RichV2 now includes each visually observed reading and translation competency',()=>{
 const l=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u01-l01-ar.rich-lesson-v2.json').Lessons[0];
 assert.equal(l.CultureCode,'ar');
 assert.ok(l.KeyConcepts.length>=9);
 assert.ok(l.WorkedExamples.length>=9);
 for(const title of [
 "\u0642\u0631\u0627\u0621\u0629 \u0627\u0644\u062a\u0639\u0627\u0628\u064a\u0631 \u0627\u0644\u0644\u0641\u0638\u064a\u0629 \u0648\u062a\u0645\u064a\u064a\u0632 \u0627\u0644\u062d\u062f \u0648\u0627\u0644\u0639\u0627\u0645\u0644",
 "\u0645\u0641\u0631\u062f\u0627\u062a \u0627\u0644\u0639\u0645\u0644\u064a\u0627\u062a \u0648\u0627\u0644\u062a\u0631\u062a\u064a\u0628 \u0627\u0644\u0644\u0641\u0638\u064a",
 "\u0637\u0631\u0642 \u0643\u062a\u0627\u0628\u0629 \u0636\u0631\u0628 \u0627\u0644\u0645\u062a\u063a\u064a\u0631\u0627\u062a \u0648\u0627\u0644\u0639\u0648\u0627\u0645\u0644"
 ])assert.ok(l.KeyConcepts.some(x=>x.Title===title),title);
 for(const title of [
 "\u0648\u0635\u0641 \u062a\u0639\u0628\u064a\u0631 \u062c\u0628\u0631\u064a \u0641\u064a\u0647 \u0643\u0633\u0631 \u0648\u0642\u0648\u0629",
 "\u0627\u0644\u0645\u0641\u0631\u062f\u0627\u062a \u0648\u0627\u062a\u062c\u0627\u0647 \u0627\u0644\u0637\u0631\u062d",
 "\u0646\u0645\u0648\u0630\u062c \u062c\u0628\u0631\u064a \u0644\u062a\u0643\u0644\u0641\u062a\u064a\u0646 \u0645\u062a\u063a\u064a\u0631\u062a\u064a\u0646"
 ])assert.ok(l.WorkedExamples.some(x=>x.Title===title),title);
 const example=l.WorkedExamples.find(x=>x.Title==="\u0627\u0644\u0645\u0641\u0631\u062f\u0627\u062a \u0648\u0627\u062a\u062c\u0627\u0647 \u0627\u0644\u0637\u0631\u062d");
 assert.ok(example.Answer.includes("\u0666\u0633\u2212\u0661\u0664"));
 assert.ok(example.Check.includes("\u0661\u0664\u2212\u0666\u0633"));
 assert.equal(6*4-14,10);
});
test('U01L01 RichV2 includes independent lesson launch context and operation vocabulary without reproducing source examples',()=>{
 const l=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g9-general-u01-l01-ar.rich-lesson-v2.json').Lessons[0];
 assert.ok(l.ExplanationParagraphs.length>=7);
 assert.ok(l.KeyConcepts.length>=10);
 assert.ok(l.WorkedExamples.length>=10);
 assert.ok(l.Visuals.length>=4);
 assert.ok(l.KeyConcepts.some(x=>x.Title==="\u0642\u0627\u0645\u0648\u0633 \u0644\u0641\u0638\u064a \u0644\u0644\u0639\u0645\u0644\u064a\u0627\u062a \u0627\u0644\u062c\u0628\u0631\u064a\u0629"));
 assert.ok(l.WorkedExamples.some(x=>x.Title==="\u062a\u0645\u0647\u064a\u062f \u0631\u064a\u0627\u0636\u064a \u0628\u062a\u0643\u0644\u0641\u0629 \u062a\u0630\u0627\u0643\u0631"));
 assert.ok(l.Visuals.some(x=>x.Title==="\u0645\u0641\u0631\u062f\u0627\u062a \u0627\u0644\u0639\u0645\u0644\u064a\u0627\u062a \u0627\u0644\u062c\u0628\u0631\u064a\u0629"&&x.Items.length>=6));
 assert.ok(l.ExplanationParagraphs.some(x=>x.includes("\u0662\u0660\u0660\u0645+\u0663\u0660\u0660\u0628")));
 assert.ok(l.ExplanationParagraphs.some(x=>x.includes("\u0662\u0660\u2212\u0646")));
 const example=l.WorkedExamples.find(x=>x.Title==="\u062a\u0645\u0647\u064a\u062f \u0631\u064a\u0627\u0636\u064a \u0628\u062a\u0643\u0644\u0641\u0629 \u062a\u0630\u0627\u0643\u0631");
 assert.ok(example.Answer.includes("\u0664\u0668 \u062f\u0631\u0647\u0645\u064b\u0627"));
 assert.equal(8*6,48);
 assert.equal(200*3+300*2,1200);
});


test('U01L01 Arabic comparative wording must not be misgraded or taught as an arithmetic difference',()=>{
 const l=read(base+'RichV2/uae-g9-general-u01-l01-ar.rich-lesson-v2.json').Lessons[0];
 const sentence=l.ExplanationParagraphs.find(x=>x.includes('«ن أقل من ٢٠»'));
 assert.ok(sentence,'must explain the comparative wording rather than silently dropping it');
 assert.match(sentence,/«ن أقل من ٢٠» تفيد المقارنة/);
 assert.match(sentence,/«الفرق بين ٢٠ ون» فيكتب ٢٠−ن/);
 assert.match(sentence,/«الفرق بين ن و٢٠» فيكتب ن−٢٠/);
 assert.doesNotMatch(sentence,/«ن أقل من ٢٠» تكتب ٢٠−ن/);
 const n=17;
 assert.equal(n<20,true);
 assert.equal(20-n,3);
});


test('U01L01 Arabic lesson vocabulary distinguishes subtractive less-than from the inequality relation',()=>{
 const l=read(base+'RichV2/uae-g9-general-u01-l01-ar.rich-lesson-v2.json').Lessons[0];
 const paragraph=l.ExplanationParagraphs.find(x=>x.includes('«ن أقل من ٢٠»'));
 const rule=l.KeyConcepts.find(x=>x.Title==='قاموس لفظي للعمليات الجبرية').Rule;
 assert.ok(paragraph.includes('«أقل من» تحتاج سياقًا'));
 assert.ok(paragraph.includes('«ن أقل من ٢٠» تفيد المقارنة'));
 assert.doesNotMatch(paragraph,/«مطروح منه» و«أقل من» للطرح/);
 assert.ok(rule.includes('لكنها قد تصف متباينة'));
 assert.doesNotMatch(rule,/الطرح: الفرق\/ناقص\/مطروح منه\/أقل من/);
});


test('U01L01 sports career context must be original, algebraically sound and cannot certify publisher media',()=>{
 const rich=read(base+'RichV2/uae-g9-general-u01-l01-ar.rich-lesson-v2.json');
 const lesson=rich.Lessons.find(x=>x.LessonCode===code);
 const example=lesson.WorkedExamples.find(x=>x.Title==='تطبيق مهني: تكلفة حملة تسويق رياضي');
 assert.ok(example,'source career category needs independent instructional equivalence');
 assert.ok(lesson.ExplanationParagraphs.some(p=>p.includes('التسويق الرياضي')&&p.includes('٢٤م+٣٥')));
 assert.match(example.Answer,/٢٤م\+٣٥/);
 assert.match(example.Answer,/١٣١/);
 assert.ok(example.Steps.length>=3);
 assert.equal(24*4+35,131);
 assert.equal(24*0+35,35);
 assert.notEqual(24*(4+35),131,'fixed design fee must not be repeated');
 const audit=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u01l01-nonnumbered-source-structure-reinspection-NO_GO.json');
 const career=audit.sourceStructuralChecks.find(x=>x.id==='EXTERNAL_SKILLS_CAREER_MEDIA');
 assert.equal(career.sourcePrintedPage,6);
 assert.equal(career.correspondingIndependentContentLocallyVerified,true);
 assert.equal(career.professionalApplicationMathValidated,true);
 assert.equal(career.officialSourceMediaCopied,false);
 assert.equal(career.publisherPhotographyAndExternalQrStillUnverified,true);
 assert.equal(career.fullSourceAtomicEquivalenceCertified,false);
 assert.equal(audit.fullRawSourceLessonAtomicAuditPassed,false);
 assert.equal(audit.canonicalGrade9LessonStageDClosed,false);
 assert.equal(audit.productionImportAllowed,false);
 assert.equal(audit.releaseDecision,'NO_GO');
});
