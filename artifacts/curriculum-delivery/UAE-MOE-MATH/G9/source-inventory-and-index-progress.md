# UAE Grade 9 General — local source inventory and staged index (A–B–C)

Status: `SOURCE_LOCK_PARTIAL`, `A_C_LESSONS_CLOSED=0`, `TestsRun=NOT_RUN`.
Scope: `UAE-MOE-MATH`, Grade 9, General/Common, Arabic RTL. **Do not include Advanced or Applied**.

## Directly observed on Our-CS, 2026-11-19
Local base: `C:\Users\khali\Downloads\New folder (2)\emirates\Grade_09\01_Student_Books`.
These PDF files contain scanned pages, with no usable embedded text in initial extraction.

- `01.pdf` — 441 PDF pages; Arabic UAE Grade 9 **General** math volume; the inspected copyright page says 2025. Its contents overview lists Units **1–7**. Unit 1–4 TOC pages were directly inspected as PDF pages 5–8 (1-based). No evidence yet that this file includes all terms/units.
- `02.pdf` — 200 pages; cover identifies **Applied pathway** (المسار التطبيقي), 2022–2023. **Excluded** from the General source set.
- `03.pdf` — 131 pages; cover identifies **Applied pathway**, 2022–2023. **Excluded**.
- `02-1.pdf` — 262 pages; older Arabic Grade 9 Integrated Math volume 2 (copyright 2020); TOC overview lists 13 units. **Alternate edition only**, not automatically merged with 2025 General.
- `03-1.pdf` — 265 pages; older Arabic Grade 9 Integrated Math term 3 (copyright 2020); TOC overview lists 13 units. **Alternate edition only**.

The original `_GRADE_INVENTORY.csv` is not a reliable live file index: its historic filenames differ from names present in the current folder. No Advanced content should be used.

## Verified partial General index from 01.pdf

| Unit | Arabic unit title | Numbered lessons verified in contents pages |
|---|---|---|
| 1 | التعابير والمعادلات والدوال | 8 |
| 2 | المعادلات الخطية | 9 |
| 3 | الدوال الخطية | 6 |
| 4 | معادلات الدوال الخطية | 4 |

Total **27 numbered lessons in Units 1–4**, *not the complete Grade 9 denominator*.

### Unit 1 — TOC page iv (PDF page 5)

1-1 المتغيرات والتعابير — printed page 5
1-2 ترتيب العمليات — p. 10
1-3 خصائص الأعداد — p. 16
1-4 خاصية التوزيع — p. 25
1-5 المعادلات — p. 33
1-6 العلاقات — p. 40
1-7 الدوال — p. 47
1-8 تفسير التمثيل البياني للدالة — p. 56

### Unit 2 — TOC page v (PDF page 6)

2-1 كتابة المعادلات — p. 75
2-2 حل معادلات الخطوة الواحدة — p. 83
2-3 حل المعادلات متعددة الخطوات — p. 91
2-4 حل معادلات تتضمن متغيرًا في كل طرف — p. 97
2-5 حل معادلات تتضمن قيمة مطلقة — p. 103
2-6 النسب والتناسب — p. 111
2-7 النسبة المئوية للتغير — p. 119
2-8 المعادلات الحرفية والتحليل البعدي — p. 126
2-9 المتوسط الحسابي المرجح — p. 132

### Unit 3 — TOC page vi (PDF page 7)

3-1 تمثيل المعادلات الخطية بيانيًا — p. 155
3-2 حل المعادلات الخطية بالتمثيل البياني — p. 163
3-3 معدل التغير والميل — p. 172
3-4 التغير الطردي — p. 182
3-5 المتتابعات الحسابية على شكل دوال خطية — p. 189
3-6 العلاقات التناسبية وغير التناسبية — p. 197

### Unit 4 — TOC page vii (PDF page 8)

4-1 تمثيل المعادلات بيانيًا بصيغة الميل والمقطع — p. 216
4-2 كتابة المعادلات بصيغة الميل والمقطع — p. 226
4-3 كتابة المعادلات بصيغة النقطة والميل — p. 233
4-4 المستقيمات المتوازية والمتعامدة — p. 239

## Next strict source-lock operations

1. Inspect remainder of `01.pdf` contents (Units 5–7), all valid **General** books, and edition/term continuity; determine if complete Grade 9 General book set exists. Do not silently fill gaps from Applied or old Integrated.
2. Compute source SHA-256 and exact PDF/printed page mapping for all chosen files; produce a complete `source-index.json` with evidence.
3. Only after complete source lock, compare live Registry/Content and start Lesson 1 in order. Old UAE Grade 9 General/Advanced content was explicitly retired in PR #649; shared engines remain reusable under Reuse → Extend → New Engine.
4. For A–B–C write tests/specifications but **never run Build, tests, semantic audit gates, E2E or CI**. Those run only in Stage D.
5. Do not mark lessons `READY_FOR_STAGE_D` or report academic/technical completion without element-level evidence, checkpoint and a lesson commit.

This file records a **partial verified source inventory**, not completion of a lesson, the academic audit, or Stage D.
