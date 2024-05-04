import { UMB_AUTH_CONTEXT } from "@umbraco-cms/backoffice/auth";
import { UmbControllerBase } from "@umbraco-cms/backoffice/class-api";
import { UmbControllerHost } from "@umbraco-cms/backoffice/controller-api";
import { ApiPreviewContentType } from "./api-preview.context";
import { ApiPreviewConfig } from "../config/api-preview.config";
import { UmbContextConsumerController } from "@umbraco-cms/backoffice/context-api";

export class ApiPreviewRepository extends UmbControllerBase {
  #apiPath: string = '';
  #getToken: () => Promise<string> = async () => '';
  #init: Promise<unknown>;
  #contextConsumer;

  constructor(host: UmbControllerHost) {
    super(host);

    this.#contextConsumer = new UmbContextConsumerController(this, UMB_AUTH_CONTEXT, (_auth) => {
			const umbOpenApi = _auth.getOpenApiConfiguration();
      this.#getToken = umbOpenApi.token;
      this.#apiPath = `${umbOpenApi.base}/umbraco/delivery-api-extensions/preview`;
		});
    this.#init = this.#contextConsumer.asPromise();
  }

  async fetchData(
    type: ApiPreviewContentType,
    uniqueId: string,
    culture: string | undefined,
    preview: boolean,
    expand: boolean,
    signal: AbortSignal)
    : Promise<unknown> {
    if (!this.#apiPath) {
      return null;
    }
    await this.#init;
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

    const response = await fetch(`${this.#apiPath}/${type}/${uniqueId}${(expand ? '?expand=properties[$all]' : '')}`, params);
    if (!response.ok) {
      throw new Error(response.statusText);
    }

    return response.json();
  }

  async fetchConfig()
    : Promise<ApiPreviewConfig | null> {
    await this.#init;
    if (!this.#apiPath) {
      return null;
    }

    const params: RequestInit & {headers: Record<string, string>} = {
      method: 'GET',
      headers: {
        'Authorization': `Bearer ${await this.#getToken()}`,
      },
      credentials: 'include'
    };

    const response = await fetch(`${this.#apiPath}/config`, params);
    return response.json();
  }
}
