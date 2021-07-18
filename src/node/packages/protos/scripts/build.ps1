#!/usr/bin/pwsh

$ErrorActionPreference = "Stop"

$DIR = "$(git rev-parse --show-toplevel)"
$DIR = Join-Path -Path $DIR -ChildPath "protos"
echo "DIR set to $DIR"

$OUT_DIR = Resolve-Path "./dist"
echo "OUT_DIR set to $OUT_DIR"

if (Test-Path $OUT_DIR) {
    echo "Removing existing OUT_DIR";
    rm -r $OUT_DIR;
}

echo "Creating OUT_DIR";
mkdir -p $OUT_DIR;

$FILES = Get-ChildItem -Path $DIR -Recurse -Filter "*.proto" -Name

protoc -I="$DIR" $FILES `
    --grpc-web_out=import_style=commonjs+dts,mode=grpcwebtext:$OUT_DIR
