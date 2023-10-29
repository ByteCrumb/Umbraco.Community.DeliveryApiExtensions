import { LitElement, PropertyValueMap, css, html } from 'lit'
import { customElement, property, state } from 'lit/decorators.js'
import { AngularElement } from './angular.element'

/* import litLogo from './assets/lit.svg'
import viteLogo from '/vite.svg' */

/**
 * The Delivery Api Extensions Preview element.
 *
 * @slot - This element has a slot
 * @csspart button - The button
 */
@customElement('api-preview')
export class ApiPreview extends AngularElement(LitElement) {
  /**
   * The number of times the button has been clicked.
   */
  @property({ type: Number })
  count = 0

  @property({ type: String })
  apiPath = ""

  @state()
  private _data = ""

  render() {
    return html`
      <uui-box>
        <uui-tab-group slot="headline">
          <uui-tab active="true">Saved</uui-tab>
          <uui-tab>Published</uui-tab>
        </uui-tab-group>
        <uui-scroll-container>
          <pre>${this._data}</pre>
        </uui-scroll-container>
      </uui-box>
      `
  }

  protected updated(_changedProperties: PropertyValueMap<any> | Map<PropertyKey, unknown>): void {
    if(_changedProperties.has('apiPath')){
      this.updateResponse();
    }
  }

  private async updateResponse() {
    console.log(this.apiPath)
    const response = await fetch(this.apiPath);
    console.log(this.apiPath)
    let responseData = await response.json();
    this._data = JSON.stringify(responseData, null, 4);
    this.requestUpdate();
  }

  static styles = css`
  `
}

declare global {
  interface HTMLElementTagNameMap {
    'api-preview': ApiPreview
  }
}
