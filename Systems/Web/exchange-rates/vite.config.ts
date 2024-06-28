import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path'

const aliases = ['pages', 'components','functions','models'];

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
})