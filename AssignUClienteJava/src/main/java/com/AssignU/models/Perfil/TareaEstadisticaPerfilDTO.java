
package com.AssignU.models.Perfil;

public class TareaEstadisticaPerfilDTO {
    public int idTarea;
    public String nombre;
    public float calificacion;

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

    public float getCalificacion() {
        return calificacion;
    }

    public void setCalificacion(float calificacion) {
        this.calificacion = calificacion;
    }
}
