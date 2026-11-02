-- US Common Core: export only curriculum lesson/outcome identities for read-only audit.
-- Execute on the intended Neon branch; do not export student, school, user or credential data.
-- Save the returned JSON object as us-ccss-neon-outcome-baseline.json outside the repository.
WITH lesson_mappings AS (
    SELECT
        l."Code" AS lesson_code,
        array_agg(o."Code" ORDER BY o."Code") AS outcome_codes
    FROM public."CurriculumPedagogicalLessons" AS l
    JOIN public."CurriculumPedagogicalLessonOutcomes" AS links
      ON links."PedagogicalLessonId" = l."Id"
    JOIN public."CurriculumPackContentNodes" AS o
      ON o."Id" = links."OutcomeNodeId"
    WHERE l."Code" LIKE 'PED:US-CCSS-MATH:%'
    GROUP BY l."Code"
)
SELECT jsonb_build_object(
    'schemaVersion', 1,
    'readOnly', true,
    'mappings', jsonb_object_agg(lesson_code, outcome_codes)
) AS export_json
FROM lesson_mappings;