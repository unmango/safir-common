import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { filter, firstValueFrom, Observable, Subject, toArray } from 'rxjs';
import { agent } from '@unmango/safir-protos';
import { GrpcResponse } from '../grpcResponse';
import { MetadataCallback, responseCallbacks, StatusCallback } from './helpers';
import { Metadata, Status } from 'grpc-web';

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

export function list(baseUrl: string): Observable<GrpcResponse<string>>;
export function list(baseUrl: string, status: StatusCallback): Observable<Metadata | string>;
export function list(baseUrl: string, metadata: MetadataCallback, status?: StatusCallback): Observable<string>;
export function list(
  baseUrl: string,
  either?: MetadataCallback | StatusCallback,
  status?: StatusCallback): Observable<GrpcResponse<string>> {
  const subject = new Subject<GrpcResponse<string>>();
  const stream = client(baseUrl).list(new Empty());
  let metadata: MetadataCallback = _ => { };

  if (status) {

  }

  stream.on('data', x => subject.next(x as string));
  stream.on('metadata', x => {
    if (either) either(x);
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
    list(baseUrl, metadata, status).pipe(
      filter((x): x is string => typeof x === 'string'),
      toArray(),
    ),
    {
      defaultValue: []
    });
};
