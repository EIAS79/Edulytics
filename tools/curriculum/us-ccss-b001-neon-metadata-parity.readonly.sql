-- B001 READ-ONLY Neon inspection (2026-10-16)
-- To run on the actual Edulytiks production project/branch, using read-only SQL access.
-- No SELECT result constitutes academic certification, HTTP rendering, or real student UAT.
-- IMPORTANT: This deployment's CurriculumLessonContents.LessonNodeId refers
-- to CurriculumPedagogicalLessons.Id, NOT OfficialLessonNodeId (which is NULL).
-- See B001 committed manifest for exact 500 lesson codes. This counts the
-- first 146 + 146 + 143 + first 65 of G4 by official grade ordinal.
-- No writes, no student identifiers or school/teacher data are accessed.

WITH batch AS (
  SELECT p."Code" AS lesson_code, p."NativeLevel" AS grade,
         p."SortOrder" AS grade_ordinal, p."Title" AS title, p."Id" AS pedagogical_id
  FROM public."CurriculumPedagogicalLessons" p
  WHERE p."Code" ~ '^PED:US-CCSS-MATH:G[1-4]:U[0-9]+:L[0-9]+$'
    AND (
      p."NativeLevel" IN ('Grade 1','Grade 2','Grade 3')
      OR (p."NativeLevel" = 'Grade 4' AND p."SortOrder" BETWEEN 1 AND 65)
    )
), parity AS (
  SELECT b.*, c."Id" AS content_id, c."Status" AS content_status,
         c."ContentVersion" AS content_version,
         c."PublishedAtUtc" AS published_at,
         (SELECT count(*) FROM public."CurriculumLessonContentTranslations" t
          WHERE t."CurriculumLessonContentId" = c."Id") AS persisted_translation_rows
  FROM batch b
  LEFT JOIN public."CurriculumLessonContents" c
         ON c."LessonNodeId" = b.pedagogical_id
)
SELECT
  count(*)::integer AS batch_rows,
  count(DISTINCT lesson_code)::integer AS unique_lesson_codes,
  count(*) FILTER (WHERE content_id IS NOT NULL)::integer AS joined_content_rows,
  count(*) FILTER (WHERE content_id IS NULL)::integer AS missing_content,
  count(*) FILTER (WHERE published_at IS NULL)::integer AS not_published_timestamp,
  count(*) FILTER (WHERE content_status <> 3 OR content_status IS NULL)::integer AS non_published_status,
  count(*) FILTER (WHERE content_version IS NULL OR content_version = '')::integer AS missing_content_version,
  count(*) FILTER (WHERE persisted_translation_rows > 0)::integer AS lessons_with_persisted_translations,
  count(*) FILTER (WHERE grade = 'Grade 1')::integer AS g1,
  count(*) FILTER (WHERE grade = 'Grade 2')::integer AS g2,
  count(*) FILTER (WHERE grade = 'Grade 3')::integer AS g3,
  count(*) FILTER (WHERE grade = 'Grade 4')::integer AS g4,
  CASE WHEN count(*) = 500
      AND count(DISTINCT lesson_code) = 500
      AND count(*) FILTER (WHERE content_id IS NULL OR published_at IS NULL
                OR content_status <> 3 OR content_version IS NULL OR content_version = '') = 0
      AND count(*) FILTER (WHERE grade = 'Grade 1') = 146
      AND count(*) FILTER (WHERE grade = 'Grade 2') = 146
      AND count(*) FILTER (WHERE grade = 'Grade 3') = 143
      AND count(*) FILTER (WHERE grade = 'Grade 4') = 65
    THEN 'PRODUCTION_METADATA_PARITY_ONLY'
    ELSE 'METADATA_PARITY_BLOCKED' END AS status
FROM parity;

-- Still required for B001 production acceptance:
-- Compare the result codes/titles/ordinals with manifest LessonCodes, not
-- just grade totals. Load actual effective ReadFromJson translations and check
-- their content fingerprint vs the software deployed at the same Render SHA.
-- Verify authenticated question generation, correct/wrong grading, persisted
-- student attempts and school isolation without accessing real student data.
-- Verify human academic review and exact source/edition licensing separately.
