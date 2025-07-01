package com.AssignU.models.Usuarios;

public class RespuestaIniciarSesionDTO {
    private int idUsuario;
    private String token;

    public RespuestaIniciarSesionDTO(int idUsuario, String token) {
        idUsuario = idUsuario;
        token = token;
    }

    public int getIdUsuario() {
        return idUsuario;
    }

    public void setIdUsuario(int idUsuario) {
        idUsuario = idUsuario;
    }

    public String getToken() {
        return token;
    }

    public void setToken(String token) {
        token = token;
    }
}
