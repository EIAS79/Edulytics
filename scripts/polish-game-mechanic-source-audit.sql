-- Verify that every Polish primary lesson (Klasa I-VI) can be keyed to
-- docs/POLISH_PRIMARY_GAME_MECHANIC_SOURCE_MAP_V1.json using UnitTitle + core item.
-- Read-only.

WITH pl AS (
    SELECT
        l."Code" AS lesson_code,
        l."NativeLevel",
        l."LogicalLevelFrom",
        l."UnitTitle",
        n."SourceLocator",
        substring(n."SourceLocator" from 'core:([0-9]+):')::int AS core_item
    FROM "CurriculumPedagogicalLessons" l
    JOIN "CurriculumFrameworkVersions" v ON v."Id" = l."FrameworkVersionId"
    JOIN "CurriculumFrameworks" f ON f."Id" = v."FrameworkId"
    LEFT JOIN "CurriculumPedagogicalLessonOutcomes" plo ON plo."PedagogicalLessonId" = l."Id"
    LEFT JOIN "CurriculumPackContentNodes" n ON n."Id" = plo."OutcomeNodeId"
    WHERE f."Code" = 'PL-NATIONAL-MATH'
      AND l."LogicalLevelFrom" BETWEEN 1 AND 6
), checks AS (
    SELECT *,
        CASE
            WHEN "LogicalLevelFrom" BETWEEN 1 AND 3 THEN CASE "UnitTitle"
                WHEN 'Osiągnięcia w zakresie rozumienia stosunków przestrzennych i cech wielkościowych' THEN core_item BETWEEN 1 AND 3
                WHEN 'Osiągnięcia w zakresie rozumienia liczb i ich własności' THEN core_item BETWEEN 1 AND 4
                WHEN 'Osiągnięcia w zakresie posługiwania się liczbami' THEN core_item BETWEEN 1 AND 4
                WHEN 'Osiągnięcia w zakresie czytania tekstów matematycznych' THEN core_item BETWEEN 1 AND 2
                WHEN 'Osiągnięcia w zakresie rozumienia pojęć geometrycznych' THEN core_item BETWEEN 1 AND 4
                WHEN 'Osiągnięcia w zakresie stosowania matematyki w sytuacjach życiowych oraz w innych' THEN core_item BETWEEN 1 AND 9
                ELSE false
            END
            WHEN "LogicalLevelFrom" BETWEEN 4 AND 6 THEN CASE "UnitTitle"
                WHEN 'Liczby naturalne w dziesiątkowym układzie pozycyjnym' THEN core_item BETWEEN 1 AND 5
                WHEN 'Działania na liczbach naturalnych' THEN core_item BETWEEN 1 AND 15
                WHEN 'Liczby całkowite' THEN core_item BETWEEN 1 AND 5
                WHEN 'Ułamki zwykłe i dziesiętne' THEN core_item BETWEEN 1 AND 14
                WHEN 'Działania na ułamkach zwykłych i dziesiętnych' THEN core_item BETWEEN 1 AND 7
                WHEN 'Elementy algebry' THEN core_item BETWEEN 1 AND 3
                WHEN 'Proste i odcinki' THEN core_item BETWEEN 1 AND 5
                WHEN 'Kąty' THEN core_item BETWEEN 1 AND 6
                WHEN 'Wielokąty, koła i okręgi' THEN core_item BETWEEN 1 AND 8
                WHEN 'Bryły' THEN core_item BETWEEN 1 AND 5
                WHEN 'Obliczenia w geometrii' THEN core_item BETWEEN 1 AND 7
                WHEN 'Obliczenia praktyczne' THEN core_item BETWEEN 1 AND 9
                WHEN 'Elementy statystyki opisowej' THEN core_item BETWEEN 1 AND 2
                WHEN 'Zadania tekstowe' THEN core_item BETWEEN 1 AND 7
                ELSE false
            END
            ELSE false
        END AS mapped_source_key
    FROM pl
)
SELECT
    count(*) AS lessons,
    count(*) FILTER (WHERE mapped_source_key) AS source_keys_mapped,
    count(*) FILTER (WHERE NOT mapped_source_key OR mapped_source_key IS NULL) AS source_keys_unmapped,
    count(DISTINCT ("UnitTitle", core_item)) AS distinct_requirement_keys
FROM checks;

-- Expected result for the audited staging snapshot:
-- lessons = 372
-- source_keys_mapped = 372
-- source_keys_unmapped = 0
-- distinct_requirement_keys = 124
