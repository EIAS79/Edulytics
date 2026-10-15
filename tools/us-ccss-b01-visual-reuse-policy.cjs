#!/usr/bin/env node
'use strict';
const assert=require('node:assert/strict');
/**
 * Use visual facts about a drawing, not a publisher name or image-file suffix.
 * This policy only determines handling; it cannot authenticate image identity,
 * certify copyright status, verify lesson mathematics or pass the L6 gate.
 */
const HANDLING=Object.freeze({
 KEEP_OWN_ORIGINAL:'KEEP_INDEPENDENT_EDULYTICS_VISUAL',
 DIRECT_SIMPLE:'USE_VERIFIED_TRIVIAL_MATHEMATICAL_DIAGRAM_WITHOUT_PUBLISHER_CREDIT',
 SKIP_PHOTO:'DO_NOT_COPY_PHOTOGRAPH',
 MAKE_OR_LICENSE:'RECREATE_INDEPENDENTLY_OR_LICENSE_DISTINCTIVE_ILLUSTRATION',
 CHECK_VISUAL:'INSPECT_ACTUAL_VISUAL_BEFORE_COPYING_SOURCE_FILE'
});
function decideVisualReuse({visualClass='UNINSPECTED',
 confirmedTrivialVisual=false,containsDistinctiveArtwork=false,
 thirdPartyPhoto=false, independentlyAuthored=false}={}){
 assert.ok(['SIMPLE_GEOMETRY','BASIC_EDUCATIONAL_PICTOGRAM','PHOTOGRAPH',
  'DISTINCTIVE_ILLUSTRATION','UNINSPECTED'].includes(visualClass),
  'Unsupported visual classification');
 if(independentlyAuthored){
  return {decision:HANDLING.KEEP_OWN_ORIGINAL,canCopySourceFile:false,
   requiresPublisherCreditForOwnMathDrawing:false,
   sourceRightsSearchForOwnMathDrawing:false};
 }
 if(thirdPartyPhoto||visualClass==='PHOTOGRAPH')
  return {decision:HANDLING.SKIP_PHOTO,canCopySourceFile:false,
   requiresPublisherCreditForOwnMathDrawing:false,
   sourceRightsSearchForOwnMathDrawing:false};
 if(containsDistinctiveArtwork||visualClass==='DISTINCTIVE_ILLUSTRATION')
  return {decision:HANDLING.MAKE_OR_LICENSE,canCopySourceFile:false,
   requiresPublisherCreditForOwnMathDrawing:false,
   sourceRightsSearchForOwnMathDrawing:false};
 // A reviewed plain polygon, clock, array of dots, counting ball or generic
 // fruit pictogram may be trivial mathematics, even if colored or repeated.
 // Verify actual visual simplicity; never infer it from TikzFile/ImageFile.
 if(confirmedTrivialVisual&&
  (visualClass==='SIMPLE_GEOMETRY'||visualClass==='BASIC_EDUCATIONAL_PICTOGRAM'))
  return {decision:HANDLING.DIRECT_SIMPLE,canCopySourceFile:true,
   requiresPublisherCreditForOwnMathDrawing:false,
   sourceRightsSearchForOwnMathDrawing:false};
 return {decision:HANDLING.CHECK_VISUAL,canCopySourceFile:false,
  requiresPublisherCreditForOwnMathDrawing:false,
  sourceRightsSearchForOwnMathDrawing:false};
}
module.exports={decideVisualReuse,HANDLING};
