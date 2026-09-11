# Unused asset archive — 2026-09-11

These files were moved out of `src/Edulytics.Web/wwwroot` after a repository-wide reachability audit.
They had no path from any non-wwwroot application/test/build source; references between the files themselves formed only an unreachable legacy subgraph.
Moving them outside `src/` keeps them out of `dotnet publish` and the production Docker image while preserving easy restoration history.

Archived files: 40
Removed from published web root: 2917006 bytes

## Files

- `src/Edulytics.Web/wwwroot/css/edulytics-game-engine.css`
- `src/Edulytics.Web/wwwroot/css/edulytics-game-experience-v2.css`
- `src/Edulytics.Web/wwwroot/css/edulytics-game-experience-v3.css`
- `src/Edulytics.Web/wwwroot/css/edulytics-game-experience-v4.css`
- `src/Edulytics.Web/wwwroot/css/edulytics-game-experience-v7.css`
- `src/Edulytics.Web/wwwroot/css/edulytics-game-experience-v9.css`
- `src/Edulytics.Web/wwwroot/css/edulytics-game-v9-story-voice.css`
- `src/Edulytics.Web/wwwroot/images/game/lantern-isles-v5-arrival.svg`
- `src/Edulytics.Web/wwwroot/images/game/lantern-isles-v5-map.svg`
- `src/Edulytics.Web/wwwroot/images/game/lantern-isles-v5-mission.svg`
- `src/Edulytics.Web/wwwroot/images/game/lantern-isles-v5-student.svg`
- `src/Edulytics.Web/wwwroot/images/game/lantern-isles-v6-world-master.webp`
- `src/Edulytics.Web/wwwroot/images/game/v7/lantern-isles-background.svg`
- `src/Edulytics.Web/wwwroot/images/game/v7/lantern-isles-foreground.svg`
- `src/Edulytics.Web/wwwroot/images/game/v7/lantern-isles-terrain.svg`
- `src/Edulytics.Web/wwwroot/images/game/v9/lumen-trail-world.webp`
- `src/Edulytics.Web/wwwroot/images/game/v9/student-explorer.webp`
- `src/Edulytics.Web/wwwroot/images/public/edulytics-math-mascot-animation.png`
- `src/Edulytics.Web/wwwroot/js/curriculum-workspace-runtime.js`
- `src/Edulytics.Web/wwwroot/js/game/activities/count-touch-and-check.v1.activity.js`
- `src/Edulytics.Web/wwwroot/js/game/activities/join-groups-to-add.activity.js`
- `src/Edulytics.Web/wwwroot/js/game/activities/join-groups-to-add.v2.activity.js`
- `src/Edulytics.Web/wwwroot/js/game/activities/join-groups-to-add.v3.activity.js`
- `src/Edulytics.Web/wwwroot/js/game/activities/join-groups-to-add.v4.activity.js`
- `src/Edulytics.Web/wwwroot/js/game/activities/join-groups-to-add.v7.activity.js`
- `src/Edulytics.Web/wwwroot/js/game/activities/join-groups-to-add.v9.activity.js`
- `src/Edulytics.Web/wwwroot/js/game/counting-grove-v1.js`
- `src/Edulytics.Web/wwwroot/js/game/edulytics-game-engine-v2.js`
- `src/Edulytics.Web/wwwroot/js/game/edulytics-game-engine-v3.js`
- `src/Edulytics.Web/wwwroot/js/game/edulytics-game-engine-v4.js`
- `src/Edulytics.Web/wwwroot/js/game/edulytics-game-engine.js`
- `src/Edulytics.Web/wwwroot/js/game/edulytics-game-v4-renderer.js`
- `src/Edulytics.Web/wwwroot/js/game/edulytics-game-v7-renderer.js`
- `src/Edulytics.Web/wwwroot/js/game/edulytics-game-v9-story-voice.js`
- `src/Edulytics.Web/wwwroot/js/game/edulytics-game-v9.js`
- `src/Edulytics.Web/wwwroot/js/public-home-commercial-v8.js`
- `src/Edulytics.Web/wwwroot/js/public-site-routing-v26.js`
- `src/Edulytics.Web/wwwroot/js/temporary-practice-preview.js`
- `src/Edulytics.Web/wwwroot/phase29-cambridge-baseline-v2.txt`
- `src/Edulytics.Web/wwwroot/phase29-cambridge-primary-stage1-v1.txt`

## Restore

Move any required file back to its original path under the repository root and add a real application reference before deployment.
