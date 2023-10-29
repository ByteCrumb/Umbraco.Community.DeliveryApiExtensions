import { defineConfig } from "vite";

export default defineConfig({
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
  base: "/App_Plugins/DeliveryApiExtensions"
});
