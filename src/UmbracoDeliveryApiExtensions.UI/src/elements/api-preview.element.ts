import {provide} from '@lit/context';
import {defineElement} from '@umbraco-ui/uui';
import {css, html, LitElement, nothing} from 'lit';
import {property} from 'lit/decorators.js';

import {apiPreviewContext} from '../contexts/api-preview.context';
import {type PreviewControllerContext} from '../controllers/preview.controller';
import {KebabCaseAttributesMixin} from '../mixins/kebab-case-attributes.mixin';

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
    context: PreviewControllerContext = undefined!;

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
