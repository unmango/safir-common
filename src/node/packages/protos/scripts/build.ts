#!/usr/bin/env ts-node

import * as child from 'child_process';
import * as fs from 'fs/promises';
import * as fsSync from 'fs';
import * as glob from 'glob';
import * as path from 'path';
import * as util from 'util';

const execAsync = util.promisify(child.exec);
const globAsync = util.promisify(glob.glob);
const cwd = process.cwd();

(async function () {
  const revParse = await execAsync('git rev-parse --show-toplevel');
  const indir = path.join(revParse.stdout.trim(), 'protos');
  const outdir = path.join(cwd, 'dist');

  await fs.rm(outdir, { recursive: true });

  const webOutOptions = [
    'import_style=commonjs+dts',
    'mode=grpcwebtext'
  ].join(',') + ':' + outdir;

  const globbedProtoPath = path.join(indir, '**/*.proto');
  const files = await globAsync(globbedProtoPath);

  const protocCommand = [
    'protoc',
    '-I=' + indir,
    ...files,
    '--grpc-web_out=' + webOutOptions
  ].join(' ');

  if (!fsSync.existsSync(outdir)) {
    await fs.mkdir(outdir);
  }

  await execAsync(protocCommand);

  const outFiles = await fs.readdir(outdir);
  console.log(outFiles);
}());
