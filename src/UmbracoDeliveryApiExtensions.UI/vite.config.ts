import {defineConfig, PluginOption} from 'vite';
import {readFile, writeFile} from 'node:fs/promises';

export default defineConfig(({mode}) => ({
  base: '/App_Plugins/DeliveryApiExtensions',
  plugins: [replaceUmbracoPackageVersion],
  build: {
    lib: {
      entry: 'src/main.ts',
      name: 'DeliveryApiExtensions',
      formats: ['es'],
    },
    outDir: 'dist/DeliveryApiExtensions',
    emptyOutDir: true,
    sourcemap: true,
    rollupOptions: {
      external: [/^@umbraco/],
    },
  },
  define: {
    'process.env.NODE_ENV': JSON.stringify(mode),
  },
  oxc: {
    jsx: {
      runtime: 'automatic',
      importSource: 'preact',
    },
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

const replaceUmbracoPackageVersion: PluginOption = {
  name: 'sync-umbraco-package-version',
  async buildStart() {
    const umbracoPackageJsonPath = 'public/umbraco-package.json';
    const content = await readFile(umbracoPackageJsonPath, 'utf-8');
    const currentVersion = content.match(/"version":\s*"([^"]+)"/)?.[1];
    if(!currentVersion){
      throw new Error('Missing version in umbraco-package.json');
    }

    const packageVersion = process.env.npm_package_version ?? '0.0.0';
    if (currentVersion === packageVersion) {
      return;
    }

    const updatedContent = content.replace(currentVersion, packageVersion);
    await writeFile(umbracoPackageJsonPath, updatedContent);
    console.log(`✓ Updated umbraco-package.json version to ${packageVersion}`);
  },
};
