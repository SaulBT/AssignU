package com.AssignU.models.Usuarios.Docente;

public class RegistrarDocenteDTO {
    public String nombreCompleto;
    public String nombreUsuario;
    public String contrasenia;
    public String correoElectronico;
    public int idGradoProfesional;

    public RegistrarDocenteDTO(String nombreCompleto, String nombreUsuario, String contrasenia, String correoElectronico, int idGradoProfesional) {
        this.nombreCompleto = nombreCompleto;
        this.nombreUsuario = nombreUsuario;
        this.contrasenia = contrasenia;
        this.correoElectronico = correoElectronico;
        this.idGradoProfesional = idGradoProfesional;
    }

    public String getNombreCompleto() {
        return nombreCompleto;
    }

    public void setNombreCompleto(String nombreCompleto) {
        this.nombreCompleto = nombreCompleto;
    }

    public String getNombreUsuario() {
        return nombreUsuario;
    }

    public void setNombreUsuario(String nombreUsuario) {
        this.nombreUsuario = nombreUsuario;
    }

    public String getContrasenia() {
        return contrasenia;
    }

    public void setContrasenia(String contrasenia) {
        this.contrasenia = contrasenia;
    }

    public String getCorreoElectronico() {
        return correoElectronico;
    }

    public void setCorreoElectronico(String correoElectronico) {
        this.correoElectronico = correoElectronico;
    }

    public int getIdGradoProfesional() {
        return idGradoProfesional;
    }

    public void setIdGradoProfesional(int idGradoProfesional) {
        this.idGradoProfesional = idGradoProfesional;
    }
}
