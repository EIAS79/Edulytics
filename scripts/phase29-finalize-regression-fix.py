from __future__ import annotations

import copy
import json
from pathlib import Path


# Align the source-acquisition matrix with the verified UAE SourceCatalog
# topology already enforced by MathematicsCurriculumPackRegistry.
matrix_path = Path('docs/PHASE_29_SOURCE_ACQUISITION_MATRIX.json')
matrix = json.loads(matrix_path.read_text(encoding='utf-8'))
uae = next(x for x in matrix['curricula'] if x['packCode'] == 'UAE-MOE-MATH')
old_scopes = {x['logicalLevel']: x for x in uae['scopes']}
if set(old_scopes) != set(range(1, 13)):
    raise SystemExit(f'Unexpected UAE matrix baseline: {sorted(old_scopes)}')

new_scopes = []
for grade in range(1, 13):
    base = old_scopes[grade]
    pathways = ['Common'] if grade <= 4 else ['General', 'Advanced']
    for pathway in pathways:
        scope = copy.deepcopy(base)
        scope['pathway'] = pathway

        if grade == 9 and pathway == 'Advanced':
            scope['pedagogicalSelectionStatus'] = 'ResolvedExact'
            scope['selectionEvidence'] = (
                'Grade 9 Advanced Term 1 has verified lesson-level pilot provenance '
                'and exact accepted UAE lesson/standard alignments.'
            )
            scope['blockingReason'] = ''
            scope['note'] = ''
        else:
            scope['pedagogicalSelectionStatus'] = 'SourceFamilyResolvedExactEditionPending'
            scope['selectionEvidence'] = (
                'Current 2026-2027 UAE Mathematics source-catalog evidence verifies '
                f'Grade {grade} {pathway} scope. Full reviewed lesson-level provenance '
                'for this scope is not yet published.'
            )
            scope['blockingReason'] = ''
            scope['note'] = (
                'Exact grade/track/term textbook lesson sequence must be captured and '
                'reviewed before the full scope becomes Published.'
            )

        new_scopes.append(scope)

if len(new_scopes) != 20:
    raise SystemExit(f'Expected 20 UAE scopes, got {len(new_scopes)}')

expected_keys = (
    [(g, 'Common') for g in range(1, 5)] +
    [(g, p) for g in range(5, 13) for p in ('General', 'Advanced')]
)
actual_keys = [(x['logicalLevel'], x['pathway']) for x in new_scopes]
if actual_keys != expected_keys:
    raise SystemExit(f'UAE scope topology drift: {actual_keys}')

resolved = [
    (x['logicalLevel'], x['pathway'])
    for x in new_scopes
    if x['pedagogicalSelectionStatus'] == 'ResolvedExact'
]
if resolved != [(9, 'Advanced')]:
    raise SystemExit(f'Unexpected resolved UAE scopes: {resolved}')

uae['scopes'] = new_scopes
matrix_path.write_text(
    json.dumps(matrix, ensure_ascii=False, indent=2) + '\n',
    encoding='utf-8')


# The old service coverage fixtures pre-date explicit UAE pathway identity.
# Keep production fail-closed behavior and make the successful fixtures explicit.
test_path = Path('tests/Edulytics.Tests/Phase29/Phase29CanonicalRepositoryCoverageTests.cs')
text = test_path.read_text(encoding='utf-8')
if 'using Edulytics.Core.Curriculum;\n' not in text:
    anchor = 'using Edulytics.Core.Entities;\n'
    if text.count(anchor) != 1:
        raise SystemExit('Expected test using anchor not found exactly once.')
    text = text.replace(anchor, 'using Edulytics.Core.Curriculum;\n' + anchor, 1)

old_call = 'StudentContexts = [Context(versionId, "UAE-MOE-MATH", "Grade 6", 6)]'
count = text.count(old_call)
if count != 2:
    raise SystemExit(f'Expected exactly 2 UAE Grade 6 student fixture calls, found {count}.')
text = text.replace(
    old_call,
    'StudentContexts = [Context(versionId, "UAE-MOE-MATH", "Grade 6", 6, "General")]')

old_helper = '''    private static CanonicalCurriculumContextRecord Context(
        Guid versionId,
        string frameworkCode,
        string gradeName,
        int gradeOrder) =>
        new(
            versionId,
            frameworkCode,
            frameworkCode + " Framework",
            "Version",
            Guid.NewGuid(),
            "Mathematics",
            "MATH",
            Guid.NewGuid(),
            gradeName,
            gradeOrder);
'''
new_helper = '''    private static CanonicalCurriculumContextRecord Context(
        Guid versionId,
        string frameworkCode,
        string gradeName,
        int gradeOrder,
        string? pathway = null)
    {
        var context = new CanonicalCurriculumContextRecord(
            versionId,
            frameworkCode,
            frameworkCode + " Framework",
            "Version",
            Guid.NewGuid(),
            "Mathematics",
            "MATH",
            Guid.NewGuid(),
            gradeName,
            gradeOrder);

        if (string.IsNullOrWhiteSpace(pathway))
            return context;

        var identity = Assert.Single(
            CurriculumLevelIdentityRegistry.ForPack(frameworkCode),
            x => x.LogicalLevel == gradeOrder &&
                 string.Equals(x.Pathway, pathway, StringComparison.Ordinal));

        return context with
        {
            CurriculumLevelKey = identity.Key,
            CurriculumLogicalLevel = identity.LogicalLevel,
            CurriculumLevelLabel = identity.Label,
            CurriculumStage = identity.Stage,
            CurriculumPathway = identity.Pathway
        };
    }
'''
if text.count(old_helper) != 1:
    raise SystemExit('Expected Context helper baseline not found exactly once.')
text = text.replace(old_helper, new_helper, 1)
test_path.write_text(text, encoding='utf-8')

print('Phase 29 final regression alignment applied.')
