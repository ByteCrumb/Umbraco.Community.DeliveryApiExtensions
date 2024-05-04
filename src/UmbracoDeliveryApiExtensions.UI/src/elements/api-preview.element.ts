import {UmbElementMixin} from '@umbraco-cms/backoffice/element-api';
import {
  css, html, LitElement, nothing,
} from 'lit';
import {customElement, state} from 'lit/decorators.js';

import {KebabCaseAttributesMixin} from '../mixins/kebab-case-attributes.mixin';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import { UMB_MEDIA_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/media';
import { DocumentVariantStateModel } from '@umbraco-cms/backoffice/external/backend-api';
import { API_PREVIEW_CONTEXT, ApiPreviewContext } from '../contexts/api-preview.context';

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

  #apiPreviewContext?: typeof API_PREVIEW_CONTEXT.TYPE;
  #contentContext?: typeof UMB_DOCUMENT_WORKSPACE_CONTEXT.TYPE;

  @state()
  private _hasPreview = false;

  @state()
  private _isPublished = false;

  constructor(){
    super();

    this.provideContext(API_PREVIEW_CONTEXT, new ApiPreviewContext(this));
    this.consumeContext(API_PREVIEW_CONTEXT, (context) => {
      this.#apiPreviewContext = context;
    });

    this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
      this.#contentContext = context;
      if(!context) return;
      this._hasPreview = true;

      this.observe(
        this.#contentContext.unique,
        (unique) => {
          this.#apiPreviewContext?.setDocumentId(unique!);
        },
        '_documentUnique',
      );

      this.observe(this.#contentContext.variants, (variants) => {
        const state = variants[0]?.state;
        this._isPublished = state === DocumentVariantStateModel.PUBLISHED || state === DocumentVariantStateModel.PUBLISHED_PENDING_CHANGES;

        const currentVariant = variants[0];
        this.#apiPreviewContext?.setCulture(currentVariant?.culture ?? undefined);
        this.#apiPreviewContext?.setUpdateDate(currentVariant?.updateDate ?? undefined);
      });
    });

    this.consumeContext(UMB_MEDIA_WORKSPACE_CONTEXT, (context) => {
      if(!context) return;
      this._hasPreview = false;
    });
  }

  render() {
    return html`
      ${this._hasPreview ? html`
        <bc-api-preview-section headline="Preview" preview></bc-api-preview-section>
      ` : nothing}
      ${this._isPublished ? html`
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
