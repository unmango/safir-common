import * as protos from '@unmango/safir-protos';
import { ClientReadableStream } from 'grpc-web';
import { GrpcClient } from './types';

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
