import Cookies from 'js-cookie';
import {type LitElement, type PropertyValueMap} from 'lit';

type Constructor<T = unknown> = new (...args: any[]) => T;

export declare class AngularElementMixinInterface {
  protected getCsrfToken(): string;
  protected parseJsonResponse(response: Response): Promise<unknown>;
}

export const AngularElementMixin = <T extends Constructor<LitElement>>(superClass: T) => {
  class AngularElementMixinClass extends superClass {
    private _angularReady = false;

    protected shouldUpdate(_changedProperties: PropertyValueMap<any> | Map<PropertyKey, unknown>): boolean {
      if (!this._angularReady) {
        this._angularReady = !(window.angular) || Boolean(window.angular.element(this)?.scope());
      }

      return this._angularReady && super.shouldUpdate(_changedProperties);
    }

    protected getCsrfToken(): string {
      return Cookies.get('UMB-XSRF-TOKEN') ?? '';
    }

    protected async parseJsonResponse(response: Response): Promise<unknown> {
      let responseBodyText = await response.text();
      if (responseBodyText.startsWith(')]}\',')) {
        responseBodyText = responseBodyText.substring(5);
      }

      return JSON.parse(responseBodyText) as unknown;
    }
  }

  return AngularElementMixinClass as unknown as Constructor<AngularElementMixinInterface> & T;
};
