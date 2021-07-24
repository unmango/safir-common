import { injectable } from 'tsyringe';
import { FileSystemClient, HostClient } from './clients';

@injectable()
export default class AgentClient {

  private readonly _fileSystem: FileSystemClient;
  private readonly _host: HostClient;

  constructor(fileSystem: FileSystemClient, host: HostClient) {
    this._fileSystem = fileSystem;
    this._host = host;
  }

  public get fileSystem(): FileSystemClient {
    return this._fileSystem;
  }

  public get host(): HostClient {
    return this._host;
  }

  public static create(baseUrl: string) {

  }

}
