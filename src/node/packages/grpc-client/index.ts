import * as protos from '@unmango/safir-protos';
import { ClientReadableStream } from 'grpc-web';
import { GrpcClient } from './types';

type ChangeReturnType<T extends Function, V> =
  T extends (...args: infer A) => any ? (...args: A) => V :
  never;

type FixedFileSystemClient = {
  [P in keyof protos.agent.FileSystemClient]:
    P extends 'listFiles' ? ChangeReturnType<
      protos.agent.FileSystemClient[P],
      ClientReadableStream<protos.agent.FileSystemEntry>
    > :
    protos.agent.FileSystemClient[P];
}

type FixedMediaClient = {
  [P in keyof protos.manager.MediaClient]:
    P extends 'list' ? ChangeReturnType<
      protos.manager.MediaClient[P],
      ClientReadableStream<protos.manager.MediaItem>
    > :
    protos.manager.MediaClient[P];
}

export type FileSystemClient = GrpcClient<FixedFileSystemClient>;
export type HostClient = GrpcClient<protos.HostClient>;
export type MediaClient = GrpcClient<FixedMediaClient>;
