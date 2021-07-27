import { Status, Metadata } from 'grpc-web';

export type GrpcResponse<T> = Status | Metadata | T;
