import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path'

const aliases = ['pages', 'components', 'functions', 'models', 'utils', 'http', 'services'];

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: aliases.map(alias => (
      {
        find: `@${alias}`,
        replacement: path.resolve(__dirname, `src/${alias}`),
      }
    ))
  },
  base: "/",
  preview: {
    port: 3000,
    strictPort: true,
  },
  server: {
    port: 3000,
    strictPort: true,
    host: true,
  },
})