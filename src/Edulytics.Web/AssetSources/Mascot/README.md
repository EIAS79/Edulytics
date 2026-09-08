# Mascot source

The three `part-*.b64` files concatenate to the approved transparent PNG used by the public home hero. The Docker build reconstructs `wwwroot/images/brand/edulytics-mascot-full.png` and verifies SHA-256 `12af7b49667e3a3df17cdebeaac25eb0fc6102da656e72dae2d86de55a8b23d7` before publish.

Do not run browser-side canvas cleanup, auto-cropping, bounding-box detection, or white-background removal on this asset.
