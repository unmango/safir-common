import * as protos from '@unmango/safir-protos';
import { ClientConstructor, GrpcClient } from './types';

export type FileSystemClient = GrpcClient<protos.agent.FileSystemClient>;
export type HostClient = GrpcClient<protos.HostClient>;

