import grpc from '@grpc/grpc-js';
import protoLoader from '@grpc/proto-loader';

const PROTO_PATH = '../proto/archivos.proto';

const packageDefinition = protoLoader.loadSync(PROTO_PATH);
const archivosProto = grpc.loadPackageDefinition(packageDefinition).archivos;

const client = new archivosProto.ArchivosService(
  'localhost:50051',
  grpc.credentials.createInsecure()
);

const idTarea = 123456;

client.EliminarArchivo({ idTarea }, (err, response) => {
  if (err) {
    console.error('Error al eliminar archivo:', err.message);
  } else {
    console.log('Respuesta del servidor:', response.mensaje);
  }
});
