package com.AssignU.models.Usuarios.Catalogo;

public class GradoProfesionalDTO {
    public int idGradoProfesional;
    public String nombre;

    public GradoProfesionalDTO(int idGradoProfesional, String nombre) {
        this.idGradoProfesional = idGradoProfesional;
        this.nombre = nombre;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public int getIdGradoProfesional() {
        return idGradoProfesional;
    }

    public void setIdGradoProfesional(int idGradoProfesional) {
        this.idGradoProfesional = idGradoProfesional;
    }
}