const app = require('./app');
const PORT = process.env.PORT_PRODUCTION // Cambia a PORT_DEVELOPMENT si estás en desarrollo

app.listen(PORT, '0.0.0.0', () => {
  console.log(`Servidor corriendo en puerto ${PORT}`);
});
