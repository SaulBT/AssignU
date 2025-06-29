const app = require('./app');
const PORT = process.env.PORT_PRODUCTION;

app.listen(PORT, '0.0.0.0', () => {
  console.log(`Servidor corriendo en puerto ${PORT}`);
});
