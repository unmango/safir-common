write() {
    if [ -n "$DEBUG" ] && $DEBUG; then
        echo $1;
    fi
}
