#!/bin/bash

set -e

CONTEXT=$(git rev-parse --show-toplevel)
TAG="safir-common-node"
docker build $CONTEXT -t $TAG
