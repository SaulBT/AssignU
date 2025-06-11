import { ObjectId } from "mongodb";
import { bucket, db } from "../storage/db.js";
import { PassThrough } from "stream";

export const cargarArchivo = async (call, callback) => {
    let idArchivo = new ObjectId();
    let nombre, tipo, idTarea;
    const pass = new PassThrough();

    call.on("data", (chunk) => {
        if (!nombre && chunk.nombre) {
            nombre = chunk.nombre;
            idTarea = chunk.idTarea;
            tipo = chunk.tipo;

            const uploadStream = bucket.openUploadStreamWithId(idArchivo, nombre, {
                metadata: { tipo, idTarea, fecha: new Date() }
            });
            pass.pipe(uploadStream);
        }

        if (chunk.chunk) {
            pass.write(chunk.chunk);
        }

        if (chunk.fin) {
            pass.end();
        }
    });

    call.on("end", () => {
        callback(null, {
            idArchivo: idArchivo.toString(),
            mensaje: "Archivo subido correctamente"
        });
    });

    call.on("error", () => {
        callback(err);
    });
};

export const descargarArchivo = async (call) => {
  const { idTarea } = call.request;

  try {
    const archivo = await db.collection("archivos.files").findOne({
      "metadata.idTarea": idTarea
    });

    if (!archivo) {
      return call.destroy(new Error(`No se encontró un archivo con la idTarea ${idTarea}`));
    }

    const downloadStream = bucket.openDownloadStream(archivo._id);

    downloadStream.on("data", (chunk) => call.write({ chunk }));
    downloadStream.on("end", () => call.end());
    downloadStream.on("error", (err) => call.destroy(err));
  } catch (error) {
    call.destroy(error);
  }
};

export const eliminarArchivo = async (call, callback) => {
  const { idTarea } = call.request;

  try {
    const archivo = await db.collection("archivos.files").findOne({
      "metadata.idTarea": idTarea
    });

    if (!archivo) {
      return callback(new Error(`No se encontró un archivo con la idTarea ${idTarea}`));
    }

    await bucket.delete(archivo._id);

    callback(null, {
      mensaje: "Archivo eliminado correctamente"
    });
  } catch (error) {
    callback(error);
  }
};