import { Metadata, Status } from 'grpc-web';
import { OperatorFunction, pipe, tap } from 'rxjs';
import { GrpcResponse } from '../grpcResponse';

export interface MetadataCallback {
  (metadata: Metadata): void;
}

export interface StatusCallback {
  (status: Status): void;
}

export interface ResponseCallbacks {
  metadata?: MetadataCallback;
  status?: StatusCallback;
}

export const isMetadata = <T>(response: GrpcResponse<T>): response is Metadata => {
  return response
    && !isStatus(response)
    && typeof response === 'object'
    && Object.values(response).every(x => typeof x === 'string');
};

export const isStatus = <T>(response: GrpcResponse<T>): response is Status => {
  return response
    && typeof response === 'object'
    && 'code' in response
    && 'details' in response;
};

export const responseCallbacks = <T>(
  { status, metadata }: ResponseCallbacks
): OperatorFunction<T, T> => {
  return pipe(
    tap(x => {
      if (status && isStatus(x)) {
        status(x);
      }
    }),
    tap(x => {
      if (metadata && isMetadata(x)) {
        metadata(x);
      }
    }),
  );
};
