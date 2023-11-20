import {defineConfig} from 'orval';

export default defineConfig({
  'umbraco-api': {
    input: 'http://localhost:34962/umbraco/swagger/delivery/swagger.json',
    output: {
      target: 'api/umbraco-api.ts',
      baseUrl: 'http://localhost:34962',
    },
  },
});
