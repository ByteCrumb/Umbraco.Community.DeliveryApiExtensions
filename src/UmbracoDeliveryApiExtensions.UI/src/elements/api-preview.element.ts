import {defineElement} from '@umbraco-ui/uui';
import {css, html, LitElement, type PropertyValueMap} from 'lit';
import {property, state} from 'lit/decorators.js';

import {AngularElementMixin} from '../mixins/angular-element.mixin';
import {KebabCaseAttributesMixin} from '../mixins/kebab-case-attributes.mixin';

/**
 * The Delivery Api Extensions Preview element.
 */
@defineElement('bc-api-preview')
export class ApiPreviewElement extends AngularElementMixin(KebabCaseAttributesMixin(LitElement)) {
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

  @property({type: String})
    apiPath = '';

  @property({type: String})
    culture = '';

  @property({type: Boolean})
    isPublished = false;

  @state()
  private _previewData: unknown = {};

  @state()
  private _publishedData: unknown = {};

  render() {
    return html`
      <uui-box headline="Preview">
        <uui-scroll-container>
        <bc-json-preview display-object-size="false" display-data-types="false" shorten-text-after-length="50" .value=${this._previewData}></bc-json-preview>
        </uui-scroll-container>
      </uui-box>

      ${this.isPublished ? html`
        <uui-box headline="Published">
          <uui-scroll-container>
            <bc-json-preview display-object-size="false" display-data-types="false" shorten-text-after-length="50" .value=${this._publishedData}></bc-json-preview>
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
        'x-umb-xsrf-token': this.getCsrfToken(),
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
      this._publishedData = await this.parseJsonResponse(publishedResponse);
    }

    params.headers = {
      ...params.headers,
      preview: 'true',
    };

    const previewResponse = await fetch(this.apiPath, params);
    this._previewData = await this.parseJsonResponse(previewResponse);
  }
}

declare global {
  interface HTMLElementTagNameMap {
    'api-preview': ApiPreviewElement;
  }
}
