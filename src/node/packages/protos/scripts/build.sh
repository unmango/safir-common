#!/bin/bash

set -e

source $(dirname "$0")/helpers.sh

DIR="$(git rev-parse --show-toplevel)/protos"
write "DIR set to $DIR"

OUT_DIR=$(dirname "$0")/../dist
OUT_DIR=$(readlink -f $OUT_DIR)
write "OUT_DIR set to $OUT_DIR"

if [ -d "$OUT_DIR" ]; then
    write "Removing existing OUT_DIR";
    rm -r $OUT_DIR;
fi

write "Creating OUT_DIR";
mkdir -p $OUT_DIR;

protoc -I=$DIR "$DIR"/**/*.proto \
    --grpc-web_out=import_style=commonjs+dts,mode=grpcwebtext:$OUT_DIR

touch "$OUT_DIR/assets.txt"

for file in "$OUT_DIR"/**/*; do
    file=$(realpath --relative-to="$OUT_DIR" $file);

    if [ "$(dirname $file)" == "." ]; then
        echo "$file" >> "$OUT_DIR/assets.txt";
    else
        grep -qxF "$(dirname $file)/" "$OUT_DIR/assets.txt" || echo "$(dirname $file)/" >> "$OUT_DIR/assets.txt";
    fi

    case "${file#*.}" in
        "d.ts" | "ts")
            echo "export * from './${file%%.*}';" >> "$OUT_DIR/index.d.ts"
            grep -qxF "index.d.ts" "$OUT_DIR/assets.txt" || echo "index.d.ts" >> "$OUT_DIR/assets.txt"
            ;;
        "js")
            echo "export * from './${file%.*}';" >> "$OUT_DIR/index.js"
            grep -qxF "index.js" "$OUT_DIR/assets.txt" || echo "index.js" >> "$OUT_DIR/assets.txt"
            ;;
        *)
            echo "Unsupported file extension ${file#*.}"
            ;;
    esac
done

echo "assets.txt" >> "$OUT_DIR/assets.txt"
