import {expect} from '@playwright/test';
import {type ApiHelpers, ConstantHelper, test} from '@umbraco/playwright-testhelpers';

test.describe('API preview - Media', () => {
  const mediaName = 'PlaywrightTestMedia';

  test.beforeEach(async ({umbracoApi}) => {
    await umbracoApi.login();

    await cleanTestMedia(umbracoApi);
    await createTestMedia(umbracoApi);
  });

  test.afterEach(async ({umbracoApi}) => {
    await cleanTestMedia(umbracoApi);
  });

  test('Preview content app is visible in saved media', async ({page, umbracoUi}) => {
    // Go to test media (PR to add helper method: https://github.com/umbraco/Umbraco.Playwright.Testhelpers/pull/34)
    await umbracoUi.goToSection(ConstantHelper.sections.media);
    await umbracoUi.refreshMediaTree();
    await umbracoUi.clickDataElementByElementName('tree-item-' + mediaName);

    // Check that the content app is visible
    const contentAppLocator = page.locator('button[data-element="sub-view-deliveryApiPreview"]');
    await expect(contentAppLocator).toBeVisible();

    // Click on the content app
    await contentAppLocator.click();

    // Verify that the preview component is visible
    const apiPreviewElement = page.locator('bc-api-preview');
    await expect(apiPreviewElement).toBeVisible();
  });

  async function createTestMedia(umbracoApi: ApiHelpers) {
    await umbracoApi.media.createDefaultFile(mediaName);
  }

  async function cleanTestMedia(umbracoApi: ApiHelpers) {
    await umbracoApi.media.ensureNameNotExists(mediaName);
  }
});
