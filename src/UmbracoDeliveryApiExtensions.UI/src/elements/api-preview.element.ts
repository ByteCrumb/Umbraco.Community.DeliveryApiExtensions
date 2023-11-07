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

    uui-box {
      display: grid;
      grid-template-rows: max-content minmax(150px, auto);
    }

    .centered {
      display:flex;
      justify-content:center;
      align-items: center;
      height: 100%;
    }

    .headline{
      display: flex;
      gap: 1rem;
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
  private _previewExpand = false;

  @state()
  private _publishedData: unknown = null;

  @state()
  private _publishedError = false;

  @state()
  private _publishedExpand = false;

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

    const togglePreviewExpand = () => {
      this._previewExpand = !this._previewExpand;
    };

    const togglePublishedExpand = () => {
      this._publishedExpand = !this._publishedExpand;
    };

    const renderPreview = (title: string, data: unknown, error: boolean, toggleExpand: () => void) => html`
      <uui-box>
          <div class="headline" slot="headline"><span>${title}</span><uui-toggle label="expand" @change=${toggleExpand}></uui-toggle></div>
          ${data ? html`
            <uui-scroll-container>
              <bc-json-preview .value=${data} display-object-size display-data-types shorten-text-after-length="50" ></bc-json-preview>
            </uui-scroll-container>
          ` : error ? renderError() : renderLoader()}
      </uui-box>
    `;

    return html`
      ${this.hasPreview ? renderPreview('Preview', this._previewData, this._previewError, togglePreviewExpand) : nothing}
      ${this.isPublished ? renderPreview('Published', this._publishedData, this._publishedError, togglePublishedExpand) : nothing}
    `;
  }

  protected updated(_changedProperties: PropertyValueMap<any> | Map<PropertyKey, unknown>): void {
    const sharedUpdateProperties = ['apiPath'];

    const updatePreview = [...sharedUpdateProperties, '_previewExpand'].some(p => _changedProperties.has(p));
    if (updatePreview) {
      void this.updatePreviewResponse();
    }

    const updatePublished = [...sharedUpdateProperties, 'isPublished', '_publishedExpand'].some(p => _changedProperties.has(p));
    if (updatePublished) {
      void this.updatePublishedResponse();
    }
  }

  private async updatePreviewResponse() {
    const params = this.getSharedApiParams();

    if (this.hasPreview) {
      params.headers.preview = 'true';

      ({response: this._previewData, error: this._previewError} = await this.fetchData(params, this._previewExpand));
    }
  }

  private async updatePublishedResponse() {
    const params = this.getSharedApiParams();

    ({response: this._publishedData, error: this._publishedError} = this.isPublished
      ? await this.fetchData(params, this._publishedExpand)
      : {response: null, error: false});
  }

  private async fetchData(params: RequestInit, expand: boolean) {
    try {
      const response = await fetch(this.apiPath + (expand ? '?expand=all' : ''), params);
      if (response.status === 200) {
        return {
          response: await this.parseJsonResponse(response),
          error: false,
        };
      }
    } catch (e) {
      console.log(e);
    }

    return {
      response: null,
      error: true,
    };
  }

  private getSharedApiParams() {
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

    return params;
  }
}

declare global {
  interface HTMLElementTagNameMap {
    'bc-api-preview': ApiPreviewElement;
  }
}
