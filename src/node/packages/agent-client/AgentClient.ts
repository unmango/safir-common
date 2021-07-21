import { FileSystemClient } from '@safir/protos/dist/agent';

export default class AgentClient {

  private readonly _baseUrl: string;
  private readonly _files: FileSystemClient;

  constructor(baseUrl: string) {
    this._baseUrl = baseUrl;
    this._files = new FileSystemClient(baseUrl);
  }

}
