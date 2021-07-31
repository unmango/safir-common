import { Observable } from 'rxjs';
import { list, listAsync } from '../filesystem';
import { ResponseCallbacks } from '../types';

export interface FileSystemClient {
  list(callbacks?: ResponseCallbacks): Observable<string>;
  listAsync(callbacks?: ResponseCallbacks): Promise<string[]>;
}

export function createClient(baseUrl: string): FileSystemClient {
  return {
    list: (c) => list(baseUrl, c),
    listAsync: (c) => listAsync(baseUrl, c),
  };
}
