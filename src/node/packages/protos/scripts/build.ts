#!/usr/bin/env ts-node

import * as child from 'child_process';
import * as fs from 'fs/promises';
import * as fsSync from 'fs';
import * as glob from 'glob';
import * as path from 'path';
import * as util from 'util';

const execAsync = util.promisify(child.exec);
const globAsync = util.promisify(glob.glob);

(async function () {
  const revParse = await execAsync('git rev-parse --show-toplevel');
  const cwd = process.cwd();
  write('cwd: ' + cwd);

  const indir = path.join(revParse.stdout.trim(), 'protos');
  write('indir: ' + indir);

  const gendir = path.join(cwd, 'generated');
  write('gendir: ' + gendir);

  const outdir = path.join(cwd, 'dist');
  write('outdir: ' + outdir);

  write('Checking if gendir exists');
  if (fsSync.existsSync(gendir)) {
    write('Removing gendir');
    await fs.rm(gendir, { recursive: true });
  }

  write('Checking if outdir exists');
  if (fsSync.existsSync(outdir)) {
    write('Removing outdir');
    await fs.rm(outdir, { recursive: true });
  }

  const webOutOptions = [
    'import_style=typescript',
    'mode=grpcwebtext'
  ].join(',') + ':' + gendir;
  write('webOutOptions: ' + webOutOptions);

  write('Collecting input files');
  const globbedProtoPath = path.join(indir, '**/*.proto');
  const files = await globAsync(globbedProtoPath);
  write('Files:\n  ' + files.join('\n  '));

  const protocCommand = [
    'protoc',
    '-I=' + indir,
    ...files,
    '--grpc-web_out=' + webOutOptions
  ].join(' ');
  write('protocCommand: ' + protocCommand);

  write('Checking if gendir exists');
  if (!fsSync.existsSync(gendir)) {
    write('Creating gendir');
    await fs.mkdir(gendir);
  }

  write('Executing protocCommand');
  await execAsync(protocCommand);

  write('Collecting directories in outdir');
  const outDirs = await readAllDirs(outdir);
  write('Output directories:\n  ' + outDirs.join('\n  '));

  for (const dir of outDirs) {
    write('Reading files from ' + dir);
    const files = await fs.readdir(dir);

    const dirs: string[] = [];
    const code: string[] = [];

    for (const file of files) {
      const fullPath = path.join(dir, file);
      const stat = await fs.stat(fullPath);

      if (stat.isFile()) {
        code.push(file);
      } else {
        dirs.push(file);
      }
    }

    const indexFile = path.join(dir, 'index.ts');

    if (dirs.length > 0) {
      for (const nDir of dirs) {
        const fullDir = path.join(dir, nDir);
        const nFiles = await fs.readdir(fullDir);

        if (nFiles.length <= 0) continue;

        write('Writing module content to ' + indexFile);
        await fs.appendFile(indexFile, asExport(nDir, true));
      }
    }

    if (code.length > 0) {
      const tsContent = code.map(x => asExport(x)).join('\n') + '\n';
      write('Writing code content to ' + indexFile);
      await fs.appendFile(indexFile, tsContent);
    }
  }

  write('Checking if outdir exists');
  if (!fsSync.existsSync(outdir)) {
    write('Creating outdir');
    await fs.mkdir(outdir);
  }
}());

function write(message: string): void {
  if (process.env.DEBUG) {
    console.log(message);
  }
};

function asExport(file: string, asModule = false): string {
  const module = file
    .replace('.d', '')
    .replace('.ts', '')
    .replace('.js', '');

  return asModule
    ? `export * as ${module} from './${module}'`
    : `export * from './${module}';`;
};

async function readAllDirs(dir: string): Promise<string[]> {
  const stat = await fs.stat(dir);
  if (!stat.isDirectory()) return [];

  const files = await fs.readdir(dir);
  const result: string[] = [dir];

  for (const file of files) {
    const fullPath = path.join(dir, file);
    const nested = await readAllDirs(fullPath);
    result.push(...nested);
  }

  return result;
}

async function readAllFiles(dir: string): Promise<string[]> {
  const stat = await fs.stat(dir);
  if (stat.isFile()) return [dir];

  if (!stat.isDirectory()) return [];

  const files = await fs.readdir(dir);
  const result: string[] = [];

  for (const file of files) {
    const fullPath = path.join(dir, file);
    const nested = await readAllFiles(fullPath);
    result.push(...nested);
  }

  return result;
}
