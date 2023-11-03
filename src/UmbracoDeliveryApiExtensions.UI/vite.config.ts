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
      external: [/^@umbraco/],
      output: {
        globals: {
          '@umbraco-ui/uui': 'uui',
        },
      },
    },
  },
  define: {
    'process.env.NODE_ENV': JSON.stringify(mode),
  },
  server: {
    port: 5173,
    strictPort: true,
  },
}));
