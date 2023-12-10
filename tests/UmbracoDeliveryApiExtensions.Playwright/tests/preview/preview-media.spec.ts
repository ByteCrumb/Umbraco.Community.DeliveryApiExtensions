import {expect} from '@playwright/test';
import {type ApiHelpers, test} from '@umbraco/playwright-testhelpers';

import {loginAsAdmin, loginAsDev, loginAsEditor} from '../../helpers/UmbracoApiHelpers';

test.describe('API preview - Media', () => {
  const mediaName = 'PlaywrightTestMedia';

  test.beforeEach(async ({umbracoApi}) => {
    await loginAsAdmin(umbracoApi.page);

    await cleanTestMedia(umbracoApi);
    await createTestMedia(umbracoApi);
  });

  test.afterEach(async ({umbracoApi}) => {
    await loginAsAdmin(umbracoApi.page);
    await cleanTestMedia(umbracoApi);
  });

  test('Preview content app is visible in saved media', async ({page, umbracoUi}) => {
    await umbracoUi.navigateToMedia(mediaName);

    // Check that the content app is visible
    const contentAppLocator = page.locator('button[data-element="sub-view-deliveryApiPreview"]');
    await expect(contentAppLocator).toBeVisible();

    // Click on the content app
    await contentAppLocator.click();

    // Verify that the preview component is visible
    const apiPreviewElement = page.locator('bc-api-preview');
    await expect(apiPreviewElement).toBeVisible();
  });

  test('Preview content app is visible for enabled group', async ({page, umbracoUi}) => {
    await loginAsDev(page);

    // Go to test node
    await umbracoUi.navigateToMedia(mediaName);

    // Check that the content app is visible
    const contentAppLocator = page.locator('button[data-element="sub-view-deliveryApiPreview"]');
    await expect(contentAppLocator).toBeVisible();

    // Click on the content app
    await contentAppLocator.click();

    // Verify that the preview component is visible
    const apiPreviewElement = page.locator('bc-api-preview');
    await expect(apiPreviewElement).toBeVisible();
  });

  test('Preview content app is not visible for non enabled group', async ({page, umbracoUi}) => {
    await loginAsEditor(page);

    // Go to test node
    await umbracoUi.navigateToMedia(mediaName);

    // Check that the content app is not visible
    const contentAppLocator = page.locator('button[data-element="sub-view-deliveryApiPreview"]');
    await expect(contentAppLocator).toBeHidden();
  });

  async function createTestMedia(umbracoApi: ApiHelpers) {
    await umbracoApi.media.createDefaultFile(mediaName);
  }

  async function cleanTestMedia(umbracoApi: ApiHelpers) {
    await umbracoApi.media.ensureNameNotExists(mediaName);
  }
});
