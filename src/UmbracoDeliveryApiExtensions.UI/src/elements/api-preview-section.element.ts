import {Task} from '@lit/task';
import {css, html, LitElement} from 'lit';
import {
  customElement, property, query, state,
} from 'lit/decorators.js';
import {cache} from 'lit/directives/cache.js';
import {ifDefined} from 'lit/directives/if-defined.js';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';

import {API_PREVIEW_CONTEXT} from '../contexts/api-preview.context';
import {KebabCaseAttributesMixin} from '../mixins/kebab-case-attributes.mixin';
import { ApiPreviewContentChangedEvent } from '../events/api-preview-content-changed';

export * from './json-preview.element';

/**
 * The Delivery Api Extensions Preview Tab element.
 */
@customElement('bc-api-preview-section')
export class ApiPreviewElementSection extends UmbElementMixin(KebabCaseAttributesMixin(LitElement)) {
  static styles = css`
    :host {
      display: flex;
    }

    uui-box {
      flex: 1;
      overflow: auto;
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

  #context?: typeof API_PREVIEW_CONTEXT.TYPE;

  @property({type: String})
    headline = '';

  @property({type: Boolean})
    preview = false;

  @query('bc-json-preview')
    jsonPreviewElement?: HTMLElement;

  @state()
  private _expand = false;

  private readonly _dataTask = new Task(this, {
    task: async ([preview, expand], {signal}) => this.#context?.fetchData(preview, expand, signal),
    args: (): [boolean, boolean] => [this.preview, this._expand],
  });

  constructor(){
    super();

    this.consumeContext(API_PREVIEW_CONTEXT, (context) => {
      this.#context = context;

      this.#context?.removeEventListener(
				ApiPreviewContentChangedEvent.TYPE,
				this.#onContentChanged as EventListener,
			);

      this.#context?.addEventListener(
				ApiPreviewContentChangedEvent.TYPE,
				this.#onContentChanged as EventListener,
			);
    });
  }

  #onContentChanged = () => {
		this._dataTask.run();
	};

  render() {
    const renderLoader = (minHeight?: number) => html`
      <div class="centered" style="min-height: ${ifDefined(minHeight)}px">
        <uui-loader></uui-loader>
      </div>
    `;

    const toggleExpand = () => {
      this._expand = !this._expand;
    };

    const content = this._dataTask.render({
      initial: () => renderLoader(),
      pending: () => renderLoader(this.jsonPreviewElement?.offsetHeight),
      complete: data => html`
      <bc-json-preview .value=${data}></bc-json-preview>
    `,
      error: error => html`
    <div class="centered">
        <span>❌ Error: ${error && typeof error === 'object' && 'message' in error ? error.message : error}!</span>
      </div>
    `,
    });

    return html`
      <uui-box>
        <div class="headline" slot="headline">
          <span>${this.headline}</span>
          <uui-toggle label="Expand" title=${this._expand ? 'all' : 'none'} label-position="left" @change=${toggleExpand}></uui-toggle>
        </div>
        ${cache(content)}
      </uui-box>
    `;
  }
}

declare global {
  interface HTMLElementTagNameMap {
    'bc-api-preview-section': ApiPreviewElementSection;
  }
}
