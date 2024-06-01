import {UmbElementMixin} from '@umbraco-cms/backoffice/element-api';
import { html, LitElement } from 'lit';
import {customElement} from 'lit/decorators.js';

import {KebabCaseAttributesMixin} from '../mixins/kebab-case-attributes.mixin';
/**
 * The Delivery Api Extensions Preview Workspace View element.
 */
@customElement('bc-api-preview-workspace-view')
export default class ApiPreviewWorkspaceView extends UmbElementMixin(KebabCaseAttributesMixin(LitElement)) {


  render() {
    return html`
      <umb-body-layout header-fit-height>
        <bc-api-preview></bc-api-preview>
      </umb-body-layout>
    `;
  }
}

declare global {
  interface HTMLElementTagNameMap {
    'bc-api-preview-workspace-view': ApiPreviewWorkspaceView;
  }
}
