import {Task} from '@lit/task';
import {defineElement} from '@umbraco-ui/uui';
import {css, html, LitElement} from 'lit';
import {property, query, state} from 'lit/decorators.js';
import {ifDefined} from 'lit/directives/if-defined.js';

import {AngularElementMixin} from '../mixins/angular-element.mixin';
import {KebabCaseAttributesMixin} from '../mixins/kebab-case-attributes.mixin';

/**
 * The Delivery Api Extensions Preview Tab element.
 */
@defineElement('bc-api-preview-section')
export class ApiPreviewElementSection extends AngularElementMixin(KebabCaseAttributesMixin(LitElement)) {
  static styles = css`
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
      justify-content: space-between;
      gap: 1rem;
    }
  `;

  @property({type: String})
    title = '';

  @property({type: String})
    apiPath = '';

  @property({type: String})
    culture = '';

  @property({type: Boolean})
    preview = false;

  @query('uui-scroll-container')
    scrollContainer?: HTMLElement;

  @state()
  private _expand = false;

  private readonly _dataTask = new Task(this, {
    task: async ([culture, preview, expand], {signal}) => this._fetchData(culture, preview, expand, signal),
    args: (): [string, boolean, boolean] => [this.culture, this.preview, this._expand],
  });

  render() {
    const renderLoader = (minHeight?: number) => html`
      <div class="centered" style="min-height: ${ifDefined(minHeight)}px">
        <uui-loader></uui-loader>
      </div>
    `;

    const toggleExpand = () => {
      this._expand = !this._expand;
    };

    return html`
      <uui-box>
        <div class="headline" slot="headline">
          <span>${this.title}</span>
          <uui-toggle label="Expand" title=${this._expand ? 'all' : 'none'} label-position="left" @change=${toggleExpand}></uui-toggle>
        </div>
        <uui-scroll-container>
          ${this._dataTask.render({
    initial: () => renderLoader(),
    pending: () => renderLoader(this.scrollContainer?.scrollHeight),
    complete: data => html`
      <bc-json-preview .value=${data}></bc-json-preview>
    `,
    error: error => html`
    <div class="centered">
        <span>❌ Error: ${error}!</span>
      </div>
    `,
  })}
        </uui-scroll-container>
      </uui-box>
    `;
  }

  private async _fetchData(culture: string, preview: boolean, expand: boolean, signal: AbortSignal): Promise<unknown> {
    const params: RequestInit & {headers: Record<string, string>} = {
      method: 'GET',
      headers: {
        'x-umb-xsrf-token': this.getCsrfToken(),
      },
      credentials: 'include',
      signal,
    };

    if (culture) {
      params.headers['Accept-Language'] = culture;
    }

    if (preview) {
      params.headers.preview = 'true';
    }

    const response = await fetch(`${this.apiPath}${(expand ? '?expand=all' : '')}`, params);
    if (!response.ok) {
      throw new Error(response.statusText);
    }

    return this.parseJsonResponse(response);
  }
}

declare global {
  interface HTMLElementTagNameMap {
    'bc-api-preview-section': ApiPreviewElementSection;
  }
}
