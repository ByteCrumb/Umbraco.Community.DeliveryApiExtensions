import { LitElement, PropertyValueMap, css, html } from 'lit'
import { customElement, property, state } from 'lit/decorators.js'
import { AngularElement } from './angular.element'
import Cookies from 'js-cookie'

/**
 * The Delivery Api Extensions Preview element.
 */
@customElement('api-preview')
export class ApiPreviewElement extends AngularElement(LitElement) {
  @property({ type: String, attribute: 'api-path' })
  apiPath = ""

  @property({ type: String })
  culture = ""

  @property({ type: Boolean, attribute: 'is-published' })
  isPublished = false

  @state()
  private _data = ""

  @state()
  private _published = false

  render() {
    return html`
      <uui-box headline="Preview">
        <uui-tab-group>
          <uui-tab active="true" @click=${(e: Event) => this._onStatusChanged(e, false)}>Saved</uui-tab>
          <uui-tab ?disabled=${!this.isPublished} @click=${(e: Event) => this._onStatusChanged(e, true)}>Published</uui-tab>
        </uui-tab-group>
        <uui-scroll-container>
          <pre>${this._data}</pre>
        </uui-scroll-container>
      </uui-box>
      `
  }

  _onStatusChanged(e: Event, published: boolean) {
    if(this._published == published){
      e.preventDefault();
      e.stopImmediatePropagation();
      return;
    }
    this._published = published;
    this.updateResponse();
  }

  protected updated(_changedProperties: PropertyValueMap<any> | Map<PropertyKey, unknown>): void {
    if(_changedProperties.has('apiPath')){
      this.updateResponse();
    }
  }

  private async updateResponse() {
    let params: RequestInit = {
      method: 'GET',
      headers: {
        'x-umb-xsrf-token': this._getXsrfToken()
      },
      credentials: 'include'
    };
    if (this.culture) {
      params.headers = {
        ...params.headers,
        'Accept-Language': this.culture
      };
    }
    if (!this._published) {
      params.headers = {
        ...params.headers,
        'Preview': 'true'
      };
    }

    const response = await fetch(this.apiPath, params);
    let responseData = await this._parseAngularResponse(response);
    this._data = responseData;
    this.requestUpdate();
  }

  _getXsrfToken(): string{
    return Cookies.get("UMB-XSRF-TOKEN") || "";
  }

  async _parseAngularResponse(response : Response) {
    let responseBodyText = await response.text();
    if(responseBodyText.startsWith(")]}',\n")){
      responseBodyText = responseBodyText.substring(6)
    }
    return responseBodyText;
  }

  static styles = css`
  `
}

declare global {
  interface HTMLElementTagNameMap {
    'api-preview': ApiPreviewElement
  }
}
