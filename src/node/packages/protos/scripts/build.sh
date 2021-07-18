#!/bin/bash

set -e

while getopts ":i:o:" opt; do
  case $opt in
    i) DIR="$OPTARG"
    ;;
    o) OUT_DIR="$OPTARG"
    ;;
    \?) echo "Invalid option -$OPTARG" >&2
    ;;
  esac
done

if [ ! -d "$DIR" ]; then
    echo "Input directory '$DIR' doesn't exist" >&2;
    exit 1;
fi

if [ -z ${OUT_DIR+x} ]; then
    echo "Output directory not provided" >&2;
    exit 1;
fi

protoc -I=$DIR *.proto \
    --grpc-web_out=import_style=typescript,mode=grpcwebtext:$OUT_DIR
