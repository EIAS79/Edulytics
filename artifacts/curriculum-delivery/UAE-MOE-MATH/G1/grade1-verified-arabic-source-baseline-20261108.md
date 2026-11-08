# UAE Grade 1 — verified Arabic source baseline (2026-11-08)

Scope: `UAE-MOE-MATH`, Grade 1 Common, Arabic. This is a SOURCE BASELINE, **not** a lesson A–B–C closure.

## Machine evidence — Our-CS (read-only)
Root: `C:\Users\khali\Downloads\New folder (2)\emirates\Grade_01\01_Student_Books`

| File | PDF pages | SHA-256 | Identification |
|---|---:|---|---|
| `01.pdf` | 368 | `BF2C47A9F2F5457AB4ABF48EFE5603BD6D946CD1A8A09B4BA1BC57120EFEAE44` | Arabic Grade 1, 2025–2026 student-book cover, teaching pages through unit 3; complete TOC through unit 10 |
| `02.pdf` | 82 | `4D22D715F9D0A49F296A77F87FE519F81A14B5FFC54584C08734E60C66D7E231` | Older Arabic 2019–2020 student-book volume covering unit 4 |
| `03.pdf` | 301 | `2C3E5D47A2AD849CE0109641AA757A92F29D417C25B1EE99FE3851814679A1E1` | Older Arabic 2019–2020 McGraw-Hill volumes 5/6; PDF teaching pages cover units 7–10, TOC includes units 1–10 |

Verification mode: file hashes and page counts confirmed on Our-CS; 02/03 edition/coverage verified from book covers/TOC uploaded in conversation. The 3 device PDFs are image-only for `pypdf` text extraction. **Do not mistake an empty extracted-text result for absent content.**

## Current 2025–2026 authority for later terms

- Term 2: user-provided Arabic UAE MOE `رياضيات الدليل الشامل 1 11.pdf`, 90 pages: PDF p.15 lists **U4 = 8 lessons; U5 = 15 lessons**.
- Term 3: user-provided Arabic UAE MOE `الدليل الشامل الرياضيات المتكاملة الصف الأول الفصل الدراسي الثالث 2025-2026.pdf`, 146 pages: PDF p.18 lists **U6 = 8; U7 = 6; U8 = 9; U9 = 10; U10 = 4** lessons. Codes for U6 are printed on p.19.
- Former Arabic 2019–2020 student-book table of contents confirms the same titles/sequence for U4 and U6–U10, **but a matching recent student-book teaching volume has not been confirmed for all terms**. Treat teacher-style comprehensive guides as curriculum-structure/outcomes authority, not proof that each student-book exercise was opened.

## Reconciled count

Existing GitHub blueprint has 11 technical units: U01 introductory (6 lessons) plus U02–U11 corresponding to textbook U1–U10. Counts: `6+13+14+9+8+15+8+6+9+10+4=102`. Formal textbook units = **10**, plus introductory `A` unit = **11 technical units**. Term 2 = 23; term 3 = 37; first-term introductory+U1–U3 = 42. `42+23+37=102`. No unsupported assumption that numbering of technical units and printed units is identical.

This cross-check supports the **102-unit-lesson count and order for planning**. It does not independently establish full Stage A element coverage or justify deleting/migrating any existing LessonCode. Recheck each lesson title/term during sequential closure.

## Resume checkpoint

Branch: `curriculum/uae-g1-arabic-abc-20261108`. Lesson 1: `PED:UAE-MOE-MATH:L1:COMMON:01:01`; original source PDF p.34–41; status `A_C_EXPLICIT_GAP`, blockers `FULL_ATOMIC_SOURCE_AUDIT`, `RUNTIME_FAMILY_ROUTING`, `RTL_UI_NOT_VERIFIED`. Six authored Arabic lessons were prepared on branch, **none yet closed**. Proceed only with lesson 1 until closure evidence is complete; no Stage D/CI run.
