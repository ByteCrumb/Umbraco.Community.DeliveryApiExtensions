import {defineElement} from '@umbraco-ui/uui';
import {css, html, LitElement, nothing} from 'lit';
import {property} from 'lit/decorators.js';

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

  @property({type: Boolean})
    hasPreview = false;

  render() {
    return html`
      ${this.hasPreview ? html`
        <bc-api-preview-tab title="Preview" api-path=${this.apiPath} culture=${this.culture} preview></bc-api-preview-tab>
      ` : nothing}
      ${this.isPublished ? html`
        <bc-api-preview-tab title="Published" api-path=${this.apiPath} culture=${this.culture}></bc-api-preview-tab>
      ` : nothing}
    `;
  }
}

declare global {
  interface HTMLElementTagNameMap {
    'bc-api-preview': ApiPreviewElement;
  }
}
