import { MongoClient, GridFSBucket } from "mongodb";

const cliente = new MongoClient("mongodb://localhost:27017/archivos_bd_assignu");
await cliente.connect();

const db = cliente.db("archivos_bd_assignu");
const bucket = new GridFSBucket(db, { bucketName: "archivos"});

export { db, bucket};