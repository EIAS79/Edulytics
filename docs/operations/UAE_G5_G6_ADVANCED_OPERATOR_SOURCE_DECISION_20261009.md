# UAE Reveal Math — operator-approved source decision (2026-10-09)

## Scope and approval
The project operator explicitly approved the locally supplied **Reveal Math UAE Edition Grade 5 Advanced, Volume 2 (2023–2024)** as a pedagogical source for the Edulytics V2 Grade 5 Advanced scope. This is an **operator content-source decision**, not a representation that the UAE Ministry of Education has certified this edition for 2025–2026 or 2026–2027. Do not relabel the source edition.

## Locally verified PDFs
Source directory on operator device `Our-CS`: `C:\Users\khali\Downloads\New folder (2)\emirates`. These are local copyrighted references, not repository assets.

| PDF filename | Cover identity | Edition | Volume | Pages |
|---|---|---|---|---:|
| `Reveal Math G5 Volume 2.pdf` | Grade 5 **Advanced** | 2023–2024 | 2 | 289 |
| `كتاب الطالب المجلد الثاني الرياضيات المتكاملة الصف الخامس ريفيل الفصل الدراسي الثاني 2025-2026.pdf` | Grade 5 **General** | 2025–2026 | 2 | 289 |
| `رياضيات كتاب الطالب متقدم 6 22.pdf` | Grade 6 **Advanced** | 2025–2026 | 1 | 304 |
| `رياضيات-كتاب-الطالب-6-22.pdf` | Grade 6 **General** | 2025–2026 | 1 | 304 |

Verified by inspecting the PDF covers on the connected operator device, not inferring from filenames.

## Volume 2 Grade 5 comparison
The printed table-of-contents pages of the 2023–2024 Advanced and 2025–2026 General PDFs align on unit numbers, unit names, lesson numbering, lesson titles, and listed printed page numbers. **This is a TOC-level comparison only; no full-page equivalence, shared-stream policy, or ministry approval has been proven.**

| Unit | Name | Numbered lessons |
|---|---|---:|
| 8 | Divide Decimals | 6 |
| 9 | Add and Subtract Fractions | 9 |
| 10 | Multiply Fractions | 9 |
| 11 | Divide Fractions | 7 |
| 12 | Measurement and Data | 5 |
| 13 | Geometry | 6 |
| 14 | Algebraic Thinking | 6 |
| **Total** | **7 units** | **48 numbered lessons** |

## Required implementation safeguards
- Preserve the provenance fields `Grade 5 / Advanced / 2023–2024 / Volume 2` and `Grade 6 / Advanced / 2025–2026 / Volume 1` on derived work, and keep source-coverage flags distinct from official-current-year-certification flags.
- Check Grade 5 Advanced Volume 1 and the complete prescribed Grade 6 Advanced scope separately; this approval does **not** establish their coverage.
- Author new pedagogical content; do not copy publisher textbook prose, problems, or illustrations into repository JSON without appropriate redistribution rights.
- Review specific lesson names and reference/outcome mapping before generating blueprint/content JSON. No invented outcomes or false official-standard mappings.
- Retain strict 64-scope audit and existing quality gates. A positive operator decision does **not** imply tests passed; keep PR #419 draft and production cutover NO-GO until tested.
- Original Render production and old Neon must remain untouched; egress benchmark, recovery, and rollback gates remain required.
