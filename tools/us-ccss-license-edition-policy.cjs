#!/usr/bin/env node
'use strict';
// Phase 01: edition-specific publisher licence screening only.
// A clean screening result is NEVER independent rights clearance or certification.
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');

function screenSource(source) {
  const publisher = String(source.publisher || '');
  const edition = String(source.edition || '');
  const url = String(source.rootUrl || '');
  const claim = String(source.licenseClaim || '');
  const isIm = /Illustrative Mathematics/i.test(publisher);
  const is360 = isIm && (/v[.\s-]*360/i.test(edition) ||
    /(^|[./])accessim\.org\b/i.test(url));
  const nonCommercial = /\bBY[-\s]?NC\b|non.?commercial/i.test(claim);
  const by4 = /\bCC\s*BY\s*4\.0\b/i.test(claim);

  // Neither a general Creative Commons declaration nor a CC BY first edition
  // source authorizes use of IM v.360 in a commercial Edulytics product.
  if (is360) {
    return {
      status: nonCommercial ? 'BLOCKED_COMMERCIAL_V360_LICENSE' :
        'BLOCKED_V360_EDITION_LICENSE_MISMATCH',
      expectedLicense: 'CC BY-NC 4.0 (non-commercial); commercial use requires separate written permission',
      claim, publisher, edition, rootUrl: url
    };
  }

  if (nonCommercial) {
    return {
      status: 'BLOCKED_NONCOMMERCIAL_SOURCE',
      expectedLicense: 'Publisher-specific commercial rights review required',
      claim, publisher, edition, rootUrl: url
    };
  }

  if (!claim.trim()) {
    return {
      status: 'BLOCKED_UNDECLARED_LICENSE',
      expectedLicense: 'Source edition and rights evidence required',
      claim, publisher, edition, rootUrl: url
    };
  }

  if (isIm && !by4) {
    return {
      status: 'BLOCKED_UNVERIFIED_IM_LICENSE',
      expectedLicense: 'Edition-specific IM K-12 Math CC BY 4.0 first-edition proof',
      claim, publisher, edition, rootUrl: url
    };
  }

  // Even for first-edition IM CC BY, attribution, trademarks, linked assets,
  // edited works, third-party exclusions and exact source edition still
  // require independent confirmation per source, not automatic PASS.
  return {
    status: 'PENDING_INDEPENDENT_RIGHTS_REVIEW',
    claim, publisher, edition, rootUrl: url
  };
}

function scanBlueprints(dir) {
  const names = fs.readdirSync(dir).filter(name =>
    /^us-ccss-math-.*\.lesson-blueprint\.json$/i.test(name)).sort();
  const entries = [];
  for (const name of names) {
    const b = JSON.parse(fs.readFileSync(path.join(dir, name), 'utf8'));
    const nested = Array.isArray(b.Sources) && b.Sources.length > 0;
    const sources = nested ? b.Sources.map(x => ({
      sourceKey: x.SourceKey || null,
      publisher: x.Publisher, edition: x.Edition,
      rootUrl: x.RootUrl, licenseClaim: x.License
    })) : [{
      sourceKey: null, publisher: b.SourcePublisher,
      edition: b.SourceEdition, rootUrl: b.SourceRootUrl,
      licenseClaim: b.SourceLicense
    }];
    for (const source of sources) {
      entries.push({
        blueprint: b.BlueprintCode || name,
        sourceFile: name, sourceKey: source.sourceKey,
        ...screenSource(source)
      });
    }
  }
  return { blueprintCount: names.length, sourceCount: entries.length, entries };
}

function run() {
  const dir = path.join(root, 'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs');
  const report = scanBlueprints(dir);
  if (report.blueprintCount !== 17) throw Error('Expected 17 US CCSS publisher blueprint files');
  const blocked = report.entries.filter(x => x.status.startsWith('BLOCKED_'));
  const result = {
    scope: 'US-CCSS-MATH:G1-G12', editionSpecificRightsScreen: 'TECHNICAL_ONLY',
    academicLicenseApproved: false, sourceCount: report.sourceCount,
    blueprintCount: report.blueprintCount, blockedCount: blocked.length,
    independentRightsDecisions: 0,
    blockingSources: blocked
  };
  process.stdout.write(JSON.stringify(result, null, 2) + '\n');
  // Never elevate the rights-review state on a successful run. A blocking
  // mismatch is a build-time alert; no approval/signoff is produced.
  if (blocked.length) process.exitCode = 2;
}

if (require.main === module) run();
module.exports = {screenSource, scanBlueprints};
