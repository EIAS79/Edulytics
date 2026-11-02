'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const {mkdtempSync,mkdirSync,writeFileSync,rmSync}=require('node:fs');
const path=require('node:path');
const os=require('node:os');
const {execFileSync}=require('node:child_process');
const {requireCurrentCandidateSha}=
  require('../tools/us-ccss-t001-release-gate.cjs');

test('T001 release only accepts verified descendant ledger-only evidence',()=>{
  const cwd=mkdtempSync(path.join(os.tmpdir(),'edulytics-t001-release-'));
  const git=(...args)=>execFileSync('git',args,{cwd,encoding:'utf8'}).trim();
  try{
    mkdirSync(path.join(cwd,'artifacts/academic-us-ccss'),{recursive:true});
    mkdirSync(path.join(cwd,'src'),{recursive:true});
    git('init');
    git('config','user.name','T001 CI');
    git('config','user.email','t001-ci@example.invalid');
    const code=path.join(cwd,'src/grade-one.cs');
    const evidence=path.join(cwd,
      'artifacts/academic-us-ccss/04-lesson-certification-records.json');
    writeFileSync(code,'candidate code v1\n');
    writeFileSync(evidence,'{"state":"PENDING"}\n');
    git('add','.');
    git('commit','-m','freeze candidate');
    const frozen=git('rev-parse','HEAD');
    const ledger={releaseCandidateSha:frozen};
    assert.doesNotThrow(()=>requireCurrentCandidateSha(ledger,frozen,cwd));
    writeFileSync(evidence,'{"state":"EVIDENCE_READY"}\n');
    git('add','.');
    git('commit','-m','evidence only');
    const reviewed=git('rev-parse','HEAD');
    assert.doesNotThrow(()=>requireCurrentCandidateSha(ledger,reviewed,cwd));
    writeFileSync(code,'candidate code v2 unauthorized\n');
    git('add','.');
    git('commit','-m','unauthorized runtime change');
    const changed=git('rev-parse','HEAD');
    assert.throws(()=>requireCurrentCandidateSha(ledger,changed,cwd),
      /must modify only the approved ledger/);
    assert.throws(()=>requireCurrentCandidateSha(
      {releaseCandidateSha:'f'.repeat(40)},changed,cwd),
      /not ancestor/);
    assert.throws(()=>requireCurrentCandidateSha(
      {releaseCandidateSha:frozen},'NOT_A_SHA',cwd),
      /must each be full Git SHAs/);
  }finally{
    rmSync(cwd,{recursive:true,force:true});
  }
});
