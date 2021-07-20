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

  const outdir = path.join(cwd, 'dist');
  write('outdir: ' + outdir);

  write('Checking if outdir exists');
  if (fsSync.existsSync(outdir)) {
    write('Removing outdir');
    await fs.rm(outdir, { recursive: true });
  }

  const webOutOptions = [
    'import_style=commonjs+dts',
    'mode=grpcwebtext'
  ].join(',') + ':' + outdir;
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

  write('Checking if outdir exists');
  if (!fsSync.existsSync(outdir)) {
    write('Creating outdir');
    await fs.mkdir(outdir);
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
    const ts: string[] = [];
    const js: string[] = [];

    for (const file of files) {
      switch (path.extname(file)) {
        case '.ts':
          ts.push(file);
          break;
        case '.js':
          js.push(file);
          break;
        default:
          dirs.push(file);
      }
    }

    const tsIndex = path.join(dir, 'index.d.ts');
    const jsIndex = path.join(dir, 'index.js');

    if (dirs.length > 0) {
      const content = dirs
        .map(x => asExport(x, true))
        .join('\n') + '\n';

      for (const nDir of dirs) {
        const fullDir = path.join(dir, nDir);
        const nFiles = await fs.readdir(fullDir);

        if (nFiles.length <= 0) continue;

        if (nFiles.some(x => path.extname(x) === '.ts')) {
          write('Writing module content to ' + tsIndex);
          await fs.appendFile(tsIndex, content);
        }

        write('Writing module content to ' + jsIndex);
        await fs.appendFile(jsIndex, content);
      }
    }

    if (ts.length > 0) {
      const tsContent = ts.map(x => asExport(x)).join('\n') + '\n';
      write('Writing ts content to ' + tsIndex);
      await fs.appendFile(tsIndex, tsContent);
    }

    if (js.length > 0) {
      const jsContent = js.map(x => asExport(x)).join('\n') + '\n';
      write('Writing js content to ' + jsIndex);
      await fs.appendFile(jsIndex, jsContent);
    }
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
