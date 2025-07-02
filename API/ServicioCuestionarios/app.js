const express = require('express');
const mongoose = require('mongoose');
const cors = require('cors');
require('dotenv').config();
const swaggerUi = require('swagger-ui-express');
const swaggerSpec = require('./configuration/swagger-config');

const manejadorErrores = require('./middleware/manejador-errores');
const { iniciarServidorRpc } = require('./rpc-server');

const app = express();

app.use(cors());
app.use(express.json());

mongoose.connect(process.env.MONGODB_URI_PRODUCTION) // Cambia a MONGODB_URI_DEVELOPMENT si estás en desarrollo
  .then(() => console.log('Conectado a MongoDB'))
  .catch((err) => console.error('Error conectando a MongoDB:', err));

app.use('/cuestionarios', require('./routes/cuestionario-rutas'));
app.use('/swagger', swaggerUi.serve, swaggerUi.setup(swaggerSpec));

app.use(manejadorErrores);

iniciarServidorRpc();

module.exports = app;