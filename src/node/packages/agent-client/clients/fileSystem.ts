import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { filter, firstValueFrom, map, Observable, Subject, toArray } from 'rxjs';
import { agent } from '@unmango/safir-protos';
import { GrpcResponse } from '../grpcResponse';

export interface FileSystemClient {
  list(): Observable<GrpcResponse<string>>;
  listAsync(): Promise<string[]>;
}

const client = (baseUrl: string): agent.FileSystemClient => {
  return new agent.FileSystemClient(baseUrl);
};

export function createClient(baseUrl: string): FileSystemClient {
  return {
    list: () => list(baseUrl),
    listAsync: () => listAsync(baseUrl),
  };
}

export function list(baseUrl: string): Observable<GrpcResponse<string>> {
  const subject = new Subject<GrpcResponse<string>>();
  const stream = client(baseUrl).list(new Empty());

  stream.on('data', x => subject.next(x as string));
  stream.on('metadata', x => subject.next(x));
  stream.on('status', x => subject.next(x));
  stream.on('error', x => subject.error(x));
  stream.on('end', () => subject.complete());

  return subject.asObservable();
};

export function listAsync(baseUrl: string): Promise<string[]> {
  return firstValueFrom(
    list(baseUrl).pipe(
      filter(x => typeof x === 'string'),
      map(x => x as string),
      toArray(),
    ),
    {
      defaultValue: []
    });
};
