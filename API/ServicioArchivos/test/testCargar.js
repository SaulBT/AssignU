import grpc from '@grpc/grpc-js';
import protoLoader from '@grpc/proto-loader';
import fs from 'fs';
import path from 'path';

const PROTO_PATH = '../proto/archivos.proto';
const packageDefinition = protoLoader.loadSync(PROTO_PATH);
const archivosProto = grpc.loadPackageDefinition(packageDefinition).archivos;

const client = new archivosProto.ArchivosService('localhost:50051', grpc.credentials.createInsecure());

const archivoPath = path.join('.', 'testfile.pdf');
const stream = fs.createReadStream(archivoPath, { highWaterMark: 1024 });

const call = client.CargarArchivo((err, response) => {
  if (err) return console.error('Error al subir archivo:', err);
  console.log('Archivo cargado con éxito:', response);
});

stream.on('data', chunk => {
  call.write({
    nombre: 'tarea6.pdf',
    idTarea: 6,
    tipo: 'app/pdf',
    chunk: chunk,
    fin: false
  });
});

stream.on('end', () => {
  call.write({
    nombre: 'testfile.txt',
    idTarea: 123456,
    tipo: 'text/plain',
    chunk: Buffer.alloc(0),
    fin: true
  });
  call.end();
});
