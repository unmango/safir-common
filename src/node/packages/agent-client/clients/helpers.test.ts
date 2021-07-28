import { Metadata } from 'grpc-web';
import { Subject } from 'rxjs';
import { isMetadata, isStatus, responseCallbacks } from './helpers';

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

  // Maybe not entirely correct, but good enough for now
  // Delete this comment in like, a couple months idk
  describe('when record type', () => {
    test('with string prop returns true', () => {
      const result = isMetadata({ thing: 'value' });

      expect(result).toBeTruthy();
    });

    test('with number prop returns false', () => {
      const result = isMetadata({ thing: 69 });

      expect(result).toBeFalsy();
    });

    test('with boolean prop returns false', () => {
      const result = isMetadata({ thing: true });

      expect(result).toBeFalsy();
    });

    test('with symbol prop returns false', () => {
      const result = isMetadata({ thing: Symbol('test') });

      expect(result).toBeFalsy();
    });

    test('with undefined prop returns false', () => {
      const result = isMetadata({ thing: undefined });

      expect(result).toBeFalsy();
    });

    test('with null prop returns false', () => {
      const result = isMetadata({ thing: null });

      expect(result).toBeFalsy();
    });
  });

  describe('when isStatus', () => {
    test('returns false', () => {
      const result = isMetadata({ code: 69 });

      expect(result).toBeFalsy();
    });
  });
});

describe('isStatus', () => {
  describe('when primitive', () => {
    test('boolean returns false', () => {
      const result = isStatus(true);

      expect(result).toBeFalsy();
    });

    test('number returns false', () => {
      const result = isStatus(69);

      expect(result).toBeFalsy();
    });

    test('string returns false', () => {
      const result = isStatus('test');

      expect(result).toBeFalsy();
    });

    test('undefined returns false', () => {
      const result = isStatus(undefined);

      expect(result).toBeFalsy();
    });

    test('null returns false', () => {
      const result = isStatus(null);

      expect(result).toBeFalsy();
    });
  });

  describe('when object', () => {
    test('and `code` and `details` properties should be true', () => {
      const result = isStatus({ code: 69, details: 'test' });

      expect(result).toBeTruthy();
    });

    test('and `code` and `details` and `metadata` properties should be true', () => {
      const result = isStatus({
        code: 69,
        details: 'test',
        metadata: {
          thing: 'test',
        },
      });

      expect(result).toBeTruthy();
    });
  });
});

describe('responseCallbacks', () => {
  describe('when metadata', () => {
    test('number', () => {
      const subject = new Subject<number>();
      let flag = false;
      const callback = (_: Metadata) => flag = true;

      const obs = subject.pipe(responseCallbacks(callback));

      obs.subscribe();
      subject.next(69);
      expect(flag).toBeTruthy();
    });
  });
});
