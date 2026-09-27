#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
SRC="$ROOT/src/Edulytics.Web/wwwroot"
OUT="$ROOT/public-frontdoor/dist"

rm -rf "$OUT"
mkdir -p "$OUT/css" "$OUT/images"

cp "$ROOT/public-frontdoor/index.html" "$OUT/index.html"
cp "$ROOT/public-frontdoor/frontdoor.js" "$OUT/frontdoor.js"
cp "$ROOT/public-frontdoor/frontdoor.css" "$OUT/frontdoor.css"
cp -R "$SRC/images/." "$OUT/images/"

CSS_FILES=(
  "css/round2-product-fixes.css"
  "css/public-home.css"
  "css/round4-ux-display.css"
  "css/public-home-commercial-v4.css"
  "css/public-home-commercial-v5.css"
  "css/public-home-commercial-v6.css"
  "css/public-home-commercial-v10.css"
  "css/public-home-commercial-v11.css"
  "css/public-home-commercial-v12.css"
  "css/public-home-commercial-v13.css"
  "css/public-home-commercial-v14.css"
  "css/public-home-commercial-v15.css"
  "css/public-home-commercial-v16.css"
  "css/public-home-commercial-v17.css"
  "css/public-home-philosophy-v23.css"
  "css/public-home-footer-v24.css"
  "css/public-home-navbar-v25.css"
  "css/public-arabic-rtl-v29.css"
  "css/public-home-visual-contract-v32.css"
  "css/public-home-mascot-transparency-v35.css"
  "css/public-home-mascot-v45.css"
  "css/public-trust-v1.css"
  "css/public-footer-layout-v2.css"
  "css/site.css"
)

: > "$OUT/css/public-site.css"
for file in "${CSS_FILES[@]}"; do
  printf '\n/* %s */\n' "$file" >> "$OUT/css/public-site.css"
  cat "$SRC/$file" >> "$OUT/css/public-site.css"
  printf '\n' >> "$OUT/css/public-site.css"
done

printf 'Static front door built at %s\n' "$OUT"
