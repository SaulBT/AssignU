
package com.AssignU.models.Usuarios.Docente;

public class DocenteDTO {
    public int idDocente;
    public String nombreCompleto;
    public String nombreUsuario;
    public String correoElectronico;
    public int idGradoProfesional;

    public DocenteDTO(int idDocente, String nombreCompleto, String nombreUsuario, String correoElectronico, int idGradoProfesional) {
        this.idDocente = idDocente;
        this.nombreCompleto = nombreCompleto;
        this.nombreUsuario = nombreUsuario;
        this.correoElectronico = correoElectronico;
        this.idGradoProfesional = idGradoProfesional;
    }

    public int getIdDocente() {
        return idDocente;
    }

    public void setIdDocente(int idDocente) {
        this.idDocente = idDocente;
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

    public String getCorreoElectronico () {
        return correoElectronico;
    }

    public void setCorreoElectronico (String correoElectronico) {
        this.correoElectronico = correoElectronico;
    }

    public int getIdGradoProfesional() {
        return idGradoProfesional;
    }

    public void setIdGradoProfesional(int idGradoProfesional) {
        this.idGradoProfesional = idGradoProfesional;
    }
}
