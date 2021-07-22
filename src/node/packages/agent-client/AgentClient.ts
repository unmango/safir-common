import { FileSystemClient, HostClient } from './clients';

export default class AgentClient {

  private readonly _fileSystem: FileSystemClient;
  private readonly _host: HostClient;

  constructor(baseUrl: string) {
    this._fileSystem = new FileSystemClient(baseUrl);
    this._host = new HostClient(baseUrl);
  }


  public get fileSystem(): FileSystemClient {
    return this._fileSystem;
  }

  public get host(): HostClient {
    return this._host;
  }

}
