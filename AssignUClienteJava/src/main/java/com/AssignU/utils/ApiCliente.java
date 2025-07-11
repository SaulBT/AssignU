package com.AssignU.utils;

import com.AssignU.models.Usuarios.ErrorREST;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.reflect.TypeToken;
import java.io.IOException;

import java.lang.reflect.Type;
import java.net.URI;
import java.net.http.*;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

public class ApiCliente {
    private static final String BASE_URL = "http://localhost:5010";
    private static final HttpClient cliente = HttpClient.newHttpClient();
    private static final Gson gson = new GsonBuilder()
            .registerTypeAdapter(LocalDateTime.class, new LocalDateTimeAdapter())
            .create();

    public static <T> T enviarSolicitud(String endpoint, String metodo, Object cuerpo, Map<String, String> cabeceras, Class<T> tipoRespuesta) throws Exception {

        String cuerpoJson = "";
        if (cuerpo != null){
            cuerpoJson =gson.toJson(cuerpo);
        }

        HttpResponse<String> respuesta;
        try {
            respuesta = procesarSolicitud(endpoint, metodo, cuerpoJson, cabeceras);
        } catch (IOException | InterruptedException e) {
            throw new Exception("Error de red al conectar con el servidor.");
        }

        int codigo = respuesta.statusCode();
        if (codigo >= 400) {
            ErrorREST error = gson.fromJson(respuesta.body(), ErrorREST.class);
            throw new ExcepcionHTTP(codigo, error.getError());
        }

        return gson.fromJson(respuesta.body(), tipoRespuesta);
    }

    public static <T> List<T> enviarSolicitudLista(String endpoint, String metodo, Object cuerpo, Map<String, String> cabeceras, Class<T> tipoElemento) throws Exception {

        String cuerpoJson = "";
        if (cuerpo != null){
            cuerpoJson =gson.toJson(cuerpo);
        }

        HttpResponse<String> respuesta;
        try {
            respuesta = procesarSolicitud(endpoint, metodo, cuerpoJson, cabeceras);
        } catch (IOException | InterruptedException e) {
            throw new Exception("Error de red al conectar con el servidor.");
        }

        int codigo = respuesta.statusCode();
        if (codigo >= 400) {
            ErrorREST error = gson.fromJson(respuesta.body(), ErrorREST.class);
            throw new ExcepcionHTTP(codigo, error.getError());
        }

        Type tipoLista = TypeToken.getParameterized(List.class, tipoElemento).getType();
        return gson.fromJson(respuesta.body(), tipoLista);
    }

    private static HttpResponse<String> procesarSolicitud(String endpoint, String metodo, String cuerpo, Map<String, String> cabeceras) throws Exception {

        HttpRequest.Builder builder = crearBuilder(BASE_URL + endpoint, metodo, cuerpo, cabeceras);

        HttpRequest request = builder.build();
        HttpResponse<String> respuesta = cliente.send(request, HttpResponse.BodyHandlers.ofString());

        return respuesta;
    }

    private static HttpRequest.Builder crearBuilder(String endpointCompleto, String metodo, String cuerpo, Map<String, String> cabeceras) throws Exception {
        Gson gson = new Gson();
        HttpRequest.Builder builder = HttpRequest.newBuilder().uri(URI.create(endpointCompleto));

        switch (metodo.toUpperCase()) {
            case "POST":
                builder.POST(HttpRequest.BodyPublishers.ofString(cuerpo));
                break;
            case "PUT":
                builder.PUT(HttpRequest.BodyPublishers.ofString(cuerpo));
                break;
            case "DELETE":
                builder.DELETE();
                break;
            case "GET":
                builder.GET();
                break;
            default:
                throw new Exception("Método no soportado");
        }

        if (cabeceras != null) {
            cabeceras.forEach(builder::header);
        }

        return builder;
    }
}