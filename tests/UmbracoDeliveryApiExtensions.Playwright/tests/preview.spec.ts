import {ApiHelpers, test} from '@umbraco/playwright-testhelpers';
import {expect} from "@playwright/test";
import { ContentBuilder, DocumentTypeBuilder } from '@umbraco/playwright-models';

test.describe('API preview', () => {
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
  
  test('Content app is visible', async ({page, umbracoUi}) => {
    // Go to test node
    await umbracoUi.navigateToContent(nodeName);

    // Check that the content app is visible
    await expect(page.locator('button[data-element="sub-view-deliveryApiPreview"]')).toBeVisible();

    // Click on the content app
    await page.locator('button[data-element="sub-view-deliveryApiPreview"]').click();

    // Check that the preview component is visible
    await expect(page.locator('bc-api-preview')).toBeVisible();
  });

  async function cleanTestContent(umbracoApi: ApiHelpers){
    const contentId = await umbracoApi.content.getContentId(nodeName);
    if(contentId){
      await umbracoApi.content.deleteById(contentId);
    }
    await umbracoApi.documentTypes.ensureNameNotExists(docTypeName);
  }

  async function createTestContent(umbracoApi: ApiHelpers){
    const saveNode = "saveNew";
    const docType = new DocumentTypeBuilder()
      .withName(docTypeName)
      .withAllowAsRoot(true)
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
});