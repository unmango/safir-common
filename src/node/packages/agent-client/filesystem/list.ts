import { FileSystemClient } from '@unmango/safir-protos/dist/agent';
import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { Observable, Subject } from 'rxjs';
import { ResponseCallbacks } from '../clients/helpers';

const client = (baseUrl: string): FileSystemClient => {
  return new FileSystemClient(baseUrl);
};

export function list(
  baseUrl: string,
  callbacks?: ResponseCallbacks
): Observable<string> {
  const subject = new Subject<string>();
  const stream = client(baseUrl).list(new Empty());

  if (callbacks?.metadata) {
    stream.on('metadata', callbacks.metadata);
  }

  if (callbacks?.status) {
    stream.on('status', callbacks.status);
  }

  stream.on('data', x => subject.next(x as string));
  stream.on('error', e => subject.error(e));
  stream.on('end', () => subject.complete());

  return subject.asObservable();
};
