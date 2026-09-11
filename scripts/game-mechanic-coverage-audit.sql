-- Edulytics game mechanic coverage audit
-- Scope: Grade/Stage 1-6 for US CCSS, Polish National, UAE MoE and Cambridge International Mathematics.
-- Read-only audit. Do not mutate curriculum data from this script.

-- 1) Canonical in-scope lesson set.
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

-- Expected v0.2 total: 1,624 lessons.
-- CAMBRIDGE 169; PL 372; UAE 204; US 879.

-- 2) Safe direct workspace routing vs deep routing.
-- A broad/mixed unit is deliberately sent to DEEP_ROUTING instead of being force-mapped.
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
), mapped AS (
    SELECT *,
        CASE
            WHEN framework_code = 'CAMBRIDGE-INTL-MATH' THEN CASE
                WHEN "UnitTitle" IN ('Number Sense and Sequences','Shape, Measure and Position','Fractions and Money') THEN 'DEEP_ROUTING'
                WHEN "UnitTitle" = 'Number and Place Value' THEN 'NUMBER_SYSTEM'
                WHEN "UnitTitle" = 'Fractions' THEN 'FRACTION_DECIMAL_PERCENT'
                WHEN "UnitTitle" IN ('Additive Structures and Relationships','Multiplicative Structures and Relationships','Number Facts and Fluency','Addition, Subtraction and Doubles') THEN 'OPERATIONS'
                WHEN "UnitTitle" = 'Geometry and Measure' THEN 'GEOMETRY'
                WHEN "UnitTitle" = 'Categorical Data' THEN 'DATA_STATISTICS'
                WHEN "UnitTitle" = 'Time' THEN 'TIME_MONEY'
                ELSE 'NEEDS_REVIEW'
            END
            WHEN framework_code = 'PL-NATIONAL-MATH' THEN CASE
                WHEN "UnitTitle" IN (
                    'Obliczenia praktyczne',
                    'Osiągnięcia w zakresie stosowania matematyki w sytuacjach życiowych oraz w innych',
                    'Osiągnięcia w zakresie czytania tekstów matematycznych',
                    'Osiągnięcia w zakresie posługiwania się liczbami',
                    'Osiągnięcia w zakresie rozumienia stosunków przestrzennych i cech wielkościowych'
                ) THEN 'DEEP_ROUTING'
                WHEN "UnitTitle" ~ 'Ułam|ułam|ulam|Ulam' THEN 'FRACTION_DECIMAL_PERCENT'
                WHEN "UnitTitle" = 'Działania na liczbach naturalnych' THEN 'OPERATIONS'
                WHEN "UnitTitle" IN ('Liczby całkowite','Liczby naturalne w dziesiątkowym układzie pozycyjnym','Osiągnięcia w zakresie rozumienia liczb i ich własności') THEN 'NUMBER_SYSTEM'
                WHEN "UnitTitle" IN ('Wielokąty, koła i okręgi','Obliczenia w geometrii','Kąty','Bryły','Proste i odcinki','Osiągnięcia w zakresie rozumienia pojęć geometrycznych') THEN 'GEOMETRY'
                WHEN "UnitTitle" = 'Elementy algebry' THEN 'RATIO_ALGEBRA'
                WHEN "UnitTitle" = 'Elementy statystyki opisowej' THEN 'DATA_STATISTICS'
                WHEN "UnitTitle" = 'Zadania tekstowe' THEN 'REASONING_MODELING'
                ELSE 'NEEDS_REVIEW'
            END
            WHEN framework_code = 'UAE-MOE-MATH' THEN CASE
                WHEN "UnitTitle" IN ('Measurement','Number Sense','Number','Number and Calculation','Number and Operations','Geometry and Data') THEN 'DEEP_ROUTING'
                WHEN "UnitTitle" = 'Advanced Reasoning' THEN 'REASONING_MODELING'
                WHEN "UnitTitle" IN ('Fractions','Fractions and Decimals','Fractions, Decimals and Percentages') THEN 'FRACTION_DECIMAL_PERCENT'
                WHEN "UnitTitle" IN ('Geometry','Geometry and Measure') THEN 'GEOMETRY'
                WHEN "UnitTitle" IN ('Statistics','Data') THEN 'DATA_STATISTICS'
                WHEN "UnitTitle" IN ('Ratio and Algebra','Ratio and Proportion','Patterns and Relations') THEN 'RATIO_ALGEBRA'
                WHEN "UnitTitle" = 'Multiplication and Division' THEN 'OPERATIONS'
                WHEN "UnitTitle" = 'Number and Place Value' THEN 'NUMBER_SYSTEM'
                ELSE 'NEEDS_REVIEW'
            END
            WHEN framework_code = 'US-CCSS-MATH' THEN CASE
                WHEN "UnitTitle" IN (
                    'Putting It All Together','Putting it All Together',
                    'Adding, Subtracting, and Working with Data',
                    'Geometry and Time','Geometry, Time, and Money',
                    'Measuring Length, Time, Liquid Volume, and Weight',
                    'Multiplicative Comparison and Measurement',
                    'Place Value Patterns and Decimal Operations'
                ) THEN 'DEEP_ROUTING'
                WHEN "UnitTitle" ~* '(Data Sets|Working with Data)' THEN 'DATA_STATISTICS'
                WHEN "UnitTitle" ~* '(Fraction|Decimal)' THEN 'FRACTION_DECIMAL_PERCENT'
                WHEN "UnitTitle" ~* '(Expressions and Equations|Ratio|Rates|Percentages)' THEN 'RATIO_ALGEBRA'
                WHEN "UnitTitle" ~* '(Geometry|Shape|Angle|Area|Perimeter|Volume|Coordinate)' THEN 'GEOMETRY'
                WHEN "UnitTitle" ~* '(Measuring Length|Length Measurements)' THEN 'MEASUREMENT'
                WHEN "UnitTitle" ~* '(Adding|Subtracting|Addition|Subtraction|Multiplication|Division|Multiplying|Dividing|Equal Groups|Factors and Multiples|Arithmetic)' THEN 'OPERATIONS'
                WHEN "UnitTitle" ~* '(Numbers to|Hundredths to Hundred-thousands|Rational Numbers)' THEN 'NUMBER_SYSTEM'
                ELSE 'NEEDS_REVIEW'
            END
            ELSE 'NEEDS_REVIEW'
        END AS workspace
    FROM scoped
)
SELECT workspace, framework_code, count(*) AS lesson_count
FROM mapped
GROUP BY workspace, framework_code
ORDER BY workspace, framework_code;

-- Expected current safe routing totals:
-- DIRECT WORKSPACE = 1,252
-- DEEP_ROUTING = 372
-- Total = 1,624

-- 3) Deep-routing rows with the best available curriculum evidence.
WITH scoped AS (
    SELECT
        f."Code" AS framework_code,
        l."Id" AS lesson_id,
        l."Code" AS lesson_code,
        l."NativeLevel",
        l."LogicalLevelFrom",
        l."UnitTitle",
        l."Title",
        CASE
            WHEN f."Code" = 'CAMBRIDGE-INTL-MATH' AND l."UnitTitle" IN ('Number Sense and Sequences','Shape, Measure and Position','Fractions and Money') THEN true
            WHEN f."Code" = 'PL-NATIONAL-MATH' AND l."UnitTitle" IN (
                'Obliczenia praktyczne',
                'Osiągnięcia w zakresie stosowania matematyki w sytuacjach życiowych oraz w innych',
                'Osiągnięcia w zakresie czytania tekstów matematycznych',
                'Osiągnięcia w zakresie posługiwania się liczbami',
                'Osiągnięcia w zakresie rozumienia stosunków przestrzennych i cech wielkościowych'
            ) THEN true
            WHEN f."Code" = 'UAE-MOE-MATH' AND l."UnitTitle" IN ('Measurement','Number Sense','Number','Number and Calculation','Number and Operations','Geometry and Data') THEN true
            WHEN f."Code" = 'US-CCSS-MATH' AND l."UnitTitle" IN (
                'Putting It All Together','Putting it All Together',
                'Adding, Subtracting, and Working with Data',
                'Geometry and Time','Geometry, Time, and Money',
                'Measuring Length, Time, Liquid Volume, and Weight',
                'Multiplicative Comparison and Measurement',
                'Place Value Patterns and Decimal Operations'
            ) THEN true
            ELSE false
        END AS requires_deep_routing
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
    s.framework_code,
    s."NativeLevel",
    s.lesson_code,
    s."UnitTitle",
    s."Title",
    n."Code" AS outcome_code,
    n."SourceLocator",
    n."OfficialText",
    n."AuthorDescription",
    t."QuickSummary",
    t."KeyConceptsAndRules"
FROM scoped s
LEFT JOIN "CurriculumPedagogicalLessonOutcomes" plo ON plo."PedagogicalLessonId" = s.lesson_id
LEFT JOIN "CurriculumPackContentNodes" n ON n."Id" = plo."OutcomeNodeId"
LEFT JOIN "CurriculumLessonContents" c ON c."LessonNodeId" = s.lesson_id
LEFT JOIN "CurriculumLessonContentTranslations" t
    ON t."CurriculumLessonContentId" = c."Id"
    AND t."CultureCode" = CASE WHEN s.framework_code = 'PL-NATIONAL-MATH' THEN 'pl' ELSE 'en' END
WHERE s.requires_deep_routing
ORDER BY s.framework_code, s."LogicalLevelFrom", s."UnitTitle", s."Title", plo."SortOrder";
