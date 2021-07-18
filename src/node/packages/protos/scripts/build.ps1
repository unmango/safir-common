#!/usr/bin/pwsh

$ErrorActionPreference = "Stop"

Import-Module $PSScriptRoot/helpers.ps1

$DIR = "$(git rev-parse --show-toplevel)"
$DIR = Join-Path -Path $DIR -ChildPath "protos"
Write "DIR set to $DIR"

$OUT_DIR = Resolve-Path "$PSScriptRoot/../dist"
Write "OUT_DIR set to $OUT_DIR"

if (Test-Path $OUT_DIR) {
    Write "Removing existing OUT_DIR";
    rm -r $OUT_DIR;
}

Write "Creating OUT_DIR";
mkdir -p $OUT_DIR;

$FILES = Get-ChildItem -Path $DIR -Recurse -Filter "*.proto" -Name

protoc -I="$DIR" $FILES `
    --grpc-web_out=import_style=commonjs+dts,mode=grpcwebtext:$OUT_DIR
