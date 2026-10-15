'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const {decideVisualReuse,HANDLING}=require('../tools/us-ccss-b01-visual-reuse-policy.cjs');
test('Verified simple geometries are not automatically claimed by a publisher',()=>{
 for(const name of ['regular pentagon','clock face','six green circles']){
  const d=decideVisualReuse({visualClass:'SIMPLE_GEOMETRY',confirmedTrivialVisual:true});
  assert.equal(d.decision,HANDLING.DIRECT_SIMPLE,name);
  assert.equal(d.canCopySourceFile,true);
  assert.equal(d.sourceRightsSearchForOwnMathDrawing,false);
 }
});
test('Plain colored fruit pictograms may be used as generic elementary counting symbols',()=>{
 const d=decideVisualReuse({visualClass:'BASIC_EDUCATIONAL_PICTOGRAM',
  confirmedTrivialVisual:true});
 assert.equal(d.decision,HANDLING.DIRECT_SIMPLE);
 assert.equal(d.requiresPublisherCreditForOwnMathDrawing,false);
});
test('No SVG, Tikz or CMS filename proves the drawing is trivial by itself',()=>{
 for(const type of ['SIMPLE_GEOMETRY','BASIC_EDUCATIONAL_PICTOGRAM','UNINSPECTED']){
  const d=decideVisualReuse({visualClass:type});
  assert.equal(d.decision,HANDLING.CHECK_VISUAL);
  assert.equal(d.canCopySourceFile,false);
 }
});
test('Actual photographs are not copied into the learner maths product',()=>{
 const d=decideVisualReuse({visualClass:'PHOTOGRAPH'});
 assert.equal(d.decision,HANDLING.SKIP_PHOTO);
 assert.equal(d.canCopySourceFile,false);
});
test('Distinctive artwork stays protected from blind copying, even if it shows a banana',()=>{
 const d=decideVisualReuse({visualClass:'BASIC_EDUCATIONAL_PICTOGRAM',
  confirmedTrivialVisual:true,containsDistinctiveArtwork:true});
 assert.equal(d.decision,HANDLING.MAKE_OR_LICENSE);
 assert.equal(d.canCopySourceFile,false);
});
test('Independently drawn platform diagrams do not need source attributions',()=>{
 const d=decideVisualReuse({visualClass:'UNINSPECTED',independentlyAuthored:true});
 assert.equal(d.decision,HANDLING.KEEP_OWN_ORIGINAL);
 assert.equal(d.sourceRightsSearchForOwnMathDrawing,false);
});
