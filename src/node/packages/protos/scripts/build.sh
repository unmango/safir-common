#!/bin/bash

set -e

DIR="$(git rev-parse --show-toplevel)/protos"
echo "DIR set to $DIR"

OUT_DIR="$(readlink -f ./dist)"
echo "OUT_DIR set to $OUT_DIR"

if [ -d "$OUT_DIR" ]; then
    echo "Removing existing OUT_DIR";
    rm -r $OUT_DIR;
fi

echo "Creating OUT_DIR";
mkdir -p $OUT_DIR;

protoc -I=$DIR "$DIR"/**/*.proto \
    --grpc-web_out=import_style=commonjs+dts,mode=grpcwebtext:$OUT_DIR
