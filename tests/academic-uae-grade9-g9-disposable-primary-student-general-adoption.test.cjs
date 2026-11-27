'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
test('G9 disposable secondary login selects exact General adoption without changing production or permanent schools',()=>{
 const source=fs.readFileSync(path.join(root,'tools/Edulytics.CiFixtures/MeetingDemoProvisioner.cs'),'utf8');
 const block=source.split('var primaryLoginClass = SelectLoginClass(')[1]?.split('var secondaryLoginClass =')[0];
 assert.ok(block,'fixture must select both student login classes');
 for(const expected of [
  'definition.Key == "uae-secondary"',
  'definition.Key == "uae-primary"',
  '"UAE-G9"',
  '"UAE-G5"',
  '"UAE-G6"',
  '"UAE-G7"',
  '"GITHUB_ACTIONS"',
  '"EDULYTICS_CI_REHEARSAL"',
  '"ASPNETCORE_ENVIRONMENT"',
  '"Staging"',
  '"edulytics_clean_ci"',
  '? "General"'
 ]) assert.ok(block.includes(expected),expected);
 const envScoped=source.split(/(?:var activeSchools = |:\s*)isUaeG9Ci\s*\?/)[1]?.split(': isUaeG8Ci')[0];
 assert.ok(envScoped?.includes('PrimaryLoginLogicalLevel = 9'));
 assert.ok(envScoped?.includes('DefaultCulture = "ar"'));
 assert.ok(envScoped?.includes('SecondaryLoginPathway = "General"'));
 const python=fs.readFileSync(path.join(root,'tools/curriculum/uae_g9_stage_d_authenticated_https.py'),'utf8');
 assert.ok(python.includes('G9_DISPOSABLE_ADOPTION_SCOPE_MISMATCH'));
 assert.ok(python.includes('pathway=="General"'));
});
