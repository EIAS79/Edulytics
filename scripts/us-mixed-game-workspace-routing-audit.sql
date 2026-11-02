-- Verify deterministic workspace routing for US Common Core mixed units in Grade 1-6.
-- Read-only. This script deliberately recognises composite Center Day sessions.

WITH us AS (
    SELECT
        l."Code" AS lesson_code,
        l."NativeLevel",
        l."UnitTitle",
        l."Title",
        lower(l."Title") AS title_l,
        string_agg(DISTINCT coalesce(n."Code", ''), ' ') AS outcome_codes
    FROM "CurriculumPedagogicalLessons" l
    JOIN "CurriculumFrameworkVersions" v ON v."Id" = l."FrameworkVersionId"
    JOIN "CurriculumFrameworks" f ON f."Id" = v."FrameworkId"
    LEFT JOIN "CurriculumPedagogicalLessonOutcomes" plo ON plo."PedagogicalLessonId" = l."Id"
    LEFT JOIN "CurriculumPackContentNodes" n ON n."Id" = plo."OutcomeNodeId"
    WHERE f."Code" = 'US-CCSS-MATH'
      AND l."NativeLevel" IN ('Grade 1','Grade 2','Grade 3','Grade 4','Grade 5','Grade 6')
      AND l."UnitTitle" IN (
        'Putting It All Together','Putting it All Together',
        'Adding, Subtracting, and Working with Data',
        'Geometry and Time','Geometry, Time, and Money',
        'Measuring Length, Time, Liquid Volume, and Weight',
        'Multiplicative Comparison and Measurement',
        'Place Value Patterns and Decimal Operations'
      )
    GROUP BY l."Code", l."NativeLevel", l."UnitTitle", l."Title"
), routed AS (
    SELECT *, CASE
        WHEN title_l ~ '^center day' THEN 'COMPOSITE_SESSION'

        -- Exact overrides for titles whose surface words conflict with the real learning evidence.
        WHEN lesson_code = 'PED:US-CCSS-MATH:G1:U01:L09' THEN 'DATA_STATISTICS'
        WHEN lesson_code = 'PED:US-CCSS-MATH:G3:U06:L16' THEN 'REASONING_MODELING'
        WHEN lesson_code = 'PED:US-CCSS-MATH:G6:U09:L02' THEN 'RATIO_ALGEBRA'
        WHEN lesson_code IN (
          'PED:US-CCSS-MATH:G3:U06:L13','PED:US-CCSS-MATH:G3:U06:L14',
          'PED:US-CCSS-MATH:G3:U06:L15','PED:US-CCSS-MATH:G3:U08:L12'
        ) THEN 'REASONING_MODELING'
        WHEN lesson_code IN (
          'PED:US-CCSS-MATH:G1:U01:L07','PED:US-CCSS-MATH:G1:U01:L08',
          'PED:US-CCSS-MATH:G1:U01:L12','PED:US-CCSS-MATH:G1:U01:L15',
          'PED:US-CCSS-MATH:G2:U01:L07','PED:US-CCSS-MATH:G2:U01:L09',
          'PED:US-CCSS-MATH:G2:U01:L14'
        ) THEN 'DATA_STATISTICS'
        WHEN lesson_code = 'PED:US-CCSS-MATH:G1:U07:L11' THEN 'FRACTION_DECIMAL_PERCENT'
        WHEN lesson_code IN ('PED:US-CCSS-MATH:G1:U08:L08','PED:US-CCSS-MATH:G2:U09:L05') THEN 'NUMBER_SYSTEM'

        WHEN title_l ~ '(clock|time|a\.m\.|p\.m\.|money|coin|coins|pennies|nickels|dimes|quarters|dollar|doubloon|calendar|timetable|hours, minutes, and seconds)' THEN 'TIME_MONEY'
        WHEN title_l ~ '(bar graph|picture graph|pictogram|survey|data|line plot|plot data|graph and answer)' THEN 'DATA_STATISTICS'
        WHEN title_l ~ '(fraction|halves|thirds|fourths|half |equal pieces|equal parts|one of the pieces|decimal|hundredths|thousandth|tenths|percentage)' THEN 'FRACTION_DECIMAL_PERCENT'
        WHEN title_l ~ '(perimeter|area |volume|shape|triangle|rectangle|polygon|angle|coordinate|reflection|symmetry|solid|flat shapes|parallel|perpendicular|turns and direction|tiny house|which one doesn.t belong)' THEN 'GEOMETRY'
        WHEN title_l ~ '(measure|measuring|measurement|length|mass|capacity|weight|liquid volume|ruler|meter|kilometer|kilogram|gram|liter|litre|milliliter|millilitre|pounds|ounces|unit conversion|two truths and a lie)' THEN 'MEASUREMENT'
        WHEN title_l ~ '(ratio|rate|proportion|equation|expression|unknown|balance|algebra|formula|how do we choose|more than two choices|picking representatives)' THEN 'RATIO_ALGEBRA'
        WHEN title_l ~ '(add|subtract|addition|subtraction|multiply|multiplication|divide|division|product|quotient|difference|sum|make 10|make ten|number bond|formal addition|formal subtraction|formal multiplication|formal division|long multiplication|long division|standard algorithm|order of operations|factor|multiple|prime|divisib|remainder|number talk|filling up)' THEN 'OPERATIONS'
        WHEN title_l ~ '(place value|number line|rounding|negative number|powers of ten|large integers|counting and representing|compare and order|comparing and ordering|tens and ones|read and write numbers|teen numbers|odd and even|sequence|number riddles|compose and decompose numbers|expanded form|grids and in words)' THEN 'NUMBER_SYSTEM'
        WHEN title_l ~ '(count, touch|small groups without counting|estimate, then count|positions in a line|count large collections|how many do you see)' THEN 'OBJECT_COUNTING'
        WHEN title_l ~ '(proof|justification|non-routine|problem solving|modeling|modelling|fermi|book drive)' THEN 'REASONING_MODELING'

        -- Standards are only used as a fallback when the domain evidence is unambiguous.
        WHEN outcome_codes ~ 'CCSS:[1-6]\.G\.' AND outcome_codes !~ 'CCSS:[1-6]\.(NF|RP|EE|SP)\.' THEN 'GEOMETRY'
        WHEN outcome_codes ~ 'CCSS:[1-6]\.NF\.' AND outcome_codes !~ 'CCSS:[1-6]\.(G|RP|EE|SP)\.' THEN 'FRACTION_DECIMAL_PERCENT'
        WHEN outcome_codes ~ 'CCSS:[1-6]\.(RP|EE)\.' AND outcome_codes !~ 'CCSS:[1-6]\.(G|NF|SP)\.' THEN 'RATIO_ALGEBRA'
        WHEN outcome_codes ~ 'CCSS:[1-6]\.SP\.' THEN 'DATA_STATISTICS'
        WHEN outcome_codes ~ 'CCSS:[1-6]\.OA\.' AND outcome_codes !~ 'CCSS:[1-6]\.(G|NF|RP|EE|SP|MD)\.' THEN 'OPERATIONS'
        WHEN outcome_codes ~ 'CCSS:[1-6]\.NBT\.' AND outcome_codes !~ 'CCSS:[1-6]\.(G|NF|RP|EE|SP|MD|OA)\.' THEN 'NUMBER_SYSTEM'
        ELSE 'NEEDS_REVIEW'
    END AS route
    FROM us
)
SELECT route, count(*) AS lessons
FROM routed
GROUP BY route
ORDER BY route;

-- Expected current snapshot:
-- COMPOSITE_SESSION 13
-- DATA_STATISTICS 19
-- FRACTION_DECIMAL_PERCENT 34
-- GEOMETRY 26
-- MEASUREMENT 14
-- NUMBER_SYSTEM 6
-- OBJECT_COUNTING 2
-- OPERATIONS 53
-- RATIO_ALGEBRA 11
-- REASONING_MODELING 7
-- TIME_MONEY 20
-- NEEDS_REVIEW 0
-- Total 205
