import {UmbElementMixin} from '@umbraco-cms/backoffice/element-api';
import {
  css, html, LitElement, nothing,
} from 'lit';
import {customElement, state} from 'lit/decorators.js';

import {KebabCaseAttributesMixin} from '../mixins/kebab-case-attributes.mixin';
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import { UMB_MEDIA_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/media';
import { DocumentVariantStateModel } from '@umbraco-cms/backoffice/external/backend-api';
import { API_PREVIEW_CONTEXT, ApiPreviewContentType, ApiPreviewContext } from '../contexts/api-preview.context';

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

  #apiPreviewContext = new ApiPreviewContext(this);

  @state()
  private _hasPreview = false;

  @state()
  private _isPublished = false;

  constructor(){
    super();
    this.provideContext(API_PREVIEW_CONTEXT, this.#apiPreviewContext);

    // Content context
    this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
      if(!context) return;
      this._hasPreview = true;
      this.#apiPreviewContext?.setType(ApiPreviewContentType.Document);

      this.observe(
        context.unique,
        (unique) => {
          this.#apiPreviewContext?.setUniqueId(unique!);
        }
      );

      this.observe(context.variants, (variants) => {
        // TODO: Get the actual current variant and not just the first one. This is also not called on Save.
        const state = variants[0]?.state;
        this._hasPreview = state && state !== DocumentVariantStateModel.NOT_CREATED ? true : false;
        this._isPublished = state === DocumentVariantStateModel.PUBLISHED || state === DocumentVariantStateModel.PUBLISHED_PENDING_CHANGES;

        const currentVariant = variants[0];
        this.#apiPreviewContext?.setCulture(currentVariant?.culture ?? undefined);
        this.#apiPreviewContext?.setUpdateDate(currentVariant?.updateDate ?? undefined);
      });
    });

    // Media context
    this.consumeContext(UMB_MEDIA_WORKSPACE_CONTEXT, (context) => {
      if(!context) return;
      this.#apiPreviewContext?.setType(ApiPreviewContentType.Media);
      this._hasPreview = false;

      this.observe(
        context.unique,
        (unique) => {
          if(!unique) return;
          this.#apiPreviewContext?.setUniqueId(unique);
        }
      );

      this.observe(context.isNew, (isNew) => {
        this._isPublished = !isNew;
      });

      this.observe(context.variants, (variants) => {
        this.#apiPreviewContext?.setUpdateDate(variants[0]?.updateDate ?? undefined);
      });
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
