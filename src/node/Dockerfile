FROM node:16.6.0

ENV CI=true
WORKDIR /work
COPY .yarn/ .yarn/
COPY package.json .yarnrc.yml yarn.lock ./
RUN yarn install --immutable --check-cache

COPY . .
