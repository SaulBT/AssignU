import amqp from 'amqplib';
import { eliminarArchivoViaRPCAsync } from './controllers/archivos-controladores.js';

async function iniciarServidorRpc() {
  const hostDevelopment = 'amqp://localhost';
  const hostProduction = 'amqp://guest:guest@rabbitmq';
  

  const connection = await amqp.connect(hostProduction);
  const channel = await connection.createChannel();

  const queue = 'cola_archivos';
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
        case 'eliminarArchivo':
          response = await eliminarArchivoViaRPCAsync(request.data);
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

export { iniciarServidorRpc };