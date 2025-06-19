const amqp = require('amqplib');

const {
    crearCuestionarioAsync,
    editarCuestionarioAsync,
    eliminarCuestionarioAsync,
    obtenerRespuestaCuestionarioAsync,
    obtenerRespuestasAsync,
    obtenerRespuestasDeTareaAsync,
    obtenerPreguntasDeTareaAsync
} = require('./controllers/cuestionario-controladores');

async function iniciarServidorRpc() {
  const connection = await amqp.connect('amqp://localhost');
  const channel = await connection.createChannel();

  const queue = 'cola_cuestionarios';
  await channel.assertQueue(queue, { durable: false });
  channel.prefetch(1);

  console.log(' [x] Servidor RPC escuchando en ' + queue);

  channel.consume(queue, async (msg) => {
    const content = msg.content.toString();
    console.log(" [.] Mensaje recibido:", content);

    let response = {};
    try {
      const request = JSON.parse(content);

      switch (request.Accion) {
        case 'crearCuestionario':
          response = await crearCuestionarioAsync(request.data);
          break;
        case 'editarCuestionario':
          response = await editarCuestionarioAsync(request.data);
          break;
        case 'eliminarCuestionario':
          response = await eliminarCuestionarioAsync(request.data);
          break;
        case 'obtenerRespuesta':
          response = await obtenerRespuestaCuestionarioAsync(request.data);
          break;
        case 'obtenerRespuestasDeListaTareas':
          response = await obtenerRespuestasAsync(request.data);
          break;
        case 'obtenerPreguntasDeTarea':
          response = await obtenerPreguntasDeTareaAsync(request.data);
          break;
          
        case 'obtenerRespuestasDeClase':
          response = await obtenerRespuestasDeTareaAsync(request.data);
          break;
        default:
          response = {
            Success: false,
            Error: {
                Tipo: "Error",
                Mensaje: "Acción no soportada",
            }
        };
      }
    } catch (err) {
      response = { Success: false, Message: 'Error procesando mensaje: ' + err.Message };
    }

    const responseBuffer = Buffer.from(JSON.stringify(response));

    channel.sendToQueue(msg.properties.replyTo,
      responseBuffer,
      { correlationId: msg.properties.correlationId });

    channel.ack(msg);
  });
}

module.exports = { iniciarServidorRpc };