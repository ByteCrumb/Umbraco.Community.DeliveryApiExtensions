import {defineElement} from '@umbraco-ui/uui';
import {css, html, LitElement, nothing, type PropertyValueMap} from 'lit';
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

    .centered {
      display:flex;
      justify-content:center;
      align-items: center;
      min-height: 150px;
    }
  `;

  @property({type: String})
    apiPath = '';

  @property({type: String})
    culture = '';

  @property({type: Boolean})
    isPublished = false;

  @property({type: Boolean})
    hasPreview = false;

  @state()
  private _previewData: unknown = null;

  @state()
  private _previewError = false;

  @state()
  private _publishedData: unknown = null;

  @state()
  private _publishedError = false;

  render() {
    const renderLoader = () => html`
      <div class="centered">
        <uui-loader></uui-loader>
      </div>
    `;

    const renderError = () => html`
      <div class="centered">
        <span>❌ Error!</span>
      </div>
    `;

    const renderPreview = (title: string, data: unknown, error: boolean) => html`
      <uui-box headline="${title}">
          ${data ? html`
            <uui-scroll-container>
              <bc-json-preview .value=${data} display-object-size display-data-types shorten-text-after-length="50" ></bc-json-preview>
            </uui-scroll-container>
          ` : error ? renderError() : renderLoader()}
      </uui-box>
    `;

    return html`
      ${this.hasPreview ? renderPreview('Preview', this._previewData, this._previewError) : nothing}
      ${this.isPublished ? renderPreview('Published', this._publishedData, this._publishedError) : nothing}
    `;
  }

  protected updated(_changedProperties: PropertyValueMap<any> | Map<PropertyKey, unknown>): void {
    if (_changedProperties.has('apiPath') || _changedProperties.has('isPublished')) {
      void this.updateResponse();
    }
  }

  private async updateResponse() {
    const params: RequestInit & {headers: Record<string, string>} = {
      method: 'GET',
      headers: {
        'x-umb-xsrf-token': this.getXsrfToken(),
      },
      credentials: 'include',
    };

    if (this.culture) {
      params.headers['Accept-Language'] = this.culture;
    }

    ({response: this._publishedData, error: this._publishedError} = this.isPublished
      ? await this.fetchData(params)
      : {response: null, error: false});

    if (this.hasPreview) {
      params.headers.preview = 'true';

      ({response: this._previewData, error: this._previewError} = await this.fetchData(params));
    }
  }

  private async fetchData(params: RequestInit) {
    try {
      const response = await fetch(this.apiPath, params);
      if (response.status === 200) {
        return {
          response: await this.parseJsonResponse(response),
          error: false,
        };
      }
    } catch (e) { }

    return {
      response: null,
      error: true,
    };
  }
}

declare global {
  interface HTMLElementTagNameMap {
    'bc-api-preview': ApiPreviewElement;
  }
}
