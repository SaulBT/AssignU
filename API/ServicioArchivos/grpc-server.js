import grpc from "@grpc/grpc-js";
import protoLoader from "@grpc/proto-loader";
import { fileURLToPath } from "url";
import path from "path";
import dotenv from 'dotenv';
import { db, bucket } from './storage/db.js';

dotenv.config();

import {
  cargarArchivo,
  descargarArchivo,
  eliminarArchivoAsync
} from "./controllers/archivos-controladores.js";

import {
  iniciarServidorRpc } from './rpc-server.js'

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const PROTO_PATH = path.join(__dirname, "proto", "archivos.proto");

const packageDefinition = protoLoader.loadSync(PROTO_PATH);
const archivosProto = grpc.loadPackageDefinition(packageDefinition).archivos;
console.log(Object.keys(archivosProto));

const server = new grpc.Server();

server.addService(archivosProto.ArchivosService.service, {
CargarArchivo: cargarArchivo,
DescargarArchivo: descargarArchivo,
EliminarArchivo: eliminarArchivoAsync
});

const PORT = process.env.PORT;
server.bindAsync(`0.0.0.0:${PORT}`, grpc.ServerCredentials.createInsecure(), () => {
console.log(`Servidor gRPC corriendo en puerto ${PORT}`);
});

iniciarServidorRpc();