import { Metadata, Status } from 'grpc-web';
import { Subject } from 'rxjs';
import {
  isMetadata,
  isStatus,
  MetadataCallback,
  responseCallbacks,
  StatusCallback,
} from './helpers';

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
  describe('when metadata callback is provided', () => {
    test('and called with number should not invoke callback', () => {
      const subject = new Subject<number>();
      const callback: MetadataCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ metadata: callback }));

      obs.subscribe();
      subject.next(69);
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with string should not invoke callback', () => {
      const subject = new Subject<string>();
      const callback: MetadataCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ metadata: callback }));

      obs.subscribe();
      subject.next('test');
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with boolean should not invoke callback', () => {
      const subject = new Subject<boolean>();
      const callback: MetadataCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ metadata: callback }));

      obs.subscribe();
      subject.next(true);
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with null should not invoke callback', () => {
      const subject = new Subject<null>();
      const callback: MetadataCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ metadata: callback }));

      obs.subscribe();
      subject.next(null);
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with undefined should not invoke callback', () => {
      const subject = new Subject<undefined>();
      const callback: MetadataCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ metadata: callback }));

      obs.subscribe();
      subject.next(undefined);
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with symbol should not invoke callback', () => {
      const subject = new Subject<Symbol>();
      const callback: MetadataCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ metadata: callback }));

      obs.subscribe();
      subject.next(Symbol('test'));
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with an object that has a number property should not invoke callback', () => {
      const subject = new Subject<Record<string, number>>();
      const callback: MetadataCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ metadata: callback }));

      obs.subscribe();
      subject.next({ test: 69 });
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with an object that has a boolean property should not invoke callback', () => {
      const subject = new Subject<Record<string, boolean>>();
      const callback: MetadataCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ metadata: callback }));

      obs.subscribe();
      subject.next({ test: true });
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with an object that has a null property should not invoke callback', () => {
      const subject = new Subject<Record<string, null>>();
      const callback: MetadataCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ metadata: callback }));

      obs.subscribe();
      subject.next({ test: null });
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with an object that has a undefined property should not invoke callback', () => {
      const subject = new Subject<Record<string, undefined>>();
      const callback: MetadataCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ metadata: callback }));

      obs.subscribe();
      subject.next({ test: undefined });
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with an object that has a symbol property should not invoke callback', () => {
      const subject = new Subject<Record<string, Symbol>>();
      const callback: MetadataCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ metadata: callback }));

      obs.subscribe();
      subject.next({ test: Symbol('test') });
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with a metadata object should invoke callback', () => {
      const subject = new Subject<Metadata>();
      const callback: MetadataCallback = jest.fn();
      const expected: Metadata = {
        test: 'test',
      };

      const obs = subject.pipe(responseCallbacks({ metadata: callback }));

      obs.subscribe();
      subject.next(expected);
      expect(callback).toHaveBeenCalledTimes(1);
      expect(callback).toHaveBeenCalledWith(expected);
    });
  });

  describe('when status callback is provided', () => {
    test('and called with number should not invoke callback', () => {
      const subject = new Subject<number>();
      const callback: StatusCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ status: callback }));

      obs.subscribe();
      subject.next(69);
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with string should not invoke callback', () => {
      const subject = new Subject<string>();
      const callback: StatusCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ status: callback }));

      obs.subscribe();
      subject.next('test');
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with boolean should not invoke callback', () => {
      const subject = new Subject<boolean>();
      const callback: StatusCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ status: callback }));

      obs.subscribe();
      subject.next(true);
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with null should not invoke callback', () => {
      const subject = new Subject<null>();
      const callback: StatusCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ status: callback }));

      obs.subscribe();
      subject.next(null);
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with undefined should not invoke callback', () => {
      const subject = new Subject<undefined>();
      const callback: StatusCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ status: callback }));

      obs.subscribe();
      subject.next(undefined);
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with symbol should not invoke callback', () => {
      const subject = new Subject<Symbol>();
      const callback: StatusCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ status: callback }));

      obs.subscribe();
      subject.next(Symbol('test'));
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with an object that has a number property should not invoke callback', () => {
      const subject = new Subject<Record<string, number>>();
      const callback: StatusCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ status: callback }));

      obs.subscribe();
      subject.next({ test: 69 });
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with an object that has a boolean property should not invoke callback', () => {
      const subject = new Subject<Record<string, boolean>>();
      const callback: StatusCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ status: callback }));

      obs.subscribe();
      subject.next({ test: true });
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with an object that has a null property should not invoke callback', () => {
      const subject = new Subject<Record<string, null>>();
      const callback: StatusCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ status: callback }));

      obs.subscribe();
      subject.next({ test: null });
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with an object that has a undefined property should not invoke callback', () => {
      const subject = new Subject<Record<string, undefined>>();
      const callback: StatusCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ status: callback }));

      obs.subscribe();
      subject.next({ test: undefined });
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with an object that has a symbol property should not invoke callback', () => {
      const subject = new Subject<Record<string, Symbol>>();
      const callback: StatusCallback = jest.fn();

      const obs = subject.pipe(responseCallbacks({ status: callback }));

      obs.subscribe();
      subject.next({ test: Symbol('test') });
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with an object that has a string property should not invoke callback', () => {
      const subject = new Subject<Record<string, string>>();
      const callback: StatusCallback = jest.fn();
      const expected: Record<string, string> = {
        test: 'test',
      };

      const obs = subject.pipe(responseCallbacks({ status: callback }));

      obs.subscribe();
      subject.next(expected);
      expect(callback).toHaveBeenCalledTimes(0);
    });

    test('and called with a status should invoke callback', () => {
      const subject = new Subject<Status>();
      const callback: StatusCallback = jest.fn();
      const expected: Status = {
        code: 69,
        details: 'details',
      };

      const obs = subject.pipe(responseCallbacks({ status: callback }));

      obs.subscribe();
      subject.next(expected);
      expect(callback).toHaveBeenCalledTimes(1);
      expect(callback).toHaveBeenCalledWith(expected);
    });

    test('and called with a status with metadata should invoke callback', () => {
      const subject = new Subject<Status>();
      const callback: StatusCallback = jest.fn();
      const expected: Status = {
        code: 69,
        details: 'details',
        metadata: {
          test: 'test',
        },
      };

      const obs = subject.pipe(responseCallbacks({ status: callback }));

      obs.subscribe();
      subject.next(expected);
      expect(callback).toHaveBeenCalledTimes(1);
      expect(callback).toHaveBeenCalledWith(expected);
    });
  });
});
