import Cookies from 'js-cookie';

export const XsrfTokenHeaderName = 'x-umb-xsrf-token';

export function getCsrfToken(): string {
  return Cookies.get('UMB-XSRF-TOKEN') ?? '';
}

export async function parseJsonResponse(response: Response): Promise<unknown> {
  let responseBodyText = await response.text();
  if (responseBodyText.startsWith(')]}\',')) {
    responseBodyText = responseBodyText.substring(5);
  }

  return JSON.parse(responseBodyText) as unknown;
}
