
package com.AssignU.models.Usuarios.Alumno;

public class AlumnoDTO {
    public int idAlumno;
    public String nombreCompleto;
    public String nombreUsuario;
    public String correoElectronico;
    public int idGradoEstudios;

    public AlumnoDTO(int idAlumno, String nombreCompleto, String nombreUsuario, String correoElectronico, int idGradoEstudios) {
        this.idAlumno = idAlumno;
        this.nombreCompleto = nombreCompleto;
        this.nombreUsuario = nombreUsuario;
        this.correoElectronico = correoElectronico;
        this.idGradoEstudios = idGradoEstudios;
    }

    public int getIdAlumno() {
        return idAlumno;
    }

    public void setIdAlumno(int idAlumno) {
        this.idAlumno = idAlumno;
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

    public String getCorreoElectronico() {
        return correoElectronico;
    }

    public void setCorreoElectronico(String correoElectronico) {
        this.correoElectronico = correoElectronico;
    }

    public int getIdGradoEstudios() {
        return idGradoEstudios;
    }

    public void setIdGradoEstudios(int idGradoEstudios) {
        this.idGradoEstudios = idGradoEstudios;
    }
}
