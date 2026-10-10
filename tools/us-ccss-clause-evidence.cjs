const fs=require('fs'),path=require('path'),root='C:/Users/khali/Edulytiks-US-CCSS-20261010/';
const auth=JSON.parse(fs.readFileSync(root+'docs/curriculum/us-ccss-authority-repair-candidates.json'));const dir=root+'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs';
const clauses=auth.clauses;const matched=[],packs=fs.readdirSync(dir).filter(x=>x.startsWith('us-ccss-')&&x.endsWith('.lesson-blueprint.json'));let all=[];
for(const f of packs){const p=JSON.parse(fs.readFileSync(path.join(dir,f)));for(const l of p.Lessons){all.push({code:l.LessonCode,title:l.Title,url:l.SourceUrl,outcomes:[...(l.OutcomeCodes||[]),...(l.FormalTargets||[]).map(x=>x.OutcomeCode)].filter(Boolean),alignments:l.Alignments||[]});}}
const normalized=s=>s.replace(/^(HS[NAFGS])\.([A-Z]{1,3})\./,'$1-$2.');
for(const c of clauses){
 const parent=normalized(c.parent);
 const code=normalized(c.code);
 const exact=all.flatMap(l=>(l.alignments||[]).filter(a=>a.Role==='Addressing'&&a.ReferenceCode&&(normalized(a.ReferenceCode)===code||a.ReferenceCode===c.code)).map(a=>({lessonCode:l.code,title:l.title,sourceUrl:l.url,exactReference:a.ReferenceCode})));
 const parentLinks=all.filter(l=>l.outcomes.includes('CCSS:'+parent)).map(l=>({lessonCode:l.code,title:l.title}));
 matched.push({parent:parent,clause:code,officialText:c.officialChildText,officialUrl:c.sourceUrl,publisherExactAddressingCount:exact.length,publisherExactAddressingLessons:exact,parentLinkedLessonCount:parentLinks.length,status:exact.length?'PUBLISHER_EXACT_REFERENCE':'ONLY_PARENT_OR_NO_CLAUSE_EVIDENCE'});
}
const unmatched=matched.filter(x=>!x.publisherExactAddressingCount);
const summary={clauseCount:matched.length,publisherExactlyReferenced:matched.length-unmatched.length,parentOnlyOrNoClauseEvidence:unmatched.length,unmatchedCodes:unmatched.map(x=>x.clause),verifiedQuestionFamilyCoverage:0,disclaimer:'Publisher references are a source-evidence inventory, NOT verification that answer generators/graders test each clause. Zero certified clause-to-question links without independent evidence.'};
fs.writeFileSync(root+'docs/curriculum/us-ccss-121-clause-publisher-evidence.json',JSON.stringify({summary,clauses:matched},null,2)+'\n');console.log(JSON.stringify(summary,null,2));