package com.AssignU.models.Usuarios.Catalogo;

public class GradoEstudioDTO {
    public int idGradoEstudios;
    public String nombre;

    public GradoEstudioDTO(int idGradoEstudios, String nombre) {
        this.idGradoEstudios = idGradoEstudios;
        this.nombre = nombre;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public int getIdGradoEstudios() {
        return idGradoEstudios;
    }

    public void setIdGradoEstudios(int idGradoEstudios) {
        this.idGradoEstudios = idGradoEstudios;
    }
}