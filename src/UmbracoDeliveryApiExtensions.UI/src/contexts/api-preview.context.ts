import { UmbControllerBase } from '@umbraco-cms/backoffice/class-api';
import { UmbContextToken } from '@umbraco-cms/backoffice/context-api';
import { UmbControllerHost } from '@umbraco-cms/backoffice/controller-api';
import { ApiPreviewRepository } from './api-preview.repository';
import { UmbStringState } from '@umbraco-cms/backoffice/observable-api';

export class ApiPreviewContext extends UmbControllerBase {
  #repo: ApiPreviewRepository;

  #type: ApiPreviewContentType = ApiPreviewContentType.Document;
  #culture? : string | undefined;
  #uniqueId? : string | undefined;

  #updateDate? = new UmbStringState(undefined);
  updateDate = this.#updateDate?.asObservable();

  constructor(
    host: UmbControllerHost) {
      super(host);
      this.#repo = new ApiPreviewRepository(host);
  }

  setType(type: ApiPreviewContentType) {
    this.#type = type;
  }

  setUniqueId(uniqueId: string) {
    this.#uniqueId = uniqueId;
  }

  setCulture(culture: string | undefined) {
    this.#culture = culture;
  }

  setUpdateDate(updateDate: string | undefined) {
    this.#updateDate?.setValue(updateDate);
  }

  async fetchData(preview: boolean, expand: boolean, signal: AbortSignal): Promise<unknown> {
    if(!this.#uniqueId) return null;
    return this.#repo.fetchData(this.#type, this.#uniqueId, this.#culture, preview, expand, signal);
  }
}

export enum ApiPreviewContentType {
  Document = 'content',
  Media = 'media',
}

export const API_PREVIEW_CONTEXT =
  new UmbContextToken<ApiPreviewContext>('api-preview-context');
