import { LitElement, PropertyValueMap} from 'lit';

type Constructor<T = {}> = new (...args: any[]) => T;

export const AngularElement = <T extends Constructor<LitElement>>(superClass: T) => {
  class AngularElement extends superClass {
    private _angularReady = false;

    protected shouldUpdate(_changedProperties: PropertyValueMap<any> | Map<PropertyKey, unknown>): boolean {
      if (!this._angularReady) {
        const angular = (window as any)?.angular;
        this._angularReady = !(angular) || !!(angular.element(this)?.scope());
      }

      return this._angularReady && super.shouldUpdate(_changedProperties);
    }
  };

  return AngularElement as T;
}
