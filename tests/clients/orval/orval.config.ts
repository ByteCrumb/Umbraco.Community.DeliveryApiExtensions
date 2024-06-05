import {defineConfig} from 'orval';

export default defineConfig({
  'umbraco-api': {
    input: '../../UmbracoDeliveryApiExtensions.TestSite/delivery.swagger.g.json',
    output: {
      target: 'api/umbraco-api.ts',
      baseUrl: 'http://localhost:34962',
    },
  },
});
