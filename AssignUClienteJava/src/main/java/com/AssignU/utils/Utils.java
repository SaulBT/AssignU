
package com.AssignU.utils;

import java.nio.file.Paths;
import java.util.Optional;
import javafx.scene.control.Alert;
import javafx.scene.control.ButtonType;

public class Utils {
    
    public static void mostrarAlerta(String tituloVentana, String cuerpo, Alert.AlertType tipo) {
        Alert alerta = new Alert(tipo);
        alerta.setTitle(tituloVentana);
        alerta.setHeaderText(null);
        alerta.setContentText(cuerpo);
        alerta.showAndWait();
    }
    
    public static boolean mostrarAlertaConfirmacion(String titulo, String mensaje){
        Alert alerta = new Alert(Alert.AlertType.CONFIRMATION);
        alerta.setTitle(titulo);
        alerta.setHeaderText(null);
        alerta.setContentText(mensaje);
        Optional<ButtonType> botonSeleccionado = alerta.showAndWait();
        return(botonSeleccionado.get() == ButtonType.OK);
    }

    public static String verificarCampoNormal(String campo, int tamanioMaximo) {
        String mensaje;
        if (campo.isEmpty()){
            mensaje = "campo vacío";
        } else if (!verificarTamanioCampo(campo, tamanioMaximo)) {
            mensaje = "no debe sobrepasar de los " + tamanioMaximo + " caracteres";
        } else if (campo.isBlank()) {
            mensaje = "no se permiten espacios";
        } else {
            mensaje = "ok";
        }
        return mensaje;
    }

    public static String verificarNombreUsuario(String nombreUsuario, int tamanioMaximo) {
        String mensaje;
        if (nombreUsuario.isEmpty()){
            mensaje = "campo vacío";
        } else if (!verificarTamanioCampo(nombreUsuario, tamanioMaximo)) {
            mensaje = "no debe sobrepasar de los " + tamanioMaximo + " caracteres";
        } else if (!nombreUsuario.matches("[a-zA-Z0-9\\s]*")) {
            mensaje = "no puede tener caracteres especiales";
        } else if (contieneEspacios(nombreUsuario)) {
            mensaje = "no se permiten espacios";
        } else {
            mensaje = "ok";
        }
        return mensaje;
    }

    public static String verificarCorreo(String correo, int tamanioMaximo) {
        String mensaje;
        if (correo.isEmpty()){
            mensaje = "campo vacío";
        } else if (!verificarTamanioCampo(correo, tamanioMaximo)) {
            mensaje = "no debe sobrepasar de los " + tamanioMaximo + " caracteres";
        } else if (!correo.matches("^[\\w.-]+@[\\w.-]+\\.[a-zA-Z]{2,}$")) {
            mensaje = "no tiene formato de correo electrónico";
        } else if (contieneEspacios(correo)) {
            mensaje = "no se permiten espacios";
        } else {
            mensaje = "ok";
        }
        return mensaje;
    }

    public static boolean verificarTamanioCampo(String campo, int tamanioMaximo) {
        return campo.length() <= tamanioMaximo;
    }

    public static boolean verificarContrasenia(String contrasenia, String confirmarContrasenia) {
        return contrasenia.equals(confirmarContrasenia);
    }
    
    public static boolean contieneEspacios(String texto) {
        return texto.matches(".*\\s.*");
    }
    
    public static String obtenerNombreArchivo(String texto) {
        if ("Ningún archivo seleccionado".equals(texto)) {
            return texto;
        }
        
        if (texto == null || texto.trim().isEmpty()) {
            return "";
        }
        
        return Paths.get(texto).getFileName().toString();
    }
}
