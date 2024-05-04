import { UmbControllerBase } from '@umbraco-cms/backoffice/class-api';
import { UmbContextToken } from '@umbraco-cms/backoffice/context-api';
import { UmbControllerHost } from '@umbraco-cms/backoffice/controller-api';
import { ApiPreviewRepository } from './api-preview.repository';
import { UmbStringState } from '@umbraco-cms/backoffice/observable-api';

export class ApiPreviewContext extends UmbControllerBase {
  #repo: ApiPreviewRepository;

  #culture? = new UmbStringState(undefined);
  culture = this.#culture?.asObservable();

  #documentId? = new UmbStringState(undefined);
  documentId = this.#documentId?.asObservable();

  #updateDate? = new UmbStringState(undefined);
  updateDate = this.#updateDate?.asObservable();

  constructor(host: UmbControllerHost) {
      super(host);
      this.#repo = new ApiPreviewRepository(host);
  }

  setDocumentId(documentId: string) {
    this.#documentId?.setValue(documentId);
  }

  setCulture(culture: string | undefined) {
    this.#culture?.setValue(culture);
  }

  setUpdateDate(updateDate: string | undefined) {
    this.#updateDate?.setValue(updateDate);
  }

  async fetchData(preview: boolean, expand: boolean, signal: AbortSignal): Promise<unknown> {
    const documentId = this.#documentId?.getValue();
    if(!documentId) return null;

    return this.#repo.fetchData(documentId, this.#culture?.getValue(), preview, expand, signal);
  }
}

export const API_PREVIEW_CONTEXT =
  new UmbContextToken<ApiPreviewContext>('api-preview');
