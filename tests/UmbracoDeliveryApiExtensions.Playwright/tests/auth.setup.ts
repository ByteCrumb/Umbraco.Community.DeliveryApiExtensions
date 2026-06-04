import {expect, test as setup} from '@playwright/test';
import {ConstantHelper, UiHelpers} from '@umbraco-cms/acceptance-test-helpers';

const authFile = 'playwright/.auth/user.json';

setup('authenticate', async ({page}) => {
  const umbracoUi = new UiHelpers(page);

  await umbracoUi.goToBackOffice();
  await expect(page.locator('[name="username"]')).toBeVisible({timeout: 10000});
  await umbracoUi.login.enterEmail(process.env.UMBRACO_USER_LOGIN ?? '');
  await umbracoUi.login.enterPassword(process.env.UMBRACO_USER_PASSWORD ?? '');
  await umbracoUi.login.clickLoginButton();
  await expect(page.getByRole('tab', {name: ConstantHelper.sections.settings})).toBeVisible({timeout: 10000});
  await umbracoUi.page.context().storageState({path: authFile});
});
