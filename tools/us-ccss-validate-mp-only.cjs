const fs=require('fs'),path=require('path');const root=path.resolve(__dirname,'..');const d=path.join(root,'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs');const out=[];
for(const file of fs.readdirSync(d).filter(x=>x.startsWith('us-ccss-')&&x.endsWith('.lesson-blueprint.json'))){
const pack=JSON.parse(fs.readFileSync(path.join(d,file),'utf8'));
for(const l of pack.Lessons){const codes=[...(l.OutcomeCodes||[]),...(l.FormalTargets||[]).map(x=>x.OutcomeCode)].filter(Boolean);
if(!codes.length||codes.some(x=>!x.startsWith('CCSS:MP.')))continue;
const sourceMP=l.Alignments.filter(a=>a.Role==='Addressing'&&a.ReferenceKind==='MathematicalPractice' || a.Role==='Addressing'&&a.ReferenceCode?.startsWith('MP.')).map(a=>a.OutcomeCode).filter(Boolean).sort();
const normalized=[...new Set(codes)].sort();
const publisherExact=JSON.stringify(normalized)===JSON.stringify([...new Set(sourceMP)].sort());
const formalMP=(l.FormalTargets||[]).filter(x=>x.OutcomeCode?.startsWith('CCSS:MP.')&&x.EvidenceKind==='VerifiedContentCoverage'&&Array.isArray(x.EvidenceReferences)&&x.EvidenceReferences.length>0).map(x=>x.OutcomeCode).sort();
const reviewedExact=JSON.stringify(normalized)===JSON.stringify([...new Set(formalMP)].sort());
if(!publisherExact&&!reviewedExact)throw new Error('Unverified MP-only link: '+l.LessonCode);
out.push({lessonCode:l.LessonCode,title:l.Title,sourceUrl:l.SourceUrl,mathematicalPractices:normalized,
evidenceType:publisherExact?'PUBLISHER_ADDRESSED_MP':'REVIEWED_PEDAGOGICAL_FORMAL_TARGET',
decision:'KEEP_EXISTING_MP_ONLY',note:'No invented content-standard association. A reviewed FormalTarget is not a publisher-supplied alignment.'});
}}
if(out.length!==35)throw new Error('Expected 35, got '+out.length);
const data={schemaVersion:1,packCode:'US-CCSS-MATH',count:out.length,reviewPolicy:'Keep actual publisher numbered MP Addressing associations; no invented official content-standard mapping.',rows:out};
fs.writeFileSync(path.join(root,'docs/curriculum/us-ccss-35-mp-publisher-decisions.json'),JSON.stringify(data,null,2)+'\n');console.log('PASS:',out.length,'MP-only lesson records have traceable publisher/prior-reviewed evidence; no invented content outcomes added.', 'Publisher:',out.filter(x=>x.evidenceType==='PUBLISHER_ADDRESSED_MP').length,'Reviewed:',out.filter(x=>x.evidenceType==='REVIEWED_PEDAGOGICAL_FORMAL_TARGET').length);