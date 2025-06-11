import fs from 'fs';
import path from 'path';
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
const folderPath = path.join('./descargas');
const filePath = path.join(folderPath, `archivo-${idTarea}.pdf`);

if (!fs.existsSync(folderPath)) {
  fs.mkdirSync(folderPath, { recursive: true });
}

const writeStream = fs.createWriteStream(filePath);

const call = client.DescargarArchivo({ idTarea });

call.on('data', (data) => {
  writeStream.write(data.chunk);
});

call.on('end', () => {
  writeStream.end();
  console.log(`Archivo descargado exitosamente como ${filePath}`);
});

call.on('error', (err) => {
  console.error('Error al descargar el archivo:', err.message);
});
