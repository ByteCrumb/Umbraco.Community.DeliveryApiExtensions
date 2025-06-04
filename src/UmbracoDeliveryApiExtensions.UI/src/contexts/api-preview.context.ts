import { UmbControllerBase } from '@umbraco-cms/backoffice/class-api';
import { UmbContextToken } from '@umbraco-cms/backoffice/context-api';
import { UmbControllerHost } from '@umbraco-cms/backoffice/controller-api';
import { ApiPreviewRepository } from './api-preview.repository';

export class ApiPreviewContext extends UmbControllerBase {
  #repo: ApiPreviewRepository;

  #type: ApiPreviewContentType = ApiPreviewContentType.Document;
  #culture? : string | undefined;
  #segment? : string | undefined;
  #uniqueId? : string | undefined;

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

  getCulture() {
    return this.#culture;
  }

  setCulture(culture: string | undefined) {
    this.#culture = culture;
  }

  getSegment() {
    return this.#segment;
  }

  setSegment(segment: string | undefined) {
    this.#segment = segment;
  }

  async fetchData(preview: boolean, expand: boolean, signal: AbortSignal): Promise<unknown> {
    if(!this.#uniqueId) return null;
    return this.#repo.fetchData(this.#type, this.#uniqueId, this.#culture, this.#segment, preview, expand, signal);
  }
}

export enum ApiPreviewContentType {
  Document = 'content',
  Media = 'media',
}

export const API_PREVIEW_CONTEXT =
  new UmbContextToken<ApiPreviewContext>('api-preview-context');
