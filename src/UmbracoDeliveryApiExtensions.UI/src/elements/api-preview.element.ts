import {provide} from '@lit/context';
import {defineElement} from '@umbraco-ui/uui';
import {css, html, LitElement, nothing} from 'lit';
import {property} from 'lit/decorators.js';

import {KebabCaseAttributesMixin} from '../mixins/kebab-case-attributes.mixin';
import {type ApiPreviewContext, apiPreviewContext} from './api-preview-context';

interface Context extends ApiPreviewContext {
  readonly hasPreview: boolean;
  readonly isPublished: boolean;
}

/**
 * The Delivery Api Extensions Preview element.
 */
@defineElement('bc-api-preview')
export class ApiPreviewElement extends KebabCaseAttributesMixin(LitElement) {
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
    context: Context = undefined!;

  render() {
    if (!this.context) {
      return nothing;
    }

    return html`
      ${this.context?.hasPreview ? html`
        <bc-api-preview-section title="Preview" preview></bc-api-preview-section>
      ` : nothing}
      ${this.context?.isPublished ? html`
        <bc-api-preview-section title="Published"></bc-api-preview-section>
      ` : nothing}
    `;
  }
}

declare global {
  interface HTMLElementTagNameMap {
    'bc-api-preview': ApiPreviewElement;
  }
}
