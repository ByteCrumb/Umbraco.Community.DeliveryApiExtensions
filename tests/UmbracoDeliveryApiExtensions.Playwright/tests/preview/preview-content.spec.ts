import {expect} from '@playwright/test';
import {ContentBuilder, DocumentTypeBuilder} from '@umbraco/playwright-models';
import {type ApiHelpers, test} from '@umbraco/playwright-testhelpers';

test.describe('API preview - Content', () => {
  const docTypeName = 'PlaywrightTestDocType';
  const nodeName = 'PlaywrightTestNode';

  test.beforeEach(async ({umbracoApi}) => {
    await umbracoApi.login();

    await cleanTestContent(umbracoApi);
    await createTestContent(umbracoApi);
  });

  test.afterEach(async ({umbracoApi}) => {
    await cleanTestContent(umbracoApi);
  });

  test('Preview content app is visible in saved document', async ({page, umbracoUi}) => {
    // Go to test node
    await umbracoUi.navigateToContent(nodeName);

    // Check that the content app is visible
    const contentAppLocator = page.locator('button[data-element="sub-view-deliveryApiPreview"]');
    await expect(contentAppLocator).toBeVisible();

    // Click on the content app
    await contentAppLocator.click();

    // Verify that the preview component is visible
    const apiPreviewElement = page.locator('bc-api-preview');
    await expect(apiPreviewElement).toBeVisible();
  });

  test('Preview content app is not visible in new document', async ({page}) => {
    // Create new document
    await page.locator('button[data-element="tree-item-options"]').first().click();
    await page.locator(`li[data-element*="${docTypeName}"]`).click();

    // Verify that the content app is not visible
    await expect(page.locator('button[data-element="sub-view-deliveryApiPreview"]')).toBeHidden();
  });

  test('Preview content app shows only Preview section in saved document', async ({page, umbracoUi}) => {
    // Navigate to content app
    await umbracoUi.navigateToContent(nodeName);
    await page.locator('button[data-element="sub-view-deliveryApiPreview"]').click();

    // Check that only the preview section is being displayed
    const sectionsLocator = page.locator('bc-api-preview > uui-box');
    await expect(sectionsLocator).toHaveCount(1);

    await expect(sectionsLocator.first().locator('*[slot=headline]')).toHaveText('Preview');
  });

  async function createTestContent(umbracoApi: ApiHelpers) {
    const saveNode = 'saveNew';
    const docType = new DocumentTypeBuilder()
      .withName(docTypeName)
      .withAlias(docTypeName)
      .withAllowAsRoot(true)
      .addGroup()
      .withName('Content')
      .withAlias('content')
      .addTextBoxProperty()
      .withLabel('Title')
      .withAlias('title')
      .done()
      .done()
      .build();

    const createdDocType = await umbracoApi.documentTypes.save(docType);

    const rootContentNode = new ContentBuilder()
      .withContentTypeAlias(createdDocType.alias)
      .withAction(saveNode)
      .addVariant()
      .withName(nodeName)
      .withSave(true)
      .done()
      .build();

    await umbracoApi.content.save(rootContentNode);
  }

  async function cleanTestContent(umbracoApi: ApiHelpers) {
    const contentId = await umbracoApi.content.getContentId(nodeName);
    if (contentId) {
      await umbracoApi.content.deleteById(contentId);
    }

    await umbracoApi.documentTypes.ensureNameNotExists(docTypeName);
  }
});
