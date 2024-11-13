import {expect} from '@playwright/test';
import {type ApiHelpers, ConstantHelper, test} from '@umbraco/playwright-testhelpers';

test.describe('API preview - Media', () => {
  const mediaName = 'PlaywrightTestMedia';

  test.beforeEach(async ({umbracoApi}) => {
    await cleanTestMedia(umbracoApi);
    await createTestMedia(umbracoApi);
  });

  test.afterEach(async ({umbracoApi}) => {
    await cleanTestMedia(umbracoApi);
  });

  test('Preview content app is visible in saved media', async ({page, umbracoUi}) => {
    await umbracoUi.goToBackOffice();

    await page.getByRole('tab', {name: ConstantHelper.sections.media}).click();
    await umbracoUi.media.mediaCardItems.filter({hasText: mediaName}).locator('button').click();

    // Check that the content app is visible
    const apiTab = page.getByRole('tab', {name: 'API'});
    await apiTab.click({force: true});

    // Verify that the preview component is visible
    const apiPreviewElement = page.locator('bc-api-preview');
    await expect(apiPreviewElement).toBeVisible();
  });

  async function createTestMedia(umbracoApi: ApiHelpers) {
    await umbracoApi.media.createDefaultMediaFile(mediaName);
  }

  async function cleanTestMedia(umbracoApi: ApiHelpers) {
    await umbracoApi.media.ensureNameNotExists(mediaName);
  }
});
