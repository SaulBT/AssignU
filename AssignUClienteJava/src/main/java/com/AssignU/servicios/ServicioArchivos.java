package com.AssignU.servicios;

import archivos.Archivos;
import archivos.ArchivosServiceGrpc;
import com.AssignU.utils.Utils;
import com.google.protobuf.ByteString;
import io.grpc.ManagedChannel;
import io.grpc.ManagedChannelBuilder;
import io.grpc.stub.StreamObserver;
import javafx.application.Platform;
import javafx.scene.control.Alert;

import java.io.*;
import java.util.function.Consumer;

public class ServicioArchivos {

    ManagedChannel canal = ManagedChannelBuilder.forAddress("localhost", 5011)
            .usePlaintext()
            .build();

    ArchivosServiceGrpc.ArchivosServiceStub stub = ArchivosServiceGrpc.newStub(canal);

    public void cargarArchivo(String rutaArchivo, int idTarea, String tipo) throws IOException {

        StreamObserver<Archivos.ArchivoRespuesta> respuesta = new StreamObserver<>() {
            @Override
            public void onNext(Archivos.ArchivoRespuesta value) {
                System.out.println("Archivo subido con ID: " + value.getId());
            }

            @Override
            public void onError(Throwable t) {
                System.err.println("Error en subida: " + t.getMessage());
            }

            @Override
            public void onCompleted() {
                System.out.println("Subida completada.");
                cerrarCanal();
            }
        };

        StreamObserver<Archivos.SubidaArchivoRequest> solicitud = stub.cargarArchivo(respuesta);

        try (InputStream input = new FileInputStream(rutaArchivo)) {
            byte[] buffer = new byte[1024];
            int bytesRead;
            boolean primera = true;

            while ((bytesRead = input.read(buffer)) != -1) {
                Archivos.SubidaArchivoRequest.Builder req = Archivos.SubidaArchivoRequest.newBuilder()
                        .setChunk(ByteString.copyFrom(buffer, 0, bytesRead))
                        .setFin(false);

                if (primera) {
                    req.setNombre(new File(rutaArchivo).getName())
                            .setIdTarea(idTarea)
                            .setTipo(tipo);
                    primera = false;
                }

                solicitud.onNext(req.build());
            }

            // Señal de fin
            solicitud.onNext(Archivos.SubidaArchivoRequest.newBuilder().setFin(true).build());
            solicitud.onCompleted();
        }
    }

    public void descargarArchivo(int idTarea, String archivoDestino) throws IOException {
        Archivos.IdTarea solicitud = Archivos.IdTarea.newBuilder()
                .setIdTarea(idTarea)
                .build();

        OutputStream output = new FileOutputStream(archivoDestino);

        stub.descargarArchivo(solicitud, new StreamObserver<>() {
            @Override
            public void onNext(Archivos.DescargaArchivoResponse chunk) {
                try {
                    output.write(chunk.getChunk().toByteArray());
                } catch (IOException e) {
                    System.err.println("Error escribiendo archivo: " + e.getMessage());
                }
            }

            @Override
            public void onError(Throwable t) {
                System.err.println("Error al descargar: " + t.getMessage());
            }

            @Override
            public void onCompleted() {
                try {
                    output.close();
                    Platform.runLater(() -> {
                        Utils.mostrarAlerta("Descarga exitosa", "El archivo se ha descargado en la carpeta downloads", Alert.AlertType.INFORMATION);
                    });
                    cerrarCanal();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        });
    }

    public void obtenerMetadatos(int idTarea, Consumer<String> callbackNombre) {
        Archivos.IdTarea solicitud = Archivos.IdTarea.newBuilder()
                .setIdTarea(idTarea)
                .build();

        stub.obtenerMetadatos(solicitud, new StreamObserver<Archivos.MetadatosArchivo>() {

            @Override
            public void onNext(Archivos.MetadatosArchivo metadatos) {

                System.out.println("📄 Metadatos del archivo:");
                System.out.println("Nombre        : " + metadatos.getNombre());
                System.out.println("Tipo          : " + metadatos.getTipo());
                System.out.println("Tamaño (bytes): " + metadatos.getTamano());
                System.out.println("Fecha subida  : " + metadatos.getFechaSubida());
                callbackNombre.accept(metadatos.getNombre());
            }

            @Override
            public void onError(Throwable t) {
                System.err.println("❌ Error al obtener metadatos: " + t.getMessage());
            }

            @Override
            public void onCompleted() {
                System.out.println("Consulta de metadatos completada.");
            }
        });
    }

    public void eliminarArchivo(int idTarea, Consumer<Boolean> callbackNombre) {
        Archivos.IdTarea solicitud = Archivos.IdTarea.newBuilder()
                .setIdTarea(idTarea)
                .build();

        stub.eliminarArchivo(solicitud, new StreamObserver<Archivos.OperacionRespuesta>() {
            @Override
            public void onNext(Archivos.OperacionRespuesta respuesta) {
                System.out.println("🗑️  " + respuesta.getMensaje());
                callbackNombre.accept(true);
            }

            @Override
            public void onError(Throwable t) {
                System.err.println("❌ Error al eliminar archivo: " + t.getMessage());
            }

            @Override
            public void onCompleted() {
                System.out.println("✅ Operación de eliminación completada.");
            }
        });
    }

    public void cerrarCanal() {
        canal.shutdownNow();
    }
}
