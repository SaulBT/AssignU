const swaggerJSDoc = require('swagger-jsdoc');

const swaggerDefinition = {
  openapi: '3.0.0',
  info: {
    title: 'API de Cuestionarios',
    version: '1.0.0',
    description: 'Documentación de la API de creación de cuestionarios',
  },
  servers: [
    {
      url: 'http://localhost:3000',
      description: 'Servidor local',
    },
  ],
  components: {
    schemas: {
      CuestionarioInput: {
        type: 'object',
        properties: {
          idTarea: { type: 'number'},
          preguntas: {
            type: 'array',
            items: {
              type: 'object',
              properties: {
                texto: { type: 'string' },
                tipo: { type: 'string' },
                opciones: {
                  type: 'array',
                  items: {
                    type: 'object',
                    properties: {
                      texto: { type: 'string' },
                      esCorrecta: { type: 'boolean' }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
  }
};

const options = {
  swaggerDefinition,
  apis: ['./routes/*.js'],
};

const swaggerSpec = swaggerJSDoc(options);

module.exports = swaggerSpec;
