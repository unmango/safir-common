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
