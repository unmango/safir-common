import { FileSystemClient } from '@unmango/safir-protos/dist/agent';
import { Observer } from 'rxjs';
import { GrpcResponse } from '../grpcResponse';
import { createClient, list, listAsync } from './fileSystem';

jest.mock('@unmango/safir-protos/dist/agent');

const baseUrl = 'testUrl';

describe('createClient', () => {
  let mock: MockClientReadableStream<string>;
  beforeEach(() => {
    mock = new MockClientReadableStream<string>();
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
});

describe('list', () => {
  let mockStream: MockClientReadableStream<string>;
  let mockObserver: Observer<GrpcResponse<string>>;
  beforeEach(() => {
    mockStream = new MockClientReadableStream<string>();
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
  error: (error: Error) => void = _ => { };
  end: () => void = () => { };

  on(eventType: 'data', callback: (response: T) => void): void;
  on(eventType: 'error', callback: (error: Error) => void): void;
  on(eventType: 'end', callback: () => void): void;
  on(eventType: string, callback: (r?: any) => void) {
    switch (eventType) {
      case 'data':
        this.data = callback;
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
