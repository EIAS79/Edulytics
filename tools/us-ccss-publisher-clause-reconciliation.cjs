'use strict';
// Verified, reproducible subordinate-standard publisher reference inventory.
// Reads only pinned repository files; never invents child standard text,
// grants academic signoff, or rewrites official curriculum mappings.
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const read = x => JSON.parse(fs.readFileSync(path.join(root, x), 'utf8'));
const registry = read('artifacts/academic-us-ccss/01-authority-registry.json');
const old = read('docs/curriculum/us-ccss-121-clause-publisher-evidence.json');
const base = 'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs';
const names = fs.readdirSync(path.join(root,base)).filter(x =>
  /^us-ccss-.*\.lesson-blueprint\.json$/.test(x)).sort();
const must = (v,s) => { if (!v) throw Error(s); };
must(names.length === 17, 'Expected exactly 17 authoritative blueprint declarations');
const lessons = names.flatMap(x => read(base+'/'+x).Lessons);
must(lessons.length === 1560, 'Canonical publisher lesson count drift');
const normalized = x => String(x||'').replace(/^CCSS:/i,'').toLowerCase();
must(registry.subordinateClauses.length===121 && old.clauses.length===121,
  '121 child clauses expected');
const legacy = new Map(old.clauses.map(x => [normalized(x.clause), x]));
const rows = registry.subordinateClauses.map(c => {
  const predecessor = legacy.get(normalized(c.clauseCode));
  must(!!predecessor && normalized(predecessor.parent)===normalized(c.parentCode) &&
    predecessor.officialUrl===c.officialPageUrl,
    'Official source evidence changed for '+c.clauseCode);
  const refs = lessons.flatMap(l => (l.Alignments||[])
    .filter(a => a.Role==='Addressing' &&
      normalized(a.ReferenceCode)===normalized(c.clauseCode))
    .map(a => ({ lessonCode:l.LessonCode, referenceCode:a.ReferenceCode,
      sourceUrl:l.SourceUrl })));
  must(refs.length===c.exactPublisherSourceRefs,
    'Publisher alignment reference-count drift for '+c.clauseCode);
  const example = c.publisherSample || null;
  if (refs.length) must(example && refs.some(x =>
    x.lessonCode===example.lessonCode &&
    normalized(x.referenceCode)===normalized(example.referenceCode)),
    'Pinned source publisher example not found for '+c.clauseCode);
  return {
    clause:c.clauseCode,parent:c.parentCode,officialUrl:c.officialPageUrl,
    publisherExactReferences:refs.length,
    examplePublisherReference:example,
    legacyExactReferences:predecessor.publisherExactAddressingCount,
    status:refs.length>0?'PUBLISHER_EXACT_REFERENCE':'PARENT_ONLY_OR_UNRESOLVED',
    independentMathReview:'PENDING',
    academicQuestionEvidence:'PENDING'
  };
});
const newOnly=rows.filter(x=>x.legacyExactReferences===0&&x.publisherExactReferences>0);
const lost=rows.filter(x=>x.legacyExactReferences>0&&!x.publisherExactReferences);
must(rows.filter(x=>x.publisherExactReferences>0).length===106 &&
  newOnly.length===24 && lost.length===0,
  'Expected 106 true publisher references, 24 corrected legacy HS false negatives');
const result={
 schemaVersion:1,scope:'US-CCSS-MATH/CCSSM-2010/G1-G12',
 sourceBaselineSha:registry.sourceCommitSha,
 canonicalSourceClaim:'17 publisher blueprint packs, 1560 unique lessons, case-insensitive HS child clause references',
 releaseMeaning:'PUBLISHER_REFERENCE_EVIDENCE_ONLY_NOT_SKILL_CERTIFICATION',
 summary:{
  clauses:121,publisherExactReferenced:106,publisherUnresolved:15,
  previousLegacyExactReferenced:82,legacyFalseNegativeRepairs:24,
  independentChildReviews:0,certifiedChildQuestionFamilies:0
 },
 unchangedPublisherUnknown:rows.filter(x=>!x.publisherExactReferences).map(x=>x.clause),
 legacyNewlyResolved:newOnly.map(x=>x.clause),
 clauses:rows
};
const destination=path.join(root,'artifacts/academic-us-ccss/05-clause-publisher-reference-reconciliation.json');
const serialized=JSON.stringify(result,null,2)+'\n';
if (process.argv.includes('--check')) {
 must(fs.readFileSync(destination,'utf8')===serialized,
  'Committed source evidence differs from recomputed 17-pack publisher evidence');
} else fs.writeFileSync(destination,serialized);
console.log(JSON.stringify(result.summary));
