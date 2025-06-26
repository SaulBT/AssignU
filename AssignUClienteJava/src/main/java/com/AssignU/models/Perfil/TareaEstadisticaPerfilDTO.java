
package com.AssignU.models.Perfil;

public class TareaEstadisticaPerfilDTO {
    public int idTarea;
    public String nombre;
    public int calificacion;

    public TareaEstadisticaPerfilDTO(int idTarea, String nombre, int calificacion) {
        this.idTarea = idTarea;
        this.nombre = nombre;
        this.calificacion = calificacion;
    }

    public int getIdTarea() {
        return idTarea;
    }

    public void setIdTarea(int idTarea) {
        this.idTarea = idTarea;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public int getCalificacion() {
        return calificacion;
    }

    public void setCalificacion(int calificacion) {
        this.calificacion = calificacion;
    }
}
