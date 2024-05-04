import { UMB_AUTH_CONTEXT } from "@umbraco-cms/backoffice/auth";
import { UmbControllerBase } from "@umbraco-cms/backoffice/class-api";
import { UmbControllerHost } from "@umbraco-cms/backoffice/controller-api";

export class ApiPreviewRepository extends UmbControllerBase {
  #apiPath: string = '';
  #getToken: () => Promise<string> = async () => '';

  constructor(host: UmbControllerHost) {
    super(host);

    this.consumeContext(UMB_AUTH_CONTEXT, (_auth) => {
      const umbOpenApi = _auth.getOpenApiConfiguration();
      this.#getToken = umbOpenApi.token;
      // TODO: Make this work for media
      this.#apiPath = `${umbOpenApi.base}/umbraco/delivery-api-extensions/preview/content`;
   });
  }

  async fetchData(
    documentId: string,
    culture: string | undefined,
    preview: boolean,
    expand: boolean,
    signal: AbortSignal)
    : Promise<unknown> {
    if (!this.#apiPath) {
      return null;
    }

    const params: RequestInit & {headers: Record<string, string>} = {
      method: 'GET',
      headers: {
        'Authorization': `Bearer ${await this.#getToken()}`,
      },
      credentials: 'include',
      signal,
    };

    if (culture) {
      params.headers['Accept-Language'] = culture;
    }

    if (preview) {
      params.headers.preview = 'true';
    }

    const response = await fetch(`${this.#apiPath}/${documentId}${(expand ? '?expand=properties[$all]' : '')}`, params);
    if (!response.ok) {
      throw new Error(response.statusText);
    }

    return response.json();
  }
}
