import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { filter, firstValueFrom, Observable, Subject, toArray } from 'rxjs';
import { agent } from '@unmango/safir-protos';
import { GrpcResponse } from '../grpcResponse';
import { MetadataCallback, ResponseCallbacks, responseCallbacks, StatusCallback } from './helpers';
import { Metadata, Status } from 'grpc-web';

export interface FileSystemClient {
  list(): Observable<GrpcResponse<string>>;
  list(callbacks: { metadata: MetadataCallback; }): Observable<Status | string>;
  list(callbacks: { status: StatusCallback; }): Observable<Metadata | string>;
  list(callbacks: { metadata: MetadataCallback, status: StatusCallback; }): Observable<string>;
  listAsync(callbacks?: ResponseCallbacks): Promise<string[]>;
}

const client = (baseUrl: string): agent.FileSystemClient => {
  return new agent.FileSystemClient(baseUrl);
};

export function createClient(baseUrl: string): FileSystemClient {
  return {
    list: (callbacks?: ResponseCallbacks) => list(baseUrl),
    listAsync: (c) => listAsync(baseUrl, c),
  };
}

export function list(baseUrl: string): Observable<GrpcResponse<string>>;
export function list(
  baseUrl: string,
  callbacks: { status: StatusCallback; }): Observable<Metadata | string>;
export function list(
  baseUrl: string,
  callbacks: { metadata: MetadataCallback; }): Observable<Status | string>;
export function list(
  baseUrl: string,
  callbacks: {
    metadata: MetadataCallback;
    status: StatusCallback;
  }): Observable<string>;
export function list(
  baseUrl: string,
  callbacks?: ResponseCallbacks): Observable<GrpcResponse<string>> {
  const subject = new Subject<GrpcResponse<string>>();
  const stream = client(baseUrl).list(new Empty());

  stream.on('data', x => subject.next(x as string));
  stream.on('metadata', x => {
    if (callbacks?.metadata) callbacks.metadata(x);
    else subject.next(x);
  });
  stream.on('status', x => {
    if (callbacks?.status) callbacks.status(x);
    else subject.next(x);
  });
  stream.on('error', x => subject.error(x));
  stream.on('end', () => subject.complete());

  return subject.asObservable();
};

export function listAsync(
  baseUrl: string,
  callbacks?: ResponseCallbacks): Promise<string[]> {
  return firstValueFrom(
    list(baseUrl).pipe(
      responseCallbacks(callbacks ?? {}),
      filter((x): x is string => typeof x === 'string'),
      toArray(),
    ),
    {
      defaultValue: []
    });
};
