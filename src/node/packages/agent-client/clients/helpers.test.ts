import { isMetadata } from './helpers';

describe('isMetadata', () => {
  describe('when primitive data returns false', () => {
    test('number', () => {
      const result = isMetadata(2);

      expect(result).toBeFalsy();
    });

    // Can't test BigInt targeting lower than ES2020
    // test('bigint', () => {
    //   const result = isMetadata(69n);

    //   expect(result).toBeFalsy();
    // });

    test('string', () => {
      const result = isMetadata('test');

      expect(result).toBeFalsy();
    });

    test('boolean', () => {
      const result = isMetadata(true);

      expect(result).toBeFalsy();
    });

    test('symbol', () => {
      const result = isMetadata(Symbol('test'));

      expect(result).toBeFalsy();
    });

    test('undefined', () => {
      const result = isMetadata(undefined);

      expect(result).toBeFalsy();
    });

    test('null', () => {
      const result = isMetadata(null);

      expect(result).toBeFalsy();
    });
  });
});
