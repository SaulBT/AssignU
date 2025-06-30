
package com.AssignU.models.Clases.Estadisticas;

public class TareaEstadisticasClaseDTO {
    public int idTarea;
    public String nombre;

    public TareaEstadisticasClaseDTO(int idTarea, String nombre) {
        this.idTarea = idTarea;
        this.nombre = nombre;
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
}
