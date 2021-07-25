#!/bin/bash

set -e

VER=$(curl -s "https://api.github.com/repos/grpc/grpc-web/releases/latest" | grep -Po '"tag_name": "\K.*?(?=")');
NAME="protoc-gen-grpc-web-$VER-linux-x86_64";
URL="https://github.com/grpc/grpc-web/releases/download/$VER/$NAME";
BINDIR="$(dirname $0)/.bin";
BIN="$BINDIR/$NAME";

echo "VER: $VER"
echo "NAME: $NAME"
echo "URL: $URL"
echo "BINDIR: $BINDIR"
echo "BIN: $BIN"

if [ ! -d "$BINDIR" ]; then
    echo "Creating BINDIR"
    mkdir -p $BINDIR;
fi

curl -sSL -o $BIN $URL
chmod +x $BIN
echo "$BINDIR" >> $GITHUB_PATH
