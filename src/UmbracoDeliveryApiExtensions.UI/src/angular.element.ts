import {type LitElement, type PropertyValueMap} from 'lit';

type Constructor<T = Record<string, unknown>> = new (...args: any[]) => T;

export const AngularElementMixin = <T extends Constructor<LitElement>>(superClass: T) => {
  class AngularElementMixin extends superClass {
    private _angularReady = false;

    protected shouldUpdate(_changedProperties: PropertyValueMap<any> | Map<PropertyKey, unknown>): boolean {
      if (!this._angularReady) {
        this._angularReady = !(window.angular) || Boolean(window.angular.element(this)?.scope());
      }

      return this._angularReady && super.shouldUpdate(_changedProperties);
    }
  }

  return AngularElementMixin as T;
};
