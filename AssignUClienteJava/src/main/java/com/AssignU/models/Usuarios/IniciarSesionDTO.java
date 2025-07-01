package com.AssignU.models.Usuarios;

public class IniciarSesionDTO {
    public String tipoUsuario;
    public String nombreUsuarioOCorreo;
    public String contrasenia;

    public IniciarSesionDTO(String tipoUsuario, String nombreUsuarioOCorreo, String contrasenia) {
        this.tipoUsuario = tipoUsuario;
        this.nombreUsuarioOCorreo = nombreUsuarioOCorreo;
        this.contrasenia = contrasenia;
    }

    public String getTipoUsuario() {
        return tipoUsuario;
    }

    public void setTipoUsuario(String tipoUsuario) {
        this.tipoUsuario = tipoUsuario;
    }

    public String getNombreUsuarioOCorreo() {
        return nombreUsuarioOCorreo;
    }

    public void setNombreUsuarioOCorreo(String nombreUsuarioOCorreo) {
        this.nombreUsuarioOCorreo = nombreUsuarioOCorreo;
    }

    public String getContrasenia() {
        return contrasenia;
    }

    public void setContrasenia(String contrasenia) {
        this.contrasenia = contrasenia;
    }
}
