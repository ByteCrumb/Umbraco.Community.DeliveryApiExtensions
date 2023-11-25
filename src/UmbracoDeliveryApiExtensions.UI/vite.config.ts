import {defineConfig} from 'vite';

export default defineConfig(({mode}) => ({
  base: '/App_Plugins/DeliveryApiExtensions',
  build: {
    lib: {
      entry: 'src/main.js',
      name: 'DeliveryApiExtensions',
      formats: ['iife'],
    },
    outDir: 'dist/DeliveryApiExtensions',
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
  esbuild: {
    jsxFactory: 'h',
    jsxFragment: 'Fragment',
    jsxInject: 'import { h, Fragment } from \'preact\'',
    legalComments: 'none',
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
    proxy: {
      // Add support for query param replacement in data files
      // Syntax: {queryParamName|fallbackValue}
      '^/src/data/.*%7B.+%7D': {
        target: 'http://localhost:5173',
        rewrite(path) {
          const url = new URL(path, 'http://localhost');
          return path.replace(/%7B(.+?)(?:%7C(.+?))?%7D/gi, (_, paramName: string, defaultValue: string) => url.searchParams.get(paramName) ?? defaultValue ?? '');
        },
      },
    },
  },
}));
