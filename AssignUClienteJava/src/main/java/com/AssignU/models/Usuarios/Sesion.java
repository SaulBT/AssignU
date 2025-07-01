
package com.AssignU.models.Usuarios;

public class Sesion {
    private static Sesion sesion;

    private final boolean esDocente;
    private final String jwt;
    private final int idUsuario;

    private Sesion(boolean esDocente, String jwt, int idUsuario) {
        this.esDocente = esDocente;
        this.jwt = jwt;
        this.idUsuario = idUsuario;
    }

    public static void iniciarSesion(boolean esDocente, String jwt, int idUsuario) {
        sesion = new Sesion(esDocente, jwt, idUsuario);
    }

    public static Sesion getSesion() {
        return sesion;
    }

    public boolean esDocente() {
        return esDocente;
    }

    public String getJwt() {
        return jwt;
    }

    public int getIdUsuario() {
        return idUsuario;
    }

    public static void cerrarSesion() {
        sesion = null;
    }
}

