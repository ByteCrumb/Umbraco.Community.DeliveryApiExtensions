import {defineConfig, devices} from '@playwright/test';
import dotenv from 'dotenv';

dotenv.config();

export default defineConfig({
  testDir: './tests/',
  timeout: 60000,
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
    {name: 'setup', testMatch: /.*\.setup\.ts/},
    {
      name: 'chromium',
      dependencies: ['setup'],
      use: {
        ...devices['Desktop Chrome'],
        storageState: 'playwright/.auth/user.json',
      },
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
