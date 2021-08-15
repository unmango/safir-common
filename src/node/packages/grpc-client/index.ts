import { FileSystemClient } from '@unmango/safir-protos/dist/agent';
import { HostClient } from '@unmango/safir-protos/dist';
import { ClientReadableStream, Metadata } from 'grpc-web';
import { Observable } from 'rxjs';

type ServerStreaming<Request, Response> = {
  (request: Request, metadata?: Metadata): ClientReadableStream<Response>;
};

type ObservableStreamProperties<T> = {
  [P in keyof T as T[P] extends ServerStreaming<unknown, unknown> ? P : never]:
    T[P] extends ServerStreaming<infer Req, infer Res>
      ? (request: Req, metadata?: Metadata) => Observable<Res>
      : never;
};

type AppendAsync<T extends string> = `${T & string}Async`;

type AsyncStreamProperties<T> = {
  [P in keyof T as T[P] extends ServerStreaming<unknown, unknown> ? AppendAsync<P & string> : never]:
    T[P] extends ServerStreaming<infer Req, infer Res>
      ? (request: Req, metadata?: Metadata) => Promise<Res[]>
      : never;
};

type Unary<Request, Response> = {
  (request: Request, metadata: Metadata | null): Promise<Response>;
};

type AsyncUnaryProperties<T> = {
  [P in keyof T as T[P] extends Unary<unknown, unknown> ? AppendAsync<P & string> : never]:
    T[P] extends Unary<infer Req, infer Res>
      ? (request: Req, metadata?: Metadata) => Promise<Res>
      : never;
};

// TODO: Why doesn't it append `Async` in the hint text?
export type GrpcClient<T> =
  & ObservableStreamProperties<T>
  & AsyncStreamProperties<T>
  & AsyncUnaryProperties<T>;

const fsTest: GrpcClient<FileSystemClient> = {
};

const hostTest: GrpcClient<HostClient> = {
};
