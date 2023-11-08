import type {R2WCOptions} from '@r2wc/core';
import r2wc from '@r2wc/react-to-web-component';
import {defineElement} from '@umbraco-ui/uui';

export interface Options<Props> extends Omit<R2WCOptions<Props>, 'props'> {
  props?: Partial<R2WCOptions<Props>['props']>;
}

export default function defineReactElement<Props extends Record<string, unknown>>(name: string, ReactComponent: React.ComponentType<Props>, options?: Options<Props>): void {
  options ??= {};
  options.shadow ??= 'open';

  const ReactWebComponentElement = r2wc(ReactComponent, options as R2WCOptions<Props>);
  defineElement(name)(ReactWebComponentElement);
}
