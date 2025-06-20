import dotenv from 'dotenv';
import { MongoClient, GridFSBucket } from "mongodb";

dotenv.config();

const cliente = new MongoClient(process.env.MONGODB_URI_PRODUCTION);
await cliente.connect();

const db = cliente.db("archivos_bd_assignu");
const bucket = new GridFSBucket(db, { bucketName: "archivos"});

export { db, bucket};