'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs'),path=require('node:path');
const source = JSON.parse(fs.readFileSync(path.join(__dirname,'../artifacts/curriculum-delivery/UAE-MOE-MATH/G10/source-193-main-toc-raw-entry-ledger.json'),'utf8'));
test('All main source-pathway TOC row IDs are independent and correctly counted',()=>{
 assert.equal(source.totalPrincipalTocSourceEntriesBeforeDedup,193);
 assert.deepEqual(source.countByPathway,{General:48,Advanced:86,Applied:59});
 assert.equal(new Set(source.rows.map(x=>x.sourceEntryId)).size,193);
 assert.equal(source.rows.filter(x=>x.pathwaySource==='Applied').length,59,'applied duplicated TOC must be counted once');
 assert.ok(source.rows.every(x=>x.finalPathway==='General'));
 assert.ok(source.rows.every(x=>x.semanticDedupStatus==='PENDING_ATOMIC_LEARNING_GOAL_COMPARISON'));
});
test('Volume-only TOC counts never falsely signal annual source body access',()=>{
 assert.ok(source.rows.some(x=>x.pathwaySource==='Advanced'&&x.sourceUnit===5&&x.bodySourcePdf===null));
 assert.ok(source.rows.some(x=>x.pathwaySource==='Applied'&&x.sourceUnit===1&&x.bodySourcePdf===null));
 assert.ok(source.rows.some(x=>x.pathwaySource==='Advanced'&&x.sourceUnit===9&&x.bodyEditionUsed==='2018–2019'));
 assert.equal(source.canonicalCount,null);
 assert.equal(source.sourceLock,false);
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
});
