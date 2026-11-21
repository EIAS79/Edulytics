'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const rel='src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general';
const read=filename=>JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/UAE-MOE-MATH/G9',filename),'utf8'));

test('Unapproved UAE G9 General static HTML and JS are quarantined from ordinary Web publish',()=>{
 const text=fs.readFileSync(path.join(root,'src/Edulytics.Web/Edulytics.Web.csproj'),'utf8');
 assert.match(text,/<Content Update="wwwroot\/lesson-labs\/uae-g9-general\/\*\*\/\*">[\s\S]*?<CopyToPublishDirectory>Never<\/CopyToPublishDirectory>[\s\S]*?<\/Content>/);
 assert.match(text,/Stage D fail-closed/);
 // Scope must be the unreleased UAE grade 9 General authoring labs alone:
 assert.ok(!text.includes('Content Remove="wwwroot/**"'));
 assert.ok(!text.includes('CopyToPublishDirectory>Never</CopyToPublishDirectory>\n    </Content>\n    <Content Update="wwwroot'));
 const src=path.join(root,rel);
 const files=fs.readdirSync(src).filter(x=>/\.(?:html|js)$/.test(x));
 assert.ok(files.length>=80, 'Expected all G9 authored labs still present locally');
 assert.ok(files.includes('u01-l01-original-61-practice-ar.html'));
 assert.ok(files.includes('u01-l01-source-plot-builder.js'));
 assert.ok(files.includes('u13-l09-parallelogram-triangle-area-ar.html'));
});

test('Hosted Stage D workflow pins repository SDK and verifies real output instead of merely parsing the project XML',()=>{
 const workflow=fs.readFileSync(path.join(root,'.github/workflows/uae-grade9-general-stage-d.yml'),'utf8');
 const sdk=JSON.parse(fs.readFileSync(path.join(root,'global.json'),'utf8')).sdk.version;
 assert.ok(workflow.includes("dotnet-version: '"+sdk+"'"),'Hosted SDK does not match global.json');
 assert.ok(workflow.includes('dotnet publish src/Edulytics.Web/Edulytics.Web.csproj'));
 assert.ok(workflow.includes('test -f g9-publish-check/Edulytics.Web.dll'));
 assert.ok(workflow.includes('find g9-publish-check/wwwroot/lesson-labs/uae-g9-general -type f'));
 assert.ok(workflow.includes('test -f g9-publish-check/Edulytics.Web.staticwebassets.endpoints.json'));
 assert.ok(workflow.includes('staticwebassets.endpoints.json'));
 assert.ok(workflow.includes("NO_GO: Unverified Grade 9 General"));
});

test('Render Docker runtime copies only dotnet publish output and cannot re-import draft files from build source',()=>{
 const docker=fs.readFileSync(path.join(root,'Dockerfile'),'utf8');
 const sdk=JSON.parse(fs.readFileSync(path.join(root,'global.json'),'utf8')).sdk.version;
 assert.ok(docker.includes('ARG DOTNET_SDK_VERSION='+sdk));
 assert.ok(docker.includes('dotnet publish'));
 assert.ok(docker.includes('src/Edulytics.Web/Edulytics.Web.csproj'));
 const runtime=docker.split(/^FROM .* AS runtime$/m).at(-1);
 assert.notEqual(runtime,docker,'Docker runtime stage not found');
 assert.ok(runtime.includes('COPY --from=build /app/publish ./'));
 assert.ok(!runtime.includes('COPY src/ src/'));
 assert.ok(!runtime.includes('COPY . .'));
 assert.ok(!runtime.includes('COPY --from=build /src'));
});

test('Quarantine does not silently authorize academic release, CI bypass or database seeding',()=>{
 const ledger=read('grade9-stage-d-execution-ledger.json');
 const progress=read('full-grade-progress.json');
 assert.equal(ledger.firstIncompleteLessonOrdinal,1);
 assert.equal(ledger.sourceSemanticLock.rawLessonAtomicAuditPassed,0);
 assert.equal(ledger.sourceSemanticLock.targetLessonElementParityPassed,0);
 assert.equal(progress.productionPublish,'NOT_STARTED');
 assert.ok(ledger.releaseDecision.startsWith('NO_GO_'));
 const core=fs.readFileSync(path.join(root,'src/Edulytics.Core/Edulytics.Core.csproj'),'utf8');
 assert.ok(!core.includes('Exclude="Curriculum/LessonContent/Packs/uae-g9-general-*.lesson-content-pack.json"'),
   'Owner-approved 82 Arabic canonical bodies may be embedded; this is not a live DB import.');
 assert.equal(read('grade9-release-validation.json').deployed,false);
 assert.equal(read('grade9-release-validation.json').productionWrites,0);
 assert.ok(core.includes('Exclude="Curriculum/LessonContent/RichV2/uae-g9-general-*.rich-lesson-v2.json"'));
});
