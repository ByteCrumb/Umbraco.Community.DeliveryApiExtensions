import {expect} from '@playwright/test';
import {type ApiHelpers, ConstantHelper, test} from '@umbraco/playwright-testhelpers';

test.describe('API preview - Content', () => {
  const docTypeName = 'PlaywrightTestDocType';
  const nodeName = 'PlaywrightTestNode';

  test.beforeEach(async ({umbracoApi}) => {
    await cleanTestContent(umbracoApi);
    await createTestContent(umbracoApi);
  });

  test.afterEach(async ({umbracoApi}) => {
    await cleanTestContent(umbracoApi);
  });

  test('Preview content app is visible in saved document', async ({page, umbracoUi}) => {
    await umbracoUi.goToBackOffice();

    // Go to test node
    await page.getByRole('tab', {name: ConstantHelper.sections.content}).click();
    await umbracoUi.content.goToContentWithName(nodeName);

    // Check that the content app is visible
    const apiTab = page.getByRole('tab', {name: 'API'});
    await apiTab.click({force: true});

    // Verify that the preview component is visible
    const apiPreviewElement = page.locator('bc-api-preview');
    await expect(apiPreviewElement).toBeVisible();
  });

  test('Preview content app is not visible in new document', async ({page, umbracoUi}) => {
    await umbracoUi.goToBackOffice();

    // Create new document
    await page.getByRole('tab', {name: ConstantHelper.sections.content}).click();
    await page.waitForSelector('umb-section-sidebar-menu-with-entity-actions #header');
    await page.locator('uui-action-bar uui-button[label="Create"]').first().click();
    await umbracoUi.content.chooseDocumentType(docTypeName);
    await page.waitForSelector('uui-input[data-mark="input:entity-name"]');

    // Verify that the content app is not visible
    await expect(page.locator('uui-tab[data-mark="workspace:view-link:deliveryApiPreview"]')).toBeHidden();
  });

  test('Preview content app shows only Preview section in saved document', async ({page, umbracoUi}) => {
    await umbracoUi.goToBackOffice();

    // Navigate to content app
    await page.getByRole('tab', {name: ConstantHelper.sections.content}).click();
    await umbracoUi.content.goToContentWithName(nodeName);
    const apiTab = page.getByRole('tab', {name: 'API'});
    await apiTab.click({force: true});

    // Check that only the preview section is being displayed
    const sectionsLocator = page.locator('bc-api-preview-section');
    await expect(sectionsLocator).toHaveCount(1);

    const previewSection = sectionsLocator.first();
    await expect(previewSection).toHaveAttribute('preview');

    await expect(previewSection.locator('bc-json-preview')).toBeVisible();
  });

  async function createTestContent(umbracoApi: ApiHelpers) {
    const documentTypeId = await umbracoApi.documentType.createDefaultDocumentTypeWithAllowAsRoot(docTypeName);
    if(!documentTypeId) {
      throw new Error(`Failed to create document type with name: ${docTypeName}`);
    }

    await umbracoApi.document.createDefaultDocument(nodeName, documentTypeId);
  }

  async function cleanTestContent(umbracoApi: ApiHelpers) {
    await umbracoApi.document.ensureNameNotExists(nodeName);
    await umbracoApi.documentType.ensureNameNotExists(docTypeName);
  }
});
