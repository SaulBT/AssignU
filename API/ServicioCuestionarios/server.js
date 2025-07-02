const app = require('./app');
const PORT = process.env.PORT_DEVELOPMENT;

app.listen(PORT, '0.0.0.0', () => {
  console.log(`Servidor corriendo en puerto ${PORT}`);
});
