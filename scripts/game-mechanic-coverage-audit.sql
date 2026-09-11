-- Edulytics game mechanic coverage audit
-- Scope: Grade/Stage 1-6 for US CCSS, Polish National, UAE MoE and Cambridge International Mathematics.
-- Read-only audit. Do not mutate curriculum data from this script.

WITH scoped AS (
    SELECT
        f."Code" AS framework_code,
        f."Name" AS framework_name,
        l."Id" AS lesson_id,
        l."Code" AS lesson_code,
        l."NativeLevel",
        l."LogicalLevelFrom",
        l."LogicalLevelTo",
        l."Pathway",
        l."UnitTitle",
        l."Title",
        l."SortOrder"
    FROM "CurriculumPedagogicalLessons" l
    JOIN "CurriculumFrameworkVersions" v ON v."Id" = l."FrameworkVersionId"
    JOIN "CurriculumFrameworks" f ON f."Id" = v."FrameworkId"
    WHERE
        (f."Code" = 'CAMBRIDGE-INTL-MATH' AND l."LogicalLevelFrom" BETWEEN 1 AND 6)
        OR (f."Code" = 'PL-NATIONAL-MATH' AND l."LogicalLevelFrom" BETWEEN 1 AND 6)
        OR (f."Code" = 'UAE-MOE-MATH' AND l."NativeLevel" IN ('Grade 1','Grade 2','Grade 3','Grade 4','Grade 5','Grade 6'))
        OR (f."Code" = 'US-CCSS-MATH' AND l."NativeLevel" IN ('Grade 1','Grade 2','Grade 3','Grade 4','Grade 5','Grade 6'))
)
SELECT framework_code, framework_name, count(*) AS lesson_count
FROM scoped
GROUP BY framework_code, framework_name
ORDER BY framework_code;

-- Expected v0.1 scope total: 1,624 lessons.

WITH scoped AS (
    SELECT f."Code" AS framework_code, l.*
    FROM "CurriculumPedagogicalLessons" l
    JOIN "CurriculumFrameworkVersions" v ON v."Id" = l."FrameworkVersionId"
    JOIN "CurriculumFrameworks" f ON f."Id" = v."FrameworkId"
    WHERE
        (f."Code" = 'CAMBRIDGE-INTL-MATH' AND l."LogicalLevelFrom" BETWEEN 1 AND 6)
        OR (f."Code" = 'PL-NATIONAL-MATH' AND l."LogicalLevelFrom" BETWEEN 1 AND 6)
        OR (f."Code" = 'UAE-MOE-MATH' AND l."NativeLevel" IN ('Grade 1','Grade 2','Grade 3','Grade 4','Grade 5','Grade 6'))
        OR (f."Code" = 'US-CCSS-MATH' AND l."NativeLevel" IN ('Grade 1','Grade 2','Grade 3','Grade 4','Grade 5','Grade 6'))
), routing AS (
    SELECT *,
        CASE
            WHEN framework_code = 'US-CCSS-MATH'
                 AND "UnitTitle" IN (
                    'Putting It All Together',
                    'Putting it All Together',
                    'Adding, Subtracting, and Working with Data',
                    'Geometry and Time',
                    'Geometry, Time, and Money',
                    'Measuring Length, Time, Liquid Volume, and Weight'
                 ) THEN 'MIXED_UNIT_ROUTING'
            WHEN framework_code = 'UAE-MOE-MATH'
                 AND "UnitTitle" IN ('Geometry and Data','Number and Calculation','Number and Operations','Advanced Reasoning')
                 THEN 'MIXED_UNIT_ROUTING'
            WHEN framework_code = 'CAMBRIDGE-INTL-MATH'
                 AND "UnitTitle" IN ('Shape, Measure and Position','Fractions and Money')
                 THEN 'MIXED_UNIT_ROUTING'
            WHEN framework_code = 'PL-NATIONAL-MATH'
                 AND "UnitTitle" IN (
                    'Obliczenia praktyczne',
                    'Osiągnięcia w zakresie stosowania matematyki w sytuacjach życiowych oraz w innych',
                    'Osiągnięcia w zakresie czytania tekstów matematycznych'
                 ) THEN 'NEEDS_DEEP_ROUTING'
            ELSE 'DIRECT_DOMAIN_UNIT'
        END AS routing_class
    FROM scoped
)
SELECT framework_code, routing_class, count(*) AS lesson_count
FROM routing
GROUP BY framework_code, routing_class
ORDER BY framework_code, routing_class;

-- Deep-routing review queue. These rows must not receive a generic multiple-choice fallback.
WITH scoped AS (
    SELECT
        f."Code" AS framework_code,
        l."Id" AS lesson_id,
        l."Code" AS lesson_code,
        l."NativeLevel",
        l."LogicalLevelFrom",
        l."UnitTitle",
        l."Title"
    FROM "CurriculumPedagogicalLessons" l
    JOIN "CurriculumFrameworkVersions" v ON v."Id" = l."FrameworkVersionId"
    JOIN "CurriculumFrameworks" f ON f."Id" = v."FrameworkId"
    WHERE
        (f."Code" = 'CAMBRIDGE-INTL-MATH' AND l."LogicalLevelFrom" BETWEEN 1 AND 6)
        OR (f."Code" = 'PL-NATIONAL-MATH' AND l."LogicalLevelFrom" BETWEEN 1 AND 6)
        OR (f."Code" = 'UAE-MOE-MATH' AND l."NativeLevel" IN ('Grade 1','Grade 2','Grade 3','Grade 4','Grade 5','Grade 6'))
        OR (f."Code" = 'US-CCSS-MATH' AND l."NativeLevel" IN ('Grade 1','Grade 2','Grade 3','Grade 4','Grade 5','Grade 6'))
)
SELECT
    s.*,
    n."Code" AS outcome_code,
    n."SourceLocator",
    n."OfficialText",
    n."AuthorDescription"
FROM scoped s
LEFT JOIN "CurriculumPedagogicalLessonOutcomes" plo ON plo."PedagogicalLessonId" = s.lesson_id
LEFT JOIN "CurriculumPackContentNodes" n ON n."Id" = plo."OutcomeNodeId"
WHERE
    (s.framework_code = 'PL-NATIONAL-MATH' AND s."UnitTitle" IN (
        'Obliczenia praktyczne',
        'Osiągnięcia w zakresie stosowania matematyki w sytuacjach życiowych oraz w innych',
        'Osiągnięcia w zakresie czytania tekstów matematycznych'
    ))
    OR (s.framework_code = 'US-CCSS-MATH' AND s."UnitTitle" IN (
        'Putting It All Together',
        'Putting it All Together',
        'Adding, Subtracting, and Working with Data',
        'Geometry and Time',
        'Geometry, Time, and Money',
        'Measuring Length, Time, Liquid Volume, and Weight'
    ))
    OR (s.framework_code = 'UAE-MOE-MATH' AND s."UnitTitle" IN ('Geometry and Data','Number and Calculation','Number and Operations','Advanced Reasoning'))
    OR (s.framework_code = 'CAMBRIDGE-INTL-MATH' AND s."UnitTitle" IN ('Shape, Measure and Position','Fractions and Money'))
ORDER BY s.framework_code, s."LogicalLevelFrom", s."UnitTitle", s."Title", plo."SortOrder";
