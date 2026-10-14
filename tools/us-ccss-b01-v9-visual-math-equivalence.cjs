'use strict';
// B01 V9: mathematically meaningful comparison, not copyrighted pixel/style matching.
// A first-edition CC BY license enables reuse with attribution; separate third-party
// licenses still govern independent owner images. This tool cannot certify L1–L6.
const assert=require('node:assert/strict');
const FIRST_EDITION_LICENSE='CC BY 4.0';
const BASE_SOURCE='https://im.kendallhunt.com/k5/teachers/';
const LICENSE_URL='https://creativecommons.org/licenses/by/4.0/';
function normalizeMath(signature){
 if(!signature||typeof signature!=='object'||Array.isArray(signature))
  return null;
 const fields=['quantities','units','labels','equations','partitions','tickValues','dataValues','scale','shapeRelations'];
 // Require at least one mathematical anchor other than a generic diagram kind.
 if(!fields.some(k=>Array.isArray(signature[k])?signature[k].length>0:
  signature[k]!==undefined&&signature[k]!==null&&signature[k]!==''))return null;
 const canonical={};
 for(const k of fields){
  if(signature[k]===undefined)continue;
  const value=signature[k];
  if(Array.isArray(value))canonical[k]=value.map(x=>typeof x==='string'?x.replace(/\s+/g,' ').trim():x);
  else canonical[k]=typeof value==='string'?value.replace(/\s+/g,' ').trim():value;
 }
 return JSON.stringify(canonical);
}
function compareVisuals(input){
 assert(input&&typeof input==='object','Visual comparison requires an object');
 const source=normalizeMath(input.originalMath),target=normalizeMath(input.renderedMath);
 if(!input.originalTeacherTaskMathematicsIndependentlyVerified ||
    !input.renderedStudentDiagramMathematicsIndependentlyVerified || !source || !target)
  return {mathematicalDecision:'INSUFFICIENT_VERIFIED_SEMANTICS',
    requiresDiagramPixelIdentity:false,lessonL1toL6Certified:false};
 return {mathematicalDecision:source===target?'MATHEMATICALLY_EQUIVALENT':'MATH_SIGNATURE_MISMATCH',
   requiresDiagramPixelIdentity:false,lessonL1toL6Certified:false};
}
function evaluateReuseRights(input){
 if(!input||input.curriculumEdition!=='IM_K5_FIRST_EDITION_2019_2021')
  return 'UNKNOWN_EDITION_NO_REUSE_AUTHORITY';
 if(input.assetPublisherRecordKind==='ImageFile' && input.explicitThirdPartyAttribution)
  return 'THIRD_PARTY_LICENSE_MUST_BE_CHECKED_SEPARATELY';
 if(input.license!==FIRST_EDITION_LICENSE||!input.originalSourceUrl?.startsWith(BASE_SOURCE))
  return 'SOURCE_LICENSE_OR_EDITION_NOT_VERIFIED';
 if(!input.properAttribution || input.originalLicenseUrl!==LICENSE_URL ||
    !input.adaptationNotice)
  return 'CC_BY_ATTRIBUTION_INCOMPLETE';
 if(input.separateImageRightsExceptionIdentified)
  return 'THIRD_PARTY_LICENSE_MUST_BE_CHECKED_SEPARATELY';
 return 'FIRST_EDITION_CURRICULUM_CC_BY_CONDITIONAL_USAGE';
}
module.exports={normalizeMath,compareVisuals,evaluateReuseRights};
