# Previously unused web assets — removal record (2026-10-28)

The original 2026-09-11 reachability audit moved **40 unreachable legacy CSS, JS and image assets** out of the active web root into this archive. Those archived binary/source copies were removed from the current Git tree as part of repository housekeeping on 2026-10-28. **No active `src/Edulytics.Web/wwwroot/` asset was deleted** by this cleanup.

The original 40 files, original full manifest and the audit rationale are retained in immutable Git history at commit `e1228e61ef305127736ec476e3ca6d656821a16b`. To inspect or restore an old file deliberately:

```bash
git show e1228e61ef305127736ec476e3ca6d656821a16b:archive/unused-assets/2026-09-11/README.md
git ls-tree -r --name-only e1228e61ef305127736ec476e3ca6d656821a16b archive/unused-assets/2026-09-11/
git restore --source=e1228e61ef305127736ec476e3ca6d656821a16b -- archive/unused-assets/2026-09-11/
```

Do not re-publish those assets without a fresh reachability and licensing review. This archival-file deletion is not curriculum evidence deletion, and it makes no claim to rewrite historical Git storage.
