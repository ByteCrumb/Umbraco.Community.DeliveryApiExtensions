import {defineConfig, devices} from '@playwright/test';
import dotenv from 'dotenv';

dotenv.config();

export default defineConfig({
  testDir: './tests/',
  fullyParallel: true,
  forbidOnly: Boolean(process.env.CI),
  retries: process.env.CI ? 2 : 0,
  workers: 1,
  reporter: 'html',
  use: {
    baseURL: process.env.URL,
    trace: 'on-first-retry',
    ignoreHTTPSErrors: true,
  },

  /* Configure projects for major browsers */
  projects: [
    {
      name: 'chromium',
      use: {...devices['Desktop Chrome']},
    },
  ],

  /* Run your local dev server before starting the tests */
  webServer: {
    command: 'dotnet run --project ..\\UmbracoDeliveryApiExtensions.TestSite',
    url: process.env.URL + '/umbraco',
    reuseExistingServer: true,
    ignoreHTTPSErrors: true,
    stdout: process.env.CI ? 'ignore' : 'pipe',
    cwd: '..\\UmbracoDeliveryApiExtensions.TestSite',
  },
});
