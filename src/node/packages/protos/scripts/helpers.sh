#!/bin/bash

write() {
    if [ -n "$DEBUG" ] && $DEBUG; then
        echo $1;
    fi
}

asset() {
    ASSETS="$1/assets.txt"

    if [ ! -f "$ASSETS" ]; then
        write "Creating assets file $ASSETS"
        echo "assets.txt" >> $ASSETS;
    fi

    write "Writing asset '$2' to '$ASSETS'";
    grep -qxF "$2" "$ASSETS" || echo "$2" >> "$ASSETS";
}
