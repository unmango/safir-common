import { isMetadata } from './helpers';

describe('isMetadata', () => {
  describe('when primitive type', () => {
    test('number returns false', () => {
      const result = isMetadata(2);

      expect(result).toBeFalsy();
    });

    // Can't test BigInt targeting lower than ES2020
    // test('bigint', () => {
    //   const result = isMetadata(69n);

    //   expect(result).toBeFalsy();
    // });

    test('string returns false', () => {
      const result = isMetadata('test');

      expect(result).toBeFalsy();
    });

    test('boolean returns false', () => {
      const result = isMetadata(true);

      expect(result).toBeFalsy();
    });

    test('symbol returns false', () => {
      const result = isMetadata(Symbol('test'));

      expect(result).toBeFalsy();
    });

    test('undefined returns false', () => {
      const result = isMetadata(undefined);

      expect(result).toBeFalsy();
    });

    test('null returns false', () => {
      const result = isMetadata(null);

      expect(result).toBeFalsy();
    });
  });

  describe('when record type', () => {
    test('returns true', () => {
      const result = isMetadata({ thing: 'value' });

      expect(result).toBeTruthy();
    });
  });
});
