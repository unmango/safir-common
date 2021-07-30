import { FileSystemClient } from '@unmango/safir-protos/dist/agent';
import { Metadata, Status } from 'grpc-web';
import { Observer } from 'rxjs';
import { GrpcResponse } from '../grpcResponse';
import { createClient, list, listAsync } from './fileSystem';
import { MetadataCallback, StatusCallback } from './helpers';

jest.mock('@unmango/safir-protos/dist/agent');

const baseUrl = 'testUrl';

describe('createClient', () => {
  let mock: MockClientReadableStream<GrpcResponse<string>>;
  beforeEach(() => {
    mock = new MockClientReadableStream<GrpcResponse<string>>();
    (FileSystemClient as jest.Mock).mockImplementation(() => ({
      list: () => mock,
    }));
  });

  test('calls list with baseUrl', () => {
    const client = createClient(baseUrl);

    client.list();

    expect(FileSystemClient).toHaveBeenCalledWith(baseUrl);
  });

  test('calls listAsync with baseUrl', async () => {
    const client = createClient(baseUrl);

    const promise = client.listAsync();

    mock.end();

    await promise;

    expect(FileSystemClient).toHaveBeenCalledWith(baseUrl);
  });

  test('calls listAsync with metadata callback', async () => {
    const client = createClient(baseUrl);
    const callback: MetadataCallback = jest.fn();
    const expected: Metadata = { test: 'test' };

    const promise = client.listAsync({ metadata: callback });

    mock.metadata(expected);
    mock.end();

    await promise;

    expect(callback).toHaveBeenCalledWith(expected);
  });

  test('calls listAsync with status callback', async () => {
    const client = createClient(baseUrl);
    const callback: StatusCallback = jest.fn();
    const expected: Status = {
      code: 420,
      details: 'blaze',
    };

    const promise = client.listAsync({ status: callback });

    mock.status(expected);
    mock.end();

    await promise;

    expect(callback).toHaveBeenCalledWith(expected);
  });
});

describe('list', () => {
  let mockStream: MockClientReadableStream<GrpcResponse<string>>;
  let mockObserver: Observer<GrpcResponse<string>>;
  beforeEach(() => {
    mockStream = new MockClientReadableStream<GrpcResponse<string>>();
    (FileSystemClient as jest.Mock).mockImplementation(() => ({
      list: () => mockStream,
    }));

    mockObserver = {
      next: jest.fn(),
      error: jest.fn(),
      complete: jest.fn(),
    };
  });

  test('completes observable when no data', () => {
    const observable = list(baseUrl);
    observable.subscribe(mockObserver);

    mockStream.end();

    expect(mockObserver.next).not.toHaveBeenCalled();
    expect(mockObserver.error).not.toHaveBeenCalled();
    expect(mockObserver.complete).toHaveBeenCalled();
  });

  test('calls error when error occurrs', () => {
    const expectedError = new Error();

    const observable = list(baseUrl);
    observable.subscribe(mockObserver);

    mockStream.error(expectedError);

    expect(mockObserver.next).not.toHaveBeenCalled();
    expect(mockObserver.complete).not.toHaveBeenCalled();
    expect(mockObserver.error).toHaveBeenCalledWith(expectedError);
  });

  test('calls next when data is received', () => {
    const expectedResult = 'data';

    const observable = list(baseUrl);
    observable.subscribe(mockObserver);

    mockStream.data(expectedResult);

    expect(mockObserver.error).not.toHaveBeenCalled();
    expect(mockObserver.complete).not.toHaveBeenCalled();
    expect(mockObserver.next).toHaveBeenCalledWith(expectedResult);
  });

  test('calls next when metadata is received', () => {
    const expectedResult: Metadata = {
      test: 'test',
    };

    const observable = list(baseUrl);
    observable.subscribe(mockObserver);

    mockStream.data(expectedResult);

    expect(mockObserver.error).not.toHaveBeenCalled();
    expect(mockObserver.complete).not.toHaveBeenCalled();
    expect(mockObserver.next).toHaveBeenCalledWith(expectedResult);
  });

  test('calls next when status is received', () => {
    const expectedResult: Status = {
      code: 69,
      details: 'immaturity',
    };

    const observable = list(baseUrl);
    observable.subscribe(mockObserver);

    mockStream.data(expectedResult);

    expect(mockObserver.error).not.toHaveBeenCalled();
    expect(mockObserver.complete).not.toHaveBeenCalled();
    expect(mockObserver.next).toHaveBeenCalledWith(expectedResult);
  });

  test('invokes callback when metadata is received', () => {
    const callback: MetadataCallback = jest.fn();
    const expectedResult: Metadata = {
      test: 'test',
    };

    const observable = list(baseUrl, { metadata: callback });
    observable.subscribe(mockObserver);

    mockStream.metadata(expectedResult);

    expect(mockObserver.error).not.toHaveBeenCalled();
    expect(mockObserver.complete).not.toHaveBeenCalled();
    expect(mockObserver.next).not.toHaveBeenCalled();
    expect(callback).toHaveBeenCalledWith(expectedResult);
  });

  test('invokes callback when status is received', () => {
    const callback: StatusCallback = jest.fn();
    const expectedResult: Status = {
      code: 1995,
      details: 'birthday',
    };

    const observable = list(baseUrl, { status: callback });
    observable.subscribe(mockObserver);

    mockStream.status(expectedResult);

    expect(mockObserver.error).not.toHaveBeenCalled();
    expect(mockObserver.complete).not.toHaveBeenCalled();
    expect(mockObserver.next).not.toHaveBeenCalled();
    expect(callback).toHaveBeenCalledWith(expectedResult);
  });
});

describe('listAsync', () => {
  let mock: MockClientReadableStream<string>;
  beforeEach(() => {
    mock = new MockClientReadableStream<string>();
    (FileSystemClient as jest.Mock).mockImplementation(() => ({
      list: () => mock,
    }));
  });

  test('returns empty array when no data', async () => {
    const promise = listAsync(baseUrl);

    mock.end();

    const result = await promise;

    expect(result).toStrictEqual<string[]>([]);
  });

  test('throws when an error occurrs', async () => {
    const expectedError = new Error();

    const promise = listAsync(baseUrl);

    mock.error(expectedError);

    await expect(promise).rejects.toThrowError(expectedError);
  });

  test('returns result when data is available', async () => {
    const expectedResult = 'expected';

    const promise = listAsync(baseUrl);

    mock.data(expectedResult);
    mock.end();

    const result = await promise;

    expect(result).toContain(expectedResult);
  });

  test('returns all data as array', async () => {
    const expectedResult = [
      'first', 'second',
    ];

    const promise = listAsync(baseUrl);

    mock.data(expectedResult[0]);
    mock.data(expectedResult[1]);
    mock.end();

    const result = await promise;

    expect(result).toStrictEqual(expectedResult);
  });
});

class MockClientReadableStream<T> {

  data: (response: T) => void = _ => { };
  metadata: (metadata: Metadata) => void = _ => { };
  status: (status: Status) => void = _ => { };
  error: (error: Error) => void = _ => { };
  end: () => void = () => { };

  on(eventType: 'data', callback: (response: T) => void): void;
  on(eventType: 'metadata', callback: (metadata: Metadata) => void): void;
  on(eventType: 'status', callback: (status: Status) => void): void;
  on(eventType: 'error', callback: (error: Error) => void): void;
  on(eventType: 'end', callback: () => void): void;
  on(eventType: string, callback: (r?: any) => void) {
    switch (eventType) {
      case 'data':
        this.data = callback;
        break;
      case 'metadata':
        this.metadata = callback;
        break;
      case 'status':
        this.status = callback;
        break;
      case 'error':
        this.error = callback;
        break;
      case 'end':
        this.end = callback;
        break;
    }
  }

}
