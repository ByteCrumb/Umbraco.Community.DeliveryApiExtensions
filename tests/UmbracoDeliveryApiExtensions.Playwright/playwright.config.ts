import { defineConfig, devices } from '@playwright/test';

require('dotenv').config();

export default defineConfig({
  testDir: './tests',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  workers: 1,
  reporter: 'html',
  use: {
    baseURL: process.env.URL,
    trace: 'on-first-retry',
  },

  /* Configure projects for major browsers */
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],

  /* Run your local dev server before starting the tests */
  webServer: {
    command: 'dotnet run --project ..\\UmbracoDeliveryApiExtensions.TestSite',
    url: process.env.URL + '/umbraco',
    reuseExistingServer: !process.env.CI,
    ignoreHTTPSErrors: true,
    stdout: process.env.CI ? "ignore": "pipe",
    cwd: '..\\UmbracoDeliveryApiExtensions.TestSite'
  },
});
