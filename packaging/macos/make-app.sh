#!/usr/bin/env bash
# Wraps a `dotnet publish` output into KeyForge.app and zips it. Must run on macOS (needs sips, iconutil, codesign, ditto).
# Usage: make-app.sh <publish-dir> <version> <output-zip>
set -euo pipefail

PUBLISH_DIR="$1"
VERSION="$2"
OUTPUT_ZIP="$3"
HERE="$(cd "$(dirname "$0")" && pwd)"

WORK="$(mktemp -d)"
APP="$WORK/KeyForge.app"
mkdir -p "$APP/Contents/MacOS" "$APP/Contents/Resources"

# Everything the single-file publish produced, minus debug symbols.
rsync -a --exclude='*.pdb' "$PUBLISH_DIR"/ "$APP/Contents/MacOS/"
chmod +x "$APP/Contents/MacOS/KeyForge"

sed "s/__VERSION__/$VERSION/g" "$HERE/Info.plist" > "$APP/Contents/Info.plist"

ICONSET="$WORK/KeyForge.iconset"
mkdir -p "$ICONSET"
for size in 16 32 128 256 512; do
  sips -z "$size" "$size" "$HERE/icon-1024.png" --out "$ICONSET/icon_${size}x${size}.png" > /dev/null
  sips -z "$((size * 2))" "$((size * 2))" "$HERE/icon-1024.png" --out "$ICONSET/icon_${size}x${size}@2x.png" > /dev/null
done
iconutil -c icns "$ICONSET" -o "$APP/Contents/Resources/KeyForge.icns"

# Ad-hoc signature (no paid certificate): required for arm64 binaries to launch, does not remove the Gatekeeper prompt.
codesign --force --deep --sign - "$APP"

# ditto keeps permissions and extended attributes that a plain zip would drop.
ditto -c -k --keepParent "$APP" "$OUTPUT_ZIP"
echo "Created $OUTPUT_ZIP"
