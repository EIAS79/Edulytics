const fs=require("fs"),path=require("path");
const root=path.resolve(__dirname,"..");
const source=JSON.parse(fs.readFileSync(path.join(root,"docs/curriculum/us-ccss-121-clause-publisher-evidence.json")));
const blueprintDir=path.join(root,"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs");
const contentDir=path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Packs");
const blue=fs.readdirSync(blueprintDir).filter(f=>f.startsWith("us-ccss-")&&f.endsWith(".lesson-blueprint.json")).flatMap(f=>JSON.parse(fs.readFileSync(path.join(blueprintDir,f))).Lessons);
const content=new Map(fs.readdirSync(contentDir).filter(f=>f.startsWith("us-ccss-")&&f.endsWith(".lesson-content-pack.json")).flatMap(f=>JSON.parse(fs.readFileSync(path.join(contentDir,f))).lessons.map(l=>[l.lessonCode,l])));
const normalized=x=>String(x||"").replace(/^CCSS:/i,"").toLowerCase();
const rows=source.clauses.map(c=>{
 const exact=blue.flatMap(l=>(l.Alignments||[]).filter(a=>a.Role==="Addressing"&&normalized(a.ReferenceCode)===normalized(c.clause)).map(a=>({lessonCode:l.LessonCode,title:l.Title,sourceUrl:l.SourceUrl,referenceCode:a.ReferenceCode})));
 const parent=blue.filter(l=>(l.OutcomeCodes||[]).some(x=>normalized(x)===normalized(c.parent))||(l.FormalTargets||[]).some(x=>normalized(x.OutcomeCode)===normalized(c.parent))||(l.Alignments||[]).some(a=>a.Role==="Addressing"&&normalized(a.OutcomeCode)===normalized(c.parent)));
 const quality=parent.map(l=>{
   const rec=content.get(l.LessonCode);
   const en=rec?.translations?.find(t=>t.cultureCode==="en");
   const fields=["explanation","keyConceptsAndRules","workedExamples","stepByStepSolutions","commonMistakes","quickSummary"];
   return {lessonCode:l.LessonCode,title:l.Title,sourceUrl:l.SourceUrl,hasEnglishBody:!!en,weakBody:!!en&&fields.some(k=>(en[k]||"").length<35),sourceAlignment:(l.Alignments||[]).filter(a=>a.Role==="Addressing").map(a=>a.ReferenceCode)};
 });
 return {clause:c.clause,parent:c.parent,officialText:c.officialText,authorityUrl:c.officialUrl,sourceReferenceStatus:exact.length?"EXACT_PUBLISHER_ALIGNMENT":"PARENT_STANDARD_OR_FORMAL_TARGET_ONLY",publisherExactAlignments:exact,relatedLessons:quality,academicQuestionStatus:"NOT_FULLY_CERTIFIED",note:"Source-aligned parent lesson or publisher code does not prove an exact question family tests this clause."};
});
const direct=rows.filter(x=>x.publisherExactAlignments.length);
const unmatched=rows.filter(x=>!x.publisherExactAlignments.length);
if(rows.length!==121||direct.length!==106||unmatched.length!==15)throw Error("Unexpected source evidence cardinality: "+JSON.stringify({clauses:rows.length,direct:direct.length,unmatched:unmatched.length}));
const summary={allClauses:121,withPublisherExactDirectCode:106,withoutExactDirectCode:15,exactMappingIsNotQuestionCertification:true,unmatchedCodes:unmatched.map(x=>x.clause)};
const report={schemaVersion:2,sourceAuthority:"https://www.thecorestandards.org/Math/Content/",sourcePolicy:"Preserve case-insensitive publisher child references without inventing new formal official alignments",summary,clauses:rows.map(x=>({clause:x.clause,parent:x.parent,sourceReferenceStatus:x.sourceReferenceStatus,exactSourceReferenceCount:x.publisherExactAlignments.length,examplePublisherReference:x.publisherExactAlignments[0]||null,parentLinkedLessonCount:x.relatedLessons.length,exampleParentLesson:x.relatedLessons[0]?{lessonCode:x.relatedLessons[0].lessonCode,title:x.relatedLessons[0].title}:null,academicQuestionStatus:x.academicQuestionStatus}))};
const target=path.join(root,"docs/curriculum/us-ccss-121-clause-evidence-corrected.json");
fs.writeFileSync(target,JSON.stringify(report,null,2)+"\n");
console.log("PASS "+JSON.stringify(summary));