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

for file in "$OUT_DIR"/**/*; do
    file=$(realpath --relative-to="$OUT_DIR" $file);

    if [ "$(dirname $file)" == "." ]; then
        asset $OUT_DIR $file
    else
        asset $OUT_DIR "$(dirname $file)/"
    fi

    case "${file#*.}" in
        "d.ts" | "ts")
            echo "export * from './${file%%.*}';" >> "$OUT_DIR/index.d.ts"
            asset $OUT_DIR "index.d.ts";
            ;;
        "js")
            echo "export * from './${file%.*}';" >> "$OUT_DIR/index.js"
            asset $OUT_DIR "index.js";
            ;;
        *)
            echo "Unsupported file extension ${file#*.}"
            ;;
    esac
done
