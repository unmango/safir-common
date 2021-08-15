import { FileSystemClient } from '@unmango/safir-protos/dist/agent';
import { HostClient } from '@unmango/safir-protos/dist';
import { ClientReadableStream, Error, Metadata } from 'grpc-web';
import { Observable } from 'rxjs';

type ServerStreaming<Request, Response> = {
  (request: Request, metadata?: Metadata): ClientReadableStream<Response>;
};

type ServerStreamingProperties<T> = {
  [P in keyof T as T[P] extends ServerStreaming<unknown, unknown> ? P : never]: T[P];
};

type Temp<T> = keyof T extends Record<string, string> ? string : never;

type ObservableStream<Request, Response> = {
  (request: Request, metadata?: Metadata): Observable<Response>;
};

type AsObservableStream<T> = T extends ServerStreaming<infer Request, infer Response>
  ? ObservableStream<Request, Response>
  : never;

type ObservableStreamProperties<T> = {
  [P in keyof T as keyof ServerStreamingProperties<T>]: AsObservableStream<T[P]>;
};

type AsyncStream<Request, Response> = {
  (request: Request, metadata?: Metadata): Promise<Response[]>;
};

type AsAsyncStream<T> = T extends ServerStreaming<infer Request, infer Response>
  ? AsyncStream<Request, Response>
  : never;

type AppendAsync<T> =
  T extends string ? `${T}Async` :
  { [P in keyof T as AppendAsync<P>]: T[P] };

type AsyncStreamProperties<T> = {
  [P in keyof T as keyof AppendAsync<ServerStreamingProperties<T>>]: AsAsyncStream<T[P]>;
};

type Unary<Request, Response> = {
  (request: Request, metadata: Metadata | null): Promise<Response>;
};

type UnaryProperties<T> = {
  [P in keyof T as T[P] extends Unary<unknown, unknown> ? P : never]: T[P];
};

type AsyncUnaryProperties<T> = {
  [P in keyof T as keyof AppendAsync<UnaryProperties<T>>]: T[P]
};

// TODO: Why does it use client_ as the hint prop name?
export type GrpcClient<T> =
  & ObservableStreamProperties<T>
  & AsyncStreamProperties<T>
  & AsyncUnaryProperties<T>;

const fsTest: GrpcClient<FileSystemClient> = {
};

const hostTest: GrpcClient<HostClient> = {
};
