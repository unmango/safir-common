import { firstValueFrom } from 'rxjs';
import { list } from './list';

test('calls agent', async () => {
  const result$ = list('localhost:6969');
  const result = await firstValueFrom(result$)
  console.log(result);
});
