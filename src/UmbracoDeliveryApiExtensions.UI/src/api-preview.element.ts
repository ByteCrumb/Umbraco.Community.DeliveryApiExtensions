import {LitElement, type PropertyValueMap, html} from 'lit';
import {customElement, property, state} from 'lit/decorators.js';
import {AngularElementMixin} from './angular.element';
import Cookies from 'js-cookie';

/**
 * The Delivery Api Extensions Preview element.
 */
@customElement('api-preview')
export class ApiPreviewElement extends AngularElementMixin(LitElement) {
  @property({type: String, attribute: 'api-path'})
    apiPath = '';

  @property({type: String})
    culture = '';

  @property({type: Boolean, attribute: 'is-published'})
    isPublished = false;

  @state()
  private _data = '';

  @state()
  private _published = false;

  render() {
    return html`
      <uui-box headline="Preview">
        <uui-tab-group>
          <uui-tab type="saved" active="true" @click=${this._onSavedSelected}>Saved</uui-tab>
          <uui-tab type="published" ?disabled=${!this.isPublished} @click=${this._onPublishedSelected}>Published</uui-tab>
        </uui-tab-group>
        <uui-scroll-container>
          <pre>${this._data}</pre>
        </uui-scroll-container>
      </uui-box>
      `;
  }

  protected updated(_changedProperties: PropertyValueMap<any> | Map<PropertyKey, unknown>): void {
    if (_changedProperties.has('apiPath')) {
      void this.updateResponse();
    }
  }

  private _onSavedSelected(e: Event) {
    void this._onTabChanged(e, false);
  }

  private _onPublishedSelected(e: Event) {
    void this._onTabChanged(e, true);
  }

  private async _onTabChanged(e: Event, published: boolean) {
    if (this._published === published) {
      e.preventDefault();
      e.stopImmediatePropagation();
      return;
    }

    this._published = published;
    await this.updateResponse();
  }

  private async updateResponse() {
    const params: RequestInit = {
      method: 'GET',
      headers: {
        'x-umb-xsrf-token': this._getXsrfToken(),
      },
      credentials: 'include',
    };
    if (this.culture) {
      params.headers = {
        ...params.headers,
        'Accept-Language': this.culture,
      };
    }

    if (!this._published) {
      params.headers = {
        ...params.headers,
        preview: 'true',
      };
    }

    const response = await fetch(this.apiPath, params);
    const responseData = await this._parseAngularResponse(response);
    this._data = responseData;
    this.requestUpdate();
  }

  private _getXsrfToken(): string {
    return Cookies.get('UMB-XSRF-TOKEN') ?? '';
  }

  private async _parseAngularResponse(response: Response) {
    let responseBodyText = await response.text();
    if (responseBodyText.startsWith(')]}\',\n')) {
      responseBodyText = responseBodyText.substring(6);
    }

    return responseBodyText;
  }
}

declare global {
  interface HTMLElementTagNameMap {
    'api-preview': ApiPreviewElement;
  }
}
