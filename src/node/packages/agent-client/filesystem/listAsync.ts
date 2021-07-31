import { firstValueFrom, toArray } from 'rxjs';
import { ResponseCallbacks } from '../clients/helpers';
import { list } from './list';

export function listAsync(
  baseUrl: string,
  callbacks?: ResponseCallbacks
): Promise<string[]> {
  return firstValueFrom(
    list(baseUrl, callbacks).pipe(toArray()),
    { defaultValue: [] });
};
