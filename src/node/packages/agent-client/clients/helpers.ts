import { Metadata, Status } from 'grpc-web';

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
