
package com.AssignU.utils;

public class Utils {

    public static String verificarCampoNormal(String campo, int tamanioMaximo) {
        String mensaje;
        if (campo.isEmpty()){
            mensaje = "campo vacío";
        } else if (!verificarTamanioCampo(campo, tamanioMaximo)) {
            mensaje = "no debe sobrepasar de los " + tamanioMaximo + " caracteres";
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
        } else {
            mensaje = "ok";
        }
        return mensaje;
    }

    public static boolean verificarContrasenia(String contrasenia, String confirmarContrasenia) {
        return contrasenia.equals(confirmarContrasenia);
    }

    public static boolean verificarTamanioCampo(String campo, int tamanioMaximo) {
        return campo.length() <= tamanioMaximo;
    }
    
}
