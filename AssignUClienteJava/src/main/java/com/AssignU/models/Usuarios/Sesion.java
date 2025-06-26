
package com.AssignU.models.Usuarios;

public class Sesion {
    public String tipoUsuario;
    public String jwt;
    public int idUsuario;

    public Sesion(String tipoUsuario, String jwt, int idUsuario) {
        this.tipoUsuario = tipoUsuario;
        this.jwt = jwt;
        this.idUsuario = idUsuario;
    }

    public String getTipoUsuario() {
        return tipoUsuario;
    }

    public void setTipoUsuario(String tipoUsuario) {
        this.tipoUsuario = tipoUsuario;
    }

    public String getJwt() {
        return jwt;
    }

    public void setJwt(String jwt) {
        this.jwt = jwt;
    }

    public int getIdUsuario() {
        return idUsuario;
    }

    public void setIdUsuario(int idUsuario) {
        this.idUsuario = idUsuario;
    }
}
