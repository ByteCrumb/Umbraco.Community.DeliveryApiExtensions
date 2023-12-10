import {type Page} from '@playwright/test';

export async function loginAsAdmin(page: Page): Promise<void> {
  await login(page, process.env.ADMIN_USER_LOGIN, process.env.USER_PASSWORD);
}

export async function loginAsDev(page: Page): Promise<void> {
  await login(page, process.env.DEV_USER_LOGIN, process.env.USER_PASSWORD);
}

export async function loginAsEditor(page: Page): Promise<void> {
  await login(page, process.env.EDITOR_USER_LOGIN, process.env.USER_PASSWORD);
}

async function login(page: Page, username: string | undefined, password: string | undefined): Promise<void> {
  // Make sure we are logged out
  await page.context().clearCookies();

  await page.request.post(process.env.URL + '/umbraco/backoffice/UmbracoApi/Authentication/PostLogin', {
    headers: {
      contentType: 'application/json',
    },
    data: {
      username,
      password,
    },
    ignoreHTTPSErrors: true,
  });

  await page.goto('/umbraco');
}
