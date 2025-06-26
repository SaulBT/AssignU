
package com.AssignU.models.Usuarios.Docente;

public class ActualizarDocenteDTO {
    public String nombreUsuario;
    public String nombreCompleto;
    public int idGradoProfesional;

    public ActualizarDocenteDTO(String nombreUsuario, String nombreCompleto, int idGradoProfesional) {
        this.nombreUsuario = nombreUsuario;
        this.nombreCompleto = nombreCompleto;
        this.idGradoProfesional = idGradoProfesional;
    }

    public String getNombreUsuario() {
        return nombreUsuario;
    }

    public void setNombreUsuario(String nombreUsuario) {
        this.nombreUsuario = nombreUsuario;
    }

    public String getNombreCompleto() {
        return nombreCompleto;
    }

    public void setNombreCompleto(String nombreCompleto) {
        this.nombreCompleto = nombreCompleto;
    }

    public int getIdGradoProfesional() {
        return idGradoProfesional;
    }

    public void setIdGradoProfesional(int idGradoProfesional) {
        this.idGradoProfesional = idGradoProfesional;
    }
}
