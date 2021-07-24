import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { firstValueFrom, Observable, Subject, toArray } from 'rxjs';
import { agent } from '@safir/protos';

export default class FileSystemClient {

  constructor(private _client: agent.FileSystemClient) { }

  public list(): Observable<string> {
    const subject = new Subject<string>();
    const stream = this._client.list(new Empty());

    stream.on('data', x => subject.next(x as string));
    stream.on('error', x => subject.error(x));
    stream.on('end', () => subject.complete());

    return subject.asObservable();
  }

  public listAsync(): Promise<string[]> {
    const result = this.list().pipe(
      toArray(),
    );

    return firstValueFrom(result, {
      defaultValue: []
    });
  }

}
