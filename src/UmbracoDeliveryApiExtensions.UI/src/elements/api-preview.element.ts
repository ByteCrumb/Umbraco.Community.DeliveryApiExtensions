import {provide} from '@lit/context';
import {UmbElementMixin} from '@umbraco-cms/backoffice/element-api';
import {
  css, html, LitElement, nothing,
} from 'lit';
import {customElement, property} from 'lit/decorators.js';

import {type ApiPreviewContext, apiPreviewContext} from '../contexts/api-preview.context';
import {KebabCaseAttributesMixin} from '../mixins/kebab-case-attributes.mixin';

export interface PreviewControllerContext extends ApiPreviewContext {
  readonly hasPreview: boolean;
  readonly isPublished: boolean;
}

/**
 * The Delivery Api Extensions Preview element.
 */
@customElement('bc-api-preview')
export default class ApiPreviewElement extends UmbElementMixin(KebabCaseAttributesMixin(LitElement)) {
  static styles = css`
    :host {
        display: flex;
        flex-direction: column;
        gap: 1rem;
    }

    @media (min-width: 1024px) {
      :host {
        flex-direction: row;
      }

      :host > * {
        flex: 1;
      }
     }
  `;

  @provide({context: apiPreviewContext})
  @property({type: Object, attribute: false})
    context: PreviewControllerContext = {
      apiPath: '/src/data/content-{expand|none}.json',
      isPublished: true,
      hasPreview: true,
    };

  render() {
    if (!this.context) {
      return nothing;
    }

    return html`
      ${this.context?.hasPreview ? html`
        <bc-api-preview-section headline="Preview" preview></bc-api-preview-section>
      ` : nothing}
      ${this.context?.isPublished ? html`
        <bc-api-preview-section headline="Published"></bc-api-preview-section>
      ` : nothing}
    `;
  }
}

declare global {
  interface HTMLElementTagNameMap {
    'bc-api-preview': ApiPreviewElement;
  }
}
