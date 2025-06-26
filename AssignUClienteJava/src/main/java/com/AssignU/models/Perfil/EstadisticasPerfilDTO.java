
package com.AssignU.models.Perfil;

import java.util.List;

public class EstadisticasPerfilDTO {
    public int idAlumno;
    public String nombreUsuario;
    public String nombreCompleto;
    public String correo;
    public int idGradoEstudios;
    public List<ClaseEstadisticaPerfilDTO> clases;

    public EstadisticasPerfilDTO(int idAlumno, String nombreUsuario, String nombreCompleto, String correo, int idGradoEstudios, List<ClaseEstadisticaPerfilDTO> clases) {
        this.idAlumno = idAlumno;
        this.nombreUsuario = nombreUsuario;
        this.nombreCompleto = nombreCompleto;
        this.correo = correo;
        this.idGradoEstudios = idGradoEstudios;
        this.clases = clases;
    }

    public int getIdAlumno() {
        return idAlumno;
    }

    public void setIdAlumno(int idAlumno) {
        this.idAlumno = idAlumno;
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

    public String getCorreo() {
        return correo;
    }

    public void setCorreo(String correo) {
        this.correo = correo;
    }

    public int getIdGradoEstudios() {
        return idGradoEstudios;
    }

    public void setIdGradoEstudios(int idGradoEstudios) {
        this.idGradoEstudios = idGradoEstudios;
    }

    public List<ClaseEstadisticaPerfilDTO> getClases() {
        return clases;
    }

    public void setClases(List<ClaseEstadisticaPerfilDTO> clases) {
        this.clases = clases;
    }
}
