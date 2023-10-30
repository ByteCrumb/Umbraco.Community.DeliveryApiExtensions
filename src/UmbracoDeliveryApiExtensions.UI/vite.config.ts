import { defineConfig } from "vite";

export default defineConfig({
  base: "/App_Plugins/DeliveryApiExtensions",
  build: {
    lib: {
      entry: "src/main.js",
      name: "DeliveryApiExtensions",
      formats: ["es", "iife"],
    },
    outDir: "../UmbracoDeliveryApiExtensions/wwwroot/DeliveryApiExtensions",
    emptyOutDir: true,
    sourcemap: true,
    rollupOptions: {
      external: [/^@umbraco/]
    },
  },
  server: {
    port: 5173,
    strictPort: true
  }
});
