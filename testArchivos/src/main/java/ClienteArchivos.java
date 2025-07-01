import archivos.Archivos;
import archivos.ArchivosServiceGrpc;
import com.google.protobuf.ByteString;
import io.grpc.ManagedChannel;
import io.grpc.ManagedChannelBuilder;
import io.grpc.stub.StreamObserver;

import java.io.*;

public class ClienteArchivos {
    public static void main(String[] args) throws Exception {
        ManagedChannel canal = ManagedChannelBuilder.forAddress("localhost", 5011)
                .usePlaintext()
                .build();

        ArchivosServiceGrpc.ArchivosServiceStub stub = ArchivosServiceGrpc.newStub(canal);

        String comando = args[0];

        switch(comando) {
            /*
                Para ir probando los métodos tienes que pasarle el tipo de método en el argumento al ejecutar la clase
                PERO ESO SÓLO ES PARA PROBAR. Si quieres implementar esto en el proyecto debes de hacer las siguientes
                modificaciones:
                1. Quitale el método main (pues si no mames).
                2. Haz que el canal (ManagedChannel) y el stub (ArchivosServiceStub) sean propeidades de la clase, o bien
                   inicializalos cada vez que llames a un método de los 4 definidios. El chiste es que, en la firma de
                   los métodos que ves abajo no tengas que pasarle el stub.
                Con esto, te explico cómo funciona cada uno:

                    cargarArchivo: carga un archivo, recibe la ruta del archivo, el id de la tarea y el tipo de archivo.
                    Cada vez que vayas a cargar un archivo primero tienes que cargar la tarea para que te devuelva el idTarea
                    y luego pasarselo al método cargaArchivo. EL TAMAÑO MÁXIMO ES 100 megabytes pero si quieres ampliarlo
                    me dices, dicho eso, tienes que hacer un método para que cheque el tamaño.

                    descargarArchivo: descarga un archivo, le pasas el idTarea y la ruta a donde vas a descargar el archivo,
                    LA RUTA TIENE QUE IR CON EL NOMBRE DEL ARCHIVO, así como lo ves en el método de abajo.

                    eliminarArcivo: elimina un archivo, le pasa el idTarea nomás.

                    obtenerMetadatos: es para obtener los datos del archivo, los que te van a servir son el nombre y el peso.
                    Tienes que pasarle la idTarea nada más.

                Ahora bien, algo que tienes que saber es que, si quieres moverle para que haga o muestre algo en un
                momento dado de la ejecución de los métodos chécate que todos tienen tres "partes": onNext digamos que
                sucede en lo que se hacen las cosas, onError es por si salta una excepción y onCompleted es una vez que
                ya se terminó de ejecutar todo.
             */
            case "upload":
                cargarArchivo(stub, "PDF-Prueba.pdf", 1, "pdf");
                break;
            case "download":
                descargarArchivo(stub, 1, "downloads/PDF-descargado.pdf");
                break;
            case "eliminar":
                eliminarArchivo(stub, 1);
                break;
            case "metadatos":
                obtenerMetadatos(stub, 1);
                break;
            default:
                System.out.println("Operacion insoportada");
                break;
        }

        Thread.sleep(3000);
        canal.shutdown();
    }

    public static void cargarArchivo(ArchivosServiceGrpc.ArchivosServiceStub stub, String rutaArchivo, int idTarea, String tipo) throws IOException {
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

    // Descargar archivo y guardarlo
    public static void descargarArchivo(ArchivosServiceGrpc.ArchivosServiceStub stub, int idTarea, String archivoDestino) throws IOException {
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
                    System.out.println("Descarga finalizada.");
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        });
    }

    public static void eliminarArchivo(ArchivosServiceGrpc.ArchivosServiceStub stub, int idTarea) {
        Archivos.IdTarea solicitud = Archivos.IdTarea.newBuilder()
                .setIdTarea(idTarea)
                .build();

        stub.eliminarArchivo(solicitud, new StreamObserver<Archivos.OperacionRespuesta>() {
            @Override
            public void onNext(Archivos.OperacionRespuesta respuesta) {
                System.out.println("🗑️  " + respuesta.getMensaje());
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

    public static void obtenerMetadatos(ArchivosServiceGrpc.ArchivosServiceStub stub, int idTarea) {
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

}
