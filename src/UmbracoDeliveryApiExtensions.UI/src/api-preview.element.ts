import { LitElement, PropertyValueMap, css, html } from 'lit'
import { customElement, property, state } from 'lit/decorators.js'
import { AngularElement } from './angular.element'

/**
 * The Delivery Api Extensions Preview element.
 */
@customElement('api-preview')
export class ApiPreviewElement extends AngularElement(LitElement) {
  @property({ type: String })
  apiPath = ""

  @property({ type: String })
  culture = ""

  @state()
  private _data = ""

  @state()
  private _published = false

  render() {
    return html`
      <uui-box>
        <uui-tab-group slot="headline">
          <uui-tab active="true" @click=${() => this._onStatusChanged(false)}>Saved</uui-tab>
          <uui-tab @click=${() => this._onStatusChanged(true)}>Published</uui-tab>
        </uui-tab-group>
        <uui-scroll-container>
          <pre>${this._data}</pre>
        </uui-scroll-container>
      </uui-box>
      `
  }

  _onStatusChanged(published: boolean) {
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
      headers: {}
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
    let responseData = await response.json();
    this._data = JSON.stringify(responseData, null, 4);
    this.requestUpdate();
  }

  static styles = css`
  `
}

declare global {
  interface HTMLElementTagNameMap {
    'api-preview': ApiPreviewElement
  }
}
