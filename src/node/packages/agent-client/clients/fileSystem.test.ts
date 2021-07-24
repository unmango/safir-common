import { firstValueFrom } from 'rxjs';
import { agent } from '@safir/protos';
import { FileSystemClient } from '@safir/protos/dist/agent';
import { list } from './fileSystem';

jest.mock('@safir/protos/dist/agent');

const baseUrl = 'testUrl';

describe('list', () => {
  test('returns empty array when no data', async () => {
    let data: (response: any) => void = _ => { };
    let error: (response: any) => void = _ => { };
    let end: (response: any) => void = _ => { };
    (FileSystemClient as jest.Mock).mockImplementation(() => ({
      list: () => ({
        on: (e: string, c: (r: any) => void) => {
          switch (e) {
            case 'data':
              data = c;
              break;
            case 'error':
              error = c;
              break;
            case 'end':
              end = c;
              break;
          }
        }
      })
    }));

    let result: string | null = null;
    let completed = false;

    const observable = list(baseUrl);
    observable.subscribe({
      next: x => result = x,
      complete: () => completed = true,
    });

    end({});

    expect(result).toBeNull();
    expect(completed).toBeTruthy();
  });
});

const mockClientStreamable = <T>(): { mock: ClientReadableStream<T>; } => {
  let data: (response: any) => void = _ => { };
  let error: (response: any) => void = _ => { };
  let end: (response: any) => void = _ => { };

  const mock = {
    on: (e: string, c: (r: any) => void) => {
      switch (e) {
        case 'data':
          data = c;
          break;
        case 'error':
          error = c;
          break;
        case 'end':
          end = c;
          break;
      }
    }
  };

  return {
    mock,
    dataCallback: data,
    errorCallback: error,
    endCallback: end,
  };
};
