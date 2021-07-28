import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { filter, firstValueFrom, Observable, Subject, toArray } from 'rxjs';
import { agent } from '@unmango/safir-protos';
import { GrpcResponse } from '../grpcResponse';
import { MetadataCallback, responseCallbacks, StatusCallback } from './helpers';

export interface FileSystemClient {
  list(): Observable<GrpcResponse<string>>;
  listAsync(metadata?: MetadataCallback, status?: StatusCallback): Promise<string[]>;
}

const client = (baseUrl: string): agent.FileSystemClient => {
  return new agent.FileSystemClient(baseUrl);
};

export function createClient(baseUrl: string): FileSystemClient {
  return {
    list: () => list(baseUrl),
    listAsync: (m, s) => listAsync(baseUrl, m, s),
  };
}

export function list(
  baseUrl: string,
  metadata?: MetadataCallback,
  status?: StatusCallback): Observable<GrpcResponse<string>> {
  const subject = new Subject<GrpcResponse<string>>();
  const stream = client(baseUrl).list(new Empty());

  stream.on('data', x => subject.next(x as string));
  stream.on('metadata', x => {
    if (metadata) metadata(x);
    else subject.next(x);
  });
  stream.on('status', x => {
    if (status) status(x);
    else subject.next(x);
  });
  stream.on('error', x => subject.error(x));
  stream.on('end', () => subject.complete());

  return subject.asObservable();
};

export function listAsync(
  baseUrl: string,
  metadata: MetadataCallback = _ => { },
  status: StatusCallback = _ => { }): Promise<string[]> {
  return firstValueFrom(
    list(baseUrl).pipe(
      responseCallbacks(metadata, status),
      filter((x): x is string => typeof x === 'string'),
      toArray(),
    ),
    {
      defaultValue: []
    });
};
