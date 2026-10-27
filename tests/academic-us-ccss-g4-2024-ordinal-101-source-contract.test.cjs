'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.join(__dirname,'..','artifacts/curriculum-delivery/US-CCSS-MATH/G4');
const get=n=>JSON.parse(fs.readFileSync(path.join(root,n),'utf8'));
test('2024 G4 lesson 101 maps by title and mathematics to old ordinal 102, not 101',()=>{
 const x=get('edition-2024-navigation-crosswalk-151.json').rows[100];
 assert.equal(x.title2024,'Base-Ten Blocks to Divide');assert.equal(x.previous2021.ordinal,102);assert.equal(x.identityVerified,false);
});
test('Place-value division candidates preserve exact arithmetic and no false approval',()=>{
 const x=get('lesson-101-2024-skill-contract.json');
 for(const c of x.numericCases)assert.equal(c.dividend/c.divisor,c.quotient);
 assert.deepEqual(x.allowedQuestionFamilies,[]);assert.equal(x.activeRuntimeMapped,false);
 assert.equal(x.graderVerified,false);assert.equal(x.testsWrittenNotRun,true);
});
