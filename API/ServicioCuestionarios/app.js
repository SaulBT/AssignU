const express = require('express');
const mongoose = require('mongoose');
const cors = require('cors');
require('dotenv').config();
const swaggerUi = require('swagger-ui-express');
const swaggerSpec = require('./middleware/swaggerConfig');

const errorHandler = require('./middleware/errorHandler');
const asyncHandler = require('./middleware/asyncHandler');

const app = express();

app.use(cors());
app.use(express.json());

// Conexión a MongoDB
mongoose.connect(process.env.MONGODB_URI)
  .then(() => console.log('Conectado a MongoDB'))
  .catch((err) => console.error('Error conectando a MongoDB:', err));

app.use('/cuestionario', require('./routes/CuestionarioRutas'));

app.use('/swagger', swaggerUi.serve, swaggerUi.setup(swaggerSpec));

app.use(errorHandler);
//app.use(asyncHandler);

module.exports = app;