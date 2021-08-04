global.XMLHttpRequest = require('xhr2');

import { FileSystemClient } from '@unmango/safir-protos/dist/agent';
import { Empty } from 'google-protobuf/google/protobuf/empty_pb';
import { firstValueFrom, timeout } from 'rxjs';
import { list } from './list';

test('calls agent', async () => {
  const client = new FileSystemClient('localhost:6969');

  const stream = client.list(new Empty());
  stream.on('data', x => console.log(x));
  stream.on('error', e => console.error(e));
  stream.on('end', () => console.log('end'));

  await new Promise(res => {
    setTimeout(res, 20000);
  });

  // const result$ = list('localhost:6969');
  // const result = await firstValueFrom(result$.pipe(
  //   timeout({ each: 60000 }),
  // ));
  // console.log(result);
}, 60000);
