const express = require('express')
const serveStatic = require('serve-static')
const app = express()

app.use(serveStatic(`${__dirname}/../Builds`, {
  setHeaders: (res, path) => {
    if (path.endsWith('.wasm.br')) {
      res.setHeader('Content-Type', 'application/wasm');
    }

    if (path.endsWith('.br')) {
      res.setHeader('Content-Encoding', 'br');
    }

    if (path.endsWith('.gz')) {
      res.setHeader('Content-Encoding', 'gzip');
    }
  }
}));

app.listen(3000, () => {
  console.log('Server has started!')
});