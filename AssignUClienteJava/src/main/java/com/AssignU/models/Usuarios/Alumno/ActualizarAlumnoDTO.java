
package com.AssignU.models.Usuarios.Alumno;

public class ActualizarAlumnoDTO {
    public String nombreCompleto;
    public String nombreUsuario;
    public int idGradoEstudios;

    public ActualizarAlumnoDTO(String nombreCompleto, String nombreUsuario, int idGradoEstudios) {
        this.nombreCompleto = nombreCompleto;
        this.nombreUsuario = nombreUsuario;
        this.idGradoEstudios = idGradoEstudios;
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

    public int getIdGradoEstudios() {
        return idGradoEstudios;
    }

    public void setIdGradoEstudios(int idGradoEstudios) {
        this.idGradoEstudios = idGradoEstudios;
    }
}
