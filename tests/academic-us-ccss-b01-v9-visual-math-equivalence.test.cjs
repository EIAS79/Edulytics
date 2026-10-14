'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const {compareVisuals,evaluateReuseRights}=require('../tools/us-ccss-b01-v9-visual-math-equivalence.cjs');
const root=path.resolve(__dirname,'..');
const rights=JSON.parse(fs.readFileSync(path.join(root,'artifacts/academic-us-ccss/batches/B01-v9-source-figure-397-rights-attribution-triage.json'),'utf8'));
const base={
 curriculumEdition:'IM_K5_FIRST_EDITION_2019_2021',license:'CC BY 4.0',
 originalSourceUrl:'https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-2/preparation.html',
 originalLicenseUrl:'https://creativecommons.org/licenses/by/4.0/',
 properAttribution:true,adaptationNotice:true,assetPublisherRecordKind:'TikzFile',
 explicitThirdPartyAttribution:false
};
test('Mathematical equivalence ignores color, illustration style, pixel hashes and identical or different artwork bytes',()=>{
 const original={quantities:[3,4],units:'dots',equations:['3+4=7'],partitions:[3,4]};
 const opts={originalMath:original,renderedMath:structuredClone(original),
   originalTeacherTaskMathematicsIndependentlyVerified:true,
   renderedStudentDiagramMathematicsIndependentlyVerified:true,
   originalSha:'a'.repeat(64),renderedSha:'b'.repeat(64),
   originalColor:'blue',renderedColor:'red'};
 assert.equal(compareVisuals(opts).mathematicalDecision,'MATHEMATICALLY_EQUIVALENT');
 assert.equal(compareVisuals({...opts,renderedSha:opts.originalSha}).mathematicalDecision,'MATHEMATICALLY_EQUIVALENT');
 assert.equal(compareVisuals({...opts,renderedMath:{...original,quantities:[3,5]}}).mathematicalDecision,'MATH_SIGNATURE_MISMATCH');
 assert.equal(compareVisuals({...opts,renderedMath:{...original,units:'centimeters'}}).mathematicalDecision,'MATH_SIGNATURE_MISMATCH');
 assert.equal(compareVisuals({...opts,renderedMath:{...original,equations:['3+4=6']}}).mathematicalDecision,'MATH_SIGNATURE_MISMATCH');
 assert.equal(compareVisuals({...opts,renderedMath:{...original,partitions:[7]}}).mathematicalDecision,'MATH_SIGNATURE_MISMATCH');
 assert.equal(compareVisuals({...opts,originalTeacherTaskMathematicsIndependentlyVerified:false}).mathematicalDecision,'INSUFFICIENT_VERIFIED_SEMANTICS');
 assert.equal(compareVisuals({...opts,renderedMath:{}}).mathematicalDecision,'INSUFFICIENT_VERIFIED_SEMANTICS');
 assert.equal(compareVisuals(opts).lessonL1toL6Certified,false);
 assert.equal(compareVisuals(opts).requiresDiagramPixelIdentity,false);
});
test('2019–2021 CC BY 4.0 allows conditional commercial reuse with real source attribution; third-party rights are separate',()=>{
 assert.equal(evaluateReuseRights(base),'FIRST_EDITION_CURRICULUM_CC_BY_CONDITIONAL_USAGE');
 assert.equal(evaluateReuseRights({...base,properAttribution:false}),'CC_BY_ATTRIBUTION_INCOMPLETE');
 assert.equal(evaluateReuseRights({...base,adaptationNotice:false}),'CC_BY_ATTRIBUTION_INCOMPLETE');
 assert.equal(evaluateReuseRights({...base,originalLicenseUrl:'https://creativecommons.org/licenses/by-nc/4.0/'}),'CC_BY_ATTRIBUTION_INCOMPLETE');
 assert.equal(evaluateReuseRights({...base,curriculumEdition:'IM_V360_2024'}),'UNKNOWN_EDITION_NO_REUSE_AUTHORITY');
 assert.equal(evaluateReuseRights({...base,explicitThirdPartyAttribution:true,assetPublisherRecordKind:'ImageFile'}),'THIRD_PARTY_LICENSE_MUST_BE_CHECKED_SEPARATELY');
 assert.equal(evaluateReuseRights({...base,separateImageRightsExceptionIdentified:true}),'THIRD_PARTY_LICENSE_MUST_BE_CHECKED_SEPARATELY');
});
test('Full 397 original figure records are conditionally attributable, exactly five remain explicit third-party exceptions, no fake L6 pass',()=>{
 let conditional=0,thirdParty=0,insufficient=0;
 for(const lesson of rights.lessons){
  for(const frame of lesson.frames){
   const verdict=evaluateReuseRights({...base,
    originalSourceUrl:lesson.sourceOriginalTeacherPageUrl,
    assetPublisherRecordKind:frame.publisherAssetKind,
    explicitThirdPartyAttribution:frame.attributionTextPresentInOriginalFigureElement});
   if(verdict==='THIRD_PARTY_LICENSE_MUST_BE_CHECKED_SEPARATELY')thirdParty++;
   else if(verdict==='FIRST_EDITION_CURRICULUM_CC_BY_CONDITIONAL_USAGE')conditional++;
   else assert.fail('Unclassified license source: '+lesson.lessonCode);
   const math=compareVisuals({originalMath:null,renderedMath:null,
     originalTeacherTaskMathematicsIndependentlyVerified:false,
     renderedStudentDiagramMathematicsIndependentlyVerified:false});
   assert.equal(math.mathematicalDecision,'INSUFFICIENT_VERIFIED_SEMANTICS');
   assert.equal(math.lessonL1toL6Certified,false);
   insufficient++;
  }
 }
 assert.equal(conditional,392);assert.equal(thirdParty,5);assert.equal(insufficient,397);
 assert.equal(rights.scope.rightsPassedForProductionAssets,0);
 assert.equal(rights.academicDecision,'BATCH_NO-GO');
});