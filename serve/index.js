let  express = require('express')
let  serveStatic = require('serve-static')
let {createProxyMiddleware} = require('http-proxy-middleware')
let  app = express()
/*
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
}));*/


//Announcements
  app.use(createProxyMiddleware({
    target: 'http://localhost:61405/',
    xfwd: true,
    changeOrigin: true
  }))

app.listen(3000, () => {
  console.log('Server has started!')
});