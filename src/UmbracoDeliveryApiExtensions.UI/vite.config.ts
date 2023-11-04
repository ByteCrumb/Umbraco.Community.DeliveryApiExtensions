import {defineConfig} from 'vite';

export default defineConfig(({mode}) => ({
  base: '/App_Plugins/DeliveryApiExtensions',
  build: {
    lib: {
      entry: 'src/main.js',
      name: 'DeliveryApiExtensions',
      formats: ['es', 'iife'],
    },
    outDir: '../UmbracoDeliveryApiExtensions/wwwroot/DeliveryApiExtensions',
    emptyOutDir: true,
    sourcemap: true,
    rollupOptions: {
      external: [/^angular/, /^@umbraco/],
      output: {
        globals: {
          angular: 'angular',
          '@umbraco-ui/uui': 'uui',
        },
      },
    },
  },
  define: {
    'process.env.NODE_ENV': JSON.stringify(mode),
  },
  resolve: {
    alias: {
      'react-dom/test-utils': 'preact/test-utils',
      'react-dom': 'preact/compat',
      react: 'preact/compat',
    },
  },
  server: {
    port: 5173,
    strictPort: true,
  },
}));
