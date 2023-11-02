import {LitElement, type PropertyValueMap, css, html} from 'lit';
import {customElement, property, state} from 'lit/decorators.js';
import {AngularElementMixin} from './angular.element';
import Cookies from 'js-cookie';

/**
 * The Delivery Api Extensions Preview element.
 */
@customElement('api-preview')
export class ApiPreviewElement extends AngularElementMixin(LitElement) {
  static styles = css`
    :host {
        display: flex;
        flex-direction: column;
        gap: 1rem;
    }
    :host > * {
      flex: 1;
      min-width: 0;
      min-height: 0;
      overflow: auto;
    }
    @media (min-width: 1024px) {
      :host {
        flex-direction: row;
      }
     }
  `;

  @property({type: String, attribute: 'api-path'})
    apiPath = '';

  @property({type: String})
    culture = '';

  @property({type: Boolean, attribute: 'is-published'})
    isPublished = false;

  @state()
  private _previewData = '';

  @state()
  private _publishedData = '';

  render() {
    return html`
      <uui-box headline="Preview">
        <uui-scroll-container>
          <pre>${this._previewData}</pre>
        </uui-scroll-container>
      </uui-box>

      ${this.isPublished ? html`
        <uui-box headline="Published">
          <uui-scroll-container>
            <pre>${this._publishedData}</pre>
          </uui-scroll-container>
        </uui-box>
      ` : undefined}
      `;
  }

  protected updated(_changedProperties: PropertyValueMap<any> | Map<PropertyKey, unknown>): void {
    if (_changedProperties.has('apiPath')) {
      void this.updateResponse();
    }
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

    if (this.isPublished) {
      const publishedResponse = await fetch(this.apiPath, params);
      this._publishedData = await this._parseAngularResponse(publishedResponse);
    }

    params.headers = {
      ...params.headers,
      preview: 'true',
    };

    const previewResponse = await fetch(this.apiPath, params);
    this._previewData = await this._parseAngularResponse(previewResponse);

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
