import { agent } from '@safir/protos';
import { firstValueFrom } from 'rxjs';
import { list } from './fileSystem';

// jest.mock('grpc-web');
// jest.mock('@safir/protos/dist/agent');

const baseUrl = 'testUrl';

describe('list', () => {
  test('returns empty array when no data', async () => {
    const observable = list(baseUrl);
  });
});
