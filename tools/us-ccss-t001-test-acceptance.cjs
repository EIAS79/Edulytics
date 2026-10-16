#!/usr/bin/env node
'use strict';
// T001 tests-only SOFTWARE readiness. This is not academic certification.
// The GitHub Checks API is the authority: no user-supplied PASS flags.
const fs = require('node:fs');
const path = require('node:path');

const required = Object.freeze([
  't001-compile-and-source-math',
  'independent-full-product-regression',
  'isolated-us-school-https'
]);
const shaPattern = /^[0-9a-f]{40}$/i;
const repoPattern = /^[A-Za-z0-9_.-]+\/[A-Za-z0-9_.-]+$/;

function evaluateChecks(checks, sha) {
  if (!shaPattern.test(sha)) throw Error('Exact full 40-character candidate SHA required');
  if (!Array.isArray(checks)) throw Error('GitHub check run list unavailable');
  const results = {};
  for (const name of required) {
    const candidates = checks
      .filter(c => c.name === name && c.head_sha === sha &&
        c.app?.slug === 'github-actions' &&
        /^https:\/\/github\.com\//.test(c.html_url || ''))
      .sort((a, b) => (b.id || 0) - (a.id || 0));
    const latest = candidates[0];
    results[name] = latest
      ? { status: latest.status, conclusion: latest.conclusion,
          url: latest.html_url, id: latest.id }
      : { status: 'missing', conclusion: null };
  }
  const passed = Object.values(results).every(c =>
    c.status === 'completed' && c.conclusion === 'success');
  return {
    batchId: 'T001', candidateSha: sha, requiredChecks: results,
    technicalReleaseAccepted: passed,
    state: passed ? 'BATCH_TESTS_ACCEPTED' : 'BATCH_TESTS_BLOCKED',
    requiresHumanAcademicReview: false,
    isAcademicCertification: false,
    claim: 'Tested software/lesson readiness only; not verified student learning or external accreditation.'
  };
}

async function fetchChecks(repository, sha, token, client = fetch) {
  if (!repoPattern.test(repository) || !shaPattern.test(sha))
    throw Error('Invalid GitHub repository or commit SHA');
  const checks = [];
  for (let page = 1; page <= 10; page++) {
    const url = `https://api.github.com/repos/${repository}/commits/${sha}/check-runs?per_page=100&page=${page}`;
    const headers = { 'Accept': 'application/vnd.github+json',
      'X-GitHub-Api-Version': '2022-11-28', 'User-Agent': 'edulytics-t001-test-acceptance' };
    if (token) headers.Authorization = 'Bearer ' + token;
    const response = await client(url, { headers });
    if (!response.ok) throw Error('GitHub Checks API returned HTTP ' + response.status);
    const payload = await response.json();
    if (!Array.isArray(payload.check_runs)) throw Error('Invalid GitHub Checks API payload');
    checks.push(...payload.check_runs);
    if (payload.check_runs.length < 100) return checks;
  }
  throw Error('More than 1000 check runs: refuse ambiguous/incomplete evidence');
}

async function main(args = process.argv.slice(2)) {
  const raw = args.find(x => x.startsWith('--sha='));
  const sha = raw?.slice(6);
  if (!sha || !shaPattern.test(sha))
    throw Error('Usage: node tools/us-ccss-t001-test-acceptance.cjs --sha=<full-40-char-Git-SHA>');
  const repository = process.env.GITHUB_REPOSITORY || 'EIAS79/Edulytics';
  // Fail closed on unrelated repositories, as T001 is repository scoped.
  if (repository !== 'EIAS79/Edulytics') throw Error('T001 repository scope mismatch');
  // Verify T001 is still a fixed, ten-lesson manifest; never silently change
  // the release unit when evaluating a passing CI suite.
  const manifest = JSON.parse(fs.readFileSync(path.resolve(__dirname,
    '../artifacts/academic-us-ccss/batches/T001-lesson-manifest.json'),'utf8'));
  if (manifest.batchId !== 'T001' || manifest.lessons?.length !== 10 ||
      manifest.ordinalStart !== 1 || manifest.ordinalEnd !== 10)
    throw Error('T001 source/manifest scope changed');
  const checks = await fetchChecks(repository, sha, process.env.GITHUB_TOKEN);
  const decision = evaluateChecks(checks, sha);
  process.stdout.write(JSON.stringify(decision, null, 2) + '\n');
  if (!decision.technicalReleaseAccepted) process.exitCode = 1;
  return decision;
}

if (require.main === module) main().catch(e => {
  console.error('[T001 TEST ACCEPTANCE] ' + e.message);
  process.exitCode = 1;
});
module.exports = { evaluateChecks, fetchChecks, main, required };
