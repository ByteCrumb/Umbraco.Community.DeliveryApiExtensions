import type {R2WCOptions} from '@r2wc/core';
import r2wc from '@r2wc/react-to-web-component';
import {defineElement} from '@umbraco-ui/uui';

declare global {
  namespace JSX {
    // IntrinsicElementMap grabs all the standard HTML tags in the TS DOM lib.
    interface IntrinsicElements extends IntrinsicElementMap { }

    // The following are custom types, not part of TS's known JSX namespace:
    type IntrinsicElementMap = {
      [K in keyof HTMLElementTagNameMap]: HTMLElementTagNameMap[K] | React.DetailedHTMLProps<React.HTMLAttributes<HTMLElementTagNameMap[K]>, HTMLElementTagNameMap[K]>
    };
  }
}

export default function defineReactElement<Props extends Record<string, unknown>>(name: string, ReactComponent: React.ComponentType<Props>, options?: R2WCOptions<Props>): void {
  options ??= {};
  options.shadow ??= 'open';

  const ReactWebComponentElement = r2wc(ReactComponent, options);
  defineElement(name)(ReactWebComponentElement);
}
