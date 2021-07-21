import { exec } from 'child_process';
import glob from 'glob';
import { promisify } from 'util';

export const execAsync = promisify(exec);
export const globAsync = promisify(glob);

export const command = (...args: string[]): string => {
  return args.join(' ');
}

export const gitRootAsync = async (): Promise<string> => {
  const revparse = command(
    'git',
    'rev-parse',
    '--show-toplevel');

  const result = await execAsync(revparse);
  return result.stdout.trim();
}

export function write(message: string, ...optionalParams: any[]): void {
  if (process.env.DEBUG) {
    console.log(message, optionalParams);
  }
};
