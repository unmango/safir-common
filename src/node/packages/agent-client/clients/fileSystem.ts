import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { firstValueFrom, Observable, Subject, toArray } from 'rxjs';
import { FileSystemClient } from '@safir/protos/dist/agent';

const client = (baseUrl: string): FileSystemClient => {
  return new FileSystemClient(baseUrl);
};

export function list(baseUrl: string): Observable<string> {
  const subject = new Subject<string>();
  const stream = client(baseUrl).list(new Empty());

  stream.on('data', x => subject.next(x as string));
  stream.on('error', x => subject.error(x));
  stream.on('end', () => subject.complete());

  return subject.asObservable();
};

export function listAsync(baseUrl: string): Promise<string[]> {
  return firstValueFrom(
    list(baseUrl).pipe(toArray()),
    {
      defaultValue: []
    });
};
