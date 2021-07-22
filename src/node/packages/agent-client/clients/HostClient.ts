import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { agent } from '@safir/protos';

export default class HostClient {

  private readonly _client = new agent.HostClient(this.baseUrl);

  constructor(private baseUrl: string) { }

  public getInfoAsync(): Promise<agent.HostInfo> {
    return this._client.getInfo(new Empty(), null);
  }

}
